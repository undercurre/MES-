/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫规则配置推送人员表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-13 10:57:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallGroupRuleRe                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-13 10:57:48
	/// 呼叫规则配置推送人员表
	/// </summary>
	[Table("ANDON_CALL_GROUP_RULE_RE")]
	public partial class AndonCallGroupRuleRe
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 呼叫规则配置推送规则ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PERSON_RULE_ID {get;set;}

		/// <summary>
		/// 接收者分组主表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal GROUP_ID {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime UPDATE_TIME {get;set;}

		/// <summary>
		/// 是否激活（Y:是 N: 否）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
