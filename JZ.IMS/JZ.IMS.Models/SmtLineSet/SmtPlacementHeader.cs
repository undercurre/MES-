/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-06 16:48:13                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtPlacementHeader                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-06 16:48:13
	/// 
	/// </summary>
	[Table("SMT_PLACEMENT_HEADER")]
	public partial class SmtPlacementHeader
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}


		//版本
		[MaxLength(22)]
		public Decimal? VERSION { get; set; }

		/// <summary>
		/// 料单
		/// </summary>
		[MaxLength(2000)]
		public String PLACEMENT {get;set;}

		/// <summary>
		/// 机台ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATION_ID {get;set;}

		/// <summary>
		/// 成品料号
		/// </summary>
		[MaxLength(100)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 板型
		/// </summary>
		[MaxLength(22)]
		public Decimal? PCB_SIDE {get;set;}

		/// <summary>
		/// 料单说明
		/// </summary>
		[MaxLength(2000)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否可用
		/// </summary>
		[MaxLength(2)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 审核状态
		/// </summary>
		[MaxLength(100)]
		public String CHECKED {get;set;}

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(100)]
		public String CHECKED_BY {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CHECKED_TIME {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal HI_OUTPUT_TIME {get;set;}

		[MaxLength(100)]
		public String PCB_ROUTE_CODE {get;set;}

		[MaxLength(22)]
		public Decimal? STANDARD_CAPACITY {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(100)]
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		[MaxLength(100)]
		public String UPDATE_BY {get;set;}


	}
}
