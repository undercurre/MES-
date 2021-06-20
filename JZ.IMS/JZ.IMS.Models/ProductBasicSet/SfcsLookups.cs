/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:37:18                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsLookups                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:37:18
	/// 
	/// </summary>
	[Table("SFCS_LOOKUPS")]
	public partial class SfcsLookups
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 类型
		/// </summary>
		[MaxLength(22)]
		public Decimal? KIND {get;set;}

		/// <summary>
		/// 键值
		/// </summary>
		[MaxLength(100)]
		public String CODE {get;set;}

		/// <summary>
		/// 英文描述
		/// </summary>
		[MaxLength(250)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 中文描述
		/// </summary>
		[MaxLength(250)]
		public String CHINESE {get;set;}

		/// <summary>
		/// 种类
		/// </summary>
		[MaxLength(500)]
		public String CATEGORY {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

	}
}
