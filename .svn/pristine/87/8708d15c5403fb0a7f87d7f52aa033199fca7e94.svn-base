/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：监控报告表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-11-15 15:08:12                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesMonitoringReportRepository                                      
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
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
	public class MesMonitoringReportRepository : BaseRepository<MesMonitoringReport, Decimal>, IMesMonitoringReportRepository
	{
		public MesMonitoringReportRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_MONITORING_REPORT WHERE ID=:ID AND IS_DELETE='N'";
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
			string sql = "UPDATE MES_MONITORING_REPORT set ENABLED=:ENABLED where  Id=:Id";
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
			string sql = "SELECT MES_MONITORING_REPORT_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}


		/// <summary>
		/// 看板的异常报告
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="lineType"></param>
		/// <returns></returns>
		public async Task<IEnumerable<MesMonitoringReportListModel>> GetMonitoringReportDataAsync(int lineId, string lineType, int topCount)
		{
			var p = new DynamicParameters();
			p.Add(":V_LINE_ID", lineId);
			p.Add(":V_LINE_TYPE", lineType);
			p.Add(":V_TOP_COUNT", topCount);

			string sSql = "SELECT * FROM(" +
				"SELECT * FROM V_MES_MONITORING_REPORT " +
				"WHERE LINE_TYPE = :V_LINE_TYPE AND LINE_ID = :V_LINE_ID AND STATUS >= 0 ORDER BY STATUS ASC,ID DESC)" +
				"WHERE ROWNUM <= :V_TOP_COUNT";

			return await _dbConnection.QueryAsync<MesMonitoringReportListModel>(sSql, p, commandType: CommandType.Text);
		}

		public async Task<bool> UpdateStatusAsync(MesMonitoringReportHandleRequestModel model)
		{
			string sql = "UPDATE MES_MONITORING_REPORT SET STATUS=:STATUS,REASON = :REASON,MODIFIER = :MODIFIER,MODIFY_TIME = :MODIFY_TIME,ERROR_TYPE=:ERROR_TYPE where Id=:Id";
			if (await _dbConnection.ExecuteAsync(sql, new
			{
				STATUS = 2,
				REASON = model.REASON,
				MODIFIER = model.MODIFIER,
				MODIFY_TIME = model.MODIFY_TIME,
				ERROR_TYPE = model.ERROR_TYPE,
				Id = model.MST_ID
			}) > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 获取系统参数
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<SfcsParameters> GetParametersByType(string type)
		{
			string sql = "SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE=:LOOKUP_TYPE AND ENABLED = 'Y'";
			return _dbConnection.Query<SfcsParameters>(sql, new { LOOKUP_TYPE = type }).ToList();
		}
	}
}