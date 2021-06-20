/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：特殊SN表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-24 14:15:40                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ImportRuncardSn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-24 14:15:40
	/// 特殊SN表
	/// </summary>
	[Table("IMPORT_RUNCARD_SN")]
	public partial class ImportRuncardSn
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(20)]
		public String WO_NO {get;set;}

		/// <summary>
		/// SN流水号
		/// </summary>
		[MaxLength(50)]
		public String SN {get;set;}

		/// <summary>
		/// 父级流水号 暂无使用
		/// </summary>
		[MaxLength(50)]
		public String PARENT_SN {get;set;}

		/// <summary>
		/// 制程ID SFCS_ROUTES.ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? ROUTE_ID {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		[MaxLength(1)]
		public String ENABLE {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(10)]
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		[MaxLength(10)]
		public String UPDATE_BY {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE1 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE2 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE3 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(255)]
		public String ATTRIBUTE4 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(255)]
		public String ATTRIBUTE5 {get;set;}

		/// <summary>
		/// 主卡IMEI
		/// </summary>
		[MaxLength(100)]
		public String MAIN_CARD_IMEI { get; set; }

		/// <summary>
		/// 副卡IMEI
		/// </summary>
		[MaxLength(100)]
		public String MINOR_CARD_IMEI { get; set; }

		/// <summary>
		/// BT
		/// </summary>
		[MaxLength(100)]
		public String BT { get; set; }

		/// <summary>
		/// MAC
		/// </summary>
		[MaxLength(50)]
		public String MAC { get; set; }

		/// <summary>
		/// MEID
		/// </summary>
		[MaxLength(50)]
		public String MEID { get; set; }

	}
}
