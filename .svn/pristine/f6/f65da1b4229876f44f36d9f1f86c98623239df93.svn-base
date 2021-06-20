/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：换线记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-15 19:06:24                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesChangeLineRecord                                     
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
	/// 2019-11-15 19:06:24
	/// 换线记录表
	/// </summary>
	[Table("MES_CHANGE_LINE_RECORD")]
	public partial class MesChangeLineRecord
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 车间类型(SMT/PCBA)
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String LINE_TYPE { get; set; }

		/// <summary>
		/// 线体ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID { get; set; }

		/// <summary>
		/// 线体名称
		/// </summary>
		[NotMapped]
		public string LINE_NAME { get; set; }

		/// <summary>
		/// 上一个工单号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PRE_WO_NO { get; set; }

		/// <summary>
		/// 上一个工单的板底板面
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRE_PCB_SIDE { get; set; }

		/// <summary>
		/// 上一个工单开始时间
		/// </summary>
		[MaxLength(7)]
		public DateTime PRE_START_TIME { get; set; }

		/// <summary>
		/// 上一个工单结束时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime PRE_END_TIME { get; set; }

		/// <summary>
		/// 下一个工单号
		/// </summary>
		[MaxLength(50)]
		public String NEXT_WO_NO { get; set; }

		/// <summary>
		/// 下一个工单的板底板面
		/// </summary>
		[MaxLength(22)]
		public Decimal? NEXT_PCB_SIDE { get; set; }

		/// <summary>
		/// 下一个工单开始时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? NEXT_START_TIME { get; set; }

		/// <summary>
		/// 下一个工单结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? NEXT_END_TIME { get; set; }

		/// <summary>
		/// 耗时分钟数
		/// </summary>
		[MaxLength(22)]
		public Decimal? TAKING_TIME { get; set; }

		/// <summary>
		/// 正常耗时下限
		/// </summary>
		[MaxLength(22)]
		public Decimal? VALUE_MIN { get; set; }

		/// <summary>
		/// 正常耗时上限
		/// </summary>
		[MaxLength(22)]
		public Decimal? VALUE_MAX { get; set; }

		/// <summary>
		/// 报告ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? REPORT_ID { get; set; }

		/// <summary>
		/// 报告编号
		/// </summary>
		[MaxLength(20)]
		public String REPORT_NO { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime ADD_TIME { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(50)]
		public String DESCRIPTION { get; set; }

		/// <summary>
		/// 换线时间体现数组
		/// </summary>
		[NotMapped]
		public List<decimal[]> TimeValue { get; set; }
	}
}
