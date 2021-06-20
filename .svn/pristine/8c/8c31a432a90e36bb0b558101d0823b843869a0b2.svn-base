/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备巡检配置                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-23 16:18:50                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：IpqaConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-23 16:18:50
	/// 巡检配置
	/// </summary>
	[Table("IPQA_CONFIG")]
	public partial class IpqaConfig
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 分类(人,机,料,法,环)
		/// </summary>
		[MaxLength(50)]
		public String CATEGORY {get;set;}

		/// <summary>
		/// 巡检项目
		/// </summary>
		[MaxLength(50)]
		public String ITEM_NAME {get;set;}

		/// <summary>
		/// 点检类型(0:SMT车间巡检，1:产线车间巡检)
		/// </summary>
		[MaxLength(22)]
		public decimal IPQA_TYPE { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		[MaxLength(22)]
		public Decimal? ORDER_ID {get;set;}

		/// <summary>
		/// 品质工艺要求
		/// </summary>
		[MaxLength(500)]
		public String PROCESS_REQUIRE {get;set;}

		/// <summary>
		/// 参考标准
		/// </summary>
		[MaxLength(500)]
		public String REFERENCE_STANDARD {get;set;}

		/// <summary>
		/// 量化标准(0: 无量化标准,1:有量化标准)
		/// </summary>
		[MaxLength(22)]
		public Decimal? QUANTIZE_TYPE {get;set;}

		/// <summary>
		/// 标准值
		/// </summary>
		[MaxLength(22)]
		public Decimal? QUANTIZE_VAL { get; set; }

		/// <summary>
		/// 是否可用(0,1)
		/// </summary>
		[MaxLength(22)]
		public Decimal? ENABLED {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATETIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATOR {get;set;}


	}
}
