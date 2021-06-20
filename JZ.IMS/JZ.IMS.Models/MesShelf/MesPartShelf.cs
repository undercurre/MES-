/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：条码与储位                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-03-05 13:05:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesPartShelf                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-03-05 13:05:38
	/// 条码与储位
	/// </summary>
	[Table("MES_PART_SHELF")]
	public partial class MesPartShelf
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 条码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CODE {get;set;}

		/// <summary>
		/// 储位(MES_PART_SHELF_CONFIG)
		/// </summary>
		[MaxLength(22)]
		public Decimal? SHELF_ID {get;set;}

		/// <summary>
		/// 创建时间j
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
		/// 更新人
		/// </summary>
		[MaxLength(50)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 状态(0:未存储1:存储中)
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 上架人
		/// </summary>
		[MaxLength(50)]
		public String PUT_SHELVES_USER {get;set;}

		/// <summary>
		/// 上架时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? PUT_SHELVES_TIME {get;set;}

		/// <summary>
		/// 下架人
		/// </summary>
		[MaxLength(50)]
		public String FALL_SHELVES_USER {get;set;}

		/// <summary>
		/// 下架时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? FALL_SHELVES_TIME {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QTY {get;set;}

		/// <summary>
		/// 储位
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String STORAGE {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO { get; set; }

		/// <summary>
		/// 型号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String DESCRIPTION { get; set; }
	}
}
