using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JZ.IMS.Models.SfcsTinRecord
{
	/// <summary>
	/// 嘉志科技
	/// 2020-02-26 16:08:37
	/// 锡条用量记录表
	/// </summary>
	[Table("SFCS_TIN_RECORD")]
	public partial class SfcsTinRecord
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		[MaxLength(22)]
		public Decimal? VERSION { get; set; }

		[MaxLength(50)]
		public String ENABLE_BILL_ID { get; set; }

		[MaxLength(50)]
		public String DISABLE_BILL_ID { get; set; }

		/// <summary>
		/// 线体
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID { get; set; }

		/// <summary>
		/// 产能日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime OUTPUT_DAY { get; set; }

		/// <summary>
		/// 产能数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal OUTPUT_NUM { get; set; }

		/// <summary>
		/// 加锡量(KG)
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ADD_TIN { get; set; }

		/// <summary>
		/// 分析结果（正常、过量、量少）
		/// </summary>
		[MaxLength(50)]
		public String RESULT { get; set; }

		/// <summary>
		/// 结果描述
		/// </summary>
		[MaxLength(200)]
		public String DESCRIPTION { get; set; }

		/// <summary>
		/// 分析时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? AUDIT_TIME { get; set; }

		/// <summary>
		/// 分析人
		/// </summary>
		[MaxLength(50)]
		public String AUDITOR { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATER { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME { get; set; }

		[MaxLength(30)]
		public String ATTRIBUTE1 { get; set; }

		[MaxLength(30)]
		public String ATTRIBUTE2 { get; set; }

		[MaxLength(30)]
		public String ATTRIBUTE3 { get; set; }

		[MaxLength(30)]
		public String ATTRIBUTE4 { get; set; }

		[MaxLength(30)]
		public String ATTRIBUTE5 { get; set; }

		/// <summary>
		/// 组织ID
		/// </summary>
		[MaxLength(50)]
		public string ORGANIZE_ID { get; set; }

	}
}
