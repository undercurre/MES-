/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-05 09:21:49                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtStencilConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-05 09:21:49
	/// 
	/// </summary>
	[Table("SMT_STENCIL_CONFIG")]
	public partial class SmtStencilConfig
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 网板编号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String STENCIL_NO {get;set;}

		/// <summary>
		/// 站点ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_SITE_ID {get;set;}

		/// <summary>
		/// 产品连板单位值
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_UNITAGE {get;set;}

		/// <summary>
		/// 警报小时
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ALARM_HOURS {get;set;}

		/// <summary>
		/// 停止小时
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STOP_HOURS {get;set;}

		/// <summary>
		/// 警报间隔时间分钟
		/// </summary>
		[MaxLength(22)]
		public Decimal? INTERVAL {get;set;}

		/// <summary>
		/// 是否启用最大数量
		/// </summary>
		[MaxLength(1)]
		public String MAX_USED_FLAG {get;set;}

		/// <summary>
		/// 详细描述
		/// </summary>
		[MaxLength(100)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 最大使用数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? CUSTOM_MAX_USED_COUNT {get;set;}

		/// <summary>
		/// 是否张力值管控
		/// </summary>
		[MaxLength(1)]
		public String TENSION_CONTROL_FLAG {get;set;}

		/// <summary>
		/// 最小张力值 
		/// </summary>
		[MaxLength(22)]
		public Decimal? TENSION_CONTROL_VALUE {get;set;}

		/// <summary>
		/// 最后清洗时间 
		/// </summary>
		[MaxLength(7)]
		public DateTime? LAST_CLEAN_TIME { get; set; }

		/// <summary>
		/// 最后报警时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? LAST_ALARM_TIME { get; set; }

	}
}
