/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsPn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:48
	/// 
	/// </summary>
	[Table("SFCS_PN")]
	public partial class SfcsPn
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(50)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(50)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 客户
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CUSTOMER_ID {get;set;}

		/// <summary>
		/// 产品系列
		/// </summary>
		[MaxLength(22)]
		public Decimal? FAMILY_ID {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MODEL_ID {get;set;}

		/// <summary>
		/// 客户料号
		/// </summary>
		[MaxLength(30)]
		public String CUSTOMER_PN {get;set;}

		/// <summary>
		/// 制造单位
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BU_CODE {get;set;}

		/// <summary>
		/// 厂部
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CLASSIFICATION {get;set;}

		/// <summary>
		/// 产品性质
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_KIND {get;set;}

		/// <summary>
		/// 生产阶段
		/// </summary>
		[MaxLength(22)]
		public Decimal? STAGE_CODE {get;set;}

		/// <summary>
		/// 是否双面板
		/// </summary>
		[MaxLength(1)]
		public String DOUBLE_SIDE {get;set;}

		/// <summary>
		/// 是否有铅
		/// </summary>
		[MaxLength(1)]
		public String LEAD_FLAG {get;set;}

		/// <summary>
		/// 是否出货
		/// </summary>
		[MaxLength(1)]
		public String SHIP_FLAG {get;set;}

		/// <summary>
		/// 是否EDI数据
		/// </summary>
		[MaxLength(1)]
		public String EDI_FLAG {get;set;}

		/// <summary>
		/// 保修期
		/// </summary>
		[MaxLength(22)]
		public Decimal? WARRANTY_LIMIT {get;set;}

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

		/// <summary>
		/// 是否自动存仓
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String TURNIN_TYPE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(300)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 类别
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CATEGORY {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(150)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE5 {get;set;}


	}
}
