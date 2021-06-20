/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:47                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductFamily                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:47
	///  查询实体
	/// </summary>
	public class SfcsProductFamilyRequestModel : PageModel
	{
		/// <summary>
		/// 客户
		/// </summary>
		public Decimal? CUSTOMER_ID { get; set; }

		/// <summary>
		/// 产品系列
		/// </summary>
		public String FAMILY_NAME { get; set; } = null;

		/// <summary>
		/// 描述
		/// </summary>
		public String DESCRIPTION { get; set; } = null;

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED { get; set; } = null;

		/// <summary>
		/// 投入日期
		/// </summary>
		public string PHASE_IN_DATE { get; set; } = null;

		/// <summary>
		/// 结束日期
		/// </summary>
		public string PHASE_OUT_DATE { get; set; } = null;

		/// <summary>
		/// Key对应的是客户名字
		/// </summary>
		public string Key { get; set; } = null;

		/// <summary>
		/// 模糊匹配
		/// </summary>
		public Decimal IS_LIKE { get; set; } = 1;

		public SfcsProductFamilyRequestModel() {
			Page = 1;
			Limit = 50;
		}
	}
}
