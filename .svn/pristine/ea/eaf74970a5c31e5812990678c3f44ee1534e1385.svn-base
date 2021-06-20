/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：看板控制器接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-12-13 11:41:58                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesKanbanControllerRepository                                      
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
using System.Data;

namespace JZ.IMS.Repository.Oracle
{
	public class MesKanbanControllerRepository : BaseRepository<MesKanbanControllerModel, Decimal>, IMesKanbanControllerRepository
	{
		public MesKanbanControllerRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_KANBAN_CONTROLLER WHERE ID=:ID";
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
			string sql = "UPDATE MES_KANBAN_CONTROLLER SET ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_KANBAN_CONTROLLER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		// <summary>
		/// 获取表的记录数
		/// </summary>
		/// <returns></returns>
		public async Task<int> GetRecordCountAsync(string conditions, object parameters = null)
		{
			return await _dbConnection.RecordCountAsync<MesKanbanControllerModel>(conditions, parameters);
		}

		public async Task<int> ModifyForSmtAsync(decimal lineId, string lineType,
			List<SmtKanbanHourYidldModel> hourYidldModel, List<MesChangeLineRecordResult> recordModels,
			SmtKanbanAoiPassRateModel aoiPassRateModel, SmtKanbanSpiPassRateModel spiPassRateModel,
			SmtKanbanFirstPassRateModel firstPassRateModel)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			IDbTransaction transaction = _dbConnection.BeginTransaction();
			try
			{
				//string sqlHourYidld = "UPDATE SMT_KANBAN_HOUR_YIDLD SET WHERE ID = :ID";

				//foreach (SmtKanbanHourYidldModel model in hourYidldModel)
				//{
				//	await _dbConnection.ExecuteAsync(sqlHourYidld, transaction);//小时产能
				//}

				string sqlRecord = "UPDATE MES_CHANGE_LINE_RECORD_RESULT SET PRE_END_TIME = :PRE_END_TIME, TAKING_TIME = :TAKING_TIME, NEXT_START_TIME = :NEXT_START_TIME WHERE ID = :ID";

				foreach (MesChangeLineRecordResult recordModel in recordModels)//换线记录
				{
					await _dbConnection.ExecuteAsync(sqlRecord, new
					{
						PRE_END_TIME = recordModel.PRE_END_TIME,
						TAKING_TIME = recordModel.TAKING_TIME,
						NEXT_START_TIME = recordModel.PRE_END_TIME.AddMinutes(Convert.ToInt32(recordModel.TAKING_TIME)),
						ID = recordModel.ID
					}, transaction);
				}

				string sql = "UPDATE SMT_KANBAN_AOI_PASS_RATE SET PASS = " + aoiPassRateModel.PASS +
					",TOTAL = " + aoiPassRateModel.TOTAL + ",RATE=" + aoiPassRateModel.RATE +
					" WHERE OPERATION_LINE_ID = " + aoiPassRateModel.OPERATION_LINE_ID;
				await _dbConnection.ExecuteAsync(sql, transaction);//AOI

				sql = "UPDATE SMT_KANBAN_SPI_PASS_RATE SET PASS = " + spiPassRateModel.PASS +
					",TOTAL = " + spiPassRateModel.TOTAL + ",RATE=" + spiPassRateModel.RATE +
					" WHERE OPERATION_LINE_ID = " + spiPassRateModel.OPERATION_LINE_ID;
				await _dbConnection.ExecuteAsync(sql, transaction);//SPI

				sql = "UPDATE SMT_KANBAN_FIRST_PASS_RATE SET PASS = " + firstPassRateModel.PASS +
					",TOTAL = " + firstPassRateModel.TOTAL + ",RATE=" + firstPassRateModel.RATE +
					" WHERE OPERATION_LINE_ID = " + firstPassRateModel.OPERATION_LINE_ID;
				await _dbConnection.ExecuteAsync(sql, transaction);//首件

				transaction.Commit(); // 提交事务
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
			return 0;
		}

		public async Task<int> ModifyForPcbaAsync(decimal lineId, string lineType,
			List<HourYidldModel> hourYidldModel, List<MesChangeLineRecordResult> recordModels,
			KanbanPassRateModel passRateModel)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			IDbTransaction transaction = _dbConnection.BeginTransaction();
			try
			{
				string sqlHourYidld = "UPDATE SFCS_KANBAN_HOUR_YIDLD SET OUTPUT_QTY = :OUTPUT_QTY, RATE = :RATE, " +
					"STATUS = :STATUS, REPORT_CONTENT = :REPORT_CONTENT, REASON = :REASON " +
					"WHERE OPERATION_LINE_ID = :OPERATION_LINE_ID AND WORK_TIME = :WORK_TIME ";

				foreach (HourYidldModel model in hourYidldModel)//小时产能
				{
					await _dbConnection.ExecuteAsync(sqlHourYidld, new
					{
						OUTPUT_QTY = model.OUTPUT_QTY,
						RATE = model.RATE,
						STATUS = model.STATUS,
						REPORT_CONTENT = model.REPORT_CONTENT,
						REASON = model.REASON,
						OPERATION_LINE_ID = model.OPERATION_LINE_ID,
						WORK_TIME = model.WORK_TIME
					}, transaction);
				}

				string sqlRecord = "UPDATE MES_CHANGE_LINE_RECORD_RESULT SET PRE_END_TIME = :PRE_END_TIME, TAKING_TIME = :TAKING_TIME, NEXT_START_TIME = :NEXT_START_TIME WHERE ID = :ID";

				foreach (MesChangeLineRecordResult recordModel in recordModels)//换线记录
				{
					await _dbConnection.ExecuteAsync(sqlRecord, new
					{
						PRE_END_TIME = recordModel.PRE_END_TIME,
						TAKING_TIME = recordModel.TAKING_TIME,
						NEXT_START_TIME = recordModel.PRE_END_TIME.AddMinutes(Convert.ToInt32(recordModel.TAKING_TIME)),
						ID = recordModel.ID
					}, transaction);
				}

				string sql = "UPDATE SFCS_KANBAN_PASS_RATE SET PASS = " + passRateModel.PASS +
					",TOTAL = " + passRateModel.TOTAL + ",RATE=" + passRateModel.RATE +
					" WHERE OPERATION_LINE_ID = " + passRateModel.OPERATION_LINE_ID;
				await _dbConnection.ExecuteAsync(sql, transaction);//直通率

				transaction.Commit(); // 提交事务
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
			return 0;
		}
	}
}