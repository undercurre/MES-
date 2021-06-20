/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-15 13:36:00                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsProductFamily                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-01-15 13:36:00
	/// 
	/// </summary>
	[Table("MES_TONGS_PRODUCT_FAMILY")]
	public partial class MesTongsProductFamily
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 夹具ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_ID {get;set;}

		/// <summary>
		/// SFCS_RPODUCT_FAMILY 的ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_FAMILY_ID {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(50)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 是否有效（Y:有效，N：无效）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
