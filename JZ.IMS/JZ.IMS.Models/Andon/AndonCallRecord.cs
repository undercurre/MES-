/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫记录                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-22 19:24:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallRecord                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-09-22 19:24:38
	/// 呼叫记录
	/// </summary>
	[Table("ANDON_CALL_RECORD")]
	public partial class AndonCallRecord
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
		/// 呼叫编号【呼叫类型代码(1位)+年月日(6位)+流水号(5位)】
		/// </summary>
		[MaxLength(50)]
		public String CALL_NO {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_LINE_ID {get;set;}

		/// <summary>
		/// 线体名称
		/// </summary>
		[MaxLength(50)]
		public String OPERATION_LINE_NAME {get;set;}

		/// <summary>
		/// 工序ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_ID {get;set;}

		/// <summary>
		/// 工序名称
		/// </summary>
		[MaxLength(50)]
		public String OPERATION_NAME {get;set;}

		/// <summary>
		/// 站位ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_SITE_ID {get;set;}

		/// <summary>
		/// 站位名称
		/// </summary>
		[MaxLength(50)]
		public String OPERATION_SITE_NAME {get;set;}

		/// <summary>
		/// 呼叫人
		/// </summary>
		[MaxLength(50)]
		public String OPERATOR {get;set;}

		/// <summary>
		/// 呼叫时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 呼叫类型代码
		/// </summary>
		[MaxLength(50)]
		public String CALL_TYPE_CODE {get;set;}

		/// <summary>
		/// 呼叫内容
		/// </summary>
		[MaxLength(250)]
		public String CALL_CONTENT {get;set;}

		/// <summary>
		/// 状态（0：待处理；1：处理中；2：处理成功；3：处理失败）
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		/// <summary>
		/// 签到时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CHECKIN_TIME {get;set;}

		/// <summary>
		/// 签到人员
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CHECKIN_OPERATOR {get;set;}

		/// <summary>
		/// 完结时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime END_TIME {get;set;}

		/// <summary>
		/// 完结操作员
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String END_OPERATOR {get;set;}

		/// <summary>
		/// 已通知次数
		/// </summary>
		[MaxLength(22)]
		public Decimal? NOTICE_COUNT {get;set;}

		/// <summary>
		/// 最后通知时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime LAST_NOTICE_TIME {get;set;}

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

		/// <summary>
		/// 呼叫内容代码
		/// </summary>
		[MaxLength(50)]
		public String CALL_CODE {get;set;}

		/// <summary>
		/// 呼叫类型名称
		/// </summary>
		[NotMapped]
		public String CALL_TYPE_NAME { get; set; }

		/// <summary>
		/// 异常标题
		/// </summary>
		[MaxLength(100)]
		public String CALL_TITLE { get; set; }
	}
}
