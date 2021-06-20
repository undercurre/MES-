/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：入库记录信息表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-04-27 17:00:28                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsInboundRecordInfo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-04-27 17:00:28
	/// 入库记录信息表
	/// </summary>
	[Table("SFCS_INBOUND_RECORD_INFO")]
	public partial class SfcsInboundRecordInfo
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_ID {get;set;}

		/// <summary>
		/// MES入库单号
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String INBOUND_NO {get;set;}

		/// <summary>
		/// ERP完工单号
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String FINISHED_NO {get;set;}

		/// <summary>
		/// 入库数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal INBOUND_QTY {get;set;}

		/// <summary>
		/// 状态：0 未处理 1 处理中 2已处理
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String STATUS {get;set;}

		/// <summary>
		/// 入库信息
		/// </summary>
		[MaxLength(2000)]
		public String INBOUND_INFO {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(10)]
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime? UPDATE_TIME { get; set; }

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(10)]
		public String UPDATE_BY { get; set; }

		/// <summary>
		/// ERP任务汇报单
		/// </summary>
		[MaxLength(10)]
		public String ERP_BILL_NO { get; set; }

		[MaxLength(1)]
		public String AUTO_EDITOR { get; set; }
	}
}
