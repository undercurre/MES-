/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：中转码                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-12 14:25:16                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesMiddleCode                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-12 14:25:16
	/// 中转码
	/// </summary>
	[Table("MES_MIDDLE_CODE")]
	public partial class MesMiddleCode
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 中转条码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CODE {get;set;}

		/// <summary>
		/// 创建日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CHREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 状态（0：待用；1：在 用；2:报废）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String STATUS {get;set;}


	}
}
