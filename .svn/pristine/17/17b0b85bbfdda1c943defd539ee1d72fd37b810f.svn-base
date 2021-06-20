/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：镭雕完成异常日志记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-23 09:24:04                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsLaserExceptionLog                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-23 09:24:04
	/// 镭雕完成异常日志记录表
	/// </summary>
	[Table("SFCS_LASER_EXCEPTION_LOG")]
	public partial class SfcsLaserExceptionLog
	{
		/// <summary>
		/// 表ID，序列SFCS_LASER_EXCEPTION_LOG_SEQ
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 关联SFCS_LASER_RANGER.ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LASER_RANGER_ID {get;set;}

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
		/// 是否无效（Y/N）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String IS_INVALID {get;set;}

		/// <summary>
		/// 无效时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime INVALID_TIME {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATETIME {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE5 {get;set;}


	}
}
