/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-06 10:41:02                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsScraperOperationHistoryRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface ISfcsScraperOperatRepository : IBaseRepository<SfcsScraperOperationHistory, Decimal>
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

		// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetSEQID();

        /// <summary>
		/// 获取刮刀状态列表
		/// </summary>
		/// <returns></returns>
		Task<List<IDNAME>> GetScraperStatusAsync();

        /// <summary>
		/// 获取刮刀runcard数据
		/// </summary>
		/// <param name="scraperNo"></param>
		/// <returns></returns>
		Task<SfcsScraperRuncard> GetScraperRuncard(string scraperNo);

        /// <summary>
        /// 获取Smt Line 列表
        /// </summary>
        /// <returns></returns>
        Task<List<IDNAME>> GetSmtLine();

        /// <summary>
        /// 获取刮刀作业历史列表
        /// </summary>
        /// <param name="scraperNo">刮刀号</param>
        /// <returns></returns>
        Task<List<SfcsScraperOperationHistory>> GetScraperOperationHistoryList(string scraperNo);

        /// <summary>
        /// 获取刮刀作业历史信息
        /// </summary>
        /// <param name="scraperNo">刮刀号</param>
        /// <param name="scraperNo">刮刀状态</param>
        /// <returns></returns>
        Task<SfcsScraperOperationHistory> GetScraperOperationHistory(string scraperNo, decimal scraper_status);

        /// <summary>
        /// 获取刮刀注册信息
        /// </summary>
        /// <param name="scraperNo"></param>
        /// <returns></returns>
        Task<SfcsScraperConfig> GetScraperConfig(string scraperNo);

        /// <summary>
        /// 获取站点刮刀runcard数据
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        Task<List<SfcsScraperRuncard>> GetScraperRuncardBySite(decimal siteID);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SfcsScraperOperatModel model);

        /// <summary>
        /// 刮刀品质柏拉图
        /// </summary>
        /// <param name="SCRAPER_NO"></param>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        Task<TableDataModel> GetScraperUseData(string SCRAPER_NO, PageModel pageModel);
    }
}