using Dapper;
using JZ.IMS.Core;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job.FinallyJobStorage
{
    public class AutoLinkPallect : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    //1、判断有没有配置需要自动产生包装
                    if (propertyprovider.pallet != null)
                    {
                        String palletNo = propertyprovider.pallet.Pallet_NO;
                        //查询caton是否满箱
                        SfcsContainerList sfcsContainerList = repository.QueryEx<SfcsContainerList>(
                            "SELECT * FROM SFCS_CONTAINER_LIST WHERE CONTAINER_SN = :CONTAINER_SN ",
                            new { CONTAINER_SN = palletNo }, transaction).FirstOrDefault();
                        if (sfcsContainerList != null && sfcsContainerList.FULL_FLAG == "Y") {
                            //查询打印模板
                            String printMappSql = @"select SPF.* from SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES  SPF 
                    where SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 6";
                            String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
                            SfcsPrintFiles sfcsPrintFiles = null;
                            List<SfcsPrintFiles> sfcsPrintMapplist = null;
                            if (sfcsContainerList.PART_NO != null && sfcsContainerList.PART_NO != "")
                            {
                                sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByPn,
                                new
                                {
                                    PART_NO = sfcsContainerList.PART_NO
                                });
                            }

                            //if ((sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                            //&& sfcsContainerList.)
                            //{
                            //    String printMappSqlByModel = printMappSql + " AND SPFM.MODEL_ID = :MODEL_ID";
                            //    sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByModel,
                            //    new
                            //    {
                            //        MODEL_ID = sfcsPn.MODEL_ID
                            //    });
                            //}
                            //if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                            //{
                            //    String printMappSqlByFamilly = printMappSql + " AND SPFM.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID";
                            //    sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByFamilly,
                            //    new
                            //    {
                            //        PRODUCT_FAMILY_ID = sfcsPn.FAMILY_ID
                            //    });
                            //}

                            //if ((sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                            //&& sfcsContainerList.CUSTOMER_PO)
                            //{
                            //    String printMappSqlByCustor = printMappSql + " AND SPFM.CUSTOMER_ID = :CUSTOMER_ID";

                            //    sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByCustor,
                            //    new
                            //    {
                            //        CUSTOMER_ID = sfcsPn.CUSTOMER_ID
                            //    });
                            //}
                            if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                            {
                                sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByPn,
                                new
                                {
                                    PART_NO = "000000"
                                });
                            }
                            if (sfcsPrintMapplist != null && sfcsPrintMapplist.Count > 0)
                            {
                                sfcsPrintFiles = sfcsPrintMapplist.FirstOrDefault();
                            }
                            else
                            {
                                throw new Exception("没有设置产品栈板打印模板!");
                            }
                            String palletResponMdelSql = @"select SCL.CONTAINER_SN PALLET_NO,SCL.SEQUENCE PALLECT_QTY,  CARTON_NO, count(SR.ID) QTY, SW.WO_NO ,SW.PART_NO,SP.DESCRIPTION 
                                from SFCS_RUNCARD SR, SFCS_WO SW, SFCS_PN SP ,SFCS_CONTAINER_LIST SCL
                                WHERE SR.WO_ID = SW.ID 
                                AND SCL.CONTAINER_SN = :PALLET_NO 
                                AND SW.PART_NO = SP.PART_NO 
                                AND SR.PALLET_NO = SCL.CONTAINER_SN
                                group by SR.CARTON_NO,SW.WO_NO ,SW.PART_NO,SP.DESCRIPTION,SCL.CONTAINER_SN ,SCL.SEQUENCE";
                            var palletResponModels = repository.QueryEx<PalletResponModel>(palletResponMdelSql,
                            new { PALLET_NO = palletNo });
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.AppendLine("NO,PALLECT_NO,PALLECT_QTY,CARTON_NO,QTY,PART_NO,MODEL");
                            for(int i = 0; i< palletResponModels.Count; i++)
                            {
                                stringBuilder.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", i+1,
                                palletResponModels[i].PALLECT_NO, palletResponModels[i].PALLECT_QTY, palletResponModels[i].CARTON_NO,
                                palletResponModels[i].QTY, palletResponModels[i].PART_NO, palletResponModels[i].MODEL));
                            }
                            decimal printTaskId = repository.QueryEx<decimal>("SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                            DynamicParameters p = new DynamicParameters();
                            p.Add("ID", printTaskId, System.Data.DbType.Decimal);
                            p.Add("PRINT_FILE_ID", sfcsPrintFiles.ID, System.Data.DbType.Decimal);
                            p.Add("OPERATOR", propertyprovider.sys_Manager.USER_NAME, System.Data.DbType.String);
                            p.Add("PRINT_DATA", stringBuilder.ToString(), System.Data.DbType.String);
                            p.Add("PALLET_NO", palletNo, System.Data.DbType.String);
                            p.Add("PART_NO", propertyprovider.product.partNumber, System.Data.DbType.String);
                            p.Add("WO_NO", propertyprovider.product.workOrder, System.Data.DbType.String);
                            String insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,PALLET_NO,PART_NO,WO_NO)VALUES(
		:ID,:PRINT_FILE_ID,:OPERATOR,sysdate,0,:PRINT_DATA,:PALLET_NO,:PART_NO,:WO_NO)";
                            repository.Execute(insertPrintTaskSql, p, transaction, commandType: CommandType.Text);
                            Printer printer = new Printer();
                            printer.isPrint = true;
                            printer.printTaskId = printTaskId;
                            propertyprovider.printer = printer;

                        } else
                        {
                            propertyprovider.printer = null;
                        }
                        propertyprovider.pallet.Pallet_NO = palletNo;
                    }
                    else
                    {
                        propertyprovider.printer = null;
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "AutoLinkPallect:" + ex.Message);
                }
            });
        }
    }
}
