/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-18 15:38:19                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsCollectDefectsDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-18 15:38:19
	/// 
	/// </summary>
	[Table("SFCS_COLLECT_DEFECTS_DETAIL")]
	public partial class SfcsCollectDefectsDetail
	{
		[Key]
		[MaxLength(22)]
		public Decimal? COLLECT_DEFECT_DETAIL_ID {get;set;}

		[MaxLength(22)]
		public Decimal? OPERATION_ID {get;set;}

		/// <summary>
		/// 序号
		/// </summary>
		[MaxLength(22)]
		public Decimal? ORDER_NO {get;set;}

		/// <summary>
		/// 不良信息
		/// </summary>
		[MaxLength(3000)]
		public String DEFECT_DETAIL {get;set;}

		[MaxLength(22)]
		public Decimal? WO_ID {get;set;}

		[MaxLength(22)]
		public Decimal? SN_ID {get;set;}

		[MaxLength(100)]
		public String SN {get;set;}

		[MaxLength(22)]
		public Decimal? OPERATION_SITE_ID {get;set;}

		[MaxLength(20)]
		public String OPERATOR {get;set;}

		[MaxLength(7)]
		public DateTime? COLLECT_TIME {get;set;}


	}
}
