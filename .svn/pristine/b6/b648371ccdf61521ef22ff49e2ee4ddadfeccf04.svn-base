/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-07 15:04:42                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProduction                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-07 15:04:42
	/// 
	/// </summary>
	[Table("SFCS_PRODUCTION")]
	public partial class SfcsProduction
	{
		[Key]
		public String BATCH_NO { get;set;}

		[MaxLength(22)]
		public Decimal? LINE_ID {get;set;}

		[MaxLength(200)]
		public String WO_NO {get;set;}

		[MaxLength(200)]
		public String PCB_PN {get;set;}

		[MaxLength(22)]
		public Decimal? PCB_SIDE {get;set;}

		[MaxLength(200)]
		public String MODEL {get;set;}

		[Required]
		[MaxLength(7)]
		public DateTime START_TIME {get;set;}

		[Required]
		[MaxLength(200)]
		public String START_BY {get;set;}

		[Required]
		[MaxLength(7)]
		public DateTime END_TIME {get;set;}

		[Required]
		[MaxLength(200)]
		public String END_BY {get;set;}

		[Required]
		[MaxLength(4)]
		public String FINISHED {get;set;}

		[MaxLength(22)]
		public Decimal? STATION_ID {get;set;}

		[MaxLength(22)]
		public Decimal? OPERATION_TYPE {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal PLACEMENT_MST_ID {get;set;}

		[Required]
		[MaxLength(200)]
		public String BACK_WO_NO {get;set;}

		[MaxLength(22)]
		public Decimal? MULTI_NO { get; set; }

		[Required]
		[MaxLength(200)]
		public String LOC_NO { get; set; }


	}
}
