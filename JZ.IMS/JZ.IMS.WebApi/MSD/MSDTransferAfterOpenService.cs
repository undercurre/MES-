using JZ.IMS.IRepository;
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using JZ.IMS.Models;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.MSD
{
    public class MSDTransferAfterOpenService
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
        public MSDTransferAfterOpenService(ISmtMsdRuncardRepository repository, IStringLocalizer<SmtMsdRuncardController> localizer, string username)
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
            if (this.msdData.CurrentAction != (decimal)MSDAction.Open)
            {
                //只有开封状态下可以修改作业区域。
                throw new Exception(_localizer["Modified_In_Unsealed"]);
            }

            if (this.msdData.MSDRuncardRow.AREA == this.msdData.ActionArea)
            {
                //当前区域与所选区域相同，请确认。
                throw new Exception(_localizer["Current_Is_Same"]);
            }

            return true;
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
        /// 執行動作
        /// </summary>
        /// <param name="inputData"></param>
        public async Task TakeAction(MSDData inputData)
        {
            try
            {
                this.msdData = inputData;
                if (!this.TakeActionCheck())
                {
                    return;
                }

                //MSDDataManager.UpdateMSDArea(this.msdData.ReelCode, this.msdData.ActionAreaType, GlobalManager.CurrentUserName);
              await  _repository.UpdateMSDArea(this.msdData.ReelCode, this.msdData.ActionAreaType, this.msdData.OperateBy);
               await _repository.LogTransferAfterOpenHistory(this.msdData.ReelCode, (decimal)MSDAction.TransferAfterOpen);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion
    }
}
