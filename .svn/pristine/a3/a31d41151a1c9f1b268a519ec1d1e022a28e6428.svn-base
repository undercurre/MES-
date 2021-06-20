/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检记录主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-31 09:31:24                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipKeepHead                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-31 09:31:24
	/// 设备点检记录主表
	/// </summary>
	[Table("SFCS_EQUIP_KEEP_HEAD")]
	public partial class SfcsEquipKeepHead
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 设备点检编号（'EK'+'YYYYMMDDHHMMSS'）
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String KEEP_CODE { get; set; }

		/// <summary>
		/// 保养设备ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal EQUIP_ID { get; set; }

		/// <summary>
		/// 保养类型（0：日保养，1月保养，2年保养）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal KEEP_TYPE { get; set; }

		/// <summary>
		/// 设备点检时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime KEEP_TIME { get; set; }

		/// <summary>
		/// 设备点检人员
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String KEEP_USER { get; set; }

		/// <summary>
		/// 设备点检审核人
		/// </summary>
		[MaxLength(50)]
		public String KEEP_CHECKER { get; set; }

		/// <summary>
		/// 设备点检状态(0：待审核，1：已审核，3：拒绝)
		/// </summary>
		[MaxLength(22)]
		public Decimal? KEEP_CHECK_STATUS { get; set; }

		/// <summary>
		/// 0:正常； 1: 闲置； 2:待维修； 3: 维修中；4: 报废；
		/// </summary>
		[MaxLength(22)]
		public Decimal? EQUIP_STATUS { get; set; }

		/// <summary>
		/// 存放地点(线别ID) 
		/// </summary>
		public Decimal? STATION_ID { get; set; }

		/// <summary>
		/// 设备出厂编号
		/// </summary>
		public string PRODUCT_NO { get; set; }

		/// <summary>
		/// 设备编号
		/// </summary>
		public string NAME { get; set; }

		/// <summary>
		/// 审核时间
		/// </summary>
		public DateTime? KEEP_CHECK_TIME { get; set; }
	}
}
