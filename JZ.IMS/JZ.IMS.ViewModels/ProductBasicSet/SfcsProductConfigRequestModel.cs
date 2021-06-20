/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductConfig                                     
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
	public class SfcsProductConfigRequestModel : PageModel
	{

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO { get; set; } = null;

		/// <summary>
		/// 配置类型
		/// </summary>
		public Decimal? CONFIG_TYPE { get; set; }

		/// <summary>
		/// 配置值
		/// </summary>
		public String CONFIG_VALUE { get; set; } = null;

		/// <summary>
		/// 描述
		/// </summary>
		public String DESCRIPTION { get; set; } = null;

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED { get; set; } = null;

		/// <summary>
		/// 配置类型里的名称
		/// </summary>
		public string ChineseName { get; set; } = null;
		public SfcsProductConfigRequestModel()
		{
			Page = 1;
			Limit = 50;
		}
	}


}
