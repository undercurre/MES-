/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：镭雕机流水号范围表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-23 08:51:00                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsLaserRanger                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-23 08:51:00
	/// 镭雕机流水号范围表
	/// </summary>
	[Table("SFCS_LASER_RANGER")]
	public partial class SfcsLaserRanger
	{
		/// <summary>
		/// 表ID，序列SFCS_LASER_RANGER_SEQ
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单ID SFCS_WO.ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal WO_ID {get;set;}

		/// <summary>
		/// 起始流水号
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
		/// 进制 SFCS_PARAMETERS RADIX_TYPE
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
		/// 排除字符 SFCS_PARAMETERS RADIX_EXCLUSIVE
		/// </summary>
		[MaxLength(50)]
		public String EXCLUSIVE_CHAR {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		/// <summary>
		/// 流水号范围规则ID SFCS_RUNCARD_RANGER_RULES.ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? RANGER_RULE_ID {get;set;}

		/// <summary>
		/// AI工单号
		/// </summary>
		[MaxLength(30)]
		public String ATTRIBUTE2 {get;set;}

		/// <summary>
		/// SMT工单号
		/// </summary>
		[MaxLength(30)]
		public String ATTRIBUTE3 {get;set;}

		/// <summary>
		/// RI工单号
		/// </summary>
		[MaxLength(30)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE5 {get;set;}

		/// <summary>
		/// 关联SFCS_RUNCARD_RANGER.ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal RUNCARD_RANGER_ID {get;set;}

		/// <summary>
		/// 最后一次镭雕完成的结束流水号
		/// </summary>
		[MaxLength(50)]
		public String LAST_LASER_SN {get;set;}


	}
}
