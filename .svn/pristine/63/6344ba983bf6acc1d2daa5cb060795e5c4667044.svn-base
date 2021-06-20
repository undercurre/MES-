/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫记录处理                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-23 16:09:13                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IAndonCallRecordHandleRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface IAndonCallRecordHandleRepository : IBaseRepository<AndonCallRecordHandle, Decimal>
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
		/// 添加处理记录并更新呼叫主表的状态
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<BaseResult> InsertAndUpdateSatus(AndonCallRecordHandle model);

		/// <summary>
		///  保存数据(行编辑保存异常记录处理)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(AndonCallRecordHandleAddOrModifyModel model);

		/// <summary>
		/// 新增异常记录处理数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> InsertRecordHandle(AndonCallRecordHandleAddOrModifyModel model);
	}
}