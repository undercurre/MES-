/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-19 09:10:09                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesQualityInfo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-19 09:10:09
	/// 
	/// </summary>
	[Table("MES_QUALITY_INFO")]
	public partial class MesQualityInfo
	{
		/// <summary>
		/// 表ID
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
		/// 部门
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DEPARTMENT {get;set;}

		/// <summary>
		/// 状态，0：未审核，1：已审核
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 物料名称
		/// </summary>
		[MaxLength(200)]
		public String PART_NAME {get;set;}

		/// <summary>
		/// 物料规格
		/// </summary>
		[MaxLength(200)]
		public String PART_DESC {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String BATCH_NO {get;set;}

		/// <summary>
		/// 批量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BATCH_QTY {get;set;}

		/// <summary>
		/// 确认类别（1：AI首件确认，2:SMT首件确认，3:SMT首五件确认）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CHECK_TYPE {get;set;}

		/// <summary>
		/// 判定结果，0：未判定，1：合格，2，不合格
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal RESULT_STATUS {get;set;}

		/// <summary>
		/// 判定说明
		/// </summary>
		[MaxLength(200)]
		public String RESULT_REMARK {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? AUDIT_TIME {get;set;}

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(30)]
		public String AUDIT_USER {get;set;}

		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime UPDATE_TIME {get;set;}

		/// <summary>
		/// 最后更新人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 板型
		/// </summary>
		[MaxLength(22)]
		public Decimal? PCB_SIDE {get;set;}

		/// <summary>
		/// 班别(白班/夜班)
		/// </summary>
		[MaxLength(20)]
		public String WORK_CLASS {get;set;}

		/// <summary>
		/// 班次(A班/B班)
		/// </summary>
		[MaxLength(20)]
		public String WORK_SHIFTS {get;set;}

        /// <summary>
        /// 生产时间
        /// </summary>
        [Required]
        [MaxLength(7)]
        public DateTime PRODUCT_DATE { get; set; }

        /// <summary>
        /// 线别名称
        /// </summary>
        [NotMapped]
        public String LINE_NAME { get; set; }

        /// <summary>
        /// 部门名称CHECK_TYPE_NAME
        /// </summary>
        [NotMapped]
        public String DEPARTMENT_NAME { get; set; }

        /// <summary>
        /// CHECK_TYPE_NAME
        /// </summary>
        [NotMapped]
        public String CHECK_TYPE_NAME { get; set; }

        /// <summary>
        /// 组织结构名称
        /// </summary>
        [NotMapped]
        public String ORGANIZE_NAME { get; set; }

		/// <summary>
		/// 首件类型：0每班首件;1新机型试产;2转线;3物料变更;4程序变更;5设计变更;6重大工艺变更;7返工;
		/// </summary>
		[MaxLength(22)]
		public int? FIRST_ITEM_TYPE { get; set; }

		/// <summary>
		/// 环保状态：0 ROHS;1 HF;
		/// </summary>
		[MaxLength(22)]
		public int? EP_STATUS { get; set; }
	}
}
