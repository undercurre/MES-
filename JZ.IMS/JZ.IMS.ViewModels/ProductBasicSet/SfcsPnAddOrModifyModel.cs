/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
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
	///  更新或者新增实体
	/// </summary>
	public class SfcsPnAddOrModifyModel
	{
		/// <summary>
		/// 主键
		/// </summary>
		public Decimal ID {get;set;}

		public Decimal? VERSION {get;set;}

		public String ENABLE_BILL_ID {get;set;}

		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO {get;set;}

		/// <summary>
		/// 客户
		/// </summary>
		public Decimal CUSTOMER_ID {get;set;}

		/// <summary>
		/// 产品系列
		/// </summary>
		public Decimal? FAMILY_ID {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		public Decimal MODEL_ID {get;set;}

		/// <summary>
		/// 客户料号
		/// </summary>
		public String CUSTOMER_PN {get;set;}

		/// <summary>
		/// 制造单位
		/// </summary>
		public Decimal BU_CODE {get;set;}

		/// <summary>
		/// 厂部
		/// </summary>
		public Decimal CLASSIFICATION {get;set;}

		/// <summary>
		/// 产品性质
		/// </summary>
		public Decimal PRODUCT_KIND {get;set;}

		/// <summary>
		/// 生产阶段
		/// </summary>
		public Decimal? STAGE_CODE {get;set;}

		/// <summary>
		/// 是否双面板
		/// </summary>
		public String DOUBLE_SIDE {get;set;}

		/// <summary>
		/// 是否有铅
		/// </summary>
		public String LEAD_FLAG {get;set;}

		/// <summary>
		/// 是否出货
		/// </summary>
		public String SHIP_FLAG {get;set;}

		/// <summary>
		/// 是否EDI数据
		/// </summary>
		public String EDI_FLAG {get;set;}

		/// <summary>
		/// 保修期
		/// </summary>
		public Decimal? WARRANTY_LIMIT {get;set;}

		/// <summary>
		/// 投入日期
		/// </summary>
		public DateTime? PHASE_IN_DATE {get;set;}

		/// <summary>
		/// 结束日期
		/// </summary>
		public DateTime? PHASE_OUT_DATE {get;set;}

		/// <summary>
		/// 是否自动存仓
		/// </summary>
		public String TURNIN_TYPE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 类别
		/// </summary>
		public Decimal CATEGORY {get;set;}

		public String ATTRIBUTE2 {get;set;}

		public String ATTRIBUTE3 {get;set;}

		public String ATTRIBUTE4 {get;set;}

		public String ATTRIBUTE5 {get;set;}


	}
}
