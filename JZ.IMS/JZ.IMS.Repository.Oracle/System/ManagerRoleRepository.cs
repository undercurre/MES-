/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员角色接口实现                                                    
*│　作    者：Admin                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2018-12-18 13:28:43                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ManagerRoleRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
	public class ManagerRoleRepository : BaseRepository<Sys_Manager_Role, decimal>, IManagerRoleRepository
	{
		public ManagerRoleRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		public decimal DeleteLogical(decimal[] ids)
		{
			string sql = "update SYS_Manager_Role set Is_Delete='Y' where Id in :Ids";
			return _dbConnection.Execute(sql, new
			{
				Ids = ids
			});
		}

		public async Task<decimal> DeleteLogicalAsync(decimal[] ids)
		{
			string sql = "update SYS_Manager_Role set Is_Delete='Y' where Id in :Ids";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				Ids = ids
			});
		}

		/// <summary>
		/// 角色是否已使用 
		/// </summary>
		/// <param name="id">角色id</param>
		/// <returns></returns>
		public bool RoleIsByUsed(decimal id)
		{
			string sql = "SELECT count(0) FROM SYS_Manager WHERE Is_Delete='N' and role_id = :id";
			object result = _dbConnection.ExecuteScalar(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		public string GetNameById(decimal id)
		{
			var item = Get(id);
			return item == null ? "角色不存在" : item.Role_Name;
		}

		public async Task<string> GetNameByIdAsync(decimal id)
		{
			var item = await GetAsync(id);
			return item == null ? "角色不存在" : item.Role_Name;

		}

		/// <summary>
		/// 事务修改
		/// </summary>
		/// <param name="model">实体对象</param>
		/// <returns></returns>
		public decimal? InsertByTrans(Sys_Manager_Role model)
		{
			decimal roleId = 0;
			string insertPermissionSql = @"INSERT INTO SYS_Role_Permission
                (id, Role_Id, Menu_Id) VALUES (sys_role_permission_seq.nextval,:RoleId,:MenuId)";
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					roleId = _dbConnection.Insert<decimal, Sys_Manager_Role>(model, tran);
					//roleId = _dbConnection.Insert(model, tran);
					if (roleId > 0 && model.Menu_Ids?.Count() > 0)
					{
						foreach (var item in model.Menu_Ids)
						{
							_dbConnection.Execute(insertPermissionSql, new
							{
								RoleId = roleId,
								MenuId = item,
							}, tran);
						}
					}
					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}

			return roleId;
		}

		/// <summary>
		/// 事务新增
		/// </summary>
		/// <param name="model">实体对象</param>
		/// <returns></returns>
		public decimal UpdateByTrans(Sys_Manager_Role model)
		{
			int result = 0;
			string insertPermissionSql = @"INSERT INTO SYS_Role_Permission
                (ID, Role_Id, Menu_Id) VALUES (sys_role_permission_seq.nextval,:RoleId,:MenuId)";
			string deletePermissionSql = "DELETE FROM SYS_Role_Permission WHERE Role_Id = :RoleId";
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					result = _dbConnection.Update(model, tran);
					if (result > 0 && model.Menu_Ids?.Count() > 0)
					{
						_dbConnection.Execute(deletePermissionSql, new
						{
							RoleId = model.Id,

						}, tran);
						foreach (var item in model.Menu_Ids)
						{
							_dbConnection.Execute(insertPermissionSql, new
							{
								RoleId = model.Id,
								MenuId = item,
							}, tran);
						}
					}
					tran.Commit();
				}
				catch (Exception ex)
				{
					tran.Rollback();
					throw ex;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}

			return result;
		}

		/// <summary>
		/// 通过角色ID获取角色分配的菜单列表
		/// </summary>
		/// <param name="roleId">角色主键</param>
		/// <returns></returns>
		public List<Sys_Menu> GetMenusByRoleId(decimal roleId)
		{
			string sql = @"SELECT m.Id, m.Parent_Id, m.Menu_Code, m.Menu_Name, m.Icon_Url, m.Link_Url, m.Sort, m.ENABLED, m.IS_SYSTEM, 
                m.Add_Manager_Id, m.Add_Time, m.Modify_Manager_Id, m.Modify_Time, m.Is_Delete, m.SPREAD, m.TARGET, m.MENU_EN  ,m.COLUMNS
			 FROM SYS_Role_Permission rp INNER JOIN SYS_Menu m ON rp.Menu_Id = m.Id 
		     WHERE  (rp.Role_Id = :RoleId) AND (m.Is_Delete = 'N') and (m.MENU_TYPE=1) ORDER BY m.Sort asc";
			return _dbConnection.Query<Sys_Menu>(sql, new
			{
				RoleId = roleId,
			}).ToList();
		}

		/// <summary>
		/// 通过角色ID获取角色分配的按钮列表
		/// </summary>
		/// <param name="roleId">角色主键</param>
		/// <returns></returns>
		public List<Sys_Menu> GetAllButtonByRoleId(decimal roleId)
		{
			string sql = @"SELECT m.Id, m.Parent_Id, m.Menu_Code, m.Menu_Name, m.Icon_Url, m.Link_Url, m.Sort, m.ENABLED, m.IS_SYSTEM, 
                m.Add_Manager_Id, m.Add_Time, m.Modify_Manager_Id, m.Modify_Time, m.MENU_TYPE, m.SPREAD, m.TARGET, m.MENU_EN ,m.COLUMNS
			 FROM SYS_Role_Permission rp INNER JOIN SYS_Menu m ON rp.Menu_Id = m.Id 
		     WHERE (rp.Role_Id = :RoleId) AND (m.Is_Delete = 'N') and (m.MENU_TYPE=2) ORDER BY m.Sort asc";
			return _dbConnection.Query<Sys_Menu>(sql, new
			{
				RoleId = roleId,
			}).ToList();
		}

		public async Task<decimal> GetSEQIDAsync()
		{
			string sql = "SELECT Sys_Manager_ROLE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}
	}
}