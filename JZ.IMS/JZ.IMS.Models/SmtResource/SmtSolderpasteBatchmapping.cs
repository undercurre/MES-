/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-03 16:03:23                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtSolderpasteBatchmapping                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-03 16:03:23
	/// 
	/// </summary>
	[Table("SMT_SOLDERPASTE_BATCHMAPPING")]
	public partial class SmtSolderpasteBatchmapping
	{
		[Required]
		[MaxLength(50)]
		public String ID {get;set;}

		[Required]
		[MaxLength(50)]
		public String BATCH_NO {get;set;}

		[Required]
		[MaxLength(200)]
		public String FRIDGE_LOC {get;set;}

		[Required]
		[MaxLength(50)]
		public String REEL_NO {get;set;}

		[Required]
		[MaxLength(20)]
		public String OPERATOR {get;set;}

		[Required]
		[MaxLength(7)]
		public DateTime OPERATION_TIME {get;set;}

		[MaxLength(100)]
		public String REMARK {get;set;}

		[MaxLength(20)]
		public String CREATED_BY {get;set;}

		[MaxLength(7)]
		public DateTime? CREATED_DATE {get;set;}

		[MaxLength(20)]
		public String UPDATED_BY {get;set;}

		[MaxLength(7)]
		public DateTime? UPDATED_DATE {get;set;}


	}
}
