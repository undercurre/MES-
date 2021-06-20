/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：烘烤标准表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-06-15 10:44:32                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtMsdBakeRule                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-06-15 10:44:32
	/// 烘烤标准表
	/// </summary>
	[Table("SMT_MSD_BAKE_RULE")]
	public partial class SmtMsdBakeRule
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
		/// 暴露最小温度
		/// </summary>
		[MaxLength(22)]
		public Decimal? MIN_OPEN_TEMPERATURE {get;set;}

		/// <summary>
		/// 暴露最大温度
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAX_OPEN_TEMPERATURE {get;set;}

		/// <summary>
		/// 暴露最小湿度
		/// </summary>
		[MaxLength(22)]
		public Decimal? MIN_OPEN_HUMIDITY {get;set;}

		/// <summary>
		/// 暴露最大湿度
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAX_OPEN_HUMIDITY {get;set;}

		/// <summary>
		/// 烘烤温度
		/// </summary>
		[MaxLength(22)]
		public Decimal? BAKE_TEMPERATURE {get;set;}

		/// <summary>
		/// 烘烤湿度
		/// </summary>
		[MaxLength(22)]
		public Decimal? BAKE_HUMIDITY {get;set;}

		/// <summary>
		/// 烘烤时间
		/// </summary>
		[MaxLength(22)]
		public Decimal? BAKE_TIME {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(5)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 清空暴露时间
		/// </summary>
		[MaxLength(5)]
		public String CLEAR_OPEN_TIME {get;set;}

		/// <summary>
		/// 超时下限
		/// </summary>
		[MaxLength(22)]
		public Decimal? MIN_OVER_TIME {get;set;}

		/// <summary>
		/// 超时上限
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAX_OVER_TIME {get;set;}


	}
}
