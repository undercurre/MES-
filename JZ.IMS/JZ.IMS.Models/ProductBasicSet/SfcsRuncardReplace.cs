/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-15 18:50:09                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsRuncardReplace                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-15 18:50:09
	/// 
	/// </summary>
	[Table("SFCS_RUNCARD_REPLACE")]
	public partial class SfcsRuncardReplace
	{
		[Key]
		public Decimal REPLACE_SN_ID {get;set;}

		[MaxLength(22)]
		public Decimal? REPLACE_OPERATION_ID {get;set;}

		[MaxLength(22)]
		public Decimal? SN_ID {get;set;}

		[MaxLength(45)]
		public String OLD_SN {get;set;}

		[MaxLength(45)]
		public String NEW_SN {get;set;}

		[MaxLength(22)]
		public Decimal? REPLACE_SITE_ID {get;set;}

		[MaxLength(22)]
		public Decimal? REPLACE_TYPE {get;set;}

		[MaxLength(60)]
		public String REPLACE_REASON {get;set;}

		[MaxLength(60)]
		public String REPLACE_REMARK {get;set;}

		[MaxLength(20)]
		public String REPLACE_BY {get;set;}

		[MaxLength(7)]
		public DateTime? REPLACE_TIME {get;set;}


	}
}
