/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具保养明细表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-25 10:30:28                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsMaintainDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-25 10:30:28
	/// 夹具保养明细表
	/// </summary>
	[Table("MES_TONGS_MAINTAIN_DETAIL")]
	public partial class MesTongsMaintainDetail
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 主表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MST_ID { get; set; }

		/// <summary>
		/// 保养事项ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ITEM_ID { get; set; }

		/// <summary>
		/// 事项名称
		/// </summary>
		public string ITEM_NAME { get; set; }

		/// <summary>
		/// 事项描述
		/// </summary>
		public string ITEM_DESC { get; set; }

		/// <summary>
		/// 保养状态，1：正常，2：异常
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ITEM_STATUS { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String REMARK { get; set; }
	}
}
