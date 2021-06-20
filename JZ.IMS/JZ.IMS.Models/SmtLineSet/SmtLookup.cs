/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-21 10:49:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtLookup                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-21 10:49:48
	/// 
	/// </summary>
	[Table("SMT_LOOKUP")]
	public partial class SmtLookup
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
		/// 类型
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String TYPE {get;set;}

		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CODE {get;set;}

		/// <summary>
		/// 键值
		/// </summary>
		[Required]
		[MaxLength(800)]
		public String VALUE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(2000)]
		public String EN_DESC {get;set;}

		/// <summary>
		/// 描述2
		/// </summary>
		[MaxLength(2000)]
		public String CN_DESC {get;set;}

		/// <summary>
		/// 是否可用
		/// </summary>
		[Required]
		[MaxLength(4)]
		public String ENABLED {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE5 {get;set;}


	}
}
