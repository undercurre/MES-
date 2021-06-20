/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具保养记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-24 17:22:51                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsMaintainHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-24 17:22:51
	/// 夹具保养记录表
	/// </summary>
	[Table("MES_TONGS_MAINTAIN_HISTORY")]
	public partial class MesTongsMaintainHistory
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 夹具ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_ID { get; set; }

		/// <summary>
		/// 操作记录信息ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal OPERATION_ID { get; set; }

		/// <summary>
		/// 类型（0：保养事项，1：激活事项）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TYPE { get; set; }

		/// <summary>
		/// 保养人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String MAINTAIN_USER { get; set; }

		/// <summary>
		/// 开始时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime START_DATE { get; set; }

		/// <summary>
		/// 结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? END_DATE { get; set; }

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(30)]
		public String AUDIT_USER { get; set; }

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? AUDIT_DATE { get; set; }

		/// <summary>
		/// 保养后状态，1、正常，2、异常
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String REMARK { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_DATE { get; set; }

		/// <summary>
		/// 保养明细
		/// </summary>
		public List<MesTongsMaintainDetail> DetailList { get; set; }
	}
}
