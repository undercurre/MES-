/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-11 10:19:01                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtFeeder                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-11 10:19:01
	/// 
	/// </summary>
	[Table("SMT_FEEDER")]
	public partial class SmtFeeder
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 料架编号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String FEEDER {get;set;}

		/// <summary>
		/// 供应商
		/// </summary>
		[Required]
		[MaxLength(150)]
		public String SUPPLIER {get;set;}

		/// <summary>
		/// 类型
		/// </summary>
		[MaxLength(50)]
		public String FTYPE {get;set;}

		/// <summary>
		/// 尺寸
		/// </summary>
		[MaxLength(50)]
		public String FSIZE {get;set;}

		/// <summary>
		/// 本体编码
		/// </summary>
		[MaxLength(50)]
		public String FBODYMARK {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		[MaxLength(22)]
		public Decimal? CHECK_USED_COUNT {get;set;}

		[MaxLength(22)]
		public Decimal? EMEND_USED_COUNT {get;set;}

		[MaxLength(22)]
		public Decimal? TOTAL_USED_COUNT {get;set;}

		[MaxLength(7)]
		public DateTime? LAST_CHECK_TIME {get;set;}

		[MaxLength(7)]
		public DateTime? LAST_EMEND_TIME {get;set;}

		[MaxLength(7)]
		public DateTime? LAST_REPAIR_TIME {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(1000)]
		public String DESCRIPTION {get;set;}

		[MaxLength(50)]
		public String CREATE_BY {get;set;}

		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		[MaxLength(50)]
		public String UPDATE_BY {get;set;}

		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 组织架构ID
		/// </summary>
		[MaxLength(50)]
		public String ORGANIZE_ID { get; set; }

	}
}
