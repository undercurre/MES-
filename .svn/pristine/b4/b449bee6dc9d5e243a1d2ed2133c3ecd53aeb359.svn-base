/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-05 09:34:53                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtStencilPartRepository                                      
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
using System.Collections.Generic;
using JZ.IMS.ViewModels;
using System.Linq;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtStencilPartRepository : BaseRepository<SmtStencilPart, String>, ISmtStencilPartRepository
    {
        public SmtStencilPartRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_STENCIL_PART WHERE ID=:ID";
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
            string sql = "UPDATE SMT_STENCIL_PART set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        ///  获取产品规格
        /// </summary>
        /// <param name="PART_NO">产品编号</param>
        /// <returns></returns>
        public async Task<string> GetPNModel(string PART_NO)
        {
            string result = string.Empty;

            string sql = @"Select DESCRIPTION as CODE from IMS_PART where CODE = :PART_NO";
            var tmpdata = await _dbConnection.QueryAsync<CodeName>(sql, new { PART_NO });

            if (tmpdata != null)
            {
                result = tmpdata.Select(t => t.CODE).FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// 获取面板集
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetPCBSideList()
        {
            List<CodeName> result = null;

            string sql = @"SELECT 'A' AS CODE, '正面' AS NAME FROM DUAL
							UNION ALL
							SELECT 'B' AS CODE, '反面' AS NAME FROM DUAL
							UNION ALL
							SELECT 'N' AS CODE, '正(反)' AS NAME FROM DUAL";
            var tmpdata = await _dbConnection.QueryAsync<CodeName>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取产品型号集
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetPartList()
        {
            List<CodeName> result = null;

            string sql = @"SELECT DISTINCT PART_NO AS CODE FROM SMT_STENCIL_PART ORDER BY PART_NO";
            var tmpdata = await _dbConnection.QueryAsync<CodeName>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT SMT_STENCIL_PART_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 导出分页分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetExportData(SmtStencilPartRequestModel model)
        {
            string sql = @"  SELECT ROW_NUMBER() OVER(ORDER BY SSP.CREATE_TIME DESC) AS ROWNO, SSP.ID, SSP.STENCIL_NO, SSP.PART_NO,U.NAME PCB_SIDE, SSP.CREATE_TIME, SSP.CREATE_BY, SSP.PN_MODEL,SSP.DESCRIPTION FROM SMT_STENCIL_PART SSP 
                             LEFT JOIN (SELECT 'A' AS CODE, '正面' AS NAME FROM DUAL
                             UNION ALL
                             SELECT 'B' AS CODE, '反面' AS NAME FROM DUAL
                             UNION ALL
                             SELECT 'N' AS CODE, '正(反)' AS NAME FROM DUAL) U ON U.CODE=SSP.PCB_SIDE
                              ";

            string conditions = " WHERE 1=1 ";

            if (!model.STENCIL_NO.IsNullOrWhiteSpace())
            {
                conditions += $" AND (INSTR(SSP.STENCIL_NO, :STENCIL_NO) > 0) ";
            }

            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                conditions += $" AND (SSP.PART_NO =:PART_NO) ";
            }

            if (!model.PCB_SIDE.IsNullOrWhiteSpace())
            {
                conditions += $" AND (SSP.PCB_SIDE =:PCB_SIDE) ";
            }


            string pagedSql = SQLBuilderClass.GetPagedSQL(sql,  conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(SSP.ID) FROM SMT_STENCIL_PART SSP 
                               LEFT JOIN (SELECT 'A' AS CODE, '正面' AS NAME FROM DUAL
                               UNION ALL
                               SELECT 'B' AS CODE, '反面' AS NAME FROM DUAL
                               UNION ALL
                               SELECT 'N' AS CODE, '正(反)' AS NAME FROM DUAL) U ON U.CODE=SSP.PCB_SIDE " + conditions;

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
        public async Task<decimal> SaveDataByTrans(SmtStencilPartModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO SMT_STENCIL_PART(ID, STENCIL_NO, PART_NO, PCB_SIDE, CREATE_TIME, CREATE_BY, PN_MODEL,DESCRIPTION) 
					    VALUES (:ID, :STENCIL_NO, :PART_NO, :PCB_SIDE, SYSDATE, :CREATE_BY, :PN_MODEL,:DESCRIPTION)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = System.Guid.NewGuid().ToString("N");
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.STENCIL_NO,
                                item.PART_NO,
                                item.PCB_SIDE,
                                item.CREATE_BY,
                                item.PN_MODEL,
                                item.DESCRIPTION,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"update smt_stencil_part set STENCIL_NO =:STENCIL_NO, PART_NO =:PART_NO, PCB_SIDE =:PCB_SIDE,
							CREATE_BY =:CREATE_BY, PN_MODEL =:PN_MODEL,DESCRIPTION=:DESCRIPTION 
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.STENCIL_NO,
                                item.PART_NO,
                                item.PCB_SIDE,
                                item.CREATE_BY,
                                item.PN_MODEL,
                                item.DESCRIPTION,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"delete from smt_stencil_part where ID=:ID ";
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