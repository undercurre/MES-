/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：手插件备料记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-25 17:16:35                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesHiReel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-25 17:16:35
	/// 手插件备料记录表
	/// </summary>
	[Table("MES_HI_REEL")]
	public partial class MesHiReel
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 物料ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String REEL_ID {get;set;}

		/// <summary>
		/// 线别
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String OPERATION_LINE_ID {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QTY {get;set;}

		/// <summary>
		/// 操作人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String OPERTOR {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 使用数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? USED_QTY {get;set;}

		/// <summary>
		/// 原本数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? ORG_QTY {get;set;}

		/// <summary>
		/// 状态（0：待用，1：在用，2：用完，3：空闲，4：挪料）
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		/// <summary>
		/// MES_HI_MATERIAL_LISTEN.BATCH_NO
		/// </summary>
		[MaxLength(50)]
		public String BATCH_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 工序ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERAITON_ID {get;set;}

		/// <summary>
		/// 站点ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_SITE_ID {get;set;}

		/// <summary>
		/// 插件人员
		/// </summary>
		[MaxLength(50)]
		public String MES_USER {get;set;}


	}
}
