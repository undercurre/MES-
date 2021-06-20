/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：组织架构表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-05 11:05:54                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISysOrganizeRepository                                      
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
    public interface ISysOrganizeRepository : IBaseRepository<SysOrganize, decimal>
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
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SysOrganizeModel model);

        /// <summary>
        /// 获取组织架构人员关联数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> LoadUserOrganize(SysUserOrganizeRequestModel model);

        /// <summary>
        /// 组织架构人员关联 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveUserOrganize(SysUserOrganizeModel model);

        /// <summary>
        /// 组织架构人员关联 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        SysOrganizeListModel GetOrganize(int ID);

    }
}