/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：收藏夹表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-28 15:48:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysFavorites                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-28 15:48:45
	/// 收藏夹表
	/// </summary>
	[Table("SYS_FAVORITES")]
	public partial class SysFavorites
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 菜单ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MENUM_ID {get;set;}

		/// <summary>
		/// 菜单地址
		/// </summary>
		[MaxLength(50)]
		public String MENUM_PATH {get;set;}

		/// <summary>
		/// 用户ID
		/// </summary>
		[MaxLength(50)]
		public String USER_ID {get;set;}

		/// <summary>
		/// 使用有效，Y:有效，N:无效
		/// </summary>
		[MaxLength(1)]
		public String ENABLE {get;set;}

		/// <summary>
		/// 排序号
		/// </summary>
		[MaxLength(22)]
		public Decimal? SORT_NUM {get;set;}


	}
}
