/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：监控报告表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-19 14:46:51                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesMonitoringReport                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-11-19 14:46:51
	/// 监控报告表
	/// </summary>
	[Table("MES_MONITORING_REPORT")]
	public partial class MesMonitoringReport
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 车间类型(SMT/PCBA)
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String LINE_TYPE {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 工单
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 板底或板面
		/// </summary>
		[MaxLength(22)]
		public Decimal? PCB_SIDE {get;set;}

		/// <summary>
		/// 报告编号
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String REPORT_NO {get;set;}

		/// <summary>
		/// 报告分类(关联MES_MONITORING_RPT_TYPE.REPORT_TYPE)
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String REPORT_TYPE {get;set;}

		/// <summary>
		/// 报告内容
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String REPORT_CONTENT {get;set;}

		/// <summary>
		/// 正常范围-上限
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal VALUE_MAX {get;set;}

		/// <summary>
		/// 正常范围-下限
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal VALUE_MIN {get;set;}

		/// <summary>
		/// 监控值
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal VALUE_MONITORING {get;set;}

		/// <summary>
		/// 报告时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// -1：正常；异常（0：待处理、1：处理中、2：已完结）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 分析结果
		/// </summary>
		[MaxLength(500)]
		public String REASON {get;set;}

		/// <summary>
		/// 产能小时
		/// </summary>
		[MaxLength(7)]
		public DateTime? WORK_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(50)]
		public String MODIFIER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? MODIFY_TIME {get;set;}


	}
}
