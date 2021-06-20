/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:48                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsProductConfigRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using JZ.IMS.ViewModels.ProductBasicSet.ComponentReplace;

namespace JZ.IMS.IRepository
{
    public interface ISfcsProductConfigRepository : IBaseRepository<SfcsProductConfig, Decimal>
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
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetProductConfig(SfcsProductConfigRequestModel model);

        /// <summary>
        /// 根据料号判断是否存在重复的配置类型
        /// </summary>
        /// <param name="partno"></param>
        /// <returns></returns>
        Task<bool> ConfigTypeIsExistByPartNo(string partno, decimal? configtype);


        /// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetExportData(SfcsProductConfigRequestModel model);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SfcsProductConfigModel model);

        /// <summary>
        /// 获取根据料号配置的制程
        /// </summary>
        /// <param name="part_no">料号</param>
        /// <returns></returns>
        decimal GetRouteIdByPartNo(string part_no);

        /// <summary>
        /// 零件替代的保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByOldComponents(SfcsReplaceModel<ComponentReplaceViewModel> model);

        /// <summary>
        /// 获取零件信息
        /// </summary>
        /// <param name="ComponentName"></param>
        /// <returns></returns>
        Task<List<ImsPart>> GetDataByOldComponents(String partNo);
    }
}