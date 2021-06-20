/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：生产计划表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-11 14:01:42                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtProducePlan                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-09-11 14:01:42
	/// 生产计划表
	/// </summary>
	[Table("SMT_PRODUCE_PLAN")]
	public partial class SmtProducePlan
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 计划日期
		/// </summary>
		[MaxLength(7)]
		public DateTime PLAN_DATE {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 订单号
		/// </summary>
		[MaxLength(100)]
		public String ORDER_NO {get;set;}

		/// <summary>
		/// 机芯
		/// </summary>
		[MaxLength(100)]
		public String MOVEMENT {get;set;}

		/// <summary>
		/// 机型
		/// </summary>
		[MaxLength(100)]
		public String MACHINE_TYPE {get;set;}

		/// <summary>
		/// 类型
		/// </summary>
		[MaxLength(30)]
		public String TYPE_ID {get;set;}

		/// <summary>
		/// 订单量
		/// </summary>
		[MaxLength(22)]
		public Decimal? ORDER_QUANTITY {get;set;}

		/// <summary>
		/// 排产量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PLAN_QUANTITY {get;set;}

		/// <summary>
		/// 国家
		/// </summary>
		[MaxLength(100)]
		public String NATIONALITY {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(200)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 创建日期
		/// </summary>
		[MaxLength(20)]
		public String CREATE_DATE {get;set;}

		/// <summary>
		/// 计划类型(0:SMT, 1:DIP)
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PLAN_TYPE { get; set; }


		//----------------- DIP计划字段 ----------------------- 

		/// <summary>
		/// 工单结余
		/// </summary>
		public string WO_SURPLUS { get; set; }

		/// <summary>
		/// 交期
		/// </summary>
		public string DELIVERY_DATE { get; set; }

		/// <summary>
		/// 客户工单
		/// </summary>
		public string CUSTOMER_WO { get; set; }

		/// <summary>
		/// 客户BOM
		/// </summary>
		public string CUSTOMER_BOM { get; set; }


	}
}
