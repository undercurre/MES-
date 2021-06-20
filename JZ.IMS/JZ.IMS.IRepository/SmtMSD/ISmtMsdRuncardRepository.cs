/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-06-20 10:43:03                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtMsdRuncardRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JZ.IMS.IRepository
{
    public interface ISmtMsdRuncardRepository : IBaseRepository<SmtMsdRuncard, String>
    {
        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<Boolean> GetEnableStatus(decimal id);

        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
        Task<decimal> ChangeEnableStatus(decimal id, bool status);

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetSEQID();

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reel_id"></param>
        /// <returns></returns>
        Task<bool> IsMsdReel(string reel_id);

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SmtMsdRuncardModel model);

        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        DateTime GetSystemTime();


        /// <summary>
        /// 取得物料資訊
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task<dynamic> GetMSDReelInfo(string reelCode);

        /// <summary>
        /// 获取MSD操作记录
        /// </summary>
        /// <param name="REEL_ID"></param>
        /// <param name="ACTION_CODE"></param>
        /// <returns></returns>
        List<SmtMsdOperationHistory> GetMSDOperationHistoryDataTable(string REEL_ID, decimal ACTION_CODE);

        /// <summary>
        /// 獲取元件烘烤標準
        /// </summary>
        /// <param name="partLevelCode"></param>
        /// <param name="partThickness"></param>
        /// <param name="NormalTemperature"></param>
        /// <param name="NormalHumidity"></param>
        /// <param name="bakeOverTime"></param>
        /// <returns></returns>
        Task<List<SmtMsdBakeRule>> GetMSDBakeStandard(string levelCode, decimal thickness, decimal openTemperature,
             decimal openHumidity, decimal overTime);

        /// <summary>
        /// 獲取MSD Floor Life
        /// </summary>
        /// <param name="levelCode"></param>
        /// <param name="thickness"></param>
        /// <param name="temperature"></param>
        /// <param name="humidity"></param>
        /// <returns></returns>
        List<SmtMsdLevelRule> GetMSDFloorLife(string levelCode, decimal thickness, decimal temperature, decimal humidity);

        /// <summary>
        /// 獲取SMT_MSD_RUNCARD
        /// </summary>
        /// <param name="REEL_ID"></param>
        /// <returns></returns>
        List<SmtMsdRuncard> GetMSDRuncardDataTable(string REEL_ID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_reelCode"></param>
        /// <returns></returns>
        Task<List<ImsReelInfoView>> GetReelInfoView(string _reelCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReelCode"></param>
        /// <param name="FloorLife"></param>
        /// <returns></returns>
        Task<int> UpdateFloorLifeEndTime(string ReelCode, decimal FloorLife);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        Task<int> DelayFloorLifeEndTime(string reelCode, double actionTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="beginTime"></param>
        /// <param name="totalOpenTime"></param>
        /// <param name="floorLifeEndTime"></param>
        /// <returns></returns>
        Task<int> UpdateFloorLifeAndBeginTime(string reelCode, DateTime beginTime, double totalOpenTime, DateTime floorLifeEndTime);

        /// <summary>
        /// SMT_MSD_OPERAT_HISTORY_SEQ
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetMSDHistorySEQID();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task<int> InsertMSDOperationHistory(decimal id, string reelCode);
        /// <summary>
        /// 更新結束動作的歷史記錄
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="operatorBy"></param>
        /// <param name="area"></param>
        Task LogFinishActionHistory(string reelCode, string operatorBy, decimal actionCode);

        /// <summary>
        /// 更新Total Open Time
        /// </summary>
        /// <param name="reelCode"></param>
        Task UpdateTotalOpenTime(string reelCode);

        /// <summary>
        /// 清除開封時間
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task ClearOpenTime(string reelCode);

        Task CreateMSDRuncard(bool msdRuncardExist, string reelCode, decimal currentAction,
            decimal temperature, decimal humidity, string operatorBy, string levelCode, decimal thickness, string area);


        /// <summary>
        /// 壓入標準耗時，標準結束時間
        /// </summary>
        /// <param name="id"></param>
        /// <param name="standardEndTime"></param>
        /// <returns></returns>
        Task AddStandardEndTime(decimal id, decimal standardEndTime);

        /// <summary>
        /// 通过子Reel获取父Reel的IMS_Reel Table
        /// </summary>
        /// <param name="childReelCode"></param>
        /// <returns></returns>
        Task<List<ImsReel>> GetParentReelByChild(string childReelCode);

        /// <summary>
        /// 获取Reel的创建时间(切分Reel的创建时间相当于切分时间)
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task<DateTime> GetReelCreateTime(string reelCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="actualEndTime"></param>
        /// <param name="parentReelHistoryID"></param>
        /// <returns></returns>
        Task<bool> CopyHistoryFromParentReel(string reelCode, DateTime actualEndTime, decimal parentReelHistoryID);

        /// <summary>
        /// MSD結束管控
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="operatorBy"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        Task<int> MSDActionEnd(string reelCode, string operatorBy, string area);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="temperature"></param>
        /// <param name="humidity"></param>
        /// <returns></returns>
        Task<int> LogActionEndHistory(string reelCode, decimal temperature, decimal humidity);

        /// <summary>
        ///  更新MSD操作區域
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="area"></param>
        /// <param name="operater"></param>
        /// <returns></returns>
        Task<int> UpdateMSDArea(string reelCode, string area, string operater);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        Task<int> LogTransferAfterOpenHistory(string reelCode, decimal actionCode);
    }
}