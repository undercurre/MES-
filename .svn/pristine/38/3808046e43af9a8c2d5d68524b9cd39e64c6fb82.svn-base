/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-17 11:14:59                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsStoplineHistoryRepository                                      
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
using System.Data;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsStoplineHistoryRepository : BaseRepository<SfcsStoplineHistory, Decimal>, ISfcsStoplineHistoryRepository
    {
        public SfcsStoplineHistoryRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT SFCS_STOPLINE_HISTORY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取停线不良统计
        /// </summary>
        /// <param name="headerID"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetStopLineDefectStatistics(decimal headerID)
        {
            string sql = @"SELECT SCD.DEFECT_CODE, SDC.DEFECT_DESCRIPTION, 
                            COUNT(* ) COUNT
                            FROM   SFCS_STOPLINE_BATCH_DETAIL SSBD,
                            SFCS_COLLECT_DEFECTS SCD,
                            SFCS_DEFECT_CONFIG SDC
                            WHERE SSBD.HEADER_ID = :HEADER_ID
                            AND SSBD.OPERATION_ID = SCD.DEFECT_OPERATION_ID
                            AND SSBD.STATUS = 2
                            AND SCD.DEFECT_CODE = SDC.DEFECT_CODE(+)
                            GROUP BY SCD.DEFECT_CODE, SDC.DEFECT_DESCRIPTION
                            ORDER BY COUNT DESC";
            var result = await _dbConnection.QueryAsync<dynamic>(sql, new { HEADER_ID= headerID });

            return result?.ToList();
        }

        /// <summary>
        /// 获取停线所有流水号
        /// </summary>
        /// <param name="headerID"></param>
        /// <param name="classification">厂部</param>
        /// <returns></returns>
        public async Task<List<StopLineDefectDetail>> GetStopLineDefectDetail(decimal headerID, decimal classification)
        {
            string sql = @"SELECT SSBH.BATCH_NO, SSBD.SN, SCD.DEFECT_CODE, SDC.DEFECT_DESCRIPTION,
                             SCD.DEFECT_TIME, SCD.COLLECT_DEFECT_DETAIL_ID, SRR.REASON_CODE, SRR.LOCATION, SCD.REPAIR_TIME
                           FROM SFCS_STOPLINE_BATCH_HEADER SSBH,
                              SFCS_STOPLINE_BATCH_DETAIL SSBD,
                              SFCS_COLLECT_DEFECTS SCD,
                              SFCS_REPAIR_RECIPE SRR,
                              SFCS_DEFECT_CONFIG SDC
                           WHERE SSBH.ID = :HEADER_ID
                             AND SDC.DEFECT_CLASS = :DEFECT_CLASS
                             AND SSBH.ID = SSBD.HEADER_ID
                             AND SSBD.OPERATION_ID = SCD.DEFECT_OPERATION_ID
                             AND SCD.DEFECT_CODE = SDC.DEFECT_CODE(+)
                             AND SCD.COLLECT_DEFECT_ID = SRR.COLLECT_DEFECT_ID(+)";
            var result = await _dbConnection.QueryAsync<StopLineDefectDetail>(sql, new { HEADER_ID = headerID , DEFECT_CLASS = classification });

            return result?.ToList();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> LoadData(SfcsStoplineHistoryRequestModel model)
        {
            string condition = " ";
            if (!string.IsNullOrWhiteSpace(model.BATCH_NO))
            {
                condition += $" and INSTR(SSH.BATCH_NO,:BATCH_NO)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.PART_NO))
            {
                condition += $" and INSTR(SP.PART_NO,:PART_NO)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.OPERATION_SITE_NAME))
            {
                condition += $" and INSTR(SOS.OPERATION_SITE_NAME,:OPERATION_SITE_NAME)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.STATUS))
            {
                condition += $" and (SHPH.STATUS =:STATUS) ";
            }
            string sql = @"SELECT ROW_NUMBER() OVER(ORDER BY SSH.ID) AS ROWNO, SSH.ID, SSH.HEADER_ID, SSH.BATCH_NO, SM.MODEL, SSH.PART_NO,
				SOS.OPERATION_SITE_NAME, PASS_COUNT, FAIL_COUNT, NDF_COUNT, TOTAL_COUNT,
				FAIL_RATE, NDF_RATE, DIVISION_CRITERIA, INCLUDE_NDF, STOPLINE_MODE,
				ISSUE_TYPE, ISSUE_MESSAGE, ISSUE_TIME, SHPH.STATUS LOCK_STATUS,
				SHPH.RELEASE_TIME, SSMH.MAINTAIN_BY, SPP.MEANING MAINTAIN_STATUS FROM SFCS_STOPLINE_HISTORY SSH,
				SFCS_HOLD_PRODUCT_HEADER SHPH, SFCS_PN SP, SFCS_MODEL SM,
				SFCS_OPERATION_SITES SOS, SFCS_STOPLINE_MAINTAIN_HISTORY SSMH, SFCS_PARAMETERS SPP
            WHERE SSH.PART_NO = SP.PART_NO AND SSH.ISSUE_TYPE = 2 AND SP.MODEL_ID = SM.ID
				AND SSH.OPERATION_SITE_ID = SOS.ID AND SSH.BATCH_NO = SHPH.HOLD_NUMBER
				AND SPP.LOOKUP_TYPE = 'STOPLINE_MAINTAIN_STATUS'
				AND SPP.LOOKUP_CODE = SSMH.MAINTAIN_STATUS AND SSH.ID = SSMH.STOPLINE_HISTORY_ID " + condition + @"
			UNION SELECT ROW_NUMBER() OVER(ORDER BY SSH.ID) AS ROWNO, SSH.ID, SSH.HEADER_ID, SSH.BATCH_NO, SM.MODEL, SSH.PART_NO,
				SOS.OPERATION_SITE_NAME, PASS_COUNT, FAIL_COUNT, NDF_COUNT, TOTAL_COUNT,
				FAIL_RATE, NDF_RATE, DIVISION_CRITERIA, INCLUDE_NDF, STOPLINE_MODE,
				ISSUE_TYPE, ISSUE_MESSAGE, ISSUE_TIME, SHPH.STATUS LOCK_STATUS,
				SHPH.RELEASE_TIME,'', 'Open' MAINTAIN_STATUS FROM SFCS_STOPLINE_HISTORY SSH,
				SFCS_HOLD_PRODUCT_HEADER SHPH, SFCS_PN SP, SFCS_MODEL SM,
				SFCS_OPERATION_SITES SOS
            WHERE SSH.PART_NO = SP.PART_NO AND SSH.ISSUE_TYPE = 2 AND SP.MODEL_ID = SM.ID
				AND SSH.OPERATION_SITE_ID = SOS.ID AND SSH.BATCH_NO = SHPH.HOLD_NUMBER
				AND SHPH.STATUS = 'Y'" + condition;

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql);
            var resdata = await _dbConnection.QueryAsync<SfcsStoplineHistoryListModel>(pagedSql, model);

            string sqlcnt = @"SELECT count(0) FROM SFCS_STOPLINE_HISTORY SSH,
				SFCS_HOLD_PRODUCT_HEADER SHPH, SFCS_PN SP, SFCS_MODEL SM,
				SFCS_OPERATION_SITES SOS, SFCS_STOPLINE_MAINTAIN_HISTORY SSMH, SFCS_PARAMETERS SPP
            WHERE SSH.PART_NO = SP.PART_NO AND SSH.ISSUE_TYPE = 2 AND SP.MODEL_ID = SM.ID
				AND SSH.OPERATION_SITE_ID = SOS.ID AND SSH.BATCH_NO = SHPH.HOLD_NUMBER
				AND SPP.LOOKUP_TYPE = 'STOPLINE_MAINTAIN_STATUS'
				AND SPP.LOOKUP_CODE = SSMH.MAINTAIN_STATUS AND SSH.ID = SSMH.STOPLINE_HISTORY_ID " + condition;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            sqlcnt = @"SELECT count(0) FROM SFCS_STOPLINE_HISTORY SSH,
				SFCS_HOLD_PRODUCT_HEADER SHPH, SFCS_PN SP, SFCS_MODEL SM,
				SFCS_OPERATION_SITES SOS
            WHERE SSH.PART_NO = SP.PART_NO AND SSH.ISSUE_TYPE = 2 AND SP.MODEL_ID = SM.ID
				AND SSH.OPERATION_SITE_ID = SOS.ID AND SSH.BATCH_NO = SHPH.HOLD_NUMBER
				AND SHPH.STATUS = 'Y'" + condition;
            int cnt2 = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt + cnt2,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="hasMaintainHistory"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsStoplineMaintainHistoryAddOrModifyModel model, bool hasMaintainHistory)
        {
            int result = 1;
            string conditions = string.Empty;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (hasMaintainHistory)
                    {
                        //写入日志
                        string I_InsertStopLineMaintainHistoryLog = @"INSERT INTO JZMES_LOG.SFCS_STOPLINE_MAINTAIN_HISTORY 
							 SELECT * FROM SFCS_STOPLINE_MAINTAIN_HISTORY WHERE STOPLINE_HISTORY_ID =:STOPLINE_HISTORY_ID ";
                        await _dbConnection.ExecuteAsync(I_InsertStopLineMaintainHistoryLog, new
                        {
                            model.STOPLINE_HISTORY_ID,
                        }, tran);
                        //更新维护记录
                        string U_UpdateStopLineMaintainHistory = @"UPDATE SFCS_STOPLINE_MAINTAIN_HISTORY SET 
							 ROOT_CAUSE= :ROOT_CAUSE, SOLUTION=:SOLUTION, RESPONSIBILITY = :RESPONSIBILITY,
							 MAINTAIN_STATUS = :MAINTAIN_STATUS, MAINTAIN_BY = :MAINTAIN_BY ,
							 MAINTAIN_TIME = SYSDATE WHERE STOPLINE_HISTORY_ID = :STOPLINE_HISTORY_ID ";
                        await _dbConnection.ExecuteAsync(U_UpdateStopLineMaintainHistory, new
                        {
                            model.STOPLINE_HISTORY_ID,
                            model.ROOT_CAUSE,
                            model.SOLUTION,
                            model.RESPONSIBILITY,
                            model.MAINTAIN_STATUS,
                            model.MAINTAIN_BY,
                        }, tran);
                    }
                    else
                    {
                        string I_InsertStopLineMaintainHistory = @"INSERT INTO SFCS_STOPLINE_MAINTAIN_HISTORY (ID, STOPLINE_HISTORY_ID,
							 ROOT_CAUSE, SOLUTION, RESPONSIBILITY, MAINTAIN_STATUS, MAINTAIN_BY)
							 VALUES (SFCS_STOPLINE_MAINTAIN_HIS_SEQ.NEXTVAL, :STOPLINE_HISTORY_ID,
							 :ROOT_CAUSE, :SOLUTION, :RESPONSIBILITY, :MAINTAIN_STATUS, :MAINTAIN_BY) ";
                        await _dbConnection.ExecuteAsync(I_InsertStopLineMaintainHistory, new
                        {
                            model.STOPLINE_HISTORY_ID,
                            model.ROOT_CAUSE,
                            model.SOLUTION,
                            model.RESPONSIBILITY,
                            model.MAINTAIN_STATUS,
                            model.MAINTAIN_BY,
                        }, tran);
                    }

                    if (model.MAINTAIN_STATUS != GlobalVariables.Open)
                    {
                        SfcsStoplineHistory stoplineHistory = await GetAsync(model.STOPLINE_HISTORY_ID);

                        conditions = "WHERE HOLD_NUMBER =:HOLD_NUMBER ";
                        List<SfcsHoldProductHeader> holdProductHeaderTable = (await GetListAsyncEx<SfcsHoldProductHeader>(conditions, new
                        {
                            HOLD_NUMBER = stoplineHistory?.BATCH_NO
                        }, tran))?.ToList();

                        foreach (var holdProductHeaderRow in holdProductHeaderTable)
                        {
                            if (holdProductHeaderRow.STATUS == GlobalVariables.EnableY)
                            {
                                string U_UpdateHoldProductHeader = @"UPDATE SFCS_HOLD_PRODUCT_HEADER SET RELEASE_CAUSE=:RELEASE_CAUSE, STATUS='N',
                                        RELEASE_EMPNO=:RELEASE_EMPNO, RELEASE_TIME=SYSDATE 
                                      WHERE HOLD_NUMBER=:HOLD_NUMBER ";
                                await _dbConnection.ExecuteAsync(U_UpdateHoldProductHeader, new
                                {
                                    RELEASE_CAUSE = model.ROOT_CAUSE,
                                    RELEASE_EMPNO = model.MAINTAIN_BY,
                                    HOLD_NUMBER = stoplineHistory?.BATCH_NO,
                                }, tran);

                                string U_UpdateHoldProductDetail = @"UPDATE SFCS_HOLD_PRODUCT_DETAIL SET STATUS='N',RELEASE_CAUSE = :RELEASE_CAUSE,
                                        RELEASE_EMPNO = :RELEASE_EMPNO,RELEASE_TIME=SYSDATE 
                                    WHERE HOLD_ID=:HOLD_ID AND STATUS='Y' ";
                                await _dbConnection.ExecuteAsync(U_UpdateHoldProductDetail, new
                                {
                                    RELEASE_CAUSE = model.ROOT_CAUSE,
                                    RELEASE_EMPNO = model.MAINTAIN_BY,
                                    HOLD_ID = holdProductHeaderRow.ID,
                                }, tran);
                            }
                            else
                            {
                                // 仍有未解鎖的Detail，繼續解鎖
                                SfcsHoldProductDetail holdProductDetail = await GetAsyncEx<SfcsHoldProductDetail>("Where HOLD_ID =:HOLD_ID and STATUS='Y'", new
                                { HOLD_ID = holdProductHeaderRow.ID }, tran);
                                if (holdProductDetail != null)
                                {
                                    string U_UpdateHoldProductDetail = @"UPDATE SFCS_HOLD_PRODUCT_DETAIL SET STATUS='N',RELEASE_CAUSE = :RELEASE_CAUSE,
                                        RELEASE_EMPNO = :RELEASE_EMPNO,RELEASE_TIME=SYSDATE 
                                    WHERE HOLD_ID=:HOLD_ID AND STATUS='Y' ";
                                    await _dbConnection.ExecuteAsync(U_UpdateHoldProductDetail, new
                                    {
                                        RELEASE_CAUSE = model.ROOT_CAUSE,
                                        RELEASE_EMPNO = model.MAINTAIN_BY,
                                        HOLD_ID = holdProductHeaderRow.ID,
                                    }, tran);
                                }
                            }
                        }

                        //初始解鎖發送郵件 InitialClosedSendMail();
                        SfcsStoplineBatchHeader batchHeader = await GetAsyncEx<SfcsStoplineBatchHeader>("WHERE ID=:ID", new { ID = stoplineHistory?.HEADER_ID }, tran);
                        if (batchHeader != null)
                        {
                            SfcsOperationSites stopLineSite = await GetAsyncEx<SfcsOperationSites>("WHERE ID=:ID", new { ID = batchHeader?.OPERATION_SITE_ID }, tran);
                            if (stopLineSite != null)
                            {
                                List<SfcsLookups> mailConfig = (await GetListAsyncEx<SfcsLookups>("WHERE KIND=:KIND AND CODE =:CODE ",
                                    new { KIND = GlobalVariables.StopLine_Initial_Closed_Mail_By_Operation, CODE = stopLineSite.OPERATION_ID }, tran))?.ToList();

                                //机种：{0}，料号{1} ，已Initial Closed，锁定单据号：{2}，请分析锁定原因。 
                                //string subject = string.Format(Properties.Resources.Msg_Initial_Closed_Subject,
                                //    this.historyRow.MODEL, this.historyRow.PART_NO, this.historyRow.BATCH_NO);
                                string subject = string.Format("机种：{0}，料号{1} ，已Initial Closed，锁定单据号：{2}，请分析锁定原因。",
                                   model.MODEL, stoplineHistory.PART_NO, stoplineHistory.BATCH_NO);

                                //string content = string.Format(Properties.Resources.Msg_Initial_Closed_Content,
                                //    this.historyRow.MODEL, this.historyRow.PART_NO, this.historyRow.OPERATION_SITE_NAME,
                                //    this.historyRow.ISSUE_MESSAGE, this.historyRow.ISSUE_TIME, this.historyRow.BATCH_NO);
                                //机种：{0}，料号{1} ，锁定站定：{2},锁定原因：{3}，锁定时间:{4}，锁定单据号：{5}，请到SFCS停线解锁分析维护填写原因及责任归属。
                                string content = string.Format("机种：{0}，料号{1} ，锁定站定：{2},锁定原因：{3}，锁定时间:{4}，锁定单据号：{5}，请到SFCS停线解锁分析维护填写原因及责任归属。",
                                    model.MODEL, stoplineHistory.PART_NO, model.OPERATION_SITE_NAME, stoplineHistory.ISSUE_MESSAGE,
                                    stoplineHistory.ISSUE_TIME, stoplineHistory.BATCH_NO);

                                foreach (var mailConfigItem in mailConfig)
                                {
                                    //發送郵件給指定用戶
                                    //messageLogger.SendMailByMembers(mailConfigRow.CATEGORY, subject, content, null, null, null);
                                    var p = new DynamicParameters();
                                    p.Add(":P_MAIL_TO", mailConfigItem.CATEGORY, DbType.String, ParameterDirection.Input, 200);
                                    p.Add(":P_SUBJECT", subject, DbType.String, ParameterDirection.Input, 200);
                                    p.Add(":P_CONTENT", content, DbType.String, ParameterDirection.Input, 200);
                                    p.Add(":P_MAIL_FROM", "", DbType.String, ParameterDirection.Input, 60);
                                    p.Add(":P_MAIL_CC", "", DbType.String, ParameterDirection.Input, 50);
                                    p.Add(":P_MAIL_BCC", "", DbType.String, ParameterDirection.Input, 50);

                                    await _dbConnection.ExecuteAsync("SFCS_MESSAGE_PKG.CALL_MESSAGER_BY_WAF", p, tran, commandType: CommandType.StoredProcedure);
                                }
                            }
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