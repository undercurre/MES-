/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具保养事项表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-20 15:51:29                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsMaintainItems                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-20 15:51:29
	/// 夹具保养事项表
	/// </summary>
	[Table("MES_TONGS_MAINTAIN_ITEMS")]
	public partial class MesTongsMaintainItems
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 事项类型（0：保养事项，1：确定事项）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ITEM_TYPE {get;set;}

		/// <summary>
		/// 夹具类别（-1：公用，0：工装，1：ICT针床，2:FCT针床）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_TYPE { get; set; }

		/// <summary>
		/// 事项名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ITEM_NAME {get;set;}

		/// <summary>
		/// 事项描述
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_DATE {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(30)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_DATE {get;set;}

		/// <summary>
		/// 是否有效（Y:有效，N：无效）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 是否合格（Y:合格，N：不合格）
		/// </summary>
		[NotMapped]
		public String ACTIVE { get; set; }
	}
}
