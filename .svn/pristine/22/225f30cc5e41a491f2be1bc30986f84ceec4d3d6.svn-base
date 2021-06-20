using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.Models
{
	public class KanBanSpotCheckModel
	{
		/// <summary>
		/// 总抽检数
		/// </summary>
		public decimal CHECK_QTY { get; set; }

		/// <summary>
		/// 总异常数
		/// </summary>
		public decimal FAIL_QTY { get; set; }

		/// <summary>
		/// 线别类型
		/// </summary>
		public string LINE_TYPE { get; set; }

		/// <summary>
		/// 线别ID
		/// </summary>
		public decimal LINE_ID { get; set; }

		/// <summary>
		/// 工单号
		/// </summary>
		public string WO_NO { get; set; }

		public List<KanBanSpotCheckDefectDetail> DetailData { get; set; }
	}

	public class KanBanSpotCheckDefectDetail
	{
		/// <summary>
		/// 批次号
		/// </summary>
		public string BATCH_NO { get; set; }

		/// <summary>
		/// 抽检人
		/// </summary>
		public string CHECKER { get; set; }

		/// <summary>
		/// 抽检时间-主
		/// </summary>
		public DateTime? CREATE_DATE { get; set; }

		/// <summary>
		/// 抽检时间-明细
		/// </summary>
		public DateTime? CREATE_TIME { get; set; }

		/// <summary>
		/// 不良代码
		/// </summary>
		public string DEFECT_CODE { get; set; }

		/// <summary>
		/// 不良位号
		/// </summary>
		public string DEFECT_LOC { get; set; }

		/// <summary>
		/// 不良描述
		/// </summary>
		public string DEFECT_DESCRIPTION { get; set; }

		/// <summary>
		/// 不良数
		/// </summary>
		public decimal DEFECTQTY { get; set; }
	}
}
