/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-11 14:06:34                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsCollectUids                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-09-11 14:06:34
	/// 
	/// </summary>
	[Table("SFCS_COLLECT_UIDS")]
	public partial class SfcsCollectUids
	{
		[Key]
		public Decimal COLLECT_UID_ID {get;set;}

		[MaxLength(22)]
		public Decimal? OPERATION_ID {get;set;}

		[MaxLength(22)]
		public Decimal? SN_ID {get;set;}

		[MaxLength(22)]
		public Decimal? WO_ID {get;set;}

		[MaxLength(22)]
		public Decimal? PRODUCT_OPERATION_CODE {get;set;}

		[MaxLength(22)]
		public Decimal? UID_ID {get;set;}

		[MaxLength(30)]
		public String UID_NAME {get;set;}

		[MaxLength(100)]
		public String UID_NUMBER {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal PLANT_CODE {get;set;}

		[MaxLength(22)]
		public Decimal? UID_QTY {get;set;}

		[MaxLength(22)]
		public Decimal? ORDER_NO {get;set;}

		[MaxLength(1)]
		public String SERIALIZED {get;set;}

		[MaxLength(1)]
		public String REWORK_REMOVE_FLAG {get;set;}

		[MaxLength(1)]
		public String REPLACE_FLAG {get;set;}

		[MaxLength(1)]
		public String EDI_FLAG {get;set;}

		[MaxLength(22)]
		public Decimal? COLLECT_SITE {get;set;}

		[MaxLength(20)]
		public String COLLECT_BY {get;set;}

		[MaxLength(7)]
		public DateTime? COLLECT_TIME {get;set;}


	}
}
