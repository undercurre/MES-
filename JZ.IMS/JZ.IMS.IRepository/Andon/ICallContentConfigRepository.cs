using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	/// <summary>
	/// 呼叫内容配置
	/// </summary>
	public interface ICallContentConfigRepository : IBaseRepository<Andon_Call_Content_Config, decimal>
	{
		/// <summary>
		/// 校验ID是否为：空
		/// </summary>
		/// <returns></returns>
		Task<decimal> GetSEQIDAsync();

		/// <summary>
		/// 通过ID修改激活状态
		/// </summary>
		/// <param name="id"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		Task<decimal> UpdateEnabledById(decimal id, string status);

		/// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetExportData(CallContentConfigRequestModel model);
	}
}
