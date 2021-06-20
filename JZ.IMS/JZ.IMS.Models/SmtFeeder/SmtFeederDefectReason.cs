/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-12 16:43:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtFeederDefectReason                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-12 16:43:25
	/// 
	/// </summary>
	[Table("SMT_FEEDER_DEFECT_REASON")]
	public partial class SmtFeederDefectReason
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}


		/// <summary>
		/// 失效代码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CODE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String DESCRIPTION {get;set;}

		[MaxLength(200)]
		public String CHINESE {get;set;}

		/// <summary>
		/// 是否可用
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
