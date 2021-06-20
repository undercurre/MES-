/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理菜单                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-01-05 17:54:04                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMenuRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface IMenuRepository : IBaseRepository<Sys_Menu, decimal>
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<decimal> DeleteSubAsync(decimal id);

		/// <summary>
		/// 根据主键获取显示状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns></returns>
		Task<Boolean> GetDisplayStatusByIdAsync(decimal id);

        /// <summary>
        /// 修改显示状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
        Task<decimal> ChangeDisplayStatusByIdAsync(decimal id, bool status);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="Name">别名</param>
        /// <returns></returns>
        Task<Boolean> IsExistsNameAsync(string Name);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="Name">别名</param>
        /// <param name="Id">主键</param>
        /// <returns></returns>
        Task<Boolean> IsExistsNameAsync(string Name, decimal Id);

		/// <summary>
		/// 是否存在
		/// </summary>
		/// <param name="LinkUrl">LinkUrl</param>
		/// <returns></returns>
		Task<Boolean> IsExistsLinkUrlAsync(string LinkUrl);

		/// <summary>
		/// 是否存在
		/// </summary>
		/// <param name="LinkUrl">LinkUrl</param>
		/// <param name="Id">主键</param>
		/// <returns></returns>
		Task<Boolean> IsExistsLinkUrlAsync(string LinkUrl, decimal Id);

		Task<decimal> GetSEQIDAsync();
	}
}