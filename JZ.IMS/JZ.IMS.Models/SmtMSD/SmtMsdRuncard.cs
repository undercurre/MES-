/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-06-20 10:43:03                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtMsdRuncard                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-06-20 10:43:03
	/// 
	/// </summary>
	[Table("SMT_MSD_RUNCARD")]
	public partial class SmtMsdRuncard
	{
		[Key]
		public String REEL_ID {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal CURRENT_ACTION {get;set;}

		[MaxLength(22)]
		public Decimal? TEMPERATURE {get;set;}

		[MaxLength(22)]
		public Decimal? HUMIDITY {get;set;}

		[MaxLength(22)]
		public Decimal? TOTAL_OPEN_TIME {get;set;}

		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		[MaxLength(50)]
		public String OPERATOR_BY {get;set;}

		[MaxLength(7)]
		public DateTime BEGIN_TIME {get;set;}

		[MaxLength(7)]
		public DateTime END_TIME {get;set;}

		[MaxLength(7)]
		public DateTime FLOOR_LIFE_END_TIME {get;set;}

		[MaxLength(50)]
		public String LEVEL_CODE {get;set;}

		[MaxLength(22)]
		public Decimal? THICKNESS {get;set;}

		[MaxLength(50)]
		public String AREA {get;set;}

     
    }
}
