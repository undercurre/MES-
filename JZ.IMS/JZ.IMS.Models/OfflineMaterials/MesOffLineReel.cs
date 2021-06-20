/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：离线备料表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-16 11:15:42                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesOffLineReel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-07-16 11:15:42
	/// 离线备料表
	/// </summary>
	[Table("MES_OFF_LINE_REEL")]
	public partial class MesOffLineReel
	{
		/// <summary>
		/// ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 线别ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 飞达
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String FEEDER {get;set;}

		/// <summary>
		/// 飞达类型，L/R  左或者右
		/// </summary>
		[Required]
		[MaxLength(2)]
		public String FEEDER_TYPE {get;set;}

		/// <summary>
		/// 料卷
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String REEL_ID {get;set;}

		/// <summary>
		/// 状态，1：待用，2：已用，3：失效
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 备料人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PREPARE_USER {get;set;}

		/// <summary>
		/// 备料时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime PREPARE_TIME {get;set;}

		/// <summary>
		/// 上料人
		/// </summary>
		[MaxLength(30)]
		public String USE_USER {get;set;}

		/// <summary>
		/// 上料时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? USE_TIME {get;set;}

		/// <summary>
		/// 产前确认编号
		/// </summary>
		[MaxLength(50)]
		public String PRE_MST_NO {get;set;}


	}
}
