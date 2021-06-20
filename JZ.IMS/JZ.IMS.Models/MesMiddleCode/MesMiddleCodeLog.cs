/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：中转码使用日志                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-12 14:25:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesMiddleCodeLog                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-12 14:25:48
	/// 中转码使用日志
	/// </summary>
	[Table("MES_MIDDLE_CODE_LOG")]
	public partial class MesMiddleCodeLog
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 中转条码ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CODE_ID {get;set;}

		/// <summary>
		/// 中转条码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CODE {get;set;}

		/// <summary>
		/// 产品流水号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SN {get;set;}

		/// <summary>
		/// 采集数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal COLLECT_QTY {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 连接站点ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINK_SITE_ID {get;set;}

		/// <summary>
		/// 连接时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime LINK_TIME {get;set;}

		/// <summary>
		/// 连接用户
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String LINK_USER {get;set;}

		/// <summary>
		/// 解邦站点
		/// </summary>
		[MaxLength(22)]
		public Decimal? BREAK_SITE_ID {get;set;}

		/// <summary>
		/// 解邦时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? BREAK_TIME {get;set;}

		/// <summary>
		/// 解邦用户
		/// </summary>
		[MaxLength(50)]
		public String BREAK_USER {get;set;}

		/// <summary>
		/// 状态：（0：未解邦；1: 已解邦）
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String STATUS {get;set;}


	}
}
