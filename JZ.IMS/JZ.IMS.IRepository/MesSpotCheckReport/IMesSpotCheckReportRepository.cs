using JZ.IMS.Core.Repository;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository.MesSpotCheckReport
{
   public interface IMesSpotCheckReportRepository : IDisposable
	{
		/// <summary>
		/// 获取抽检异常明细报表数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<MesCheckFailReportListModel>> GetCheckFailReportData(MesCheckReportRequestModel model);

		/// <summary>
		/// 获取抽检合格率汇总报表数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<MesCheckPassRateSumListModel>> GetCheckPassRateSumDayData(MesCheckReportRequestModel model);

		/// <summary>
		/// 获取抽检合格率汇总报表数据(月)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<MesCheckPassRateSumMonthListModel>> GetCheckPassRateSumMonthData(MesCheckReportRequestModel model);
	}
}
