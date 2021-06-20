using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// AOI/SPI集成看板模型
	/// </summary>
	public class AoiAndSpiReportListModel
	{
		/// <summary>
		/// 组织ID
		/// </summary>
		public string ORGANIZE_ID { get; set; }
		/// <summary>
		/// 机台名称
		/// </summary>
		public string STATION_NAME { get; set; }
		/// <summary>
		/// 机台类型
		/// </summary>
		public string STATION_TYPE { get; set; }
		/// <summary>
		/// 楼层
		/// </summary>
		public string FLOOR { get; set; }
		/// <summary>
		/// 工单号
		/// </summary>
		public string WO_NO { get; set; }
		/// <summary>
		/// 料号
		/// </summary>
		public string PART_NO { get; set; }
		/// <summary>
		/// 线别ID
		/// </summary>
		public decimal LINE_ID { get; set; }
		/// <summary>
		/// 线别名称
		/// </summary>
		public string LINE_NAME { get; set; }
		/// <summary>
		/// 机种
		/// </summary>
		public string PART_DESC { get; set; }
		/// <summary>
		/// 直通率
		/// </summary>
		public decimal FIRST_PASS_YIELD { get; set; }
		/// <summary>
		/// 异常数
		/// </summary>
		public decimal PASS_QTY { get; set; }
		/// <summary>
		/// 总数
		/// </summary>
		public decimal TOTAL_QTY { get; set; }

		public decimal Page { get; set; }

        public decimal Limit { get; set; }
        
        /// <summary>
        /// TOP 5不良现象数据
        /// </summary>
        public List<AoiAndSpiReportDetail> DefectList { get; set; }
	}

	/// <summary>
	/// AOI/SPI集成看板不良现象类
	/// </summary>
	public class AoiAndSpiReportDetail
	{
		/// <summary>
		/// 不良名称
		/// </summary>
		public string DEFECT_NAME { get; set; }
		/// <summary>
		/// 不良数量
		/// </summary>
		public decimal DEFECT_QTY { get; set; }
	}
}
