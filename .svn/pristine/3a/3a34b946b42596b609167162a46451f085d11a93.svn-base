/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限表                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-01-05 17:54:04                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IRolePermissionRepository                                      
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
	public interface IRolePermissionRepository : IBaseRepository<Sys_Role_Permission, decimal>
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
		/// 通过角色主键获取菜单主键数组
		/// </summary>
		/// <param name="RoleId"></param>
		/// <returns></returns>
		decimal[] GetIdsByRoleId(decimal RoleId);

		/// <summary>
		/// 通过角色主键获取有权限的菜单列表
		/// </summary>
		/// <param name="RoleId"></param>
		/// <returns></returns>
		List<Sys_Menu> GetMenuByRoleId(decimal RoleId);

		/// <summary>
		/// 角色信息查询
		/// </summary>
		/// <returns></returns>
		Task<List<dynamic>> GetRole();

		/// <summary>
		/// 角色用户关联信息查询
		/// </summary>
		/// <param name="Account"></param>
		/// <param name="Name"></param>
		/// <param name="RoleName"></param>
		/// <returns></returns>
		Task<TableDataModel> GetUserRoleInfo(string Account, string Name, string RoleName, string PageIndex, string PageSize);

		

		/// <summary>
		/// 用户查询
		/// </summary>
		/// <param name="Account"></param>
		/// <param name="Name"></param>
		/// <param name="OrganizeID"></param>
		/// <param name="OrganizeName"></param>
		/// <param name="PageIndex"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		Task<TableDataModel> GetUser(string Account, string Name, string OrganizeID, string OrganizeName, string PageIndex, string PageSize);

		/// <summary>
		/// 用户角色配置
		/// </summary>
		/// <param name="UserID"></param>
		/// <param name="RoleID"></param>
		/// <returns></returns>
		Task<bool> SetUserRole(string UserID, string RoleID);
	}
}