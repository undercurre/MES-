/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具储位表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-12-20 14:05:11                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesTongsStoreConfigRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
	public class MesTongsStoreConfigRepository : BaseRepository<MesTongsStoreConfig, Decimal>, IMesTongsStoreConfigRepository
	{
		public MesTongsStoreConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_TONGS_STORE_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE MES_TONGS_STORE_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_TONGS_STORE_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 判断编码是否存在
		/// </summary>
		/// <param name="code">编码</param>
		/// <param name="id">排除ID</param>
		/// <returns></returns>
		public async Task<bool> IsExistsByCode(string code, decimal? id = null)
		{
			string sql = "SELECT COUNT(*) FROM MES_TONGS_STORE_CONFIG WHERE CODE=:CODE";
			if (id != null && id != 0)
				sql += " AND ID != :ID ";

			var count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { CODE = code, ID = id });
			return count > 0;
		}

		/// <summary>
		/// 判断当前储位是否在夹具中使用
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<bool> IsExistsByTongsInfo(decimal id)
		{
			string sql = "select count(*) from MES_TONGS_INFO where store_id = :STORE_ID";

			return (await _dbConnection.ExecuteScalarAsync<int>(sql, new { STORE_ID = id })) > 0;
		}
	}
}