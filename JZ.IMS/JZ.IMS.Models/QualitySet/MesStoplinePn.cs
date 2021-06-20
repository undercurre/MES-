/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：停线管控产品表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-24 15:19:35                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesStoplinePn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-24 15:19:35
	/// 停线管控产品表
	/// </summary>
	[Table("MES_STOPLINE_PN")]
	public partial class MesStoplinePn
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
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 料号规格描述
		/// </summary>
		[MaxLength(100)]
		public String MODEL {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
