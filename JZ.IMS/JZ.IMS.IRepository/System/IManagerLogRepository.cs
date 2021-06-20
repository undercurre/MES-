/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：操作日志                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-01-05 17:54:04                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IManagerLogRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.WebApi.Public;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface IManagerLogRepository : IBaseRepository<Sys_Manager_Log, decimal>
    {
		/// <summary>
		/// 逻辑删除返回影响的行数
		/// </summary>
		/// <param name="ids">需要删除的主键数组</param>
		/// <returns>影响的行数</returns>
		decimal DeleteLogical(decimal[] ids);
        /// <summary>
        /// 逻辑删除返回影响的行数（异步操作）
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        Task<decimal> DeleteLogicalAsync(decimal[] ids);

		Task<decimal> GetSEQIDAsync();

		Task<bool> SaveErrorLog(ErrorInfoClass model);
	}
}