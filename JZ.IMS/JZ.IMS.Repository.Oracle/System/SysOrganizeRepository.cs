/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：组织架构表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-05-05 11:05:54                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SysOrganizeRepository                                      
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
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SysOrganizeRepository : BaseRepository<SysOrganize, decimal>, ISysOrganizeRepository
    {
        public SysOrganizeRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SYS_ORGANIZE WHERE ID=:ID";
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
            string sql = "UPDATE SYS_ORGANIZE set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SYS_ORGANIZE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SYS_USER_ORGANIZE where ORGANIZE_ID =:id";
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
        public async Task<decimal> SaveDataByTrans(SysOrganizeModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SYS_ORGANIZE 
					(ID,ORGANIZE_NAME,ORGANIZE_TYPE_ID,PARENT_ORGANIZE_ID,ENABLED,REMARK,CREATE_BY,CREATE_TIME,MODIFY_BY,MODIFY_TIME) 
					VALUES (:ID,:ORGANIZE_NAME,:ORGANIZE_TYPE_ID,:PARENT_ORGANIZE_ID,:ENABLED,:REMARK,:User_Name,SYSDate,:User_Name,SYSDate)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.ORGANIZE_NAME,
                                item.ORGANIZE_TYPE_ID,
                                item.PARENT_ORGANIZE_ID,
                                item.ENABLED,
                                item.REMARK,
                                item.User_Name,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SYS_ORGANIZE set ORGANIZE_NAME=:ORGANIZE_NAME,ORGANIZE_TYPE_ID=:ORGANIZE_TYPE_ID, PARENT_ORGANIZE_ID=:PARENT_ORGANIZE_ID,
							ENABLED=:ENABLED,REMARK=:REMARK,MODIFY_BY=:MODIFY_BY,MODIFY_TIME=SYSDate  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.ORGANIZE_NAME,
                                item.ORGANIZE_TYPE_ID,
                                item.PARENT_ORGANIZE_ID,
                                item.ENABLED,
                                item.REMARK,
                                MODIFY_BY = item.User_Name,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SYS_ORGANIZE where ID=:ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.ID
                            }, tran);
                        }
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
        /// 获取组织架构人员关联数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadUserOrganize(SysUserOrganizeRequestModel model)
        {
            string conditions = " WHERE uo.ID > 0 ";
            if ((model.ID ?? 0) > 0)
            {
                conditions += $"and (uo.ID =:ID) ";
            }
            if ((model.MANAGER_ID ?? 0) > 0)
            {
                conditions += $"and (uo.MANAGER_ID =:MANAGER_ID) ";
            }
            if (!model.USER_NAME.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(sm.USER_NAME, :USER_NAME) > 0) ";
            }
            if ((model.ORGANIZE_ID ?? 0) > 0)
            {
                conditions += $"and (uo.ORGANIZE_ID =:ORGANIZE_ID) ";
            }
            if (!model.STATUS.IsNullOrWhiteSpace())
            {
                conditions += $"and (uo.STATUS =:STATUS) ";
            }

            string sql = @"select ROWNUM AS ROWNO, uo.id, uo.manager_id, uo.organize_id, uo.start_date, uo.creator, uo.end_date, uo.status, sm.USER_NAME,
                               sm.NICK_NAME, oz.ORGANIZE_NAME 
				           from sys_user_organize uo left join Sys_Manager sm on uo.manager_id = sm.ID 
                           left join SYS_ORGANIZE oz on uo.organize_id = oz.ID ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "uo.id", conditions);
            var resdata = await _dbConnection.QueryAsync<SysUserOrganizeListModel>(pagedSql, model);

            string sqlcnt = @" select count(0) From sys_user_organize uo left join Sys_Manager sm on uo.manager_id = sm.ID " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 组织架构人员关联 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveUserOrganize(SysUserOrganizeModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SYS_USER_ORGANIZE 
					(ID,MANAGER_ID,ORGANIZE_ID,START_DATE,CREATOR,END_DATE,STATUS) 
					VALUES (:ID,:MANAGER_ID,:ORGANIZE_ID,SYSDate,:CREATOR,:END_DATE,:STATUS)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.MANAGER_ID,
                                item.ORGANIZE_ID,
                                item.CREATOR,
                                item.END_DATE,
                                item.STATUS,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SYS_USER_ORGANIZE set MANAGER_ID=:MANAGER_ID,ORGANIZE_ID=:ORGANIZE_ID,CREATOR=:CREATOR,END_DATE=:END_DATE,
										    STATUS=:STATUS  
						                 Where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.MANAGER_ID,
                                item.ORGANIZE_ID,
                                item.CREATOR,
                                item.END_DATE,
                                item.STATUS,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SYS_USER_ORGANIZE where ID=:ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.ID
                            }, tran);
                        }
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
        /// 获取组织结构
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SysOrganizeListModel GetOrganize(int ID)
        {
            string sql = "SELECT * FROM SYS_ORGANIZE WHERE ID = :ID";
            var result = _dbConnection.Query<SysOrganizeListModel>(sql, new
            {
                ID = ID
            }).FirstOrDefault();

            return result;
        }
    }
}