/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-17 16:44:25                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtLineConfigRepository                                      
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
    public class SmtLineConfigRepository : BaseRepository<SmtLineConfig, Decimal>, ISmtLineConfigRepository
    {
        public SmtLineConfigRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_LINE_CONFIG WHERE ID=:ID";
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
            string sql = "UPDATE SMT_LINE_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
        {
            return false;
            string sql = "select count(0) from SMT_LINE_CONFIG where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
		/// 线别列表
		/// </summary>
		/// <returns></returns>
		public async Task<List<IDNAME>> GetLineList()
        {
            List<IDNAME> result = null;

            string sql = @"SELECT ID, Line_Name NAME FROM SMT_LINES  order by Line_Name";
            var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
		/// 配置类型列表
		/// </summary>
		/// <returns></returns>
		public async Task<List<IDNAME>> GetConfigTypeList()
        {
            List<IDNAME> result = null;

            string sql = @"SELECT CODE AS ID, CN_DESC NAME FROM SMT_LOOKUP WHERE TYPE = 'LINECONFIG'  order by CN_DESC ";
            var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 导出分页分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetExportData(SmtLineConfigRequestModel model)
        {
            string sql = @"  SELECT ROW_NUMBER() OVER(ORDER BY SLC.ID DESC) AS ROWNO, SLC.ID,SL.LINE_NAME LINE_ID,LP.CN_DESC CONFIG_TYPE,SLC.VALUE,SLC.DESCRIPTION,SLC.ENABLED FROM SMT_LINE_CONFIG SLC
                             LEFT JOIN SMT_LINES SL ON SL.ID=SLC.LINE_ID
                             LEFT JOIN SMT_LOOKUP LP ON LP.CODE=SLC.CONFIG_TYPE AND LP.TYPE='LINECONFIG' ";

            string conditions = " WHERE SLC.ID > 0 ";
      
            if (model.LINE_ID != null && model.LINE_ID > 0)
            {
                conditions += $" and (SLC.LINE_ID =:LINE_ID) ";
            }
            if (model.CONFIG_TYPE != null && model.CONFIG_TYPE > 0)
            {
                conditions += $" and (SLC.CONFIG_TYPE =:CONFIG_TYPE) ";
            }


            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(SLC.ID) FROM  SMT_LINE_CONFIG SLC
                               LEFT JOIN SMT_LINES SL ON SL.ID=SLC.LINE_ID
                               LEFT JOIN SMT_LOOKUP LP ON LP.CODE=SLC.CONFIG_TYPE AND LP.TYPE='LINECONFIG' " + conditions;

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
        public async Task<decimal> SaveDataByTrans(SmtLineConfigModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO SMT_LINE_CONFIG 
					(ID,LINE_ID,CONFIG_TYPE,VALUE,DESCRIPTION,ENABLED) 
					VALUES (:ID,:LINE_ID,:CONFIG_TYPE,:VALUE,:DESCRIPTION,:ENABLED)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.LINE_ID,
                                item.CONFIG_TYPE,
                                item.VALUE,
                                item.DESCRIPTION,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_LINE_CONFIG set LINE_ID=:LINE_ID, CONFIG_TYPE=:CONFIG_TYPE, VALUE=:VALUE, DESCRIPTION=:DESCRIPTION, ENABLED=:ENABLED  
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.LINE_ID,
                                item.CONFIG_TYPE,
                                item.VALUE,
                                item.DESCRIPTION,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SMT_LINE_CONFIG where ID=:ID ";
                    if (model.removeRecords != null && model.removeRecords.Count > 0)
                    {
                        foreach (var item in model.removeRecords)
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
    }
}