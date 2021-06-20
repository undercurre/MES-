/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产线挪料表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-01-09 17:26:19                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesMovePart                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-01-09 17:26:19
	/// 产线挪料表
	/// </summary>
	[Table("MES_MOVE_PART")]
	public partial class MesMovePart
	{
		/// <summary>
		/// ID主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 工序ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal OPERATION_ID {get;set;}

		/// <summary>
		/// 类型（1：产线加料，2：产线减料）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MOVE_TYPE {get;set;}

		/// <summary>
		/// 成品料号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PRODUCT_NO {get;set;}

		/// <summary>
		/// 零件料号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 零件数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PART_QTY {get;set;}

		/// <summary>
		/// 零件位号
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String PART_LOC {get;set;}

		/// <summary>
		/// 状态（0：初始，1：已审核）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 开始时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? BEGIN_DATE {get;set;}

		/// <summary>
		/// 结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? END_DATE {get;set;}

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(30)]
		public String AUDIT_USER {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? AUDIT_DATE {get;set;}

		/// <summary>
		/// 是否有效（Y/N）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_DATE {get;set; }

		/// <summary>
		/// 工序名称
		/// </summary>
		[NotMapped]
		public String OPERATION_NAME { get; set; }
	}
}
