/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员角色                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2018-12-18 13:28:43                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IManagerRoleRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JZ.IMS.IRepository
{
    public interface IManagerRoleRepository : IBaseRepository<Sys_Manager_Role, decimal>
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
        /// 根据主键获取名称
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>名称</returns>
        string GetNameById(decimal id);
        /// <summary>
        /// 根据主键获取名称（异步操作）
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>名称</returns>
        Task<string> GetNameByIdAsync(decimal id);

		/// <summary>
		/// 事务新增,并保存关联表数据
		/// </summary>
		/// <param name="model">实体对象</param>
		/// <returns></returns>
		decimal? InsertByTrans(Sys_Manager_Role model);

		/// <summary>
		/// 事务修改，并保存关联表数据
		/// </summary>
		/// <param name="model">实体对象</param>
		/// <returns></returns>
		decimal UpdateByTrans(Sys_Manager_Role model);

        /// <summary>
        /// 通过角色ID获取角色分配的菜单列表
        /// </summary>
        /// <param name="roleId">角色主键</param>
        /// <returns></returns>
        List<Sys_Menu> GetMenusByRoleId(decimal roleId);

		/// <summary>
		/// 通过角色ID获取角色分配的按钮列表
		/// </summary>
		/// <param name="roleId">角色主键</param>
		/// <returns></returns>
		List<Sys_Menu> GetAllButtonByRoleId(decimal roleId);

		Task<decimal> GetSEQIDAsync();

		/// <summary>
		/// 角色是否已使用
		/// </summary>
		/// <param name="id">角色id</param>
		/// <returns></returns>
		bool RoleIsByUsed(decimal id);
	}
}