/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-17 11:59:41                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtLines                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-17 11:59:41
	/// 
	/// </summary>
	[Table("SMT_LINES")]
	public partial class SmtLines
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 线体名称
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String LINE_NAME {get;set;}

		/// <summary>
		/// 位置
		/// </summary>
		[MaxLength(600)]
		public String LOCATION {get;set;}

		/// <summary>
		/// 厂部
		/// </summary>
		[MaxLength(600)]
		public String PLANT {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		public Decimal ORGANIZE_ID { get; set; }

	}
}
