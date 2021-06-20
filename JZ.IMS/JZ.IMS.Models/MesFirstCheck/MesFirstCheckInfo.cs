/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：首五件检验主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-13 09:51:01                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesFirstCheckInfo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-13 09:51:01
	/// 首五件检验主表
	/// </summary>
	[Table("MES_FIRST_CHECK_INFO")]
	public partial class MesFirstCheckInfo
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ORGANIZE_ID { get; set; }

		/// <summary>
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID { get; set; }

		/// <summary>
		/// 部门
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DEPARTMENT { get; set; }

		/// <summary>
		/// 状态，0：未审核，1：已审核
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS { get; set; }

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PART_NO { get; set; }

		/// <summary>
		/// 物料名称
		/// </summary>
		[MaxLength(200)]
		public String PART_NAME { get; set; }

		/// <summary>
		/// 物料规格
		/// </summary>
		[MaxLength(200)]
		public String PART_DESC { get; set; }

		/// <summary>
		/// 批号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String BATCH_NO { get; set; }

		/// <summary>
		/// 批量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BATCH_QTY { get; set; }

		/// <summary>
		/// 是否新产品（Y/N）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String NEW_PART { get; set; }

		/// <summary>
		/// 生产日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime PRODUCT_DATE { get; set; }

		/// <summary>
		/// 判定结果，1：合格，2，不合格
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal RESULT_STATUS { get; set; }

		/// <summary>
		/// 判定说明
		/// </summary>
		[MaxLength(200)]
		public String RESULT_REMARK { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER { get; set; }

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? AUDIT_TIME { get; set; }

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(30)]
		public String AUDIT_USER { get; set; }

		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime UPDATE_TIME { get; set; }

		/// <summary>
		/// 最后更新人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String UPDATE_USER { get; set; }

		/// <summary>
		/// 属性1
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE1 { get; set; }

		/// <summary>
		/// 属性2
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE2 { get; set; }

		/// <summary>
		/// 属性3
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE3 { get; set; }

		/// <summary>
		/// 属性4
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE4 { get; set; }

		/// <summary>
		/// 属性5
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE5 { get; set; }

		/// <summary>
		/// 线别名称
		/// </summary>
		[NotMapped]
		public String LINE_NAME { get; set; }

		/// <summary>
		/// 部门名称
		/// </summary>
		[NotMapped]
		public String DEPARTMENT_NAME { get; set; }
	}
}
