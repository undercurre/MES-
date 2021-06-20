/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-25 09:05:16                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesProductionPreMstRepository                                      
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
    public interface IMesProductionPreMstRepository : IBaseRepository<MesProductionPreMst, Decimal>
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
		/// 线体列表
		/// </summary>
		/// <returns></returns>
		Task<List<IDNAME>> GetLineList();

		/// <summary>
		/// 明细列表
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<MesProductionPreDtlListModel>> GetProductionPreDtlList(decimal id);

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(MesProductionPreMstModel model);

		/// <summary>
		///删除 
		/// </summary>
		/// <param name="id">主表id</param>
		/// <returns></returns>
		Task<int> DeleteByTrans(decimal id);
	}
}