/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-31 16:22:05                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsAllObjectsRepository                                      
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
    public class SfcsAllObjectsRepository : BaseRepository<SfcsAllObjects, Decimal>, ISfcsAllObjectsRepository
    {
        public SfcsAllObjectsRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_ALL_OBJECTS WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_ALL_OBJECTS set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT MES_SEQ_ID.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_ALL_OBJECTS where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 获取采集类型种类
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetBjectCategory()
        {
            string sql = @"SELECT LOOKUP_CODE as Code, CHINESE as NAME 
                           FROM SFCS_PARAMETERS
                           WHERE LOOKUP_TYPE = 'OBJECT_CATEGORY' AND ENABLED = 'Y'";
            return (await _dbConnection.QueryAsync<CodeName>(sql))?.ToList();   //CHINESE||'   '||ENABLED 
        }

        /// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SfcsAllObjectsRequestModel model)
        {
            string conditions = " WHERE m.ID > 0 ";
            if (model.OBJECT_CATEGORY != null && model.OBJECT_CATEGORY > 0)
            {
                conditions += $"and m.OBJECT_CATEGORY = :OBJECT_CATEGORY ";
            }
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(m.OBJECT_NAME, :Key) > 0 or instr(m.OBJECT_MARK, :Key) > 0 or instr(m.DESCRIPTION, :Key) > 0)";
            }

            string sql = @"SELECT ROWNUM AS ROWNO,m.ID,m.OBJECT_NAME,m.OBJECT_MARK,pm.CHINESE as OBJECT_CATEGORY,m.ISACTIVE,m.DESCRIPTION  
                           From SFCS_ALL_OBJECTS m  
                           LEFT JOIN SFCS_PARAMETERS pm ON m.OBJECT_CATEGORY = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'OBJECT_CATEGORY' AND pm.ENABLED = 'Y' ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
            string sqlcnt = @"SELECT COUNT(0) From SFCS_ALL_OBJECTS m  
                           LEFT JOIN SFCS_PARAMETERS pm ON m.OBJECT_CATEGORY = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'OBJECT_CATEGORY' AND pm.ENABLED = 'Y' " + conditions;

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
        public async Task<decimal> SaveDataByTrans(SfcsAllObjectsModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_ALL_OBJECTS 
					(ID,OBJECT_NAME,OBJECT_MARK,OBJECT_CATEGORY,ISACTIVE,DESCRIPTION) 
					VALUES (:ID,:OBJECT_NAME,:OBJECT_MARK,:OBJECT_CATEGORY,:ISACTIVE,:DESCRIPTION)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.OBJECT_NAME,
                                item.OBJECT_MARK,
                                item.OBJECT_CATEGORY,
                                item.ISACTIVE,
                                item.DESCRIPTION,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_ALL_OBJECTS set OBJECT_NAME=:OBJECT_NAME,OBJECT_MARK=:OBJECT_MARK,OBJECT_CATEGORY=:OBJECT_CATEGORY, 
                                         ISACTIVE=:ISACTIVE,DESCRIPTION=:DESCRIPTION 
						                 where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.OBJECT_NAME,
                                item.OBJECT_MARK,
                                item.OBJECT_CATEGORY,
                                item.ISACTIVE,
                                item.DESCRIPTION,
                            }, tran);
                        }
                    }
                    //删除
                    //string deleteSql = @"Delete from SFCS_ALL_OBJECTS where ID=:ID ";
                    //if (model.removeRecords != null && model.removeRecords.Count > 0)
                    //{
                    //    foreach (var item in model.removeRecords)
                    //    {
                    //        var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                    //        {
                    //            item.ID
                    //        }, tran);
                    //    }
                    //}

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