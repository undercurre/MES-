/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-10 19:51:17                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsPrintFiles                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-10 19:51:17
	///  查询实体
	/// </summary>
	public class SfcsPrintFilesRequestModel : PageModel
	{
		/// <summary>
		/// 文件名
		/// </summary>
		public String FILE_NAME { get; set; }

		/// <summary>
		/// 标签类型
		/// </summary>
		public Decimal? LABEL_TYPE { get; set; }

		public SfcsPrintFilesRequestModel()
		{
			Page = 1;
			Limit = 50;
		}
	}
}
