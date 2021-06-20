/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫规则配置表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-12 17:36:04                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallRuleConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-12 17:36:04
	/// 呼叫规则配置表
	/// </summary>
	[Table("ANDON_CALL_RULE_CONFIG")]
	public partial class AndonCallRuleConfig
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 异常内容配置ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CALL_CONTENT_ID {get;set;}

		/// <summary>
		/// 是否激活（Y:是 N: 否）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}


	}
}
