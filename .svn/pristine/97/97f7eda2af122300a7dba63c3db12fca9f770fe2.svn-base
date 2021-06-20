/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-03 17:21:30                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsScraperCleanHistoryRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsScraperCleanRepository : BaseRepository<SfcsScraperCleanHistory, Decimal>, ISfcsScraperCleanRepository
    {
        public SfcsScraperCleanRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_SCRAPER_CLEAN_HISTORY WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_SCRAPER_CLEAN_HISTORY set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_SCRAPER_CLEAN_HISTORY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取刮刀状态列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetScraperStatusAsync()
        {
            List<IDNAME> result = null;

            string sql = @"SELECT CODE AS ID, CN_DESC AS NAME FROM SMT_LOOKUP where TYPE = 'SCRAPER_STATUS' AND ENABLED = 'Y'";
            var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取刮刀runcard数据
        /// </summary>
        /// <param name="scraperNo"></param>
        /// <returns></returns>
        public async Task<SfcsScraperRuncard> GetScraperRuncard(string scraperNo)
        {
            SfcsScraperRuncard result = null;

            string sql = @"select * from (
							select tb.*, rownum from sfcs_scraper_runcard tb where scraper_no =:SCRAPER_NO order by id)
							where rownum=1 ";
            var tmpdata = await _dbConnection.QueryAsync<SfcsScraperRuncard>(sql, new { SCRAPER_NO = scraperNo });

            if (tmpdata != null)
            {
                result = tmpdata.FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// 获取刮刀清洗记录数据
        /// </summary>
        /// <param name="scraperNo"></param>
        /// <returns></returns>
        public async Task<SfcsScraperCleanHistory> GetScraperCleanHistory(string scraperNo)
        {
            SfcsScraperCleanHistory result = null;

            string sql = @"select * from (
							select tb.*, rownum from SFCS_SCRAPER_CLEAN_HISTORY tb where scraper_no =:SCRAPER_NO order by id desc)
							where rownum=1 ";
            var tmpdata = await _dbConnection.QueryAsync<SfcsScraperCleanHistory>(sql, new { SCRAPER_NO = scraperNo });

            if (tmpdata != null)
            {
                result = tmpdata.FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// 获取刮刀清洗记录列表
        /// </summary>
        /// <param name="scraperNo">刮刀号</param>
        /// <returns></returns>
        public async Task<List<SfcsScraperCleanHistory>> GetScraperCleanHistoryList(string scraperNo)
        {
            List<SfcsScraperCleanHistory> result = null;

            string sql = @"select * from SFCS_SCRAPER_CLEAN_HISTORY where scraper_no =:SCRAPER_NO order by id";
            var tmpdata = await _dbConnection.QueryAsync<SfcsScraperCleanHistory>(sql, new { SCRAPER_NO = scraperNo });

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取Smt Line
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IDNAME> GetSmtLine(decimal id)
        {
            IDNAME result = null;

            string sql = @"select ID, Line_Name AS NAME from SMT_LINES WHERE ID = :ID";

            var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql, new { ID = id });

            if (tmpdata != null)
            {
                result = tmpdata.FirstOrDefault();
            }
            return result;
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsScraperCleanModel model)
        {
            int result = 1;
            decimal scraperResult = (model.INSPECT_RESULT == "OK") ? (decimal)ScraperEnm.SCRAPER_CLEAN : scraperResult = (decimal)ScraperEnm.SCRAPER_FAIL;

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //記錄刮刀清洗記錄
                    string sql = @"INSERT INTO SFCS_SCRAPER_CLEAN_HISTORY(ID, SCRAPER_NO, PRINT_COUNT, CLEAN_USER, INSPECT_RESULT)
								   VALUES(SFCS_SCRAPER_CLEAN_HISTORY_SEQ.NEXTVAL, :SCRAPER_NO, :PRINT_COUNT, :CLEAN_USER, :INSPECT_RESULT)";
                    var resdata = await _dbConnection.ExecuteAsync(sql, new
                    {
                        model.SCRAPER_NO,
                        model.PRINT_COUNT,
                        model.CLEAN_USER,
                        model.INSPECT_RESULT,
                    }, tran);

                    if (resdata > 0 && model.INSPECT_RESULT == "OK")
                    {
                        //重置刮刀的使用次數
                        sql = @"UPDATE SFCS_SCRAPER_RUNCARD
								SET TOTAL_PRINT_COUNT =
										DECODE (TOTAL_PRINT_COUNT, NULL, 0, TOTAL_PRINT_COUNT)
										+ PRINT_COUNT,
									TOTAL_PRODUCT_PASS_COUNT =
										DECODE (TOTAL_PRODUCT_PASS_COUNT,
												NULL, 0,
												TOTAL_PRODUCT_PASS_COUNT
												)
										+ PRODUCT_PASS_COUNT,
									PRINT_COUNT = 0,
									PRODUCT_PASS_COUNT = 0
								WHERE SCRAPER_NO = :SCRAPER_NO";
                        await _dbConnection.ExecuteAsync(sql, new { model.SCRAPER_NO });
                    }

                    //處理Scraper Runcard Status
                    if (resdata > 0)
                    {
                        sql = @"SELECT * FROM SFCS_SCRAPER_RUNCARD WHERE SCRAPER_NO = :SCRAPER_NO";
                        var tmpdata = await _dbConnection.QueryAsync<SfcsScraperRuncard>(sql, new { model.SCRAPER_NO });
                        if (tmpdata != null && tmpdata.Count() == 0)
                        {
                            sql = @"INSERT INTO SFCS_SCRAPER_RUNCARD(ID, SCRAPER_NO, PRINT_COUNT, PRODUCT_PASS_COUNT, STATUS,
									OPERATION_TIME, BIND_SITE_ID ,BIND_OPERATOR)VALUES(SFCS_SCRAPER_RUNCARD_SEQ.NEXTVAL,
									:SCRAPER_NO, :PRINT_COUNT, :PRODUCT_PASS_COUNT, :STATUS, SYSDATE, :BIND_SITE_ID, :OPERATOR)";
                            await _dbConnection.ExecuteAsync(sql, new
                            {
                                model.SCRAPER_NO,
                                PRINT_COUNT = 0,
                                PRODUCT_PASS_COUNT = 0,
                                STATUS = scraperResult,
                                BIND_SITE_ID = model.SiteID,
                                OPERATOR = model.CLEAN_USER,
                            }, tran);
                        }
                        else if (tmpdata != null && tmpdata.Count() > 0)
                        {
                            sql = @" UPDATE SFCS_SCRAPER_RUNCARD 
									 SET STATUS = :STATUS, OPERATION_TIME = SYSDATE, BIND_SITE_ID = :BIND_SITE_ID, OPERATOR = :OPERATOR 
									 WHERE SCRAPER_NO=:SCRAPER_NO";
                            await _dbConnection.ExecuteAsync(sql, new
                            {
                                model.SCRAPER_NO,
                                STATUS = scraperResult,
                                BIND_SITE_ID = model.SiteID,
                                OPERATOR = model.CLEAN_USER,
                            }, tran);
                        }
                    }
                    //插入刮刀操作記錄
                    if (resdata > 0)
                    {
                        sql = @"INSERT INTO SFCS_SCRAPER_OPERATION_HISTORY(ID, SCRAPER_NO, SCRAPER_STATUS, WORKER_NO, OPERATION_TIME, OPERATION_BY) 
                                VALUES (SFCS_SCRAPER_OP_HISTORY_SEQ.NEXTVAL, :SCRAPER_NO, :SCRAPER_STATUS, :WORKER_NO, SYSDATE, :OPERATION_BY)";
                        await _dbConnection.ExecuteAsync(sql, new
                        {
                            model.SCRAPER_NO,
                            SCRAPER_STATUS = scraperResult,
                            WORKER_NO = model.CLEAN_USER,
                            OPERATION_BY = model.CLEAN_USER,
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
    }
}