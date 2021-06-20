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
using System.Data;

namespace JZ.IMS.Repository.Oracle
{
	public class CallConfigRepository : BaseRepository<Andon_Call_Config, decimal>, ICallConfigRepository
	{

		public CallConfigRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		public async Task<decimal> GetSEQIDAsync()
		{
			string sql = "SELECT Andon_Call_Config_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取重复项（通过：OPERATION_LINE_ID、OPERATION_SITE_ID、CALL_TYPE_CODE）
		/// </summary>
		/// <param name="conf">用来匹配的实体</param>
		/// <returns>匹配到的实体集合</returns>
		public List<Andon_Call_Config> GetRepeatedItem(Andon_Call_Config conf)
		{
			return this.GetList(@"WHERE OPERATION_LINE_ID = :OPERATION_LINE_ID and 
								OPERATION_SITE_ID = :OPERATION_SITE_ID and
								CALL_TYPE_CODE = :CALL_TYPE_CODE",
							new
							{
								conf.OPERATION_LINE_ID,
								conf.OPERATION_SITE_ID,
								conf.CALL_TYPE_CODE
							}).AsList();
		}

		/// <summary>
		/// 通过ID更新对应的激活状态
		/// </summary>
		/// <param name="id">改变了状态的ID</param>
		/// <param name="status">改变后的状态</param>
		/// <returns>成功更新的条数</returns>
		public async Task<decimal> UpdateEnabledById(decimal id, string status)
		{
			string sql = "UPDATE Andon_Call_Config SET ENABLED=:ENABLED WHERE ID=:ID";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status,
				ID = id,
			});
		}

		/// <summary>
		/// 添加数据到呼叫配置表和呼叫人员配置表
		/// </summary>
		/// <param name="model">呼叫配置</param>
		/// <param name="callPersList">呼叫人员配置</param>
		/// <returns></returns>
		public async Task<decimal> InsCallAndPerson(Andon_Call_Config model, List<AndonCallPersonConfig> callPersList)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (IDbTransaction transaction = _dbConnection.BeginTransaction())
			{ // 创建事务	
				try
				{
					await _dbConnection.InsertAsync(model, transaction);

					#region 添加呼叫人员
					string sqlIns = @"INSERT INTO ANDON_CALL_PERSON_CONFIG
					(ID, MST_ID, USER_ID) VALUES (:ID, :MST_ID, :USER_ID)";
					foreach (var item in callPersList)
					{
						await _dbConnection.ExecuteAsync(sqlIns, new
						{
							item.ID,
							item.MST_ID,
							item.USER_ID
						}, transaction);
					}
					#endregion

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

		/// <summary>
		/// 修改呼叫配置表和呼叫人员配置表
		/// </summary>
		/// <param name="model">呼叫配置</param>
		/// <param name="callPersList">呼叫人员配置</param>
		/// <returns></returns>
		public async Task<decimal> UpdCallAndPerson(Andon_Call_Config model, List<AndonCallPersonConfig> callPersList)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (IDbTransaction transaction = _dbConnection.BeginTransaction())
			{ // 创建事务	
				try
				{
					await _dbConnection.UpdateAsync(model, transaction);

					#region 从AndonCallPersonConfig表中删除与该呼叫配置相关的数据
					string sqlDel = @"DELETE FROM ANDON_CALL_PERSON_CONFIG
					WHERE MST_ID=:ID";
					await _dbConnection.ExecuteAsync(sqlDel, new
					{
						ID = model.ID
					}, transaction);
					#endregion

					#region 重新添加呼叫人员
					string sqlIns = @"INSERT INTO ANDON_CALL_PERSON_CONFIG
					(ID, MST_ID, USER_ID) VALUES (:ID, :MST_ID, :USER_ID)";
					foreach (var item in callPersList)
					{
						await _dbConnection.ExecuteAsync(sqlIns, new
						{
							item.ID,
							item.MST_ID,
							item.USER_ID
						}, transaction);
					}
					#endregion

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

		/// <summary>
		/// 通过ID删除 呼叫人员表 和 呼叫人员配置表 中对应的记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> DelOneByIdAsync(decimal id)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					if (DeleteList("WHERE ID=:id", new { id }, tran) < 1)
					{
						throw new Exception("删除失败");
					}
					string sql = @"DELETE FROM ANDON_CALL_PERSON_CONFIG WHERE MST_ID=:ID";
					int count = await _dbConnection.ExecuteAsync(sql, new { ID = id }, tran);
					if (count < 1)
					{
						throw new Exception("该记录没有相关的呼叫人员，删除失败");
					}
					tran.Commit(); // 提交事务
					return 1;
				}
				catch
				{
					tran.Rollback(); // 回滚事务					
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
	}
}
