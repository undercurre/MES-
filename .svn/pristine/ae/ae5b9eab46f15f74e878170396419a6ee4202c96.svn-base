/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-17 17:10:09                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsStoplineMaintainHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-17 17:10:09
	/// 
	/// </summary>
	[Table("SFCS_STOPLINE_MAINTAIN_HISTORY")]
	public partial class SfcsStoplineMaintainHistory
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 停线历史ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? STOPLINE_HISTORY_ID {get;set;}

		/// <summary>
		/// 原因分析
		/// </summary>
		[MaxLength(1000)]
		public String ROOT_CAUSE {get;set;}

		/// <summary>
		/// 解决方案
		/// </summary>
		[MaxLength(1000)]
		public String SOLUTION {get;set;}

		/// <summary>
		/// 责任归属
		/// </summary>
		[MaxLength(1000)]
		public String RESPONSIBILITY {get;set;}

		/// <summary>
		/// 状态(新增时为0, 当状态为2(即Closed)时不能修改)
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAINTAIN_STATUS {get;set;}

		/// <summary>
		/// 维护人
		/// </summary>
		[MaxLength(20)]
		public String MAINTAIN_BY {get;set;}

		/// <summary>
		/// 维护时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? MAINTAIN_TIME {get;set;}

	}
}
