/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：制程品质异常停线通知单表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-11-02 11:09:31                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesIpqaStopNoticeRepository                                      
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
    public class MesIpqaStopNoticeRepository:BaseRepository<MesIpqaStopNotice,Decimal>, IMesIpqaStopNoticeRepository
    {
        public MesIpqaStopNoticeRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_IPQA_STOP_NOTICE WHERE ID=:ID";
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
			string sql = "UPDATE MES_IPQA_STOP_NOTICE set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_IPQA_STOP_NOTICE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_IPQA_STOP_NOTICE where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MesIpqaStopNoticeModel model)
		{
			decimal result = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MES_IPQA_STOP_NOTICE 
					(ID,ORGANIZE_ID,LINE_ID,PRODUCTION_DATE,WO_NO,PCB_PN,MODEL,DESCRIPTION,TOTAL_QTY,FINISHED_QTY,CREATE_DEP_ID,CREATE_USER,CREATE_TIME,RECEIPT_DEP_ID,RECEIPT_USER,FEEDBACK_TIME,PRACTICAL_TIME,EXCEPTION_DESCRIPTION,EXCEPTION_FAIL_INFO,EXCEPTION_FAIL_RATE,EXCEPTION_IPQA,EXCEPTION_IPQA_HEAD,ANALYSIS_OPINION,ANALYSIS_REASON,ANALYSIS_QE,ANALYSIS_QE_HEAD,SOLUTION_METHOD,SOLUTION_SIGN,SOLUTION_DATE,EFFECT_TRACKING,EFFECT_IPQA,EFFECT_DATE,RESUME_NOTICE,FILE_CODE,STATUS,AUDIT_USER,AUDIT_TIME,AUDIT_CONTENT,APPROVAL_USER,APPROVAL_TIME,APPROVAL_CONTENT,UPDATE_USER,UPDATE_TIME,ATTRIBUTE1,ATTRIBUTE2,ATTRIBUTE3,ATTRIBUTE4,ATTRIBUTE5) 
					VALUES (:ID,:ORGANIZE_ID,:LINE_ID,:PRODUCTION_DATE,:WO_NO,:PCB_PN,:MODEL,:DESCRIPTION,:TOTAL_QTY,:FINISHED_QTY,:CREATE_DEP_ID,:CREATE_USER,:CREATE_TIME,:RECEIPT_DEP_ID,:RECEIPT_USER,:FEEDBACK_TIME,:PRACTICAL_TIME,:EXCEPTION_DESCRIPTION,:EXCEPTION_FAIL_INFO,:EXCEPTION_FAIL_RATE,:EXCEPTION_IPQA,:EXCEPTION_IPQA_HEAD,:ANALYSIS_OPINION,:ANALYSIS_REASON,:ANALYSIS_QE,:ANALYSIS_QE_HEAD,:SOLUTION_METHOD,:SOLUTION_SIGN,:SOLUTION_DATE,:EFFECT_TRACKING,:EFFECT_IPQA,:EFFECT_DATE,:RESUME_NOTICE,:FILE_CODE,:STATUS,:AUDIT_USER,:AUDIT_TIME,:AUDIT_CONTENT,:APPROVAL_USER,:APPROVAL_TIME,:APPROVAL_CONTENT,:UPDATE_USER,:UPDATE_TIME,:ATTRIBUTE1,:ATTRIBUTE2,:ATTRIBUTE3,:ATTRIBUTE4,:ATTRIBUTE5)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.ORGANIZE_ID,
								item.LINE_ID,
								item.PRODUCTION_DATE,
								item.WO_NO,
								item.PCB_PN,
								item.MODEL,
								item.DESCRIPTION,
								item.TOTAL_QTY,
								item.FINISHED_QTY,
								item.CREATE_DEP_ID,
								item.CREATE_USER,
								item.CREATE_TIME,
								item.RECEIPT_DEP_ID,
								item.RECEIPT_USER,
								item.FEEDBACK_TIME,
								item.PRACTICAL_TIME,
								item.EXCEPTION_DESCRIPTION,
								item.EXCEPTION_FAIL_INFO,
								item.EXCEPTION_FAIL_RATE,
								item.EXCEPTION_IPQA,
								item.EXCEPTION_IPQA_HEAD,
								item.ANALYSIS_OPINION,
								item.ANALYSIS_REASON,
								item.ANALYSIS_QE,
								item.ANALYSIS_QE_HEAD,
								item.SOLUTION_METHOD,
								item.SOLUTION_SIGN,
								item.SOLUTION_DATE,
								item.EFFECT_TRACKING,
								item.EFFECT_IPQA,
								item.EFFECT_DATE,
								item.RESUME_NOTICE,
								item.FILE_CODE,
								item.STATUS,
								item.AUDIT_USER,
								item.AUDIT_TIME,
								item.AUDIT_CONTENT,
								item.APPROVAL_USER,
								item.APPROVAL_TIME,
								item.APPROVAL_CONTENT,
								item.UPDATE_USER,
								item.UPDATE_TIME,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
							}, tran);
							result = newid;
						}
					}
					//更新
					string updateSql = @"Update MES_IPQA_STOP_NOTICE set ORGANIZE_ID=:ORGANIZE_ID,LINE_ID=:LINE_ID,PRODUCTION_DATE=:PRODUCTION_DATE,WO_NO=:WO_NO,PCB_PN=:PCB_PN,MODEL=:MODEL,DESCRIPTION=:DESCRIPTION,TOTAL_QTY=:TOTAL_QTY,FINISHED_QTY=:FINISHED_QTY,CREATE_DEP_ID=:CREATE_DEP_ID,CREATE_USER=:CREATE_USER,CREATE_TIME=:CREATE_TIME,RECEIPT_DEP_ID=:RECEIPT_DEP_ID,RECEIPT_USER=:RECEIPT_USER,FEEDBACK_TIME=:FEEDBACK_TIME,PRACTICAL_TIME=:PRACTICAL_TIME,EXCEPTION_DESCRIPTION=:EXCEPTION_DESCRIPTION,EXCEPTION_FAIL_INFO=:EXCEPTION_FAIL_INFO,EXCEPTION_FAIL_RATE=:EXCEPTION_FAIL_RATE,EXCEPTION_IPQA=:EXCEPTION_IPQA,EXCEPTION_IPQA_HEAD=:EXCEPTION_IPQA_HEAD,ANALYSIS_OPINION=:ANALYSIS_OPINION,ANALYSIS_REASON=:ANALYSIS_REASON,ANALYSIS_QE=:ANALYSIS_QE,ANALYSIS_QE_HEAD=:ANALYSIS_QE_HEAD,SOLUTION_METHOD=:SOLUTION_METHOD,SOLUTION_SIGN=:SOLUTION_SIGN,SOLUTION_DATE=:SOLUTION_DATE,EFFECT_TRACKING=:EFFECT_TRACKING,EFFECT_IPQA=:EFFECT_IPQA,EFFECT_DATE=:EFFECT_DATE,RESUME_NOTICE=:RESUME_NOTICE,FILE_CODE=:FILE_CODE,STATUS=:STATUS,AUDIT_USER=:AUDIT_USER,AUDIT_TIME=:AUDIT_TIME,AUDIT_CONTENT=:AUDIT_CONTENT,APPROVAL_USER=:APPROVAL_USER,APPROVAL_TIME=:APPROVAL_TIME,APPROVAL_CONTENT=:APPROVAL_CONTENT,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=:UPDATE_TIME,ATTRIBUTE1=:ATTRIBUTE1,ATTRIBUTE2=:ATTRIBUTE2,ATTRIBUTE3=:ATTRIBUTE3,ATTRIBUTE4=:ATTRIBUTE4,ATTRIBUTE5=:ATTRIBUTE5  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.ORGANIZE_ID,
								item.LINE_ID,
								item.PRODUCTION_DATE,
								item.WO_NO,
								item.PCB_PN,
								item.MODEL,
								item.DESCRIPTION,
								item.TOTAL_QTY,
								item.FINISHED_QTY,
								item.CREATE_DEP_ID,
								item.CREATE_USER,
								item.CREATE_TIME,
								item.RECEIPT_DEP_ID,
								item.RECEIPT_USER,
								item.FEEDBACK_TIME,
								item.PRACTICAL_TIME,
								item.EXCEPTION_DESCRIPTION,
								item.EXCEPTION_FAIL_INFO,
								item.EXCEPTION_FAIL_RATE,
								item.EXCEPTION_IPQA,
								item.EXCEPTION_IPQA_HEAD,
								item.ANALYSIS_OPINION,
								item.ANALYSIS_REASON,
								item.ANALYSIS_QE,
								item.ANALYSIS_QE_HEAD,
								item.SOLUTION_METHOD,
								item.SOLUTION_SIGN,
								item.SOLUTION_DATE,
								item.EFFECT_TRACKING,
								item.EFFECT_IPQA,
								item.EFFECT_DATE,
								item.RESUME_NOTICE,
								item.FILE_CODE,
								item.STATUS,
								item.AUDIT_USER,
								item.AUDIT_TIME,
								item.AUDIT_CONTENT,
								item.APPROVAL_USER,
								item.APPROVAL_TIME,
								item.APPROVAL_CONTENT,
								item.UPDATE_USER,
								item.UPDATE_TIME,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
							}, tran);
							result = item.ID;
						}
					}
					////删除
					//string deleteSql = @"Delete from MES_IPQA_STOP_NOTICE where ID=:ID ";
					//if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					//{
					//	foreach (var item in model.RemoveRecords)
					//	{
					//		var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
					//		{
					//			item.ID
					//		}, tran);
					//	}
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




		/// <summary>
		/// 数据分页查询
		/// </summary>
		public async Task<TableDataModel> LoadDataPagedList(MesIpqaStopNoticeRequestModel model)
		{
			string conditions = " WHERE A.ID > 0 ";
            if (model.ID.HasValue && model.ID.Value > 0)
            {
                conditions += $"and (A.ID = :ID) ";
            }
            if (model.LINE_ID.HasValue && model.LINE_ID.Value > 0)
            {
                conditions += $"and (A.LINE_ID = :LINE_ID) ";
            }
            if (!model.PCB_PN.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(A.PCB_PN, :PCB_PN) > 0) ";
            }

            string sql = @"SELECT ROWNUM AS rowno, A.*, L.LINE_NAME, CD.DEP_NAME AS CREATE_DEP_NAME, RD.DEP_NAME AS RECEIPT_DEP_NAME
						FROM MES_IPQA_STOP_NOTICE A
						INNER JOIN V_MES_LINES L ON A.LINE_ID = L.LINE_ID
						INNER JOIN SYS_DEPARTMENT CD ON A.CREATE_DEP_ID = CD.ID
						LEFT JOIN SYS_DEPARTMENT RD ON A.RECEIPT_DEP_ID = CD.ID ";

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "A.ID desc", conditions);
			var resdata = await _dbConnection.QueryAsync<MesIpqaStopNoticeListModel>(pagedSql, model);
			string sqlcnt = @"SELECT COUNT(0) FROM MES_IPQA_STOP_NOTICE A
						INNER JOIN V_MES_LINES L ON A.LINE_ID = L.LINE_ID
						INNER JOIN SYS_DEPARTMENT CD ON A.CREATE_DEP_ID = CD.ID
						LEFT JOIN SYS_DEPARTMENT RD ON A.RECEIPT_DEP_ID = CD.ID " + conditions;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 线体数据
		/// </summary>
		/// <param name="organizeId"></param>
		/// <param name="lineType">SMT</param>
		/// <returns></returns>
		public async Task<TableDataModel> GetLinesList(string organizeId = "1", string lineType = "")
        {
			if (string.IsNullOrEmpty(organizeId)) organizeId = "1";
			string sql = @" SELECT L.*
							FROM V_MES_LINES L INNER JOIN SYS_ORGANIZE_LINE O ON L.LINE_ID = O.LINE_ID {0}
							WHERE EXISTS
									(SELECT 1
										FROM (    SELECT ID
													FROM SYS_ORGANIZE
												START WITH ID = :ORGANIZE_ID
												CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
										WHERE ID = O.ORGANIZE_ID)
						ORDER BY O.ORGANIZE_ID, O.LINE_TYPE, O.ATTRIBUTE6, TO_NUMBER (O.ATTRIBUTE5)";

			string sSqlLineType = "";
			if (lineType == "SMT" || lineType == "PCBA")
			{
				sSqlLineType = " AND L.LINE_TYPE = '" + lineType + "' ";
			}
			var resdata = await _dbConnection.QueryAsync<MesIpqaStopNoticeLinesResultModel>(string.Format(sql, sSqlLineType), new { ORGANIZE_ID = organizeId });

			string sqlcnt = @"SELECT COUNT(0) FROM V_MES_LINES L INNER JOIN SYS_ORGANIZE_LINE O ON L.LINE_ID = O.LINE_ID {0}
							WHERE EXISTS
									(SELECT 1
										FROM (    SELECT ID
													FROM SYS_ORGANIZE
												START WITH ID = :ORGANIZE_ID
												CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
										WHERE ID = O.ORGANIZE_ID)
						ORDER BY O.ORGANIZE_ID, O.LINE_TYPE, O.ATTRIBUTE6, TO_NUMBER (O.ATTRIBUTE5) ";

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(string.Format(sqlcnt, sSqlLineType), new { ORGANIZE_ID = organizeId });
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 获取单据状态
		/// </summary>
		public async Task<decimal> GetBillStatus(decimal id)
		{
			string sql = "SELECT STATUS FROM MES_IPQA_STOP_NOTICE WHERE ID = :id ";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new
			{
				id,
			});
			return result;
		}

		/// <summary>
		/// 根据工单号获取产品信息
		/// </summary>
		/// <param name="wo_no">工单号</param>
		/// <returns></returns>
		public async Task<PartModel> GetPartDataByWoNo(string wo_no)
		{
			string sql = @" SELECT T1.ID,T1.WO_NO,T1.PART_NO,T1.MODEL AS PART_NAME,T1.DESCRIPTION AS PART_DESC,T2.TARGET_QTY FROM SMT_WO T1,SFCS_WO T2 WHERE T1.WO_NO = T2.WO_NO AND T1.WO_NO = :WONO";

			return (await _dbConnection.QueryAsync<PartModel>(sql, new { WONO = wo_no })).FirstOrDefault();
		}


		/// <summary>
		///获取当前线别在线工单 
		/// </summary>
		/// <param name="line_id">线别ID</param>
		/// <returns></returns>
		public async Task<string> GetWoNoByLineId(decimal line_id)
        {
			string sql = @"SELECT WO_NO
						  FROM (SELECT LINE_ID, WO_NO
								  FROM SMT_PRODUCTION
								 WHERE FINISHED = 'N'
								UNION
								SELECT LINE_ID, WO_NO
								  FROM SFCS_PRODUCTION
								 WHERE FINISHED = 'N') TAB1
						 WHERE TAB1.LINE_ID = :LINE_ID";

			return (await _dbConnection.ExecuteScalarAsync<string>(sql, new { LINE_ID = line_id }));
		}


		/// <summary>
		/// 审核
		/// </summary>
		public async Task<decimal> AuditBill(MesIpqaStopNoticeAuditBillRequestModel model)
        {
			string sql = "UPDATE MES_IPQA_STOP_NOTICE SET STATUS = 1, AUDIT_USER= :AUDIT_USER, AUDIT_TIME = SYSDATE, AUDIT_CONTENT = :AUDIT_CONTENT WHERE ID=:ID";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				AUDIT_USER = model.AUDIT_USER,
				AUDIT_CONTENT = model.AUDIT_CONTENT,
				model.ID
			});
		}

		/// <summary>
		/// 批准
		/// </summary>
		public async Task<decimal> ApprovalBill(MesIpqaStopNoticeApprovalBillRequestModel model)
		{
			string sql = "UPDATE MES_IPQA_STOP_NOTICE SET STATUS = 2, APPROVAL_USER= :APPROVAL_USER, APPROVAL_TIME = SYSDATE, APPROVAL_CONTENT = :APPROVAL_CONTENT WHERE ID=:ID";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				model.APPROVAL_USER,
				model.APPROVAL_CONTENT,
				model.ID
			});
		}


	}
}