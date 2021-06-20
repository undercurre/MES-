/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:37:18                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsLookups                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:37:18
	///  批量保存实体
	/// </summary>
	public class SfcsLookupsModel
	{
		/// <summary>
		/// 新增的记录
		/// </summary>
		public List<SfcsLookupsAddOrModifyModel> insertRecords { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<SfcsLookupsAddOrModifyModel> updateRecords { get; set; }

		///// <summary>
		///// 删除的记录
		///// </summary>
		//public List<SfcsLookupsAddOrModifyModel> removeRecords { get; set; }
	}
}
