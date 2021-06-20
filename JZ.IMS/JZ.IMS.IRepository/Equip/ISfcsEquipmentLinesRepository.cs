using JZ.IMS.Core.Repository;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface ISfcsEquipmentLinesRepository
	{
		/// <summary>
		/// 获取所有线别列表
		/// </summary>
		/// <returns></returns>
		List<SfcsEquipmentLinesModel> GetLinesList();

		List<AllLinesModel> GetLinesList(string organizeId = "1", string lineType = "");

		/// <summary>
		/// 获取产线列表
		/// </summary>
		/// <returns></returns>
		List<SfcsEquipmentLinesModel> GetRoHSLinesList();

		/// <summary>
		/// 获取SMT线列表
		/// </summary>
		/// <returns></returns>
		List<SfcsEquipmentLinesModel> GetSMTLinesList();

        List<SfcsLineView> GetVMesLinesList();
    }
}
