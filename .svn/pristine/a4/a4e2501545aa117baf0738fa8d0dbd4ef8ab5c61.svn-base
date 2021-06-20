using System;
using System.Data;
using System.Text;
using JZ.IMS.Core;
using System.Linq;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Repository;
using System.Collections.Generic;
using Dapper;
using System.Reflection;

namespace JZ.IMS.Job.SubModuleJobStorage
{
    public class StopLine
    {
        #region field

        public IBaseRepository<SfcsRuncard, decimal> repositorySL;

        public Propertyprovider propertyproviderSL;

        public IDbTransaction transactionSL;

        private List<ProductStoplineModel> productStopLineList;
        private List<SfcsStoplineBatchHeaderListModel> stopLineBatchHeaderList;

        /// <summary>
        /// Hold產品業務類
        /// </summary>
        private HoldProduct holdProduct;

        private string programName;
        private string batchNumber;
        private string issueMessage;
        private decimal totalCount;
        private decimal passCount;
        private decimal failCount;
        private decimal ndfCount;
        private decimal failRate;
        private decimal ndfRate;
        private decimal headerID;
        private decimal stopLineMode;
        private decimal runcardStatus;
        private int mainCondition;
        private int subsidiaryCondition;
        private int holdAction;

        #endregion

        #region property

        /// <summary>
        /// Product StopLine List
        /// </summary>
        public List<ProductStoplineModel> ProductStopLineList
        {
            get
            {
                return this.productStopLineList;
            }
        }

        /// <summary>
        /// Product StopLine Data Row
        /// Part Number + StopLine Mode + Product Operation Code聯合為一
        /// </summary>
        public ProductStoplineModel ProductStopLineModel
        {
            get
            {
                if (this.productStopLineList == null || this.productStopLineList.Count() <= 0)
                {
                    return new ProductStoplineModel();
                }
                else
                {
                    return this.productStopLineList[0];
                }
            }
        }

        /// <summary>
        /// StopLine Batch Header
        /// </summary>
        public SfcsStoplineBatchHeaderListModel StopLineBatchHeaderModel
        {
            get
            {
                if (this.stopLineBatchHeaderList == null || this.stopLineBatchHeaderList.Count() <= 0)
                {
                    return new SfcsStoplineBatchHeaderListModel();
                }
                else
                {
                    return this.stopLineBatchHeaderList[0];
                }
            }
        }


        /// <summary>
        /// 異常事件消息
        /// </summary>
        public string IssueMessage
        {
            get
            {
                return this.issueMessage;
            }
            set
            {
                this.issueMessage = value;
            }
        }

        /// <summary>
        /// 停線管控模式
        /// </summary>
        public decimal StopLineMode
        {
            get
            {
                return this.stopLineMode;
            }
            set
            {
                this.stopLineMode = value;
            }
        }

        /// <summary>
        /// 取得系统时间
        /// </summary>
        public DateTime SystemTime
        {
            get
            {
                return repositorySL.GetCurrentDateTimeEx();
            }
        }

        /// <summary>
        /// 本Batch中的Pass數量
        /// </summary>
        public decimal PassCount
        {
            get
            {
                return this.passCount;
            }
            set
            {
                this.passCount = value;
            }
        }

        /// <summary>
        /// 本Batch中的Fail數量
        /// </summary>
        public decimal FailCount
        {
            get
            {
                return this.failCount;
            }
            set
            {
                this.failCount = value;
            }
        }

        /// <summary>
        /// 本Batch中NDF數量
        /// </summary>
        public decimal NDFCount
        {
            get
            {
                return this.ndfCount;
            }
            set
            {
                this.ndfCount = value;
            }
        }

        /// <summary>
        /// 不良率
        /// </summary>
        public decimal FailRate
        {
            get
            {
                return this.failRate;
            }
            set
            {
                this.failRate = value;
            }
        }

        /// <summary>
        /// Batch總量
        /// </summary>
        public decimal TotalCount
        {
            get
            {
                return this.totalCount;
            }
            set
            {
                this.totalCount = value;
            }
        }

        /// <summary>
        /// 成品工序代码
        /// </summary>
        public decimal ProductOperationCode
        {
            get
            {
                return Convert.ToDecimal(this.propertyproviderSL.route.sfcsRouteConfigs.Where(m => m.CURRENT_OPERATION_ID == this.propertyproviderSL.sfcsOperationSites.OPERATION_ID)?.FirstOrDefault().PRODUCT_OPERATION_CODE);
            }
        }

        #endregion

        #region constructor

        public StopLine()
        {
            this.InitializeParameters();
            this.programName = this.GetType().BaseType.Name + GlobalVariables.Hyphen + this.GetType().Name;
            productStopLineList = new List<ProductStoplineModel>();
            stopLineBatchHeaderList = new List<SfcsStoplineBatchHeaderListModel>();
            this.holdProduct = new HoldProduct();
            this.holdProduct.ShowMessage = false;
            this.holdProduct.CurrentOperator = "SFCS";
            this.mainCondition = (int)HoldProductMainCondition.HoldSiteByPartNumberOrWorkOrder;
            this.subsidiaryCondition = -1;
            this.holdAction = (int)HoldOperation.HoldWIP;
            this.issueMessage = null;
        }

        #endregion

        #region custom method

        private void InitializeParameters()
        {
            this.headerID = 0;
            this.passCount = 0;
            this.failCount = 0;
            this.ndfCount = 0;
            this.totalCount = 0;
            this.failRate = 0;
            this.ndfRate = 0;
        }

        #endregion

        #region stop line method

        /// <summary>
        /// 取得StopLine配置信息
        /// </summary>
        protected void GetProductStopLine(GetProductStopLineRequestModel model)
        {
            productStopLineList = new List<ProductStoplineModel>();
            String S_SelectStopLine = new QueryHelp().InquireSQLHelper<GetProductStopLineRequestModel>(model, "SFCS_PRODUCT_STOPLINE");
            productStopLineList = repositorySL.QueryEx<ProductStoplineModel>(S_SelectStopLine, model, transactionSL);
        }

        /// <summary>
        /// 初始化產品StopLine管控
        /// </summary>
        protected void InitializeProductStopLine()
        {
            //partNumber 成品料號  productOperationCode 產品工序代碼
            this.GetProductStopLine(new GetProductStopLineRequestModel()
            {
                PART_NO = this.propertyproviderSL.product.partNumber,
                STOPLINE_MODE = this.StopLineMode,
                STOP_OPERATION_CODE = ProductOperationCode,
                ENABLED = GlobalVariables.EnableY
            });
        }

        /// <summary>
        /// 取得StopLine Header
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected void GetStopLineHeaderList(GetStopLineHeaderRequestModel model)
        {
            stopLineBatchHeaderList = new List<SfcsStoplineBatchHeaderListModel>();
            String S_SelectStopLineHeader = new QueryHelp().InquireSQLHelper<GetStopLineHeaderRequestModel>(model, "SFCS_STOPLINE_BATCH_HEADER");
            stopLineBatchHeaderList = repositorySL.QueryEx<SfcsStoplineBatchHeaderListModel>(S_SelectStopLineHeader, model, transactionSL);
        }

        /// <summary>
        /// 添加StopLine Batch Header記錄
        /// </summary>
        protected void InsertStopLineHeader()
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("ID", this.headerID, System.Data.DbType.String);
            p.Add("BATCH_NO", this.batchNumber, System.Data.DbType.String);
            p.Add("PART_NO", this.propertyproviderSL.product.partNumber, System.Data.DbType.String);
            p.Add("OPERATION_SITE_ID", this.propertyproviderSL.sfcsOperationSites.ID, System.Data.DbType.String);
            p.Add("PASS_COUNT", this.passCount, System.Data.DbType.String);
            p.Add("FAIL_COUNT", this.failCount, System.Data.DbType.String);
            p.Add("NDF_COUNT", this.ndfCount, System.Data.DbType.String);
            p.Add("DIVISION_CRITERIA", ProductStopLineModel.DIVISION_CRITERIA, System.Data.DbType.String);
            p.Add("ALARM_CYCLE_COUNT", ProductStopLineModel.ALARM_INTERVAL - 1, System.Data.DbType.String);
            p.Add("STOPLINE_MODE", ProductStopLineModel.STOPLINE_MODE, System.Data.DbType.String);
            p.Add("STATUS", GlobalVariables.RecordActive, System.Data.DbType.String);
            repositorySL.Execute(I_InsertStopLineHeader, p, transactionSL, commandType: CommandType.Text);
        }

        /// <summary>
        /// 更新StopLine Batch Header記錄
        /// </summary>
        protected void UpdateStopLineHeader(decimal status)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("PASS_COUNT", this.passCount, System.Data.DbType.String);
            p.Add("FAIL_COUNT", this.failCount, System.Data.DbType.String);
            p.Add("NDF_COUNT", this.ndfCount, System.Data.DbType.String);
            p.Add("DIVISION_CRITERIA", this.ProductStopLineModel.DIVISION_CRITERIA, System.Data.DbType.String);
            p.Add("STATUS", status, System.Data.DbType.String);
            p.Add("ID", this.headerID, System.Data.DbType.String);
            repositorySL.Execute(U_UpdateStopLineHeader, p, transactionSL, commandType: CommandType.Text);
        }

        /// <summary>
        /// 按Batch修改統計數據的狀態
        /// </summary>
        protected void UpdateStopLineFailStatisticsStatus(decimal status, decimal? id, decimal? headerID)
        {
            String sWhere = "";
            DynamicParameters p = new DynamicParameters();
            p.Add("STATUS", status, System.Data.DbType.String);
            if (id != null)
            {
                sWhere += " ID = :ID ";
                p.Add("ID", id, System.Data.DbType.String);
            }
            if (headerID != null)
            {
                sWhere = String.IsNullOrEmpty(sWhere) ? " HEADER_ID = :HEADER_ID" : " AND HEADER_ID = :HEADER_ID";
                p.Add("HEADER_ID", headerID, System.Data.DbType.String);
            }

            repositorySL.Execute("UPDATE SFCS_STOPLINE_FAIL_STATISTICS SET STATUS = :STATUS WHERE " + sWhere, p, transactionSL, commandType: CommandType.Text);
        }

        /// <summary>
        /// 取得StopLine Header
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected List<SfcsStoplineBatchDetailListModel> GetStopLineDetailList(StoplineBatchDetailRequestModel model)
        {
            String sQuery = new QueryHelp().InquireSQLHelper<StoplineBatchDetailRequestModel>(model, "SFCS_STOPLINE_BATCH_DETAIL");
            return repositorySL.QueryEx<SfcsStoplineBatchDetailListModel>(sQuery, model, transactionSL);
        }

        /// <summary>
        /// 獲取產品收集不良
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected List<SfcsCollectDefectsListModel> GetCollectDefects(GetCollectDefectsRequestModel model)
        {
            String sQuery = new QueryHelp().InquireSQLHelper<GetCollectDefectsRequestModel>(model, "SFCS_COLLECT_DEFECTS");
            return repositorySL.QueryEx<SfcsCollectDefectsListModel>(sQuery, model, transactionSL);
        }

        /// <summary>
        /// 獲取維修數據信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected List<RepairRecipeListModel> GetRepairRecipeData(GetRepairRecipeRequestModel model)
        {
            String sQuery = new QueryHelp().InquireSQLHelper<GetRepairRecipeRequestModel>(model, "SFCS_REPAIR_RECIPE");
            return repositorySL.QueryEx<RepairRecipeListModel>(sQuery, model, transactionSL);
        }

        /// <summary>
        /// 獲取停線不良統計DataTable
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected List<SfcsStoplineFailStatisticsListModel> GetStopLineFailStatisticsDataTable(GetStoplineFailStatisticsRequestModel model)
        {
            String sQuery = new QueryHelp().InquireSQLHelper<GetStoplineFailStatisticsRequestModel>(model, "SFCS_STOPLINE_FAIL_STATISTICS");
            return repositorySL.QueryEx<SfcsStoplineFailStatisticsListModel>(sQuery, model, transactionSL);
        }

        /// <summary>
        /// 更新停線不良統計
        /// 必需包含參數VALUE_COUNT和NEW_STATUS
        /// </summary>
        /// <param name="parameters"></param>
        public void UpdateStopLneFailStatisticsCount(decimal valueCount, decimal id)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("VALUE_COUNT", valueCount, System.Data.DbType.String);
            p.Add("ID", id, System.Data.DbType.String);
            repositorySL.Execute(U_UpdateStopLineFailStatisticsCount, p, transactionSL, commandType: CommandType.Text);
        }

        /// <summary>
        /// 新增停線不良統計
        /// </summary>
        protected void InsertStopLineFailStatistics(decimal dataType, string dataValue, decimal status)
        {
            //新增停線不良統計記錄
            DynamicParameters p = new DynamicParameters();
            p.Add("HEADER_ID", this.headerID, System.Data.DbType.String);
            p.Add("SN_ID", propertyproviderSL.sfcsRuncard.ID, System.Data.DbType.String);
            p.Add("PART_NO", this.propertyproviderSL.product.partNumber, System.Data.DbType.String);
            p.Add("OPERATION_SITE_ID", this.propertyproviderSL.sfcsOperationSites.ID, System.Data.DbType.String);
            p.Add("STOPLINE_MODE", this.ProductStopLineModel.STOPLINE_MODE, System.Data.DbType.String);
            p.Add("DATA_TYPE", dataType, System.Data.DbType.String);
            p.Add("DATA_VALUE", dataValue, System.Data.DbType.String);
            p.Add("VALUE_COUNT", 1, System.Data.DbType.String);
            p.Add("STATUS", status, System.Data.DbType.String);
            repositorySL.Execute(I_InsertStopLineFailStatistics, p, transactionSL, commandType: CommandType.Text);
        }

        /// <summary>
        /// 記錄StopLine Batch的具體信息
        /// </summary>
        protected void InsertStopLineDetail()
        {
            //記錄停線處理具體信息
            DynamicParameters p = new DynamicParameters();
            p.Add("HEADER_ID", this.headerID, System.Data.DbType.String);
            p.Add("SN_ID", propertyproviderSL.sfcsRuncard.ID, System.Data.DbType.String);
            p.Add("SN", propertyproviderSL.sfcsRuncard.SN, System.Data.DbType.String);
            p.Add("OPERATION_ID", propertyproviderSL.OperationId, System.Data.DbType.String);//操作唯一標識
            p.Add("STATUS", this.runcardStatus, System.Data.DbType.String);
            repositorySL.Execute(I_InsertStopLineDetail, p, transactionSL, commandType: CommandType.Text);
        }

        /// <summary>
        /// 取得對應狀態（Pass/Fail）數量
        /// </summary>
        /// <returns></returns>
        protected decimal GetDetailCount(decimal? status)
        {
            StoplineBatchDetailRequestModel model = new StoplineBatchDetailRequestModel()
            {
                HEADER_ID = this.headerID,
                STATUS = status
            };

            String sQuery = new QueryHelp().InquireSQLHelper<StoplineBatchDetailRequestModel>(model, "SFCS_STOPLINE_BATCH_DETAIL");
            List<SfcsStoplineBatchDetailListModel> stopLineBatchDetailDataTable = repositorySL.QueryEx<SfcsStoplineBatchDetailListModel>(sQuery, model, transactionSL);

            return stopLineBatchDetailDataTable != null ? stopLineBatchDetailDataTable.Count : 0;
        }

        /// <summary>
        /// 取得NDF數量
        /// </summary>
        /// <returns></returns>
        protected decimal GetNDFCount()
        {
            GetStoplineFailStatisticsRequestModel model = new GetStoplineFailStatisticsRequestModel()
            {
                HEADER_ID = this.headerID,
                DATA_TYPE = GlobalVariables.NDFCount
            };

            String sQuery = new QueryHelp().InquireSQLHelper<GetStoplineFailStatisticsRequestModel>(model, "SFCS_STOPLINE_FAIL_STATISTICS");
            List<SfcsStoplineFailStatisticsListModel> stopLineFailStatisticsDataTable = repositorySL.QueryEx<SfcsStoplineFailStatisticsListModel>(sQuery, model, transactionSL);

            decimal ndfCount = 0;
            foreach (var row in stopLineFailStatisticsDataTable)
            {
                ndfCount += Convert.ToDecimal(row.VALUE_COUNT);
            }
            return ndfCount;
        }

        /// <summary>
        /// 刷新數量(PASS/FAIL/NDF)統計
        /// </summary>
        protected void RefreshCount()
        {
            this.PassCount = this.GetDetailCount(GlobalVariables.Pass);
            this.totalCount = this.GetDetailCount(null);
            this.NDFCount = this.GetNDFCount();
            if (this.ProductStopLineModel.INCLUDE_NDF == GlobalVariables.EnableY)
            {
                this.FailCount = this.GetDetailCount(GlobalVariables.Fail) - this.NDFCount;
            }
            else
            {
                this.FailCount = this.GetDetailCount(GlobalVariables.Fail);
            }
        }

        /// <summary>
        /// 計算不良狀況
        /// </summary>
        protected void RefreshRate()
        {
            this.failRate = System.Convert.ToDecimal(((double)this.failCount / (double)this.totalCount).ToString("f2"));
            this.ndfRate = System.Convert.ToDecimal(((double)this.ndfCount / (double)this.totalCount).ToString("f2"));
        }

        /// <summary>
        /// 記錄停線異常
        /// </summary>
        protected void LogStopLineHistory(decimal issueType)
        {
            //記錄停線事件
            DynamicParameters p = new DynamicParameters();
            p.Add("HEADER_ID", this.headerID, System.Data.DbType.String);
            p.Add("BATCH_NO", this.batchNumber, System.Data.DbType.String);
            p.Add("PART_NO", propertyproviderSL.product.partNumber, System.Data.DbType.String);
            p.Add("OPERATION_SITE_ID", propertyproviderSL.sfcsOperationSites.ID, System.Data.DbType.String);
            p.Add("PASS_COUNT", this.passCount, System.Data.DbType.String);
            p.Add("FAIL_COUNT", this.failCount, System.Data.DbType.String);
            p.Add("NDF_COUNT", this.ndfCount, System.Data.DbType.String);
            p.Add("TOTAL_COUNT", this.totalCount, System.Data.DbType.String);
            p.Add("FAIL_RATE", this.failRate, System.Data.DbType.String);
            p.Add("NDF_RATE", this.ndfRate, System.Data.DbType.String);
            p.Add("DIVISION_CRITERIA", ProductStopLineModel.DIVISION_CRITERIA, System.Data.DbType.String);
            p.Add("INCLUDE_NDF", ProductStopLineModel.INCLUDE_NDF, System.Data.DbType.String);
            p.Add("STOPLINE_MODE", ProductStopLineModel.STOPLINE_MODE, System.Data.DbType.String);
            p.Add("ISSUE_TYPE", issueType, System.Data.DbType.String);
            p.Add("ISSUE_MESSAGE", this.issueMessage, System.Data.DbType.String);
            repositorySL.Execute(I_InsertStopLineHistory, p, transactionSL, commandType: CommandType.Text);
        }

        /// <summary>
        /// 更新循環報警計算器
        /// </summary>
        protected void UpdateStopLineHeaderAlarmCycleCount(decimal alarmCycleCount)
        {
            //記錄停線事件
            DynamicParameters p = new DynamicParameters();
            p.Add("ALARM_CYCLE_COUNT", alarmCycleCount, System.Data.DbType.String);
            p.Add("ID", this.headerID, System.Data.DbType.String);
            repositorySL.Execute(U_UpdateStopLineHeaderAlarmCycleCount, p, transactionSL, commandType: CommandType.Text);
        }

        #endregion

        #region stop line common process

        /// <summary>
        /// 初始化SN狀態
        /// </summary>
        protected void InitializeRuncardStatus()
        {
            //this.runcardStatus = this.propertyproviderSL.product.runcardStatus == GlobalVariables.PassStatus ? GlobalVariables.Pass : GlobalVariables.Fail;
            this.runcardStatus = this.propertyproviderSL.defects == null ? GlobalVariables.Pass : GlobalVariables.Fail;
            this.batchNumber = null;
        }

        /// <summary>
        /// 處理Batch Header
        /// </summary>
        protected void ProcessBatchHeader()
        {
            this.GetStopLineHeaderList(new GetStopLineHeaderRequestModel()
            {
                PART_NO = this.propertyproviderSL.product.partNumber,
                OPERATION_SITE_ID = this.propertyproviderSL.sfcsOperationSites.ID,
                STOPLINE_MODE = this.stopLineMode,
                STATUS = GlobalVariables.RecordActive
            });

            if (this.stopLineBatchHeaderList == null || this.stopLineBatchHeaderList.Count <= 0)
            {
                this.batchNumber = GetStopLineBatchNumber();
                this.headerID = GetStopLineHeaderID();
                this.InsertStopLineHeader();
                this.GetStopLineHeaderList(new GetStopLineHeaderRequestModel() { ID = this.headerID });
            }
            else
            {
                this.batchNumber = this.StopLineBatchHeaderModel.BATCH_NO;
                this.headerID = this.StopLineBatchHeaderModel.ID;
            }
        }

        /// <summary>
        /// 判斷Batch關閉時間是否超出
        /// </summary>
        private void JudgeBatchCloseTimeOver()
        {
            switch ((int)this.stopLineMode)
            {
                case (int)GlobalVariables.FailCountInBaseTimeControl:
                case (int)GlobalVariables.FailRateInBaseTimeControl:
                case (int)GlobalVariables.LocationFailRateInBaseTimeControl:
                case (int)GlobalVariables.NDFCountInBaseTimeControl:
                case (int)GlobalVariables.NDFRateInBaseTimeControl:
                    // 數量達到切入點時，開始判斷是否超標
                    TimeSpan timeSpan = this.SystemTime - this.StopLineBatchHeaderModel.CREATED_TIME;
                    if ((decimal)timeSpan.TotalHours >= this.ProductStopLineModel.DIVISION_CRITERIA)
                    {
                        this.UpdateBatchDataStatus(GlobalVariables.RecordClosed);
                        this.ProcessBatchHeader();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 統計NDF數量
        /// </summary>
        protected void ProcessNDFStatistics()
        {
            // 設定需要統計NDF數量
            if (this.ProductStopLineModel.INCLUDE_NDF == GlobalVariables.EnableY)
            {
                List<SfcsStoplineBatchDetailListModel> serialNumberBatchDetailDataTable = GetStopLineDetailList(new StoplineBatchDetailRequestModel()
                {
                    HEADER_ID = this.headerID,
                    SN_ID = propertyproviderSL.sfcsRuncard.ID
                });
                if (serialNumberBatchDetailDataTable != null && serialNumberBatchDetailDataTable.Count > 0)
                {
                    serialNumberBatchDetailDataTable = serialNumberBatchDetailDataTable.OrderByDescending(m => m.COLLECT_TIME).ToList();
                    SfcsStoplineBatchDetailListModel serialNumberBatchDetailRow = serialNumberBatchDetailDataTable[0];

                    // 若上一次的記錄為Fail，通過Operation ID判斷上次Fail的對應的維修是否為NDF
                    // 若是，統計NDF次數
                    if (serialNumberBatchDetailRow.STATUS == GlobalVariables.Fail)
                    {
                        List<SfcsCollectDefectsListModel> collectDefectsDataTable = GetCollectDefects(new GetCollectDefectsRequestModel()
                        {
                            DEFECT_OPERATION_ID = serialNumberBatchDetailRow.OPERATION_ID
                        });
                        foreach (var collectDefectRow in collectDefectsDataTable)
                        {
                            List<RepairRecipeListModel> repairRecipeDataTable = GetRepairRecipeData(new GetRepairRecipeRequestModel()
                            {
                                COLLECT_DEFECT_ID = collectDefectRow.COLLECT_DEFECT_ID
                            });
                            foreach (var repairRecipeRow in repairRecipeDataTable)
                            {
                                if (repairRecipeRow.REASON_CODE == GlobalVariables.NTF)
                                {
                                    List<SfcsStoplineFailStatisticsListModel> failStatisticsDataTable = GetStopLineFailStatisticsDataTable(new GetStoplineFailStatisticsRequestModel()
                                    {
                                        HEADER_ID = this.headerID,
                                        SN_ID = propertyproviderSL.sfcsRuncard.ID,//流水號鍵值
                                        DATA_TYPE = GlobalVariables.NDFCount
                                    });
                                    if (failStatisticsDataTable != null && failStatisticsDataTable.Count > 0)
                                    {
                                        SfcsStoplineFailStatisticsListModel failStatisticsRow = failStatisticsDataTable[0];
                                        decimal value_count = Convert.ToDecimal(failStatisticsRow.VALUE_COUNT) + 1;
                                        UpdateStopLneFailStatisticsCount(value_count, failStatisticsRow.ID);
                                    }
                                    else
                                    {
                                        this.InsertStopLineFailStatistics(GlobalVariables.NDFCount, propertyproviderSL.sfcsRuncard.SN, GlobalVariables.RecordActive);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 處理本批次的StopLine數據
        /// </summary>
        protected void ProcessStopLineBatch()
        {
            // 初始化SN狀態
            this.InitializeRuncardStatus();

            // 處理Batch Header
            this.ProcessBatchHeader();

            // 判斷Batch是否超期管控
            this.JudgeBatchCloseTimeOver();

            // 統計NDF數量
            this.ProcessNDFStatistics();

            // 記錄Batch Detail
            this.InsertStopLineDetail();

            // 刷新數量
            this.RefreshCount();

            // 計算比率
            this.RefreshRate();
        }

        /// <summary>
        /// 是否產生過停線事件
        /// </summary>
        /// <returns></returns>
        protected bool HasStopLineIssue()
        {
            StoplineHistoryRequestModel model = new StoplineHistoryRequestModel() { HEADER_ID = this.headerID };
            String sQuery = new QueryHelp().InquireSQLHelper<StoplineHistoryRequestModel>(model, "SFCS_STOPLINE_HISTORY");
            List<SfcsStoplineHistoryListModel> list = repositorySL.QueryEx<SfcsStoplineHistoryListModel>(sQuery, model, transactionSL);
            if (list != null && list.Count() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新Batch數據的狀態
        /// </summary>
        protected void UpdateBatchDataStatus(decimal status)
        {
            this.UpdateStopLineHeader(status);
            if (status == GlobalVariables.RecordClosed)
            {
                UpdateStopLineFailStatisticsStatus(GlobalVariables.RecordClosed, null, this.headerID);
            }
        }

        #endregion

        #region Hold Site

        /// <summary>
        /// 鎖定站點
        /// </summary>
        protected void HoldOperationSite(IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            this.holdProduct.HoldCause = this.issueMessage;
            this.holdProduct.ProcessProductOperationSite(
                this.mainCondition, this.subsidiaryCondition,
                this.holdAction, propertyproviderSL.product.partNumber,
                propertyproviderSL.sfcsOperationSites.OPERATION_SITE_NAME,
                propertyproviderSL.sfcsOperationSites.ID, this.batchNumber,
                ProductOperationCode, repositorySL);//ProductOperationCode 產品工序代碼
        }

        #endregion

        #region SQL

        #region SFCS_STOPLINE_BATCH_HEADER

        public const string I_InsertStopLineHeader = @"INSERT INTO SFCS_STOPLINE_BATCH_HEADER(ID, BATCH_NO, PART_NO, OPERATION_SITE_ID, PASS_COUNT, FAIL_COUNT, NDF_COUNT, DIVISION_CRITERIA, ALARM_CYCLE_COUNT, STOPLINE_MODE, STATUS) VALUES (:ID, :BATCH_NO, :PART_NO, :OPERATION_SITE_ID, :PASS_COUNT, :FAIL_COUNT, :NDF_COUNT, :DIVISION_CRITERIA, :ALARM_CYCLE_COUNT, :STOPLINE_MODE, :STATUS) ";

        public const string S_SelectStopLineHeaderIDSequence = @"SELECT SFCS_STOPLINE_HEADER_SEQ.NEXTVAL ID FROM DUAL ";

        public const string S_SelectStopLineBatchNumberSequence = @"SELECT 'SL' || LPAD(SFCS_STOPLINE_BATCH_SEQ.NEXTVAL,15,'0') BATCH_NO FROM DUAL ";

        public const string U_UpdateStopLineHeader = @"UPDATE SFCS_STOPLINE_BATCH_HEADER SET PASS_COUNT = :PASS_COUNT,
                                                       FAIL_COUNT = :FAIL_COUNT, NDF_COUNT = :NDF_COUNT,
                                                       DIVISION_CRITERIA = :DIVISION_CRITERIA, STATUS = :STATUS,
                                                       UPDATED_TIME = SYSDATE WHERE ID = :ID ";

        public const string U_UpdateStopLineHeaderAlarmCycleCount = @"UPDATE SFCS_STOPLINE_BATCH_HEADER 
                                                                      SET ALARM_CYCLE_COUNT = :ALARM_CYCLE_COUNT,
                                                                      UPDATED_TIME = SYSDATE WHERE ID = :ID ";

        #endregion

        #region SFCS_STOPLINE_FAIL_STATISTICS

        public const string U_UpdateStopLineFailStatisticsCount = @"UPDATE SFCS_STOPLINE_FAIL_STATISTICS SET VALUE_COUNT = :VALUE_COUNT, UPDATED_TIME = SYSDATE WHERE ID = :ID ";

        public const string I_InsertStopLineFailStatistics = @"INSERT INTO SFCS_STOPLINE_FAIL_STATISTICS (ID, HEADER_ID,
                                                               SN_ID, PART_NO, OPERATION_SITE_ID, STOPLINE_MODE, DATA_TYPE,
                                                               DATA_VALUE, VALUE_COUNT, STATUS)
                                                               VALUES (SFCS_STOPLINE_FAIL_STAT_SEQ.NEXTVAL, :HEADER_ID,
                                                               :SN_ID, :PART_NO, :OPERATION_SITE_ID, :STOPLINE_MODE, :DATA_TYPE,
                                                               :DATA_VALUE, :VALUE_COUNT, :STATUS) ";
        #endregion

        #region SFCS_STOPLINE_BATCH_DETAIL

        public const string I_InsertStopLineDetail = @"INSERT INTO SFCS_STOPLINE_BATCH_DETAIL (ID, HEADER_ID, SN_ID,
                                                       SN, OPERATION_ID,  STATUS)
                                                       VALUES (SFCS_STOPLINE_DETAIL_SEQ.NEXTVAL, :HEADER_ID, :SN_ID,
                                                       :SN, :OPERATION_ID, :STATUS) ";

        #endregion

        #region SFCS_STOPLINE_HISTORY

        public const string I_InsertStopLineHistory = @"INSERT INTO SFCS_STOPLINE_HISTORY (ID, HEADER_ID, BATCH_NO, PART_NO,
                                                        OPERATION_SITE_ID, PASS_COUNT, FAIL_COUNT, NDF_COUNT,
                                                        TOTAL_COUNT, FAIL_RATE, NDF_RATE,
                                                        DIVISION_CRITERIA, INCLUDE_NDF, STOPLINE_MODE, ISSUE_TYPE, ISSUE_MESSAGE)
                                                        VALUES (SFCS_STOPLINE_HISTORY_SEQ.NEXTVAL, :HEADER_ID, :BATCH_NO, :PART_NO,
                                                        :OPERATION_SITE_ID, :PASS_COUNT, :FAIL_COUNT, :NDF_COUNT,
                                                        :TOTAL_COUNT, :FAIL_RATE, :NDF_RATE,
                                                        :DIVISION_CRITERIA, :INCLUDE_NDF, :STOPLINE_MODE, :ISSUE_TYPE, :ISSUE_MESSAGE) ";

        #endregion

        #endregion

        #region StopLineManager

        #region SFCS_STOPLINE_BATCH_HEADER

        /// <summary>
        /// 獲取停線處理總況ID
        /// </summary>
        /// <returns></returns>
        public Decimal GetStopLineHeaderID()
        {
            return repositorySL.QueryEx<Decimal>(S_SelectStopLineHeaderIDSequence).FirstOrDefault();
        }

        /// <summary>
        /// 獲取停線處理單據號碼
        /// </summary>
        /// <returns></returns>
        public String GetStopLineBatchNumber()
        {
            return repositorySL.QueryEx<String>(S_SelectStopLineBatchNumberSequence).FirstOrDefault();
        }
        #endregion

        #endregion

    }

    public delegate void DelegateOfShowOperationMessage(string message);

    public class HoldProduct
    {
        #region field

        private const int ReleaseThroughSN = 0;
        private const int ReleaseThroughComponentSN = 1;

        private List<SfcsRuncard> runcardTable;
        private int mainConditionSelectIndex;
        private int subsidiaryConditionSelectIndex;
        private int actionSelectIndex;

        private string workOrder;
        private string partNumber;
        private string model;
        private string carton;
        private string pallet;
        private string compPartNumber;
        private decimal operationID;

        private string serialNumber;
        private string compSerialNumber;
        private string snFilePath;
        private string compSnFilePath;

        private decimal beginStringIndex;
        private decimal endStringIndex;
        private string defineString;
        private string holdCause;
        private string ecnNo;

        private DateTime beginTime;
        private DateTime endTime;
        private string inventory;

        private string billNumber;
        private string currentOperator;

        private bool showMessage;

        private List<decimal> releaseResponsibilityList;

        #endregion

        #region property

        public int MainConditionSelectIndex
        {
            get
            {
                return this.mainConditionSelectIndex;
            }
            set
            {
                this.mainConditionSelectIndex = value;
            }
        }

        public int SubsidiaryConditionSelectIndex
        {
            get
            {
                return this.subsidiaryConditionSelectIndex;
            }
            set
            {
                this.subsidiaryConditionSelectIndex = value;
            }
        }

        public int ActionSelectIndex
        {
            get
            {
                return this.actionSelectIndex;
            }
            set
            {
                this.actionSelectIndex = value;
            }
        }

        //public string CurrentOperator
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(this.currentOperator))
        //        {
        //            return AuthorityControl.CurrentIdentity.User.WorkCode;
        //        }
        //        else
        //        {
        //            return this.currentOperator;
        //        }
        //    }
        //    set
        //    {
        //        // For系統自動鎖定
        //        this.currentOperator = value;
        //    }
        //}

        public string HoldCause
        {
            get
            {
                return this.holdCause;
            }
            set
            {
                this.holdCause = value;
            }
        }

        public string EcnNo
        {
            get
            {
                return this.ecnNo;
            }
            set
            {
                this.ecnNo = value;
            }
        }

        public string BillNumber
        {
            get
            {
                return this.billNumber;
            }
        }

        public bool ShowMessage
        {
            get
            {
                return this.showMessage;
            }
            set
            {
                this.showMessage = value;
            }
        }


        public string CurrentOperator
        {
            get
            {
                if (String.IsNullOrEmpty(this.currentOperator))
                {
                    //return AuthorityControl.CurrentIdentity.User.WorkCode;
                    return "";
                }
                else
                {
                    return this.currentOperator;
                }
            }
            set
            {
                // For系統自動鎖定
                this.currentOperator = value;
            }
        }

        /// <summary>
        /// 解鎖權限
        /// </summary>
        public List<decimal> ReleaseResponsibilityList
        {
            get
            {
                return this.releaseResponsibilityList;
            }
            set
            {
                this.releaseResponsibilityList = value;
            }
        }

        //public RuncardDataSet.SFCS_RUNCARDDataTable RuncardTable
        //{
        //    set
        //    {
        //        this.runcardTable = value;
        //    }
        //}

        public DelegateOfShowOperationMessage showOperationHistory;

        #endregion

        /// <summary>
        /// 處理料號，工單，站點
        /// </summary>
        /// <param name="mainConditionIndex"></param>
        /// <param name="subsidiaryConditionIndex"></param>
        /// <param name="actionIndex"></param>
        /// <param name="data"></param>
        /// <param name="operationSiteName"></param>
        /// <param name="operationSiteID"></param>
        /// <param name="holdNumber"></param>
        /// <param name="productOperationCode"></param>
        public void ProcessProductOperationSite(int mainConditionIndex,
            int subsidiaryConditionIndex, int actionIndex,
            string data, string operationSiteName, decimal operationSiteID,
            string holdNumber, decimal productOperationCode, IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            this.subsidiaryConditionSelectIndex = subsidiaryConditionIndex;
            this.mainConditionSelectIndex = mainConditionIndex;
            this.actionSelectIndex = actionIndex;
            this.IdentifyInputData(data, repositorySL);

            if (String.IsNullOrEmpty(data))
            {
                data = operationSiteName;
                operationSiteName = string.Empty;
            }

            try
            {
                this.billNumber = String.IsNullOrEmpty(holdNumber) ? GetHoldBill(repositorySL) : holdNumber;
                decimal holdID = GetHoldID(repositorySL);
                InsertHoldProductHeader(this.billNumber, holdID,
                    this.mainConditionSelectIndex,
                    data, this.subsidiaryConditionSelectIndex,
                    operationSiteName,
                    this.actionSelectIndex, GlobalVariables.EnableY,
                    1, this.holdCause, this.CurrentOperator, this.ecnNo, repositorySL);

                InsertHoldProductDetail(
                    holdID, -1, string.Empty, string.Empty,
                    this.actionSelectIndex, GlobalVariables.EnableY,
                    this.partNumber, this.workOrder, -1, operationSiteID,
                    productOperationCode, -1, this.model, repositorySL);

                if ((this.showOperationHistory != null) && this.showMessage)
                {
                    this.showOperationHistory(string.Format(" {0} &gt; 锁定成功！", data));
                }
                return;

            }
            catch (Exception e)
            {
                //throw new MESException(e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 辨識刷入數據
        /// </summary>
        /// <param name="data"></param>
        private void IdentifyInputData(string data, IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            this.carton = null;
            this.pallet = null;
            this.workOrder = null;
            this.partNumber = null;
            this.model = null;
            this.compPartNumber = null;
            this.operationID = 0;
            this.runcardTable = null;

            if (String.IsNullOrEmpty(data))
            {
                return;
            }

            //SELECT ID,SN,PARENT_SN,WO_ID,ROUTE_ID,CURRENT_SITE,WIP_OPERATION,LAST_OPERATION,STATUS,TURNIN_NO,TRACKING_NO,CARTON_NO,PALLET_NO,GG_NO,GG_ITEM,SMT_TURNIN_NO FROM SFCS_RUNCARD

            //SfcsRuncard model = new SfcsRuncard() { CARTON_NO = data };
            String sQuery = new QueryHelp().InquireSQLHelper<SfcsRuncard>(new SfcsRuncard() { CARTON_NO = data }, "SFCS_RUNCARD");
            this.runcardTable = repositorySL.QueryEx<SfcsRuncard>(sQuery, model);
            if (this.runcardTable != null && this.runcardTable.Count() > 0)
            {
                this.carton = data;
                return;
            }

            sQuery = new QueryHelp().InquireSQLHelper<SfcsRuncard>(new SfcsRuncard() { PALLET_NO = data }, "SFCS_RUNCARD");
            this.runcardTable = repositorySL.QueryEx<SfcsRuncard>(sQuery, model);
            if (this.runcardTable != null && this.runcardTable.Count() > 0)
            {
                this.pallet = data;
                return;
            }


            this.runcardTable = repositorySL.QueryEx<SfcsRuncard>("SELECT R.* FROM SFCS_RUNCARD R, SFCS_WO W WHERE R.WO_ID = W.ID AND W.WO_NO = '" + data + "'");
            if (this.runcardTable != null && this.runcardTable.Count() > 0)
            {
                this.workOrder = data;
                return;
            }

            if (this.actionSelectIndex == (int)HoldOperation.HoldShip)
            {
                //不鎖定已出貨的SN
                this.runcardTable = GetRuncardTableByPNAndRuncardStatus(false, true, new GetRuncardByPNAndRuncardStatusRequestModel() { PALLET_NO = data, NOT_IN_STATUS = GlobalVariables.Shipped }, repositorySL);
                if (this.runcardTable != null && this.runcardTable.Count() > 0)
                {
                    this.partNumber = data;
                    return;
                }
            }
            else if (this.actionSelectIndex == (int)HoldOperation.HoldTurnIn)
            {
                //不鎖定已存倉和已出貨的SN
                this.runcardTable = GetRuncardTableByPNAndRuncardStatus(true, true, new GetRuncardByPNAndRuncardStatusRequestModel() { PART_NO = data }, repositorySL);
                if (this.runcardTable != null && this.runcardTable.Count() > 0)
                {
                    this.partNumber = data;
                    return;
                }
            }
            else if (this.actionSelectIndex == (int)HoldOperation.HoldWIP)
            {
                //不鎖定已存倉和已出貨的SN
                this.runcardTable = GetRuncardTableByPNAndRuncardStatus(true, true, new GetRuncardByPNAndRuncardStatusRequestModel() { PART_NO = data }, repositorySL);
                if (this.runcardTable != null && this.runcardTable.Count() > 0)
                {
                    this.partNumber = data;
                    return;
                }
            }
            else
            {
                this.runcardTable = GetRuncardTableByProduct(new GetRuncardByPNAndRuncardStatusRequestModel() { PART_NO = data }, repositorySL);
                if (this.runcardTable != null && this.runcardTable.Count() > 0)
                {
                    this.partNumber = data;
                    return;
                }
            }

            this.runcardTable = GetRuncardTableByProduct(new GetRuncardByPNAndRuncardStatusRequestModel() { MODEL = data }, repositorySL);
            if (this.runcardTable != null && this.runcardTable.Count() > 0)
            {
                this.model = data;
                return;
            }

            if (this.actionSelectIndex != (int)HoldOperation.HoldAssemply)
            {
                this.runcardTable = GetRuncardDataTableByComponent(new GetRuncardByPNAndRuncardStatusRequestModel() { CUSTOMER_COMPONENT_PN = data }, repositorySL);
                if (this.runcardTable != null && this.runcardTable.Count() > 0)
                {
                    this.compPartNumber = data;
                    return;
                }
            }
        }

        /// <summary>
        /// BYPN搜索SN時是否排除已存倉或者已出貨SN
        /// </summary>
        /// <param name="withoutTurnIn"></param>
        /// <param name="withoutShipping"></param>
        /// <param name="model"></param>
        /// <param name="repositorySL"></param>
        /// <returns></returns>
        public List<SfcsRuncard> GetRuncardTableByPNAndRuncardStatus(bool withoutTurnIn, bool withoutShipping, GetRuncardByPNAndRuncardStatusRequestModel model, IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            string sQuery = new QueryHelp().InquireSQLHelper<GetRuncardByPNAndRuncardStatusRequestModel>(model, " SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM ", " DISTINCT SR.* ", " SR.WO_ID = SW.ID AND SM.ID = SW.MODEL_ID ");

            if (withoutTurnIn)
            {
                sQuery += " AND SR.STATUS != 4 ";
            }
            if (withoutShipping)
            {
                sQuery += " AND SR.STATUS != 5 ";
            }

            return repositorySL.QueryEx<SfcsRuncard>(sQuery, model);
        }

        /// <summary>
        /// 通過工單、料號獲取Runcard
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<SfcsRuncard> GetRuncardTableByProduct(GetRuncardByPNAndRuncardStatusRequestModel model, IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            string sQuery = new QueryHelp().InquireSQLHelper<GetRuncardByPNAndRuncardStatusRequestModel>(model, " SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM ", " DISTINCT SR.* ", " SR.WO_ID = SW.ID AND SM.ID = SW.MODEL_ID ");

            return repositorySL.QueryEx<SfcsRuncard>(sQuery, model);
        }

        /// <summary>
        /// 根據零件序號獲取流水號信息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<SfcsRuncard> GetRuncardDataTableByComponent(GetRuncardByPNAndRuncardStatusRequestModel model, IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            string sQuery = new QueryHelp().InquireSQLHelper<GetRuncardByPNAndRuncardStatusRequestModel>(model, " SFCS_RUNCARD SR, SFCS_COLLECT_COMPONENTS SCO ", " SR.* ", " SCO.SN_ID = SR.ID ");

            return repositorySL.QueryEx<SfcsRuncard>(sQuery, model);
        }

        #region HoldProductManager

        /// <summary>
        /// 獲取hold單據
        /// </summary>
        /// <returns></returns>
        public string GetHoldBill(IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            return repositorySL.QueryEx<string>("SELECT 'HB'||LPAD(SFCS_HOLD_HEADER_SEQ.NEXTVAL,15,'0') BATCH FROM DUAL ").FirstOrDefault();
        }

        /// <summary>
        /// 獲取hold單據主鍵
        /// </summary>
        /// <returns></returns>
        public decimal GetHoldID(IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            return repositorySL.QueryEx<decimal>("SELECT SFCS_HOLD_HEADER_ID_SEQ.NEXTVAL ID FROM DUAL").FirstOrDefault();
        }

        /// <summary>
        /// 插入hold單據頭
        /// </summary>
        public void InsertHoldProductHeader(string holdBill, decimal id,
            decimal mainCondition, string mainValue, decimal subsidiaryCondition,
            string subsidiaryValue, decimal holdAction,
            string status, decimal holdQty, string holdCause, string holdEmpno, string ecnNo, IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            string I_InsertHoldProductHeader = @"INSERT INTO SFCS_HOLD_PRODUCT_HEADER(ID,HOLD_NUMBER,MAIN_CONDITION,MAIN_VALUE,
                                                          SUBSIDIARY_CONDITION,SUBSIDIARY_VALUE,HOLD_ACTION,STATUS,HOLD_QTY,HOLD_CAUSE,
                                                          HOLD_EMPNO,ECN_NO) VALUES(:ID,:HOLD_NUMBER,:MAIN_CONDITION,:MAIN_VALUE,
                                                          :SUBSIDIARY_CONDITION,:SUBSIDIARY_VALUE,:HOLD_ACTION,:STATUS,:HOLD_QTY,:HOLD_CAUSE,:HOLD_EMPNO,:ECN_NO) ";
            DynamicParameters p = new DynamicParameters();
            p.Add("ID", id, System.Data.DbType.String);
            p.Add("HOLD_NUMBER", holdBill, System.Data.DbType.String);
            p.Add("MAIN_CONDITION", mainCondition, System.Data.DbType.String);
            p.Add("MAIN_VALUE", mainValue, System.Data.DbType.String);
            p.Add("SUBSIDIARY_CONDITION", subsidiaryCondition, System.Data.DbType.String);
            p.Add("SUBSIDIARY_VALUE", subsidiaryValue, System.Data.DbType.String);
            p.Add("HOLD_ACTION", holdAction, System.Data.DbType.String);
            p.Add("STATUS", status, System.Data.DbType.String);
            p.Add("HOLD_QTY", holdQty, System.Data.DbType.String);
            p.Add("HOLD_CAUSE", holdCause, System.Data.DbType.String);
            p.Add("HOLD_EMPNO", holdEmpno, System.Data.DbType.String);
            p.Add("ECN_NO", ecnNo, System.Data.DbType.String);
            repositorySL.Execute(I_InsertHoldProductHeader, p);
        }

        /// <summary>
        /// 插入hold單據詳細
        /// </summary>
        public static void InsertHoldProductDetail(decimal holdID, decimal snID,
            string componentSN, string customerComponentPN, decimal holdAction, string status,
            string partNumber, string WO_NO, decimal operationLineID, decimal operationSiteID,
            decimal productOperationCode, decimal currentOperationID, string model, IBaseRepository<SfcsRuncard, decimal> repositorySL)
        {
            string I_InsertHoldProductDetail = @"INSERT INTO SFCS_HOLD_PRODUCT_DETAIL(ID,HOLD_ID,SN_ID,COMPONENT_SN,
                                                          CUSTOMER_COMPONENT_PN,HOLD_ACTION,STATUS,PART_NO,WO_NO,OPERATION_LINE_ID,
                                                          OPERATION_SITE_ID,PRODUCT_OPERATION_CODE,CURRENT_OPERATION_ID,MODEL) 
                                                          VALUES (SFCS_HOLD_DETAIL_ID_SEQ.NEXTVAL,
                                                          :HOLD_ID,:SN_ID,:COMPONENT_SN,:CUSTOMER_COMPONENT_PN,:HOLD_ACTION,:STATUS,
                                                          :PART_NO,:WO_NO,:OPERATION_LINE_ID,:OPERATION_SITE_ID,:PRODUCT_OPERATION_CODE,
                                                          :CURRENT_OPERATION_ID,:MODEL) ";
            DynamicParameters p = new DynamicParameters();
            p.Add("HOLD_ID", holdID, System.Data.DbType.String);
            p.Add("SN_ID", snID, System.Data.DbType.String);
            p.Add("COMPONENT_SN", componentSN, System.Data.DbType.String);
            p.Add("CUSTOMER_COMPONENT_PN", customerComponentPN, System.Data.DbType.String);
            p.Add("HOLD_ACTION", holdAction, System.Data.DbType.String);
            p.Add("STATUS", status, System.Data.DbType.String);
            p.Add(GlobalVariables.PART_NO, partNumber, System.Data.DbType.String);
            p.Add("WO_NO", WO_NO, System.Data.DbType.String);
            p.Add("OPERATION_LINE_ID", operationLineID, System.Data.DbType.String);
            p.Add("OPERATION_SITE_ID", operationSiteID, System.Data.DbType.String);
            p.Add(GlobalVariables.PRODUCT_OPERATION_CODE, productOperationCode, System.Data.DbType.String);
            p.Add("CURRENT_OPERATION_ID", currentOperationID, System.Data.DbType.String);
            p.Add(GlobalVariables.MODEL, model, System.Data.DbType.String);
            repositorySL.Execute(I_InsertHoldProductDetail, p);
        }

        #endregion
    }

    /// <summary>
    /// 主條件
    /// </summary>
    public enum HoldProductMainCondition
    {
        HoldBySerialNumber,//單筆/批量產品序號
        HoldByCustomSerialNumber,//料號與自定義產品序號
        HoldByCartonOrPallet,//卡通/棧板
        HoldByWorkOrderOrPartNumberOrModel,//工單/料號/機種
        HoldByComponentCustomerPartNumber,//零件客戶料號
        HoldByComponentSerialNumber,//單筆/批量零件序號
        HoldByCustomComponentSerialNumber,//料號與自定義零件序號
        HoldSiteByPartNumberOrWorkOrder,//料號/工單與站點
        HoldSite,//站點
        HoldWipOperationBySerialNumber,//產品序號與工序
        HoldWIPOperationByPartNumberAndOperationLine = 21,
        HoldWIPOperationByPartNumber = 22
    }

    /// <summary>
    /// 管控動作
    /// </summary>
    public enum HoldOperation
    {
        HoldWIP,
        HoldRework,
        HoldRMA,
        HoldAssemply,
        HoldShip,
        HoldTurnIn,
        HoldRepair
    }

    public class QueryHelp
    {

        #region GetSQLByModelHelper

        public String InquireSQLHelper<T>(T model, String ViewName, String FieldName = "*", String sWhere = "")
        {
            sWhere = String.IsNullOrEmpty(sWhere) ? " 1=1 " : sWhere;
            PropertyInfo[] propertys = model.GetType().GetProperties();

            foreach (PropertyInfo pinfo in propertys)
            {
                if (pinfo.PropertyType.FullName == typeof(String).FullName)
                {
                    var data = pinfo.GetValue(model, null);
                    if (data != null)
                    {
                        var data2 = data.ToString();
                        if (!String.IsNullOrEmpty(data2))
                        {
                            sWhere += $"AND {pinfo.Name} = :{pinfo.Name} ";
                        }
                    }
                }
                else if (pinfo.PropertyType.FullName == typeof(Decimal).FullName || pinfo.PropertyType.FullName == typeof(Decimal?).FullName)
                {
                    var data = pinfo.GetValue(model, null);
                    if (data != null)
                    {
                        var data2 = (Decimal)data;
                        if (data2 > GlobalVariables.DecimalDefaults)
                        {
                            sWhere += $"AND {pinfo.Name} = :{pinfo.Name} ";
                        }
                    }
                }
                else if (pinfo.PropertyType.FullName == typeof(DateTime).FullName || pinfo.PropertyType.FullName == typeof(DateTime?).FullName)
                {
                    var data = pinfo.GetValue(model, null);
                    if (data != null)
                    {
                        var data2 = (DateTime)data;
                        if (data2.Year != 1)
                        {
                            sWhere += $"AND {pinfo.Name} = :{pinfo.Name} ";
                        }
                    }
                }
            }

            return String.Format("SELECT {0} FROM {1} WHERE {2} ", FieldName, ViewName, sWhere);
        }

        #endregion

    }

}
