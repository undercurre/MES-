/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-23 16:57:11                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsTurninBatchHeader                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-23 16:57:11
	/// 
	/// </summary>
	[Table("SFCS_TURNIN_BATCH_HEADER")]
	public partial class SfcsTurninBatchHeader
	{
		[Key]
		public String BATCH_NO {get;set;}

		[Required]
		[MaxLength(20)]
		public String SUBINVENTORY_CODE {get;set;}

		[Required]
		[MaxLength(20)]
		public String LOCATION {get;set;}

		[MaxLength(22)]
		public Decimal? QUANTITY {get;set;}

		[MaxLength(1)]
		public String BATCH_FLAG {get;set;}

		[MaxLength(1)]
		public String PROCESS_FLAG {get;set;}

		[MaxLength(20)]
		public String SLIP_NUMBER {get;set;}

		[MaxLength(1)]
		public String FAIL_UNITS {get;set;}

		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		[MaxLength(1)]
		public String IO_FLAG {get;set;}

		[MaxLength(30)]
		public String IO_REMARK {get;set;}

		[MaxLength(30)]
		public String CREATED_BY {get;set;}


	}
}
