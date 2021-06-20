using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-09-23 10:14:20
	/// 部门类
	/// </summary>
	public class SfcsDepartment
	{
		/// <summary>
		/// 部门ID
		/// </summary>
		public decimal ID { get; set; }

		/// <summary>
		/// 部门中文描述
		/// </summary>
		public string CHINESE { get; set; }

		/// <summary>
		/// 经营单位ID
		/// </summary>
		public decimal SBU_ID { get; set; }

		/// <summary>
		/// 经营单位
		/// </summary>
		public string SBU_CHINESE { get; set; }
	}
}
