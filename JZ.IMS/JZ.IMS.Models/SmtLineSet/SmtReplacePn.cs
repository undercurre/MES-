/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-23 09:03:32                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtReplacePn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-23 09:03:32
	/// 
	/// </summary>
	[Table("SMT_REPLACE_PN")]
	public partial class SmtReplacePn
	{
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(200)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(200)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 生产工单
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 成品料号
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String PCB_PN {get;set;}

		/// <summary>
		/// 当前料号
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String COMPONENT_PN {get;set;}

		/// <summary>
		/// 替代料号
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String REPLACE_PN {get;set;}

		/// <summary>
		/// 供应商
		/// </summary>
		[MaxLength(200)]
		public String VENDOR_CODE {get;set;}

		/// <summary>
		/// 制造商料号
		/// </summary>
		[MaxLength(200)]
		public String MAKER_PN {get;set;}

		/// <summary>
		/// 是否可用
		/// </summary>
		[MaxLength(4)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 起效时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? BEGINTIME {get;set;}

		/// <summary>
		/// 失效时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? ENDTIME {get;set;}

		[MaxLength(22)]
		public Decimal? COMPONENT_PN_QTY {get;set;}

		[MaxLength(22)]
		public Decimal? REPLACE_PN_QTY {get;set;}


	}
}
