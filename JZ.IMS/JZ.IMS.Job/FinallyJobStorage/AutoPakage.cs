using Dapper;
using JZ.IMS.Core;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job.FinallyJobStorage
{
    public class AutoPakage : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    //1、判断有没有配置需要自动产生包装
                    if (propertyprovider.carton != null)
                    {
                        String Carton_NO = propertyprovider.carton.Carton_NO;
                        //查询caton是否满箱
                        SfcsContainerList sfcsContainerList = repository.QueryEx<SfcsContainerList>(
                            "SELECT * FROM SFCS_CONTAINER_LIST WHERE CONTAINER_SN = :CONTAINER_SN ",
                            new { CONTAINER_SN = propertyprovider.carton.Carton_NO }, transaction).FirstOrDefault();
                        if (sfcsContainerList == null)
                        {
                            if (propertyprovider.carton.collectDataList != null && propertyprovider.carton.collectDataList.Count > 0)
                            {
                                Carton_NO = propertyprovider.carton.collectDataList[0].Data;
                                var qty = repository.QueryEx<decimal>("SELECT QUANTITY FROM SFCS_COLLECT_CARTONS WHERE PART_NO =:PART_NO AND CARTON_NO=:CARTON_NO",
                                    new
                                    {
                                        PART_NO = propertyprovider.product.partNumber,
                                        CARTON_NO = Carton_NO
                                    }, transaction).FirstOrDefault();

                                propertyprovider.carton.Carton_NO = Carton_NO;
                                propertyprovider.carton.CurrentQty = qty;
                            }
                            return new KeyValuePair<bool, string>(true, "");
                        }
                        if (sfcsContainerList.FULL_FLAG == "Y")
                        {
                            //查询打印模板
                            String printMappSql = @"select SPF.* from SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES  SPF 
                    where SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 3";
                            var sfcsPnlist = repository.QueryEx<SfcsPn>("select SP.* from SFCS_PN SP where SP.PART_NO =:PART_NO",
                                new
                                {
                                    PART_NO = sfcsContainerList.PART_NO
                                });
                            SfcsPn sfcsPn = sfcsPnlist.FirstOrDefault();
                            String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
                            SfcsPrintFiles sfcsPrintFiles = null;
                            List<SfcsPrintFiles> sfcsPrintMapplist = null;
                            sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByPn,
                                new
                                {
                                    PART_NO = sfcsPn.PART_NO
                                });

                            if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                            {
                                String printMappSqlByModel = printMappSql + " AND SPFM.MODEL_ID = :MODEL_ID";
                                sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByModel,
                                new
                                {
                                    MODEL_ID = sfcsPn.MODEL_ID
                                });
                            }
                            if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                            {
                                String printMappSqlByFamilly = printMappSql + " AND SPFM.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID";
                                sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByFamilly,
                                new
                                {
                                    PRODUCT_FAMILY_ID = sfcsPn.FAMILY_ID
                                });
                            }
                            if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                            {
                                String printMappSqlByCustor = printMappSql + " AND SPFM.CUSTOMER_ID = :CUSTOMER_ID";

                                sfcsPrintMapplist = repository.QueryEx<SfcsPrintFiles>(printMappSqlByCustor,
                                new
                                {
                                    CUSTOMER_ID = sfcsPn.CUSTOMER_ID
                                });
                            }
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
                                throw new Exception("没有设置产品包装打印模板!");
                            }

                            String sql = @"SELECT SR.CARTON_NO,SP.PART_NO PN, SP.DESCRIPTION MODEL, SCL.SEQUENCE QTY,SR.CARTON_NO QR_NO,SW.WO_NO, SR.SN
                                FROM SFCS_RUNCARD SR, SFCS_WO SW, SFCS_PN SP ,SFCS_CONTAINER_LIST SCL
                                WHERE SR.WO_ID = SW.ID AND SW.PART_NO = SP.PART_NO
                                AND SCL.CONTAINER_SN = :CARTON_NO
                                AND SR.CARTON_NO = SCL.CONTAINER_SN";
                            var cartonNoResponModelList = repository.QueryEx<CartonResoponeModel>(sql, new
                            {
                                CARTON_NO = Carton_NO
                            });
                            if (cartonNoResponModelList == null | cartonNoResponModelList.Count() <= 0)
                            {
                                throw new Exception(String.Format("箱号:{0}没有查到产品信息!", Carton_NO));
                            }

                            //检查打印模板文件是否配置打印数据源，如果有按配置写入打印数据
                            StringBuilder stringBuilder = new StringBuilder();
                            SfcsPrintFilesDetail printFilesDetail = repository.QueryEx<SfcsPrintFilesDetail>("SELECT * FROM SFCS_PRINT_FILES_DETAIL WHERE PRINT_FILES_ID =:PRINT_FILES_ID ", new { PRINT_FILES_ID = sfcsPrintFiles.ID }).FirstOrDefault();
                            if (printFilesDetail != null)
                            {
                                String file_content = System.Text.Encoding.Default.GetString(printFilesDetail.FILE_CONTENT);
                                Dictionary<string, object> dictionary = new GetRangerNo().GetData(propertyprovider);//获取查询数据
                                List<dynamic> dList = repository.QueryEx<dynamic>(file_content, dictionary, transaction);

                                if (dList != null && dList.Count() > 0)
                                {
                                    String itemData = "";
                                    stringBuilder = null;
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
                                }
                                else
                                {
                                    return new KeyValuePair<bool, string>(false, "您配置的打印模板中数据查询不了数据,请IE确定配置的打印模板数据!");
                                }
                            }
                            else
                            {

                                //获取外箱打印附加值
                                String detail = "", detailValue = "", header = "";
                                detail = repository.QueryEx<String>("SELECT CONFIG_VALUE FROM SFCS_PRODUCT_CONFIG WHERE PART_NO =:PART_NO AND CONFIG_TYPE =:CONFIG_TYPE AND ENABLED='Y'",
                                            new { PART_NO = sfcsPn.PART_NO, CONFIG_TYPE = GlobalVariables.CartonPrintData }).FirstOrDefault();
                                if (detail != null && detail != "")
                                {
                                    var detailArr = detail.Split("|");
                                    for (int i = 0; i < detailArr.Length; i++)
                                    {
                                        header += "," + GlobalVariables.PrintHeader + (i + 1);
                                        detailValue += "," + detailArr[i];
                                    }
                                }

                                StringBuilder snheader = new StringBuilder();
                                StringBuilder snDetail = new StringBuilder();
                                for (int i = 0; i < cartonNoResponModelList.Count; i++)
                                {
                                    string index = (i + 1).ToString();
                                    snheader.Append(String.Format(",SN{0}", index));
                                    snDetail.Append(String.Format(",{0}", cartonNoResponModelList[i].SN));
                                }
                                stringBuilder.AppendLine(String.Format("BOX_NO,PN,MODEL,LINE_NAME,PRODUCT_TIME,QTY,QR_NO,WO_NO{0}{1}", snheader.ToString(), header));
                                stringBuilder.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7}{8}{9}",
                                    Carton_NO, cartonNoResponModelList[0].PN, cartonNoResponModelList[0].MODEL,
                                    propertyprovider.sfcsOperationLines.OPERATION_LINE_NAME,
                                    DateTime.Now,
                                    cartonNoResponModelList.Count,
                                    Carton_NO,
                                    cartonNoResponModelList[0].WO_NO,
                                    snDetail.ToString(),
                                    detailValue));
                            }

                            #region 添加IMS_REEL表

                            //string BarcodeFormatter = "M{0:yyMMdd}{1:000000}";
                            //decimal reel_id = (await QueryAsyncEx<decimal>("SELECT SEQ_REEL.NEXTVAL FROM DUAL")).FirstOrDefault();
                            string reel_code = Carton_NO;//string.Format(BarcodeFormatter, DateTime.Now, reel_id);//物料条码
                                                         //rpModel.REEL_CODE.Add(reel_code);

                            //SELECT IMS.IMS_REEL_SEQ.NEXTVAL FROM DUAL
                            //SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL
                            decimal reelId = (repository.QueryEx<decimal>("SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL")).FirstOrDefault();

                            //IMS_REEL@WMS IMS.IMS_REEL
                            string selectVendorIdSql = @"SELECT ID FROM IMS_VENDOR@WMS WHERE CODE = 'Pilot'";
                            string selectPartIdSql = @"SELECT * FROM IMS_PART@WMS WHERE CODE=:CODE";
                            string selectCartonNoSql = @"SELECT COUNT(*) FROM SFCS_RUNCARD  WHERE CARTON_NO=:CARTON_NO ";
                            string selectImsReelSql = @"SELECT COUNT(*) FROM IMS_REEL@WMS WHERE CODE=:CODE ";
                            string insertImsReelSql = @"INSERT INTO IMS_REEL@WMS(ID,CODE,VENDOR_ID,PART_ID,MAKER_PART_ID,DATE_CODE,LOT_CODE,MSD_LEVEL,ESD_FLAG,CASE_QTY,IQC_FLAG,ORIGINAL_QUANTITY) VALUES(:ID,:CODE,:VENDOR_ID,:PART_ID,:MAKER_PART_ID,:DATE_CODE,:LOT_CODE,:MSD_LEVEL,:ESD_FLAG,:CASE_QTY,:IQC_FLAG,:ORIGINAL_QUANTITY)";

                            var vendorID = repository.QueryEx<decimal>(selectVendorIdSql);
                            var partID = (repository.QueryEx<ImsPart>(selectPartIdSql, new { CODE = propertyprovider.product.partNumber })).FirstOrDefault()?.ID ?? 0;
                            var countOfSn = repository.QueryEx<decimal>(selectCartonNoSql, new { CARTON_NO = Carton_NO });
                            var count = repository.ExecuteScalar(selectImsReelSql, new { CODE = reel_code });

                            if (count <= 0)
                            {
                                var effectNum = repository.Execute(insertImsReelSql, new
                                {
                                    ID = reelId,
                                    CODE = reel_code,
                                    VENDOR_ID = vendorID,
                                    PART_ID = partID,
                                    MAKER_PART_ID = -1,
                                    DATE_CODE = DateTime.Now.Date,
                                    LOT_CODE = "",
                                    MSD_LEVEL = "1",
                                    ESD_FLAG = "",
                                    CASE_QTY = 1,
                                    IQC_FLAG = "Y",
                                    ORIGINAL_QUANTITY = countOfSn
                                }, transaction);

                                if (effectNum <= 0)
                                    throw new Exception("更新条码数据异常，请稍后再尝试。");
                            }

                            #endregion
                            //未打印
                            string selectPrintTask = @" SELECT * FROM SFCS_PRINT_TASKS WHERE PRINT_FILE_ID=:PRINT_FILE_ID AND CARTON_NO=:CARTON_NO AND  PART_NO=:PART_NO AND WO_NO=:WO_NO AND PALLET_NO=:PALLET_NO AND PRINT_STATUS=0 ORDER BY ID ";
                            var PALLET_NO = propertyprovider.pallet != null ? propertyprovider.pallet.Pallet_NO : "";
                            var printTaskModel = (repository.QueryEx<SfcsPrintTasks>(selectPrintTask, new
                            {
                                PRINT_FILE_ID = sfcsPrintFiles.ID,
                                CARTON_NO = Carton_NO,
                                PART_NO = propertyprovider.product.partNumber,
                                WO_NO = propertyprovider.product.workOrder,
                                PALLET_NO = PALLET_NO
                            })).FirstOrDefault();

                            decimal printTaskId = 0;
                            Printer printer = new Printer();
                            if (printTaskModel != null)
                            {
                                printTaskId = printTaskModel.ID;
                            }
                            else
                            {
                                printTaskId = repository.QueryEx<decimal>("SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                                DynamicParameters p = new DynamicParameters();
                                p.Add("ID", printTaskId, System.Data.DbType.Decimal);
                                p.Add("PRINT_FILE_ID", sfcsPrintFiles.ID, System.Data.DbType.Decimal);
                                p.Add("OPERATOR", propertyprovider.sys_Manager.USER_NAME, System.Data.DbType.String);
                                p.Add("PRINT_DATA", stringBuilder.ToString(), System.Data.DbType.String);
                                p.Add("CARTON_NO", Carton_NO, System.Data.DbType.String);
                                p.Add("PART_NO", propertyprovider.product.partNumber, System.Data.DbType.String);
                                p.Add("WO_NO", propertyprovider.product.workOrder, System.Data.DbType.String);
                                p.Add("PALLET_NO", PALLET_NO, System.Data.DbType.String);

                                String insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,CARTON_NO,PART_NO,WO_NO,PALLET_NO)VALUES(
		                                                        :ID,:PRINT_FILE_ID,:OPERATOR,SYSDATE,0,:PRINT_DATA,:CARTON_NO,:PART_NO,:WO_NO,:PALLET_NO)";
                                repository.Execute(insertPrintTaskSql, p, transaction, commandType: CommandType.Text);

                                string currentOperationID = @"select * from sfcs_runcard where sn=:sn";
                                var snModel = (repository.QueryEx<SfcsRuncard>(currentOperationID, new { sn = propertyprovider.data })).FirstOrDefault();

                                if (snModel == null)
                                    throw new Exception("没有找到SN:{0}相关信息,请注意检查输入的信息!");

                                //如果是包装工序就入库
                                if (GlobalVariables.Packing.Equals(propertyprovider.sfcsOperationSites.OPERATION_ID))
                                {
                                    #region 完工入库

                                    string insertInboundRecordSql = @"INSERT INTO SFCS_INBOUND_RECORD_INFO (ID,WO_ID,INBOUND_NO,INBOUND_QTY,STATUS,CREATE_TIME,CREATE_BY,AUTO_EDITOR) 
					                                           VALUES (:ID,:WO_ID,:INBOUND_NO,:INBOUND_QTY,'0',SYSDATE,:CREATE_BY,:AUTO_EDITOR)";
                                    var newid = repository.ExecuteScalar("SELECT MES_SEQ_ID.NEXTVAL FROM DUAL");
                                    SfcsWo swModel = repository.QueryEx<SfcsWo>("SELECT * FROM SFCS_WO WHERE WO_NO = :WO_NO", new { WO_NO = propertyprovider.product.workOrder })?.FirstOrDefault();
                                    if (swModel == null) { throw new Exception("WO_NO_NOT_EXIST"); }

                                    //是否自动审核
                                    var isAutoEditor = GlobalVariables.FailedCode;
                                    if (swModel.TURNIN_TYPE.Equals("Y"))
                                    {
                                        isAutoEditor = GlobalVariables.successCode;
                                    }
                                    var WO_ID = Convert.ToDecimal(swModel.ID);

                                    //將序列轉成36進制表示
                                    string r = Core.Utilities.RadixConvertPublic.RadixConvert(newid.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);
                                    //六位表示
                                    string ReleasedSequence = r.PadLeft(6, '0');
                                    string yymmdd = repository.QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                                    var INBOUND_NO = "BT" + yymmdd + ReleasedSequence;
                                    var resdata = repository.Execute(insertInboundRecordSql, new
                                    {
                                        ID = newid,
                                        WO_ID = WO_ID,
                                        INBOUND_NO = INBOUND_NO,
                                        INBOUND_QTY = propertyprovider.carton.CurrentQty,
                                        CREATE_BY = propertyprovider.sys_Manager.USER_NAME,
                                        AUTO_EDITOR = isAutoEditor
                                    }, transaction);

                                    #endregion
                                }

                            }

                            printer.isPrint = true;
                            printer.printTaskId = printTaskId;
                            propertyprovider.printer = printer;

                        }
                        else
                        {
                            propertyprovider.printer = null;

                        }
                        propertyprovider.carton.Carton_NO = Carton_NO;
                        propertyprovider.carton.CurrentQty = (decimal)sfcsContainerList.SEQUENCE;
                    }
                    else
                    {
                        propertyprovider.printer = null;
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "AutoPakage:" + ex.Message);
                }
            });
        }


    }
}
