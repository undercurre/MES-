/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：巡检主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 13:51:00                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：IpqaMst                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-28 13:51:00
	/// 巡检主表
	/// </summary>
	[Table("IPQA_MST")]
	public partial class IpqaMst
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 单据编号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String BILL_CODE {get;set;}

		/// <summary>
		/// 巡检分类(0:SMT车间巡检，1:产线车间巡检)
		/// </summary>
		[Required]
		[MaxLength(22)]
		public decimal IPQA_TYPE { get; set; }

		/// <summary>
		/// 经营单位ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public Decimal BUSINESS_UNIT_ID {get;set;}

		/// <summary>
		/// 部门ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public Decimal DEPARTMENT_ID {get;set;}

		/// <summary>
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(30)]
		public Decimal U_LINE_ID { get; set; }

		/// <summary>
		/// 生产单号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PRODUCT_BILLNO { get; set; }

		/// <summary>
		/// 产品名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PRODUCT_NAME {get;set;}

		/// <summary>
		/// 产品型号
		/// </summary> 
		[Required]
		[MaxLength(50)]
		public String PRODUCT_MODEL {get;set;}

		/// <summary>
		/// 生产日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime PRODUCT_DATE {get;set;}

		/// <summary>
		/// 生产数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_QTY {get;set;}

		/// <summary>
		/// 创建日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATEDATE {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 巡检时间(时分)
		/// </summary>
		[Required]
		[MaxLength(7)]
		public String CREATETIME {get;set;}

		/// <summary>
		/// 巡检状态
		/// </summary>
		[Required]
		[MaxLength(22)]
		public decimal CHECK_STATUS { get; set; }

		/// <summary>
		/// 审核日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CHECK_TIME { get; set; }

		/// <summary>
		/// 审核人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CHECKER { get; set; }
	}
}
