/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-30 10:03:34                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsLookupsRepository                                      
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
    public class SfcsLookupsRepository : BaseRepository<SfcsLookups, Decimal>, ISfcsLookupsRepository
    {
        public SfcsLookupsRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_LOOKUPS WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_LOOKUPS set ENABLED=:ENABLED WHERE ID=:Id";
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

		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
		{
			string sql = "select count(0) from SFCS_LOOKUPS where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});
            return (Convert.ToInt32(result) > 0);
        }

        /// 获取生产类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetProductKind()
        {
            string sql = @"SELECT LOOKUP_CODE AS Code, MEANING AS NAME 
                              FROM SFCS_PARAMETERS
                             WHERE LOOKUP_TYPE = 'VARIOUS_DATA_TYPE' ORDER BY MEANING asc "; //AND  CHINESE IS NOT NULL
            return (await _dbConnection.QueryAsync<CodeName>(sql))?.ToList();
        }

        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetExportData(SfcsLookupsRequestModel model)
        {
            string conditions = " WHERE lp.ID > 0 ";

            if (model.KIND != null && model.KIND > 0)
            {
                conditions += $"and lp.KIND =:KIND ";
            }
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(lp.CODE, :Key) > 0 or instr(lp.DESCRIPTION, :Key) > 0 or instr(lp.CHINESE, :Key) > 0) ";
            }

            string sql = @"SELECT ROWNUM AS ROWNO, lp.id, pm.CHINESE AS kind, lp.code, lp.description, lp.chinese, lp.category, lp.enabled 
                           From sfcs_lookups lp  
                           INNER JOIN SFCS_PARAMETERS pm ON lp.KIND = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'VARIOUS_DATA_TYPE' ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "lp.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<SfcsLookupsExportModel>(pagedSql, model);
            string sqlcnt = @"SELECT COUNT(0) From sfcs_lookups lp  
                           INNER JOIN SFCS_PARAMETERS pm ON lp.KIND = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'VARIOUS_DATA_TYPE' " + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadData(SfcsLookupsRequestModel model)
        {
            string conditions = " WHERE lk.ID > 0 ";

            if (model.KIND != null && model.KIND > 0)
            {
                conditions += $"and lk.KIND =:KIND ";
            }

            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(lk.CODE, :Key) > 0 or instr(lk.DESCRIPTION, :Key) > 0 or instr(lk.CHINESE, :Key) > 0) ";
            }
            string sql = @"select ROWNUM as rowno, par.MEANING as TypeName, lk.* from Sfcs_Lookups lk 
                            left join SFCS_PARAMETERS par on lk.KIND=par.LOOKUP_CODE and LOOKUP_TYPE = 'VARIOUS_DATA_TYPE' ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "lk.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @"SELECT COUNT(0) from Sfcs_Lookups lk 
                            left join SFCS_PARAMETERS par on lk.KIND=par.LOOKUP_CODE and LOOKUP_TYPE = 'VARIOUS_DATA_TYPE' " + conditions;

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
        public async Task<decimal> SaveDataByTrans(SfcsLookupsModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_LOOKUPS 
					(ID,KIND,CODE,DESCRIPTION,CHINESE,CATEGORY,ENABLED) 
					VALUES (:ID,:KIND,:CODE,:DESCRIPTION,:CHINESE,:CATEGORY,:ENABLED)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.KIND,
                                item.CODE,
                                item.DESCRIPTION,
                                item.CHINESE,
                                item.CATEGORY,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_LOOKUPS set KIND=:KIND,CODE=:CODE,DESCRIPTION=:DESCRIPTION,CHINESE=:CHINESE,CATEGORY=:CATEGORY,ENABLED=:ENABLED   
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.KIND,
                                item.CODE,
                                item.DESCRIPTION,
                                item.CHINESE,
                                item.CATEGORY,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //删除
                    //string deleteSql = @"Delete from SFCS_LOOKUPS where ID=:ID ";
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