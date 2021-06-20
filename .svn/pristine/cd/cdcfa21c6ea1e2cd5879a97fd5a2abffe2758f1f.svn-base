/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检维修表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-31 10:50:09                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsEquipRepairHeadRepository                                      
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
    public interface ISfcsEquipRepairHeadRepository : IBaseRepository<SfcsEquipRepairHead, decimal>
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

		// <summary>
		/// 获取维修记录列表
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<SfcsEquipRepairHeadListModel>> GetLoadData(int pageNumber, int rowsPerPage, decimal equipId);

		// <summary>
		/// 获取维修记录数
		/// </summary>
		/// <returns></returns>
		Task<int> GetLoadDataCnt(decimal equipId);

		/// <summary>
		/// 获取正在维修的记录ID
		/// </summary>
		/// <returns></returns>
		decimal? GetRepairID(decimal equipId);

		/// <summary>
		/// 新增或更新设备维修记录并对设备以及维修配件信息做相应操作
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<BaseResult> AddOrModifyData(SfcsEquipRepairHeadAddOrModifyModel item);
	}
}