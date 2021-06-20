/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：生产计划表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-09-11 14:01:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtProducePlanRepository                                      
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
    public class SmtProducePlanRepository : BaseRepository<SmtProducePlan, Decimal>, ISmtProducePlanRepository
    {
        public SmtProducePlanRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_PRODUCE_PLAN WHERE ID=:ID";
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
            string sql = "UPDATE SMT_PRODUCE_PLAN set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SMT_PRODUCE_PLAN_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        ///线体ID
        /// </summary>
        /// <param name="line_name">线体名称</param>
        /// <returns></returns>
        public async Task<decimal> GetLineByName(string line_name)
        {
            string sql = "select id from smt_lines where line_name = :line_name";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                line_name
            });
            if (result == null)
            {
                return -1;

            }
            return Convert.ToDecimal(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetLineByID(decimal id)
        {
            string sql = "select line_name from smt_lines where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });
            if (result == null)
            {
                return string.Empty;

            }
            return Convert.ToString(result);
        }

        /// <summary>
        ///项目是否已被使用 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<bool> ItemIsByUsed(decimal id)
        {
            string sql = "select count(0) from SMT_PRODUCE_PLAN where id = :id";
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
        public async Task<decimal> SaveDataByTrans(SmtProducePlanModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SMT_PRODUCE_PLAN 
					(ID,PLAN_DATE,LINE_ID,ORDER_NO,MOVEMENT,MACHINE_TYPE,TYPE_ID,ORDER_QUANTITY,PLAN_QUANTITY,NATIONALITY,WO_NO,DESCRIPTION,CREATE_DATE) 
					VALUES (:ID,:PLAN_DATE,:LINE_ID,:ORDER_NO,:MOVEMENT,:MACHINE_TYPE,:TYPE_ID,:ORDER_QUANTITY,:PLAN_QUANTITY,:NATIONALITY,:WO_NO,:DESCRIPTION,:CREATE_DATE)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.PLAN_DATE,
                                item.LINE_ID,
                                item.ORDER_NO,
                                item.MOVEMENT,
                                item.MACHINE_TYPE,
                                item.TYPE_ID,
                                item.ORDER_QUANTITY,
                                item.PLAN_QUANTITY,
                                item.NATIONALITY,
                                item.WO_NO,
                                item.DESCRIPTION,
                                item.CREATE_DATE,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_PRODUCE_PLAN set PLAN_DATE=:PLAN_DATE,LINE_ID=:LINE_ID,ORDER_NO=:ORDER_NO,MOVEMENT=:MOVEMENT,MACHINE_TYPE=:MACHINE_TYPE,TYPE_ID=:TYPE_ID,ORDER_QUANTITY=:ORDER_QUANTITY,PLAN_QUANTITY=:PLAN_QUANTITY,NATIONALITY=:NATIONALITY,WO_NO=:WO_NO,DESCRIPTION=:DESCRIPTION,CREATE_DATE=:CREATE_DATE  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.PLAN_DATE,
                                item.LINE_ID,
                                item.ORDER_NO,
                                item.MOVEMENT,
                                item.MACHINE_TYPE,
                                item.TYPE_ID,
                                item.ORDER_QUANTITY,
                                item.PLAN_QUANTITY,
                                item.NATIONALITY,
                                item.WO_NO,
                                item.DESCRIPTION,
                                item.CREATE_DATE,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SMT_PRODUCE_PLAN where ID=:ID ";
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
        /// 导入计划保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByPlanFile(List<SmtProducePlan> model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    foreach (var item in model)
                    {
                        //已存在线别，工单，日期 的排期，不保存 
                        string sql = "select count(0) from smt_produce_plan where LINE_ID =:LINE_ID AND WO_NO =:WO_NO AND PLAN_DATE = to_date(:PLAN_DATE,'yyyy-mm-dd') ";
                        string plan_date = item.PLAN_DATE.ToString("yyyy-MM-dd");
                        object res = await _dbConnection.ExecuteScalarAsync(sql, new
                        {
                            item.LINE_ID,
                            PLAN_DATE = plan_date,
                            item.WO_NO
                        });

                        if (Convert.ToInt32(res) == 0)
                        {
                            var newid = await GetSEQID();
                            item.ID = newid;
                            var resdata = await _dbConnection.InsertAsync(item, tran);
                        }
                        else
                        {
                            string updateSql = @"Update smt_produce_plan set ORDER_NO=:ORDER_NO, MOVEMENT=:MOVEMENT, MACHINE_TYPE=:MACHINE_TYPE, TYPE_ID=:TYPE_ID,
                                ORDER_QUANTITY=:ORDER_QUANTITY,PLAN_QUANTITY=:PLAN_QUANTITY, NATIONALITY=:NATIONALITY,DESCRIPTION=:DESCRIPTION 
						     where LINE_ID =:LINE_ID AND WO_NO =:WO_NO AND PLAN_DATE = to_date(:PLAN_DATE,'yyyy-mm-dd') ";

                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                PLAN_DATE = plan_date,
                                item.LINE_ID,
                                item.ORDER_NO,
                                item.MOVEMENT,
                                item.MACHINE_TYPE,
                                item.TYPE_ID,
                                item.ORDER_QUANTITY,
                                item.PLAN_QUANTITY,
                                item.NATIONALITY,
                                item.WO_NO,
                                item.DESCRIPTION,
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
        ///线体ID
        /// </summary>
        /// <param name="line_name">线体名称</param>
        /// <param name="plan_type">计划类型(计划类型：0:SMT, 1:DIP)</param>
        /// <returns></returns>
        public async Task<decimal> GetLineByName(string line_name, decimal plan_type)
        {
            string sql;
            if (plan_type == 0)
                sql = "select id from smt_lines where line_name = :line_name";
            else
                sql = "select id from sfcs_operation_lines where operation_line_name = :line_name";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                line_name
            });
            if (result == null)
            {
                return -1;
            }
            return Convert.ToDecimal(result);
        }

    }
}