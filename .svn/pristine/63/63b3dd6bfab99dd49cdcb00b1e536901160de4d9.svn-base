/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：报表自定义SQL-语句表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 16:19:07                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ReportMst                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-07-22 16:19:07
	/// 报表自定义SQL-语句表
	/// </summary>
	[Table("REPORT_MST")]
	public partial class ReportMst
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// sql语句
		/// </summary>
		[Required]
		[MaxLength(4000)]
		public String SQL {get;set;}

		/// <summary>
		/// 字段配置 JSON
		/// </summary>
		[MaxLength(4000)]
		public String COLUMN_CONFIG { get; set; }

		/// <summary>
		/// 工具栏配置 JSON
		/// </summary>
		[MaxLength(4000)]
		public String TOOLBAR_CONFIG { get; set; }

		/// <summary>
		/// 创建日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_DATE {get;set;}

		/// <summary>
		/// 最后修改日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? LAST_EDIT_DATE {get;set;}

		/// <summary>
		/// 接口名称
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String ACTION_NAME {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(50)]
		public String REMARK {get;set;}

		[MaxLength(1)]
		public String ENABLED { get;set;}


	}
}
