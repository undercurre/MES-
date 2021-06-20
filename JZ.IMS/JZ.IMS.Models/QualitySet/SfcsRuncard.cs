
/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-23 18:19:55                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsRuncard                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-23 18:19:55
	/// 
	/// </summary>
	[Table("SFCS_RUNCARD")]
	public partial class SfcsRuncard
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 流水号
		/// </summary>
		[MaxLength(100)]
		public String SN {get;set;}

		/// <summary>
		/// 副级流水号
		/// </summary>
		[MaxLength(45)]
		public String PARENT_SN {get;set;}

		/// <summary>
		/// 工单ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? WO_ID {get;set;}

		/// <summary>
		/// 制程ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? ROUTE_ID {get;set;}

		/// <summary>
		/// 当前站点
		/// </summary>
		[MaxLength(22)]
		public Decimal? CURRENT_SITE {get;set;}

		/// <summary>
		/// WIP工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? WIP_OPERATION {get;set;}

		/// <summary>
		/// 最后工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? LAST_OPERATION {get;set;}

		/// <summary>
		/// 经过最后工序多少次
		/// </summary>
		[MaxLength(22)]
		public Decimal? LAST_OPERATION_COUNTER {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		/// <summary>
		/// 存仓号
		/// </summary>
		[MaxLength(30)]
		public String TURNIN_NO {get;set;}

		[MaxLength(30)]
		public String TRACKING_NO {get;set;}

		/// <summary>
		/// 卡通号
		/// </summary>
		[MaxLength(60)]
		public String CARTON_NO {get;set;}

		/// <summary>
		/// 栈板号
		/// </summary>
		[MaxLength(60)]
		public String PALLET_NO {get;set;}

		/// <summary>
		/// 出货单号
		/// </summary>
		[MaxLength(35)]
		public String GG_NO {get;set;}

		/// <summary>
		/// 出货批次
		/// </summary>
		[MaxLength(22)]
		public Decimal? GG_ITEM {get;set;}

		/// <summary>
		/// 自动化存仓号
		/// </summary>
		[MaxLength(30)]
		public String SMT_TURNIN_NO {get;set;}

		/// <summary>
		/// 返工数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? RMA_COUNT {get;set;}

		/// <summary>
		/// 投入时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? INPUT_TIME {get;set;}

		/// <summary>
		/// 操作时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? OPERATION_TIME {get;set;}

		/// <summary>
		/// 存仓时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? TURNIN_TIME {get;set;}

		[MaxLength(7)]
		public DateTime? SHIP_TIME {get;set;}

		/// <summary>
		/// 替换标签
		/// </summary>
		[MaxLength(10)]
		public String REPLACE_FLAG {get;set;}

		/// <summary>
		/// 抽检编号
		/// </summary>
		[MaxLength(30)]
		public String SAMPLE_BATCH {get;set;}

		/// <summary>
		/// 抽检标志
		/// </summary>
		[MaxLength(10)]
		public String SAMPLE_FLAG {get;set;}

		[MaxLength(22)]
		public Decimal? WARRANTY {get;set;}


	}
}

