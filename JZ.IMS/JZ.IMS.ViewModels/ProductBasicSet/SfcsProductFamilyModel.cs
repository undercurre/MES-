/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:47                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductFamily                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:47
	///  批量保存实体
	/// </summary>
	public class SfcsProductFamilyModel
	{
		/// <summary>
		/// 新增的记录
		/// </summary>
		public List<SfcsProductFamilyAddOrModifyModel> insertRecords { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<SfcsProductFamilyAddOrModifyModel> updateRecords { get; set; }

		/// <summary>
		/// 删除的记录
		/// </summary>
		public List<SfcsProductFamilyAddOrModifyModel> removeRecords { get; set; }
	}
}
