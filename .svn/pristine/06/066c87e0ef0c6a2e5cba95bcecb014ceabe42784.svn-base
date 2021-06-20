/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-16 12:07:39                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductStopline                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-16 12:07:39
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_STOPLINE")]
	public partial class SfcsProductStopline
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 停线管控模式
		/// </summary>
		[MaxLength(22)]
		public Decimal? STOPLINE_MODE {get;set;}

		/// <summary>
		/// 停线管控工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? STOP_OPERATION_CODE {get;set;}

		/// <summary>
		/// 警告标准
		/// </summary>
		[MaxLength(22)]
		public Decimal? ALARM_CRITERIA {get;set;}

		/// <summary>
		/// 停线标准
		/// </summary>
		[MaxLength(22)]
		public Decimal? STOP_CRITERIA {get;set;}

		/// <summary>
		/// 分割标准
		/// </summary>
		[MaxLength(22)]
		public Decimal? DIVISION_CRITERIA {get;set;}

		/// <summary>
		/// 开始计算切入点
		/// </summary>
		[MaxLength(22)]
		public Decimal? DIVISION_START {get;set;}

		/// <summary>
		/// 单位
		/// </summary>
		[MaxLength(22)]
		public Decimal? DIVISION_UNIT {get;set;}

		/// <summary>
		/// 警报间隔(PCS)
		/// </summary>
		[MaxLength(22)]
		public Decimal? ALARM_INTERVAL {get;set;}

		/// <summary>
		/// 停线后是否限制投入
		/// </summary>
		[MaxLength(1)]
		public String INPUT_CONTROL {get;set;}

		/// <summary>
		/// 投入工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? INPUT_OPERATION_CODE {get;set;}

		/// <summary>
		/// 投入标准(PCS)
		/// </summary>
		[MaxLength(22)]
		public Decimal? INPUT_CONTROL_CRITERIA {get;set;}

		/// <summary>
		/// 是否累计误测
		/// </summary>
		[MaxLength(1)]
		public String INCLUDE_NDF {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 工序ID(2020-08-24新增)
		/// </summary>
		[MaxLength(22)]
		public Decimal? STOP_OPERATION_ID { get; set; }

	}
}
