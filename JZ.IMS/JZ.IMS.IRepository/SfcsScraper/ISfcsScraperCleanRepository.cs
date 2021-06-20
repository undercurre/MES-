/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-03 17:21:30                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsScraperCleanHistoryRepository                                      
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
    public interface ISfcsScraperCleanRepository : IBaseRepository<SfcsScraperCleanHistory, Decimal>
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
		/// 获取刮刀清洗记录数据
		/// </summary>
		/// <param name="scraperNo"></param>
		/// <returns></returns>
		Task<SfcsScraperCleanHistory> GetScraperCleanHistory(string scraperNo);

		/// <summary>
		/// 获取刮刀清洗记录列表
		/// </summary>
		/// <param name="scraperNo">刮刀号</param>
		/// <returns></returns>
		Task<List<SfcsScraperCleanHistory>> GetScraperCleanHistoryList(string scraperNo);

		/// <summary>
		/// 获取Smt Line
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<IDNAME> GetSmtLine(decimal id);

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SfcsScraperCleanModel model);

	}
}