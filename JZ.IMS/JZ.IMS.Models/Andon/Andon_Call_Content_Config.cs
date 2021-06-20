/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-03-07 16:50:56                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：CallContent                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models {
	public partial class Andon_Call_Content_Config {
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public decimal ID { get; set; }

		/// <summary>
		/// 
		/// </summary>		
		
		public decimal VERSION { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MaxLength(50)]
		public string ENABLE_BILL_ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MaxLength(50)]
		public string DISABLE_BILL_ID { get; set; }

		/// <summary>
		/// 呼叫类型代码
		/// </summary>
		[MaxLength(50)]
		[Required]
		public string CALL_TYPE_CODE { get; set; }

		/// <summary>
		/// 呼叫内容代码
		/// </summary>
		[MaxLength(50)]
		[Required]
		public string CALL_CODE { get; set; }

		/// <summary>
		/// 呼叫内容（EN）
		/// </summary>
		[MaxLength(250)]
		[Required]
		public string DESCRIPTION { get; set; }

		/// <summary>
		/// 呼叫内容（CN）
		/// </summary>
		[MaxLength(250)]
		[Required]
		public string CHINESE { get; set; }

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		[Required]
		public string ENABLED { get; set; }
	}
}
