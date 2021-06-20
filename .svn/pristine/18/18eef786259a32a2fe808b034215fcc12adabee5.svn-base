/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：飞达点检记录详细内容表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-02 14:44:17                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsFeederKeepContent                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-02 14:44:17
	/// 飞达点检记录详细内容表
	/// </summary>
	[Table("SFCS_FEEDER_KEEP_CONTENT")]
	public partial class SfcsFeederKeepContent
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public int ID {get;set;}

		/// <summary>
		/// SFCS_FEEDER_KEEP_DETAIL表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public int KEEP_DETAIL_ID {get;set;}

		/// <summary>
		/// 飞达ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public int FEEDER_ID {get;set;}

		/// <summary>
		/// 飞达状态 1:可用 2: 使用中 3:待保养 4:待校正 5:待维修 6:报废 7:禁用
		/// </summary>
		[MaxLength(22)]
		public int FEEDER_STATUS {get;set;}

		/// <summary>
		/// 点检备注
		/// </summary>
		[MaxLength(1000)]
		public String CHECK_REMARKS {get;set;}

		/// <summary>
		/// 点检时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CHECK_TIME {get;set;}

		/// <summary>
		/// 点检人员
		/// </summary>
		[MaxLength(255)]
		public String CHECK_USER {get;set;}


	}
}
