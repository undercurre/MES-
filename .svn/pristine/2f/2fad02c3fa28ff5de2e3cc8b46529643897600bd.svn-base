/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-08 10:25:06                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtPlacementDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-08 10:25:06
	/// 
	/// </summary>
	[Table("SMT_PLACEMENT_DETAIL")]
	public partial class SmtPlacementDetail
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}


		/// <summary>
		/// 主表主键
		/// </summary>
		[MaxLength(22)]
		public Decimal? MST_ID {get;set;}

		[MaxLength(100)]
		public String TABLENO {get;set;}

		/// <summary>
		/// VERSION
		/// </summary>
		[MaxLength(22)]
		public Decimal? VERSION { get; set; }

		/// <summary>
		/// 模块
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String STAGE {get;set;}

		/// <summary>
		/// 料站
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String SLOT {get;set;}

		/// <summary>
		/// 副料站
		/// </summary>
		[MaxLength(100)]
		public String SUB_SLOT {get;set;}

		/// <summary>
		/// 站位
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String LOCATION {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 单位用量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal UNITQTY {get;set;}

		/// <summary>
		/// 料号描述
		/// </summary>
		[MaxLength(2000)]
		public String PNDESC {get;set;}

		/// <summary>
		/// 料架规格
		/// </summary>
		[MaxLength(2000)]
		public String FEEDERTYPE {get;set;}

		/// <summary>
		/// 位号
		/// </summary>
		[MaxLength(4000)]
		public String REFDESIGNATOR {get;set;}

		/// <summary>
		/// 是否可用
		/// </summary>
		[MaxLength(2)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 跳站
		/// </summary>
		[MaxLength(2)]
		public String SKIP {get;set;}

		[MaxLength(100)]
		public String LOCATION_KEY {get;set;}

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
