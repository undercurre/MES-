/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-09 09:51:57                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsComponentReplace                                     
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.ViewModels.ProductBasicSet.ComponentReplace;
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-09 09:51:57
	///  批量保存实体
	/// </summary>
	public class SfcsReplaceModel<T>
	{
		/// <summary>
		/// 新增的记录
		/// </summary>
		public List<T> InsertRecords { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<T> UpdateRecords { get; set; }

		/// <summary>
		/// 删除的记录
		/// </summary>
		//public List<T> RemoveRecords { get; set; }

		/// <summary>
		/// 用户
		/// </summary>
		public string UserName { get; set; }
	}
}
