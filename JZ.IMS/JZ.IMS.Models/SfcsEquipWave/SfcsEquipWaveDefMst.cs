/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-29 16:20:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipWaveDefMst                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-29 16:20:53
	/// 
	/// </summary>
	[Table("SFCS_EQUIP_WAVE_DEF_MST")]
	public partial class SfcsEquipWaveDefMst
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal? STATUS { get; set; }
		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal? ORGANIZE_ID {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal? LINE_ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 单双波
		/// </summary>
		[MaxLength(100)]
		public String WAVE {get;set;}

		/// <summary>
		/// 喷雾气压
		/// </summary>
		[MaxLength(100)]
		public String PRESSURE {get;set;}

		/// <summary>
		/// 链速
		/// </summary>
		[MaxLength(100)]
		public String SPEED {get;set;}

		/// <summary>
		/// 仰角
		/// </summary>
		[MaxLength(100)]
		public String CONER {get;set;}

		/// <summary>
		/// 流量
		/// </summary>
		[MaxLength(100)]
		public String FLOW {get;set;}

		/// <summary>
		/// 助焊剂厂家
		/// </summary>
		[MaxLength(100)]
		public String FLUX_FROM {get;set;}

		/// <summary>
		/// 锡焊厂家
		/// </summary>
		[MaxLength(100)]
		public String SOLDERING_FROM {get;set;}

		/// <summary>
		/// 临时改善措施
		/// </summary>
		[MaxLength(100)]
		public String DEAL {get;set;}

		/// <summary>
		/// 处理人
		/// </summary>
		[MaxLength(100)]
		public String DEAL_USER {get;set;}

		/// <summary>
		/// 处理时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? DEAL_TIME {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(100)]
		public String REMARK {get;set;}

		/// <summary>
		/// 审核结果
		/// </summary>
		[MaxLength(100)]
		public String CHECK_RESULT {get;set;}

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(100)]
		public String CHECK_USER {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CHECK_TIME {get;set;}

		/// <summary>
		/// 抽检时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? COLLECT_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(100)]
		public String CREATE_USER { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime CREATE_TIME { get; set; }

		[MaxLength(100)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(100)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(100)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(100)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(100)]
		public String ATTRIBUTE5 {get;set;}


	}
}
