/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫记录处理                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-23 16:09:13                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：AndonCallRecordHandle                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-09-23 16:09:13
	/// 呼叫记录处理
	/// </summary>
	[Table("ANDON_CALL_RECORD_HANDLE")]
	public partial class AndonCallRecordHandle
	{
		[Key]
		public Decimal Id {get;set;}

		[MaxLength(22)]
		public Decimal VERSION {get;set;}

		[MaxLength(50)]
		public String ENABLE_BILL_ID {get;set;}

		
		[MaxLength(50)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 呼叫记录ID(ANDON_CALL_ RECORD.ID)
		/// </summary>
		[MaxLength(22)]
		public Decimal? MST_ID {get;set;}

		/// <summary>
		/// 处理人
		/// </summary>
		[MaxLength(50)]
		public String HANDLER {get;set;}

		/// <summary>
		/// 处理时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? HANDLE_TIME {get;set;}

		/// <summary>
		/// 处理内容
		/// </summary>
		[MaxLength(250)]
		public String HANDLE_CONTENT {get;set;}

		/// <summary>
		/// 处理结果（0：待后续处理&不完结；1：处理成功&完结；2：处理失败&完结）
		/// </summary>
		[MaxLength(22)]
		public Decimal? HANDLE_STATUS {get;set;}

		 
		[MaxLength(30)]
		public String ATTRIBUTE1 {get;set;}

		 
		[MaxLength(30)]
		public String ATTRIBUTE2 {get;set;}

		 
		[MaxLength(30)]
		public String ATTRIBUTE3 {get;set;}

		 
		[MaxLength(30)]
		public String ATTRIBUTE4 {get;set;}

		 
		[MaxLength(30)]
		public String ATTRIBUTE5 {get;set;}


		/// <summary>
		/// 责任归属
		/// </summary>
		[MaxLength(50)]
		public String TO_USER { get; set; }

		/// <summary>
		/// 解决方案
		/// </summary>
	    [MaxLength(50)]
		public String SOLUTION { get; set; }

	}
}
