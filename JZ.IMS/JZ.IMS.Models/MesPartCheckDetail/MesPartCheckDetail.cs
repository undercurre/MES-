/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-03-03 14:04:01                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesPartCheckDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-03-03 14:04:01
	/// 
	/// </summary>
	[Table("MES_PART_CHECK_DETAIL")]
	public partial class MesPartCheckDetail
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 条码编号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String REEL_CODE {get;set;}

		/// <summary>
		/// 物料号
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 主表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal HEADER_ID {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		[MaxLength(10)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(100)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? QTY { get; set; }
	}
}
