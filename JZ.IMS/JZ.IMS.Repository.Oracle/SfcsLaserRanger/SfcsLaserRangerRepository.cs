/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：镭雕机流水号范围表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-11-20 09:07:35                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsLaserRangerRepository                                      
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
	public class SfcsLaserRangerRepository : BaseRepository<SfcsLaserRanger, Decimal>, ISfcsLaserRangerRepository
	{
		public SfcsLaserRangerRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_LASER_RANGER WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_LASER_RANGER set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_LASER_RANGER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_LASER_RANGER where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsLaserRangerModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_LASER_RANGER 
					(ID,WO_ID,SN_BEGIN,SN_END,QUANTITY,DIGITAL,RANGE,FIX_HEADER,FIX_TAIL,HEADER_LENGTH,TAIL_LENGTH,PRINTED,EXCLUSIVE_CHAR,STATUS,RANGER_RULE_ID,ATTRIBUTE2,ATTRIBUTE3,ATTRIBUTE4,ATTRIBUTE5,RUNCARD_RANGER_ID) 
					VALUES (:ID,:WO_ID,:SN_BEGIN,:SN_END,:QUANTITY,:DIGITAL,:RANGE,:FIX_HEADER,:FIX_TAIL,:HEADER_LENGTH,:TAIL_LENGTH,:PRINTED,:EXCLUSIVE_CHAR,:STATUS,:RANGER_RULE_ID,:ATTRIBUTE2,:ATTRIBUTE3,:ATTRIBUTE4,:ATTRIBUTE5,:RUNCARD_RANGER_ID)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.WO_ID,
								item.SN_BEGIN,
								item.SN_END,
								item.QUANTITY,
								item.DIGITAL,
								item.RANGE,
								item.FIX_HEADER,
								item.FIX_TAIL,
								item.HEADER_LENGTH,
								item.TAIL_LENGTH,
								item.PRINTED,
								item.EXCLUSIVE_CHAR,
								item.STATUS,
								item.RANGER_RULE_ID,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
								item.RUNCARD_RANGER_ID,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_LASER_RANGER set WO_ID=:WO_ID,SN_BEGIN=:SN_BEGIN,SN_END=:SN_END,QUANTITY=:QUANTITY,DIGITAL=:DIGITAL,RANGE=:RANGE,FIX_HEADER=:FIX_HEADER,FIX_TAIL=:FIX_TAIL,HEADER_LENGTH=:HEADER_LENGTH,TAIL_LENGTH=:TAIL_LENGTH,PRINTED=:PRINTED,EXCLUSIVE_CHAR=:EXCLUSIVE_CHAR,STATUS=:STATUS,RANGER_RULE_ID=:RANGER_RULE_ID,ATTRIBUTE2=:ATTRIBUTE2,ATTRIBUTE3=:ATTRIBUTE3,ATTRIBUTE4=:ATTRIBUTE4,ATTRIBUTE5=:ATTRIBUTE5,
					  RUNCARD_RANGER_ID=:RUNCARD_RANGER_ID
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.WO_ID,
								item.SN_BEGIN,
								item.SN_END,
								item.QUANTITY,
								item.DIGITAL,
								item.RANGE,
								item.FIX_HEADER,
								item.FIX_TAIL,
								item.HEADER_LENGTH,
								item.TAIL_LENGTH,
								item.PRINTED,
								item.EXCLUSIVE_CHAR,
								item.STATUS,
								item.RANGER_RULE_ID,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
								item.RUNCARD_RANGER_ID,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_LASER_RANGER where ID=:ID ";
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



		public async Task<string> GetSysdateByFormat(string format)
		{
			string sql = "SELECT TO_CHAR(SYSDATE, '{0}') YYWW FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(string.Format(sql, format));
			return (string)result;
		}
		public async Task<string> Get_Intel_YIW()
		{
			string sql = "SELECT SFCS.SFCS_CONST_PKG.GET_INTEL_YIW YIW FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (string)result;
		}

		public async Task<SfcsLaserRanger> GetLastRangerByRule(decimal rangerRuleID, decimal? digital, decimal? ranger, string fixHeader, string fixTail)
        {
			string condiction = @" WHERE RANGER_RULE_ID = :RANGER_RULE_ID
								AND DIGITAL = :DIGITAL
								AND RANGE = :RANGE";
			if(!fixHeader.IsNullOrWhiteSpace())
            {
				//AND (/*?FIX_HEADER<FIX_HEADER = :FIX_HEADER|1=1>*/ :FIX_HEADER IS NULL)
				condiction += " AND FIX_HEADER = :FIX_HEADER";
			}
			if (!fixTail.IsNullOrWhiteSpace())
			{
				//AND (/*?FIX_TAIL<FIX_TAIL = :FIX_TAIL|1=1>*/ :FIX_TAIL IS NULL)
				condiction += " AND FIX_TAIL = :FIX_TAIL";
			}
			string sql = @"SELECT * FROM SFCS_LASER_RANGER {0} ORDER BY SN_END DESC";
			var resdata = await _dbConnection.QueryAsync<SfcsLaserRanger>(string.Format(sql, condiction),
				new { 
					RANGER_RULE_ID = rangerRuleID,
					DIGITAL = digital,
					RANGE = ranger,
					FIX_HEADER = fixHeader,
					FIX_TAIL = fixTail
				});

			//string sqlcnt = @"select count(0) FROM SFCS_LASER_RANGER {0} ORDER BY SN_END DESC";
			//int cnt = await _dbConnection.ExecuteScalarAsync<int>(string.Format(sqlcnt, condiction),
			//	new
			//	{
			//		RANGER_RULE_ID = rangerRuleID,
			//		DIGITAL = digital,
			//		RANGE = ranger,
			//		FIX_HEADER = fixHeader,
			//		FIX_TAIL = fixTail
			//	});

			//return new TableDataModel
			//{
			//	count = cnt,
			//	data = resdata?.ToList(),
			//};

			return resdata?.FirstOrDefault();
		}

		public async Task<List<SfcsLaserRanger>> GetLaserRangerBySN(decimal woId, string SN)
        {
			string sql = @"SELECT SLR.* FROM SFCS_LASER_RANGER SLR
				WHERE (WO_ID = :WO_ID)
				AND LENGTH(:SN) = LENGTH(SN_END)
				AND (FIX_HEADER = SUBSTR(:SN, 1, HEADER_LENGTH) OR FIX_HEADER IS NULL)
				AND (FIX_TAIL = SUBSTR(:SN, LENGTH(:SN) - TAIL_LENGTH + 1, TAIL_LENGTH) OR FIX_TAIL IS NULL)";
			var resdata = await _dbConnection.QueryAsync<SfcsLaserRanger>(string.Format(sql),
				new
				{
					WO_ID = woId,
					SN = SN
				});
			return resdata?.ToList();
		}

		public async Task<int> UpdateLastLaserSN(decimal id, string sn)
        {
			string sql = @"UPDATE SFCS_LASER_RANGER SET LAST_LASER_SN = :SN WHERE ID = :ID";
			var resdata = await _dbConnection.ExecuteAsync(sql, new
			{
				ID = id,
				SN = sn
			});
			return resdata;
		}
	}
}