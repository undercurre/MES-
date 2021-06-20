/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备巡检配置                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-23 16:18:50                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IIpqaConfigRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface IIpqaConfigRepository : IBaseRepository<IpqaConfig, Decimal>
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
		/// 巡检项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> IpqaConfigIsByUsed(decimal id);

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		Task<decimal> GetSEQID();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(IpqaConfigModel model);

		/// <summary>
		/// 导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetExportData(IpqaConfigRequestModel model);
	}
}