/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产线挪料表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-01-09 17:26:19                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesMovePartRepository                                      
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
	public interface IMesMovePartRepository : IBaseRepository<MesMovePart, Decimal>
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
		/// 审核
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<BaseResult> AuditData(decimal id, string user);

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		Task<decimal> GetSEQID();

		/// <summary>
		/// 根据工单号获取成品品信息
		/// </summary>
		/// <param name="wo_no"></param>
		/// <returns></returns>
		Task<V_IMS_WO> GetPartByWoNo(string wo_no);

		/// <summary>
		/// 判断料号是否存在
		/// </summary>
		/// <param name="part_no"></param>
		/// <returns></returns>
		Task<bool> IsExistPart(string part_no);

		/// <summary>
		/// 根据ID获取数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<MesMovePart> GetMovePartById(decimal id);

		/// <summary>
		/// 判断零件料号是否存在于成品料号中
		/// </summary>
		/// <returns></returns>
		Task<bool> IsExistPart(string product_no, decimal operationId, string part_no);

		/// <summary>
		/// 获取审核/激活后上料结果
		/// </summary>
		/// <param name="wo_no"></param>
		/// <param name="product_no"></param>
		/// <returns></returns>
		Task<IEnumerable<MesMovePartResultModel>> GetAuditResultData(MesMovePartRequestModel model);
	}
}