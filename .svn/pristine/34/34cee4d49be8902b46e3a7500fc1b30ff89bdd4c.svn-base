using JZ.IMS.IRepository;
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using JZ.IMS.Models;
using System;
using System.Threading.Tasks;


namespace JZ.IMS.WebApi.MSD
{
    public class MSDEndService
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
        public MSDEndService(ISmtMsdRuncardRepository repository, IStringLocalizer<SmtMsdRuncardController> localizer, string username)
        {
            _repository = repository;
            _localizer = localizer;
            UserName = username;
            this.msdData = new MSDData();

        }

        #endregion

        #region method

        /// <summary>
        /// 執行動作前檢查
        /// </summary>
        /// <returns></returns>
        public bool TakeActionCheck()
        {
            // 只有Msg_MSD_Line和加工房可以END
            //if ((this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Line) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Process_Room))
            //{
            //    throw new Exception(Properties.Resources.Msg_MSD_Action_Area_Not_Match);
            //}

            // Error For Bake to End
            // Error For Dry to End
            // Error For Vacuum to End
            if ((this.msdData.CurrentAction == (decimal)MSDAction.Bake) ||
                (this.msdData.CurrentAction == (decimal)MSDAction.Dry) ||
                (this.msdData.CurrentAction == (decimal)MSDAction.Vacuum))
            {
                //料卷{0}当前都工作是{1}，泵进行用完操作。
                throw new Exception(string.Format(_localizer["Err_Wrong_MSD_Route"],
                    this.msdData.ReelCode, this.msdData.CurrentActionName));
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

            //if (ClientPublic.ShowQuestionBox("料卷已用完？") == System.Windows.Forms.DialogResult.Yes)
            {
                // 結束前一次操作
                //MSDDataManager.LogFinishActionHistory(this.msdData.ReelCode, GlobalManager.CurrentUserName, this.msdData.CurrentAction);
               await _repository.LogFinishActionHistory(this.msdData.ReelCode, this.msdData.OperateBy, this.msdData.CurrentAction);
                // 結束MSD Runcard
                //MSDDataManager.MSDActionEnd(this.msdData.ReelCode, GlobalManager.CurrentUserName, this.msdData.ActionAreaType);
               await _repository.MSDActionEnd(this.msdData.ReelCode, this.msdData.OperateBy, this.msdData.ActionAreaType);
                // 記錄操作歷史
               await _repository.LogActionEndHistory(this.msdData.ReelCode, (decimal)MSDStandard.NormalTemperature,
                    (decimal)MSDStandard.NormalHumidity);
            }
        }
        #endregion
    }
}
