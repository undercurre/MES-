/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-23 09:21:27                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ImportMst                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-23 09:21:27
	/// 导入主表
	/// </summary>
	[Table("IMPORT_MST")]
	public partial class ImportMst
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 目标表名
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String TOTABLE_NAME {get;set;}

		/// <summary>
		/// 中文说明
		/// </summary>
		[MaxLength(100)]
		public String DESC_CN {get;set;}

		/// <summary>
		/// 英文说明
		/// </summary>
		[MaxLength(100)]
		public String DESC_EN {get;set;}

		/// <summary>
		/// 使用的序列 
		/// </summary>
		[MaxLength(100)]
		public String USED_SEQUENCE {get;set;}


	}
}
