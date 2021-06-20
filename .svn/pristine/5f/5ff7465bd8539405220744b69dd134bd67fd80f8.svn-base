/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 11:25:23                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsModel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 11:25:23
	/// 
	/// </summary>
	[Table("SFCS_MODEL")]
	public partial class SfcsModel
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? ID {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String MODEL {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[Required]
		[MaxLength(300)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

	}
}
