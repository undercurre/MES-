/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：打印任务表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-29 11:49:59                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsPrintTasks                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
    /// <summary>
    /// 嘉志科技
    /// 2020-09-29 11:49:59
    /// 打印任务表
    /// </summary>
    [Table("SFCS_PRINT_TASKS")]
    public partial class SfcsPrintTasks
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Key]
        public Decimal ID { get; set; }

        /// <summary>
        /// 打印文件ID
        /// </summary>
        [MaxLength(22)]
        public Decimal? PRINT_FILE_ID { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        [MaxLength(50)]
        public String OPERATOR { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [MaxLength(7)]
        public DateTime? CREATE_TIME { get; set; }

        /// <summary>
        /// 打印状态0：未打印；1：已打印；2：报废
        /// </summary>
        [MaxLength(1)]
        public String PRINT_STATUS { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        [MaxLength(7)]
        public DateTime? PRINT_TIME { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        [MaxLength(50)]
        public String PART_NO { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        [MaxLength(50)]
        public String WO_NO { get; set; }

        /// <summary>
        /// 打印人员
        /// </summary>
        [MaxLength(50)]
        public String PRINTER { get; set; }

        /// <summary>
        /// 打印数据
        /// </summary>
        [MaxLength(4000)]
        public String PRINT_DATA { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        [MaxLength(50)]
        public String CARTON_NO { get; set; }

        /// <summary>
        /// 栈板编号
        /// </summary>
        [MaxLength(50)]
        public String PALLET_NO { get; set; }

        /// <summary>
        /// 打印次数
        /// </summary>
        [MaxLength(22)]
        public Decimal? PRINT_NO { get; set; }

    }
}
