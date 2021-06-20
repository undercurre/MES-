/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-27 10:33:10                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtBom1                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-27 10:33:10
	/// 
	/// </summary>
	[Table("SMT_BOM1")]
	public partial class SmtBom1
	{
		/// <summary>
		/// 
		/// </summary>
		[MaxLength(22)]
		public Decimal? SHEET_NO {get;set;}

		[MaxLength(22)]
		public Decimal? SHEET_TYPE {get;set;}

		[Required]
		[MaxLength(100)]
		public String PARTENT_CODE {get;set;}

		[Required]
		[MaxLength(800)]
		public String CREATE_DATE {get;set;}

		[Required]
		[MaxLength(800)]
		public String ALTER_DATE {get;set;}

		[MaxLength(22)]
		public Decimal? SHEET_DATE {get;set;}

		[Required]
		[MaxLength(4)]
		public String SHEET_STA {get;set;}

		[Required]
		[MaxLength(800)]
		public String AUDIT_DATE {get;set;}

		[MaxLength(400)]
		public String USER_NO {get;set;}

		[MaxLength(400)]
		public String AUDIT_USER {get;set;}

		[MaxLength(510)]
		public String REM {get;set;}

		[Key]
		public String BOM_ID {get;set;}

		[Required]
		[MaxLength(8)]
		public String VERSION_TIMES {get;set;}

		[Required]
		[MaxLength(100)]
		public String BOM_TYPE {get;set;}


	}
}
