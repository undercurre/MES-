/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：换线记录表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-11-15 19:06:24                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesChangeLineRecordRepository                                      
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
using System.Data;
using JZ.IMS.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace JZ.IMS.Repository.Oracle
{
	public class MesChangeLineRecordRepository : BaseRepository<MesChangeLineRecord, Decimal>, IMesChangeLineRecordRepository
	{
		public MesChangeLineRecordRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_CHANGE_LINE_RECORD WHERE ID=:ID AND IS_DELETE='N'";
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
			string sql = "UPDATE MES_CHANGE_LINE_RECORD set ENABLED=:ENABLED where  Id=:Id";
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
			string sql = "SELECT MES_CHANGE_LINE_RECORD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 根据时间段获取换线记录数
		/// </summary>
		/// <param name="lineId">产线ID</param>
		/// <param name="beginTime">开始时间</param>
		/// <param name="endTime">结束时间</param>
		/// <returns></returns>
		public int ExistByDate(decimal id, decimal lineId, DateTime beginTime, DateTime endTime)
		{
			string sql = @"SELECT COUNT(*) FROM MES_CHANGE_LINE_RECORD WHERE LINE_ID=:LINE_ID AND PRE_END_TIME < :EndDate AND NEXT_START_TIME > :BeginDate";
			if (id != 0)
				sql += " AND ID != :ID";

			return _dbConnection.ExecuteScalar<int>(sql, new { LINE_ID = lineId, BeginDate = beginTime, EndDate = endTime, ID = id });
		}

		public async Task<decimal> InsertChangeLineRecordAsync(MesChangeLineRecord model)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (IDbTransaction transaction = _dbConnection.BeginTransaction())
			{ // 创建事务	
				try
				{
					//1.写入数据到MES_CHANGE_LINE_RECORD
					await _dbConnection.InsertAsync(model, transaction);

					DateTime startTimeTmp = model.PRE_END_TIME;
					DateTime endTimeTmp = Convert.ToDateTime(model.PRE_END_TIME.ToString("yyyy-MM-dd HH") + ":59:59");
					//2.向 前工单 写入“非工作时间”
					CalcHourYidldDtl(model.LINE_ID, model.LINE_TYPE, model.PRE_WO_NO, Convert.ToDecimal(model.PRE_PCB_SIDE), startTimeTmp, endTimeTmp, transaction);

					startTimeTmp = Convert.ToDateTime(Convert.ToDateTime(model.NEXT_START_TIME).ToString("yyyy-MM-dd HH") + ":00:00");
					endTimeTmp = Convert.ToDateTime(model.NEXT_START_TIME);
					//3.向 后工单 写入“非工作时间”
					CalcHourYidldDtl(model.LINE_ID, model.LINE_TYPE, model.NEXT_WO_NO, Convert.ToDecimal(model.NEXT_PCB_SIDE), startTimeTmp, endTimeTmp, transaction);

					transaction.Commit(); // 提交事务
					return 1;
				}
				catch
				{
					transaction.Rollback(); // 回滚事务
					throw;
				}
				finally
				{
					if (_dbConnection.State != ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		public async Task<decimal> UpdateChangeLineRecordAsync(MesChangeLineRecord model)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (IDbTransaction transaction = _dbConnection.BeginTransaction())
			{ // 创建事务	
				try
				{
					//1.写入数据到MES_CHANGE_LINE_RECORD
					await _dbConnection.UpdateAsync(model, transaction);

					DateTime startTimeTmp = model.PRE_END_TIME;
					DateTime endTimeTmp = Convert.ToDateTime(model.PRE_END_TIME.ToString("yyyy-MM-dd HH") + ":59:59");
					//2.向 前工单 写入“非工作时间”
					CalcHourYidldDtl(model.LINE_ID, model.LINE_TYPE, model.PRE_WO_NO, Convert.ToDecimal(model.PRE_PCB_SIDE), startTimeTmp, endTimeTmp, transaction);

					startTimeTmp = Convert.ToDateTime(Convert.ToDateTime(model.NEXT_START_TIME).ToString("yyyy-MM-dd HH") + ":00:00");
					endTimeTmp = Convert.ToDateTime(model.NEXT_START_TIME);
					//3.向 后工单 写入“非工作时间”
					CalcHourYidldDtl(model.LINE_ID, model.LINE_TYPE, model.NEXT_WO_NO, Convert.ToDecimal(model.NEXT_PCB_SIDE), startTimeTmp, endTimeTmp, transaction);

					transaction.Commit(); // 提交事务
					return 1;
				}
				catch
				{
					transaction.Rollback(); // 回滚事务
					throw;
				}
				finally
				{
					if (_dbConnection.State != ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		public async Task<int> DeleteChangeLineRecordAsync(decimal id)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (IDbTransaction transaction = _dbConnection.BeginTransaction())
			{ // 创建事务	
				try
				{
					//1.查询MES_CHANGE_LINE_RECORD的数据
					MesChangeLineRecord model = await _dbConnection.GetAsync<MesChangeLineRecord>(id, transaction);

					DateTime startTimeTmp = model.PRE_END_TIME;
					DateTime endTimeTmp = Convert.ToDateTime(model.PRE_END_TIME.ToString("yyyy-MM-dd HH") + ":59:59");
					//2.向 前工单 写入“非工作时间”
					CalcHourYidldDtl(model.LINE_ID, model.LINE_TYPE, model.PRE_WO_NO, Convert.ToDecimal(model.PRE_PCB_SIDE), startTimeTmp, endTimeTmp, transaction, true);

					startTimeTmp = Convert.ToDateTime(Convert.ToDateTime(model.NEXT_START_TIME).ToString("yyyy-MM-dd HH") + ":00:00");
					endTimeTmp = Convert.ToDateTime(model.NEXT_START_TIME);
					//3.向 后工单 写入“非工作时间”
					CalcHourYidldDtl(model.LINE_ID, model.LINE_TYPE, model.NEXT_WO_NO, Convert.ToDecimal(model.NEXT_PCB_SIDE), startTimeTmp, endTimeTmp, transaction, true);

					//4.删除表MES_CHANGE_LINE_RECORD的数据
					await _dbConnection.DeleteAsync<MesChangeLineRecord>(id, transaction);

					transaction.Commit(); // 提交事务
					return 1;
				}
				catch
				{
					transaction.Rollback(); // 回滚事务
					throw;
				}
				finally
				{
					if (_dbConnection.State != ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		#region 处理换线记录的业务逻辑

		private async void CalcHourYidldDtl(decimal lineId, string lineType, string woNo, decimal pcbSide, DateTime startTimeTmp, DateTime endTimeTmp, IDbTransaction transaction, bool bDel = false)
		{
			bool bNeedInsert = true;
			int startMinute = Convert.ToInt32(startTimeTmp.ToString("mm"));//开始分钟
			int endMinute = Convert.ToInt32(endTimeTmp.ToString("mm")) + 1;//结束分钟				
			int hsMinute = endMinute - startMinute;//耗时分钟

			//1.查询表MES_KANBAN_HOUR_YIDLD的数据（最多只有一条）
			var tmpdata = await QueryHourYidld(lineId, lineType, Convert.ToDateTime(startTimeTmp.ToString("yyyy-MM-dd HH") + ":00:00"), woNo, pcbSide);
			if (tmpdata != null)
			{
				List<dynamic> hourYidldHeaderList = tmpdata.ToList();
				foreach (dynamic hourYidld in hourYidldHeaderList)
				{
					var dicHourYidld = (IDictionary<string, object>)hourYidld;
					decimal mstId = Convert.ToDecimal(dicHourYidld["ID"]);

					//2.向表MES_KANBAN_HOUR_YIDLD_DTL删除 换线时间 记录
					await DeleteHourYidldDtl(mstId, 2, transaction);
					if (bDel == false)//如果不是删除换线记录，则需要写入
					{
						//3.向表MES_KANBAN_HOUR_YIDLD_DTL写入换线时间
						await InsertHourYidldDtl(mstId, 2, hsMinute, startMinute, endMinute, transaction);
					}
					//4.重新计算工作分钟，并更新相关数据
					CalcHourYidldWorkMinutes(lineId, lineType, startTimeTmp.ToString("HH") + ":00:00", mstId, startMinute, endMinute, transaction, bDel);
					bNeedInsert = false;
					break;
				}
			}

			//if (bNeedInsert == true)
			//{
			//	decimal mstId = await GetHourYidldSEQID();
			//	string partNo = "";
			//	//:PCB_SIDE,:WO_NO,:PART_NO,:OUTPUT_QTY,:STANDARD_CAPACITY
			//	//await InsertHourYidld(mstId, lineId, lineType,woNo,pcbSide, partNo,0,0, transaction);

			//	//2.向表MES_KANBAN_HOUR_YIDLD_DTL写入工作时间
			//	await InsertHourYidldDtl(mstId, 0, hsMinute, startMinute, endMinute, transaction);

			//	//3.向表MES_KANBAN_HOUR_YIDLD_DTL写入换线时间
			//	await InsertHourYidldDtl(mstId, 2, hsMinute, startMinute, endMinute, transaction);
			//}
		}

		public async void CalcHourYidldWorkMinutes(decimal lineId, string lineType, string workHour, decimal mstId, int startMinute, int endMinute, IDbTransaction transaction, bool bDel)
		{
			#region 1.换线分钟集合
			ArrayList listChange = new ArrayList();
			if (bDel == false)//true:删除换线记录，则换线分钟集合为空
			{
				for (int i = startMinute + 1; i < endMinute; i++)
				{
					if (listChange.Contains(i) == false)
					{
						listChange.Add(i);
					}
				}
			}
			#endregion

			#region 2.工作分钟集合
			ArrayList listWork = new ArrayList();//工作分钟集合
			var tmpdata = await GetWorkingMinutes(lineId, lineType, workHour);
			if (tmpdata != null)
			{
				List<dynamic> workingMinuteList = tmpdata.ToList();
				foreach (dynamic workingMinute in workingMinuteList)
				{
					var dicWorkingMinute = (IDictionary<string, object>)workingMinute;
					for (int i = Convert.ToInt32(dicWorkingMinute["START_MINUTE"]); i <= Convert.ToInt32(dicWorkingMinute["END_MINUTE"]); i++)
					{
						if (listWork.Contains(i) == false)
						{
							listWork.Add(i);
						}
					}
				}
			}
			#endregion

			#region 3.获取 最终 的工作分钟集合
			foreach (object change in listChange)
			{
				listWork.Remove(change);
			}
			#endregion

			//4.删除 原有的 工作时间 记录
			await DeleteHourYidldDtl(mstId, 0, transaction);

			#region 5.写入新的工作时间段
			decimal start = 0;
			decimal end = 0;
			decimal endTmp = 0;
			for (int i = 0; i < listWork.Count; i++)
			{
				if (i == 0)
				{
					start = Convert.ToDecimal(listWork[i]);
					endTmp = start + 1;
				}
				else
				{
					if (endTmp == Convert.ToDecimal(listWork[i]))
					{
						endTmp = Convert.ToDecimal(listWork[i]) + 1;
					}
					else//工作分钟 不连续
					{
						end = endTmp - 1;
						if (start != end)
						{
							//写入一条工作时间段的数据
							await InsertHourYidldDtl(mstId, 0, end - start, start, end, transaction);
						}

						start = Convert.ToDecimal(listWork[i]);//重新开始
						endTmp = start + 1;
					}
				}
			}

			if (endTmp - 1 != start && endTmp > 0)
			{
				end = endTmp - 1;
				//写入一条工作时间段的数据
				await InsertHourYidldDtl(mstId, 0, end - start, start, end, transaction);
			}
			#endregion

			//6.更新MES_KANBAN_HOUR_YIDLDCURRENT_HOUR = 'Y'
			//PS:SYNC_MES_KANBAN_HOUR_YIDLD_PC4(JOB 84)会重新计算工作分钟、标准产能、正常产能范围、报告记录
			await UpdateHourYidldCurrentHour(mstId, transaction);
		}

		#region MES_KANBAN_HOUR_YIDLD、MES_KANBAN_HOUR_YIDLD_DTL 表的相关操作
		private async Task<IEnumerable<dynamic>> GetWorkingMinutes(decimal lineId, string lineType, string workTime)
		{
			string sql = "SELECT * FROM SYS_WORK_SHIFT_DETAIL_MINUTES WHERE LINE_ID = :LINE_ID AND LINE_TYPE = :LINE_TYPE AND WORK_TIME = :WORK_TIME";
			return await _dbConnection.QueryAsync(sql, new
			{
				LINE_ID = lineId,
				LINE_TYPE = lineType,
				WORK_TIME = workTime
			});
		}
		//private async Task<int> InsertHourYidld()
		//{

		//	string sql = "INSERT INTO MES_KANBAN_HOUR_YIDLD(ID,LINE_TYPE,LINE_ID,PCB_SIDE,WO_NO,PART_NO," +
		//				 "OUTPUT_QTY,STANDARD_CAPACITY,STANDARD_CAPACITY_WORK,WORK_TIME,VALUE_MAX,VALUE_MIN,CURRENT_HOUR)" +
		//				 "VALUES(:ID,:LINE_TYPE,:LINE_ID,:PCB_SIDE,:WO_NO,:PART_NO,:OUTPUT_QTY,:STANDARD_CAPACITY," +
		//				":STANDARD_CAPACITY_WORK,:WORK_TIME,:VALUE_MAX,:VALUE_MIN,'N')";
		//	return await _dbConnection.ExecuteAsync(sql, new
		//	{
		//		ID = Id
		//	}, transaction);
		//}

		//public async Task<decimal> GetHourYidldSEQID()
		//{
		//	string sql = "SELECT MES_KANBAN_HOUR_YIDLD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
		//	var result = await _dbConnection.ExecuteScalarAsync(sql);
		//	return (decimal)result;
		//}

		private async Task<IEnumerable<dynamic>> QueryHourYidld(decimal lineId, string lineType, DateTime workTime, string woNo, decimal pcbSide)
		{
			string sqlSelect = "SELECT * FROM MES_KANBAN_HOUR_YIDLD WHERE LINE_ID = :LINE_ID AND LINE_TYPE = :LINE_TYPE AND WORK_TIME = :WORK_TIME AND WO_NO = :WO_NO AND PCB_SIDE = :PCB_SIDE";
			return await _dbConnection.QueryAsync<dynamic>(sqlSelect, new
			{
				LINE_ID = lineId,
				LINE_TYPE = lineType,
				WORK_TIME = workTime,
				WO_NO = woNo,
				PCB_SIDE = pcbSide
			});
		}

		private async Task<int> UpdateHourYidldCurrentHour(decimal Id, IDbTransaction transaction)
		{
			string sqlSelect = "UPDATE MES_KANBAN_HOUR_YIDLD SET CURRENT_HOUR = 'Y' WHERE ID = :ID";
			return await _dbConnection.ExecuteAsync(sqlSelect, new
			{
				ID = Id
			}, transaction);
		}

		//private async Task<IEnumerable<dynamic>> QueryHourYidldDtl(decimal mstId, decimal type, IDbTransaction transaction)
		//{
		//	string sqlSelect = "SELECT * FROM MES_KANBAN_HOUR_YIDLD_DTL WHERE MST_ID = :MST_ID AND DTL_TYPE = :DTL_TYPE";
		//	return await _dbConnection.QueryAsync<dynamic>(sqlSelect, new
		//	{
		//		MST_ID = mstId,
		//		DTL_TYPE = type
		//	}, transaction);
		//}

		private async Task<int> DeleteHourYidldDtl(decimal mstId, decimal type, IDbTransaction transaction)
		{
			string sqlSelect = "DELETE MES_KANBAN_HOUR_YIDLD_DTL WHERE MST_ID = :MST_ID AND DTL_TYPE = :TYPE";
			return await _dbConnection.ExecuteAsync(sqlSelect, new
			{
				MST_ID = mstId,
				TYPE = type
			}, transaction);
		}

		private async Task<int> InsertHourYidldDtl(decimal mstId, decimal type, decimal hsMinute, decimal startMinute, decimal endMinute, IDbTransaction transaction)
		{
			string sqlDtl = @"INSERT INTO MES_KANBAN_HOUR_YIDLD_DTL(ID,MST_ID,DTL_TYPE,DTL_MINUTES,DTL_START_MINUTE,DTL_END_MINUTE,DTL_DESCRIPTION)
								VALUES(MES_KANBAN_HOUR_YIDLD_DTL_SEQ.NEXTVAL,:MST_ID,:TYPE,:TAKING_MINUTES,:START_MINUTE,:END_MINUTE,'')";

			return await _dbConnection.ExecuteAsync(sqlDtl, new
			{
				MST_ID = mstId,
				TYPE = type,
				TAKING_MINUTES = hsMinute,
				START_MINUTE = startMinute,
				END_MINUTE = endMinute
			}, transaction);
		}

		#endregion

		#endregion
	}
}