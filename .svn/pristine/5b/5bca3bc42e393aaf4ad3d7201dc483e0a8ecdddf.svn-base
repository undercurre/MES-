/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具产品对照表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-20 17:51:19                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsPart                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-20 17:51:19
	/// 夹具产品对照表
	/// </summary>
	[Table("MES_TONGS_PART")]
	public partial class MesTongsPart
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 夹具ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_ID { get; set; }

		/// <summary>
		/// 产品编码
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO { get; set; }

		/// <summary>
		/// 产品品名
		/// </summary>
		[MaxLength(50)]
		public String PART_NAME { get; set; }

		/// <summary>
		/// 型号（产品规格）
		/// </summary>
		[MaxLength(200)]
		public String PART_DESC { get; set; }

		/// <summary>
		/// 版本号
		/// </summary>
		[MaxLength(50)]
		public String VERSION { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_DATE { get; set; }

		/// <summary>
		/// 是否有效（Y:有效，N：无效）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ENABLED { get; set; }
	}
}
