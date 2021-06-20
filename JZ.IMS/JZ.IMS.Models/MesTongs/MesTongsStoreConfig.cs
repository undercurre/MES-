/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具储位表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-20 14:05:11                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsStoreConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-20 14:05:11
	/// 夹具储位表
	/// </summary>
	[Table("MES_TONGS_STORE_CONFIG")]
	public partial class MesTongsStoreConfig
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 储位编号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CODE {get;set;}

		/// <summary>
		/// 储位名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String NAME {get;set;}

		/// <summary>
		/// 储位类别
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TYPE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}

		/// <summary>
		/// 是否有效（Y:有效，N：无效）
		/// </summary>
		[MaxLength(1)]
		public String ENABLED { get; set; }
	}
}
