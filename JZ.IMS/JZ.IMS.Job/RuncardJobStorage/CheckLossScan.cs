using Dapper;
using JZ.IMS.Core;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job.RuncardJobStorage
{
    public class CheckLossScan:IMesJob<SfcsRuncard,decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    SfcsIgnoreSiteHistory sfcsIgnoreSiteHistory =
                            repository.QueryEx<SfcsIgnoreSiteHistory>(
                                "SELECT * FROM  SFCS_IGNORE_SITE_HISTORY WHERE SN_ID = :SN_ID AND ENABLED = :ENABLED",
                                new
                                {
                                    SN_ID = propertyprovider.sfcsRuncard.ID,
                                    ENABLED = "Y"
                                }).FirstOrDefault();
                    if (sfcsIgnoreSiteHistory != null)
                    {
                        return new KeyValuePair<bool, string>(true, "");
                    }

                    decimal wipOrder = 0;
                    decimal siteOrder = -1;
                    SfcsOperations sfcsOperations = repository.QueryEx<SfcsOperations>("SELECT * FROM SFCS_OPERATIONS WHERE ID = :ID",
                        new
                        {
                            ID = propertyprovider.sfcsRuncard.WIP_OPERATION
                        }
                            ).FirstOrDefault();
                    List<SfcsRouteConfig> sfcsRouteConfigs = propertyprovider.route.sfcsRouteConfigs;
                    foreach (SfcsRouteConfig sfcsRouteConfig in sfcsRouteConfigs)
                    {
                        if (sfcsRouteConfig.CURRENT_OPERATION_ID == propertyprovider.sfcsRuncard.WIP_OPERATION)
                        {
                            wipOrder = sfcsRouteConfig.ORDER_NO;
                        }

                        if (sfcsRouteConfig.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID)
                        {
                            siteOrder = sfcsRouteConfig.ORDER_NO;
                        }
                    }

                    #region 终检跳站

                    int opQty = repository.QueryEx<int>("SELECT COUNT(0) FROM SFCS_OPERATIONS WHERE ID = :ID AND OPERATION_CATEGORY = :OPERATION_CATEGORY ", new { ID = propertyprovider.sfcsOperationSites.OPERATION_ID, OPERATION_CATEGORY = ViewModels.GlobalVariables.OQAOperation }).FirstOrDefault();//获取工序是OQA的
                    if (opQty > 0 && ((siteOrder - wipOrder) <= 10))
                    {
                        int result_qty = repository.QueryEx<int>("SELECT COUNT(0) RESULT_QTY  FROM MES_SPOTCHECK_HEADER WHERE QC_TYPE = '1' AND STATUS = '3' AND RESULT = '0' AND PARENT_BATCH_NO IN (SELECT BATCH_NO FROM MES_SPOTCHECK_DETAIL WHERE SN = :SN) ", new { SN = propertyprovider.sfcsRuncard.SN }).FirstOrDefault();
                        if (result_qty > 0)
                        {
                            return new KeyValuePair<bool, string>(true, "");
                        }
                    }
                    #endregion

                    if (siteOrder == -1)
                    {
                        return new KeyValuePair<bool, string>(false, String.Format("流水号{0}的制程中不存在当前站点工序，请确定作业站点是否正确！", propertyprovider.sfcsRuncard.SN));
                    }
                    //EndOperation:999
                    if (propertyprovider.sfcsRuncard.WIP_OPERATION == 999)
                    {
                        return new KeyValuePair<bool, string>(false, String.Format("流水号{0}已经是END!不能直接返到线上,疑是重复SN，请联系QE确认是否重复SN。", propertyprovider.sfcsRuncard.SN));
                    }
                    if (wipOrder > siteOrder)
                    {
                        //已刷，拋異常，不需要提交
                        //throw new MESException(SerialNumber + Properties.Resource.Err_HasScanned, operationRow.DESCRIPTION);
                        return new KeyValuePair<bool, string>(false, String.Format("流水号:{0}当前站点已经刷过,请确定作业站点是否正确，或SN是否为重复，当前SN应到工序:{1}", propertyprovider.sfcsRuncard.SN, sfcsOperations.DESCRIPTION));

                    }
                    if (wipOrder < siteOrder)
                    {
                        /*decimal jumpSiteCount = (siteOrder - wipOrder) / 10;
                        var p = new DynamicParameters();
                        p.Add(":P_SAMPLE_OPERATION_COUNT", jumpSiteCount, DbType.Decimal, ParameterDirection.Input);
                        p.Add(":P_SN", propertyprovider.sfcsRuncard.SN, DbType.String, ParameterDirection.Input);
                        p.Add(":P_MESSAGE", "", DbType.String, ParameterDirection.Output);
                        p.Add(":P_RESULT", "", DbType.Decimal, ParameterDirection.Output);
                        p.Add(":P_TRACE", "", DbType.String, ParameterDirection.Output);
  //                  PROCEDURE judge_spot_check_jump_site(
  //p_sn IN       VARCHAR2,
  //p_sample_operation_count IN       NUMBER,
  //p_result OUT      NUMBER,
  //p_message OUT      VARCHAR2,
  //p_trace OUT      VARCHAR2,
  //p_language IN       VARCHAR2 DEFAULT 'EN'
  // )

                        repository.Execute("SFCS_FUNCTIONS_PKG.JUDGE_SPOT_CHECK_JUMP_SITE", p, transaction, commandType: CommandType.StoredProcedure);
                        Decimal reuslt = p.Get<Decimal>(":P_RESULT");
                        String msg = p.Get<String>(":P_MESSAGE");
                        if (reuslt == 1)
                        {

                            decimal operationId = repository.QueryEx<Decimal>("SELECT SFCS_OPERATION_SEQ.NEXTVAL FROM DUAL ").FirstOrDefault();
                            String InsertLogSql = @"INSERT INTO SFCS_OPERATION_HISTORY(SN_ID, OPERATION_ID, WO_ID, ROUTE_ID, SITE_OPERATION_ID, OPERATION_SITE_ID,
                                                         OPERATOR, OPERATION_STATUS, OPERATION_TIME) VALUES(:SN_ID,:OPERATION_ID,:WO_ID,
                                                            :ROUTE_ID,:SITE_OPERATION_ID,:OPERATION_SITE_ID,:OPERATOR,:OPERATION_STATUS, SYSDATE)";
                            //LossScan:6
                            repository.Execute(InsertLogSql, new
                            {
                                SN_ID = propertyprovider.sfcsRuncard.ID,
                                OPERATION_ID = operationId,
                                WO_ID = propertyprovider.product.workOrderId,
                                ROUTE_ID = propertyprovider.route.routeID,
                                SITE_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID,
                                OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                                OPERATOR = propertyprovider.sys_Manager.USER_NAME,
                                OPERATION_STATUS = 6
                            });
                            if (propertyprovider.sfcsRuncard.WIP_OPERATION == 0)
                            {
                                return new KeyValuePair<bool, string>(false, "工单制程错误，请检查工单选择的制程!");
                            }
                            //前站漏刷，要求提交漏刷記錄
                            return new KeyValuePair<bool, string>(false, String.Format("{0}前站漏刷，请确定作业站点是否正确，或SN是否重复，当前SN应到工序{1}!", propertyprovider.sfcsRuncard.SN, sfcsOperations.DESCRIPTION));


                        }
                        else if (reuslt == -1)
                        {
                            return new KeyValuePair<bool, string>(false, msg);

                        }*/

                        //前站漏刷，要求提交漏刷記錄
                        return new KeyValuePair<bool, string>(false, String.Format("{0}前站漏刷，请确定作业站点是否正确，或SN是否重复，当前SN应到工序{1}!", propertyprovider.sfcsRuncard.SN, sfcsOperations.DESCRIPTION));
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {
                    return new KeyValuePair<bool, string>(false, "CheckLossScan:"+ex.Message);
                }
            });
        }

    }
}
