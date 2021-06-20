/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:48
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_CONFIG")]
	public partial class SfcsProductConfig
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(50)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(50)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 配置类型
		/// </summary>
		[MaxLength(22)]
		public Decimal? CONFIG_TYPE {get;set;}

		/// <summary>
		/// 配置值
		/// </summary>
		[Required]
		[MaxLength(300)]
		public String CONFIG_VALUE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		[MaxLength(45)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(45)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(45)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(45)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(45)]
		public String ATTRIBUTE5 {get;set;}


	}
}
