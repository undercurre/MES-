using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	/// <summary>
	/// 数据字典
	/// </summary>
	public interface ISfcsParametersRepository : IBaseRepository<SfcsParameters, decimal>
	{
		/// <summary>
		/// 通过条件获取数据
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsParameters> GetListByCondition();

		/// <summary>
		/// 通过条件获取呼叫类型（条件为：CHINESE）
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsParameters> GetListByChinese(string CHINESE);

		/// <summary>
		/// 获取全部线体的物理位置
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsParameters> GetPhysicalLocationList();

		/// <summary>
		/// 获取全部线体的厂别
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsParameters> GetPlantCodeList();

		/// <summary>
		/// 获取全部工序类别
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsParameters> GetOperationCategoryList();

		/// <summary>
		/// 获取全部设备类别
		/// </summary>
		List<SfcsParameters>  GetEquipmentCategoryList();

		/// <summary>
		/// 获取经营单位
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsParameters> GetBusinessUnitsList();

		/// <summary>
		/// 获取全部部门
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsDepartment> GetDepartmentList();

		/// <summary>
		/// 根据类型获取字典信息
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsParameters> GetListByType(string type);

		/// <summary>
		/// 获取类型列表
		/// </summary>
		/// <returns></returns>
		Task<TableDataModel> GetDistinctList(SfcsParametersRequestModel model);

		/// <summary>
		/// 保存方法
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SfcsParametersModel model);

        /// <summary>
		/// 根据类型获取字典信息
		/// </summary>
		/// <returns>结果集</returns>
		List<SfcsParameters> GetSmtLookupListByType(string type);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<bool> SaveMachineByTrans(SaveMachineConfigModel model);

	}
}
