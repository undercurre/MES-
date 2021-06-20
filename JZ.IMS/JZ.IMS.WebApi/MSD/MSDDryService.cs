using JZ.IMS.IRepository;
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.MSD
{
    public class MSDDryService
    {
        #region fields
        private readonly ISmtMsdRuncardRepository _repository;
        private readonly IStringLocalizer<SmtMsdRuncardController> _localizer;

        private MSDData msdData;
        private bool clearOpenTime;
        private double actionTime;
        private string userName;
        #endregion

        #region properties
        public string UserName { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// 構造函數
        /// </summary>
        public MSDDryService(ISmtMsdRuncardRepository repository, IStringLocalizer<SmtMsdRuncardController> localizer, string username)
        {
            _repository = repository;
            _localizer = localizer;
            UserName = username;
            this.msdData = new MSDData();
            this.clearOpenTime = false;
            this.actionTime = 0;

        }
        #endregion

        private async Task InitializeForFinishAction()
        {
            TimeSpan timeSpan =  _repository.GetSystemTime() - this.msdData.MSDRuncardRow.BEGIN_TIME;
            this.actionTime = timeSpan.TotalHours;
        }

        /// <summary>
        /// 執行動作前檢查
        /// </summary>
        /// <returns></returns>
        public bool TakeActionCheck()
        {
            // 只有MSD_Dry_Box可以乾燥
            //if (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Dry_Box)
            //{
            //    throw new Exception(Properties.Resources.Msg_MSD_Action_Area_Not_Match);
            //}
            //if ((this.msdData.CurrentAction != (decimal)MSDAction.Open) &&
            //    (this.msdData.CurrentAction != 0))
            //{
            //    throw new SysException(Properties.Resources.Err_MSD_Prior_Action_Not_Finish,
            //        this.msdData.ReelCode, this.msdData.CurrentActionName);
            //}

            // 檢查當前的開封時間是否已超期
            if (this.msdData.FloorLife - this.msdData.TotalOpenTime <= 0)
            {
                //料卷{0}的暴露时间已经超过有效期，请进行烘烤。
                throw new Exception(string.Format(_localizer["MSD_Need_Bake"], this.msdData.ReelCode));
            }

            return true;
        }

        /// <summary>
        /// 執行動作
        /// </summary>
        /// <param name="inputData"></param>
        public async Task TakeAction(MSDData inputData)
        {
            this.msdData = inputData;
            if (!this.TakeActionCheck())
            {
                return;
            }

            //MSDDataManager.CreateMSDRuncard(!this.msdData.MSDRuncardRow.IsNullOrEmpty(), 
            //    this.msdData.ReelCode, (decimal)MSDAction.Dry, this.msdData.Temperature, 
            //    this.msdData.Humidity, GlobalManager.CurrentUserName, this.msdData.LevelCode, 
            //    this.msdData.Thickness, this.msdData.ActionAreaType);
            _repository.CreateMSDRuncard(this.msdData.MSDRuncardRow!=null,
                this.msdData.ReelCode, (decimal)MSDAction.Dry, this.msdData.Temperature,
                this.msdData.Humidity, this.msdData.OperateBy, this.msdData.LevelCode,
                this.msdData.Thickness, this.msdData.ActionAreaType);

            // 記錄操作歷史
            decimal msdOperationHistoryID =await _repository.GetMSDHistorySEQID();
            _repository.InsertMSDOperationHistory(msdOperationHistoryID, this.msdData.ReelCode);
        }

        /// <summary>
        /// 結束動作前檢查
        /// </summary>
        /// <returns></returns>
        public bool FinishActionCheck()
        {
            this.clearOpenTime = false;

            // 乾燥后均不清除開封時間
            //double actualDryTime = this.actionTime * 60 * 60;
            //double totalOpenTime = this.msdData.MSDRuncardRow.IsTOTAL_OPEN_TIMENull() ? 0 :
            //    (double)this.msdData.MSDRuncardRow.TOTAL_OPEN_TIME;
            //totalOpenTime = totalOpenTime * 60 * 60;

            //MSDDataSet.SMT_MSD_DRY_RULERow dryRuleRow =
            //    MSDDataManager.GetMSDDryRuleByRange(this.msdData.LevelCode, (decimal)totalOpenTime, (decimal)actualDryTime).FirstOrDefault();
            //if (!dryRuleRow.IsNullOrEmpty())
            //{
            //    this.clearOpenTime = dryRuleRow.CLEAR_OPEN_TIME == GlobalVariables.EnableY ? true : false;
            //}
            //else
            //{
            //    //if (ClientPublic.ShowQuestionBox("沒有達到乾燥標準，是否決定拿出來使用？") == System.Windows.Forms.DialogResult.No)
            //    //{
            //    //    return false;
            //    //}
            //}

            return true;
        }

        /// <summary>
        /// 結束動作
        /// </summary>
        /// <param name="inputData"></param>
        public void FinishAction(MSDData inputData)
        {
            this.msdData = inputData;
            this.InitializeForFinishAction();
            if (!this.FinishActionCheck())
            {
                return;
            }

            if (this.clearOpenTime)
            {
                // 清除開封時間統計，重新計算
                _repository.ClearOpenTime(this.msdData.ReelCode);
            }
            else
            {
                // 延期時間
                _repository.DelayFloorLifeEndTime(this.msdData.ReelCode, this.actionTime);

                // 延期Fuji Trax的MSD開封時間
                //FujiTraxManager.DelayFujitraxReelMSDOpenTime(this.msdData.ReelCode, this.actionTime);
            }

            // 記錄操作歷史
            _repository.LogFinishActionHistory(this.msdData.ReelCode, UserName, this.msdData.CurrentAction);
        }

    }
}
