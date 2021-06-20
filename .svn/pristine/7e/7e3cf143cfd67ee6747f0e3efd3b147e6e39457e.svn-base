/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-06 14:36:26                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductResources                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-06 14:36:26
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_RESOURCES")]
	public partial class SfcsProductResources
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 资源名称
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal RESOURCE_ID {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		[MaxLength(200)]
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 固定值
		/// </summary>
		[MaxLength(100)]
		public String FIXED_VALUE {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? RESOURCE_QTY {get;set;}

		[MaxLength(1)]
		public String BINDING_SITE {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		[MaxLength(1)]
		public String REPEATED {get;set;}

		/// <summary>
		/// 返工是否自动清除
		/// </summary>
		[MaxLength(1)]
		public String REWORK_REMOVE_FLAG {get;set;}

		/// <summary>
		/// 是否EDI
		/// </summary>
		[MaxLength(1)]
		public String EDI_FLAG {get;set;}

		/// <summary>
		/// 采集工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? COLLECT_OPERATION_ID {get;set;}


	}
}
