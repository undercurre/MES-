using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository {
	public interface ICallConfigRepository : IBaseRepository<Andon_Call_Config, decimal> {
		/// <summary>
		/// 获取ID序列
		/// </summary>
		/// <returns>ID</returns>
		Task<decimal> GetSEQIDAsync();

		/// <summary>
		/// 通过ID修改激活状态
		/// </summary>
		/// <param name="id"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		Task<decimal> UpdateEnabledById(decimal id, string status);

		/// <summary>
		/// 添加数据到呼叫配置表和呼叫人员配置表
		/// </summary>
		/// <param name="model">呼叫配置</param>
		/// <param name="callPersList">呼叫人员配置</param>
		/// <returns>大于0为成功</returns>
		Task<decimal> InsCallAndPerson(Andon_Call_Config model, List<AndonCallPersonConfig> callPersList);

		/// <summary>
		/// 修改呼叫配置表和呼叫人员配置表
		/// </summary>
		/// <param name="model">呼叫配置</param>
		/// <param name="callPersList">呼叫人员配置</param>
		/// <returns>大于0为成功</returns>
		Task<decimal> UpdCallAndPerson(Andon_Call_Config model, List<AndonCallPersonConfig> callPersList);

		/// <summary>
		/// 通过ID删除 呼叫人员表 和 呼叫人员配置表 中对应的记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns>大于0为成功</returns>
		Task<decimal> DelOneByIdAsync(decimal id);

		/// <summary>
		/// 获取重复项（通过：OPERATION_LINE_ID、OPERATION_SITE_ID、CALL_TYPE_CODE）
		/// </summary>
		/// <param name="conf">用来匹配的实体</param>
		/// <returns>匹配到的实体集合</returns>
		List<Andon_Call_Config> GetRepeatedItem(Andon_Call_Config conf);
	}
}
