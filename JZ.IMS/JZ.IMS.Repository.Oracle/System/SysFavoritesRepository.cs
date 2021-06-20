/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：收藏夹表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-28 14:22:27                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SysFavoritesRepository                                      
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
    public class SysFavoritesRepository : BaseRepository<SysFavorites, Decimal>, ISysFavoritesRepository
    {
        public SysFavoritesRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SYS_FAVORITES WHERE ID=:ID";
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
            string sql = "UPDATE SYS_FAVORITES set ENABLED=:ENABLED WHERE ID=:Id";
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
		///加载数据
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadData(SysFavoritesRequestModel model)
        {
            string conditions = " WHERE sf.ID > 0 ";
            if ((model.ID ?? 0) > 0)
            {
                conditions += $"and (sf.ID =:ID) ";
            }
            if ((model.MENUM_ID ?? 0) > 0)
            {
                conditions += $"and (sf.MENUM_ID =:MENUM_ID) ";
            }
            if (!model.USER_ID.IsNullOrWhiteSpace())
            {
                conditions += $"and (sf.USER_ID =:USER_ID) ";
            }
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(sm.Menu_Name,:Menu_Name) > 0)";
            }
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(sm.MENU_EN,:MENU_EN) > 0)";
            }

            string sql = @"select ROWNUM AS ROWNO, sf.id, sf.menum_id, sf.menum_path, sf.user_id, sf.enable, sf.sort_num, sm.Menu_Name, sm.MENU_EN 
				 From sys_favorites sf left join Sys_Menu sm on sf.MENUM_ID=sm.ID ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "sf.sort_num desc ", conditions);
            var resdata = await _dbConnection.QueryAsync<SysFavoritesListModel>(pagedSql, model);

            string sqlcnt = @" select count(0) From sys_favorites sf inner join Sys_Menu sm on sf.MENUM_ID= sm.ID  " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SysFavoritesModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SYS_FAVORITES 
					(ID,MENUM_ID,MENUM_PATH,USER_ID,ENABLE,SORT_NUM) 
					VALUES (:ID,:MENUM_ID,:MENUM_PATH,:USER_ID,:ENABLE,SYS_FAVORITES_SORT_SEQ.NEXTVAL)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var res = await _dbConnection.QueryAsync<dynamic>("Select * from SYS_FAVORITES Where MENUM_PATH=:MENUM_PATH and USER_ID=:USER_ID", new
                            {
                                item.MENUM_PATH,
                                item.USER_ID
                            }, tran);

                            if (res == null || res.Count() == 0)
                            {
                                var newid = await GetSEQID();
                                var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                                {
                                    ID = newid,
                                    item.MENUM_ID,
                                    item.MENUM_PATH,
                                    item.USER_ID,
                                    item.ENABLE,

                                }, tran);
                            }
                        }
                    }
                    //更新
                    //string updateSql = @"Update SYS_FAVORITES set MENUM_ID=:MENUM_ID,MENUM_PATH=:MENUM_PATH,USER_ID=:USER_ID,ENABLE=:ENABLE  
                    //	where ID=:ID ";
                    //if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    //{
                    //	foreach (var item in model.UpdateRecords)
                    //	{
                    //		var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                    //		{
                    //			item.ID,
                    //			item.MENUM_ID,
                    //			item.MENUM_PATH,
                    //			item.USER_ID,
                    //			item.ENABLE,

                    //		}, tran);
                    //	}
                    //}
                    //删除
                    string deleteSql = @"Delete from SYS_FAVORITES Where MENUM_PATH=:MENUM_PATH and USER_ID=:USER_ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.MENUM_PATH,
                                item.USER_ID,
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
    }
}