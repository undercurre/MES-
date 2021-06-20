/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsPn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:48
	///  批量保存实体
	/// </summary>
	public class SfcsPnModel
	{
		/// <summary>
		/// 新增的记录
		/// </summary>
		public List<SfcsPnAddOrModifyModel> insertRecords { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<SfcsPnAddOrModifyModel> updateRecords { get; set; }

		///// <summary>
		///// 删除的记录
		///// </summary>
		//public List<SfcsPnAddOrModifyModel> removeRecords { get; set; }
	}
}
