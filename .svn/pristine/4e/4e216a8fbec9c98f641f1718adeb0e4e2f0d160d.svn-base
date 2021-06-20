using JZ.IMS.Core.Repository;
using System;
using JZ.IMS.Models;
using System.Collections.Generic;
using System.Text;
using JZ.IMS.IRepository;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.DbHelper;
using Dapper;
using System.Linq;

namespace JZ.IMS.Repository.Oracle {
	public class AndonCallPersonConfigRepository : BaseRepository<AndonCallPersonConfig, Decimal>, IAndonCallPersonConfigRepository {

		public AndonCallPersonConfigRepository(IOptionsSnapshot<DbOption> options) {
			_dbOption = options.Get("iWMS");
			if (_dbOption == null) {
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetSEQIDAsync() {
			string sql = "SELECT ANDON_CALL_PERSON_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 通过呼叫配置ID查询对应的人员配置记录
		/// </summary>
		/// <returns>只有一条人员配置记录集合</returns>
		public List<AndonCallPersonConfig> SelOneByMST_ID(int id) {					
			return GetList("WHERE MST_ID = :id", new { id = id }).ToList(); ;
		}
	}
}
