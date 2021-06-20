/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 15:59:15                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductComponents                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-03 15:59:15
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_COMPONENTS")]
	public partial class SfcsProductComponents
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
		/// 零件ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal COMPONENT_ID {get;set;}

		/// <summary>
		/// 本厂料号(零件料号)
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String ODM_COMPONENT_PN {get;set;}

		/// <summary>
		/// 客户零件料号
		/// </summary>
		[MaxLength(30)]
		public String CUSTOMER_COMPONENT_PN {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		[MaxLength(300)]
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal COMPONENT_QTY {get;set;}

		/// <summary>
		/// 是否唯一序列
		/// </summary>
		[MaxLength(1)]
		public String SERIALIZED {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

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
		/// 是否检查不良
		/// </summary>
		[MaxLength(10)]
		public String CHECK_DEFECT_FLAG {get;set;}

		[MaxLength(2000)]
		public String COMPONENT_LOCATIONS {get;set;}

		/// <summary>
		/// 采集工序ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? COLLECT_OPERATION_ID {get;set;}

		/// <summary>
		/// 是否生成SN.BOM
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String ATTRIBUTE1 { get; set; }
		/// <summary>
		/// 零件描述
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String ATTRIBUTE2 { get; set; }

	}
}
