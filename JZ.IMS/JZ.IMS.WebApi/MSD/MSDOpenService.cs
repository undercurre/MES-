using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.MSD
{
    public class MSDOpenService
    {

        #region fields
        private readonly ISmtMsdRuncardRepository _repository;
        private readonly IStringLocalizer<SmtMsdRuncardController> _localizer;

        private MSDData msdData;

        private string parentReelCode;
        private DateTime childReelBeginTime;
        private DateTime childReelFloorLifeEndTime;
        private double parentReelTotalOpenTime;
        private bool needToSyncFloorLife;
        private string userName;
        #endregion

        #region properties
        public string UserName { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// 構造函數
        /// </summary>
        public MSDOpenService(ISmtMsdRuncardRepository repository, IStringLocalizer<SmtMsdRuncardController> localizer,string username)
        {
            _repository = repository;
            _localizer = localizer;
            UserName = username;
            this.msdData = new MSDData();
            this.needToSyncFloorLife = false;
            this.parentReelTotalOpenTime = 0;
        }

        #endregion

        #region methods


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
            if (!this.FinishActionCheck())
            {
                return;
            }

            // 記錄操作歷史
            
            _repository.LogFinishActionHistory(this.msdData.ReelCode,UserName, this.msdData.CurrentAction);
            _repository.UpdateTotalOpenTime(this.msdData.ReelCode);
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
                //料卷{0}的暴露时间已经超过有效期，请进行烘烤。
                throw new Exception(string.Format(_localizer["Err_MSDReelNeedToBake"], this.msdData.ReelCode));
            }

            return true;
        }

        /// <summary>
        /// 初始化父階條碼信息
        /// </summary>
        private async Task InitalizeParentReelInfo()
        {
            this.needToSyncFloorLife = false;

            // 新Reel開封時，如有切料，參考原料卷的開封時間和暴露時長
            if (this.msdData.MSDRuncardRow==null)
            {

                ImsReel parentReelRow =(await _repository.GetParentReelByChild(msdData.ReelCode)).FirstOrDefault();
                if (parentReelRow==null)
                {
                    this.parentReelCode = string.Empty;
                    return;
                }
                this.parentReelCode = parentReelRow.CODE;

                // 切分時間
                DateTime cutTime =await _repository.GetReelCreateTime(this.msdData.ReelCode);

                string S_SelectMSDRuncardDataTable = @"
             SELECT * FROM SMT_MSD_RUNCARD 
             WHERE REEL_ID=:REEL_ID ";
                string S_MSDOperationHistoryDataTable = @"
            SELECT * FROM SMT_MSD_OPERATION_HISTORY WHERE REEL_ID=:REEL_ID AND ACTION_CODE=:ACTION_CODE ";
                string S_SelectMSDOperationHistoryDataTable = @"
            SELECT * FROM SMT_MSD_OPERATION_HISTORY WHERE REEL_ID=:REEL_ID  ";
                SmtMsdRuncard parentMSDRuncardRow =(await _repository.QueryAsyncEx<SmtMsdRuncard>(S_SelectMSDRuncardDataTable, new { REEL_ID = this.parentReelCode })).FirstOrDefault();
                 
                if (parentMSDRuncardRow!=null)
                {

                    SmtMsdOperationHistory parentReelLastBakeRow = (await _repository.QueryAsyncEx<SmtMsdOperationHistory>(S_MSDOperationHistoryDataTable, new
                      {
                          REEL_ID = this.parentReelCode,
                          ACTION_CODE = (decimal)MSDAction.Bake
                      })).Where(f => f.BEGIN_TIME <= cutTime).OrderByDescending(f => f.BEGIN_TIME).FirstOrDefault();

                    SmtMsdOperationHistory parentReelOpenRow = null;
                    if (parentReelLastBakeRow!=null)
                    {
             
                        // 找到父階條碼切分前最近一次的烘烤時間，以此作為節點，找烘烤后第一次開封時間作為子條碼的開封時間
                        parentReelOpenRow =(await _repository.QueryAsyncEx<SmtMsdOperationHistory>(S_MSDOperationHistoryDataTable,new { REEL_ID= this.parentReelCode, ACTION_CODE= (decimal)MSDAction.Open }))
                            .Where(f => f.BEGIN_TIME <= cutTime && f.BEGIN_TIME >= parentReelLastBakeRow.BEGIN_TIME).OrderBy(f => f.BEGIN_TIME).FirstOrDefault();
                    }
                    else
                    {
                        // 如果父階條碼切分前沒有進行烘烤，找切分前最早一筆開封時間作為子條碼的開封時間
                        parentReelOpenRow =
                             parentReelOpenRow = (await _repository.QueryAsyncEx<SmtMsdOperationHistory>(S_MSDOperationHistoryDataTable, new { REEL_ID = this.parentReelCode, ACTION_CODE = (decimal)MSDAction.Open }))
                             .Where(f => f.BEGIN_TIME <= cutTime).OrderBy(f => f.BEGIN_TIME).FirstOrDefault();
                        
                    }

                    if (parentReelOpenRow!=null)
                    {
                        this.childReelBeginTime = parentReelOpenRow.BEGIN_TIME;
                        this.childReelFloorLifeEndTime = parentReelOpenRow.BEGIN_TIME.AddHours((double)this.msdData.FloorLife);

                      List<SmtMsdOperationHistory> parentReelHistoryTable = (await _repository.QueryAsyncEx<SmtMsdOperationHistory>(S_SelectMSDOperationHistoryDataTable, new { REEL_ID = this.parentReelCode }))?.ToList();

                        DateTime actualEndTime = _repository.GetSystemTime();
                        this.childReelBeginTime = actualEndTime;
                        // 以下循環不會包含烘烤記録
                        foreach (SmtMsdOperationHistory row in parentReelHistoryTable.Where(
                            f => (f.BEGIN_TIME >= parentReelOpenRow.BEGIN_TIME && f.BEGIN_TIME <= cutTime)).OrderBy(f => f.BEGIN_TIME))
                        {
                            if (row.ACTUAL_END_TIME==null)
                            {
                                // 當End Time為空時，嘗試獲取下一筆記録的開始時間作為這筆記録的結束時間
                                SmtMsdOperationHistory nextOperationHistoryRow =
                                    parentReelHistoryTable.Where(f => f.BEGIN_TIME > row.BEGIN_TIME).OrderBy(f => f.BEGIN_TIME).FirstOrDefault();
                                if (nextOperationHistoryRow!=null)
                                {
                                    actualEndTime = nextOperationHistoryRow.BEGIN_TIME;
                                }

                                // 否則用當前時間作為結束時間，即Default值
                                actualEndTime = this.childReelBeginTime;
                            }
                            else
                            {
                                actualEndTime = row.ACTUAL_END_TIME;
                            }

                            if (row.ACTION_CODE == (decimal)MSDAction.Dry || row.ACTION_CODE == (decimal)MSDAction.Vacuum)
                            {
                                TimeSpan timeSpan = actualEndTime - row.BEGIN_TIME;
                                double actionTime = timeSpan.TotalHours;
                                this.childReelFloorLifeEndTime = this.childReelFloorLifeEndTime.AddHours(actionTime);
                            }
                            else if (row.ACTION_CODE == (decimal)MSDAction.Open)
                            {
                                TimeSpan timeSpan = actualEndTime - row.BEGIN_TIME;
                                this.parentReelTotalOpenTime += timeSpan.TotalHours;
                            }

                            // 將父階條碼的記録Copy到子條碼下

                           await _repository.CopyHistoryFromParentReel(this.msdData.ReelCode, actualEndTime, row.ID);
                        }

                        this.parentReelTotalOpenTime = Math.Round(this.parentReelTotalOpenTime, 3);
                        this.needToSyncFloorLife = true;
                    }
                }
            }
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

                await this.InitalizeParentReelInfo();

                // MSD Runcard
                //MSDDataManager.CreateMSDRuncard(!this.msdData.MSDRuncardRow.IsNullOrEmpty(), 
                //    this.msdData.ReelCode, (decimal)MSDAction.Open, this.msdData.Temperature, 
                //    this.msdData.Humidity, GlobalManager.CurrentUserName, this.msdData.LevelCode, 
                //    this.msdData.Thickness, this.msdData.ActionAreaType);
                await _repository.CreateMSDRuncard(this.msdData.MSDRuncardRow != null,
                     this.msdData.ReelCode, (decimal)MSDAction.Open, this.msdData.Temperature,
                     this.msdData.Humidity, this.msdData.OperateBy, this.msdData.LevelCode,
                     this.msdData.Thickness, this.msdData.ActionAreaType);

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
                decimal msdOperationHistoryID = (await _repository.GetMSDHistorySEQID());
                await _repository.InsertMSDOperationHistory(msdOperationHistoryID, this.msdData.ReelCode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion
    }
}
