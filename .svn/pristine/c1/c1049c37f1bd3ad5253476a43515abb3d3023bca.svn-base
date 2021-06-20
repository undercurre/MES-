/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：连扳主SN记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-25 10:27:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsWoRgMultiHeader                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-25 10:27:25
	/// 连扳主SN记录表
	/// </summary>
	[Table("SFCS_WO_RG_MULTI_HEADER")]
	public partial class SfcsWoRgMultiHeader
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 流水号范围ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_RANGER_ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 0:标识连扳;1：标识拆板；默认值0
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 拼板数
		/// </summary>
		[MaxLength(100)]
		public Decimal SPLICING_QTY {get;set;}

		/// <summary>
		/// 主码SN
		/// </summary>
		[MaxLength(100)]
		public String SN {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(255)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(255)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}


	}
}
