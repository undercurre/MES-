/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-18 15:41:41                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtStation                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-18 15:41:41
	/// 
	/// </summary>
	[Table("SMT_STATION")]
	public partial class SmtStation
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(200)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(200)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 机器名称
		/// </summary>
		[Required]
		[MaxLength(400)]
		public String SMT_NAME {get;set;}

		/// <summary>
		/// 机器类型
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TYPE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(2000)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否可用
		/// </summary>
		[MaxLength(4)]
		public String ENABLED {get;set;}


	}
}
