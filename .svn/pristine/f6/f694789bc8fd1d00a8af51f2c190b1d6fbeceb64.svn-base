/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-05 09:21:49                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtStencilConfigRepository                                      
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
    public interface ISmtStencilConfigRepository : IBaseRepository<SmtStencilConfig, Decimal>
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
		///钢网号是否已被使用 
		/// </summary>
		/// <param name="STENCIL_NO">钢网号</param>
		/// <param name="STENCIL_ID">钢网ID</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(string STENCIL_NO, decimal STENCIL_ID);

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SmtStencilConfigModel model);

        /// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadData(SmtStencilConfigRequestModel model);

        /// <summary>
        /// PDA钢网查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> LoadDataPDA(SmtStencilPDARequestModel model);

        /// <summary>
        /// 保存钢网资源数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> SaveStencilResourceInfo(SmtStencilResourceListModel model);

        /// <summary>
        /// 根据钢网的资源ID获取钢网资源数据
        /// </summary>
        /// <param name="resource_id">钢网的资源ID</param>
        /// <returns></returns>
        Task<SmtStencilResourceListModel> GetStencilResourceInfo(string resource_id);

        /// <summary>
        /// 获取钢网资源列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IEnumerable<SmtStencilResourceListModel>> GetStencilResourceList(SmtStencilResourceRequestModel model);

        /// <summary>
		/// 获取钢网资源列表条数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetStencilResourceListCount(SmtStencilResourceRequestModel model);
    }
}