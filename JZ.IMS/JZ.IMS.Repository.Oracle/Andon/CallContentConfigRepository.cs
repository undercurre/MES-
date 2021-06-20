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
using JZ.IMS.Core.Extensions;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
	/// <summary>
	/// 呼叫内容配置
	/// </summary>
	public class CallContentConfigRepository : BaseRepository<Andon_Call_Content_Config, decimal>, ICallContentConfigRepository
	{
		public CallContentConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT Andon_Call_Content_Config_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		public async Task<decimal> UpdateEnabledById(decimal id, string status)
		{
			string sql = "UPDATE Andon_Call_Content_Config SET ENABLED=:ENABLED WHERE ID=:ID";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status,
				ID = id,
			});
		}

		/// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(CallContentConfigRequestModel model)
		{
			string conditions = "WHERE m.ID > 0 ";
			if (!model.CALL_TYPE_CODE.IsNullOrWhiteSpace())
			{
				conditions += $"AND instr(m.CALL_TYPE_CODE, :CALL_TYPE_CODE) > 0 ";
			}
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $"AND (instr(m.CALL_CODE, :Key) > 0 OR instr(CHINESE, :Key) > 0)";
			}

			string sql = @"SELECT ROWNUM AS ROWNO, m.ID, pm.CHINESE as CALL_TYPE_CODE, m.DESCRIPTION, m.CHINESE, m.ENABLED, m.CALL_CODE 
                           From Andon_Call_Content_Config m  
                           LEFT JOIN SFCS_PARAMETERS pm ON m.Call_Type_Code = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'ANDON_CALL_TYPE'AND pm.ENABLED = 'Y' ";

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id desc", conditions);
			var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
			string sqlcnt = @"SELECT COUNT(0) From Andon_Call_Content_Config m  " + conditions;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}
	}
}
