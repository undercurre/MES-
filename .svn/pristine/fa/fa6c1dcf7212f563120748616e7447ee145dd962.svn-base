/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫规则配置推送规则表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-13 11:38:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallPersonRule                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-13 11:38:53
	/// 呼叫规则配置推送规则表
	/// </summary>
	[Table("ANDON_CALL_PERSON_RULE")]
	public partial class AndonCallPersonRule
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 呼叫规则配置ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CALL_RULE_ID {get;set;}

		/// <summary>
		/// 呼叫等级（0：一级 1：二级 2：三级）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal? CALL_LEVEL {get;set;}

		/// <summary>
		/// 推送规则（0：时间规则）
		/// </summary>
		[MaxLength(22)]
		public Decimal? RULE_TYPE {get;set;}

		/// <summary>
		/// 规则内容
		/// </summary>
		[MaxLength(50)]
		public String RULE {get;set;}

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
		[MaxLength(30)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 规则单位（0：秒 1：分 2：时）
		/// </summary>
		[MaxLength(22)]
		public Decimal? RULE_UNIT {get;set;}


	}
}
