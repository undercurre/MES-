/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 16:44:04                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsWo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-14 16:44:04
	/// 
	/// </summary>
	[Table("SFCS_WO")]
	public partial class SfcsWo
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// OEM料号
		/// </summary>
		[MaxLength(30)]
		public String OEM_PN {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MODEL_ID {get;set;}

		[MaxLength(22)]
		public Decimal? SO_ID {get;set;}

		/// <summary>
		/// 工单状态
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_STATUS {get;set;}

		/// <summary>
		/// 工单类型
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_TYPE {get;set;}

		/// <summary>
		/// 工单生产阶段
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STAGE_CODE {get;set;}

		/// <summary>
		/// 厂部
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PLANT_CODE {get;set;}

		/// <summary>
		/// 制程
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ROUTE_ID {get;set;}

		/// <summary>
		/// 目标产量
		/// </summary>
		[MaxLength(22)]
		public Decimal? TARGET_QTY {get;set;}

		/// <summary>
		/// 投入量
		/// </summary>
		[MaxLength(22)]
		public Decimal? INPUT_QTY {get;set;}

		/// <summary>
		/// 产出量
		/// </summary>
		[MaxLength(22)]
		public Decimal? OUTPUT_QTY {get;set;}

		/// <summary>
		/// 报废量
		/// </summary>
		[MaxLength(22)]
		public Decimal? SCRAP_QTY {get;set;}

		[MaxLength(22)]
		public Decimal? TRANSFER_QTY {get;set;}

		/// <summary>
		/// 出货量
		/// </summary>
		[MaxLength(22)]
		public Decimal? SHIPPED_QTY {get;set;}

		/// <summary>
		/// 是否自动存仓
		/// </summary>
		[MaxLength(1)]
		public String TURNIN_TYPE {get;set;}

		/// <summary>
		/// 制造群
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BU_CODE {get;set;}

		/// <summary>
		/// 工单类型
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CLASSIFICATION {get;set;}

		/// <summary>
		/// 计划投产日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? START_DATE {get;set;}

		/// <summary>
		/// 计划完成日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? DUE_DATE {get;set;}

		/// <summary>
		/// 实际投产日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? ACTUAL_START_DATE {get;set;}

		/// <summary>
		/// 实际完成日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? ACTUAL_DUE_DATE {get;set;}

		/// <summary>
		/// 出货日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? SHIP_DATE {get;set;}

		/// <summary>
		/// 物料投入日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? MATERIAL_START_DATE {get;set;}

		/// <summary>
		/// 物料释放日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? MATERIAL_RELEASED_DATE {get;set;}

		/// <summary>
		/// 制造类别
		/// </summary>
		[MaxLength(22)]
		public Decimal? MANUFACTURE_TYPE {get;set;}

		[MaxLength(22)]
		public Decimal? WIP_STATUS_TYPE {get;set;}


	}
}
