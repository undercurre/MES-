/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 列表显示实体                                                   
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
	///  列表显示实体
	/// </summary>
	public class SfcsRuncardRangerRulesListModel
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 客户
		/// </summary>
		public Decimal? CUSTOMER_ID {get;set;}

		/// <summary>
		/// 产品系列ID
		/// </summary>
		public Decimal? PRODUCT_FAMILY_ID {get;set;}

		/// <summary>
		/// 产品系列名称
		/// </summary>
		public String FAMILY_NAME { get; set; }

		public Decimal? PLATFORM_ID {get;set;}

		/// <summary>
		/// 模块
		/// </summary>
		public Decimal? MODEL_ID {get;set;}

		/// <summary>
		/// 成品料号
		/// </summary>
		public String PART_NO {get;set;}

		/// <summary>
		/// 销售订单
		/// </summary>
		public String SALES_ORDER { get; set; }

		/// <summary>
		/// 工单号
		/// </summary>
		public String WO_NO { get; set; }

		/// <summary>
		/// 前导符
		/// </summary>
		public String FIX_HEADER {get;set;}

		/// <summary>
		/// 结束符
		/// </summary>
		public String FIX_TAIL {get;set;}

		/// <summary>
		/// 流水范围长度
		/// </summary>
		public Decimal? RANGE_LENGTH {get;set;}

		/// <summary>
		/// 流水范围开始字符
		/// </summary>
		public String RANGE_START_CODE {get;set;}

		/// <summary>
		/// 进制
		/// </summary>
		public Decimal? DIGITAL {get;set;}

		/// <summary>
		/// 不包括字符
		/// </summary>
		public String EXCLUSIVE_CHAR {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED {get;set;}

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
