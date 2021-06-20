/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：操作日志接口实现                                                    
*│　作    者：Admin                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-05 17:54:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ManagerLogRepository                                      
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
using JZ.IMS.WebApi.Public;
using JZ.IMS.Core.Helper;

namespace JZ.IMS.Repository.Oracle
{
	public class ManagerLogRepository : BaseRepository<Sys_Manager_Log, decimal>, IManagerLogRepository
	{
		public ManagerLogRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options?.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		public ManagerLogRepository(DbOption options)
		{
			_dbOption = options;
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		public decimal DeleteLogical(decimal[] ids)
		{
			string sql = "update SYS_MANAGER_LOG set Is_Delete='Y' where Id in @Ids";
			return _dbConnection.Execute(sql, new
			{
				Ids = ids
			});
		}

		public async Task<decimal> DeleteLogicalAsync(decimal[] ids)
		{
			string sql = "update SYS_MANAGER_LOG set Is_Delete='Y' where Id in @Ids";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				Ids = ids
			});
		}

		public async Task<decimal> GetSEQIDAsync()
		{
			string sql = "SELECT Sys_Manager_Log_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取系统错误日志序列
		/// </summary>
		/// <returns></returns>
		private async Task<decimal> GetSysErrorLogSEQAsync()
		{
			string sql = "SELECT SYS_ERRORLOG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 保存错误日志
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<bool> SaveErrorLog(ErrorInfoClass model)
		{
			int result = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			string insertSql = @"INSERT INTO SYS_ErrorLog (ID, CreateTime, ErrorType, ErrorMessage, ErrorContent) 
							   VALUES (:ID, :CreateTime, :ErrorType, :ErrorMessage, :ErrorContent)";
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					var curid = await GetSysErrorLogSEQAsync();
					result = await _dbConnection.ExecuteAsync(insertSql, new
					{
						ID = curid,
						CreateTime = DateTime.Now,
						model.ErrorType,
						ErrorMessage = model.Message,
						ErrorContent = JsonHelper.ObjectToJSON(model),
					}, tran);

					#region 错误路径表

					if (result > 0 && model.RefPathItems != null && model.RefPathItems?.Count > 0)
					{
						int index = 1;
						string insertDtlSql = @"INSERT INTO sys_errorlogpath(errorlogid, sortindex, namespace, classname, methodname, parametername) 
												VALUES (:errorlogid, :sortindex, :names_pace, :classname, :methodname, :parametername)";
						foreach (var item in model.RefPathItems)
						{
							await _dbConnection.ExecuteAsync(insertDtlSql, new
							{
								errorlogid = curid,
								sortindex = index,
								names_pace = item.NameSpace,
								classname = item.ClassName,
								methodname = item.MethodName,
								parametername = item.ParameterName,
							}, tran);

							index++;
						}
					}
					#endregion

					tran.Commit();
				}
				catch (Exception ex)
				{
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
			return result > 0;
		}
	}
}