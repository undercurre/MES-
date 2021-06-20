/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-17 11:14:59                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsStoplineHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-17 11:14:59
	/// 
	/// </summary>
	[Table("SFCS_STOPLINE_HISTORY")]
	public partial class SfcsStoplineHistory
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? HEADER_ID {get;set;}

		/// <summary>
		/// 单据号
		/// </summary>
		[MaxLength(40)]
		public String BATCH_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 锁定站点
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_SITE_ID {get;set;}

		/// <summary>
		/// PASS数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? PASS_COUNT {get;set;}

		/// <summary>
		/// FAIL数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? FAIL_COUNT {get;set;}

		/// <summary>
		/// 误测数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? NDF_COUNT {get;set;}

		/// <summary>
		/// 产品总量
		/// </summary>
		[MaxLength(22)]
		public Decimal? TOTAL_COUNT {get;set;}

		/// <summary>
		/// 不良率
		/// </summary>
		[MaxLength(22)]
		public Decimal? FAIL_RATE {get;set;}

		/// <summary>
		/// 误测率
		/// </summary>
		[MaxLength(22)]
		public Decimal? NDF_RATE {get;set;}

		/// <summary>
		/// 停线管控分割标准
		/// </summary>
		[MaxLength(22)]
		public Decimal? DIVISION_CRITERIA {get;set;}

		/// <summary>
		/// 是否累计误测
		/// </summary>
		[MaxLength(1)]
		public String INCLUDE_NDF {get;set;}

		/// <summary>
		/// 停线模式
		/// </summary>
		[MaxLength(22)]
		public Decimal? STOPLINE_MODE {get;set;}

		/// <summary>
		/// 异常类型
		/// </summary>
		[MaxLength(22)]
		public Decimal? ISSUE_TYPE {get;set;}

		/// <summary>
		/// 异常信息
		/// </summary>
		[MaxLength(4000)]
		public String ISSUE_MESSAGE {get;set;}

		/// <summary>
		/// 异常时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? ISSUE_TIME {get;set;}


	}
}
