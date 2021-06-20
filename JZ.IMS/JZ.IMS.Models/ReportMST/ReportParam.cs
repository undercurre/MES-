/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：报表自定义SQL-语句表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 16:19:07                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ReportParam                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-07-22 16:19:07
	/// 报表自定义SQL-语句表
	/// </summary>
	[Table("REPORT_PARAM")]
	public partial class ReportParam
	{
		/// <summary>
		/// 参数名
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String PARAM_NAME { get;set;}

		/// <summary>
		/// 参数类型
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String DATA_TYPE { get;set;}

		/// <summary>
		/// 默认值
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PARAM_VALUE { get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(100)]
		public String REMARK { get;set;}

		/// <summary>
		/// Report_MST的ID
		/// </summary>
		[MaxLength(7)]
		public Decimal MST_ID { get;set;}

		/// <summary>
		/// 操作类型：1：等于，2：大于，3：大于等于，4：小于，5：小于等于，6：模糊搜索
		/// </summary>
		[MaxLength(100)]
		public Decimal Action_Type { get; set; }
	}
}
