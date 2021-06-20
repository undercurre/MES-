/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检记录详细表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-03-03 14:00:25                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesPartCheckHeader                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-03-03 14:00:25
	/// 设备点检记录详细表
	/// </summary>
	[Table("MES_PART_CHECK_HEADER")]
	public partial class MesPartCheckHeader
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(30)]
		public String MESSAGE {get;set;}

		/// <summary>
		/// 0:未核对 1:已核对
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(100)]
		public String MARK {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(5)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIMTE {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		[MaxLength(5)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 核对数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QTY {get;set;}


	}
}
