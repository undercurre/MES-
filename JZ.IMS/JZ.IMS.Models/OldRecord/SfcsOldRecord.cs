/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：老化记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-03-19 11:27:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsOldRecord                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-03-19 11:27:38
	/// 老化记录表
	/// </summary>
	[Table("SFCS_OLD_RECORD")]
	public partial class SfcsOldRecord
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 流水号ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SN_ID {get;set;}

		/// <summary>
		/// 流水号
		/// </summary>
		[MaxLength(50)]
		public String SN {get;set;}

		/// <summary>
		/// 站点ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SITE_ID {get;set;}

		/// <summary>
		/// 开始时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? BEGIN_TIME {get;set;}

		/// <summary>
		/// 开始人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String BEGING_CREATOR {get;set;}

		/// <summary>
		/// 结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? END_TIME {get;set;}

		/// <summary>
		/// 结束人
		/// </summary>
		[MaxLength(10)]
		public String END_CREATOR {get;set;}

		/// <summary>
		/// 是否结束(结束Y;开始N;)
		/// </summary>
		[MaxLength(1)]
		public String STAUTS {get;set;}


	}
}
