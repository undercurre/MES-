/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：连板记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-30 19:13:16                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsCollectMultiRuncard                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-30 19:13:16
	/// 连板记录表
	/// </summary>
	[Table("SFCS_COLLECT_MULTI_RUNCARD")]
	public partial class SfcsCollectMultiRuncard
	{
		/// <summary>
		/// 连板批次ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ID {get;set;}

		/// <summary>
		/// 流水号ID SFCS_RUNCARD.ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SN_ID {get;set;}

		/// <summary>
		/// 工单ID SFCS_WO.ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_ID {get;set;}

		/// <summary>
		/// 连板SN排序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORDER_NO {get;set;}

		/// <summary>
		/// 连板工序在制程中的PRODUCT_OPERATION_CODE SFCS_ROUTE_CONFIG.PRODUCT_OPERATION_CODE
		/// </summary>
		[MaxLength(22)]
		public Decimal? LINK_OPERATION_CODE {get;set;}

		/// <summary>
		/// 拆板工序在制程中的PRODUCT_OPERATION_CODE SFCS_ROUTE_CONFIG.PRODUCT_OPERATION_CODE
		/// </summary>
		[MaxLength(22)]
		public Decimal? BREAK_OPERATION_CODE {get;set;}

		/// <summary>
		/// 连板站点ID SFCS_OPERATION_SITES.ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? COLLECT_SITE_ID {get;set;}

		/// <summary>
		/// 拆板站点ID SFCS_OPERATION_SITES.ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? BREAK_SITE_ID {get;set;}

		/// <summary>
		/// 连板状态 连板 打断 SFCS_PARAMETERS.LOOKUP_TYPE=MULTI_RUNCARD_STATUS
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 连板操作者 工号
		/// </summary>
		[MaxLength(20)]
		public String COLLECT_BY {get;set;}

		/// <summary>
		/// 连板时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? COLLECT_TIME {get;set;}

		/// <summary>
		/// 打断操作者 工号
		/// </summary>
		[MaxLength(20)]
		public String BREAK_BY {get;set;}

		/// <summary>
		/// 打断时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? BREAK_TIME {get;set;}


	}
}
