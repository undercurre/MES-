/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产能报工表结构接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-10-08 09:11:53                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsCapReportRepository                                      
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
using System.Text;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsCapReportRepository : BaseRepository<SfcsCapReport, Decimal>, ISfcsCapReportRepository
    {
        public SfcsCapReportRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_CAP_REPORT WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_CAP_REPORT set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_CAP_REPORT_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_CAP_REPORT where id = :id";
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
        public async Task<decimal> SaveDataByTrans(SfcsCapReportModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_CAP_REPORT 
					(ID,WO_ID,OPERATION_SITE_ID,OPERATOR,QTY,REPORT_TIME,CREATE_TIME,CARTON_ID) 
					VALUES (:ID,:WO_ID,:OPERATION_SITE_ID,:OPERATOR,:QTY,:REPORT_TIME,:CREATE_TIME,:CARTON_ID)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.WO_ID,
                                item.OPERATION_SITE_ID,
                                item.OPERATOR,
                                item.QTY,
                                item.REPORT_TIME,
                                item.CREATE_TIME,
                                item.CARTON_ID,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_CAP_REPORT set WO_ID=:WO_ID,OPERATION_SITE_ID=:OPERATION_SITE_ID,OPERATOR=:OPERATOR,QTY=:QTY,REPORT_TIME=:REPORT_TIME,CREATE_TIME=:CREATE_TIME,CARTON_ID=:CARTON_ID  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.WO_ID,
                                item.OPERATION_SITE_ID,
                                item.OPERATOR,
                                item.QTY,
                                item.REPORT_TIME,
                                item.CREATE_TIME,
                                item.CARTON_ID,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_CAP_REPORT where ID=:ID ";
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
        /// 提交产能报工
        /// </summary>
        /// <param name="wo_id">工单id</param>
        /// <param name="site_id">站点id</param>
        /// <param name="Operator">操作人</param>
        /// <param name="qty">产能数量</param>
        /// <param name="reportTime">报工时间</param>
        /// <returns></returns>
        public async Task<int> PostToCapacityReport(decimal wo_id, decimal site_id, string Operator, decimal qty, DateTime reportTime)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string insertSql = @"INSERT INTO SFCS_CAP_REPORT(ID,WO_ID,OPERATION_SITE_ID,OPERATOR,QTY,REPORT_TIME,CREATE_TIME,CARTON_ID)
                                                  VALUES (SFCS_CAP_REPORT_SEQ.NEXTVAL,:WO_ID,:OPERATION_SITE_ID,:OPERATOR,:QTY,:REPORT_TIME,SYSDATE,'0') ";
                    int resdata = await _dbConnection.ExecuteAsync(insertSql, new
                    {
                        WO_ID = wo_id,
                        OPERATION_SITE_ID = site_id,
                        OPERATOR = Operator,
                        QTY = qty,
                        REPORT_TIME = reportTime
                    }, tran);

                    if (resdata > 0)
                    {
                        string selectSite = @"SELECT COUNT(1) FROM SFCS_SITE_STATISTICS WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID AND WORK_TIME = :WORK_TIME";
                        //获取站點統計信息
                        int SSResult = await _dbConnection.ExecuteScalarAsync<int>(selectSite, new
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
                                PASS = qty,
                                FAIL = 0,
                                REPASS = 0,
                                REFAIL = 0
                            }, tran);
                        }
                        else
                        {
                            string updateSiteSql = @"UPDATE SFCS_SITE_STATISTICS SET PASS=PASS+:QTY WHERE WO_ID=:WO_ID AND OPERATION_SITE_ID=:OPERATION_SITE_ID AND WORK_TIME=:WORK_TIME";
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
                        throw new Exception("提交产能报工失败，请重试！");
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
        /// 撤销产能报工
        /// </summary>
        /// <param name="id">产能报工id</param>
        /// <param name="Operator">操作人</param>
        /// <param name="pass">撤销的产能数量</param>
        /// <param name="wo_id">工单id</param>
        /// <param name="site_id">站点id</param>
        /// <param name="wokTime">报工时间</param>
        /// <param name="isLastOperation">是否最后一个工序</param>
        /// <returns></returns>
        public async Task<int> ClearCapacityReport(decimal id, string Operator, decimal pass, SfcsWo woModel, decimal siteId, DateTime wokTime, decimal? lineId, bool isLastOperation, decimal? inBoundRecord)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {


                    string updateToLogSql = @"INSERT INTO JZMES_LOG.SFCS_CAP_REPORT select SCR.* , :OPER OPER , sysdate from SFCS_CAP_REPORT SCR where  id = :ID";
                    string deletSql = @"DELETE FROM SFCS_CAP_REPORT WHERE  ID = :ID";
                    string updateStaticSql = @"update SFCS_SITE_STATISTICS set PASS = PASS-:PASS,FAIL = FAIL - :FAIL  where WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID AND WORK_TIME = :WORK_TIME";

                    #region 最后一个工序更新output数量
                    if (isLastOperation)
                    {

                        String deleteInboundRecordSql = "DELETE from SFCS_INBOUND_RECORD_INFO where ID=:ID ";
                        String sql = @"UPDATE SFCS_WO SET OUTPUT_QTY=OUTPUT_QTY-:PASS,WO_STATUS={0} WHERE ID=:ID";
                        //String smtProductionSql = " UPDATE SMT_PRODUCTION SET OUTPUT_QTY=OUTPUT_QTY-:PASS WHERE WO_NO=:WO_NO AND LINE_ID=:LINE_ID AND FINISHED!='Y' ";
                        String sfcsProductionSql = " UPDATE SFCS_PRODUCTION SET OUTPUT_QTY=OUTPUT_QTY-:PASS WHERE WO_NO=:WO_NO AND LINE_ID=:LINE_ID AND FINISHED!='Y' ";

                        if (inBoundRecord != null && inBoundRecord > 0)
                            //todo:处理撤销完工
                           await _dbConnection.ExecuteAsync(deleteInboundRecordSql, new { ID = inBoundRecord },tran);

                        //todo:output的数量
                        if (woModel.TARGET_QTY == (woModel.OUTPUT_QTY - pass))
                        {
                            //修改完工状态
                            sql = string.Format(sql, 4);
                        }
                        else if (woModel.OUTPUT_QTY - pass > 0)
                        {
                            //投入状态
                            sql = sql = string.Format(sql, 2);
                        }
                        if (woModel.OUTPUT_QTY - pass <= 0)
                        {
                            //未投入
                            sql = sql = string.Format(sql, 1);
                            pass = 0;
                        }

                        //await _dbConnection.ExecuteAsync(smtProductionSql, new
                        //{
                        //    PASS = pass,
                        //    WO_NO = woModel.WO_NO,
                        //    LINE_ID = lineId
                        //});

                        await _dbConnection.ExecuteAsync(sfcsProductionSql, new
                        {
                            PASS = pass,
                            WO_NO = woModel.WO_NO,
                            LINE_ID = lineId
                        });
                        var data = await _dbConnection.ExecuteAsync(sql, new
                        {
                            PASS = pass,
                            ID = woModel.ID
                        });
                    }

                    #endregion

                    await _dbConnection.ExecuteAsync(updateToLogSql, new { OPER = Operator, ID = id }, tran);
                    await _dbConnection.ExecuteAsync(deletSql, new { ID = id }, tran);
                    await _dbConnection.ExecuteAsync(updateStaticSql, new
                    {
                        PASS = pass,
                        FAIL = 0,
                        WO_ID = woModel.ID,
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

        /// <summary>
		/// 获取站点总的合格率【无码报工使用】
		/// 公式：(SUM(PASS)-SUM(FAIL))/SUM(PASS)*100
        /// type="Today"
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="woId"></param>
		/// <returns></returns>
        public async Task<TableDataModel> GetSitePassRate(decimal siteId, decimal woId, string type)
        {
            TableDataModel tableMolde = new TableDataModel();
            try
            {
                string conditions = "";
                //获取今天的合格率条件
                if (type == "Today")
                {
                    conditions += " SSS.WORK_TIME >= TO_DATE(TO_CHAR (SYSDATE, 'yyyy-mm-dd')|| ' 00:00:00','yyyy-mm-dd,hh24:mi:ss') AND ";
                }
                string passSql = $@"SELECT NVL(SUM(SSS.PASS),0)  
				FROM SFCS_SITE_STATISTICS SSS ,SFCS_OPERATION_SITES SOS1,SFCS_OPERATION_SITES SOS2 
				WHERE {conditions} SSS.WO_ID = :WO_ID AND SSS.OPERATION_SITE_ID IN (SOS1.ID) AND SOS2.ID = :OPERATION_SITE_ID 
				AND SOS2.OPERATION_ID = SOS1.OPERATION_ID";

                decimal passCount = await _dbConnection.ExecuteScalarAsync<decimal>(passSql, new { WO_ID = woId, OPERATION_SITE_ID = siteId });
                if (passCount <= 0)
                {
                    tableMolde.data = null;
                    return tableMolde;
                }
                string sql = $@"SELECT NVL((SUM(SSS.PASS)-SUM(SSS.FAIL)),0) AS PASS,NVL(SUM(SSS.PASS),0) AS TOTAL,
				NVL(TRUNC((SUM(SSS.PASS)-SUM(SSS.FAIL))/(SUM(SSS.PASS))*100,2),0) AS RATE 
				FROM SFCS_SITE_STATISTICS SSS ,SFCS_OPERATION_SITES SOS1,SFCS_OPERATION_SITES SOS2 
				WHERE {conditions} SSS.WO_ID = :WO_ID AND SSS.OPERATION_SITE_ID IN (SOS1.ID) AND SOS2.ID = :OPERATION_SITE_ID 
				AND SOS2.OPERATION_ID = SOS1.OPERATION_ID";
                tableMolde.data = await _dbConnection.QueryAsync<dynamic>(sql, new { WO_ID = woId, OPERATION_SITE_ID = siteId });
            }
            catch (Exception ex)
            {
                tableMolde.code = -1;
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }

            return tableMolde;
        }

        /// <summary>
		/// 获取站点当天的每小时产能（PASS + REPASS） 不良产能(Fail)
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="woId"></param>
        public async Task<TableDataModel> GetSiteHourYield(decimal siteId, decimal woId)
        {
            TableDataModel tableMolde = new TableDataModel();
            try
            {
                string sql = @"select  WORK_HOUR, sum(PASS)AS PASS,sum(FAIL) AS FAIL FROM(
			                       SELECT to_char(SSS.WORK_TIME, 'dd') || '日' || to_char(SSS.WORK_TIME, 'hh24') || '时' AS WORK_HOUR, (SSS.PASS + SSS.REPASS) AS PASS,SSS.FAIL AS FAIL
			                       FROM SFCS_SITE_STATISTICS SSS ,SFCS_OPERATION_SITES SOS1,SFCS_OPERATION_SITES SOS2
			                       WHERE SSS.WORK_TIME >= TO_DATE(TO_CHAR (SYSDATE, 'yyyy-mm-dd')|| ' 00:00:00','yyyy-mm-dd,hh24:mi:ss') 
			                       AND SSS.WO_ID = :WO_ID AND SSS.OPERATION_SITE_ID IN (SOS1.ID) AND SOS2.ID = :OPERATION_SITE_ID 
			                       AND SOS2.OPERATION_ID = SOS1.OPERATION_ID AND (SSS.PASS > 0 OR SSS.REPASS > 0 OR SSS.FAIL>0) ) 
			                   group by WORK_HOUR ORDER BY WORK_HOUR DESC";

                tableMolde.data = await _dbConnection.QueryAsync<dynamic>(sql, new { WO_ID = woId, OPERATION_SITE_ID = siteId });
            }
            catch (Exception ex)
            {
                tableMolde.code = -1;
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }

            return tableMolde;
        }


        /// <summary>
		/// 获取站点总的TOP X 不良现象【无码报工使用】
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="woId"></param>
		/// <param name="topCount"></param>
		/// <returns></returns>
        public async Task<TableDataModel> GetSiteTopDefect(decimal? siteId, decimal woId, int topCount, string type)
        {
            TableDataModel tableMolde = new TableDataModel();
            try
            {

                string conditions = "";
                if (siteId > 0)
                {
                    conditions += " AND OPERATION_SITE_ID = :OPERATION_SITE_ID ";
                }

                //获取今天不良现象
                if (type == "Today")
                {
                    conditions += " AND REPORT_TIME >= TO_DATE(TO_CHAR (SYSDATE, 'yyyy-mm-dd')|| ' 00:00:00','yyyy-mm-dd,hh24:mi:ss') ";
                }
                String sql = $@"SELECT A.DEFECT_CODE,B.DEFECT_DESCRIPTION,QTY FROM(
			                        SELECT ROW_NUMBER() OVER(ORDER BY SUM(QTY) DESC) AS RowNumer,DEFECT_CODE,SUM(QTY) AS QTY 
			                        FROM SFCS_DEFECT_REPORT_WORK 
			                        WHERE WO_ID = :WO_ID  {conditions}
			                        AND DEFECT_CODE not in ('FCT_FAIL','ICT_FAIL')
			                        GROUP BY DEFECT_CODE) A 
			                        INNER JOIN SFCS_DEFECT_CONFIG B ON A.DEFECT_CODE = B.DEFECT_CODE 
			                        WHERE RowNumer <= :TOP_COUNT ORDER BY RowNumer DESC";

                tableMolde.data = await _dbConnection.QueryAsync<dynamic>(sql, new { WO_ID = woId, OPERATION_SITE_ID = siteId, TOP_COUNT = topCount });
            }
            catch (Exception ex)
            {
                tableMolde.code = -1;
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }

            return tableMolde;
        }

        /// <summary>
        /// 根据工单下标获取当前线别生产信息
        /// </summary>
        /// <param name="lineId">线别</param>
        /// <param name="index">工单下标 0：当前工单，1：上一工单，2：上两工单</param>
        /// <returns></returns>
        public async Task<SfcsProduction> GetProductionByIndex(Decimal lineId, int index = 0)
        {
            SfcsProduction tableMolde = new SfcsProduction();
            try
            {
                string sql = @"select * from (
                        select row_number() over (order by t.start_time desc) as rowIndex,t.WO_NO,t.STATION_ID from sfcs_production t where t.line_id=:LINE_ID
                        union
                        select row_number() over (order by t.start_time desc) as rowIndex,t.WO_NO,t.STATION_ID from smt_production t where t.line_id=:LINE_ID
                        ) where rowIndex = :ROWINDEX";

                tableMolde = (await _dbConnection.QueryAsync<SfcsProduction>(sql, new { LINE_ID = lineId, ROWINDEX = index + 1 }))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }

            return tableMolde;
        }


        public async Task<decimal> GetStandardCapacity(string PART_NO, decimal OPERATION_ID)
        {
            decimal result = 0;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT B.* FROM SOP_ROUTES A ");
                sql.Append("INNER JOIN SOP_OPERATIONS_ROUTES B ON A.ID = B.ROUTE_ID ");
                sql.Append("WHERE A.PART_NO = :PART_NO AND CURRENT_OPERATION_ID = :OPERATION_ID AND A.STATUS = 1 ");

                var dtResult = (await _dbConnection.QueryAsync<dynamic>(sql.ToString(), new { PART_NO = PART_NO, OPERATION_ID = OPERATION_ID }))?.ToList();
                if (dtResult == null || dtResult.Count == 0)
                {
                    result = 0;
                }
                else if (dtResult.FirstOrDefault().STANDARD_CAPACITY == DBNull.Value)
                {
                    result = 0;
                }
                else
                {
                    result = Convert.ToDecimal(dtResult.FirstOrDefault().STANDARD_CAPACITY);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 获取报工的制程
        /// </summary>
        /// <param name="wo_id"></param>
        /// <returns></returns>
        public async Task<List<SfcsRoutesSiteListModel>> GetRouteCapacityDataByWoId(decimal wo_id, decimal route_id)
        {
            List<SfcsRoutesSiteListModel> reuslt = new List<SfcsRoutesSiteListModel>();
            try
            {
                String sQuery = @"SELECT SRC.CURRENT_OPERATION_ID,SRC.ORDER_NO,T.PASS FROM SFCS_ROUTE_CONFIG SRC LEFT JOIN (SELECT SRC.CURRENT_OPERATION_ID,SRC.ORDER_NO,T.QTY AS PASS FROM SFCS_ROUTE_CONFIG SRC,SFCS_WO SW,(SELECT SSS.WO_ID,SOS.OPERATION_ID,(SUM(SSS.PASS) + SUM(SSS.FAIL)) QTY FROM SFCS_SITE_STATISTICS SSS,SFCS_OPERATION_SITES SOS WHERE SSS.OPERATION_SITE_ID = SOS.ID AND SSS.WO_ID = :WO_ID GROUP BY SSS.WO_ID,SOS.OPERATION_ID)T WHERE SRC.CURRENT_OPERATION_ID = T.OPERATION_ID AND SRC.ROUTE_ID = SW.ROUTE_ID AND SW.ID =:WO_ID)T ON SRC.CURRENT_OPERATION_ID = T.CURRENT_OPERATION_ID WHERE SRC.ROUTE_ID = :ROUTE_ID ORDER BY SRC.ORDER_NO DESC ";
                reuslt = (await _dbConnection.QueryAsync<SfcsRoutesSiteListModel>(sQuery, new { WO_ID = wo_id, ROUTE_ID = route_id }))?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reuslt;
        }

        /// <summary>
        /// 更新产出数量
        /// </summary>
        /// <param name="wo_id">工单ID</param>
        /// <param name="qty">报工数量</param>
        /// <returns></returns>
        public async Task<bool> UpdateOutputData(SfcsWo woModel, decimal qty, decimal? lineId)
        {
            bool result = false;
            try
            {
                string sql = "";
                //瑞晶项目不统计smt_produciton
                //String smtProductionSql = " UPDATE SMT_PRODUCTION SET OUTPUT_QTY=OUTPUT_QTY+:OUTPUT_QTY WHERE WO_NO=:WO_NO AND LINE_ID=:LINE_ID AND FINISHED!='Y' "; 
                String sfcsProductionSql = " UPDATE SFCS_PRODUCTION SET OUTPUT_QTY=OUTPUT_QTY+:OUTPUT_QTY WHERE WO_NO=:WO_NO AND LINE_ID=:LINE_ID AND FINISHED!='Y' ";

                if (woModel.TARGET_QTY == (woModel.OUTPUT_QTY + qty))
                {
                    //修改完工状态
                    sql = " UPDATE SFCS_WO SET OUTPUT_QTY=OUTPUT_QTY+:OUTPUT_QTY,WO_STATUS=4 WHERE ID=:ID ";
                }
                else
                {
                    sql = " UPDATE SFCS_WO SET OUTPUT_QTY=OUTPUT_QTY+:OUTPUT_QTY WHERE ID=:ID ";
                }
                //await _dbConnection.ExecuteAsync(smtProductionSql, new
                //{
                //    OUTPUT_QTY = qty,
                //    WO_NO= woModel.WO_NO,
                //    LINE_ID = lineId
                //});

                await _dbConnection.ExecuteAsync(sfcsProductionSql, new
                {
                    OUTPUT_QTY = qty,
                    WO_NO = woModel.WO_NO,
                    LINE_ID = lineId
                });
                var data = await _dbConnection.ExecuteAsync(sql, new
                {
                    OUTPUT_QTY = qty,
                    ID = woModel.ID
                });

                result = data <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}