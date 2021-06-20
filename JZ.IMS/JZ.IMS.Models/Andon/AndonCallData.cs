/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：异常呼叫记录数据表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-20 10:24:27                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallData                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-20 10:24:27
	/// 异常呼叫记录数据表
	/// </summary>
	[Table("ANDON_CALL_DATA")]
	public partial class AndonCallData
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 呼叫编号（HJ+YYMMDDHHMISS）
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CALL_NO {get;set;}

		/// <summary>
		/// 异常内容配置表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CALL_CONTENT_ID {get;set;}

		/// <summary>
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 线别名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String LINE_NAME {get;set;}

		/// <summary>
		/// 工序ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_ID {get;set;}

		/// <summary>
		/// 工序名称
		/// </summary>
		[MaxLength(50)]
		public String OPERATION_NAME {get;set;}

		/// <summary>
		/// 站点ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_SITE_ID {get;set;}

		/// <summary>
		/// 站点名称
		/// </summary>
		[MaxLength(50)]
		public String OPERATION_SITE_NAME {get;set;}

		/// <summary>
		/// 操作员
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String OPERATOR {get;set;}

		/// <summary>
		/// 操作时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(50)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 制程ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? ROUTE_ID {get;set;}

		/// <summary>
		/// 制程名称
		/// </summary>
		[MaxLength(50)]
		public String ROUTE_NAME {get;set;}

		/// <summary>
		/// 标准值
		/// </summary>
		[MaxLength(50)]
		public String STANDARD_DATA {get;set;}

		/// <summary>
		/// 标准最小值
		/// </summary>
		[MaxLength(50)]
		public String MIN_STANDARD_DATA {get;set;}

		/// <summary>
		/// 标准最大值
		/// </summary>
		[MaxLength(50)]
		public String MAX_STANDARD_DATA {get;set;}

		/// <summary>
		/// 实际值
		/// </summary>
		[MaxLength(50)]
		public String ACTUAL_DATA {get;set;}

		/// <summary>
		/// 实际最小值
		/// </summary>
		[MaxLength(50)]
		public String MIN_ACTUAL_DATA {get;set;}

		/// <summary>
		/// 实际最大值
		/// </summary>
		[MaxLength(50)]
		public String MAX_ACTUAL_DATA {get;set;}

		/// <summary>
		/// 通知状态（1：已通知 0：未通知 2：无需通知）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}


	}
}
