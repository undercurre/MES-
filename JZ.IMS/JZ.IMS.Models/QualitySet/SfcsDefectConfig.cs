/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 17:24:04                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsDefectConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 17:24:04
	/// 
	/// </summary>
	[Table("SFCS_DEFECT_CONFIG")]
	public partial class SfcsDefectConfig
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 不良代码
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String DEFECT_CODE {get;set;}

		/// <summary>
		/// 不良类型
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DEFECT_TYPE {get;set;}

		/// <summary>
		/// 不良种类
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DEFECT_CLASS {get;set;}

		/// <summary>
		/// 不良类别
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DEFECT_CATEGORY {get;set;}

		/// <summary>
		/// 不良等级
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LEVEL_CODE {get;set;}

		/// <summary>
		/// 不良来源
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String SOURCE {get;set;}

		/// <summary>
		/// 英文描述
		/// </summary>
		[Required]
		[MaxLength(300)]
		public String DEFECT_DESCRIPTION {get;set;}

		/// <summary>
		/// 中文描述
		/// </summary>
		[MaxLength(300)]
		public String CHINESE_DESCRIPTION {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

	}
}
