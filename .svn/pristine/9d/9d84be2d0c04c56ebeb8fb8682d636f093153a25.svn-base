/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：瑞德镭雕机流水号规则表（复杂流程）                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-08 18:46:06                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsLaserRangerRules                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-08 18:46:06
	/// 瑞德镭雕机流水号规则表（复杂流程）
	/// </summary>
	[Table("SFCS_LASER_RANGER_RULES")]
	public partial class SfcsLaserRangerRules
	{
		/// <summary>
		/// 表ID，序列MES_SEQ_ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号 SFCS_WO.WO_NO
		/// </summary>
		[MaxLength(20)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 前导符
		/// </summary>
		[MaxLength(50)]
		public String FIX_HEADER {get;set;}

		/// <summary>
		/// 结束符
		/// </summary>
		[MaxLength(50)]
		public String FIX_TAIL {get;set;}

		/// <summary>
		/// 流水范围长度
		/// </summary>
		[MaxLength(22)]
		public Decimal? RANGE_LENGTH {get;set;}

		/// <summary>
		/// 流水范围开始字符
		/// </summary>
		[MaxLength(100)]
		public String RANGE_START_CODE {get;set;}

		/// <summary>
		/// 进制 SFCS_PARAMETERS RADIX_TYPE
		/// </summary>
		[MaxLength(22)]
		public Decimal? DIGITAL {get;set;}

		/// <summary>
		/// 不包括字符 SFCS_PARAMETERS RADIX_EXCLUSIVE
		/// </summary>
		[MaxLength(50)]
		public String EXCLUSIVE_CHAR {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(50)]
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		[MaxLength(50)]
		public String UPDATE_BY {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE5 {get;set;}


	}
}
