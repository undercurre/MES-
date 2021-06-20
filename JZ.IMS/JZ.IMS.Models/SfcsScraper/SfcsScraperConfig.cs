/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-03 10:54:15                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsScraperConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-03 10:54:15
	/// 
	/// </summary>
	[Table("SFCS_SCRAPER_CONFIG")]
	public partial class SfcsScraperConfig
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		[MaxLength(22)]
		public Decimal ID {get;set;}

		/// <summary>
		/// 刮刀号
		/// </summary>
		[MaxLength(50)]
		public String SCRAPER_NO {get;set;}

		/// <summary>
		/// 储位
		/// </summary>
		[MaxLength(50)]
		public String LOCATION {get;set;}

		/// <summary>
		/// 入库日期
		/// </summary>
		[MaxLength(30)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		[MaxLength(20)]
		public String MAX_USE_FLAG {get;set;}

		/// <summary>
		/// 警报小时数
		/// </summary>
		[MaxLength(22)]
		public Decimal? ALARM_HOURS {get;set;}

		/// <summary>
		/// 停用小时数
		/// </summary>
		[MaxLength(22)]
		public Decimal? STOP_HOURS {get;set;}

		/// <summary>
		/// 警报间隔(分钟)
		/// </summary>
		[MaxLength(22)]
		public Decimal? INTERVAL {get;set;}

	}
}
