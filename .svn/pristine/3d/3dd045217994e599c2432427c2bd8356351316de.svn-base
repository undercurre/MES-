/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检记录主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-31 09:31:24                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsEquipKeepHeadRepository                                      
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
	/// <summary>
	/// 
	/// </summary>
    public interface ISfcsEquipKeepRepository : IBaseRepository<SfcsEquipKeepHead, Decimal>
    {
		/// <summary>
		/// 获取列表数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadData(EquipKeepRequestModel model);

		/// <summary>
		/// 获取可用设备列表
		/// </summary>
		/// <returns></returns>
		Task<List<SfcsEquipment>> GetEquipmentAsync();


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
		Task<SfcsEquipKeepHead> LoadMainAsync(decimal id);

		/// <summary>
		/// 获取明细列表
		/// </summary>
		/// <param name="m_id"></param>
		/// <returns></returns>
		Task<List<EquipDtlViewModel>> LoadDetailLine(decimal m_id);

		/// <summary>
		/// 获取配置列表
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		Task<List<SfcsEquipContentConf>> GetKeepConfigLine(decimal equip_id, decimal keep_type);

		/// <summary>
		/// 获取配置列表(带组织架构)
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		Task<List<SfcsEquipContentConf>> GetKeepConfigLineByOrganize(decimal equip_id, decimal keep_type, string user_name);
		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(EquipKeepAddOrModifyModel model);

		/// <summary>
		/// 获取设备状态
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		Task<decimal> GetEquipmentStatusByIdAsync(decimal id);

		/// <summary>
		/// 获取点检作业图
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		Task<List<SOP_OPERATIONS_ROUTES_RESOURCE>> LoadSOPDataync(decimal id);

		/// <summary>
		/// 加载月份数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadMonthDataAsync(EquipKeepRequestModel model);

		/// <summary>
		/// 加载日数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadDayDataAsync(EquipKeepRequestModel model);
	}
}