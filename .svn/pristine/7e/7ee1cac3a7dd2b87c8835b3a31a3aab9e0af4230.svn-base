using JZ.IMS.IRepository;
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Common
{
    

    public class MSDService
    {

        private readonly ISmtMsdRuncardRepository _repository;
        private readonly IStringLocalizer<SmtMsdRuncardController> _localizer;

        #region fields

        private MSDData msdData;

        private string parentReelCode;
        private DateTime childReelBeginTime;
        private DateTime childReelFloorLifeEndTime;
        private double parentReelTotalOpenTime;
        private bool needToSyncFloorLife;

        #endregion

        #region properties

        #endregion

        #region constructor


        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="localizer"></param>
        public MSDService(ISmtMsdRuncardRepository repository, IStringLocalizer<SmtMsdRuncardController> localizer)
        {
            _repository = repository;
            _localizer = localizer;
            this.msdData = new MSDData();
            this.needToSyncFloorLife = false;
            this.parentReelTotalOpenTime = 0;

        }

        #endregion

        #region IMSDAction

        public void Clear()
        {

        }

        /// <summary>
        /// 執行動作前檢查
        /// </summary>
        /// <returns></returns>
        public bool TakeActionCheck()
        {
            // 只有MSD_Line,MSD_Material_Area,MSD_Warehouse,MSD_Test_Area,Msg_MSD_Process_Room可以開封
            //if ((this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Line) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Material_Area) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Warehouse) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Test_Area) &&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_Process_Room)&&
            //    (this.msdData.ActionAreaType != Properties.Resources.Msg_MSD_IQC)
            //    )
            //{
            //    throw new Exception(Properties.Resources.Msg_MSD_Action_Area_Not_Match);
            //}

            //if ((this.msdData.CurrentAction != 0) &&
            //    (this.msdData.CurrentAction == (decimal)MSDAction.Bake) &&
            //    (this.msdData.NewAction == (decimal)MSDAction.Open))
            //{
            //    throw new SysException("該料卷狀態為烘烤中，不允許開封");
            //}

            //if (this.msdData.FloorLife <= 0)
            //{
            //    throw new SysException("沒有找到原件開封後的使用週期標準，請確認");
            //}

            if (this.msdData.FloorLife - this.msdData.TotalOpenTime <= 0)
            {
                throw new Exception(string.Format(_localizer["Err_MSDReelNeedToBake"], this.msdData.ReelCode));
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
            this.msdData = inputData;
            if (!this.TakeActionCheck())
            {
                return;
            }

            //this.InitalizeParentReelInfo();

            // MSD Runcard
            //_repository.CreateMSDRuncard(!this.msdData.MSDRuncardRow.IsNullOrEmpty(), 
            //    this.msdData.ReelCode, (decimal)MSDAction.Open, this.msdData.Temperature, 
            //    this.msdData.Humidity, GlobalManager.CurrentUserName, this.msdData.LevelCode, 
            //    this.msdData.Thickness, this.msdData.ActionAreaType);
            var model = new SmtMsdRuncardAddOrModifyModel()
            {
                REEL_ID = this.msdData.ReelCode,
                CURRENT_ACTION = (decimal)MSDAction.Open,
                TEMPERATURE = this.msdData.Temperature,
                HUMIDITY = this.msdData.Humidity,
                OPERATOR_BY = this.msdData.OperateBy,
                LEVEL_CODE = this.msdData.LevelCode,
                THICKNESS = this.msdData.Thickness,
                AREA = this.msdData.ActionAreaType
            };
            var insertmodel = new SmtMsdRuncardModel();
            insertmodel.InsertRecords.Add(model);
            await _repository.SaveDataByTrans(insertmodel);


            if (this.msdData.TotalOpenTime == 0)
            {
                // 更新MSD Runcard的FLOOR_LIFE_END_TIME
               await _repository.UpdateFloorLifeEndTime(this.msdData.ReelCode, this.msdData.FloorLife);
            }

            if (this.needToSyncFloorLife)
            {
                // 切分料，按父條碼更新FLOOR_LIFE_END_TIME和BEGIN_TIME
               await _repository.UpdateFloorLifeAndBeginTime(
                    this.msdData.ReelCode, this.childReelBeginTime, this.parentReelTotalOpenTime, this.childReelFloorLifeEndTime);
            }

            // 記錄操作歷史
            decimal msdOperationHistoryID = await _repository.GetMSDHistorySEQID();
            _repository.InsertMSDOperationHistory(msdOperationHistoryID, this.msdData.ReelCode);
        }

    

        #endregion



    }
}
