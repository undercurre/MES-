/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-10 18:09:05                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtStencilStore                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-10 18:09:05
	/// 
	/// </summary>
	[Table("SMT_STENCIL_STORE")]
	public partial class SmtStencilStore
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 钢网ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? STENCIL_ID {get;set;}

		/// <summary>
		/// 储位
		/// </summary>
		[MaxLength(50)]
		public String LOCATION {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		/// <summary>
		/// /// <summary>
		/// 制造日期
		/// </summary>
		/// </summary>
		[MaxLength(7)]
		public DateTime? MANUFACTURE_TIME {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}


	}
}
