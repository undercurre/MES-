/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：不良报工表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-26 14:37:35                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsDefectReportWork                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-26 14:37:35
	/// 不良报工表
	/// </summary>
	[Table("SFCS_DEFECT_REPORT_WORK")]
	public partial class SfcsDefectReportWork
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 不良位号
		/// </summary>
		[MaxLength(10)]
		public String LOC {get;set;}

		/// <summary>
		/// 不良代码
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String DEFECT_CODE {get;set;}

		/// <summary>
		/// 工单ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_ID {get;set;}

		/// <summary>
		/// 站点ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal OPERATION_SITE_ID {get;set;}

		/// <summary>
		/// 用户
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String OPERATOR {get;set;}

		/// <summary>
		/// 报工数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QTY {get;set;}

		/// <summary>
		/// 报工时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime REPORT_TIME {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

        /// <summary>
        /// 不良描述
        /// </summary>
        [NotMapped]
        public String DEFECT_NAME { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        [NotMapped]
        public String LINE_NAME { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        [NotMapped]
        public String WO_NO { get; set; }

    }
}
