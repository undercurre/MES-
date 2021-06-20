/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-17 17:16:58                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtLineConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-17 17:16:58
	/// 
	/// </summary>
	[Table("SMT_LINE_CONFIG")]
	public partial class SmtLineConfig
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 线别
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 配置类型
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CONFIG_TYPE {get;set;}

		/// <summary>
		/// 配置值
		/// </summary>
		[MaxLength(200)]
		public String VALUE {get;set;}

		/// <summary>
		/// 说明
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
