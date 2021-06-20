/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-27 11:18:23                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtWoV                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-27 11:18:23
	/// 
	/// </summary>
	[Table("SMT_WO_V")]
	public partial class SmtWoV
	{
		/// <summary>
		/// 工单ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ID {get;set;}

		/// <summary>
		///  工单号
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 料号ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PN_ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 料号名称
		/// </summary>
		[MaxLength(300)]
		public String PN_NAME {get;set;}

		/// <summary>
		/// 机种ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MODEL_ID {get;set;}

		/// <summary>
		/// 机种名称
		/// </summary>
		[MaxLength(100)]
		public String MODEL_NAME {get;set;}

		/// <summary>
		/// 客户ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CUSTOMER_ID {get;set;}

		/// <summary>
		/// 客户名称
		/// </summary>
		[MaxLength(200)]
		public String CUSTOMER {get;set;}


	}
}
