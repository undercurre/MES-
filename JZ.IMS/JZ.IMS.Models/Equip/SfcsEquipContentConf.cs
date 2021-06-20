/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检内容配置表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-30 15:43:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipContentConf                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-30 15:43:48
	/// 设备点检内容配置表
	/// </summary>
	[Table("SFCS_EQUIP_CONTENT_CONF")]
	public partial class SfcsEquipContentConf
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 保养类型（0：日保养，1月保养，2年保养）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal KEEP_TYPE { get; set; }

		/// <summary>
		/// 保养内容事项
		/// </summary>
		[MaxLength(100)]
		public String KEEP_CONTENT { get; set; }

		/// <summary>
		/// 保养工具辅料
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String KEEP_TOOLS { get; set; }

		/// <summary>
		/// 设备分类ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CATEGORY_ID { get; set; }

		/// <summary>
		/// 是否有效（Y:有效，N：无效）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLE { get; set; }

		/// <summary>
		/// 排序数字
		/// </summary>
		[Required]
		[MaxLength(22)]
		public decimal ORDER_NO { get; set; }

		/// <summary>
		/// 设备分类名称
		/// </summary>
		[NotMapped]
		public virtual String CATEGORY_NAME { get; set; }

		/// <summary>
		/// 图片资源列表
		/// </summary>
		[NotMapped]
		public virtual List<SOP_OPERATIONS_ROUTES_RESOURCE> RESOURCE_LIST { get; set; }
	}
}
