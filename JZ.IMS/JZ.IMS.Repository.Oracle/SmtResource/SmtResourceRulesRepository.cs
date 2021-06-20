/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-02-27 14:15:50                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtResourceRulesRepository                                      
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
    public class SmtResourceRulesRepository : BaseRepository<SmtResourceRules, Decimal>, ISmtResourceRulesRepository
    {
        public SmtResourceRulesRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_RESOURCE_RULES WHERE ID=:ID";
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
            string sql = "UPDATE SMT_RESOURCE_RULES set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        /// 获取辅料规则工序选择列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResourceRulesProcess>> GetProcessAsync()
        {
            List<ResourceRulesProcess> result = null;

            string sql = @"SELECT SRR.ID, SAO.CN_DESC OBJECT_NAME, SP.VALUE MEANING 
							FROM SMT_RESOURCE_ROUTE SRR,
							  SMT_LOOKUP SP,
							  SMT_LOOKUP SAO,
							  SMT_LOOKUP SPT
							WHERE SRR.CURRENT_OPERATION = SP.CODE
							  AND SP.TYPE = 'RESOURCE_ROUTE'
							  AND SRR.OBJECT_ID = SAO.CODE
							  AND SPT.TYPE = 'RESOURCE_ROUTE'
							  AND SAO.TYPE = 'RESOURCE_OBJECT'
							  AND SRR.NEXT_OPERATION = SPT.CODE
							ORDER BY SRR.ORDER_NO ASC";
            var tmpdata = await _dbConnection.QueryAsync<ResourceRulesProcess>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        ///项目是否已被使用 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        //public async Task<bool> ItemIsByUsed(decimal id)
        //{
        //	string sql = "select count(0) from smt_resource_runcard where ROUTE_ID = :id";
        //	object result = await _dbConnection.ExecuteScalarAsync(sql, new
        //	{
        //		id
        //	});

        //	return (Convert.ToInt32(result) > 0);
        //}

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetResourceRulesList(SmtResourceRulesRequestModel model)
        {
            string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY SRR.ID DESC) ROWNO, SRR.ID,  SRR.OBJECT_ID,LP.EN_DESC, SRR.CATEGORY_ID,RC.CATEGORY_NAME, SRR.ROUTE_OPERATION_ID,T.MEANING, SRR.STANDARD_TIME, SRR.STANDARD_FLAG, SRR.VALID_FLAG, SRR.EXPOSE_FLAG, SRR.ENABLED FROM SMT_RESOURCE_RULES SRR
                            LEFT JOIN SMT_LOOKUP LP ON  LP.TYPE = 'RESOURCE_OBJECT' AND LP.ENABLED = 'Y' AND LP.CODE=SRR.OBJECT_ID
                            LEFT JOIN SMT_RESOURCE_CATEGORY RC ON RC.ENABLED = 'Y' AND RC.CATEGORY_ID=SRR.CATEGORY_ID
                            LEFT JOIN (SELECT SRR.ID, SAO.CN_DESC OBJECT_NAME, SP.VALUE MEANING 
                            FROM SMT_RESOURCE_ROUTE SRR,
                              SMT_LOOKUP SP,
                              SMT_LOOKUP SAO,
                              SMT_LOOKUP SPT
                              WHERE SRR.CURRENT_OPERATION = SP.CODE
                              AND SP.TYPE = 'RESOURCE_ROUTE'
                              AND SRR.OBJECT_ID = SAO.CODE
                              AND SPT.TYPE = 'RESOURCE_ROUTE'
                              AND SAO.TYPE = 'RESOURCE_OBJECT'
                              AND SRR.NEXT_OPERATION = SPT.CODE) T  ON T.ID=SRR.ROUTE_OPERATION_ID  ";
            string condition = " WHERE SRR.ID > 0 ";

            if (model.OBJECT_ID!=null&&model.OBJECT_ID>0)
            {
                condition += $"and (SRR.OBJECT_ID = :OBJECT_ID) ";
            }

            if (model.CATEGORY_ID != null && model.CATEGORY_ID > 0)
            {
                condition += $"and (SRR.CATEGORY_ID = :CATEGORY_ID) ";
            }

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "SRR.ID DESC", condition);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(SRR.ID) FROM SMT_RESOURCE_RULES SRR
                            LEFT JOIN SMT_LOOKUP LP ON  LP.TYPE = 'RESOURCE_OBJECT' AND LP.ENABLED = 'Y' AND LP.CODE=SRR.OBJECT_ID
                            LEFT JOIN SMT_RESOURCE_CATEGORY RC ON RC.ENABLED = 'Y' AND RC.CATEGORY_ID=SRR.CATEGORY_ID
                            LEFT JOIN (SELECT SRR.ID, SAO.CN_DESC OBJECT_NAME, SP.VALUE MEANING 
                            FROM SMT_RESOURCE_ROUTE SRR,
                              SMT_LOOKUP SP,
                              SMT_LOOKUP SAO,
                              SMT_LOOKUP SPT
                              WHERE SRR.CURRENT_OPERATION = SP.CODE
                              AND SP.TYPE = 'RESOURCE_ROUTE'
                              AND SRR.OBJECT_ID = SAO.CODE
                              AND SPT.TYPE = 'RESOURCE_ROUTE'
                              AND SAO.TYPE = 'RESOURCE_OBJECT'
                              AND SRR.NEXT_OPERATION = SPT.CODE) T  ON T.ID=SRR.ROUTE_OPERATION_ID  " + condition;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            }; 
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async  Task<TableDataModel> GetExportData(SmtResourceRulesRequestModel model)
        {
            string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY SRR.ID DESC) ROWNO, SRR.ID, LP.EN_DESC OBJECT_ID,RC.CATEGORY_NAME CATEGORY_ID, T.MEANING ROUTE_OPERATION_ID, SRR.STANDARD_TIME, SRR.STANDARD_FLAG, SRR.VALID_FLAG, SRR.EXPOSE_FLAG, SRR.ENABLED FROM SMT_RESOURCE_RULES SRR
                            LEFT JOIN SMT_LOOKUP LP ON  LP.TYPE = 'RESOURCE_OBJECT' AND LP.ENABLED = 'Y' AND LP.CODE=SRR.OBJECT_ID
                            LEFT JOIN SMT_RESOURCE_CATEGORY RC ON RC.ENABLED = 'Y' AND RC.CATEGORY_ID=SRR.CATEGORY_ID
                            LEFT JOIN (SELECT SRR.ID, SAO.CN_DESC OBJECT_NAME, SP.VALUE MEANING 
                            FROM SMT_RESOURCE_ROUTE SRR,
                              SMT_LOOKUP SP,
                              SMT_LOOKUP SAO,
                              SMT_LOOKUP SPT
                              WHERE SRR.CURRENT_OPERATION = SP.CODE
                              AND SP.TYPE = 'RESOURCE_ROUTE'
                              AND SRR.OBJECT_ID = SAO.CODE
                              AND SPT.TYPE = 'RESOURCE_ROUTE'
                              AND SAO.TYPE = 'RESOURCE_OBJECT'
                              AND SRR.NEXT_OPERATION = SPT.CODE) T  ON T.ID=SRR.ROUTE_OPERATION_ID   ";
            string condition = " WHERE SRR.ID > 0 ";

            if (model.OBJECT_ID != null && model.OBJECT_ID > 0)
            {
                condition += $"and (SRR.OBJECT_ID = :OBJECT_ID) ";
            }

            if (model.CATEGORY_ID != null && model.CATEGORY_ID > 0)
            {
                condition += $"and (SRR.CATEGORY_ID = :CATEGORY_ID) ";
            }

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "SRR.ID DESC", condition);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(SRR.ID) FROM SMT_RESOURCE_RULES SRR
                            LEFT JOIN SMT_LOOKUP LP ON  LP.TYPE = 'RESOURCE_OBJECT' AND LP.ENABLED = 'Y' AND LP.CODE=SRR.OBJECT_ID
                            LEFT JOIN SMT_RESOURCE_CATEGORY RC ON RC.ENABLED = 'Y' AND RC.CATEGORY_ID=SRR.CATEGORY_ID
                            LEFT JOIN (SELECT SRR.ID, SAO.CN_DESC OBJECT_NAME, SP.VALUE MEANING 
                            FROM SMT_RESOURCE_ROUTE SRR,
                              SMT_LOOKUP SP,
                              SMT_LOOKUP SAO,
                              SMT_LOOKUP SPT
                              WHERE SRR.CURRENT_OPERATION = SP.CODE
                              AND SP.TYPE = 'RESOURCE_ROUTE'
                              AND SRR.OBJECT_ID = SAO.CODE
                              AND SPT.TYPE = 'RESOURCE_ROUTE'
                              AND SAO.TYPE = 'RESOURCE_OBJECT'
                              AND SRR.NEXT_OPERATION = SPT.CODE) T  ON T.ID=SRR.ROUTE_OPERATION_ID  " + condition;

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
        public async Task<decimal> SaveDataByTrans(SmtResourceRulesModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SMT_RESOURCE_RULES (ID,OBJECT_ID,CATEGORY_ID,ROUTE_OPERATION_ID,STANDARD_TIME,STANDARD_FLAG,VALID_FLAG,EXPOSE_FLAG,ENABLED) 
					VALUES (:ID,:OBJECT_ID,:CATEGORY_ID,:ROUTE_OPERATION_ID,:STANDARD_TIME,:STANDARD_FLAG,:VALID_FLAG,:EXPOSE_FLAG,:ENABLED)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.OBJECT_ID,
                                item.CATEGORY_ID,
                                item.ROUTE_OPERATION_ID,
                                item.STANDARD_TIME,
                                item.STANDARD_FLAG,
                                item.VALID_FLAG,
                                item.EXPOSE_FLAG,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_RESOURCE_RULES set OBJECT_ID =:OBJECT_ID, CATEGORY_ID =:CATEGORY_ID, ROUTE_OPERATION_ID =:ROUTE_OPERATION_ID, 
							STANDARD_TIME =:STANDARD_TIME, STANDARD_FLAG =:STANDARD_FLAG, VALID_FLAG =:VALID_FLAG, 
							EXPOSE_FLAG =:EXPOSE_FLAG, ENABLED =:ENABLED 
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.OBJECT_ID,
                                item.CATEGORY_ID,
                                item.ROUTE_OPERATION_ID,
                                item.STANDARD_TIME,
                                item.STANDARD_FLAG,
                                item.VALID_FLAG,
                                item.EXPOSE_FLAG,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from smt_resource_rules where ID=:ID ";
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

        /// <summary>
        /// 获取辅料回温情况
        /// </summary>
        /// <returns></returns>
        public async Task<List<SmtResourceWarmVM>> GetSmtResourceWarm()
        {
            List<SmtResourceWarmVM> result = null;

            string sql = @"select RC.RESOURCE_NO, RE.ATTRIBUTE4 AS Bottle_NO, RC.BEGIN_OPERATION_TIME, 
                                round((sysdate - RC.BEGIN_OPERATION_TIME)*24*60,2) Warm_Time 
                           From SMT_RESOURCE_RUNCARD RC, IMS_reel RE 
                           WHERE RC.RESOURCE_NO = RE.CODE AND STATUS = 6";
            var tmpdata = await _dbConnection.QueryAsync<SmtResourceWarmVM>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取辅料使用情况
        /// </summary>
        /// <returns></returns>
        public async Task<List<SmtResourceUseVM>> GetSmtResourceUse()
        {
            List<SmtResourceUseVM> result = null;

            string sql = @"select RC.RESOURCE_NO, RE.ATTRIBUTE4 AS Bottle_NO,
                                  SL.LINE_NAME, RC.BEGIN_OPERATION_TIME, round((sysdate - RC.BEGIN_OPERATION_TIME)*24*60,2) USED_Time 
                            From SMT_RESOURCE_RUNCARD RC, IMS_reel RE,
                                 SMT_RESOURCE_WO WO, smt_lines SL
                            WHERE RC.RESOURCE_NO = RE.CODE
                                AND RC.RESOURCE_NO = WO.RESOURCE_NO(+) AND WO.LINE_ID = SL.ID(+) 
                                AND STATUS = 1 ";
            var tmpdata = await _dbConnection.QueryAsync<SmtResourceUseVM>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

    }
}