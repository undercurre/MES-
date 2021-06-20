/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-06 14:36:40                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductPallet                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-06 14:36:40
	///  查询实体
	/// </summary>
	public class SfcsProductPalletRequestModel : PageModel
	{

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO { get; set; }

		/// <summary>
		/// 格式限定
		/// </summary>
		public String FORMAT { get; set; }

		/// <summary>
		/// 采集工序
		/// </summary>
		public Decimal? COLLECT_OPERATION_ID { get; set; }
		public SfcsProductPalletRequestModel()
		{
			Page = 1;
			Limit = 50;
		}
	}
}
