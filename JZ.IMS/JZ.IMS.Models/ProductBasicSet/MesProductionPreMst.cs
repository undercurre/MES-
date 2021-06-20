/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-25 09:05:16                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesProductionPreMst                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-25 09:05:16
	/// 产前确认主表
	/// </summary>
	[Table("MES_PRODUCTION_PRE_MST")]
	public partial class MesProductionPreMst
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 机种ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MODEL_ID {get;set;}

		/// <summary>
		/// 客户ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CUSTOMER_ID {get;set;}

		/// <summary>
		/// 料号名称
		/// </summary>
		[MaxLength(50)]
		public String PART_NAME {get;set;}

		/// <summary>
		/// 机种描述
		/// </summary>
		[MaxLength(50)]
		public String MODEL_NAME {get;set;}

		/// <summary>
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? ORAGE_ID {get;set;}

		/// <summary>
		/// 生产时间
		/// </summary>
		[Required]
		[MaxLength(22)]
		public DateTime PRODUCTION_TIME {get;set;}

		/// <summary>
		/// 判断状态;Y :正确;N :失败
		/// </summary>
		[MaxLength(1)]
		public String END_STATUS {get;set;}

		/// <summary>
		/// 产线确定编号:PREYYYMMDDHHMISS
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String MST_NO {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATIME {get;set;}


	}
}
