using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels.MesTongs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository.MesTongs
{
	public interface IMesTongsApplyRepository : IBaseRepository<MesTongsApply, Decimal>
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
		/// 新增/修改夹具申请信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<BaseResult> AddOrModify(MesTongsApplyListModel model);

		/// <summary>
		/// 获取夹具申请信息列表信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>	
		Task<IEnumerable<MesTongsApplyListModel>> GetTongsApplyData(MesTongsApplyRequestModel model);

		/// <summary>
		/// 获取夹具申请信息数量
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetTongsApplyDataCount(MesTongsApplyRequestModel model);

		/// <summary>
		/// 获取夹具申请信息列表信息（包含对应产品信息）
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>	
		Task<List<dynamic>> GetTongsApplyAndPartData(MesTongsApplyRequestModel model);

		/// <summary>
		/// 获取夹具申请信息数量
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetTongsApplyAndPartDataCount(MesTongsApplyRequestModel model);

		/// <summary>
		/// 根据ID获取夹具申请信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<MesTongsApplyListModel> GetTongsApplyById(decimal id);

		/// <summary>
		/// 根据夹具申请ID获取对应产品信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<MesTongsPartModel>> GetTongsApplyPartData(decimal id);

		/// <summary>
		/// 根据夹具申请ID获取对应已入库夹具信息
		/// </summary>
		/// <param name="organizeId"></param>
		/// <param name="applyId"></param>
		/// <returns></returns>
		Task<List<MesTongsInfoListModel>> GetTongsDataByApplyId(string organizeId, decimal applyId);

		/// <summary>
		/// 根据ID删除夹具申请信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<BaseResult> DeleteById(decimal id);

		/// <summary>
		/// 审核夹具申请信息
		/// </summary>
		/// <param name="id"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<BaseResult> AuditData(decimal id, string user);

	}

}
