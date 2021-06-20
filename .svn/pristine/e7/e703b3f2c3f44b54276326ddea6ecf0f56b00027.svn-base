/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：工装验证记录详细内容表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-19 16:48:05                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsValidationDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-01-19 16:48:05
	/// 工装验证记录详细内容表
	/// </summary>
	[Table("MES_TONGS_VALIDATION_DETAIL")]
	public partial class MesTongsValidationDetail
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// MES_TONGS_VALIDATION_HEAD表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal VALIDATION_HEAD_ID {get;set;}

		/// <summary>
		/// 工装ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_ID {get;set;}

		/// <summary>
		/// 工装验证状态(0:未验证,1:合格,2:不合格)
		/// </summary>
		[MaxLength(22)]
		public Decimal? TONGS_STATUS {get;set;}

		/// <summary>
		/// 验证备注
		/// </summary>
		[MaxLength(1000)]
		public String CHECK_REMARKS {get;set;}

		/// <summary>
		/// 验证时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CHECK_TIME {get;set;}

		/// <summary>
		/// 验证人员
		/// </summary>
		[MaxLength(255)]
		public String CHECK_USER {get;set;}

		/// <summary>
		/// 保养ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_MAINTAIN_ID {get;set;}


	}
}
