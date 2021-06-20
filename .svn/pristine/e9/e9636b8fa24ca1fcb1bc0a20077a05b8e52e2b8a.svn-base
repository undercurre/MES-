/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:47                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductFamily                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:47
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_FAMILY")]
	public partial class SfcsProductFamily
	{
		/// <summary>
		///  主键
		/// </summary>
		[MaxLength(22)]
		public Decimal? ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(50)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(50)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 客户
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CUSTOMER_ID {get;set;}

		/// <summary>
		/// 产品系列
		/// </summary>
		[Required]
		[MaxLength(80)]
		public String FAMILY_NAME {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(100)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 投入日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? PHASE_IN_DATE {get;set;}

		/// <summary>
		/// 结束日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? PHASE_OUT_DATE {get;set;}

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


	}
}
