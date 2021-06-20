/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：换线记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-15 19:06:24                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesChangeLineRecordRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface IMesChangeLineRecordRepository : IBaseRepository<MesChangeLineRecord, Decimal>
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
		/// 根据时间段获取换线记录数
		/// </summary>
		/// <param name="lineId">产线ID</param>
		/// <param name="beginTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <returns></returns>
		int ExistByDate(decimal id, decimal lineId, DateTime beginTime, DateTime endTime);

		Task<decimal> InsertChangeLineRecordAsync(MesChangeLineRecord model);

		Task<decimal> UpdateChangeLineRecordAsync(MesChangeLineRecord model);

		Task<int> DeleteChangeLineRecordAsync(decimal id);
	}
}