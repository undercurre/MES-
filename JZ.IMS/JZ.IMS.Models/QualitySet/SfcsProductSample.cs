/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-16 13:37:10                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductSample                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-16 13:37:10
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_SAMPLE")]
	public partial class SfcsProductSample
	{
		/// <summary>
		/// 主键
		/// </summary>
		[MaxLength(22)]
		public Decimal? ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 抽检模式 SFCS_PARAMETERS SAMPLE_MODE
		/// </summary>
		[MaxLength(22)]
		public Decimal? SAMPLE_MODE {get;set;}

		/// <summary>
		/// 抽检方案
		/// </summary>
		[MaxLength(22)]
		public Decimal? PROJECT_ID {get;set;}

		/// <summary>
		/// 标记工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? DELIVER_OPERATION_CODE {get;set;}

		/// <summary>
		/// 抽检工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? SAMPLE_OPERATION_CODE {get;set;}

		/// <summary>
		/// 当前抽检比例
		/// </summary>
		[MaxLength(22)]
		public Decimal? CURRENT_SAMPLE_RATIO {get;set;}

		/// <summary>
		/// 连续抽检工序个数
		/// </summary>
		[MaxLength(22)]
		public Decimal? SAMPLE_OPERATION_COUNT {get;set;}

		[MaxLength(22)]
		public Decimal? DELIVER_COUNT {get;set;}

		[MaxLength(22)]
		public Decimal? SAMPLE_PASS_COUNT {get;set;}

		[MaxLength(22)]
		public Decimal? SAMPLE_FAIL_COUNT {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 制程内Fail重流必检
		/// </summary>
		[MaxLength(30)]
		public String MUST_SIGN_WITH_FAIL {get;set;}


	}
}
