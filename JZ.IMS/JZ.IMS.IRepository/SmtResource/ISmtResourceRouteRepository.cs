/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-02-27 14:08:53                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtResourceRouteRepository                                      
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
    public interface ISmtResourceRouteRepository : IBaseRepository<SmtResourceRoute, Decimal>
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
		/// 获取辅料名称
		/// </summary>
		/// <returns></returns>
		Task<List<IDNAME>> GetNameAsync();

        /// <summary>
		/// 获取辅料工序列表
		/// </summary>
		/// <returns></returns>
		Task<List<IDNAME>> GetProcessAsync();

        /// <summary>
		/// 获取特性列表
		/// </summary>
		/// <returns></returns>
		Task<List<IDNAME>> GetPropertyAsync();

        /// <summary>
		/// 获取辅料类型列表
		/// </summary>
		/// <returns></returns>
		Task<List<IDNAME>> GetCategoryAsync();

        /// <summary>
        /// 巡检配置项目是否已被使用 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SmtResourceRouteModel model);

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetExportData(SmtResourceRouteRequestModel model);
    }
}