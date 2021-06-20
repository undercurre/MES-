using Dapper;
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.IRepository.MesSpotCheckReport;
using JZ.IMS.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Repository.Oracle.MesSpotCheckReport
{
	public class MesSpotCheckReportRepository : IMesSpotCheckReportRepository
	{
		protected DbOption _dbOption;
		protected IDbConnection _dbConnection;

		public MesSpotCheckReportRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		/// 获取抽检异常明细报表数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<List<MesCheckFailReportListModel>> GetCheckFailReportData(MesCheckReportRequestModel model)
		{
			string sql = @"SELECT V.*
						  FROM V_CHECK_FAIL_REPORT V
							   INNER JOIN SYS_ORGANIZE_LINE L ON V.LINE_ID = L.LINE_ID
						 WHERE 1=1 ";

			if (!string.IsNullOrEmpty(model.ORGANIZE_ID))
				sql += @" AND EXISTS
								  (SELECT 1
									 FROM (    SELECT ID
												 FROM SYS_ORGANIZE
										   START WITH ID = :ORGANIZE_ID
										   CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
									WHERE ID = L.ORGANIZE_ID)";

			if (!string.IsNullOrEmpty(model.WO_CLASS))
				sql += " AND V.WO_CLASS = :WO_CLASS ";

			if (model.LINE_TYPE == "SMT" || model.LINE_TYPE == "PCBA")
				sql += " AND INSTR(L.ATTRIBUTE4,:LINE_TYPE)>0 ";

			if (model.LINE_TYPE == "AIRI")
				sql += " AND (INSTR(L.ATTRIBUTE4,'AI')>0 OR INSTR(L.ATTRIBUTE4,'RI')>0) ";

			if (model.BEGIN_TIME != null)
				sql += " AND V.CHECK_DATE >= :BEGIN_TIME ";
			if (model.END_TIME != null)
				sql += " AND V.CHECK_DATE <= :END_TIME ";

			var data = await _dbConnection.QueryAsync<MesCheckFailReportListModel>(sql, model);
			return data.ToList();
		}

		/// <summary>
		/// 获取抽检合格率汇总报表数据(日)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<List<MesCheckPassRateSumListModel>> GetCheckPassRateSumDayData(MesCheckReportRequestModel model)
		{
			string where = "", where2 = "";

			if (!string.IsNullOrEmpty(model.ORGANIZE_ID))
				where2 += @" AND EXISTS
								  (SELECT 1
									 FROM (    SELECT ID
												 FROM SYS_ORGANIZE
										   START WITH ID = :ORGANIZE_ID
										   CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
									WHERE ID = O.ORGANIZE_ID)";

			if (model.LINE_TYPE == "SMT" || model.LINE_TYPE == "PCBA")
				where2 += " AND O.ATTRIBUTE6 = :LINE_TYPE ";

			if (model.LINE_TYPE == "AIRI")
				where2 += " AND (O.ATTRIBUTE6 ='AI' OR O.ATTRIBUTE6 ='RI') ";

			if (!string.IsNullOrEmpty(model.WO_CLASS))
				where += " AND WO_CLASS = :WO_CLASS ";
			if (model.BEGIN_TIME != null)
				where += " AND CHECK_DATE >= :BEGIN_TIME ";
			if (model.END_TIME != null)
				where += " AND CHECK_DATE <= :END_TIME ";

			string sql = string.Format(@"SELECT O.ATTRIBUTE6 AS LINE_TYPE,
							 O.ATTRIBUTE4 AS LINE_SHORT_NAME,
							 L.LINE_NAME,
							 TAB.*,
							 ROUND (BATCH_PASS_QTY / BATCH_ALL_QTY * 100, 2) AS BATCH_RATE,
							 ROUND (PASS_QTY / ALL_QTY * 100, 2) AS BATCH_QTY_RATE,
							 98.5 AS TARGET_RATE
						FROM (  SELECT LINE_ID,
									   SUM (ALL_QTY) AS ALL_QTY,
									   SUM (PASS_QTY) AS PASS_QTY,
									   SUM (FAIL_QTY) AS FAIL_QTY,
									   SUM (BATCH_ALL_QTY) AS BATCH_ALL_QTY,
									   SUM (BATCH_PASS_QTY) AS BATCH_PASS_QTY
								  FROM V_CHECK_PASS_SUM
								 WHERE 1 = 1 {0}
							  GROUP BY LINE_ID) TAB
							 INNER JOIN SYS_ORGANIZE_LINE O ON TAB.LINE_ID = O.LINE_ID
							 INNER JOIN V_MES_LINES L ON TAB.LINE_ID = L.LINE_ID
							 WHERE 1 = 1 {1}
					ORDER BY O.ATTRIBUTE4", where, where2);

			var data = await _dbConnection.QueryAsync<MesCheckPassRateSumListModel>(sql, model);
			return data.ToList();
		}

		/// <summary>
		/// 获取抽检合格率汇总报表数据(月)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<List<MesCheckPassRateSumMonthListModel>> GetCheckPassRateSumMonthData(MesCheckReportRequestModel model)
		{
			string where = "";

			if (!string.IsNullOrEmpty(model.ORGANIZE_ID))
				where += @" AND EXISTS
								  (SELECT 1
									 FROM (    SELECT ID
												 FROM SYS_ORGANIZE
										   START WITH ID = :ORGANIZE_ID
										   CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
									WHERE ID = ORGANIZE_ID)";

			if (model.LINE_TYPE == "SMT" || model.LINE_TYPE == "PCBA")
				where += " AND LINE_TYPE = :LINE_TYPE ";

			if (model.LINE_TYPE == "AIRI")
				where += " AND (LINE_TYPE ='AI' OR LINE_TYPE ='RI') ";

			if (!string.IsNullOrEmpty(model.WO_CLASS))
				where += " AND WO_CLASS = :WO_CLASS ";

			if (!string.IsNullOrEmpty(model.MONTHS))
				where += " AND MONTHS = :MONTHS ";

			string sql = string.Format(@"  SELECT MONTHS,
												 CHECK_DATE,
												 SUM (ALL_QTY) AS ALL_QTY,
												 SUM (PASS_QTY) AS PASS_QTY,
												 SUM (FAIL_QTY) AS FAIL_QTY,
												 ROUND (SUM (PASS_QTY) / SUM (ALL_QTY) * 100, 2) AS PASS_RATE,
												 98.5 AS TARGET_RATE
											FROM V_CHECK_PASS_SUM_MONTH WHERE 1=1 {0}
										GROUP BY MONTHS, CHECK_DATE ORDER BY CHECK_DATE", where);

			var data = await _dbConnection.QueryAsync<MesCheckPassRateSumMonthListModel>(sql, model);
			return data.ToList();
		}

		#region IDisposable Support
		private bool disposedValue = false; // 要检测冗余调用

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: 释放托管状态(托管对象)。
					_dbConnection?.Dispose();
				}

				// TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
				// TODO: 将大型字段设置为 null。

				disposedValue = true;
			}
		}

		// TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
		// ~BaseRepository() {
		//   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
		//   Dispose(false);
		// }

		// 添加此代码以正确实现可处置模式。
		public void Dispose()
		{
			// 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
			Dispose(true);
			// TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
