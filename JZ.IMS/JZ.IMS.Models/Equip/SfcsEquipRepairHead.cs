/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检维修表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-31 10:50:09                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipRepairHead                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-31 10:50:09
	/// 设备点检维修表
	/// </summary>
	[Table("SFCS_EQUIP_REPAIR_HEAD")]
	public partial class SfcsEquipRepairHead
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? ID {get;set;}

		/// <summary>
		/// 维修编号('ER'+'YYYYMMDDHHMMSS')
		/// </summary>
		[MaxLength(50)]
		public String REPAIR_CODE {get;set;}

		/// <summary>
		/// 保养设备ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? EQUIP_ID {get;set;}

		/// <summary>
		/// 维修人员
		/// </summary>
		[MaxLength(50)]
		public String REPAIR_USER {get;set;}

		/// <summary>
		/// 维修开始时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? BEGINTIME {get;set;}

		/// <summary>
		/// 维修结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? ENDTIME { get; set; }

		/// <summary>
		/// 维修记录
		/// </summary>
		[MaxLength(100)]
		public String REPAIR_CONTENT {get;set;}

		/// <summary>
		/// 维修结果（0：正常，3报废）
		/// </summary>
		[MaxLength(1)]
		public String REPAIR_STATUS {get;set;}


	}
}
