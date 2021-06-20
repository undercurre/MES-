/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-21 14:13:11                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IImportDtlRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JZ.IMS.IRepository
{
    public interface IImportDtlRepository : IBaseRepository<ImportDtl, Decimal>
    {
		/// <summary>
		/// 获取表对应的导入字段
		/// </summary>
		/// <param name="tableName">表名</param>
		List<ImportItemVM> GetTemplateInfo(string tableName);

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		Task<decimal> GetSEQID();

		/// <summary>
		/// 获取导入主表信息
		/// </summary>
		/// <param name="table_name"></param>
		/// <returns></returns>
		Task<ImportMst> GetImportMst(string table_name);

		/// <summary>
		/// 获取所有导入主表集
		/// </summary>
		/// <returns></returns>
		Task<List<ImportMst>> GetImportMstList();

		/// <summary>
		/// 保存表信息数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveMainDataByTrans(ImportMstModel model);

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(ImportDtlModel model);

		/// <summary>
		/// 保存导入的Excel数据数据
		/// </summary>
		/// <param name="excelItem">导入的Excel数据列表</param>
		/// <param name="tplList">导入模板数据列表</param>
		/// <param name="table_name">导入目标表名称</param>
		/// <returns></returns>
		Task<ImportResult> SaveImportExcelData(List<ImportExcelItem> excelItem, List<ImportDtl> tplList, string table_name);
	}
}