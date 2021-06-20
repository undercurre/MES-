/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：打印标签设计内容配置表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-30 17:23:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsPrintFilesDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-30 17:23:53
	/// 打印标签设计内容配置表
	/// </summary>
	[Table("SFCS_PRINT_FILES_DETAIL")]
	public partial class SfcsPrintFilesDetail
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// SFCS_PRINT_FILES.ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRINT_FILES_ID {get;set;}

		/// <summary>
		/// 文件内容配置
		/// </summary>
		[MaxLength(4000)]
		public Byte[] FILE_CONTENT {get;set;}

		/// <summary>
		/// 创建用户
		/// </summary>
		[MaxLength(100)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}


	}
}
