using JZ.IMS.IRepository;
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using JZ.IMS.Models;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.MSD
{
    public class MSDVacuumService
    {
    
        #region fields
        private readonly ISmtMsdRuncardRepository _repository;
        private readonly IStringLocalizer<SmtMsdRuncardController> _localizer;

        private MSDData msdData;
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
        public MSDVacuumService(ISmtMsdRuncardRepository repository, IStringLocalizer<SmtMsdRuncardController> localizer, string username)
        {
            _repository = repository;
            _localizer = localizer;
            UserName = username;
            this.msdData = new MSDData();
          
        }

        #endregion

        /// <summary>
        /// 執行動作前檢查
        /// </summary>
        /// <returns></returns>
        public bool TakeActionCheck()
        {
            // 只有MSD_Dry_Box可以抽真空包
            //if ((this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Material_Area) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Returnhouse) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Warehouse) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Vacuum_Package) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Process_Room) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Line) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_IQC)
            //    )
            //{
            //    return false;
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
                //Err_MSDReelNeedToBake 料卷{0}的暴露时间已经超过有效期，请进行烘烤。
                throw new Exception(string.Format(_localizer["Err_MSDReelNeedToBake"], this.msdData.ReelCode));
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
            //    this.msdData.ReelCode, (decimal)MSDAction.Vacuum, this.msdData.Temperature, 
            //    this.msdData.Humidity, GlobalManager.CurrentUserName, this.msdData.LevelCode, 
            //    this.msdData.Thickness, this.msdData.ActionAreaType);
           await _repository.CreateMSDRuncard(this.msdData.MSDRuncardRow!=null,
                this.msdData.ReelCode, (decimal)MSDAction.Vacuum, this.msdData.Temperature,
                this.msdData.Humidity, this.msdData.OperateBy, this.msdData.LevelCode,
                this.msdData.Thickness, this.msdData.ActionAreaType);

            // 記錄操作歷史
            decimal msdOperationHistoryID = await _repository.GetMSDHistorySEQID();
          await  _repository.InsertMSDOperationHistory(msdOperationHistoryID, this.msdData.ReelCode);
        }



        private async Task InitalizeForFinishAction()
        {
            TimeSpan timeSpan =  _repository.GetSystemTime() - this.msdData.MSDRuncardRow.BEGIN_TIME;
            this.actionTime = timeSpan.TotalHours;
        }


        /// <summary>
        /// 結束動作前檢查
        /// </summary>
        /// <returns></returns>
        public bool FinishActionCheck()
        {
            return true;
        }

        /// <summary>
        /// 結束動作
        /// </summary>
        /// <param name="inputData"></param>
        public void FinishAction(MSDData inputData)
        {
            this.msdData = inputData;

            this.InitalizeForFinishAction();

            if (!this.FinishActionCheck())
            {
                return;
            }

            // 延期時間
            _repository.DelayFloorLifeEndTime(this.msdData.ReelCode, this.actionTime);

            // 延期Fuji Trax的MSD開封時間
            //FujiTraxManager.DelayFujitraxReelMSDOpenTime(this.msdData.ReelCode, this.actionTime);

            // 記錄操作歷史
            _repository.LogFinishActionHistory(this.msdData.ReelCode, UserName, this.msdData.CurrentAction);

            // 將MSD Runcard改為Open狀態，修改溫度濕度
            //MSDDataManager.CreateMSDRuncard(!this.msdData.MSDRuncardRow.IsNullOrEmpty(),
            //    this.msdData.ReelCode, (decimal)MSDAction.Open, (decimal)MSDStandard.NormalTemperature,
            //    (decimal)MSDStandard.NormalHumidity, GlobalManager.CurrentUserName, this.msdData.LevelCode,
            //    this.msdData.Thickness, this.msdData.ActionArea);
        }

    }
}
