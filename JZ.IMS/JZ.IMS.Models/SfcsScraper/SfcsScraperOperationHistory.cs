/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-06 10:47:18                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsScraperOperationHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-06 10:47:18
	/// 
	/// </summary>
	[Table("SFCS_SCRAPER_OPERATION_HISTORY")]
	public partial class SfcsScraperOperationHistory
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 刮刀号
		/// </summary>
		[MaxLength(20)]
		public String SCRAPER_NO {get;set;}

		/// <summary>
		/// 刮刀状态
		/// </summary>
		[MaxLength(22)]
		public Decimal? SCRAPER_STATUS {get;set;}

		/// <summary>
		/// 作业工号
		/// </summary>
		[MaxLength(10)]
		public String WORKER_NO {get;set;}

		/// <summary>
		/// 作业时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? OPERATION_TIME {get;set;}

		/// <summary>
		/// 作业人
		/// </summary>
		[MaxLength(30)]
		public String OPERATION_BY {get;set;}


	}
}
