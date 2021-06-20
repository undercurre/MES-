/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理菜单                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-03-07 16:50:56                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：Menu                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// Admin
	/// 2019-03-07 16:50:56
	/// 后台管理菜单
	/// </summary>
	public partial class Sys_Menu
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public decimal Id {get;set;}

		/// <summary>
		/// 父菜单ID
		/// </summary>
		[Required]
		[MaxLength(10)]
		public decimal Parent_Id {get;set;}

		/// <summary>
		/// 菜单代码
		/// </summary>
		[Required]
		[MaxLength(32)]
		public String Menu_Code {get;set;}

		/// <summary>
		/// 菜单名称
		/// </summary>
		[MaxLength(128)]
		public String Menu_Name {get;set;}

		/// <summary>
		/// 菜单类型(1:菜单;2:按钮;)
		/// </summary>
		public int MENU_TYPE { get; set; }

		/// <summary>
		/// 图标地址
		/// </summary>
		[MaxLength(128)]
		public String Icon_Url {get;set;}

		/// <summary>
		/// 链接地址
		/// </summary>
		[MaxLength(128)]
		public String Link_Url {get;set;}

		/// <summary>
		/// 排序数字
		/// </summary>
		[MaxLength(10)]
		public decimal? Sort {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 是否系统默认
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String Is_System {get;set;}

		/// <summary>
		/// 添加人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public decimal Add_Manager_Id {get;set;}

		/// <summary>
		/// 添加时间
		/// </summary>
		[Required]
		[MaxLength(23)]
		public DateTime Add_Time {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(10)]
		public decimal? Modify_Manager_Id {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(23)]
		public DateTime? Modify_Time {get;set;}

		/// <summary>
		/// 是否删除
		/// </summary>
		[Required]
		[MaxLength(1)]
		public string Is_Delete {get;set;}

		/// <summary>
		/// 是否展开
		/// </summary>
		public string Spread { get; set; } = "N";

		/// <summary>
		/// 窗口打开方式
		/// </summary>
		public string Target { get; set; }

		/// <summary>
		/// 英文名称
		/// </summary>
		public string MENU_EN { get; set; }
		/// <summary>
		/// 排列数
		/// </summary>
		[MaxLength(10)]
		public decimal? COLUMNS { get; set; }
	}
}
