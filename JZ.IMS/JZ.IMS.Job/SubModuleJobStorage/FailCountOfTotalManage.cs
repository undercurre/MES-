using System;
using System.Text;
using JZ.IMS.Core;
using System.Data;
using JZ.IMS.Models;
using JZ.IMS.Core.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.ViewModels;

namespace JZ.IMS.Job.SubModuleJobStorage
{
    public class FailCountOfTotalManage : StopLine, IMesSubModuleJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    this.propertyproviderSL = propertyprovider;
                    this.repositorySL = repository;
                    this.transactionSL = transaction;

                    this.StopLineMode = GlobalVariables.FailCountInBaseCountControl;
                    StopLineProcessor();

                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {
                    return new KeyValuePair<bool, string>(false, "FailCountOfTotalManage:" + ex.Message);
                }
            });
        }

        #region Custom Method

        /// <summary>
        /// 停線管控處理
        /// </summary>
        /// <returns></returns>
        private int StopLineProcessor()
        {
            // 初始化產品停線配置
            this.InitializeProductStopLine();

            try
            {
                if (this.ProductStopLineList != null && this.ProductStopLineList.Count > 0)
                {
                    // 統一處理本批次的StopLine數據
                    this.ProcessStopLineBatch();

                    // 數量達到切入點時，開始判斷是否超標
                    if (this.TotalCount >= this.ProductStopLineModel.DIVISION_START)
                    {
                        // 判斷是否達到Stop Criteria
                        if (this.FailCount >= this.ProductStopLineModel.STOP_CRITERIA)
                        {
                            //this.IssueMessage = string.Format(Err_StopLine_Over_Criteria, this.propertyproviderSL.product.partNumber,
                            //    string.Format(Msg_In_Base_Count, this.ProductStopLineModel.DIVISION_CRITERIA.ToString()),
                            //    string.Format(Msg_Fail_Count, this.FailCount.ToString()),
                            //    Msg_Stop, this.ProductStopLineModel.STOP_CRITERIA.ToString());

                            // Hold Site
                            this.HoldOperationSite(this.repositorySL);

                            // Log Stop Line History
                            this.LogStopLineHistory(GlobalVariables.StopLineStop);

                            // 超標時，直接Close Batch Header
                            this.UpdateBatchDataStatus(GlobalVariables.RecordClosed);

                            return GlobalVariables.ProcessContinued;
                        }

                        // 判斷是否達到Alarm Criteria
                        if (this.FailCount >= this.ProductStopLineModel.ALARM_CRITERIA)
                        {
                            if (this.StopLineBatchHeaderModel.ALARM_CYCLE_COUNT + 1 >= this.ProductStopLineModel.ALARM_INTERVAL)
                            {
                                //this.IssueMessage = string.Format(Err_StopLine_Over_Criteria,
                                //    this.propertyproviderSL.product.partNumber,
                                //    string.Format(Msg_In_Base_Count, this.ProductStopLineModel.DIVISION_CRITERIA.ToString()),
                                //    string.Format(Msg_Fail_Count, this.FailCount.ToString()), Msg_Alarm,
                                //    this.ProductStopLineModel.ALARM_CRITERIA.ToString());

                                // Log Stop Line History
                                this.LogStopLineHistory(GlobalVariables.StopLineAlarm);

                                // 重新歸零Batch Header的循環報警次數
                                this.UpdateStopLineHeaderAlarmCycleCount(0);
                            }
                            else
                            {
                                this.UpdateStopLineHeaderAlarmCycleCount(Convert.ToInt32(this.StopLineBatchHeaderModel.ALARM_CYCLE_COUNT) + 1);
                            }
                        }
                    }

                    // 更新Batch Header 
                    decimal batchStatus = 0;

                    //udpate active狀態時再一次與數據庫資料比對，以免髒數據更新導致問題再次發生
                    this.RefreshCount();
                    if (this.TotalCount >= this.ProductStopLineModel.DIVISION_CRITERIA)
                    {
                        batchStatus = GlobalVariables.RecordClosed;
                    }
                    else
                    {
                        batchStatus = GlobalVariables.RecordActive;
                    }
                    this.UpdateBatchDataStatus(batchStatus);

                }
                return GlobalVariables.ProcessContinued;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region MyRegion
        public String Err_StopLine_Over_Criteria = "{0}在{1}内{2}达到停线{3}标准({4})，请注意。";
        public String Msg_In_Base_Time = "规定时间({0}小时)";
        public String Msg_Fail_Count = "不良数量({0})";
        public String Msg_Stop = "中止";
        public String Msg_Alarm = "警告";
        public String Msg_In_Base_Count = "规定数量({0}套)";
        #endregion

    }
}
