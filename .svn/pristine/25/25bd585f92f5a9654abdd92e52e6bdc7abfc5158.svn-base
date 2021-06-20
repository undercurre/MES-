/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-18 11:07:21                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtFeederRepair                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-18 11:07:21
	/// 
	/// </summary>
	[Table("SMT_FEEDER_REPAIR")]
	public partial class SmtFeederRepair
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 料架ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? FEEDER_ID {get;set;}

		/// <summary>
		/// 检查人员
		/// </summary>
		[MaxLength(50)]
		public String CHECKER_BY {get;set;}

		/// <summary>
		/// 维修人员
		/// </summary>
		[MaxLength(50)]
		public String REPAIRER_BY {get;set;}

		/// <summary>
		/// 失效代码
		/// </summary>
		[MaxLength(50)]
		public String DEFECT_CODE {get;set;}

		/// <summary>
		/// 失效原因
		/// </summary>
		[MaxLength(50)]
		public String REASON_CODE {get;set;}

		/// <summary>
		/// 损坏部件
		/// </summary>
		[MaxLength(50)]
		public String DAMAGE_PART {get;set;}

		/// <summary>
		/// 维修项目
		/// </summary>
		[MaxLength(50)]
		public String REPAIR_ITEM {get;set;}

		/// <summary>
		/// 维修方法
		/// </summary>
		[MaxLength(150)]
		public String METHOD {get;set;}

		/// <summary>
		/// 保修时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? DEFECT_TIME {get;set;}

		/// <summary>
		/// 维修时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? REPAIR_TIME {get;set;}

		/// <summary>
		/// 结果
		/// </summary>
		[MaxLength(22)]
		public Decimal? RESULT {get;set;}

		/// <summary>
		/// 位置
		/// </summary>
		[MaxLength(22)]
		public Decimal? FEEDER_LOCATION {get;set;}


	}
}
