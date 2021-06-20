/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-03 17:21:30                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsScraperCleanHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-03 17:21:30
	/// 
	/// </summary>
	[Table("SFCS_SCRAPER_CLEAN_HISTORY")]
	public partial class SfcsScraperCleanHistory
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 刮刀号
		/// </summary>
		[MaxLength(30)]
		public String SCRAPER_NO {get;set;}

		/// <summary>
		/// 使用次数
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRINT_COUNT {get;set;}

		/// <summary>
		/// 清洗人
		/// </summary>
		[MaxLength(20)]
		public String CLEAN_USER {get;set;}

		[MaxLength(20)]
		public String INSPECT_USER {get;set;}

		/// <summary>
		/// 结果
		/// </summary>
		[MaxLength(200)]
		public String INSPECT_RESULT {get;set;}

		/// <summary>
		/// 清洗时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CLEAN_TIME {get;set;}

		[MaxLength(7)]
		public DateTime? NEXT_CLEAN_TIME {get;set;}

	}
}
