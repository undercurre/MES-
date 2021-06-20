/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：工序                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-23 11:12:27                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsOperationsRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface ISfcsOperationsRepository : IBaseRepository<SfcsOperations, Decimal>
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
		Task<TableDataModel> GetExportData(SfcsOperationsRequestModel model);

		/// <summary>
		/// 获取制程工序
		/// </summary>
		/// <param name="route_id"></param>
		/// <returns></returns>
		Task<dynamic> GetRouteOperationByRouteID(int route_id);

	}
}