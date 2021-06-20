using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository {
	public interface IAndonCallPersonConfigRepository : IBaseRepository<AndonCallPersonConfig, Decimal> {
		/// <summary>
		/// 校验ID是否为：空
		/// </summary>
		/// <returns></returns>
		Task<decimal> GetSEQIDAsync();

		/// <summary>
		/// 通过呼叫配置ID查询对应的人员配置记录
		/// </summary>
		/// <returns>只有一条人员配置记录集合</returns>
		List<AndonCallPersonConfig> SelOneByMST_ID(int id);
	}
}
