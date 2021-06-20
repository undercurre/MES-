/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫通知内容                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-23 22:15:35                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallNotice                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-09-23 22:15:35
	/// 呼叫通知内容
	/// </summary>
	[Table("ANDON_CALL_NOTICE")]
	public partial class AndonCallNotice
	{
		[Key]
		public Decimal Id {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal VERSION {get;set;}

		[Required]
		[MaxLength(50)]
		public String ENABLE_BILL_ID {get;set;}

		[Required]
		[MaxLength(50)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 呼叫记录ID(ANDON_CALL_ RECORD.ID)
		/// </summary>
		[MaxLength(22)]
		public Decimal? MST_ID {get;set;}

		/// <summary>
		/// 通知类型(sms：短信；email：邮件)
		/// </summary>
		[MaxLength(50)]
		public String NOTICE_TYPE {get;set;}

		/// <summary>
		/// 通知内容
		/// </summary>
		[MaxLength(250)]
		public String NOTICE_CONTENT {get;set;}

		/// <summary>
		/// 通知时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? NOTICT_TIME {get;set;}

		/// <summary>
		/// 通知状态（0：待通知；1：已通知）
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		[Required]
		[MaxLength(30)]
		public String ATTRIBUTE1 {get;set;}

		[Required]
		[MaxLength(30)]
		public String ATTRIBUTE2 {get;set;}

		[Required]
		[MaxLength(30)]
		public String ATTRIBUTE3 {get;set;}

		[Required]
		[MaxLength(30)]
		public String ATTRIBUTE4 {get;set;}

		[Required]
		[MaxLength(30)]
		public String ATTRIBUTE5 {get;set;}


	}
}
