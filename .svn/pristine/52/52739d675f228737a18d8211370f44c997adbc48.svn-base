/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-22 09:40:14                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsHoldProductHeaderRepository                                      
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
    public interface ISfcsLockProductHeaderRepository : IBaseRepository<SfcsHoldProductHeader, Decimal>
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
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SfcsHoldProductHeaderModel model);
        /// <summary>
        /// 获取RuncardDataTable
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        Task<List<SfcsRuncard>> GetRuncardDataTable(string SN);
        /// <summary>
        /// 获取SfcsOperationHistory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<SfcsOperationHistory> GetLastBFTHistory(decimal? id);
        /// <summary>
        /// 保存1.單筆/批量產品序號 //10.產品序號與工序
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<List<string>> RuncardSave(List<SfcsRuncard> runcardTable, int mainConditionSelectIndex, string data, int subsidiaryConditionSelectIndex, string inventory, DateTime beginTime, DateTime endTime, int actionSelectIndex, string holdcause, string CurrentOperator);
        /// <summary>
        /// 获取GetHoldBill
        /// </summary>
        /// <returns></returns>
        Task<string> GetHoldBill();
        /// <summary>
        /// 获取GetHoldID
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetHoldID();
        /// <summary>
        /// 获取GetBatchHeaderDataTable
        /// </summary>
        /// <param name="TURNIN_NO"></param>
        /// <returns></returns>
        List<SfcsTurninBatchHeader> GetBatchHeaderDataTable(string TURNIN_NO);
        /// <summary>
        /// HOLD_QTY=1 SAVE
        /// </summary>
        /// <param name="runcardTable"></param>
        /// <param name="partNumber"></param>
        /// <param name="workOrder"></param>
        /// <param name="operationID"></param>
        /// <param name="model"></param>
        /// <param name="mainConditionSelectIndex"></param>
        /// <param name="data"></param>
        /// <param name="subsidiaryConditionSelectIndex"></param>
        /// <param name="inventory"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="actionSelectIndex"></param>
        /// <param name="holdcause"></param>
        /// <param name="CurrentOperator"></param>
        /// <param name="ecnNo"></param>
        /// <returns></returns>
        Task<decimal> HoldQTYSave(List<SfcsRuncard> runcardTable, int mainConditionSelectIndex, string data, int subsidiaryConditionSelectIndex, string inventory, DateTime beginTime, DateTime endTime, int actionSelectIndex, string holdcause, string CurrentOperator, int count);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="runcards"></param>
        /// <param name="actionSelectIndex"></param>
        /// <returns></returns>
        Task<List<SfcsRuncard>> IdentifyInputData(string data, List<SfcsRuncard> runcards, int actionSelectIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runcardTable"></param>
        /// <param name="mainConditionSelectIndex"></param>
        /// <param name="data"></param>
        /// <param name="subsidiaryConditionSelectIndex"></param>
        /// <param name="inventory"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="actionSelectIndex"></param>
        /// <param name="holdcause"></param>
        /// <param name="CurrentOperator"></param>
        /// <param name="count"></param>
        /// <param name="compList"></param>
        /// <returns></returns>

        Task<List<string>> HoldComponentQTYSave(List<SfcsRuncard> runcardTable, int mainConditionSelectIndex, string data, int subsidiaryConditionSelectIndex, string inventory, DateTime beginTime, DateTime endTime, int actionSelectIndex, string holdcause, string CurrentOperator, decimal count, List<string> compList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runcardTable"></param>
        /// <param name="mainConditionSelectIndex"></param>
        /// <param name="data"></param>
        /// <param name="operationSiteName"></param>
        /// <param name="operationSiteID"></param>
        /// <param name="subsidiaryConditionSelectIndex"></param>
        /// <param name="inventory"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="actionSelectIndex"></param>
        /// <param name="holdcause"></param>
        /// <param name="CurrentOperator"></param>
        Task<decimal> ProductOperationSiteSave(List<SfcsRuncard> runcardTable, int mainConditionSelectIndex, string data, string operationSiteName, decimal? operationSiteID, int subsidiaryConditionSelectIndex, string inventory, DateTime? beginTime, DateTime? endTime, int actionSelectIndex, string holdcause, string CurrentOperator);
    }
}