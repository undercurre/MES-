/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:46                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsParameters                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:46
	///  查询实体
	/// </summary>
	public class SfcsParametersRequestModel : PageModel
	{
		/// <summary>
		/// 类型(EN)
		/// </summary>
		public String LOOKUP_TYPE { get; set; }

		/// <summary>
		/// 类型(CN)
		/// </summary>
		public String NAME { get; set; }

		/// <summary>
		/// 代码
		/// </summary>
		public Decimal LOOKUP_CODE { get; set; }

		/// <summary>
		/// 键值
		/// </summary>
		public String MEANING { get; set; }

		/// <summary>
		/// 描述(EN)
		/// </summary>
		public String DESCRIPTION { get; set; }

		/// <summary>
		/// 描述(CN)
		/// </summary>
		public String CHINESE { get; set; }

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED { get; set; }


		public SfcsParametersRequestModel()
		{
			Page = 1;
			Limit = 50;
		}
	}
}
