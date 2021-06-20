/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检维修零件表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-01 10:47:11                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipRepairDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-11-01 10:47:11
	/// 设备点检维修零件表
	/// </summary>
	[Table("SFCS_EQUIP_REPAIR_DETAIL")]
	public partial class SfcsEquipRepairDetail
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// SFCS_EQUIP_REPAIR_HEAD表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal EQUIP_REPAIR_HEAD_ID {get;set;}

		/// <summary>
		/// 配件名称
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String COMPONENT_NAME {get;set;}

		/// <summary>
		/// 配件规格
		/// </summary>
		[MaxLength(100)]
		public String COMPONENT_MODEL {get;set;}


	}
}
