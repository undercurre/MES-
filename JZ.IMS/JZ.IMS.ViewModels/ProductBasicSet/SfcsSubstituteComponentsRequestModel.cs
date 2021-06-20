/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 10:34:52                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsSubstituteComponents                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-03 10:34:52
	///  查询实体
	/// </summary>
	public class SfcsSubstituteComponentsRequestModel : PageModel
	{

		/// <summary>
		/// 零件组件ID（PRODUCT_COMPONENT_ID）
		/// </summary>
		public string PRODUCT_COMPONENT_ID { get; set; }
		public SfcsSubstituteComponentsRequestModel()
		{
			Page = 1;
			Limit = 50;
		}
	}
}
