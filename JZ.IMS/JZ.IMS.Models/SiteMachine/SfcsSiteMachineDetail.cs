/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：站点设备配置表明细                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-02 17:20:40                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsSiteMachineDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-02 17:20:40
	/// 站点设备配置表明细
	/// </summary>
	[Table("SFCS_SITE_MACHINE_DETAIL")]
	public partial class SfcsSiteMachineDetail
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 站点设备配置表ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? MST_ID {get;set;}

		/// <summary>
		/// 配置项
		/// </summary>
		[MaxLength(22)]
		public Decimal? KEY {get;set;}

		/// <summary>
		/// 配置内容
		/// </summary>
		[MaxLength(100)]
		public String VALUE {get;set;}

		/// <summary>
		/// 配置描述
		/// </summary>
		[MaxLength(1000)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 创建日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? CHREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(50)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 是否启用(Y:启用；N:禁用)
		/// </summary>
		[MaxLength(1)]
		public String ENABLE {get;set;}


	}
}
