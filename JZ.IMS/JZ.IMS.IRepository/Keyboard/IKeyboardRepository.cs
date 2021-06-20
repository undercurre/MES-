using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface IKeyboardRepository : IDisposable
	{
		/// <summary>
		/// 获取产品站位信息
		/// </summary>
		/// <param name="partNo"></param>
		/// <returns></returns>
		Task<IEnumerable<MesPartLocModel>> GetProcLocDataAsync(string partNo, string locNo, int topCount);
	}
}
