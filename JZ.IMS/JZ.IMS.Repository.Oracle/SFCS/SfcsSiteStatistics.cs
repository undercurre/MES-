/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：站点统计表，统计产品的首次通过率 FPY=First Pass Yields                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-17 16:29:30                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsSiteStatistics                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-09-17 16:29:30
	/// 站点统计表，统计产品的首次通过率 FPY=First Pass Yields
	/// </summary>
	[Table("SFCS_SITE_STATISTICS")]
	public partial class SfcsSiteStatistics
	{
		[MaxLength(22)]
		public Decimal? WO_ID {get;set;}

		[MaxLength(22)]
		public Decimal? OPERATION_SITE_ID {get;set;}

		[MaxLength(7)]
		public DateTime? WORK_TIME {get;set;}

		[MaxLength(22)]
		public Decimal? PASS {get;set;}

		[MaxLength(22)]
		public Decimal? FAIL {get;set;}

		[MaxLength(22)]
		public Decimal? REPASS {get;set;}

		[MaxLength(22)]
		public Decimal? REFAIL {get;set;}


	}
}
