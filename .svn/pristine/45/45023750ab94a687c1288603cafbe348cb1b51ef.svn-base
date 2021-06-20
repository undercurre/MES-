/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-10-15 10:41:26                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtDefectsRecordsRepository                                      
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
    public class SmtDefectsRecordsRepository:BaseRepository<SmtDefectsRecords,Decimal>, ISmtDefectsRecordsRepository
    {
        public SmtDefectsRecordsRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SMT_DEFECTS_RECORDS WHERE ID=:ID";
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
			string sql = "UPDATE SMT_DEFECTS_RECORDS set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SMT_DEFECTS_RECORDS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetSEQID_DTL()
		{
			string sql = "SELECT SMT_DEFECTS_RECORD_DTL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SMT_DEFECTS_RECORDS where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SmtDefectsRecordsModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"INSERT INTO SMT_DEFECTS_RECORDS
  (ID, ORGANIZE_ID, WORK_CLASS, LINE_ID, MODEL, CREATE_TIME, CREATE_USER,REPAIR_USER,REPAIR_TIME,  REPAIR_NO, ORDER_NO, WO_NO, PART_NO, STATUS)
VALUES
(:ID, :ORGANIZE_ID, :WORK_CLASS, :LINE_ID, :MODEL, SYSDATE, :CREATE_USER,:REPAIR_USER, SYSDATE, :REPAIR_NO, :ORDER_NO, :WO_NO, :PART_NO, 1)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							result = (int)newid;
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.ORGANIZE_ID,
								item.WO_NO,
								item.WORK_CLASS,
								item.LINE_ID,
								item.MODEL,
								item.REPAIR_NO,
								item.ORDER_NO,
								item.PART_NO,
								item.REPAIR_USER,
								item.STATUS,
								item.CREATE_TIME,
								item.CREATE_USER,
								item.EXAMINE_TIME,
								item.EXAMINE_USER,
							}, tran);
						}
					}
					//更新
					string updateSql = @"UPDATE SMT_DEFECTS_RECORDS
									   SET 
									       WORK_CLASS = :WORK_CLASS,
									       LINE_ID = :LINE_ID,
									       MODEL = :MODEL,
									       REPAIR_NO = :REPAIR_NO,
									       ORDER_NO = :ORDER_NO,
									       WO_NO = :WO_NO,
									       PART_NO = :PART_NO,
									       STATUS = :STATUS
									 WHERE ID = :ID
									";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							result = (int)item.ID;
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.ORGANIZE_ID,
								item.WO_NO,
								item.WORK_CLASS,
								item.LINE_ID,
								item.MODEL,
								item.REPAIR_NO,
								item.ORDER_NO,
								item.PART_NO,
								item.REPAIR_USER,
								item.STATUS,
								item.CREATE_TIME,
								item.CREATE_USER,
								item.EXAMINE_TIME,
								item.EXAMINE_USER,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SMT_DEFECTS_RECORDS where ID=:ID ";
					if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					{
						foreach (var item in model.RemoveRecords)
						{
							result = (int)item.ID;
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
        /// 查询报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<dynamic> GetReportDefectsRecordsList(string conditions,SmtDefectsRecordsRequestModel model)
        {
            string sql = @"SELECT A.LOCATION,B.DEFECT_DESCRIPTION,A.LOCATION ||' '|| B.DEFECT_DESCRIPTION AS XTITEL,SUM(ALL_QTY) ALL_QTY,SUM(DEFECT_QTY) DEFECT_QTY,
                        ROUND(SUM(DEFECT_QTY)/SUM(ALL_QTY),2) * 100 L_QTY
                        FROM SMT_DEFECTS_RECORDS A
                        LEFT JOIN SFCS_DEFECT_CONFIG B ON A.DEFECT_CODE = B.DEFECT_CODE {0}
                        GROUP BY A.LOCATION ||' '|| B.DEFECT_DESCRIPTION,A.LOCATION,B.DEFECT_DESCRIPTION";
            sql = string.Format(sql, conditions);
            return (await _dbConnection.QueryAsync<dynamic>(sql,model)).ToList();
        }

		/// <summary>
		/// 保存明细数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveReportDefectsRecordDtl(SmtDefectsRecordDtlModel model) {
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"INSERT INTO SMT_DEFECTS_RECORD_DTL
  (ID, MST_ID, SN, LOCATION, DEFECT_CODE, DEFECT_DES, DEFECT_REMARK, IS_OK, REPAIR_USER, REPAIR_TIME, CREATE_TIME, CREATE_USER,QUANTITY)
VALUES
  (:ID, :MST_ID, :SN, :LOCATION, :DEFECT_CODE, :DEFECT_DES, :DEFECT_REMARK, :IS_OK, :REPAIR_USER, SYSDATE, SYSDATE, :CREATE_USER, :QUANTITY)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID_DTL();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.MST_ID,
								item.SN,
								item.LOCATION,
								item.DEFECT_CODE,
								item.DEFECT_DES,
								item.DEFECT_REMARK,
								item.IS_OK,
								item.REPAIR_USER,
								item.CREATE_USER,
								item.QUANTITY
							}, tran);
						}
					}
					//更新
					string updateSql = @"UPDATE SMT_DEFECTS_RECORD_DTL
										   SET SN = :SN,
										       LOCATION = :LOCATION,
										       DEFECT_CODE = :DEFECT_CODE,
										       DEFECT_DES = :DEFECT_DES,
										       DEFECT_REMARK = :DEFECT_REMARK,
										       IS_OK = :IS_OK,
										       REPAIR_USER = :REPAIR_USER,
										       REPAIR_TIME = :REPAIR_TIME,
											   QUANTITY=:QUANTITY
										 WHERE ID = :ID";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.MST_ID,
								item.SN,
								item.LOCATION,
								item.DEFECT_CODE,
								item.DEFECT_DES,
								item.DEFECT_REMARK,
								item.IS_OK,
								item.REPAIR_USER,
								item.REPAIR_TIME,
								item.QUANTITY
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SMT_DEFECTS_RECORD_DTL where ID=:ID ";
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
		/// 获取明细数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<dynamic> GetReportDefectsRecordDtlList(SmtDefectsRecordDtlRequestModel model) {
			var sql = "SELECT * FROM SMT_DEFECTS_RECORD_DTL  WHERE MST_ID=:MST_ID";
			return (await _dbConnection.QueryAsync<dynamic>(sql, model)).ToList();
		}

		/// <summary>
		/// 取消审核
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="repairUser"></param>
		/// <returns></returns>
        public async Task<decimal> CancelCheck(int ID, string examineUser)
        {
			var sql = "UPDATE SMT_DEFECTS_RECORDS SET STATUS = 1,EXAMINE_USER=null,EXAMINE_TIME = null WHERE ID=:ID";
			return (await _dbConnection.ExecuteAsync(sql,new { ID }));
        }
		/// <summary>
		/// 审核
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="examineUser"></param>
		/// <returns></returns>
        public async Task<decimal> Check(int ID, string examineUser)
        {
			var sql = "UPDATE SMT_DEFECTS_RECORDS SET STATUS = 2,EXAMINE_USER=:EXAMINE_USER,EXAMINE_TIME=SYSDATE WHERE ID=:ID";
			return (await _dbConnection.ExecuteAsync(sql, new { ID, EXAMINE_USER = examineUser }));
		}
		/// <summary>
		/// 获取线体的工单信息
		/// </summary>
		/// <param name="LineId"></param>
		/// <returns></returns>
        public async Task<SmtWo> GetWoInfoByLine(string LineId)
        {
			var sql = "SELECT B.* FROM SMT_PRODUCTION A LEFT JOIN SMT_WO B ON A.WO_NO = B.WO_NO WHERE FINISHED='N' AND LINE_ID=:LINE_ID";
			return (await _dbConnection.QueryFirstAsync<SmtWo>(sql, new { LINE_ID = LineId }));
		}
    }
}