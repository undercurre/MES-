/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：平板菜单权限表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-30 09:43:44                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesPadClientMenus                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-30 09:43:44
	/// 平板菜单权限表
	/// </summary>
	[Table("MES_PAD_CLIENT_MENUS")]
	public partial class MesPadClientMenus
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 类型 默认0 菜单 1 按钮
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MENU_TYPE {get;set;}

		/// <summary>
		/// 上级ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal P_ID {get;set;}

		/// <summary>
		/// 层级关系
		/// </summary>
		[MaxLength(510)]
		public String P_ID_ARR {get;set;}

		/// <summary>
		/// 页面路径
		/// </summary>
		[MaxLength(255)]
		public String PAGE_URL {get;set;}

		/// <summary>
		/// 链接地址
		/// </summary>
		[MaxLength(255)]
		public String LINK_URL {get;set;}

		/// <summary>
		/// 链接地址
		/// </summary>
		[MaxLength(255)]
		public String ICON_URL {get;set;}

		/// <summary>
		/// 调用代码
		/// </summary>
		[MaxLength(255)]
		public String MENU_CODE {get;set;}

		/// <summary>
		/// 菜单名称(页面名称)
		/// </summary>
		[MaxLength(255)]
		public String MENU_NAME {get;set;}

		/// <summary>
		/// 英文名
		/// </summary>
		[MaxLength(255)]
		public String MENU_EN {get;set;}

		/// <summary>
		/// 是否激活（Y:是 N: 否）
		/// </summary>
		[MaxLength(10)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 排序数字
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SORT {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(255)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(255)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}


	}
}
