/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-13 16:59:09                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtStencilMaintainHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-13 16:59:09
	/// 
	/// </summary>
	[Table("SMT_STENCIL_MAINTAIN_HISTORY")]
	public partial class SmtStencilMaintainHistory
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ID {get;set;}

		/// <summary>
		/// 钢网ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? STENCIL_ID {get;set;}

		/// <summary>
		/// 使用次数
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRINT_COUNT {get;set;}

		/// <summary>
		/// 过板数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRODUCT_PASS_COUNT {get;set;}

		/// <summary>
		/// 当前状态
		/// </summary>
		[MaxLength(22)]
		public Decimal? CURRENT_STATUS {get;set;}

		/// <summary>
		/// 维护后状态
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAINTAIN_STATUS {get;set;}

		/// <summary>
		/// 操作时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? OPERATION_TIME {get;set;}

		/// <summary>
		/// 维护人
		/// </summary>
		[MaxLength(20)]
		public String OPERATOR {get;set;}

		/// <summary>
		/// 维护保养备注
		/// </summary>
		[MaxLength(500)]
		public String MAINTAIN_DESCRIPTION {get;set;}

	}
}
