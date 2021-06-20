/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 10:41:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsPrintFilesMapping                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-14 10:41:48
	///  查询实体
	/// </summary>
	public class SfcsPrintFilesMappingRequestModel : PageModel
	{
		/// <summary>
		/// 客户
		/// </summary>
		public Decimal? CUSTOMER_ID { get; set; }
		/// <summary>
		/// 机种
		/// </summary>
		public Decimal? MODEL_ID { get; set; }
		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO { get; set; }
		/// <summary>
		/// 文件名
		/// </summary>
		public Decimal? PRINT_FILE_ID { get; set; }
		/// <summary>
		/// 输入文件名
		/// </summary>
		public String FILE_NAME { get; set; }
		/// <summary>
		/// 打印工序
		/// </summary>
		public Decimal? SITE_OPERATION_ID { get; set; }

		/// <summary>
		/// 产品系列id
		/// </summary>
		public int PRODUCT_FAMILY_ID { get; set; } = -1;


		public SfcsPrintFilesMappingRequestModel()
		{
			Page = 1;
			Limit = 50;
		}
	}
}
