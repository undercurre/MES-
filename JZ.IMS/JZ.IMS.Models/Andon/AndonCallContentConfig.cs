/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫内容配置                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-12 11:10:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallContentConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-12 11:10:38
	/// 呼叫内容配置
	/// </summary>
	[Table("ANDON_CALL_CONTENT_CONFIG")]
	public partial class AndonCallContentConfig
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 呼叫类型代码/异常类型代码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CALL_TYPE_CODE {get;set;}

		/// <summary>
		/// 呼叫内容/异常内容模板（EN）
		/// </summary>
		[Required]
		[MaxLength(250)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 呼叫内容/异常内容模板（CN）
		/// </summary>
		[Required]
		[MaxLength(250)]
		public String CHINESE {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 呼叫内容代码/异常代码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CALL_CODE {get;set;}

		/// <summary>
		/// 异常标题
		/// </summary>
		[MaxLength(100)]
		public String CALL_TITLE {get;set;}

		/// <summary>
		/// 异常种类代码
		/// </summary>
		[MaxLength(50)]
		public String CALL_CATEGORY_CODE {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(10)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 异常种类名称
		/// </summary>
		[NotMapped]
		public String CALL_CATEGORY_NAME { get; set; }

		/// <summary>
		/// 呼叫类型代码/异常类型名称
		/// </summary>
		[NotMapped]
		public String CALL_TYPE_NAME { get; set; }


	}
}
