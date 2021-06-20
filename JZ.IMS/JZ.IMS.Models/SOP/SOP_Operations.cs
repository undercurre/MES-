using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.Models.SOP {

	/// <summary>
	/// 
	/// </summary>
	public class SOP_Operations {
		/// <summary>
		/// 工序子表主键
		/// </summary>
		public decimal ID { get; set; }

		/// <summary>
		/// 制程id
		/// </summary>
		public decimal ROUTE_ID { get; set; }

		/// <summary>
		/// 当前工序
		/// </summary>
		public decimal CURRENT_OPERATION_ID { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		public decimal ORDER_NO { get; set; }

		/// <summary>
		/// 标准人力
		/// </summary>
		public decimal? STANDARD_HUMAN { get; set; }

		/// <summary>
		/// 标准工时
		/// </summary>
		public decimal? STANDARD_WORK_FORCE { get; set; }

		/// <summary>
		/// 标准产能
		/// </summary>
		public decimal? STANDARD_CAPACITY { get; set; }
	}
}
