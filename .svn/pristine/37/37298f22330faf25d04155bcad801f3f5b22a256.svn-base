/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-24 14:19:22                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesStoplineCall                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-24 14:19:22
	/// 
	/// </summary>
	[Table("MES_STOPLINE_CALL")]
	public partial class MesStoplineCall
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 停线管控配置表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_STOPLINE_ID {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 异常内容配置表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CALL_TONTENT_ID {get;set;}


	}


	 
}
