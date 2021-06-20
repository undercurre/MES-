/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 10:34:52                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsSubstituteComponents                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-03 10:34:52
	/// 
	/// </summary>
	[Table("SFCS_SUBSTITUTE_COMPONENTS")]
	public partial class SfcsSubstituteComponents
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 产品零件
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_COMPONENT_ID {get;set;}

		/// <summary>
		/// 替代料本厂料号
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String SUBSTITUTE_COMP_PN {get;set;}

		/// <summary>
		/// 替代料客户料号
		/// </summary>
		[Required]
		[MaxLength(80)]
		public String CUSTOMER_COMPONENT_PN {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		[Required]
		[MaxLength(300)]
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal COMPONENT_QTY {get;set;}

		/// <summary>
		/// 开始日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? BEGIN_DATE {get;set;}

		/// <summary>
		/// 结束日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? END_DATE {get;set;}

		/// <summary>
		/// 是否唯一序列
		/// </summary>
		[MaxLength(1)]
		public String SERIALIZED {get;set;}

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
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 是否测试零件
		/// </summary>
		[MaxLength(1)]
		public String DEVICE_FLAG {get;set;}

		/// <summary>
		/// 测试零件标准使用次数
		/// </summary>
		[MaxLength(22)]
		public Decimal? STANDARD_USE_COUNT {get;set;}


		/// <summary>
		/// 零件描述
		/// </summary>

		public String ATTRIBUTE2 { get; set; }

	}
}
