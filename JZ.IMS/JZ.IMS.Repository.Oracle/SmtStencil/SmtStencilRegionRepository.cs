/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-05 09:34:54                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtStencilRegionRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using JZ.IMS.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtStencilRegionRepository : BaseRepository<SmtStencilRegion, Decimal>, ISmtStencilRegionRepository
    {
        public SmtStencilRegionRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_STENCIL_REGION WHERE ID=:ID";
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
            string sql = "UPDATE SMT_STENCIL_REGION set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        /// 获取区间状态集
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetStatusList()
        {
            List<CodeName> result = null;

            string sql = @"SELECT CODE, CN_DESC as NAME 
                            FROM SMT_LOOKUP
                            WHERE TYPE = 'STENCIL_STATUS'
	                            AND ENABLED = 'Y'";
            var tmpdata = await _dbConnection.QueryAsync<CodeName>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取钢网类型集
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetStencilTypeList()
        {
            List<CodeName> result = null;

            string sql = @"SELECT TO_NUMBER(CODE) AS CODE, VALUE as NAME  
                            FROM SMT_LOOKUP
                            WHERE TYPE = 'STENCIL_NAME'
	                            AND ENABLED = 'Y'";
            var tmpdata = await _dbConnection.QueryAsync<CodeName>(sql);

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
        public async Task<TableDataModel> GetExportData(SmtStencilRegionRequestModel model)
        {
            string sql = @"  SELECT ROW_NUMBER() OVER(ORDER BY SSR.ID DESC) AS ROWNO,  SSR.ID, LP1.VALUE STENCIL_ID, SSR.BEGIN_COUNT, SSR.END_COUNT,LP2.CN_DESC BETWEEN_STATUS, LP3.CN_DESC OUTSIDE_STATUS, SSR.ORDER_NO, SSR.DESCRIPTION FROM SMT_STENCIL_REGION SSR
                             LEFT JOIN SMT_LOOKUP LP1 ON LP1.TYPE = 'STENCIL_NAME' AND LP1.ENABLED = 'Y' AND TO_NUMBER(LP1.CODE)=SSR.STENCIL_ID
                             LEFT JOIN SMT_LOOKUP LP2 ON  LP2.TYPE = 'STENCIL_STATUS' AND LP2.ENABLED = 'Y' AND LP2.CODE=SSR.BETWEEN_STATUS
                             LEFT JOIN SMT_LOOKUP LP3 ON  LP3.TYPE = 'STENCIL_STATUS' AND LP3.ENABLED = 'Y' AND LP3.CODE=SSR.OUTSIDE_STATUS ";

            string conditions = " WHERE SSR.ID>0 ";

            if (!string.IsNullOrWhiteSpace(model.Key))
            {
                conditions += $" AND SSR.STENCIL_ID =:Key";
            }


            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(SSR.ID) FROM SMT_STENCIL_REGION SSR
                               LEFT JOIN SMT_LOOKUP LP1 ON LP1.TYPE = 'STENCIL_NAME' AND LP1.ENABLED = 'Y' AND TO_NUMBER(LP1.CODE)=SSR.STENCIL_ID
                               LEFT JOIN SMT_LOOKUP LP2 ON  LP2.TYPE = 'STENCIL_STATUS' AND LP2.ENABLED = 'Y' AND LP2.CODE=SSR.BETWEEN_STATUS
                               LEFT JOIN SMT_LOOKUP LP3 ON  LP3.TYPE = 'STENCIL_STATUS' AND LP3.ENABLED = 'Y' AND LP3.CODE=SSR.OUTSIDE_STATUS
                             " + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SmtStencilRegionModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO SMT_STENCIL_REGION(ID, STENCIL_ID, BEGIN_COUNT, END_COUNT, BETWEEN_STATUS, OUTSIDE_STATUS, ORDER_NO, DESCRIPTION) 
					    VALUES (:ID, :STENCIL_ID, :BEGIN_COUNT, :END_COUNT, :BETWEEN_STATUS, :OUTSIDE_STATUS, :ORDER_NO, :DESCRIPTION)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.STENCIL_ID,
                                item.BEGIN_COUNT,
                                item.END_COUNT,
                                item.BETWEEN_STATUS,
                                item.OUTSIDE_STATUS,
                                item.ORDER_NO,
                                item.DESCRIPTION,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"update smt_stencil_region set STENCIL_ID =:STENCIL_ID, BEGIN_COUNT =:BEGIN_COUNT, END_COUNT =:END_COUNT,
							BETWEEN_STATUS =:BETWEEN_STATUS, OUTSIDE_STATUS =:OUTSIDE_STATUS, ORDER_NO =:ORDER_NO, DESCRIPTION =:DESCRIPTION 
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.STENCIL_ID,
                                item.BEGIN_COUNT,
                                item.END_COUNT,
                                item.BETWEEN_STATUS,
                                item.OUTSIDE_STATUS,
                                item.ORDER_NO,
                                item.DESCRIPTION,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"delete from smt_stencil_region where ID=:ID ";
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