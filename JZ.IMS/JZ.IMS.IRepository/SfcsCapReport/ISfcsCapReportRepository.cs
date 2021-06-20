/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产能报工表结构                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-08 09:11:53                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsCapReportRepository                                      
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
    public interface ISfcsCapReportRepository : IBaseRepository<SfcsCapReport, Decimal>
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
		Task<decimal> SaveDataByTrans(SfcsCapReportModel model);

        /// <summary>
        /// 提交产能报工
        /// </summary>
        /// <returns></returns>
        Task<int> PostToCapacityReport(decimal wo_id, decimal site_id, string Operator, decimal Qty, DateTime reportTime);

        /// <summary>
        /// 撤销产能报工
        /// </summary>
        /// <returns></returns>
        Task<int> ClearCapacityReport(decimal id, string Operator, decimal pass, SfcsWo woModel, decimal siteId, DateTime wokTime, decimal? lineId, bool isLastOperation, decimal? inBoundRecord);


        /// <summary>
        /// 获取站点总的合格率【无码报工使用】
        /// 公式：(SUM(PASS)-SUM(FAIL))/SUM(PASS)*100
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="woId"></param>
        /// <returns></returns>
        Task<TableDataModel> GetSitePassRate(decimal siteId, decimal woId, string type);

        /// <summary>
        /// 获取站点的第几小时产能
        /// </summary>
        /// <param name="currentWoId">工单ID</param>
        /// <param name="currentOperationSiteID">站点ID</param>
        /// <returns>PASS产能</returns>
        Task<TableDataModel> GetSiteHourYield(decimal siteId, decimal woId);

        /// <summary>
        /// 获取站点总的TOP X 不良现象
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="woId"></param>
        /// <param name="topCount"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<TableDataModel> GetSiteTopDefect(decimal? siteId, decimal woId, int topCount, string type);

        /// <summary>
        /// 根据工单下标获取当前线别生产信息
        /// </summary>
        /// <param name="lineId">线别</param>
        /// <param name="index">工单下标 0：当前工单，1：上一工单，2：上两工单</param>
        /// <returns></returns>
        Task<SfcsProduction> GetProductionByIndex(Decimal lineId, int index = 0);

        /// <summary>
        /// 获取标准产能
        /// </summary>
        /// <param name="PART_NO"></param>
        /// <param name="OPERATION_ID"></param>
        /// <returns></returns>
        Task<decimal> GetStandardCapacity(string PART_NO, decimal OPERATION_ID);

        /// <summary>
        /// 获取报工的制程
        /// </summary>
        /// <param name="wo_id"></param>
        /// <returns></returns>
        Task<List<SfcsRoutesSiteListModel>> GetRouteCapacityDataByWoId(decimal wo_id, decimal route_id);

        /// <summary>
        /// 更新产出数量
        /// </summary>
        /// <param name="wo_id">工单ID</param>
        /// <param name="qty">报工数量</param>
        /// <returns></returns>
        Task<bool> UpdateOutputData(SfcsWo woModel, decimal qty, decimal? lineId);
    }
}