using JZ.IMS.IRepository;
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using JZ.IMS.Models;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.MSD
{
    public class MSDBakeService
    {

        #region fields
        private readonly ISmtMsdRuncardRepository _repository;
        private readonly IStringLocalizer<SmtMsdRuncardController> _localizer;

        private MSDData msdData;

        //private decimal ruleBakeTime;

        private DateTime bakeStanadardEndTime;

        #endregion

        #region properties
        public string UserName { get; set; }
        #endregion

        public MSDBakeService(ISmtMsdRuncardRepository repository, IStringLocalizer<SmtMsdRuncardController> localizer, string username)
        {
            this.msdData = new MSDData();
            _repository = repository;
            _localizer = localizer;
            UserName = username;
            //this.ruleBakeTime = 0;
        }

        /// <summary>
        /// 執行動作前檢查
        /// </summary>
        /// <returns></returns>
        public bool TakeActionCheck()
        {
            // 只有MSD_Bake_Box可以烘烤
            //if (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Bake_Box)
            //{
            //    throw new Exception(Properties.Resources.Msg_MSD_Action_Area_Not_Match);
            //}

            //if (this.msdData.CurrentAction == (decimal)MSDAction.Vacuum)
            //{
            //    throw new SysException("該料卷{0}正在真空包中，不能進行烘烤", this.msdData.ReelCode);
            //}

            //if ((this.msdData.CurrentAction != (decimal)MSDAction.Open) &&
            //    (this.msdData.CurrentAction != 0))
            //{
            //    throw new SysException(Properties.Resources.Err_MSD_Prior_Action_Not_Finish,
            //        this.msdData.ReelCode, this.msdData.CurrentActionName);
            //}

            // Error For Dry to Bake
            if (this.msdData.CurrentAction == (decimal)MSDAction.Dry)
            {
                //料卷{0}当前都工作是{1}，泵进行烘烤操作。
                throw new Exception(string.Format(_localizer["Material_Currently_Working"],
                    this.msdData.ReelCode, this.msdData.CurrentActionName));
            }

            if (this.msdData.StandardBakeTime == 0)
            {
                //请选择正确的烘烤标准。
                throw new Exception(_localizer["Select_Correct_Baking"]);
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
            //    this.msdData.ReelCode, (decimal)MSDAction.Bake, this.msdData.Temperature, 
            //    this.msdData.Humidity, GlobalManager.CurrentUserName, this.msdData.LevelCode, 
            //    this.msdData.Thickness, this.msdData.ActionAreaType);

           await _repository.CreateMSDRuncard(this.msdData.MSDRuncardRow!=null,
                this.msdData.ReelCode, (decimal)MSDAction.Bake, this.msdData.Temperature,
                this.msdData.Humidity, this.msdData.OperateBy, this.msdData.LevelCode,
                this.msdData.Thickness, this.msdData.ActionAreaType);

            // 記錄操作歷史
            decimal msdOperationHistoryID =await _repository.GetMSDHistorySEQID();
          await  _repository.InsertMSDOperationHistory(msdOperationHistoryID, this.msdData.ReelCode);
            _repository.AddStandardEndTime(msdOperationHistoryID, this.msdData.StandardBakeTime);
        }


        public async Task InitalizeForFinishAction()
        {
            string cmd = @"  SELECT * FROM SMT_MSD_OPERATION_HISTORY WHERE REEL_ID=:REEL_ID AND ACTION_CODE=:ACTION_CODE";
          var historyRow=(await _repository.QueryAsyncEx<SmtMsdOperationHistory>(cmd,new { REEL_ID= this.msdData.ReelCode ,ACTION_CODE = this.msdData.CurrentAction }))?.OrderByDescending(f => f.ID).FirstOrDefault();

            this.bakeStanadardEndTime = historyRow.STANDARD_END_TIME;

        }

        /// <summary>
        /// 結束動作前檢查
        /// </summary>
        /// <returns></returns>
        public bool FinishActionCheck()
        {
            if (DateTime.Compare(_repository.GetSystemTime(), this.bakeStanadardEndTime) < 0)
            {
                //烘烤时间还没达到额定的最少时间，请确认。
                throw new Exception(_localizer["BakingTime_No_Yet"]);
            }

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

            // 記錄操作歷史
            _repository.LogFinishActionHistory(this.msdData.ReelCode,UserName, this.msdData.CurrentAction);
            // 清除開封時間統計，重新計算
            _repository.ClearOpenTime(this.msdData.ReelCode);
        }
    }
}
