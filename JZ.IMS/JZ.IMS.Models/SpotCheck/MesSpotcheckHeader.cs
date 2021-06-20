/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：抽检报告主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-26 10:07:59                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesSpotcheckHeader                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
    /// <summary>
    /// 嘉志科技
    /// 2019-11-26 10:07:59
    /// 抽检报告主表
    /// </summary>
    [Table("MES_SPOTCHECK_HEADER")]
    public partial class MesSpotcheckHeader
    {
        /// <summary>
        /// 抽检批次号
        /// </summary>
        [Key]
        public String BATCH_NO { get; set; }

        /// <summary>
        /// 线别ID
        /// </summary>
        [Required]
        [MaxLength(22)]
        public Decimal LINE_ID { get; set; }

        /// <summary>
        /// 线别类型：SMT，PCBA
        /// </summary>
        [Required]
        [MaxLength(4)]
        public String LINE_TYPE { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public String WO_NO { get; set; }

        /// <summary>
        /// 送检数
        /// </summary>
        [Required]
        [MaxLength(22)]
        public Decimal ALL_QTY { get; set; }

        /// <summary>
        /// 抽检数
        /// </summary>
        [Required]
        [MaxLength(22)]
        public Decimal CHECK_QTY { get; set; }

        /// <summary>
        /// 不良数
        /// </summary>
        [MaxLength(22)]
        public Decimal? FAIL_QTY { get; set; }

        /// <summary>
        /// 抽检标准
        /// </summary>
        [Required]
        [MaxLength(22)]
        public Decimal SAMP_STANDART { get; set; }

        /// <summary>
        /// 抽样水平
        /// </summary>
        [MaxLength(22)]
        public Decimal? SAMP_SIZE { get; set; }

        /// <summary>
        /// 状态:0 新增 ; 2确认；3审核
        /// </summary>
        [Required]
        [MaxLength(22)]
        public Decimal STATUS { get; set; }

        /// <summary>
        /// 检验员，创建人员
        /// </summary>
        [MaxLength(10)]
        public String CHECKER { get; set; }

        /// <summary>
        /// 确认人
        /// </summary>
        [MaxLength(10)]
        public String CONFIRM { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [MaxLength(10)]
        public String AUDITOR { get; set; }

        /// <summary>
        /// 抽检判断结果0：合格，1：特采，2：返工，3：报废
        /// </summary>
        [MaxLength(22)]
        public Decimal? RESULT { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [MaxLength(7)]
        public DateTime CREATE_DATE { get; set; } = DateTime.Now;

        /// <summary>
        /// 组织架构ID
        /// </summary>
        [MaxLength(50)]
        public String ORGANIZE_ID { get; set; }

        /// <summary>
        /// 批量（工单数量）
        /// </summary>
        [MaxLength(22)]
        public Decimal? WO_QTY { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [MaxLength(22)]
        public Decimal? ORDER_NO { get; set; }

        /// <summary>
        /// 外观抽检数
        /// </summary>
        [MaxLength(22)]
        public Decimal? OUTER_CHECK_QTY { get; set; }

        /// <summary>
        /// 外观不良数
        /// </summary>
        [MaxLength(22)]
        public Decimal? OUTER_FAIL_QTY { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200)]
        public String REMARK { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        [MaxLength(10)]
        public String WO_CLASS { get; set; }

        /// <summary>
        /// 检验类型0：过程检验，1：完工检验，2：终检检验
        /// </summary>
        [MaxLength(22)]
        public Decimal? QC_TYPE { get; set; }

        /// <summary>
        /// 父级检验单
        /// </summary>
        [MaxLength(50)]
        public String PARENT_BATCH_NO { get; set; } = "";

        /// <summary>
        /// 站点ID
        /// </summary>
        [MaxLength(22)]
        public Decimal? OPERATION_SITE_ID { get; set; } = null;

        /// <summary>
        /// 质检方案ID
        /// </summary>
        [MaxLength(22)]
        public Decimal? QCSCHEMAHEAD { get; set; } = null;

        /// <summary>
        /// 质检方案名称
        /// </summary>
        [MaxLength(100)]
        public String QCSCHEMANAME { get; set; } = "";

        /// <summary>
        /// 质检方案版本
        /// </summary>
        [MaxLength(50)]
        public String QCSCHEMAVERSION { get; set; } = "";

        /// <summary>
        /// 审核时间
        /// </summary>
        [MaxLength(7)]
        public DateTime? AUDIT_TIME { get; set; } = null;
    }
}
