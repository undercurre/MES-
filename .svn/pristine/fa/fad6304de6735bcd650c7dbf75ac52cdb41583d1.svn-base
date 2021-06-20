/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-12 14:02:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtDefectsRecords                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-12 14:02:53
	/// 
	/// </summary>
	[Table("SMT_DEFECTS_RECORDS")]
	public partial class SmtDefectsRecords
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}

		/// <summary>
		/// 班别
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WORK_CLASS {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String MODEL {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? EXAMINE_TIME {get;set;}

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(50)]
		public String EXAMINE_USER {get;set;}

		/// <summary>
		/// 1
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE1 {get;set;}

		/// <summary>
		/// 2
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE2 {get;set;}

		/// <summary>
		/// 3
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE3 {get;set;}

		/// <summary>
		/// 4
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE4 {get;set;}

		/// <summary>
		/// 5
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE5 {get;set;}

		/// <summary>
		/// 维修编号
		/// </summary>
		[MaxLength(50)]
		public String REPAIR_NO {get;set;}

		/// <summary>
		/// 订单号
		/// </summary>
		[MaxLength(50)]
		public String ORDER_NO {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 维修人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String REPAIR_USER {get;set;}

		/// <summary>
		/// 维修时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime REPAIR_TIME {get;set;}

		/// <summary>
		/// 当前状态 0:待审核 1:已审核
		/// </summary>
		[MaxLength(50)]
		public String STATUS {get;set;}


	}
}
