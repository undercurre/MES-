/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-07 13:42:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtDrivingRecordMst                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-07 13:42:53
	/// 
	/// </summary>
	[Table("SMT_DRIVING_RECORD_MST")]
	public partial class SmtDrivingRecordMst
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 表单号
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String TABLE_NO {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime TEST_TIME { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String CREATE_USER {get;set;}


		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATE_USER_NAME { get; set; }

		/// <summary>
		/// 审核状态(0：待审核；1：已审核)
		/// </summary>
		[MaxLength(22)]
		public Decimal? CHECK_STATUS {get;set;}

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(100)]
		public String CHECKER {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CHECK_TIME {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String ORGANIZE_ID {get;set;}

		/// <summary>
		/// 属性1
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE1 {get;set;}

		/// <summary>
		/// 属性2
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE2 {get;set;}

		/// <summary>
		/// 属性3
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE3 {get;set;}

		/// <summary>
		/// 属性4
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE4 {get;set;}

		/// <summary>
		/// 属性5
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE5 {get;set;}


	}
}
