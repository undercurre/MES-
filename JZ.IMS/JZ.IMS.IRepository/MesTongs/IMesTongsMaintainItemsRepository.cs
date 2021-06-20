/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具保养事项表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-20 15:51:29                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesTongsMaintainItemsRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface IMesTongsMaintainItemsRepository : IBaseRepository<MesTongsMaintainItems, Decimal>
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
		/// 判断当前事项是否在夹具中使用
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> IsExistsByTongsInfo(decimal id);
	}
}