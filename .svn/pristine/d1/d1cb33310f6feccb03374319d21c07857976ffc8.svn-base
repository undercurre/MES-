/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：飞达点检记录详细表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-02 14:43:01                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsFeederKeepDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-02 14:43:01
	/// 飞达点检记录详细表
	/// </summary>
	[Table("SFCS_FEEDER_KEEP_DETAIL")]
	public partial class SfcsFeederKeepDetail
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public int ID {get;set;}

		/// <summary>
		/// SFCS_FEEDER_KEEP_HEAD表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal KEEP_HEAD_ID {get;set;}

		/// <summary>
		/// 飞达类型
		/// </summary>
		[MaxLength(50)]
		public String FEEDER_TYPE {get;set;}

		/// <summary>
		/// 飞达尺寸
		/// </summary>
		[MaxLength(50)]
		public String FEEDER_SIZE {get;set;}

		/// <summary>
		/// 类型总数
		/// </summary>
		[MaxLength(22)]
		public int FEEDER_TYPE_TOTAL {get;set;}

		/// <summary>
		/// 点检数量
		/// </summary>
		[MaxLength(22)]
		public int CHECK_QTY {get;set;}


	}
}
