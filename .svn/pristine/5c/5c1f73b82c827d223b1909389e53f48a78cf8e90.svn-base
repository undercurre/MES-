/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：不良报工表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-05-26 14:37:35                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsDefectReportWorkRepository                                      
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
using JZ.IMS.Core.Extensions;
using JZ.IMS.ViewModels;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsDefectReportWorkRepository:BaseRepository<SfcsDefectReportWork,Decimal>, ISfcsDefectReportWorkRepository
    {
        public SfcsDefectReportWorkRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
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
			string sql = "SELECT ENABLED FROM SFCS_DEFECT_REPORT_WORK WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_DEFECT_REPORT_WORK set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_DEFECT_REPORT_WORK_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

        /// <summary>
        /// 重写获取数据方法获取数据
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SfcsDefectReportWork>> GetDataPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby,object parameters = null)
        {
            string sql = @"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY T.WO_NO ASC,QTY DESC) AS PagedNumber, 
                            T.*,C.DEFECT_DESCRIPTION AS DEFECT_NAME,LINE_NAME FROM (
                            SELECT WO_NO,LINE_ID,DEFECT_CODE,LOC,COUNT(1) AS QTY 
                            FROM V_DEFECT_LOC_SUM 
                            GROUP BY WO_NO,LINE_ID,DEFECT_CODE,LOC) T 
                            INNER JOIN SFCS_DEFECT_CONFIG  C ON T.DEFECT_CODE = C.DEFECT_CODE
                            INNER JOIN V_MES_LINES D ON T.LINE_ID = D.LINE_ID
                            INNER JOIN SYS_ORGANIZE_LINE E ON T.LINE_ID = E.LINE_ID
                            WHERE {2}) u 
                            WHERE PagedNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})";
            sql = string.Format(sql, pageNumber, rowsPerPage,conditions);
            return await _dbConnection.QueryAsync<SfcsDefectReportWork>(sql, parameters);
        }

        /// <summary>
        /// 重写获取条数方法
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public new async Task<int> RecordCountAsync(string conditions, object parameters = null)
        {
            string sql = @"SELECT count(1) FROM (
                            SELECT WO_NO,LINE_ID,DEFECT_CODE,LOC,COUNT(1) AS QTY 
                            FROM V_DEFECT_LOC_SUM 
                            GROUP BY WO_NO,LINE_ID,DEFECT_CODE,LOC) T 
                            INNER JOIN SFCS_DEFECT_CONFIG  C ON T.DEFECT_CODE = C.DEFECT_CODE
                            INNER JOIN V_MES_LINES D ON T.LINE_ID = D.LINE_ID
                            INNER JOIN SYS_ORGANIZE_LINE E ON T.LINE_ID = E.LINE_ID
                            WHERE {0}";
            var parData = parameters as SfcsDefectReportWorkRequestModel;
            sql = string.Format(sql, conditions);
            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);
            return result;
        }

        /// <summary>
        /// 提交不良报工
        /// </summary>
        /// <param name="wo_id">工单id</param>
        /// <param name="site_id">站点id</param>
        /// <param name="Operator">操作人</param>
        /// <param name="qty">不良数量</param>
        /// <param name="reportTime">时间</param>
        /// <param name="defect_code">不良代码</param>
        /// <returns></returns>
        public async Task<int> PostToDefectReport(decimal wo_id, decimal site_id, string Operator, decimal qty, DateTime reportTime, string defect_code, string defect_loc)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string insertSql = @"INSERT INTO SFCS_DEFECT_REPORT_WORK(ID,LOC,DEFECT_CODE,WO_ID,OPERATION_SITE_ID,OPERATOR,QTY,REPORT_TIME,CREATE_TIME)
                                                  VALUES (:ID,:LOC,:DEFECT_CODE,:WO_ID,:OPERATION_SITE_ID,:OPERATOR,:QTY,:REPORT_TIME,SYSDATE) ";
                    var newid = await GetSEQID();
                    int resdata = await _dbConnection.ExecuteAsync(insertSql, new
                    {
                        ID = newid,
                        DEFECT_CODE = defect_code,
                        LOC = defect_loc,
                        WO_ID = wo_id,
                        OPERATION_SITE_ID = site_id,
                        OPERATOR = Operator,
                        QTY = qty,
                        REPORT_TIME = reportTime

                    }, tran);

                    if (resdata > 0)
                    {
                        string selectSiteSql = @"SELECT COUNT(1) FROM SFCS_SITE_STATISTICS WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID AND WORK_TIME = :WORK_TIME ";
                        //获取站點統計信息
                        int SSResult = await _dbConnection.ExecuteScalarAsync<int>(selectSiteSql, new
                        {
                            WO_ID = wo_id,
                            OPERATION_SITE_ID = site_id,
                            WORK_TIME = reportTime
                        });
                        if (SSResult == 0)
                        {
                            string insertSiteSql = @"INSERT INTO SFCS_SITE_STATISTICS(WO_ID,OPERATION_SITE_ID,WORK_TIME,PASS,FAIL,REPASS,REFAIL)
                                                       VALUES(:WO_ID,:OPERATION_SITE_ID,:WORK_TIME,:PASS,:FAIL,:REPASS,:REFAIL)";
                            await _dbConnection.ExecuteAsync(insertSiteSql, new
                            {
                                WO_ID = wo_id,
                                OPERATION_SITE_ID = site_id,
                                WORK_TIME = reportTime,
                                PASS = 0,
                                FAIL = qty,
                                REPASS = 0,
                                REFAIL = 0
                            }, tran);
                        }
                        else
                        {
                            string updateSiteSql = @"UPDATE SFCS_SITE_STATISTICS SET FAIL=FAIL+:QTY WHERE WO_ID=:WO_ID AND OPERATION_SITE_ID=:OPERATION_SITE_ID AND WORK_TIME=:WORK_TIME";
                            await _dbConnection.ExecuteAsync(updateSiteSql, new
                            {
                                WO_ID = wo_id,
                                OPERATION_SITE_ID = site_id,
                                WORK_TIME = reportTime,
                                QTY = qty
                            }, tran);
                        }
                    }
                    else
                    {
                        throw new Exception("提交不良报工失败，请重试！");
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
                    tran.Rollback();//回滚事务
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
        /// 撤销不良报工
        /// </summary>
        /// <param name="id">不良报工id</param>
        /// <param name="Operator">操作人</param>
        /// <param name="fail">不良数量</param>
        /// <param name="woId">工单id</param>
        /// <param name="siteId">站点id</param>
        /// <param name="wokTime">报工时间</param>
        /// <returns></returns>
        public async Task<int> ClearDefectReport(decimal id, string Operator, decimal fail, decimal woId, decimal siteId, DateTime wokTime)
        {

            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string updateToLogSql = @"INSERT INTO JZMES_LOG.SFCS_DEFECT_REPORT_WORK select SDR.* , :OPER OPER , sysdate from SFCS_DEFECT_REPORT_WORK SDR where  id = :ID";
                    string deletSql = @"DELETE FROM SFCS_DEFECT_REPORT_WORK WHERE  ID = :ID";
                    string updateStaticSql = @"update SFCS_SITE_STATISTICS set PASS = PASS-:PASS,FAIL = FAIL - :FAIL  where WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID AND WORK_TIME = :WORK_TIME";

                    await _dbConnection.ExecuteAsync(updateToLogSql, new { OPER = Operator, ID = id }, tran);
                    await _dbConnection.ExecuteAsync(deletSql, new { ID = id }, tran);
                    await _dbConnection.ExecuteAsync(updateStaticSql, new
                    {
                        PASS = 0,
                        FAIL = fail,
                        WO_ID = woId,
                        OPERATION_SITE_ID = siteId,
                        WORK_TIME = wokTime
                    }, tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
                    tran.Rollback();//回滚事务
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