/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-03 10:54:23                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsScraperConfigRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface ISfcsScraperConfigRepository : IBaseRepository<SfcsScraperConfig, decimal>
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
		///刮刀号是否已被使用 
		/// </summary>
		/// <param name="SCRAPER_NO">刮刀号</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(string SCRAPER_NO);

        /// <summary>
		/// 查询列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadData(SfcsScraperConfigRequestModel model);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SfcsScraperConfigModel model);
    }
}