/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：下架记录                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-03-05 15:37:01                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesPartShelfRecord                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-03-05 15:37:01
	/// 下架记录
	/// </summary>
	[Table("MES_PART_SHELF_RECORD")]
	public partial class MesPartShelfRecord
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 条码
		/// </summary>
		[MaxLength(30)]
		public String CODE {get;set;}

		/// <summary>
		/// 储位
		/// </summary>
		[MaxLength(10)]
		public String STORAGE {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? QTY {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(10)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 类型(1:下架,2:切分)
		/// </summary>
		[MaxLength(1)]
		public String TYPE {get;set;}

		/// <summary>
		/// 主表ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? HID {get;set;}


	}
}
