/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：拼扳子SN记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-25 10:27:56                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsWoRgMultiDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-25 10:27:56
	/// 拼扳子SN记录表
	/// </summary>
	[Table("SFCS_WO_RG_MULTI_DETAIL")]
	public partial class SfcsWoRgMultiDetail
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 主表ID SFCS_WO_RG_MULTI_HEADER.ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MST_ID {get;set;}

		/// <summary>
		/// 所有拼板SN
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
