using JZ.IMS.Core.DbHelper;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.ViewModels.SmtMSD;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.MSD;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.BomVsPlacement
{


    public enum MSDStandard
    {
        NormalTemperature = 30,
        NormalHumidity = 60,
        DryTemperature = 25,
        DryHumidity = 0,
        VacuumTemperature = 30,
        VacuumHumidity = 0
    }

    public  class SmtMsdRuncardService
    {
        private readonly ISmtMsdRuncardRepository _repository;
        private readonly IStringLocalizer<SmtMsdRuncardController> _localizer;
        public SmtMsdRuncardService(ISmtMsdRuncardRepository repository, IStringLocalizer<SmtMsdRuncardController> localizer)
        {
            _repository = repository;
            _localizer = localizer;
           msdRuncardTable = new List<SmtMsdRuncard>();
        }

        #region const

        #endregion

        #region fields

        private  string reelCode;
        private  string reelPartNO;
        private  string reelMakerCode;
        private  string partLevelCode;
        private  decimal partThickness;
        private  string operateBy;
        private  string userName;

        #region 啟動時賦值，無需清空

        private  decimal newAction;
        private  decimal bakeTemperature;
        private  decimal bakeHumidity;
        private  decimal standardBakeTime;

        private string actionArea;
        private string actionAreaType;

        #endregion

        private  List<SmtMsdRuncard> msdRuncardTable;
        private  ImsReelInfoView reelInfoViewRow;

        #endregion  

        #region properties

        /// <summary>
        /// 料卷編號
        /// </summary>
        public string ReelCode
        {
            get
            {
                return this.reelCode;
            }
            set
            {
                this.reelCode = value;
            }
        }

        public  string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 新動作
        /// </summary>
        public  decimal NewAction
        {
            get
            {
                return this.newAction;
            }
            set
            {
                this.newAction = value;
            }
        }

        /// <summary>
        /// 操作區域
        /// </summary>
        public string ActionArea
        {
            get
            {
                return this.actionArea;
            }
            set
            {
                this.actionArea = value;
            }
        }

        /// <summary>
        /// 操作區域類型
        /// </summary>
        public string ActionAreaType
        {
            get
            {
                return this.actionAreaType;
            }
            set
            {
                this.actionAreaType = value;
            }
        }

        /// <summary>
        /// 烘烤溫度
        /// </summary>
        public decimal BakeTemperature
        {
            get
            {
                return this.bakeTemperature;
            }
            set
            {
                this.bakeTemperature = value;
            }
        }

        /// <summary>
        /// 烘烤溫度
        /// </summary>
        public decimal BakeHumidity
        {
            get
            {
                return this.bakeHumidity;
            }
            set
            {
                this.bakeHumidity = value;
            }
        }

        /// <summary>
        /// 標準烘烤時數
        /// </summary>
        public decimal StandardBakeTime
        {
            get
            {
                return this.standardBakeTime;
            }
            set
            {
                this.standardBakeTime = value;
            }
        }

        /// <summary>
        /// 操作者
        /// </summary>
        public string OperateBy
        {
            get { return this.operateBy; }
            set { this.operateBy = value; }
        }


        /// <summary>
        /// MSD Runcard
        /// </summary>
        private SmtMsdRuncard MSDRuncardRow
        {
            get
            {
                if (this.msdRuncardTable.Count == 0)
                {
                    return null;
                }
                return this.msdRuncardTable.FirstOrDefault();
            }
            set { this.MSDRuncardRow = value; }
        
        }

        #region Reel Info

        /// <summary>
        /// Reel Info 
        /// </summary>
        private ImsReelInfoView ReelInfoViewRow
        {
            get
            {
                return this.reelInfoViewRow;
            }
            set { this.reelInfoViewRow = value; }
        }

        /// <summary>
        /// 元件料號
        /// </summary>
        public string ReelPartNO
        {
            get
            {
                return this.reelPartNO;
            }
            set 
            { 
                this.reelPartNO = value; 
            }
        }

        /// <summary>
        /// 元件厚度
        /// </summary>
        public string PartThickness
        {
            get
            {
                return this.partThickness.ToString();
            }
            set 
            {
                this.partThickness = Convert.ToDecimal(value);
            }
        }

        /// <summary>
        /// 元件等級
        /// </summary>
        public string PartLevelCode
        {
            get
            {
                return this.partLevelCode;
            }
            set 
            {
                this.partLevelCode = value;
            }
        }

        /// <summary>
        /// 當前動作
        /// </summary>
        public decimal CurrentAction
        {
            get
            {
                if (this.MSDRuncardRow == null)
                {
                    return 0;
                }
                return this.MSDRuncardRow.CURRENT_ACTION;
            }
        }

        /// <summary>
        /// 開始時間
        /// </summary>
        public string BeginTime
        {
            get
            {
                if (this.MSDRuncardRow == null)
                {
                    return string.Empty;
                }
                return this.MSDRuncardRow.BEGIN_TIME.ToString();
            }
        }

        /// <summary>
        /// 經過時間
        /// </summary>
        public string PassedTime
        {
            get
            {
                if (this.MSDRuncardRow == null)
                {
                    return "0";
                }

                DateTime endTime = this.MSDRuncardRow.END_TIME == null ? _repository.GetSystemTime() : this.MSDRuncardRow.END_TIME;

                TimeSpan timeSpan = endTime - this.MSDRuncardRow.BEGIN_TIME ;
                return Math.Round(timeSpan.TotalHours, 3).ToString();
            }
        }

        /// <summary>
        /// 額定管控時間(Bake)
        /// </summary>
        public string BakeRuleHours
        {
            get
            {
                //decimal bakeOverTime = this.GetBakeOverTime();
                //MSDDataSet.SMT_MSD_BAKE_RULERow bakeRuleRow =
                //    MSDDataManager.GetMSDBakeStandard(this.partLevelCode, this.partThickness,
                //    (decimal)MSDStandard.NormalTemperature, (decimal)MSDStandard.NormalHumidity, bakeOverTime).FirstOrDefault();
                //if (bakeRuleRow==Null)
                //{
                //    return "0";
                //}
                //return bakeRuleRow.BAKE_TIME.ToString();

                if (this.MSDRuncardRow == null)
                {
                    return "0";
                }

                var historyRow = _repository.GetMSDOperationHistoryDataTable(this.MSDRuncardRow.REEL_ID, (decimal)MSDAction.Bake).FirstOrDefault();
                TimeSpan timeSpan = historyRow.STANDARD_END_TIME  - historyRow.BEGIN_TIME ;

                return timeSpan.TotalHours.ToString();
            }
        }

        /// <summary>
        /// 烘烤后的總開封時間(小時)
        /// </summary>
        public double TotalOpenTimeAfterBake
        {
            get
            {
                //        if (this.MSDRuncardRow==Null)
                //        {
                //            return 0;
                //        }

                //        if (this.MSDRuncardRow.CURRENT_ACTION == (decimal)MSDAction.Bake)
                //        {
                //            // 烘烤時不計算總暴露時間
                //            return 0;
                //        }

                //        MSDDataSet.SMT_MSD_OPERATION_HISTORYDataTable msdOperationHistoryTable =
                //            MSDDataManager.GetMSDOperationHistoryDataTable(
                //            new KeyValuePair<string, object>(GlobalVariables.REEL_ID, this.MSDRuncardRow.REEL_ID),
                //            new KeyValuePair<string, object>(GlobalVariables.ACTION_CODE, (decimal)MSDAction.Bake));

                //        double totalOpenTime = 0;
                //        if (msdOperationHistoryTable.Rows.Count > 0)
                //        {
                //            // 當前正在烘烤不會進入這段邏輯，所以最後一筆烘烤記錄之後必定存在其他記錄
                //            // 計算最後一次烘烤記錄后的暴露時間
                //            MSDDataSet.SMT_MSD_OPERATION_HISTORYRow msdOperationHistoryRow = 
                //                msdOperationHistoryTable.OrderByDescending(f => f.BEGIN_TIME).FirstOrDefault();
                //            if (msdOperationHistoryRow==Null)
                //            {
                //                return 0;
                //            }
                //            totalOpenTime = MSDDataManager.GetMSDActionTotalTime(this.MSDRuncardRow.REEL_ID, (decimal)MSDAction.Open, msdOperationHistoryRow.BEGIN_TIME);
                //        }
                //        else
                //        {
                //            // 沒有烘烤記錄，計算整個生命週期所有的開封時間
                //            totalOpenTime = MSDDataManager.GetMSDActionTotalTime(this.MSDRuncardRow.REEL_ID, (decimal)MSDAction.Open, null);
                //            if (totalOpenTime == 0)
                //            {
                //                // 如果沒有歷史記錄，通過MSD Runcard計算開封時間
                //                TimeSpan timsSpan = LookupManager.GetSysDate() - this.MSDRuncardRow.BEGIN_TIME;
                //                totalOpenTime = timsSpan.TotalHours;
                //            }
                //        }

                //        return Math.Round(totalOpenTime, 3);

                if (this.MSDRuncardRow == null || this.MSDRuncardRow == null)
                {
                    return 0;
                }

                if (this.MSDRuncardRow.CURRENT_ACTION == (decimal)MSDAction.Open)
                {
                    TimeSpan timeSpan = _repository.GetSystemTime() - MSDRuncardRow.BEGIN_TIME;
                    // 當前已開封的時間+累計開封時間，作為總的開封時間
                    return Math.Round(timeSpan.TotalHours + (double)this.MSDRuncardRow.TOTAL_OPEN_TIME, 3);
                }
                else
                {
                    return (double)this.GetTotalOpenTime();
                }
            }
        }

        /// <summary>
        /// 當前區域
        /// </summary>
        public string CurrentArea
        {
            get
            {
                if (this.MSDRuncardRow == null)
                {
                    return string.Empty;
                }
                return this.MSDRuncardRow.AREA;
            }
        }

        #endregion

        #endregion

        #region 開封(Open)

        #endregion

        #region 抽真空包(Vacuum)

        #endregion

        #region 烘烤(Bake)

        /// <summary>
        /// 獲取Bake Over Time
        /// </summary>
        private decimal GetBakeOverTime()
        {


            if (_repository.GetMSDOperationHistoryDataTable(this.reelCode, (decimal)MSDAction.Open).Count == 0)
            {
                //this.bakeOverTime = 73;
                return 73;
            }
            else
            {
                decimal bakeOverTime = this.GetTotalOpenTime() - this.GetMSDFloorLife();
                if (bakeOverTime < 0)
                {
                    //this.bakeOverTime = 1;
                    bakeOverTime = 1;
                }
                return bakeOverTime;
            }
        }

        #endregion

        #region 乾燥(Dry)

        #endregion

        #region get

        /// <summary>
        /// 取得物料資訊
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public dynamic GetMSDReelInfo(string reelCode)//SMT_MSD_REEL_INFODataTable
        {
            return _repository.GetMSDReelInfo(reelCode);
        }

        /// <summary>
        /// 獲取MSD Floor Life
        /// </summary>
        /// <returns></returns>
        public decimal GetMSDFloorLife()
        {
            var levelRuleRow = _repository.GetMSDFloorLife(this.partLevelCode, this.partThickness,
                (decimal)MSDStandard.NormalTemperature, (decimal)MSDStandard.NormalHumidity).FirstOrDefault();
            decimal floorLife = 0;
            if (levelRuleRow!=null)
            {
                floorLife = levelRuleRow.FLOOR_LIFE??0;
            }
            
            if (floorLife == 0)
            {
                ///"没有找到等级{0}，厚度{1}，暴露温度{2}，暴露湿度{3}的MSD管控标准，请确认。"
                throw new Exception(string.Format(_localizer["No_MSD_CONTROL_STANDARDS"],
                    this.partLevelCode, this.partThickness, (decimal)MSDStandard.NormalTemperature, (decimal)MSDStandard.NormalHumidity));
            }

            return floorLife;
        }

        /// <summary>
        /// 取得MSD Runcard的Total Open Time
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalOpenTime()
        {
            if (this.MSDRuncardRow == null || this.MSDRuncardRow.TOTAL_OPEN_TIME == null)
            {
                return 0;
            }
            return this.MSDRuncardRow.TOTAL_OPEN_TIME ?? 0;
        }

        #endregion

        #region methods

        /// <summary>
        /// 獲取元件烘烤標準
        /// </summary>
        /// <returns></returns>
        public async Task<List<SmtMsdBakeRule>> GetMSDBakeStandard()
        {
            // 默认室温30度，湿度60%
            decimal bakeOverTime = this.GetBakeOverTime();
           var list= ( await _repository.GetMSDBakeStandard(this.partLevelCode, this.partThickness,
               (decimal)MSDStandard.NormalTemperature, (decimal)MSDStandard.NormalHumidity, bakeOverTime));
            return list;
        }

        /// <summary>
        /// 檢查Reel的合法性
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IdentifyReel(string _reelCode)
        {

           await GetData(_reelCode);
            if (this.reelInfoViewRow == null)
            {
                // Reel在IMS中不存在 料卷{0}不存在，请检查。
                throw new Exception(string.Format(_localizer["REEL_NOT_EXIST_ERROR"], reelCode));
            }

            this.reelCode = _reelCode;
            this.reelPartNO = this.reelInfoViewRow.PART_NO;
            if (!string.IsNullOrEmpty(this.reelInfoViewRow.MAKER_CODE))
            {
                this.reelMakerCode = this.reelInfoViewRow.MAKER_CODE;
            }

            if (this.msdRuncardTable.Count == 0)
            {
                // Reel第一次管控MSD時，從R12獲取等級和厚度
                string msdR12cmd = "SELECT * FROM IMS_MSD_R12 WHERE PART_CODE=:PART_CODE AND MAKER_CODE=:MAKER_CODE";
                string msdR12cmdEx = "SELECT * FROM IMS_MSD_R12 WHERE PART_CODE=:PART_CODE ";
                List<ImsMsdR12> msdR12Table = (await _repository.QueryAsyncEx<ImsMsdR12>(msdR12cmd, new { PART_CODE = this.reelPartNO, MAKER_CODE = this.reelMakerCode }))?.ToList();

                if (msdR12Table.Count == 0)
                {

                    msdR12Table = (await _repository.QueryAsyncEx<ImsMsdR12>(msdR12cmdEx, new { PART_CODE = this.reelPartNO }))?.ToList();
                }

                if (msdR12Table.Count > 0)
                {
                    this.partLevelCode = msdR12Table[0].LEVEL_CODE.ToString().ToLower();
                    this.partThickness = msdR12Table[0].PART_THICKNESS == null ? 0 : (decimal)msdR12Table[0].PART_THICKNESS;
                }
                else
                {
                    //"没有找到元件等级，请确认。"
                    throw new Exception(_localizer["COMPONENT_NO_FOUND"]);
                }
            }
            else
            {
                // 已有管控的，按録入時候的等級和厚度繼續管控
                this.partLevelCode = this.msdRuncardTable.FirstOrDefault().LEVEL_CODE;
                this.partThickness = this.msdRuncardTable.FirstOrDefault().THICKNESS ?? 0;
            }

            // 只有Reel第一次管控MSD時，檢查等級并提示
            if (this.partLevelCode == null || (this.partLevelCode == "0") || (this.partLevelCode == "1"))
            {
                string hintMessage = string.Empty;
                if (this.partLevelCode == null)
                {
                    //"料卷{0}等级缺省，不需要进行管控(系统默认)。"
                    hintMessage = string.Format(_localizer["MATERIAL_NO_CONTROL"], reelCode);
                }
                else
                {
                    //料卷{0}等级为<{1}>，不需要进行管控。
                    hintMessage = string.Format(_localizer["MATERIAL_NO_CONTROLEX"], reelCode, this.partLevelCode);
                }
                throw new Exception(hintMessage);
                // Messenger.Hint(hintMessage);
                // return false;
            }

            // R12沒有元件厚度時，獲取默認厚度
            if (this.partThickness == 0)
            {
                SmtLookup lookupRow = (await _repository.QueryAsyncEx<SmtLookup>("SELECT * FROM SMT_LOOKUP WHERE TYPE='MSD_DEFAULT_THICKNESS' AND Enabled='Y'")).FirstOrDefault();
                if (lookupRow != null)
                {
                    this.partThickness = Convert.ToDecimal(lookupRow.VALUE);
                }
            }

            //if (this.msdRuncardTable.Rows.Count == 0 &&
            //    IQCManager.GetDMRReel(
            //        new KeyValuePair<string, object>(GlobalVariables.REEL_ID, this.reelInfoViewRow.ID)).Rows.Count > 0)
            //{
            //    Messenger.Hint(string.Format("料卷{0}是DMR特殊物料，有可能过期或破损，请务必先烘烤再使用。", this.reelCode));
            //}

            return true;
        }


        /// <summary>
        /// 取得元件資訊
        /// </summary>
        public async Task<dynamic> GetMSDInfo(MsdInfoProviderView msdinfo)
        {
            try
            {

                // 元件料號
                msdinfo.ReelPartNO = this.ReelPartNO;
                // 元件厚度
                msdinfo.PartThickness = this.PartThickness;
                // 元件等級
                msdinfo.PartLevelCode = this.PartLevelCode;
                //當前動作
                msdinfo.CurrentAction = this.CurrentAction;
                // 開始時間
                msdinfo.BeginTime = this.BeginTime;
                // 當前動作經歷時間
                msdinfo.PassedTime = this.PassedTime;

                // 額定管控時間(Bake)
                if (this.CurrentAction == (decimal)MSDAction.Bake)
                {
                    msdinfo.RatedControlTime = this.BakeRuleHours;
                }

                // 暴露時間
                msdinfo.TotalOpenTimeAfterBake = this.TotalOpenTimeAfterBake.ToString();
                //this.totalOpenTimeTextEdit.Text = this.msdInfoProvider.GetTotalOpenTime().ToString();

                if (this.CurrentAction == (decimal)MSDAction.Open)
                {
                    msdinfo.RatedControlTime = this.GetMSDFloorLife().ToString();

                    // 剩餘有效時數
                    double floorLife = Convert.ToDouble(msdinfo.RatedControlTime) - Convert.ToDouble(msdinfo.TotalOpenTimeAfterBake);
                    msdinfo.FloorLife = floorLife.ToString();
                    if (floorLife <= 0)
                    {
                        //料卷{0}的暴露时间已经超过有效期，请进行烘烤。
                        throw new Exception(string.Format(_localizer["Err_MSDReelNeedToBake"], msdinfo.ReelCode));
                    }
                }

                msdinfo.CurrentArea = this.CurrentArea;

                // Reel信息
                return await _repository.GetMSDReelInfo(msdinfo.ReelCode);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        #endregion

        #region finish action

        public void FinishCurrentAction(MSDData msdData)
        {
            try
            {
                if (msdData.NewAction == (decimal)MSDAction.TransferAfterOpen)
                {
                    // 修改作業區域不需要進行結束動作
                    return;
                }

                // 非首次操作時，自動結束當前操作再進入下一操作
                if (this.MSDRuncardRow != null)
                {
                    switch ((int)this.CurrentAction)
                    {
                        case (int)MSDAction.Open:
                            MSDOpenService msdOpen = new MSDOpenService(_repository, _localizer, UserName);
                            msdOpen.FinishAction(msdData);
                            break;
                        case (int)MSDAction.Bake:
                            MSDBakeService msdBake = new MSDBakeService(_repository, _localizer, UserName);
                            msdBake.FinishAction(msdData);
                            break;
                        case (int)MSDAction.Dry:
                            MSDDryService msdDry = new MSDDryService(_repository, _localizer, UserName);
                            msdDry.FinishAction(msdData);
                            break;
                        case (int)MSDAction.Vacuum:
                            MSDVacuumService msdVacuum = new MSDVacuumService(_repository, _localizer, UserName);
                            msdVacuum.FinishAction(msdData);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region process

        /// <summary>
        /// 初始化數據
        /// </summary>
        /// <param name="msdData"></param>
        public async Task InitalizeMSDData(MSDData msdData)
        {
            try
            {
                msdData.ReelCode = this.reelCode;

                // 溫度濕度
                switch ((int)this.newAction)
                {
                    case (int)MSDAction.Open:
                        msdData.Temperature = (decimal)MSDStandard.NormalTemperature;
                        msdData.Humidity = (decimal)MSDStandard.NormalHumidity;
                        break;
                    case (int)MSDAction.Bake:
                        msdData.Temperature = this.bakeTemperature;
                        msdData.Humidity = this.bakeHumidity;
                        msdData.StandardBakeTime = this.standardBakeTime;
                        break;
                    case (int)MSDAction.Dry:
                        msdData.Temperature = (decimal)MSDStandard.DryTemperature;
                        msdData.Humidity = (decimal)MSDStandard.DryHumidity;
                        break;
                    case (int)MSDAction.Vacuum:
                        msdData.Temperature = (decimal)MSDStandard.VacuumTemperature;
                        msdData.Humidity = (decimal)MSDStandard.VacuumHumidity;
                        break;
                    default:
                        break;
                }

                msdData.CurrentAction = this.CurrentAction;
                string cmd = @"	SELECT * FROM SMT_LOOKUP WHERE TYPE = 'MSD_ACTION' AND CODE =:CODE ";
                var msdActionRow = (await _repository.QueryAsyncEx<SmtLookup>(cmd, new { CODE = msdData.CurrentAction })).FirstOrDefault();
                msdData.CurrentActionName = msdActionRow == null ? string.Empty : msdActionRow.CN_DESC;
                msdData.NewAction = this.newAction;
                msdData.ActionArea = this.actionArea;
                msdData.ActionAreaType = this.actionAreaType;
                msdData.FloorLife = this.GetMSDFloorLife();
                msdData.LevelCode = this.partLevelCode;
                msdData.Thickness = this.partThickness;
                msdData.MSDRuncardRow = this.MSDRuncardRow;
                msdData.TotalOpenTime = this.GetTotalOpenTime();
                msdData.OperateBy = string.IsNullOrEmpty(this.operateBy) ? userName : operateBy;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }


        /// <summary>
        /// 處理
        /// </summary>
        public async Task Process(string reelCode)
        {
       
            try
            {
                if (!(await IdentifyReel(reelCode)))
                {
                    return;
                }

                if ((await CheckBeforeProcess()))
                {
                    // 初始化MSD Data
                    MSDData msdData = new MSDData();

                   await InitalizeMSDData(msdData);

                    // 第一次操作且非Open時，自動補上Open操作
                    //if ((this.MSDRuncardRow.IsNullOrEmpty()) && (this.newAction != (decimal)MSDAction.End) &&
                    //    (this.newAction > (decimal)MSDAction.Open))
                    //{
                    //    MSDOpen msdOpen = new MSDOpen();
                    //    msdOpen.TakeAction(msdData);
                    //}

                   FinishCurrentAction(msdData);
                  await  InitalizeMSDDataBeforeTakeAction(msdData);
                  await TakeAction(msdData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task InitalizeMSDDataBeforeTakeAction(MSDData msdData)
        {
            // MSD Runcard已變更，需重新刷新資料
            string cmd = "SELECT * FROM SMT_MSD_RUNCARD WHERE REEL_ID=:REEL_ID";
            this.msdRuncardTable=(await _repository.QueryAsyncEx<SmtMsdRuncard>(cmd, new { REEL_ID = reelCode }))?.ToList();
            msdData.MSDRuncardRow = this.MSDRuncardRow;

            // 獲取最新的開封總時間
            msdData.TotalOpenTime = this.GetTotalOpenTime();
        }

        /// <summary>
        /// 執行操作
        /// </summary>
        public async Task TakeAction(MSDData msdData)
        {
            try
            {
                switch ((int)this.newAction)
                {
                    case (int)MSDAction.Open:
                        MSDOpenService msdOpen = new MSDOpenService(_repository, _localizer, UserName);
                       await msdOpen.TakeAction(msdData);

                        // 同步開封時間到Fuji Trax
                        //MSDDataSet.SMT_MSD_RUNCARDRow msdRuncardRow =
                        //    MSDDataManager.GetMSDRuncardDataTable(new KeyValuePair<string, object>(GlobalVariables.REEL_ID, msdData.ReelCode)).FirstOrDefault();
                        //if (!msdRuncardRow.IsNullOrEmpty() && !msdRuncardRow.IsFLOOR_LIFE_END_TIMENull())
                        //{
                        // 正常情況下，上述條件都不會為空
                        // 反向計算Reel的開封時間
                        //DateTime openTime = msdRuncardRow.FLOOR_LIFE_END_TIME.AddHours((double )(-msdData.FloorLife));
                        // 將開封時間壓入Fuji
                        // IMS的MSD管控標準比Fuji嚴格2小時，所以加2小時，然後轉成分鐘
                        //FujiTraxManager.UpdateFujitraxReelMSDOpenByReel(msdData.ReelCode, openTime, (msdData.FloorLife + 2) * 60);
                        //}
                        break;
                    case (int)MSDAction.Bake:
                        MSDBakeService msdBake = new MSDBakeService(_repository, _localizer, UserName);
                      await  msdBake.TakeAction(msdData);
                        //FujiTraxManager.UpdateFujitraxReelMSDBakeByReel(msdData.ReelCode); //add by matthew 2017/4/10 .更新fujitrax中MSD記錄
                        break;
                    case (int)MSDAction.Dry:
                        MSDDryService msdDry = new MSDDryService(_repository, _localizer, UserName);
                      await  msdDry.TakeAction(msdData);
                        //FujiTraxManager.UpdateFujitraxReelMSDDryOrVacuumByReel(msdData.ReelCode);   //add by matthew 2017/4/10 .更新fujitrax中MSD記錄
                        break;
                    case (int)MSDAction.Vacuum:
                        MSDVacuumService msdVacuum = new MSDVacuumService(_repository, _localizer, UserName);
                       await msdVacuum.TakeAction(msdData);
                        //FujiTraxManager.UpdateFujitraxReelMSDDryOrVacuumByReel(msdData.ReelCode);     //add by matthew 2017/4/10 .更新fujitrax中MSD記錄
                        break;
                    case (int)MSDAction.End:
                        MSDEndService msdEnd = new MSDEndService(_repository, _localizer, UserName);
                      await  msdEnd.TakeAction(msdData);
                        break;
                    case (int)MSDAction.TransferAfterOpen:
                        MSDTransferAfterOpenService msdTransferAfterOpen = new MSDTransferAfterOpenService(_repository, _localizer, UserName);
                       await msdTransferAfterOpen.TakeAction(msdData);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 操作成功提示
        /// </summary>
        /// <param name="reelCode"></param>
        private void HintProcessOK(string reelCode)
        {
            //this.Clear();
            //ClientPublic.ShowMessageBox(Properties.Resources.Msg_ProcessOK);
            //this.reelCodeTextEdit.Text = reelCode;
            //this.reelCodeTextEdit.SelectAll();
            //this.reelCodeTextEdit.Focus();
        }

        /// <summary>
        /// 檢查
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckBeforeProcess()
        {
            
            if (this.MSDRuncardRow!=null &&
                this.MSDRuncardRow.CURRENT_ACTION == (decimal)MSDAction.End)
            {
                // Double Check Reel狀態 料卷已刷用完，不能作业。
                throw new Exception(_localizer["Material_Used_Finish"]);
            }

            // 第一次操作且非Open時提醒
            if ((this.MSDRuncardRow==null) && (this.newAction != (decimal)MSDAction.End) &&
                (this.newAction > (decimal)MSDAction.Open))
            {
                //全新料卷必须先开封，再进行其他操作。
                throw new Exception(_localizer["Must_Be_Operations"]);
            }

            //if (RuncardManager.IsReelOnLineUsing(this.reelCode))
            //{
            //    throw new Exception(string.Format("料卷{0}還在生產中，不能進行MSD管控作業", this.reelCode));
            //}

            if (this.newAction == this.CurrentAction)
            {
                //料卷{0}正在进行相同的动作，请确认。
                throw new Exception(string.Format(_localizer["Material_Same_Operation"], this.reelCode));
            }

            if (this.newAction != (decimal)MSDAction.End)
            {
                // 指定物料禁止烘烤，超期后直接提示刷用完
                string cmd = @"	SELECT * FROM SMT_LOOKUP WHERE TYPE = 'FORBIDDEN_BAKE_PART' and VALUE =:VALUE and Enabled = 'Y'
";
               var lookUpTemp =(await _repository.QueryAsyncEx<SmtLookup>(cmd, new { VALUE = this.reelPartNO }))?.ToList();
                if (lookUpTemp.Count > 0)
                {
                    if (this.TotalOpenTimeAfterBake > (double)this.GetMSDFloorLife())
                    {
                        //物料{0}开封已超期，此类物料禁止烘烤，请刷用完。
                        throw new Exception(string.Format(_localizer["Material_ Forbidden_Bake"], this.reelPartNO));
                    }
                }
            }

            return true;
        }








        #endregion

        #region 公共方法
        public async Task GetData(string _reelCode)
        {
            // 獲取MSD Runcard信息
            this.msdRuncardTable = _repository.GetMSDRuncardDataTable(_reelCode);

            // 檢查Reel是否存在
            this.reelInfoViewRow = (await _repository.GetReelInfoView(_reelCode)).FirstOrDefault();
        }
        #endregion

    }
}
