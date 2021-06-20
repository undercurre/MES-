/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：手插件物料状态监听表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-19 20:26:22                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesHiMaterialListen                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-11-19 20:26:22
	/// 手插件物料状态监听表
	/// </summary>
	[Table("MES_HI_MATERIAL_LISTEN")]
	public partial class MesHiMaterialListen
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[MaxLength(22)]
		public Decimal ID {get;set;}

		/// <summary>
		/// 监听批次号（SFCS_PRODUCTION.BATCH_NO）
		/// </summary>
		[MaxLength(50)]
		public String BATCH_NO {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(50)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 当前条码ID
		/// </summary>
		[MaxLength(50)]
		public String CURR_REEL_ID {get;set;}

		/// <summary>
		/// 线别ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_LINE_ID {get;set;}

		/// <summary>
		/// 物料料号
		/// </summary>
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 单位用量
		/// </summary>
		[MaxLength(22)]
		public Decimal? UNITY_QTY {get;set;}

		/// <summary>
		/// 备料量
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRE_QTY {get;set;}

		/// <summary>
		/// 使用量
		/// </summary>
		[MaxLength(22)]
		public Decimal? USED_QTY {get;set;}

		/// <summary>
		/// 状态（0：在用，1：关闭）
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}


	}
}
