using Dapper;
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.IRepository;
using JZ.IMS.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Repository.Oracle
{
	class KeyboardRepository : IKeyboardRepository
	{
		protected DbOption _dbOption;
		protected IDbConnection _dbConnection;

		public KeyboardRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		/// 获取产品站位信息
		/// </summary>
		/// <param name="partNo"></param>
		/// <returns></returns>
		public async Task<IEnumerable<MesPartLocModel>> GetProcLocDataAsync(string partNo, string locNo, int topCount)
		{
			string sql = @"SELECT * FROM MES_PART_LOC WHERE ENABLED = 'Y' AND PART_NO = :PART_NO ORDER BY LOC_NO ASC";

			if (!string.IsNullOrEmpty(locNo))
				sql += " AND LOC_NO LIKE :LOC_NO||'%'";

			if (topCount > 0)
				sql += " AND ROWNUM <= :TOPCOUNT";

			return await _dbConnection.QueryAsync<MesPartLocModel>(sql, new { PART_NO = partNo, LOC_NO = locNo, TOPCOUNT = topCount });
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
