/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-07 13:43:07                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtDrivingRecordDtl                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-07 13:43:07
	/// 
	/// </summary>
	[Table("SMT_DRIVING_RECORD_DTL")]
	public partial class SmtDrivingRecordDtl
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 主表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MST_ID {get;set;}

		/// <summary>
		/// 元件位置
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String POINT {get;set;}

		/// <summary>
		/// 元件封装
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SEAL {get;set;}

		/// <summary>
		/// 测试值
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal VALUE {get;set;}

		/// <summary>
		/// 标准值
		/// </summary>
		[MaxLength(22)]
		public Decimal? STANDER_VALUE {get;set;}

		/// <summary>
		/// 测试结果
		/// </summary>
		[MaxLength(22)]
		public Decimal? RESULT {get;set;}

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
		[MaxLength(50)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATE_USER_NAME { get; set; }

		/// <summary>
		/// 属性1
		/// </summary>
		[MaxLength(50)]
		public string ATTRIBUTE1 {get;set;}

		/// <summary>
		/// 属性2
		/// </summary>
		[MaxLength(50)]
		public string ATTRIBUTE2 {get;set;}

		/// <summary>
		/// 属性3
		/// </summary>
		[MaxLength(50)]
		public string ATTRIBUTE3 {get;set;}

		/// <summary>
		/// 属性4
		/// </summary>
		[MaxLength(50)]
		public string ATTRIBUTE4 {get;set;}

		/// <summary>
		/// 属性5
		/// </summary>
		[MaxLength(50)]
		public string ATTRIBUTE5 {get;set;}


	}
}
