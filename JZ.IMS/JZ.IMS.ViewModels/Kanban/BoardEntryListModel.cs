using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	public class BoardEntryListModel
	{
		/// <summary>
		/// ID
		/// </summary>
		public decimal ID { get; set; }
		/// <summary>
		/// 组织ID
		/// </summary>
		public string ORGANIZE_ID { get; set; }
		/// <summary>
		/// 线别类型
		/// </summary>
		public string LINE_TYPE { get; set; }
		/// <summary>
		/// 线别ID
		/// </summary>
		public decimal LINE_ID { get; set; }
		/// <summary>
		/// 线别简称
		/// </summary>
		public string ATTRIBUTE4 { get; set; }
		/// <summary>
		/// 线别名称
		/// </summary>
		public string LINE_NAME { get; set; }
		/// <summary>
		/// 组织名称
		/// </summary>
		public string ORGANIZE_NAME { get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public string ATTRIBUTE5 { get; set; }
		/// <summary>
		/// 线体类别，SMT区分AI/RI/SMT
		/// </summary>
		public string ATTRIBUTE6 { get; set; }
		/// <summary>
		/// 楼层
		/// </summary>
		public string ATTRIBUTE7 { get; set; }
	}
}
