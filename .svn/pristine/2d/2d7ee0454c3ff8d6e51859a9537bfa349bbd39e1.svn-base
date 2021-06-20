using JZ.IMS.ViewModels.MesTongs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository.MesTongs
{
	public interface IMesTongsBoardRepository
	{
		/// <summary>
		/// 获取夹具申请数量信息
		/// </summary>
		/// <returns></returns>
		Task<MesTongsBoardDataModel> GetApplyQty();

		/// <summary>
		/// 获取申请信息列表
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<MesTongsBoardDataModel>> GetApplyList();

		/// <summary>
		/// 获取夹具状态分布信息
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<MesTongsBoardDataModel>> GetTongsStatusDis();

		/// <summary>
		/// 获取夹具借出信息列表
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<MesTongsBoardDataModel>> GetTongsBorrowList();
	}
}
