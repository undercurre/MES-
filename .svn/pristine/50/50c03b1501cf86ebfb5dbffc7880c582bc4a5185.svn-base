
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using JZ.IMS.Core;
using JZ.IMS.Models;
using System.Threading.Tasks;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using System.Net;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace JZ.IMS.Job.FinallyJobStorage
{
    public class AutoLinkOrBreakSn : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    SfcsRouteConfig currentRouteConfig =  propertyprovider.route.sfcsRouteConfigs.Where(f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID).FirstOrDefault();
                    if(currentRouteConfig == null)
                    {
                        return new KeyValuePair<bool, string>(true, "");
                    }
                    String otherSql = @"select SCMR.* from  
                                SFCS_COLLECT_MULTI_RUNCARD  SCMR, 
                                SFCS_RUNCARD SR , 
                                SMT_MULTIPANEL_DETAIL   SMD1, 
                                SMT_MULTIPANEL_DETAIL  SMD2
                                where SCMR.SN_ID = SR.ID 
                                AND SR.SN = SMD1.SN
                                AND SMD2.MULT_HEADER_ID = SMD1.MULT_HEADER_ID
                                AND SMD2.SN = :SN
                                AND SCMR.WO_ID = :WO_ID";

                    Decimal multId =0;
                    SfcsCollectMultiRuncard otherMultRuncard = repository.QueryEx<SfcsCollectMultiRuncard>(otherSql,
                        new
                        {
                            SN = propertyprovider.sfcsRuncard.SN,
                            WO_ID = propertyprovider.product.sfcswo.ID
                        }).FirstOrDefault();
                    if(otherMultRuncard != null)
                    {
                        multId = otherMultRuncard.ID;
                    }
                    String linkSql = @"select SPMR.* from SFCS_PRODUCT_MULTI_RUNCARD SPMR  
                        WHERE SPMR.PART_NO = :PART_NO 
                        AND SPMR.ENABLED = 'Y'
                        AND SPMR.LINK_OPERATION_CODE = :CURRENT_OPERATION_CODE ";
                    var linkProductionList = repository.QueryEx<SfcsProductMultiRuncard>(linkSql, new
                    {
                        PART_NO = propertyprovider.product.partNumber,
                        CURRENT_OPERATION_CODE = currentRouteConfig.PRODUCT_OPERATION_CODE
                    });
                    if (linkProductionList != null && linkProductionList.Count() > 0)
                    {
                        string existSql = @"select * from SFCS_COLLECT_MULTI_RUNCARD SCMR WHERE SCMR.SN_ID = :SN_ID
                                AND SCMR.WO_ID = :WO_ID
                                AND SCMR.LINK_OPERATION_CODE = :CURRENT_OPERATION_CODE";
                        SfcsCollectMultiRuncard existSfcsCollectMultiRuncard = repository.QueryEx<SfcsCollectMultiRuncard>(existSql, new
                        {
                            SN_ID = propertyprovider.sfcsRuncard.ID,
                            WO_ID = propertyprovider.product.sfcswo.ID,
                            CURRENT_OPERATION_CODE = currentRouteConfig.PRODUCT_OPERATION_CODE
                        }).FirstOrDefault();
                        if(existSfcsCollectMultiRuncard != null)
                        {
                            return new KeyValuePair<bool, string>(true, "");
                        }

                            String multSql = @"select SMD1.*from SMT_MULTIPANEL_DETAIL SMD1 WHERE  SMD1.SN = :SN";
                            SmtMultipanelDetail smtMultpanelDetail = repository.QueryEx<SmtMultipanelDetail>(multSql, new
                            {
                                SN = propertyprovider.sfcsRuncard.SN
                            }).FirstOrDefault();
                            if(smtMultpanelDetail ==  null)
                            {
                                return new KeyValuePair<bool, string>(true, "");
                            }
                            if (multId == 0)
                            {
                                String mutlIdSql = @"SELECT SFCS_COLLECT_MULTI_RUNCARD_SEQ.NEXTVAL FROM DUAL";
                                multId = repository.QueryEx<Decimal>(mutlIdSql).FirstOrDefault();
                            }
                                String linkRuncardSql = @" INSERT INTO SFCS_COLLECT_MULTI_RUNCARD(ID,
                                                    SN_ID,
                                                    WO_ID,
                                                    ORDER_NO,
                                                    LINK_OPERATION_CODE,
                                                    COLLECT_SITE_ID,
                                                    STATUS,
                                                    COLLECT_BY,
                                                    COLLECT_TIME)VALUES( :ID,:SN_ID,:WO_ID,:ORDER_NO,:LINK_OPERATION_CODE,:COLLECT_SITE_ID,1,:OPERATOR,SYSDATE)";
                            repository.Execute(linkRuncardSql, new
                            {
                                ID = multId,
                                SN_ID = propertyprovider.sfcsRuncard.ID,
                                WO_ID = propertyprovider.product.sfcswo.ID,
                                ORDER_NO = smtMultpanelDetail.TASK_NO,
                                LINK_OPERATION_CODE = currentRouteConfig.PRODUCT_OPERATION_CODE,
                                COLLECT_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                                OPERATOR = propertyprovider.sys_Manager.USER_NAME
                            },transaction);
                    }
                        //进行拆板处理
                        String breakSql = @"select SPMR.* from SFCS_PRODUCT_MULTI_RUNCARD SPMR
                        WHERE SPMR.PART_NO = :PART_NO AND SPMR.ENABLED = 'Y'
                        AND SPMR.BREAK_OPERATION_CODE = :CURRENT_OPERATION_CODE";
                        var breakProductionList = repository.QueryEx<SfcsProductMultiRuncard>(breakSql, new
                        {
                            PART_NO = propertyprovider.product.partNumber,
                            CURRENT_OPERATION_CODE = currentRouteConfig.PRODUCT_OPERATION_CODE
                        });
                        if (breakProductionList != null && breakProductionList.Count() > 0)
                        {
                            if(multId == 0)
                            {
                                return new KeyValuePair<bool, string>(false, "产品没有进行连扳处理，分板处理异常!");
                            }
                            else
                            {
                                String updateBreakSql = @"UPDATE SFCS_COLLECT_MULTI_RUNCARD
                                       SET BREAK_OPERATION_CODE = :BREAK_OPERATION_CODE,
                                           BREAK_SITE_ID = :BREAK_SITE_ID,
                                           BREAK_BY = :BREAK_BY,
                                           BREAK_TIME = SYSDATE,
                                           STATUS = 2
                                     WHERE ID = :MULT_ID";
                                repository.Execute(updateBreakSql, new
                                {
                                    BREAK_OPERATION_CODE = currentRouteConfig.PRODUCT_OPERATION_CODE,
                                    BREAK_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                                    BREAK_BY = propertyprovider.sys_Manager.USER_NAME,
                                    MULT_ID = multId
                                }, transaction);
                            }
                        }
                        return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {
                    return new KeyValuePair<bool, string>(false, "AutoLinkOrBreakSn:" + ex.Message);
                }
            });
        }
    }
}
