/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-06-15 10:42:16                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ImsMsdR12                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-06-15 10:42:16
	/// 
	/// </summary>
	[Table("IMS_MSD_R12")]
	public partial class ImsMsdR12
	{
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(6)]
		public String ORGANIZATION_CODE {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(80)]
		public String PART_CODE {get;set;}

		/// <summary>
		/// 料号描述
		/// </summary>
		[MaxLength(480)]
		public String DESCRIPTION {get;set;}

		[MaxLength(20)]
		public String MAKER_CODE {get;set;}

		/// <summary>
		/// 等级
		/// </summary>
		[MaxLength(10)]
		public String LEVEL_CODE {get;set;}

		/// <summary>
		/// 厚度
		/// </summary>
		[MaxLength(22)]
		public Decimal? PART_THICKNESS {get;set;}

		[MaxLength(60)]
		public String MSD_REEL {get;set;}

		[MaxLength(508)]
		public String MSD_SPECIAL {get;set;}

		[MaxLength(40)]
		public String MSD_TIME {get;set;}

		[MaxLength(60)]
		public String MSD_TRAY {get;set;}

		[Required]
		[MaxLength(7)]
		public DateTime CREATION_DATE {get;set;}

		[Required]
		[MaxLength(7)]
		public DateTime LAST_UPDATE_DATE {get;set;}

		/// <summary>
		/// 是否可用
		/// </summary>
		[MaxLength(5)]
		public String ENABLED {get;set;}


	}
}
