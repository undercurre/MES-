/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫记录                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-22 23:39:21                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IAndonCallRecordRepository                                      
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
	public interface IAndonCallRecordRepository : IBaseRepository<AndonCallRecord, Decimal>
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
		/// 呼叫记录
		/// </summary>
		/// <param name="operationLineId"></param>
		/// <param name="operationLineName"></param>
		/// <param name="operationId"></param>
		/// <param name="operationName"></param>
		/// <param name="operationSiteId"></param>
		/// <param name="operationSiteName"></param>
		/// <param name="user"></param>
		/// <param name="andonTye"></param>
		/// <param name="callCode"></param>
		/// <param name="callContent"></param>
		/// <returns></returns>
		Task<AndonCallRecord> AddCallRecord(Decimal operationLineId, String operationLineName,
			Decimal operationId, String operationName, Decimal operationSiteId, String operationSiteName,
			String user, String andonTye, String callCode, String callContent, Decimal? callContentId=null);

		/// <summary>
		/// 查询数据(呼叫页面展示使用)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		Task<TableDataModel> GetCallRecordData(AndonCallRecordCallPageRequestModel model);

		/// <summary>
		/// 查询数据(编辑页面展示使用)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<AndonCallRecordExtendListModel>> GetCallRecordToEditData(AndonCallRecordCallPageRequestModel model);

		/// <summary>
		/// 查询首页预警信息数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetEarlyWarningData(AndonCallRecordHomePageRequestModel model);

		/// <summary>
		/// 查询首页待办事项信息数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetWaitTakeData(AndonCallRecordHomePageRequestModel model);

	}
}