/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-06 10:41:02                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsScraperOperationHistoryRepository                                      
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
using System.Linq;
using JZ.IMS.ViewModels;
using System.Collections.Generic;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsScraperOperatRepository : BaseRepository<SfcsScraperOperationHistory, Decimal>, ISfcsScraperOperatRepository
    {
        public SfcsScraperOperatRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_SCRAPER_OPERATION_HISTORY WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_SCRAPER_OPERATION_HISTORY set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_SCRAPER_OP_HISTORY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
        /// 获取站点刮刀runcard数据
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public async Task<List<SfcsScraperRuncard>> GetScraperRuncardBySite(decimal siteID)
        {
            List<SfcsScraperRuncard> result = null;

            string sql = @"select * from sfcs_scraper_runcard where BIND_SITE_ID =:BIND_SITE_ID ";
            var tmpdata = await _dbConnection.QueryAsync<SfcsScraperRuncard>(sql, new { BIND_SITE_ID = siteID });

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取刮刀注册信息
        /// </summary>
        /// <param name="scraperNo"></param>
        /// <returns></returns>
        public async Task<SfcsScraperConfig> GetScraperConfig(string scraperNo)
        {
            SfcsScraperConfig result = null;

            string sql = @"select * from (
							select tb.*, rownum from SFCS_SCRAPER_CONFIG tb where scraper_no =:SCRAPER_NO order by id)
							where rownum=1 ";
            var tmpdata = await _dbConnection.QueryAsync<SfcsScraperConfig>(sql, new { SCRAPER_NO = scraperNo });

            if (tmpdata != null)
            {
                result = tmpdata.FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// 获取Smt Line
        /// </summary>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetSmtLine()
        {
            List<IDNAME> result = null;

            string sql = @"select ID, Line_Name AS NAME from SMT_LINES ORDER BY ID ";

            var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取刮刀作业历史列表
        /// </summary>
        /// <param name="scraperNo">刮刀号</param>
        /// <returns></returns>
        public async Task<List<SfcsScraperOperationHistory>> GetScraperOperationHistoryList(string scraperNo)
        {
            List<SfcsScraperOperationHistory> result = null;

            string sql = @"select * from SFCS_SCRAPER_OPERATION_HISTORY where scraper_no =:SCRAPER_NO order by id";
            var tmpdata = await _dbConnection.QueryAsync<SfcsScraperOperationHistory>(sql, new { SCRAPER_NO = scraperNo });

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取刮刀作业历史信息
        /// </summary>
        /// <param name="scraperNo">刮刀号</param>
        /// <param name="scraperNo">刮刀状态</param>
        /// <returns></returns>
        public async Task<SfcsScraperOperationHistory> GetScraperOperationHistory(string scraperNo, decimal scraper_status)
        {
            SfcsScraperOperationHistory result = null;

            string sql = @"select * from (
							select tb.*, rownum from SFCS_SCRAPER_OPERATION_HISTORY tb where scraper_no =:SCRAPER_NO and SCRAPER_STATUS=:SCRAPER_STATUS 
                            order by OPERATION_TIME DESC)
							where rownum=1 ";
            var tmpdata = await _dbConnection.QueryAsync<SfcsScraperOperationHistory>(sql, new
            {
                SCRAPER_NO = scraperNo,
                SCRAPER_STATUS = scraper_status
            });

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
        public async Task<decimal> SaveDataByTrans(SfcsScraperOperatModel model)
        {
            int result = 1;
            int resdata = 0;
            //刮刀归还时为: 0 
            if (model.ActionType == (int)ScraperEnm.SCRAPER_STORED)
            {
                model.SiteID = 0;
            }

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //處理Scraper Runcard Status
                    string sql = @"SELECT * FROM SFCS_SCRAPER_RUNCARD WHERE SCRAPER_NO = :SCRAPER_NO";
                    var tmpdata = await _dbConnection.QueryAsync<SfcsScraperRuncard>(sql, new { model.SCRAPER_NO });
                    if (tmpdata != null && tmpdata.Count() == 0)
                    {
                        sql = @"INSERT INTO SFCS_SCRAPER_RUNCARD(ID, SCRAPER_NO, PRINT_COUNT, PRODUCT_PASS_COUNT, STATUS,
									OPERATION_TIME, BIND_SITE_ID ,BIND_OPERATOR)VALUES(SFCS_SCRAPER_RUNCARD_SEQ.NEXTVAL,
									:SCRAPER_NO, :PRINT_COUNT, :PRODUCT_PASS_COUNT, :STATUS, SYSDATE, :BIND_SITE_ID, :OPERATOR)";
                        resdata = await _dbConnection.ExecuteAsync(sql, new
                        {
                            model.SCRAPER_NO,
                            PRINT_COUNT = 0,
                            PRODUCT_PASS_COUNT = 0,
                            STATUS = model.ActionType,
                            BIND_SITE_ID = model.SiteID,
                            OPERATOR = model.WorkerNo,
                        }, tran);
                    }
                    else if (tmpdata != null && tmpdata.Count() > 0)
                    {
                        sql = @" UPDATE SFCS_SCRAPER_RUNCARD 
									 SET STATUS = :STATUS, OPERATION_TIME = SYSDATE, BIND_SITE_ID = :BIND_SITE_ID, OPERATOR = :OPERATOR 
									 WHERE SCRAPER_NO=:SCRAPER_NO";
                        resdata = await _dbConnection.ExecuteAsync(sql, new
                        {
                            model.SCRAPER_NO,
                            STATUS = model.ActionType,
                            BIND_SITE_ID = model.SiteID,
                            OPERATOR = model.WorkerNo,
                        }, tran);
                    }

                    //修改工单绑定刮刀的状态
                    sql = @"UPDATE SMT_SCRAPER_WO set IS_USING = 'N' WHERE SCRAPER_NO = :SCRAPER_NO";
                    await _dbConnection.ExecuteAsync(sql, new
                    {
                        SCRAPER_NO=  model.SCRAPER_NO
                    }, tran);

                    //插入刮刀操作記錄
                    if (resdata > 0)
                    {
                        sql = @"INSERT INTO SFCS_SCRAPER_OPERATION_HISTORY(ID, SCRAPER_NO, SCRAPER_STATUS, WORKER_NO, OPERATION_TIME, OPERATION_BY) 
                                VALUES (SFCS_SCRAPER_OP_HISTORY_SEQ.NEXTVAL, :SCRAPER_NO, :SCRAPER_STATUS, :WORKER_NO, SYSDATE, :OPERATION_BY)";
                        await _dbConnection.ExecuteAsync(sql, new
                        {
                            model.SCRAPER_NO,
                            SCRAPER_STATUS = model.ActionType,
                            WORKER_NO = model.WorkerNo,
                            OPERATION_BY = model.UserName,
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

        /// <summary>
        /// 刮刀使用次数
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetScraperUseData(string SCRAPER_NO, PageModel pageModel)
        {
            var conditions = " WHERE 1=1 ";
            if (!SCRAPER_NO.IsNullOrEmpty())
            {
                conditions += " AND A.SCRAPER_NO = :SCRAPER_NO ";
            }
            var sqlPage = $@"SELECT * FROM (select A.*,rownum num,C.NICK_NAME  , DECODE (A.STATUS,
               1, '使用中',
               2, '已清洗',
               3, '上线',
               4, '下线',
               5, '不合格',
               6, '领用中',
               7, '储存中',
               NULL, '储存中') STATUS_NAME from SFCS_SCRAPER_RUNCARD A  LEFT JOIN SYS_MANAGER C ON A.OPERATOR = C.USER_NAME {conditions}) u WHERE u.num BETWEEN ((:Page-1) * :Limit + 1) AND (:Page * :Limit)";

            var data = await _dbConnection.QueryAsync<dynamic>(sqlPage, new { SCRAPER_NO, pageModel.Limit, pageModel.Page });

            var countSql = $@"select COUNT(1) from SFCS_SCRAPER_RUNCARD A {conditions}";
            var count = await _dbConnection.ExecuteScalarAsync<int>(countSql, new { SCRAPER_NO, pageModel.Limit, pageModel.Page });

            return new TableDataModel() { data = data, count = count };
        }
    }
}