/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：批次管理表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-17 15:48:19                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBatchManager                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-17 15:48:19
	/// 批次管理表
	/// </summary>
	[Table("MES_BATCH_MANAGER")]
	public partial class MesBatchManager
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 线别名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String LINE_NAME {get;set;}

		/// <summary>
		/// 批次号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String LOC_NO {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 开工日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime PRODUCTION_TIME {get;set;}

		/// <summary>
		/// 生产数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCTION_QTY {get;set;}


	}
}
