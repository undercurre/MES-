/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认详细表 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-25 10:28:35                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：MesProductionPreDtl                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-25 10:28:35
	/// 产前确认详细表 批量保存实体
	/// </summary>
	public class MesProductionPreDtlModel
	{
		/// <summary>
		/// 新增的记录
		/// </summary>
		public List<MesProductionPreDtlAddOrModifyModel> InsertRecords { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<MesProductionPreDtlAddOrModifyModel> UpdateRecords { get; set; }

		/// <summary>
		/// 删除的记录
		/// </summary>
		public List<MesProductionPreDtlAddOrModifyModel> RemoveRecords { get; set; }
	}
}
