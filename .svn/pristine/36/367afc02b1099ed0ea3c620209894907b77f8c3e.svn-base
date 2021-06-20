/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产能报工表结构                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-08 09:11:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsCapReport                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-08 09:11:53
	/// 产能报工表结构
	/// </summary>
	[Table("SFCS_CAP_REPORT")]
	public partial class SfcsCapReport
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_ID {get;set;}

		/// <summary>
		/// 站点ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal OPERATION_SITE_ID {get;set;}

		/// <summary>
		/// 用户
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String OPERATOR {get;set;}

		/// <summary>
		/// 报工数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QTY {get;set;}

		/// <summary>
		/// 报工时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime REPORT_TIME {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 包装ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? CARTON_ID {get;set;}


	}
}
