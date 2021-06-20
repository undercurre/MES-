/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-22 09:40:14                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsHoldProductHeaderRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JZ.IMS.IRepository
{
    public interface ISfcsHoldProductHeaderRepository : IBaseRepository<SfcsHoldProductHeader, Decimal>
    {   
		/// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetSEQID();

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

		/// <summary>
		/// 產品流水號 前鎖定的數據
		/// </summary>
		/// <param name="sn"></param>
		/// <returns></returns>
		Task<List<SfcsHoldProductDetailVListModel>> GetHoldDetailViewAddByOldSN(string sn);

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(UnLockBillSaveModel model);

		/// <summary>
		/// 解锁产品之解除产品锁定
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<UnLockProductSaveReturn> UnLockProductSave(UnLockProductSaveModel model);
	}
}