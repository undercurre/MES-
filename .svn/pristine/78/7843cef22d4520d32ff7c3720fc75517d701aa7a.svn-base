/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具维修记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-25 19:41:33                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsRepair                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-25 19:41:33
	/// 夹具维修记录表
	/// </summary>
	[Table("MES_TONGS_REPAIR")]
	public partial class MesTongsRepair
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 夹具ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_ID {get;set;}

		/// <summary>
		/// 操作记录信息ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_ID {get;set;}

		/// <summary>
		/// 维修结果，1正常，2报废
		/// </summary>
		[MaxLength(22)]
		public Decimal? REPAIR_RESULT {get;set;}

		/// <summary>
		/// 维修人员
		/// </summary>
		[MaxLength(30)]
		public String REPAIRER {get;set;}

		/// <summary>
		/// 维修开始时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime BEGIN_TIME {get;set;}

		/// <summary>
		/// 维修结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? END_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}


	}
}
