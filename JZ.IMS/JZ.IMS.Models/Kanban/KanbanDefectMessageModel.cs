using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 看板不良品信息
	/// </summary>
	public class KanbanDefectMessageModel
	{
		/// <summary>
		/// 产品条码
		/// </summary>
		public string SN { get; set; }
		/// <summary>
		/// 不良现象
		/// </summary>
		public string CHINESE_DESCRIPTION { get; set; }
		/// <summary>
		/// 不良位置
		/// </summary>
		public string OPERATION_SITE_NAME { get; set; }
		/// <summary>
		/// 不良时间
		/// </summary>
		public DateTime DEFECT_TIME { get; set; }
		/// <summary>
		/// 维修人员
		/// </summary>
		public string REPAIRER { get; set; }
		/// <summary>
		/// 维修时间
		/// </summary>
		public DateTime? REPAIR_TIME { get; set; }
		/// <summary>
		/// 当前状态
		/// </summary>
		public string STATUS { get; set; }
		/// <summary>
		/// 当前工序
		/// </summary>
		public string DESCRIPTION { get; set; }
	}
}
