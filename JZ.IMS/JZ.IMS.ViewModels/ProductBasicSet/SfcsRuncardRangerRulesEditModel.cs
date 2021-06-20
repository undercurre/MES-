/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-11 10:06:08                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsRuncardRangerRules                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-11 10:06:08
	///  编辑数据实体
	/// </summary>
	public class SfcsRuncardRangerRulesEditModel
	{
		/// <summary>
		/// 流水范围类型(0: 产品系列, 1: 客户, 2: 成品料号)
		/// </summary>
		public int RangerRuleType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Decimal? PLATFORM_ID { get; set; }

		/// <summary>
		/// 客户ID
		/// </summary>
		public Decimal? CUSTOMER_ID { get; set; }

		/// <summary>
		/// 成品料号
		/// </summary>
		public String PART_NO { get; set; }

		/// <summary>
		/// 销售订单
		/// </summary>
		public String SALES_ORDER { get; set; }

		/// <summary>
		/// 产品系列id
		/// </summary>
		public int PRODUCT_FAMILY_ID { get; set; } = 0;

		/// <summary>
		/// 规则类型 0: 流水号规则 1: 箱号规则 2: 栈板号规则
		/// </summary>
		public int RULE_TYPE { get; set; } = 0;

		/// <summary>
		/// 排序(1.递增--默认 2.递减)
		/// </summary>
		public int SORT_TYPE { get; set; } = 1;

	}
}
