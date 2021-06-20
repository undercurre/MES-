/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-28 10:51:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SysPdaMenusRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
    /// <summary>
    /// 
    /// </summary>
    public class SysPdaMenusRepository : BaseRepository<SysPdaMenus, Decimal>, ISysPdaMenusRepository
    {
        public SysPdaMenusRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
        {
            string sql = "SELECT ENABLED FROM SYS_PDA_MENUS WHERE ID=:ID";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
            {
                ID = id,
            });

            return result == "Y" ? true : false;
        }

        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
		public async Task<decimal> ChangeEnableStatus(decimal id, bool status)
        {
            string sql = "UPDATE SYS_PDA_MENUS set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT SYS_PDA_MENUS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
        {
            string sql = "select count(0) from SYS_PDA_MENUS where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SysPdaMenusModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SYS_PDA_MENUS 
					(ID,MENU_ID,MENU_NAME,MODULE_NAME,DESCRIPTION,PARAM_INFO,ORDER_SEQ,FORM_NAME,CREATE_BY,CREATE_DATE,UPDATE_BY,UPDATE_DATE,ENABLED) 
					VALUES (:ID,:MENU_ID,:MENU_NAME,:MODULE_NAME,:DESCRIPTION,:PARAM_INFO,:ORDER_SEQ,:FORM_NAME,:CREATE_BY,SYSDate,:UPDATE_BY,SYSDate,:ENABLED)";
                    if (model.SysPdaMenus != null && model.SysPdaMenus.ID == 0)
                    {
                        var item = model.SysPdaMenus;
                        var newid = await GetSEQID();
                        var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = newid,
                            item.MENU_ID,
                            item.MENU_NAME,
                            item.MODULE_NAME,
                            item.DESCRIPTION,
                            item.PARAM_INFO,
                            item.ORDER_SEQ,
                            item.FORM_NAME,
                            CREATE_BY = item.User_Name,
                            UPDATE_BY = item.User_Name,
                            item.ENABLED,
                        }, tran);

                        //权限子表处理
                        if (model.RolesList != null && model.RolesList.InsertRecords != null && model.RolesList.InsertRecords.Count > 0)
                        {
                            string insert_rolesSql = @"insert into SYS_PDA_MENUS_ROLES(ID,MST_ID,ROLE_ID) 
                                       VALUES (SYS_PDA_MENUS_SEQ.NEXTVAL,:MST_ID,:ROLE_ID)";
                            foreach (var role in model.RolesList.InsertRecords)
                            {
                                await _dbConnection.ExecuteAsync(insert_rolesSql, new
                                {
                                    MST_ID = newid,
                                    role.ROLE_ID,
                                }, tran);
                            }
                        }
                    }
                    else if (model.SysPdaMenus != null && model.SysPdaMenus.ID > 0)
                    {
                        //更新
                        string updateSql = @"Update SYS_PDA_MENUS set MENU_ID=:MENU_ID,MENU_NAME=:MENU_NAME,MODULE_NAME=:MODULE_NAME,DESCRIPTION=:DESCRIPTION,
						   PARAM_INFO=:PARAM_INFO,ORDER_SEQ=:ORDER_SEQ,FORM_NAME=:FORM_NAME,UPDATE_BY=:UPDATE_BY,UPDATE_DATE=SYSDate,
						   ENABLED=:ENABLED  
						where ID=:ID ";
                        var item = model.SysPdaMenus;
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            item.ID,
                            item.MENU_ID,
                            item.MENU_NAME,
                            item.MODULE_NAME,
                            item.DESCRIPTION,
                            item.PARAM_INFO,
                            item.ORDER_SEQ,
                            item.FORM_NAME,
                            UPDATE_BY = item.User_Name,
                            item.ENABLED,
                        }, tran);

                        //权限子表新增
                        if (model.RolesList != null && model.RolesList.InsertRecords != null && model.RolesList.InsertRecords.Count > 0)
                        {
                            foreach (var insertRecord in model.RolesList.InsertRecords)
                            {
                                insertRecord.MST_ID = model.SysPdaMenus.ID;
                            }
                            string insert_rolesSql = @"insert into SYS_PDA_MENUS_ROLES(ID,MST_ID,ROLE_ID) 
                                       VALUES (SYS_PDA_MENUS_SEQ.NEXTVAL,:MST_ID,:ROLE_ID)";
                            await _dbConnection.ExecuteAsync(insert_rolesSql, model.RolesList.InsertRecords, tran);
                        }

                        //权限子表更新
                        if (model.RolesList != null && model.RolesList.UpdateRecords != null && model.RolesList.UpdateRecords.Count > 0)
                        {
                            string update_rolesSql = @"update SYS_PDA_MENUS_ROLES set ROLE_ID=:ROLE_ID Where ID=:ID";
                            await _dbConnection.ExecuteAsync(update_rolesSql, model.RolesList.UpdateRecords, tran);
                        }

                        //权限子表删除
                        if (model.RolesList != null && model.RolesList.RemoveRecords != null && model.RolesList.RemoveRecords.Count > 0)
                        {
                            string delete_rolesSql = @"Delete from SYS_PDA_MENUS_ROLES WHERE ID =:ID";
                            await _dbConnection.ExecuteAsync(delete_rolesSql, model.RolesList.RemoveRecords, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SYS_PDA_MENUS where ID=:ID ";
                    if (model.RemoveItem != null)
                    {
                        var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                        {
                            model.RemoveItem.ID
                        }, tran);

                        string delete_rolesSql = @"Delete from SYS_PDA_MENUS_ROLES WHERE MST_ID =:MST_ID";
                        await _dbConnection.ExecuteAsync(delete_rolesSql, new
                        {
                            MST_ID = model.RemoveItem.ID,
                        }, tran);
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
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
        /// 查询角色对应的PDA菜单列表
        /// </summary>
        /// <param name="role_id">角色ID</param>
        /// <returns></returns>
        public async Task<List<SysPdaMenusOfRoleID>> LoadPdaMenusByRole(decimal role_id)
        {
            string sql = @"Select NVL(a.ID,0) as ID, b.id AS MST_ID, b.MENU_NAME, NVL2(a.ID,1,0) as IsAuth  
                From  (SELECT * FROM SYS_PDA_MENUS_ROLES WHERE Role_id =:role_id) a right join SYS_PDA_MENUS b on a.mst_id = b.id  
                order by b.id ";
            return (await _dbConnection.QueryAsync<SysPdaMenusOfRoleID>(sql, new { role_id }))?.ToList();
        }

        /// <summary>
        /// 保存角色对应的授权数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveData2Role(SysPdaMenusOfRoleIDSave model)
        {
            int result = 1;
            if (model.MenusList == null || model.MenusList.Count == 0)
            {
                return -1;
            }
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增权限子表
                    var insertItem = model.MenusList.Where(t => t.ID == 0 && t.IsAuth == 1).ToList();
                    if (insertItem != null && insertItem.Count > 0)
                    {
                        string insert_rolesSql = @"insert into SYS_PDA_MENUS_ROLES(ID, MST_ID, ROLE_ID) 
                                       VALUES (SYS_PDA_MENUS_SEQ.NEXTVAL, :MST_ID, :ROLE_ID)";
                        foreach (var role in insertItem)
                        {
                            await _dbConnection.ExecuteAsync(insert_rolesSql, new
                            {
                                role.MST_ID,
                                model.ROLE_ID,
                            }, tran);
                        }
                    }
                    //删除权限子表
                    var deleteItem = model.MenusList.Where(t => t.ID > 0 && t.IsAuth == 0).ToList();
                    if (deleteItem != null && deleteItem.Count > 0)
                    {
                        string delete_rolesSql = @"Delete from SYS_PDA_MENUS_ROLES WHERE ID =:ID";
                        await _dbConnection.ExecuteAsync(delete_rolesSql, deleteItem, tran);
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
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

    }
}