/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产线线体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-23 10:14:20                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsOperationLinesRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface ISfcsOperationLinesRepository : IBaseRepository<SfcsOperationLines, Decimal>
    {
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
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetExportData(SfcsOperationLineRequestModel model);

		/// <summary>
		/// 查询列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadData(SfcsOperationLineRequestModel model);

        /// <summary>
		/// 根据组织ID获取所有线别信息
		/// </summary>
		/// <param name="organizeId">组织ID，默认为“1”（集团）</param>
		/// <returns></returns>
		List<AllLinesModel> GetLinesList(string organizeId = "1", string lineType = "");
		/// <summary>
		/// 根据用户获取线别
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		List<AllLinesModel> GetLinesListByUser(String userName);

	}
}