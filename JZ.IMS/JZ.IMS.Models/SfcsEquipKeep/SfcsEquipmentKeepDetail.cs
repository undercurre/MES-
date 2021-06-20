/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：飞达点检记录详细内容表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-21 13:57:11                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipmentKeepDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-01-21 13:57:11
	/// 飞达点检记录详细内容表
	/// </summary>
	[Table("SFCS_EQUIPMENT_KEEP_DETAIL")]
	public partial class SfcsEquipmentKeepDetail
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// MES_TONGS_KEEP_HEAD表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal KEEP_HEAD_ID {get;set;}

		/// <summary>
		/// 工装ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_ID {get;set;}

		/// <summary>
		/// 工装盘点状态(0:未盘点,1:已盘点)
		/// </summary>
		[MaxLength(22)]
		public Decimal? TONGS_STATUS {get;set;}

		/// <summary>
		/// 点检备注
		/// </summary>
		[MaxLength(1000)]
		public String CHECK_REMARKS {get;set;}

		/// <summary>
		/// 点检时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CHECK_TIME {get;set;}

		/// <summary>
		/// 点检人员
		/// </summary>
		[MaxLength(255)]
		public String CHECK_USER {get;set;}


	}
}
