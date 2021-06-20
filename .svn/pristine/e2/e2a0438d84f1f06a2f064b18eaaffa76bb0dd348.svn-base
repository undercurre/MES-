/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：工装验证记录主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-21 14:33:33                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipmentValidationHead                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-01-21 14:33:33
	/// 工装验证记录主表
	/// </summary>
	[Table("SFCS_EQUIPMENT_VALIDATION_HEAD")]
	public partial class SfcsEquipmentValidationHead
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ID {get;set;}

		/// <summary>
		/// 设备验证编号（'EQVL'+'YYYYMMDDHHMMSS'）
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CHECK_CODE {get;set;}

		/// <summary>
		/// 设备验证状态(0：新增 1：未审核 3：已审核)
		/// </summary>
		[MaxLength(22)]
		public Decimal? CHECK_STATUS {get;set;}

		/// <summary>
		/// 设备总数
		/// </summary>
		[MaxLength(22)]
		public Decimal? EQUIPMENT_QTY {get;set;}

		/// <summary>
		/// 验证次数
		/// </summary>
		[MaxLength(22)]
		public Decimal? CHECK_QTY {get;set;}

		/// <summary>
		/// 点检人员
		/// </summary>
		[MaxLength(100)]
		public String CHECK_USER {get;set;}

		/// <summary>
		/// 点检开始时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CHECK_START_TIME {get;set;}

		/// <summary>
		/// 点检备注
		/// </summary>
		[MaxLength(1000)]
		public String CHECK_REMARKS {get;set;}

		/// <summary>
		/// 审核人员
		/// </summary>
		[MaxLength(100)]
		public String AUDIT_USER {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? AUDIT_TIME {get;set;}

		/// <summary>
		/// 审核备注
		/// </summary>
		[MaxLength(1000)]
		public String AUDIT_REMARKS {get;set;}

		/// <summary>
		/// 组织架构ID
		/// </summary>
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}

		/// <summary>
		/// 验证结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CHECK_END_TIME {get;set;}

		/// <summary>
		/// 验证次数(月/次)
		/// </summary>
		[MaxLength(22)]
		public Decimal? CHECK_COUNT {get;set;}


	}
}
