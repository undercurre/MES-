/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：停线管控线别表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-24 11:51:57                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesStoplineLines                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-24 11:51:57
	/// 停线管控线别表
	/// </summary>
	[Table("MES_STOPLINE_LINES")]
	public partial class MesStoplineLines
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
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 线别类型
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String LINE_TYPE {get;set;}

		/// <summary>
		/// 线别名称
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String LINE_NAME {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORGANIZE_ID {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
