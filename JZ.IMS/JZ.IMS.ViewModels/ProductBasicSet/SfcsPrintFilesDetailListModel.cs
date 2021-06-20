/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：打印标签设计内容配置表 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-30 17:23:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsPrintFilesDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-30 17:23:53
	/// 打印标签设计内容配置表 列表显示实体
	/// </summary>
	public class SfcsPrintFilesDetailListModel
	{
		/// <summary>
		/// 表ID
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// SFCS_PRINT_FILES.ID
		/// </summary>
		public Decimal PRINT_FILES_ID {get;set;}

		/// <summary>
		/// 文件内容配置
		/// </summary>
		public Byte[] FILE_CONTENT {get;set;}

		/// <summary>
		/// 创建用户
		/// </summary>
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CREATE_TIME {get;set;}


	}
}
