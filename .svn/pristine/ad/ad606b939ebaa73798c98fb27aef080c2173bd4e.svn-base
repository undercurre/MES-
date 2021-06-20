/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 14:53:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsRuncardRanger                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-14 14:53:45
	/// 
	/// </summary>
	[Table("SFCS_RUNCARD_RANGER")]
	public partial class SfcsRuncardRanger
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_ID {get;set;}

		/// <summary>
		/// 开始流水号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SN_BEGIN {get;set;}

		/// <summary>
		/// 结束流水号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SN_END {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QUANTITY {get;set;}

		/// <summary>
		/// 进制
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DIGITAL {get;set;}

		/// <summary>
		/// 变化位数
		/// </summary>
		[MaxLength(22)]
		public Decimal? RANGE {get;set;}

		/// <summary>
		/// 固定头码
		/// </summary>
		[MaxLength(50)]
		public String FIX_HEADER {get;set;}

		/// <summary>
		/// 固定尾码
		/// </summary>
		[MaxLength(50)]
		public String FIX_TAIL {get;set;}

		/// <summary>
		/// 固定头位数
		/// </summary>
		[MaxLength(22)]
		public Decimal? HEADER_LENGTH {get;set;}

		/// <summary>
		/// 固定尾位数
		/// </summary>
		[MaxLength(22)]
		public Decimal? TAIL_LENGTH {get;set;}

		/// <summary>
		/// 是否已打印
		/// </summary>
		[MaxLength(1)]
		public String PRINTED {get;set;}

		/// <summary>
		/// 排除字符
		/// </summary>
		[MaxLength(50)]
		public String EXCLUSIVE_CHAR {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[MaxLength(22)]
		public Decimal STATUS { get; set; } = 1;

		/// <summary>
		/// 流水号范围规则ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? RANGER_RULE_ID {get;set;}


	}
}
