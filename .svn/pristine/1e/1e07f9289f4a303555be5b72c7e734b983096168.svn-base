using Dapper;
using JZ.IMS.Core;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job.FinallyJobStorage
{
    public class AutoQcDocData : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    //记录质检单有那些SN
                    //propertyprovider变量中的SpotCheck变量不为空，并且SpotCheck里面的qcDocNo变量不为空
                    if (propertyprovider.spotCheck != null && !string.IsNullOrEmpty(propertyprovider.spotCheck.qcDocNo))
                    {
                        int result = 0;
                        string qcDocNo = propertyprovider.spotCheck.qcDocNo;
                        SfcsContainerList sfcsContainerList = repository.QueryEx<SfcsContainerList>("SELECT * FROM SFCS_CONTAINER_LIST WHERE CONTAINER_SN = :CONTAINER_SN ", new { CONTAINER_SN = propertyprovider.spotCheck.qcDocNo }, transaction).FirstOrDefault();
                        if (sfcsContainerList != null && sfcsContainerList.FULL_FLAG == "N")//N: 本单未结束 Y:本单已结束
                        {
                            propertyprovider.spotCheck.qcQty = (decimal)sfcsContainerList.SEQUENCE;
                            MesSpotcheckHeader header = repository.QueryEx<MesSpotcheckHeader>("SELECT * FROM MES_SPOTCHECK_HEADER WHERE BATCH_NO = :BATCH_NO ", new { BATCH_NO = propertyprovider.spotCheck.qcDocNo }, transaction).FirstOrDefault();
                            if (header == null)
                            {
                                #region 获取U9检验方案相关数据
                                QcDocListModel qcDoc = new QcDocListModel();
                                if (propertyprovider.product != null && !string.IsNullOrEmpty(propertyprovider.product.workOrder))
                                {
                                    string postUrl = repository.QueryEx<string>("SELECT T.DESCRIPTION FROM SFCS_PARAMETERS T WHERE T.LOOKUP_TYPE ='QC_URL' AND T.ENABLED = 'Y' AND T.LOOKUP_CODE = '0'")?.FirstOrDefault();

                                    postUrl = postUrl + "?docNo=" + propertyprovider.product.workOrder;//根据工单号获取质检方案数据
                                    //请求路径
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
                                    request.Method = "POST";
                                    request.ContentType = "text/html;charset=UTF-8";
                                    request.ContentLength = 0;

                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    Stream myResponseStream = response.GetResponseStream();
                                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                                    string retString = myStreamReader.ReadToEnd();
                                    myStreamReader.Close();
                                    myResponseStream.Close();
                                    Newtonsoft.Json.Linq.JObject jo = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(retString);
                                    if (jo["Code"].ToString() == "1")
                                    {
                                        qcDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<QcDocListModel>(jo["Data"].ToString());
                                    }
                                }
                                #endregion

                                #region 抽检报告主表添加数据
                                DynamicParameters parameters = new DynamicParameters();
                                parameters.Add("BATCH_NO", qcDocNo, System.Data.DbType.String);//质检单号
                                parameters.Add("LINE_ID", propertyprovider.sfcsOperationLines.Id, System.Data.DbType.Decimal);//线别id
                                parameters.Add("LINE_TYPE", propertyprovider.sfcsOperationLines.PLANT_CODE, System.Data.DbType.String);//线别类型：SMT,PCBA
                                parameters.Add("WO_NO", propertyprovider.product.workOrder, System.Data.DbType.String);//工单号
                                parameters.Add("ALL_QTY", propertyprovider.spotCheck.qcQty, System.Data.DbType.Decimal);//送检数 
                                parameters.Add("CHECK_QTY", propertyprovider.spotCheck.qcQty, System.Data.DbType.Decimal);//抽检数 抽检数量等于送检数量
                                parameters.Add("FAIL_QTY", 0, System.Data.DbType.Decimal);//不良数  判断当前SN有没有不良 
                                //parameters.Add("SAMP_STANDART", , System.Data.DbType.String);//抽检标准（先不管）
                                //parameters.Add("SAMP_SIZE", , System.Data.DbType.String);//抽样水平（先不管）
                                parameters.Add("STATUS", 0, System.Data.DbType.Decimal);//状态：0 新增； 2 确认；3审核；
                                parameters.Add("CHECKER", propertyprovider.sys_Manager.USER_NAME, System.Data.DbType.String);//检验员，创建人员
                                //parameters.Add("CONFIRM", qcDocNo, System.Data.DbType.String);//确认人
                                //parameters.Add("AUDITOR", qcDocNo, System.Data.DbType.String);//审核人
                                //parameters.Add("RESULT", qcDocNo, System.Data.DbType.String);//抽检判断结果：0合格；1：特采；2：返工；3：报废；(新增留null，本单结束后才需要产生结果)
                                //parameters.Add("CREATE_DATE", qcDocNo, System.Data.DbType.String);//创建时间
                                parameters.Add("ORGANIZE_ID", propertyprovider.sfcsOperationLines.ORGANIZE_ID, System.Data.DbType.String);//组织架构id
                                //parameters.Add("WO_QTY", qcDocNo, System.Data.DbType.String);//批量（工单数量）（先不管）
                                //parameters.Add("ORDER_NO", qcDocNo, System.Data.DbType.String);//批次（先不管）
                                //parameters.Add("OUTER_CHECK_QTY", qcDocNo, System.Data.DbType.String);//外观抽检数（先不管）
                                //parameters.Add("OUTER_FAIL_QTY", qcDocNo, System.Data.DbType.String);//外观不良数（先不管）
                                //parameters.Add("REMARK", qcDocNo, System.Data.DbType.String);//备注（先不管）
                                //parameters.Add("WO_CLASS", qcDocNo, System.Data.DbType.String);//班次（先不管）
                                parameters.Add("QC_TYPE", 0, System.Data.DbType.Decimal);//检验类型：0：过程检验，1：完工检验，2：终检检验
                                parameters.Add("PARENT_BATCH_NO", "", System.Data.DbType.String);//父级检验单ID
                                parameters.Add("QCSCHEMAHEAD", qcDoc.QCSchemaHead, System.Data.DbType.String);//质检方案ID（获取ERP数据）
                                parameters.Add("QCSCHEMANAME", qcDoc.QCSchemaName, System.Data.DbType.String);//质检方案名称（获取ERP数据）
                                parameters.Add("QCSCHEMAVERSION", qcDoc.QCSchemaVersion, System.Data.DbType.String);//质检方案版本（获取ERP数据）
                                parameters.Add("OPERATION_SITE_ID", propertyprovider.sfcsOperationSites.ID, System.Data.DbType.Decimal);//站点
                                string insertSpotcheckHeaderSql = @"INSERT INTO MES_SPOTCHECK_HEADER (BATCH_NO, LINE_ID, LINE_TYPE, WO_NO, ALL_QTY, CHECK_QTY, FAIL_QTY, SAMP_STANDART, SAMP_SIZE, STATUS, CHECKER, CONFIRM, AUDITOR, RESULT, CREATE_DATE, ORGANIZE_ID, WO_QTY, ORDER_NO, OUTER_CHECK_QTY, OUTER_FAIL_QTY, REMARK, WO_CLASS, QC_TYPE, PARENT_BATCH_NO, QCSCHEMAHEAD, QCSCHEMANAME, QCSCHEMAVERSION, OPERATION_SITE_ID) VALUES (:BATCH_NO, :LINE_ID, :LINE_TYPE, :WO_NO, :ALL_QTY, :CHECK_QTY, :FAIL_QTY, 0, null, :STATUS, :CHECKER, null, null, null, SYSDATE, :ORGANIZE_ID, null, null, null, null, null, null, :QC_TYPE, :PARENT_BATCH_NO, :QCSCHEMAHEAD, :QCSCHEMANAME, :QCSCHEMAVERSION, :OPERATION_SITE_ID)";
                                result += repository.Execute(insertSpotcheckHeaderSql, parameters, transaction, commandType: CommandType.Text);
                                #endregion

                                #region 抽检报告子表添加数据
                                if (qcDoc.DetailsData != null)
                                {
                                    foreach (var item in qcDoc.DetailsData)
                                    {
                                        DynamicParameters parametersB = new DynamicParameters();
                                        decimal iteamsId = repository.QueryEx<decimal>("SELECT MES_SPOTCHECK_ITEAMS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                                        parametersB.Add("ID", iteamsId, System.Data.DbType.Decimal);//ID
                                        parametersB.Add("BATCH_NO", qcDocNo, System.Data.DbType.String);//主表质检编号
                                        parametersB.Add("STEPID", item.STEPID, System.Data.DbType.Decimal);//质检方案检验步骤ID
                                        parametersB.Add("ORDER_NO", item.ORDER_NO, System.Data.DbType.String);//序号
                                        parametersB.Add("ITEM", item.ITEM, System.Data.DbType.String);//质检名称
                                        parametersB.Add("SUB_ORDER_NO", item.SUB_ORDER_NO, System.Data.DbType.String);//子序号
                                        parametersB.Add("STANDARD", item.STANDARD, System.Data.DbType.String);//指标
                                        parametersB.Add("GUIDELINEVALUE1", item.GuidelineValue1, System.Data.DbType.String);//指标值
                                        parametersB.Add("INSPECT_METHOD", item.INSPECT_METHOD, System.Data.DbType.String);//检验方式
                                        parametersB.Add("QCLEVEL", item.QCLevel, System.Data.DbType.String);//检验水平
                                        parametersB.Add("AQL", item.AQL, System.Data.DbType.String);//
                                        parametersB.Add("GUIDERANGER", "", System.Data.DbType.String);//指标范围
                                        parametersB.Add("CHECK_QTY", propertyprovider.spotCheck.qcQty, System.Data.DbType.String);//检验数量（等于qcQty）
                                        int passQty = propertyprovider.defects == null ? 1 : 0;
                                        int failQty = propertyprovider.defects == null ? 0 : 1;
                                        parametersB.Add(GlobalVariables.PassStatus, passQty, System.Data.DbType.Decimal);//合格数量（根据SN PASS 判断 FAIL 的个数判断）
                                        parametersB.Add(GlobalVariables.FailStatus, failQty, System.Data.DbType.Decimal);//不合格数 （根据SN PASS 判断 FAIL 的个数判断）
                                        parametersB.Add("RESULT", 0, System.Data.DbType.Decimal);//是否合格（默认合格 0合格 1 不合格 2 不合格）
                                        string insertIteamsSql = @"INSERT INTO MES_SPOTCHECK_ITEAMS (ID, BATCH_NO, STEPID, ORDER_NO, ITEM, SUB_ORDER_NO, STANDARD, GUIDELINEVALUE1, INSPECT_METHOD, QCLEVEL, AQL, GUIDERANGER, CHECK_QTY, PASS, FAIL, RESULT) VALUES (:ID, :BATCH_NO, :STEPID, :ORDER_NO, :ITEM, :SUB_ORDER_NO, :STANDARD, :GUIDELINEVALUE1, :INSPECT_METHOD, :QCLEVEL, :AQL, :GUIDERANGER, :CHECK_QTY, :PASS, :FAIL, :RESULT)";
                                        result += repository.Execute(insertIteamsSql, parametersB, transaction, commandType: CommandType.Text);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                //更新抽检报告主表
                                string updateHeaderSql = @"UPDATE MES_SPOTCHECK_HEADER SET ALL_QTY= :ALL_QTY,CHECK_QTY= :CHECK_QTY WHERE BATCH_NO=:BATCH_NO";
                                result += repository.Execute(updateHeaderSql, new
                                {
                                    ALL_QTY = propertyprovider.spotCheck.qcQty,
                                    CHECK_QTY = propertyprovider.spotCheck.qcQty,
                                    BATCH_NO = qcDocNo
                                }, transaction);

                                //更新抽检报告子表
                                String sQuery = @"SELECT NVL(COUNT(1) ,0) FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO AND STATUS = :STATUS ";
                                decimal passQty = repository.QueryEx<decimal>(sQuery, new { BATCH_NO = qcDocNo, STATUS = GlobalVariables.PassStatus }).FirstOrDefault();
                                decimal failQty = repository.QueryEx<decimal>(sQuery, new { BATCH_NO = qcDocNo, STATUS = GlobalVariables.FailStatus }).FirstOrDefault();
                                if (propertyprovider.defects == null) { passQty += 1; } else { failQty += 1; }

                                string updateIteamsSql = @"UPDATE MES_SPOTCHECK_ITEAMS SET CHECK_QTY= :CHECK_QTY,PASS= :PASS,FAIL= :FAIL WHERE BATCH_NO=:BATCH_NO";
                                result += repository.Execute(updateIteamsSql, new
                                {
                                    CHECK_QTY = propertyprovider.spotCheck.qcQty,
                                    PASS = passQty,
                                    FAIL = failQty,
                                    BATCH_NO = qcDocNo
                                }, transaction);
                            }

                            #region 添加抽检明细
                            decimal detailId = 0;
                            string status = propertyprovider.defects == null ? GlobalVariables.PassStatus : GlobalVariables.FailStatus;
                            string id = repository.QueryEx<string>("SELECT T.ID FROM MES_SPOTCHECK_DETAIL T WHERE T.BATCH_NO = :BATCH_NO AND T.SN =:SN ", new { BATCH_NO = qcDocNo, SN = propertyprovider.data }).FirstOrDefault();
                            if (id.IsNullOrEmpty())
                            {
                                DynamicParameters parametersC = new DynamicParameters();
                                detailId = repository.QueryEx<decimal>("SELECT MES_SPOTCHECK_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                                parametersC.Add("ID", detailId, System.Data.DbType.Decimal);
                                parametersC.Add("BATCH_NO", qcDocNo, System.Data.DbType.String);
                                parametersC.Add("SN", propertyprovider.data, System.Data.DbType.String);
                                parametersC.Add("STATUS", status, System.Data.DbType.String);//抽检状态:PASS,FAIL
                                parametersC.Add("CREATOR", propertyprovider.sys_Manager.USER_NAME, System.Data.DbType.String);//创建人
                                string insertDetailSql = @"INSERT INTO MES_SPOTCHECK_DETAIL (ID, BATCH_NO, SN, STATUS, CREATE_TIME, CREATOR) VALUES (:ID, :BATCH_NO, :SN, :STATUS, SYSDATE, :CREATOR)";
                                result += repository.Execute(insertDetailSql, parametersC, transaction, commandType: CommandType.Text);

                            }
                            else
                            {
                                detailId = Convert.ToDecimal(id);
                                string U_UpadateDetailSeq = @"UPDATE MES_SPOTCHECK_DETAIL SET STATUS= :STATUS,CREATOR= :CREATOR,CREATE_TIME= SYSDATE WHERE BATCH_NO=:BATCH_NO AND SN =:SN";
                                result += repository.Execute(U_UpadateDetailSeq, new
                                {
                                    STATUS = status,
                                    CREATOR = propertyprovider.sys_Manager.USER_NAME,
                                    BATCH_NO = qcDocNo,
                                    SN = propertyprovider.data
                                }, transaction);
                            }
                            #endregion

                            #region 添加不良详细描述
                            if (status == "FAIL")
                            {
                                int i = 0;
                                DynamicParameters parametersD = new DynamicParameters();
                                decimal failId = repository.QueryEx<decimal>("SELECT MES_SPOTCHECK_FAIL_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                                parametersD.Add("ID", failId, System.Data.DbType.Decimal);
                                parametersD.Add("SPOTCHECK_DETAIL_ID", detailId, System.Data.DbType.Decimal);//抽检明细关联ID
                                string defect_code = "";//不良代码
                                foreach (var defect in propertyprovider.defects)
                                {
                                    if (defect.collectDataList != null)
                                    {
                                        defect_code = defect.collectDataList[0].Data;
                                    }
                                }
                                parametersD.Add("DEFECT_CODE", defect_code, System.Data.DbType.String);//不良代码
                                parametersD.Add("DEFECT_LOC", "", System.Data.DbType.String);//不良位号
                                string description = "";
                                foreach (var defect in propertyprovider.defects)
                                {
                                    i = 0;
                                    if (defect.defectDetailList != null)
                                    {
                                        foreach (string msg in defect.defectDetailList)
                                        {
                                            if (i == 0)
                                            {
                                                description = msg;
                                            }
                                            else
                                            {
                                                description += "," + msg;
                                            }
                                            i++;
                                        } 
                                    }
                                }
                                parametersD.Add("DEFECT_DESCRIPTION", description, System.Data.DbType.String);//不良描述
                                SfcsDefectConfig DefectList = repository.QueryEx<SfcsDefectConfig>("SELECT * FROM SFCS_DEFECT_CONFIG WHERE DEFECT_CODE = :DEFECT_CODE AND ENABLED = 'Y' ORDER BY ID DESC  ", new { DEFECT_CODE = defect_code }, transaction).FirstOrDefault();//根据不良代码获取不良现象
                                String defect_description = DefectList == null ? "" : DefectList.DEFECT_DESCRIPTION;
                                parametersD.Add("DEFECT_MSG", defect_description, System.Data.DbType.String);//不良现象
                                string insertFailSql = @"INSERT INTO MES_SPOTCHECK_FAIL_DETAIL (ID, SPOTCHECK_DETAIL_ID, DEFECT_CODE, DEFECT_LOC, DEFECT_DESCRIPTION, DEFECT_MSG) VALUES (:ID, :SPOTCHECK_DETAIL_ID, :DEFECT_CODE, :DEFECT_LOC, :DEFECT_DESCRIPTION, :DEFECT_MSG)";
                                result += repository.Execute(insertFailSql, parametersD, transaction, commandType: CommandType.Text);
                            }
                            #endregion
                        }
                    }

                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {
                    return new KeyValuePair<bool, string>(false, "AutoQcDocData:" + ex.Message);
                }
            });
        }
    }
}
