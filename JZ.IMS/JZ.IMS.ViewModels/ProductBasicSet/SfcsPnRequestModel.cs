/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsPn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:48
	///  查询实体
	/// </summary>
	public class SfcsPnRequestModel : PageModel
	{
		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO { get; set; }

		/// <summary>
		/// 客户
		/// </summary>
		public Decimal CUSTOMER_ID { get; set; }

		/// <summary>
		/// 产品系列
		/// </summary>
		public Decimal? FAMILY_ID { get; set; }

		/// <summary>
		/// 机种
		/// </summary>
		public Decimal MODEL_ID { get; set; }

		/// <summary>
		/// 客户料号
		/// </summary>
		public String CUSTOMER_PN { get; set; }

		/// <summary>
		/// 制造单位
		/// </summary>
		public Decimal BU_CODE { get; set; }

		/// <summary>
		/// 厂部
		/// </summary>
		public Decimal CLASSIFICATION { get; set; }

		/// <summary>
		/// 产品性质
		/// </summary>
		public Decimal PRODUCT_KIND { get; set; }

		/// <summary>
		/// 生产阶段
		/// </summary>
		public Decimal? STAGE_CODE { get; set; }

		/// <summary>
		/// 是否双面板
		/// </summary>
		public String DOUBLE_SIDE { get; set; }

		public SfcsPnRequestModel()
		{
			Page = 1;
			Limit = 50;
		}

	}
}
