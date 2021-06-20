/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-22 10:08:05                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsHoldProductDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-22 10:08:05
	/// 
	/// </summary>
	[Table("SFCS_HOLD_PRODUCT_DETAIL")]
	public partial class SfcsHoldProductDetail
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 主表ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? HOLD_ID {get;set;}

		/// <summary>
		/// 流水号ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? SN_ID {get;set;}

		/// <summary>
		/// 零件序号
		/// </summary>
		[MaxLength(80)]
		public String COMPONENT_SN {get;set;}

		/// <summary>
		/// 客户零件料号
		/// </summary>
		[MaxLength(50)]
		public String CUSTOMER_COMPONENT_PN {get;set;}

		/// <summary>
		/// 管控动作
		/// </summary>
		[MaxLength(22)]
		public Decimal? HOLD_ACTION {get;set;}

		/// <summary>
		/// 是否受控
		/// </summary>
		[MaxLength(30)]
		public String STATUS {get;set;}

		/// <summary>
		/// 管控时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 成品料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(30)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 管控线别ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_LINE_ID {get;set;}

		/// <summary>
		/// 管控站点ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_SITE_ID {get;set;}

		/// <summary>
		/// 管控工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRODUCT_OPERATION_CODE {get;set;}

		/// <summary>
		/// 当前工序ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? CURRENT_OPERATION_ID {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		[MaxLength(100)]
		public String MODEL {get;set;}

		/// <summary>
		/// 解锁原因
		/// </summary>
		[MaxLength(2000)]
		public String RELEASE_CAUSE {get;set;}

		/// <summary>
		/// 解锁人员
		/// </summary>
		[MaxLength(20)]
		public String RELEASE_EMPNO {get;set;}

		/// <summary>
		/// 解锁时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? RELEASE_TIME {get;set;}


	}
}
