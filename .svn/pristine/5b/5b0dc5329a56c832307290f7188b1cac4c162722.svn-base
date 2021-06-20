/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 15:59:15                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductComponents                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-03 15:59:15
	///  查询实体
	/// </summary>
	public class SfcsProductComponentsRequestModel : PageModel
	{
		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO { get; set; }

		/// <summary>
		/// 零件ID
		/// </summary>
		public Decimal? COMPONENT_ID { get; set; }

		/// <summary>
		/// 本厂料号(零件料号)
		/// </summary>
		public String ODM_COMPONENT_PN { get; set; }

		/// <summary>
		/// 客户零件料号
		/// </summary>
		public String CUSTOMER_COMPONENT_PN { get; set; }

		/// <summary>
		/// 格式限定
		/// </summary>
		public String DATA_FORMAT { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		public Decimal COMPONENT_QTY { get; set; }

		/// <summary>
		/// 采集工序ID
		/// </summary>
		public Decimal? COLLECT_OPERATION_ID { get; set; }


		public SfcsProductComponentsRequestModel() {
			Page = 1;
			Limit = 50;
		}
	}
}
