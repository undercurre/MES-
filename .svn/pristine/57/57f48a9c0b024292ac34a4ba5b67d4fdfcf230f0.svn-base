/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：巡检主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 13:51:00                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IIpqaMstRepository                                      
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
    public interface IIpqaMstRepository : IBaseRepository<IpqaMst, Decimal>
    {
		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadData(IpqaMstRequestModel model);

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		Task<decimal> GetSEQID();

		/// <summary>
		/// 获取主表对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<IpqaMst> LoadMainAsync(decimal id);

		/// <summary>
		/// 获取明细列表
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		Task<List<IpqaDtlViewModel>> LoadDetailAsync(decimal mst_id);

		/// <summary>
		/// 获取配置列表
		/// </summary>
		/// <param name="ipqa_type"></param>
		/// <returns></returns>
		Task<List<IpqaConfigVM>> GetIpqaConfigAsync(decimal ipqa_type);

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(IpqaMstModel model);

		/// <summary>
		/// 获取单据状态
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<decimal> GetBillStatus(decimal id);

		/// <summary>
		/// 更新状态
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> UpdateStatusById(BillStatusModel model);

		/// <summary>
		/// 删除单据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<decimal> DeleteBill(decimal id);

		/// <summary>
		/// 获取获取工单信息列表
		/// </summary>
		/// <returns></returns>
		Task<TableDataModel> GetWoList(PageModel model);

		/// <summary>
		/// 获取线别当前在线工单产品信息
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<V_IMS_WO> GetPartByLineId(decimal lineId);
	}
}