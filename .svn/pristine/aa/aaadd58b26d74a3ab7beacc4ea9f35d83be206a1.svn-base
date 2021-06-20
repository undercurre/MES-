/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：看板-每小时产能记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-25 16:55:23                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesKanbanHourYidld                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-25 16:55:23
	/// 看板-每小时产能记录表
	/// </summary>
	[Table("MES_KANBAN_HOUR_YIDLD")]
	public partial class MesKanbanHourYidld
	{
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
		[MaxLength(22)]
		public Decimal? LINE_ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(20)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 板底或板面
		/// </summary>
		[MaxLength(22)]
		public Decimal? PCB_SIDE {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? WORK_TIME {get;set;}

		/// <summary>
		/// 生产数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? OUTPUT_QTY {get;set;}

		/// <summary>
		/// 标准产能
		/// </summary>
		[MaxLength(22)]
		public Decimal? STANDARD_CAPACITY {get;set;}

		/// <summary>
		/// 标准产能*（上班分钟/60）
		/// </summary>
		[MaxLength(22)]
		public Decimal? STANDARD_CAPACITY_WORK {get;set;}

		/// <summary>
		/// 正常范围-上限
		/// </summary>
		[MaxLength(22)]
		public Decimal? VALUE_MAX {get;set;}

		/// <summary>
		/// 正常范围-下限
		/// </summary>
		[MaxLength(22)]
		public Decimal? VALUE_MIN {get;set;}

		/// <summary>
		/// 是否当前小时(Y/N)
		/// </summary>
		[MaxLength(1)]
		public String CURRENT_HOUR {get;set;}

		/// <summary>
		/// 报告ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? REPORT_ID {get;set;}

		/// <summary>
		/// 报告编号
		/// </summary>
		[MaxLength(20)]
		public String REPORT_NO {get;set;}

        /// <summary>
        /// 线别
        /// </summary>
        [NotMapped]
        public String LINE_NAME { get; set; }

        /// <summary>
        /// 线别
        /// </summary>
        [NotMapped]
        public String CN_DESC { get; set; }

        /// <summary>
        /// 报告内容
        /// </summary>
        [NotMapped]
        public String REPORT_CONTENT { get; set; }

        /// <summary>
        /// 产率
        /// </summary>
        [NotMapped]
        public String OUTPUT_TIVITY { get; set; }

        /// <summary>
        /// 上报人数
        /// </summary>
        [NotMapped]
        public String REPORT_NUM { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [NotMapped]
        public String PRICE { get; set; }

        /// <summary>
        /// 平均产量
        /// </summary>
        [NotMapped]
        public String AVERAGE_QTY { get; set; }

        /// <summary>
        /// 总产值
        /// </summary>
        [NotMapped]
        public String TOTAL_PRICE { get; set; }

        /// <summary>
        /// 平均产值
        /// </summary>
        [NotMapped]
        public String AVERAGE_PRICE { get; set; }
    }
}
