/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫通知内容                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-23 22:15:35                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IAndonCallNoticeRepository                                      
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
	public interface IAndonCallNoticeRepository : IBaseRepository<AndonCallNotice, Decimal>
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
		/// 获取通知列表
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<IEnumerable<AndonCallNoticeListModel>> GetListByIdAsync(decimal id);

		/// <summary>
		/// 获取通知列表
		/// </summary>
		/// <param name="AndonCallNoticeRequestModel"></param>
		/// <returns></returns>
		Task<IEnumerable<AndonCallNoticeListExModel>> GetListByModelAsync(AndonCallNoticeRequestModel model,string sqlWhere);

		Task<int> GetCountByModelAsync(AndonCallNoticeRequestModel model, string sqlWhere);
	}
}