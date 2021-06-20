/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-02 13:38:05                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesIpqaStopNotice                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-02 13:38:05
	/// 
	/// </summary>
	[Table("MES_IPQA_STOP_NOTICE")]
	public partial class MesIpqaStopNotice
	{
		/// <summary>
		/// 表ID，序列MES_IPQA_STOP_NOTICE_SEQ
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}

		/// <summary>
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 生产日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime PRODUCTION_DATE {get;set;}

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
		public String PCB_PN {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String MODEL {get;set;}

		/// <summary>
		/// 生产批量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TOTAL_QTY {get;set;}

		/// <summary>
		/// 已生产数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal FINISHED_QTY {get;set;}

		/// <summary>
		/// 发文部门ID，关联SYS_DEPARTMENT.ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CREATE_DEP_ID {get;set;}

		/// <summary>
		/// 发文人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 收发部门ID，关联SYS_DEPARTMENT.ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? RECEIPT_DEP_ID {get;set;}

		/// <summary>
		/// 签收人
		/// </summary>
		[MaxLength(50)]
		public String RECEIPT_USER {get;set;}

		/// <summary>
		/// 要求反馈时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime FEEDBACK_TIME {get;set;}

		/// <summary>
		/// 实际反馈时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? PRACTICAL_TIME {get;set;}

		/// <summary>
		/// 品质异常描述
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String EXCEPTION_DESCRIPTION {get;set;}

		/// <summary>
		/// 品质异常-不良现象
		/// </summary>
		[MaxLength(200)]
		public String EXCEPTION_FAIL_INFO {get;set;}

		/// <summary>
		/// 品质异常-不良率
		/// </summary>
		[MaxLength(50)]
		public String EXCEPTION_FAIL_RATE {get;set;}

		/// <summary>
		/// 品质异常-IPQA
		/// </summary>
		[MaxLength(50)]
		public String EXCEPTION_IPQA {get;set;}

		/// <summary>
		/// 品质异常-IPQA主管
		/// </summary>
		[MaxLength(50)]
		public String EXCEPTION_IPQA_HEAD {get;set;}

		/// <summary>
		/// QE初步分析意见
		/// </summary>
		[MaxLength(200)]
		public String ANALYSIS_OPINION {get;set;}

		/// <summary>
		/// QE初步分析-原因分析，多个之间用逗号分隔  SFCS_PARAMETERS.LOOKUP_TYPE='IPQA_STOP_REASON'
		/// </summary>
		[MaxLength(50)]
		public String ANALYSIS_REASON {get;set;}

		/// <summary>
		/// QE初步分析-QE工程师
		/// </summary>
		[MaxLength(50)]
		public String ANALYSIS_QE {get;set;}

		/// <summary>
		/// QE初步分析-QE主管
		/// </summary>
		[MaxLength(50)]
		public String ANALYSIS_QE_HEAD {get;set;}

		/// <summary>
		/// 应急对策及防止再发生措施
		/// </summary>
		[MaxLength(200)]
		public String SOLUTION_METHOD {get;set;}

		/// <summary>
		/// 应急对策签名
		/// </summary>
		[MaxLength(50)]
		public String SOLUTION_SIGN {get;set;}

		/// <summary>
		/// 应急对策日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? SOLUTION_DATE {get;set;}

		/// <summary>
		/// 效果追踪
		/// </summary>
		[MaxLength(200)]
		public String EFFECT_TRACKING {get;set;}

		/// <summary>
		/// 效果追踪-IPQA
		/// </summary>
		[MaxLength(50)]
		public String EFFECT_IPQA {get;set;}

		/// <summary>
		/// 效果追踪-日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? EFFECT_DATE {get;set;}

		/// <summary>
		/// 复线通知
		/// </summary>
		[MaxLength(200)]
		public String RESUME_NOTICE {get;set;}

		/// <summary>
		/// 文件编号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String FILE_CODE {get;set;}

		/// <summary>
		/// 状态，0=拟制，1=已审核，2=已批准
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 审核人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String AUDIT_USER {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime AUDIT_TIME {get;set;}

		/// <summary>
		/// 审核内容
		/// </summary>
		[MaxLength(50)]
		public String AUDIT_CONTENT {get;set;}

		/// <summary>
		/// 批准人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String APPROVAL_USER {get;set;}

		/// <summary>
		/// 批准时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime APPROVAL_TIME {get;set;}

		/// <summary>
		/// 批准内容
		/// </summary>
		[MaxLength(50)]
		public String APPROVAL_CONTENT {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime UPDATE_TIME {get;set;}

		/// <summary>
		/// 属性1
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE1 {get;set;}

		/// <summary>
		/// 属性2
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE2 {get;set;}

		/// <summary>
		/// 属性3
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE3 {get;set;}

		/// <summary>
		/// 属性4
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE4 {get;set;}

		/// <summary>
		/// 属性5
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE5 {get;set;}


	}
}
