using Dapper;
using System;
using System.Data;
using System.Linq;
using System.Text;
using JZ.IMS.Core;
using JZ.IMS.Models;
using JZ.IMS.Core.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using JZ.IMS.Core.Utilities.Reflect;

namespace JZ.IMS.Job.FinallyJobStorage
{
    /// <summary>
    /// 根据产品与标签打印关系生成打印任务
    /// </summary>
    public class AutoPrint : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (propertyprovider.product != null && !String.IsNullOrEmpty(propertyprovider.product.partNumber) && propertyprovider.sfcsOperationSites != null && propertyprovider.sfcsOperationSites.OPERATION_ID != null && propertyprovider.sfcsOperationSites.OPERATION_ID > 0)
                    {
                        String part_no = propertyprovider.product.partNumber;//料号
                        Decimal operation_id = (decimal)propertyprovider.sfcsOperationSites.OPERATION_ID;//工序id

                        //包装有设置打印，需要排除包装工序
                        String sQuery = @" SELECT COUNT(0) FROM SFCS_OPERATIONS WHERE ID= :ID AND OPERATION_CATEGORY IN(SELECT LOOKUP_CODE FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'OPERATION_CATEGORY' AND DESCRIPTION LIKE '%PACKING')";
                        if (repository.QueryEx<int>(sQuery, new { ID = operation_id }).FirstOrDefault() > 0) { return new KeyValuePair<bool, string>(true, ""); }

                        propertyprovider.product.SiteId = propertyprovider.sfcsOperationSites != null ? propertyprovider.sfcsOperationSites.ID : 0;
                        propertyprovider.product.sn = propertyprovider.sfcsRuncard != null ? propertyprovider.sfcsRuncard.SN : "";

                        //根据产品料号，当前站点工序，是否是自动打印，并且是激活状态查找打印模板文件
                        sQuery = @"SELECT FD.* FROM SFCS_PRINT_FILES_DETAIL FD,SFCS_PRINT_FILES PF,SFCS_PRINT_FILES_MAPPING FM WHERE FD.PRINT_FILES_ID = PF.ID AND FM.PRINT_FILE_ID = PF.ID AND FM.PART_NO = :PART_NO AND FM.SITE_OPERATION_ID = :OPERATION_ID AND FM.AUTO_PRINT_FLAG = 'Y' AND FM.ENABLED = 'Y' ";

                        SfcsPrintFilesDetail detailModel = repository.QueryEx<SfcsPrintFilesDetail>(sQuery, new { PART_NO = part_no, OPERATION_ID = operation_id }, transaction).FirstOrDefault();
                        if (detailModel != null)
                        {

                            String file_content = System.Text.Encoding.Default.GetString(detailModel.FILE_CONTENT);

                            //PART_NO WO_NO ROUTE_ID SITE_ID SN
                            Dictionary<string, object> dictionary = new GetRangerNo().GetData(propertyprovider);//获取查询数据

                            List<dynamic> dList = repository.QueryEx<dynamic>(file_content, dictionary, transaction);

                            if (dList != null && dList.Count() > 0)
                            {
                                String itemData = "";
                                StringBuilder stringBuilder = null;
                                foreach (var item in dList)
                                {
                                    if (stringBuilder == null)
                                    {
                                        stringBuilder = new StringBuilder();
                                        itemData = string.Join(",", ((IDictionary<string, object>)item).Keys);
                                        stringBuilder.AppendLine(itemData);
                                    }
                                    itemData = string.Join(",", ((IDictionary<string, object>)item).Values);
                                    stringBuilder.AppendLine(itemData);
                                }

                                decimal printTaskId = repository.QueryEx<decimal>("SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                                DynamicParameters p = new DynamicParameters();
                                p.Add("ID", printTaskId, System.Data.DbType.Decimal);
                                p.Add("PRINT_FILE_ID", detailModel.PRINT_FILES_ID, System.Data.DbType.Decimal);
                                p.Add("OPERATOR", propertyprovider.sys_Manager.USER_NAME, System.Data.DbType.String);
                                p.Add("PRINT_DATA", stringBuilder.ToString(), System.Data.DbType.String);
                                String carton_no = propertyprovider.carton != null ? propertyprovider.carton.Carton_NO : "";
                                p.Add("CARTON_NO", carton_no, System.Data.DbType.String);
                                p.Add("PART_NO", part_no, System.Data.DbType.String);
                                p.Add("WO_NO", propertyprovider.product.workOrder, System.Data.DbType.String);
                                p.Add("PALLET_NO", propertyprovider.pallet != null ? propertyprovider.pallet.Pallet_NO : "", System.Data.DbType.String);

                                String insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,CARTON_NO,PART_NO,WO_NO,PALLET_NO)
                                                                  VALUES(:ID,:PRINT_FILE_ID,:OPERATOR,SYSDATE,0,:PRINT_DATA,:CARTON_NO,:PART_NO,:WO_NO,:PALLET_NO)";
                                repository.Execute(insertPrintTaskSql, p, transaction, commandType: CommandType.Text);

                                Printer printer = new Printer();
                                printer.isPrint = true;
                                printer.printTaskId = printTaskId;
                                propertyprovider.printer = printer;

                            }
                            else
                            {
                                return new KeyValuePair<bool, string>(false, "您配置的打印模板中数据查询不了数据,请IE确定配置的打印模板数据!");
                            }
                        }
                    }

                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "AutoPrint:" + ex.Message);
                }
            });
        }
    }

}
