/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-14 16:33:04                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MstBom2DetailQty                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-14 16:33:04
	/// 
	/// </summary>
	[Table("MST_BOM2_DETAIL_QTY")]
	public partial class MstBom2DetailQty
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 成品料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 物料料号
		/// </summary>
		[MaxLength(40)]
		public String PART_CODE {get;set;}

		/// <summary>
		/// 欠料数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? SHORTTAGE_QTY {get;set;}

		/// <summary>
		/// 已发数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? SENT_QTY {get;set;}


	}
}
