/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-23 11:43:12                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEcndoc                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-09-23 11:43:12
	/// 
	/// </summary>
	[Table("SFCS_ECNDOC")]
	public partial class SfcsEcndoc
	{
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 工程变更单
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ECNDOC { get; set; }

		/// <summary>
		/// DocNo
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String DOCNO { get; set; }

		/// <summary>
		/// 申请单
		/// </summary>
		[MaxLength(22)]
		public Decimal? ECR { get; set; }

		/// <summary>
		/// 变更原因ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CHANGEREASON { get; set; }

		/// <summary>
		/// 优先级
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRIORITY { get; set; }

		/// <summary>
		/// 变更类型ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CHANGETYPE { get; set; }

		/// <summary>
		/// 当前状态
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WFCURRENTSTATE { get; set; }

		/// <summary>
		/// 之前状态
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WFORIGINALSTATE { get; set; }

		/// <summary>
		/// 成本卷积
		/// </summary>
		[MaxLength(22)]
		public Decimal? ISCOSTROLLUP { get; set; }

		/// <summary>
		/// 单据类型ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ECNDOCTYPE { get; set; }

		/// <summary>
		/// 批量变更
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ISBATCHCHANGE { get; set; }

		/// <summary>
		/// 批量变更类型
		/// </summary>
		[MaxLength(22)]
		public Decimal? BATCHCHANGETYPE { get; set; }

		/// <summary>
		/// 批量变更对象
		/// </summary>
		[MaxLength(22)]
		public Decimal? BATCHCHANGEOBJ { get; set; }

		/// <summary>
		/// 申请人ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PROPOSER { get; set; }

		/// <summary>
		/// 申请部门ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DEPARTMENT { get; set; }

		/// <summary>
		/// 申请日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime PROPOSEDATE { get; set; }

		/// <summary>
		/// 现场更新
		/// </summary>
		[MaxLength(22)]
		public Decimal? ISUPDATEREALTIME { get; set; }

		/// <summary>
		/// 业务制单人ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? BUSINESSCREATEDBY { get; set; }

		/// <summary>
		/// 业务制单日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? BUSINESSCREATEDON { get; set; }

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(22)]
		public Decimal? APPROVEUSER { get; set; }

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? APPROVEDATE { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS { get; set; }

		/// <summary>
		/// 计划人ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? PLANUSER { get; set; }

		/// <summary>
		/// 计划日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? PLANDATE { get; set; }

		/// <summary>
		/// 发行人ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? ISSUEUSER { get; set; }

		/// <summary>
		/// 发行日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? ISSUEDATE { get; set; }

		/// <summary>
		/// 弃审人ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? UNAPPROVEUSER { get; set; }

		/// <summary>
		/// 弃审时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UNAPPROVEDATE { get; set; }

		/// <summary>
		/// 增加新版本
		/// </summary>
		[MaxLength(22)]
		public Decimal? ISADDNEWVERSION { get; set; }

		/// <summary>
		/// 数据创建人
		/// </summary>
		[MaxLength(50)]
		public String CREATEDBY { get; set; }

		/// <summary>
		/// 数据创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATEDDATE { get; set; }

		/// <summary>
		/// 申请人名称
		/// </summary>
		[MaxLength(50)]
		public String PROPOSERNAME { get; set; }

		/// <summary>
		/// 变更原因代码
		/// </summary>
		[MaxLength(50)]
		public String CHANGEREASONCODE { get; set; }

		/// <summary>
		/// 变更原因明细
		/// </summary>
		[MaxLength(50)]
		public String CHANGEREASONDETAIL { get; set; }

		/// <summary>
		/// 变更类型代码
		/// </summary>
		[MaxLength(50)]
		public String CHANGETYPECODE { get; set; }

		/// <summary>
		/// 变更类型说明
		/// </summary>
		[MaxLength(50)]
		public String CHANGETYPEDETAIL { get; set; }

		/// <summary>
		/// 单据类型代码
		/// </summary>
		[MaxLength(50)]
		public String ECNDOCTYPECODE { get; set; }

		/// <summary>
		/// 单据类型说明
		/// </summary>
		[MaxLength(50)]
		public String ECNDOCTYPEDETAIL { get; set; }

		/// <summary>
		/// 申请部门名称
		/// </summary>
		[MaxLength(50)]
		public String DEPARTMENTNAME { get; set; }

		/// <summary>
		/// 业务制单人名称
		/// </summary>
		[MaxLength(50)]
		public String BUSINESSCREATEDNAME { get; set; }

		/// <summary>
		/// 审核人名称
		/// </summary>
		[MaxLength(50)]
		public String APPROVEUSERNAME { get; set; }

		/// <summary>
		/// 计划人名称
		/// </summary>
		[MaxLength(50)]
		public String PLANUSERNAME { get; set; }

		/// <summary>
		/// 发行人名称
		/// </summary>
		[MaxLength(50)]
		public String ISSUEUSERNAME { get; set; }

		/// <summary>
		/// 弃审人名称
		/// </summary>
		[MaxLength(50)]
		public String UNAPPROVEUSERNAME { get; set; }


	}
}
