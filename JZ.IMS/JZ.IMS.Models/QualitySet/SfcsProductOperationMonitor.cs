/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-20 11:41:41                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductOperationMonitor                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-20 11:41:41
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_OPERATION_MONITOR")]
	public partial class SfcsProductOperationMonitor
	{
		/// <summary>
		/// 主键
		/// </summary>
		[MaxLength(22)]
		public Decimal? ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 监控模式
		/// </summary>
		[MaxLength(22)]
		public Decimal? MONITOR_MODE {get;set;}

		/// <summary>
		/// 起始工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BEGIN_OPERATION_CODE {get;set;}

		/// <summary>
		/// 结束工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal END_OPERATION_CODE {get;set;}

		/// <summary>
		/// 警告标准
		/// </summary>
		[MaxLength(22)]
		public Decimal? ALARM_CRITERIA {get;set;}

		/// <summary>
		/// 中止标准
		/// </summary>
		[MaxLength(22)]
		public Decimal? STOP_CRITERIA {get;set;}

		/// <summary>
		/// 单位
		/// </summary>
		[MaxLength(22)]
		public Decimal? CRITERIA_UNIT {get;set;}

		/// <summary>
		/// 比对模式
		/// </summary>
		[MaxLength(22)]
		public Decimal? COMPARE_MODE {get;set;}

		/// <summary>
		/// 是否中止时锁定
		/// </summary>
		[MaxLength(1)]
		public String STOP_AND_HOLD {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
