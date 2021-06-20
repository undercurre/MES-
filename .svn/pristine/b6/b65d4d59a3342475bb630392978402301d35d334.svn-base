using System;
using Dapper;
using System.Data;
using JZ.IMS.Core;
using System.Linq;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System.Threading.Tasks;
using JZ.IMS.Core.Repository;
using System.Collections.Generic;
using JZ.IMS.Core.Utilities.Reflect;

namespace JZ.IMS.Job.FinallyJobStorage
{
    /// <summary>
    /// 根据产品检验方案产生终检检验单
    /// </summary>
    public class AutoOQADocData : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (propertyprovider.product != null && !String.IsNullOrEmpty(propertyprovider.product.partNumber) && propertyprovider.sfcsOperationSites != null && propertyprovider.sfcsOperationSites.OPERATION_ID != null && propertyprovider.sfcsOperationSites.OPERATION_ID > 0)
                    {
                        String part_no = propertyprovider.product.partNumber;//产品料号
                        Decimal operation_id = (decimal)propertyprovider.sfcsOperationSites.OPERATION_ID;//当前过站的工序ID

                        //1.根据产品料号和工序id获取产品检验方案产品料号和标记工序相同的检验方案
                        SfcsProductSample productSample = repository.QueryEx<SfcsProductSample>(productSampleSql, new { PART_NO = part_no, OPERATION_ID = operation_id }, transaction)?.FirstOrDefault();
                        if (productSample != null && productSample.PROJECT_ID > 1)
                        {
                            List<SfcsSampleProjectConfig> SPCList = repository.QueryEx<SfcsSampleProjectConfig>(sampleProjectConfigSql, new { PROJECT_ID = productSample.PROJECT_ID }, transaction);//抽检比例设置数据
                            if (SPCList != null && SPCList.Count() > 0)
                            {
                                MesSpotcheckHeader header = repository.QueryEx<MesSpotcheckHeader>(spotCheckHeaderSql, new
                                {
                                    WO_NO = propertyprovider.product.workOrder,
                                    SITE_ID = propertyprovider.sfcsOperationSites.ID
                                }, transaction)?.FirstOrDefault();//获取最新的一张终检单号
                                if (header == null)
                                {
                                    //没有进行过抽检
                                    //根据当前站点获取已经过站的SN数量
                                    int snQty = repository.QueryEx<int>(snQtySql, new { WO_ID = propertyprovider.sfcsRuncard.WO_ID, SITE_ID = propertyprovider.sfcsOperationSites.ID }, transaction).FirstOrDefault();

                                    SaveSpotCheck(propertyprovider, SPCList[0], snQty, 0, repository, transaction);
                                }
                                else
                                {
                                    //已经进行过抽检
                                    if (header.CHECK_QTY < header.ALL_QTY)
                                    {
                                        //1.上一终检单未结束 更新上一终检单
                                        int fail_qty = propertyprovider.defects == null ? 0 : 1; header.FAIL_QTY += fail_qty;//不良数
                                        ModifySpotCheckHeader(header, repository, transaction);

                                        AddSpotCheckDetail(propertyprovider, header.BATCH_NO, repository, transaction);
                                    }
                                    else
                                    {
                                        //2.上一终检单已结束 计算连续Pass和连续Fail 生成下一终检单
                                        int passQty = repository.QueryEx<int>(passQtySql, new { WO_NO = propertyprovider.product.workOrder, SITE_ID = propertyprovider.sfcsOperationSites.ID }, transaction).FirstOrDefault();//已过的Sn数量
                                        int passSNQty = repository.QueryEx<int>(snQtySql, new { WO_ID = propertyprovider.sfcsRuncard.WO_ID, SITE_ID = propertyprovider.sfcsOperationSites.ID }, transaction).FirstOrDefault();//全部过站的sn数量
                                        int snQty = passSNQty - passQty;//待检验的sn数量
                                        SfcsSampleProjectConfig previousPlan = SPCList.Where(m => m.ID == header.QCSCHEMAHEAD)?.FirstOrDefault();//上一个检验方案

                                        decimal? order_no = previousPlan.ORDER_NO, keepPassQty = 0, keepFailQty = 0;
                                        List<MesSpotcheckDetailListModel> detailList = repository.QueryEx<MesSpotcheckDetailListModel>(spotCheckDetailSql, new { BATCH_NO = header.BATCH_NO }, transaction);
                                        foreach (var item in detailList)
                                        {
                                            if (item.STATUS.ToUpper() == "PASS") { keepPassQty++; keepFailQty = 0; }
                                            else if (item.STATUS.ToUpper() == "FAIL") { keepFailQty++; keepPassQty = 0; }

                                            //当前抽检比例 SAMPLE_RATIO SAMPLE_RATIO_COUNT 连续Pass数量 UP_RATIO UP_RATIO_LIMIT_COUNT 连续fail数量 DOWN_RATIO DOWN_RATIO_LIMIT_COUNT
                                            if (previousPlan.UP_RATIO_LIMIT_COUNT == keepPassQty)
                                            {
                                                decimal? max_no = SPCList.Max(m => m.ORDER_NO);
                                                order_no = (order_no + 10) > max_no ? max_no : (order_no + 10);
                                                break;
                                            }
                                            else if (previousPlan.DOWN_RATIO_LIMIT_COUNT == keepFailQty)
                                            {
                                                decimal? min_no = SPCList.Min(m => m.ORDER_NO);
                                                order_no = (order_no - 10) < min_no ? min_no : (order_no - 10);
                                                break;
                                            }

                                        }

                                        SfcsSampleProjectConfig nextPlan = SPCList.Where(m => m.ORDER_NO == order_no)?.FirstOrDefault();//下一个检验方案
                                        if (nextPlan == null) { nextPlan = previousPlan; }//没有新的就用旧的解决方案

                                        SaveSpotCheck(propertyprovider, nextPlan, snQty, passQty, repository, transaction);

                                    }
                                }
                            }
                        }
                    }

                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {
                    return new KeyValuePair<bool, string>(false, "AutoOQADocData:" + ex.Message);
                }
            });
        }

        #region SQL

        // 产品料号: PART_NO; 当前工序: CURRENT_OPERATION_ID;
        // 标记工序: DELIVER_OPERATION_CODE; 抽检工序: SAMPLE_OPERATION_CODE; 关联关系 唯一作业标识: SFCS_ROUTE_CONFIG.PRODUCT_OPERATION_CODE;
        public const string productSampleSql = @"SELECT SPS.* FROM SFCS_PRODUCT_SAMPLE SPS, SFCS_ROUTE_CONFIG SRC, SFCS_ROUTES SR WHERE SPS.DELIVER_OPERATION_CODE = SRC.PRODUCT_OPERATION_CODE AND SRC.ROUTE_ID = SR.ID AND SPS.PART_NO = :PART_NO AND SRC.CURRENT_OPERATION_ID = :OPERATION_ID ORDER BY SR.ID ASC ";

        //抽检方案名称ID: PROJECT_ID;
        public const string sampleProjectConfigSql = @"SELECT * FROM SFCS_SAMPLE_PROJECT_CONFIG WHERE PROJECT_ID = :PROJECT_ID ORDER BY ORDER_NO ";

        public const string spotCheckHeaderSql = @"SELECT * FROM MES_SPOTCHECK_HEADER WHERE WO_NO = :WO_NO AND QC_TYPE = '2' AND OPERATION_SITE_ID = :SITE_ID ORDER BY CREATE_DATE DESC ";

        public const string spotCheckDetailSql = @"SELECT * FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO  ORDER BY ID DESC ";

        public const string passQtySql = @"SELECT NVL(SUM(SAMPLE_RATIO_COUNT) ,0) PASS_QTY  FROM MES_SPOTCHECK_HEADER SH,SFCS_SAMPLE_PROJECT_CONFIG PC WHERE SH.QCSCHEMAHEAD = PC.ID AND SH.WO_NO = :WO_NO AND SH.QC_TYPE = '2' AND SH.OPERATION_SITE_ID = :SITE_ID ORDER BY CREATE_DATE DESC ";

        public const string snQtySql = @"SELECT COUNT(0) OPERATION_QTY FROM (SELECT SN_ID,OPERATION_SITE_ID FROM SFCS_OPERATION_HISTORY WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :SITE_ID GROUP BY SN_ID,OPERATION_SITE_ID) ";

        public const string parametersSql = @"SELECT PA.LOOKUP_CODE,PA.MEANING,PA.DESCRIPTION FROM SFCS_PARAMETERS PA WHERE PA.LOOKUP_TYPE= 'SAMPLE_RATIO' AND PA.ENABLED = 'Y' AND PA.LOOKUP_CODE = :LOOKUP_CODE ORDER BY PA.LOOKUP_CODE ";

        public const string insertSpotcheckHeaderSql = @"INSERT INTO MES_SPOTCHECK_HEADER (BATCH_NO, LINE_ID, LINE_TYPE, WO_NO, ALL_QTY, CHECK_QTY, FAIL_QTY, SAMP_STANDART, SAMP_SIZE, STATUS, CHECKER, CONFIRM, AUDITOR, RESULT, CREATE_DATE, ORGANIZE_ID, WO_QTY, ORDER_NO, OUTER_CHECK_QTY, OUTER_FAIL_QTY, REMARK, WO_CLASS, QC_TYPE, PARENT_BATCH_NO, QCSCHEMAHEAD, QCSCHEMANAME, QCSCHEMAVERSION, OPERATION_SITE_ID) VALUES (:BATCH_NO, :LINE_ID, :LINE_TYPE, :WO_NO, :ALL_QTY, :CHECK_QTY, :FAIL_QTY, 0, null, :STATUS, :CHECKER, null, null, null, SYSDATE, :ORGANIZE_ID, null, null, null, null, null, null, :QC_TYPE, :PARENT_BATCH_NO, :QCSCHEMAHEAD, :QCSCHEMANAME, :QCSCHEMAVERSION, :OPERATION_SITE_ID)";

        public const string insertDetailSql = @"INSERT INTO MES_SPOTCHECK_DETAIL (ID, BATCH_NO, SN, STATUS, CREATE_TIME, CREATOR) VALUES (:ID, :BATCH_NO, :SN, :STATUS, SYSDATE, :CREATOR)";

        public const string insertFailSql = @"INSERT INTO MES_SPOTCHECK_FAIL_DETAIL (ID, SPOTCHECK_DETAIL_ID, DEFECT_CODE, DEFECT_LOC, DEFECT_DESCRIPTION, DEFECT_MSG) VALUES (:ID, :SPOTCHECK_DETAIL_ID, :DEFECT_CODE, :DEFECT_LOC, :DEFECT_DESCRIPTION, :DEFECT_MSG)";

        public const string U_UpadateDetailSeq = @"UPDATE MES_SPOTCHECK_DETAIL SET STATUS= :STATUS,CREATOR= :CREATOR,CREATE_TIME= SYSDATE WHERE BATCH_NO=:BATCH_NO AND SN =:SN";

        public const string detailIdSql = @"SELECT T.ID FROM MES_SPOTCHECK_DETAIL T WHERE T.BATCH_NO = :BATCH_NO AND T.SN =:SN ";

        //根据不良代码获取不良现象
        public const string defectConfigSql = @"SELECT * FROM SFCS_DEFECT_CONFIG WHERE DEFECT_CODE = :DEFECT_CODE AND ENABLED = 'Y' ORDER BY ID DESC  ";

        #endregion

        /// <summary>
        /// 保存终检单数据
        /// </summary>
        private void SaveSpotCheck(Propertyprovider propertyprovider, SfcsSampleProjectConfig nextPlan, int snQty, int passQty, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            int RatioCount = Convert.ToInt32(nextPlan.SAMPLE_RATIO_COUNT) + passQty;//当前方案的抽检比例 + 已过的SN数量 = 当前抽检比例
            int StartQty = 0, all_qty = 0;//开始抽检数
            SfcsParameters parameters = repository.QueryEx<SfcsParameters>(parametersSql, new { LOOKUP_CODE = nextPlan.SAMPLE_RATIO }, transaction)?.FirstOrDefault();//获取抽检比例
            if (parameters != null && !String.IsNullOrEmpty(parameters.MEANING))
            {
                try
                {
                    var mArr = parameters.MEANING.Split(':'); if (mArr.Length < 1) { mArr = parameters.MEANING.Split('：'); }
                    if (mArr.Length > 0)
                    {
                        all_qty = Convert.ToInt32(mArr[0]);//送检数
                        StartQty = (RatioCount - all_qty) + 1;//开始抽检数
                    }
                }
                catch (Exception) { StartQty = 0; }
            }
            if (StartQty > 0 && RatioCount > 0 && snQty == StartQty)
            {
                String qcDocNo = GetBatchNo(repository);
                AddSpotCheckHeader(propertyprovider, qcDocNo, all_qty, nextPlan.ID, repository, transaction);

                AddSpotCheckDetail(propertyprovider, qcDocNo, repository, transaction);
            }
        }

        /// <summary>
        /// 添加终检单数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="qcDocNo"></param>
        /// <param name="all_qty"></param>
        /// <param name="QCSchemaHead"></param>
        /// <param name="repository"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private int AddSpotCheckHeader(Propertyprovider propertyprovider, String qcDocNo, int all_qty, Decimal? QCSchemaHead, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            int result = 0;

            int fail_qty = propertyprovider.defects == null ? 0 : 1;
            DynamicParameters p = new DynamicParameters();
            p.Add("BATCH_NO", qcDocNo, System.Data.DbType.String);//质检单号
            p.Add("LINE_ID", propertyprovider.sfcsOperationLines.Id, System.Data.DbType.Decimal);//线别id
            p.Add("LINE_TYPE", propertyprovider.sfcsOperationLines.PLANT_CODE, System.Data.DbType.String);//线别类型：SMT,PCBA
            p.Add("WO_NO", propertyprovider.product.workOrder, System.Data.DbType.String);//工单号
            p.Add("ALL_QTY", all_qty, System.Data.DbType.Decimal);//送检数
            p.Add("CHECK_QTY", 1, System.Data.DbType.Decimal);//抽检数
            p.Add("FAIL_QTY", fail_qty, System.Data.DbType.Decimal);//不良数  判断当前SN有没有不良 
            p.Add("STATUS", 0, System.Data.DbType.Decimal);//状态：0 新增； 2 确认；3审核；
            p.Add("CHECKER", propertyprovider.sys_Manager.USER_NAME, System.Data.DbType.String);//检验员，创建人员
            p.Add("ORGANIZE_ID", propertyprovider.sfcsOperationLines.ORGANIZE_ID, System.Data.DbType.String);//组织架构id
            p.Add("QC_TYPE", 2, System.Data.DbType.Decimal);//检验类型：0：过程检验，1：完工检验，2：终检检验
            p.Add("PARENT_BATCH_NO", "", System.Data.DbType.String);//父级检验单ID
            p.Add("QCSCHEMAHEAD", QCSchemaHead, System.Data.DbType.String);//质检方案ID
            p.Add("QCSCHEMANAME", "", System.Data.DbType.String);//质检方案名称
            p.Add("QCSCHEMAVERSION", "", System.Data.DbType.String);//质检方案版本
            p.Add("OPERATION_SITE_ID", propertyprovider.sfcsOperationSites.ID, System.Data.DbType.Decimal);//站点
            result = repository.Execute(insertSpotcheckHeaderSql, p, transaction, commandType: CommandType.Text);

            return result;
        }

        /// <summary>
        /// 添加终检单详情数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="qcDocNo"></param>
        /// <param name="repository"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private int AddSpotCheckDetail(Propertyprovider propertyprovider, String qcDocNo, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            int result = 0;

            #region 添加抽检明细
            decimal detailId = 0;
            String status = propertyprovider.defects == null ? GlobalVariables.PassStatus : GlobalVariables.FailStatus;
            String id = repository.QueryEx<String>(detailIdSql, new { BATCH_NO = qcDocNo, SN = propertyprovider.sfcsRuncard.SN }).FirstOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                DynamicParameters parametersC = new DynamicParameters();
                detailId = repository.QueryEx<decimal>("SELECT MES_SPOTCHECK_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                parametersC.Add("ID", detailId, System.Data.DbType.Decimal);
                parametersC.Add("BATCH_NO", qcDocNo, System.Data.DbType.String);
                parametersC.Add("SN", propertyprovider.sfcsRuncard.SN, System.Data.DbType.String);
                parametersC.Add("STATUS", status, System.Data.DbType.String);//抽检状态:PASS,FAIL
                parametersC.Add("CREATOR", propertyprovider.sys_Manager.USER_NAME, System.Data.DbType.String);//创建人
                result += repository.Execute(insertDetailSql, parametersC, transaction, commandType: CommandType.Text);
            }
            else
            {
                detailId = Convert.ToDecimal(id);
                result += repository.Execute(U_UpadateDetailSeq, new
                {
                    STATUS = status,
                    CREATOR = propertyprovider.sys_Manager.USER_NAME,
                    BATCH_NO = qcDocNo,
                    SN = propertyprovider.sfcsRuncard.SN
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
                SfcsDefectConfig DefectList = repository.QueryEx<SfcsDefectConfig>(defectConfigSql, new { DEFECT_CODE = defect_code }, transaction).FirstOrDefault();
                String defect_description = DefectList == null ? "" : DefectList.DEFECT_DESCRIPTION;
                parametersD.Add("DEFECT_MSG", defect_description, System.Data.DbType.String);//不良现象
                result += repository.Execute(insertFailSql, parametersD, transaction, commandType: CommandType.Text);
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 更新终检单
        /// </summary>
        /// <param name="header"></param>
        /// <param name="repository"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private int ModifySpotCheckHeader(MesSpotcheckHeader header, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            int result = 0;
            String upSql = "UPDATE MES_SPOTCHECK_HEADER SET CHECK_QTY= :CHECK_QTY,FAIL_QTY= :FAIL_QTY WHERE BATCH_NO=:BATCH_NO";
            Decimal check_qty = header.CHECK_QTY + 1;
            if (check_qty == header.ALL_QTY) { upSql = "UPDATE MES_SPOTCHECK_HEADER SET CHECK_QTY= :CHECK_QTY,FAIL_QTY= :FAIL_QTY,STATUS = '2' WHERE BATCH_NO=:BATCH_NO"; }
            result += repository.Execute(upSql, new
            {
                CHECK_QTY = check_qty,
                FAIL_QTY = header.FAIL_QTY,
                BATCH_NO = header.BATCH_NO
            }, transaction);

            return result;
        }

        /// <summary>
        /// 获取单号
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private String GetBatchNo(IBaseRepository<SfcsRuncard, decimal> repository)
        {
            //使用SFCS_PACKING_CARTON_SEQ
            decimal sequence = repository.QueryEx<decimal>("SELECT SFCS_PACKING_CARTON_SEQ.NEXTVAL FROM DUAL ").FirstOrDefault();

            //將序列轉成36進制表示
            string result = Core.Utilities.RadixConvertPublic.RadixConvert(sequence.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

            //六位表示
            string ReleasedSequence = result.PadLeft(6, '0');
            string yymmdd = repository.QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
            return "QC" + yymmdd + ReleasedSequence;
        }

    }
}
