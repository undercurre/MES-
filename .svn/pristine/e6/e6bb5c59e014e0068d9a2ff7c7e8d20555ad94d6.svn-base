/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-17 10:47:28                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtFeederMaintain                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-17 10:47:28
	/// 
	/// </summary>
	[Table("SMT_FEEDER_MAINTAIN")]
	public partial class SmtFeederMaintain
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 料架编号
		/// </summary>
		[MaxLength(22)]
		public Decimal? FEEDER_ID {get;set;}

		/// <summary>
		/// 使用次数
		/// </summary>
		[MaxLength(22)]
		public Decimal? USED_COUNT {get;set;}

		/// <summary>
		/// 维护类型
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAINTAIN_KIND {get;set;}

		/// <summary>
		/// 维护人
		/// </summary>
		[MaxLength(50)]
		public String MAINTAIN_BY {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(50)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 维护时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? MAINTAIN_TIME {get;set;}


	}
}
