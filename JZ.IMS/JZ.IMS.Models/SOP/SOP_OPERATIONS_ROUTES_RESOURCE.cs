using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JZ.IMS.Models
{

	/// <summary>
	/// 
	/// </summary>
	public class SOP_OPERATIONS_ROUTES_RESOURCE
	{
		/// <summary>
		/// 主键
		/// </summary>
		public decimal ID { get; set; }

		/// <summary>
		/// 关联(SOP_OPERATIONS_ROUTES.ID)或（SOP_ROUTES.ID） 
		/// </summary>
		public decimal MST_ID { get; set; }

		/// <summary>
		/// 资源类型(0:图片；1：视频)
		/// </summary>
		public decimal RESOURCE_TYPE { get; set; }

		/// <summary>
		/// 资源URL（原图URL/视频URL）
		/// </summary>
		public string RESOURCE_URL { get; set; }

		/// <summary>
		/// 资源(封面图/缩略图)URL
		/// </summary>
		public string RESOURCE_URL_THUMB { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		public decimal ORDER_NO { get; set; }

		/// <summary>
		/// 资源类别（0：产品图，1：作业图，2：零件图）
		/// </summary>
		public decimal RESOURCES_CATEGORY { get; set; }

		/// <summary>
		/// 资源名称
		/// </summary>
		public string RESOURCE_NAME { get; set; }

		/// <summary>
		/// 资源大小
		/// </summary>
		public decimal RESOURCE_SIZE { get; set; }

		/// <summary>
		/// 资源说明
		/// </summary>
		public string RESOURCE_MSG { get; set; }

		/// <summary>
		/// 资源PDFURL
		/// </summary>
		public string RESOUTCE_PDF_URL { get; set; }

		/// <summary>
		/// 零件料号
		/// </summary>
		[NotMapped]
		public string PART_NO { get; set; }
		/// <summary>
		/// 零件名称
		/// </summary>
		[NotMapped]
		public string PART_NAME { get; set; }
		/// <summary>
		/// 用量
		/// </summary>
		[NotMapped]
		public decimal PART_QTY { get; set; }
		/// <summary>
		/// 规格
		/// </summary>
		[NotMapped]
		public string PART_DESC { get; set; }
		/// <summary>
		/// 位置
		/// </summary>
		[NotMapped]
		public string PART_LOCATION { get; set; }

		/// <summary>
		/// 是否扫码
		/// </summary>
		[NotMapped]
		public string IS_SCAN { get; set; }
	}
}
