/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：首五件检验主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-13 09:51:01                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesFirstCheckInfoRepository                                      
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
	public interface IMesFirstCheckInfoRepository : IBaseRepository<MesFirstCheckInfo, Decimal>
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
		/// 根据主表ID删除明细数据
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
		Task<BaseResult> DeleteDetailByMstId(decimal mstId);

		/// <summary>
		/// 获取首五件检验明细数据
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
		Task<IEnumerable<MesFirstCheckDetailListModel>> GetDetailData(decimal mstId);

		/// <summary>
		/// 新增修改首五件明细数据
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<BaseResult> AddOrModifyDetailSave(MesFirstCheckDetailAddOrModifyModel item);

		/// <summary>
		/// 删除首五件明细数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<BaseResult> DeleteDetail(decimal id);

		/// <summary>
		/// 获取检验明细项目信息
		/// </summary>
		/// <param name="detailId"></param>
		/// <returns></returns>
		Task<IEnumerable<MesFirstCheckDetailItemListModel>> GetDetailItemData(decimal detailId);

		/// <summary>
		/// 审核首五件信息
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<BaseResult> AuditData(MesFirstCheckInfoAddOrModifyModel item);
	}
}