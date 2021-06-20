/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-11 10:06:08                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsRuncardRangerRules                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-11 10:06:08
	/// 
	/// </summary>
	[Table("SFCS_RUNCARD_RANGER_RULES")]
	public partial class SfcsRuncardRangerRules
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 客户
		/// </summary>
		[MaxLength(22)]
		public Decimal? CUSTOMER_ID {get;set;}

		/// <summary>
		/// 产品系列ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRODUCT_FAMILY_ID {get;set;}

		[MaxLength(22)]
		public Decimal? PLATFORM_ID {get;set;}

		/// <summary>
		/// 模块
		/// </summary>
		[MaxLength(22)]
		public Decimal? MODEL_ID {get;set;}

		/// <summary>
		/// 成品料号
		/// </summary>
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 销售订单
		/// </summary>
		[MaxLength(50)]
		public String SALES_ORDER { get; set; }

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String WO_NO { get; set; }

		/// <summary>
		/// 前导符
		/// </summary>
		[MaxLength(50)]
		public String FIX_HEADER {get;set;}

		/// <summary>
		/// 结束符
		/// </summary>
		[MaxLength(50)]
		public String FIX_TAIL {get;set;}

		/// <summary>
		/// 流水范围长度
		/// </summary>
		[MaxLength(22)]
		public Decimal? RANGE_LENGTH {get;set;}

		/// <summary>
		/// 流水范围开始字符
		/// </summary>
		[MaxLength(100)]
		public String RANGE_START_CODE {get;set;}

		/// <summary>
		/// 进制
		/// </summary>
		[MaxLength(22)]
		public Decimal? DIGITAL {get;set;}

		/// <summary>
		/// 不包括字符
		/// </summary>
		[MaxLength(50)]
		public String EXCLUSIVE_CHAR {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set; }

		/// <summary>
		/// 规则类型 0: 流水号规则 1: 箱号规则 2: 栈板号规则
		/// </summary>
		[MaxLength(22)]
		public int RULE_TYPE { get; set; } = 0;

		/// <summary>
		/// 排序类型:1:递增(默认) 2.是递减
		/// </summary>
		[MaxLength(3)]
		public int SORT_TYPE { get; set; } = 1;

	}
}
