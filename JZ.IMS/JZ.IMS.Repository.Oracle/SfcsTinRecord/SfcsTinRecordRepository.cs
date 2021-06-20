using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using JZ.IMS.Models.SfcsTinRecord;

namespace JZ.IMS.Repository.Oracle
{
	public class SfcsTinRecordRepository : BaseRepository<SfcsTinRecord, Decimal>, ISfcsTinRecordRepository
	{
		public SfcsTinRecordRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_TIN_RECORD WHERE ID=:ID";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
			{
				ID = id,
			});

			return result == "Y" ? true : false;
		}

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT SFCS_TIN_RECORD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		public async Task<decimal> GetApsOutputAsync(Decimal LINE_ID, DateTime OUTPUT_DAY)
		{
			if (await GetTinRecordAsync(LINE_ID, OUTPUT_DAY) > 0)
			{
				return -1;
			}

			string sql = "SELECT PRODUCTQYT FROM V_APS_OUTPUT_DATE WHERE ID = :LINE_ID AND PRODUCTDATE = :PRODUCTDATE";
			var result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				LINE_ID = LINE_ID,
				PRODUCTDATE = OUTPUT_DAY
			});
			if (result == null)
			{
				return 0;
			}
			else
			{
				return (decimal)result;
			}
		}

		public async Task<decimal> GetTinRecordAsync(Decimal LINE_ID, DateTime OUTPUT_DAY)
		{
			string sql = "SELECT COUNT(ID) FROM SFCS_TIN_RECORD WHERE LINE_ID = :LINE_ID AND OUTPUT_DAY = :OUTPUT_DAY";
			var result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				LINE_ID = LINE_ID,
				OUTPUT_DAY = OUTPUT_DAY
			});
			if (result == null)
			{
				return 0;
			}
			else
			{
				return (decimal)result;
			}
		}
	}
}
