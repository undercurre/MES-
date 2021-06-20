/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：等级标准表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-06-18 16:47:16                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtMsdLevelRule                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-06-18 16:47:16
	/// 等级标准表
	/// </summary>
	[Table("SMT_MSD_LEVEL_RULE")]
	public partial class SmtMsdLevelRule
	{
		/// <summary>
		/// 唯一标识码
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 元件等级
		/// </summary>
		[MaxLength(10)]
		public String LEVEL_CODE {get;set;}

		/// <summary>
		/// 最小厚度
		/// </summary>
		[MaxLength(22)]
		public Decimal? MIN_THICKNESS {get;set;}

		/// <summary>
		/// 最大厚度
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAX_THICKNESS {get;set;}

		/// <summary>
		/// 暴露温度
		/// </summary>
		[MaxLength(22)]
		public Decimal? TEMPERATURE {get;set;}

		/// <summary>
		/// 暴露湿度
		/// </summary>
		[MaxLength(22)]
		public Decimal? HUMIDITY {get;set;}

		/// <summary>
		/// 使用周期
		/// </summary>
		[MaxLength(22)]
		public Decimal? FLOOR_LIFE {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
