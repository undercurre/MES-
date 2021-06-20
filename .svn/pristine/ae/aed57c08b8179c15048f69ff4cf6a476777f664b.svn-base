/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动产生序列号记录表，使用在自动产生卡通号，自动产生栈板号等。                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-08 15:38:28                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsContainerList                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-08 15:38:28
	/// 自动产生序列号记录表，使用在自动产生卡通号，自动产生栈板号等。
	/// </summary>
	[Table("SFCS_CONTAINER_LIST")]
	public partial class SfcsContainerList
	{
		/// <summary>
		/// 类型 SFCS_PARAMETERS.LOOKUP_TYPE=CONTAINER_TYPE
		/// </summary>
		[MaxLength(22)]
		public Decimal? DATA_TYPE {get;set;}

		/// <summary>
		/// 序号
		/// </summary>
		[MaxLength(100)]
		public String CONTAINER_SN {get;set;}

		/// <summary>
		/// 料号 SFCS_PN.PART_NO
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 工单ID SFCS_WO.ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? WO_ID {get;set;}

		/// <summary>
		/// 流水号 SFCS_RUNCARD.SN
		/// </summary>
		[MaxLength(60)]
		public String RUNCARD_SN {get;set;}

		/// <summary>
		/// 工序 暂无使用
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_CODE {get;set;}

		/// <summary>
		/// 站点ID SFCS_OPERATION_SITES.ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? SITE_ID {get;set;}

		/// <summary>
		/// 发票号 批次号
		/// </summary>
		[MaxLength(30)]
		public String INVOICE {get;set;}

		/// <summary>
		/// 客户订单号
		/// </summary>
		[MaxLength(40)]
		public String CUSTOMER_PO {get;set;}

		/// <summary>
		/// 版本
		/// </summary>
		[MaxLength(30)]
		public String REVISION {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? QUANTITY {get;set;}

		/// <summary>
		/// 是否已刷满 Y/N
		/// </summary>
		[MaxLength(1)]
		public String FULL_FLAG {get;set;}

		/// <summary>
		/// 序列
		/// </summary>
		[MaxLength(22)]
		public Decimal? SEQUENCE {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATED_DATE {get;set;}

		/// <summary>
		/// 最后一次更新时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATED_DATE {get;set; }

		/// <summary>
		/// 流水号范围规则ID SFCS_RUNCARD_RANGER_RULES.ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? RANGER_RULE_ID { get; set; }

		/// <summary>
		/// 进制 SFCS_PARAMETERS RADIX_TYPE
		/// </summary>
		[MaxLength(22)]
		public Decimal? DIGITAL { get; set; }

		/// <summary>
		/// 变化位数
		/// </summary>
		[MaxLength(22)]
		public Decimal? RANGE { get; set; }

		/// <summary>
		/// 固定头码
		/// </summary>
		[MaxLength(50)]
		public String FIX_HEADER { get; set; }

		/// <summary>
		/// 固定尾码
		/// </summary>
		[MaxLength(50)]
		public String FIX_TAIL { get; set; }


	}
}
