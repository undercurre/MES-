/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫通知接收人                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-27 11:00:05                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallNoticeReceiver                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-09-27 11:00:05
	/// 呼叫通知接收人
	/// </summary>
	[Table("ANDON_CALL_NOTICE_RECEIVER")]
	public partial class AndonCallNoticeReceiver
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
		/// 呼叫通知内容ID(ANDON_CALL_NOTICE.ID)
		/// </summary>
		[MaxLength(22)]
		public Decimal? MST_ID {get;set;}

		/// <summary>
		/// 通知接收人ID
		/// </summary>
		[MaxLength(22)]
		public Decimal USER_ID {get;set;}

		/// <summary>
		/// 通知接收人用户名
		/// </summary>
		[MaxLength(50)]
		public String USER_NAME { get; set; }

		/// <summary>
		/// 通知接收帐号
		/// </summary>
		[MaxLength(50)]
		public String NOTICE_ACCOUNT {get;set;}

		/// <summary>
		/// 通知类型(sms：短信；email：邮件)
		/// </summary>
		[MaxLength(50)]
		public String NOTICE_TYPE {get;set;}

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
