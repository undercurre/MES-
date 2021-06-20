/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-28 10:51:42                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysPdaMenus                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-28 10:51:42
	/// 
	/// </summary>
	[Table("SYS_PDA_MENUS")]
	public partial class SysPdaMenus
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 菜单ID
		/// </summary>
		[MaxLength(200)]
		public String MENU_ID {get;set;}

		/// <summary>
		/// 菜单名称
		/// </summary>
		[MaxLength(200)]
		public String MENU_NAME {get;set;}

		/// <summary>
		/// 模块名称
		/// </summary>
		[MaxLength(200)]
		public String MODULE_NAME {get;set;}

		/// <summary>
		/// 说明
		/// </summary>
		[MaxLength(2000)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 参数信息
		/// </summary>
		[MaxLength(4000)]
		public String PARAM_INFO {get;set;}

		/// <summary>
		/// 顺序号
		/// </summary>
		[MaxLength(22)]
		public Decimal? ORDER_SEQ {get;set;}

		/// <summary>
		/// 页面名称
		/// </summary>
		[MaxLength(100)]
		public String FORM_NAME {get;set;}

		///// <summary>
		///// 权限ID字符串: 以,分隔
		///// </summary>
		//[MaxLength(2000)]
		//public string ROLES_STRING { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(50)]
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_DATE {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(50)]
		public String UPDATE_BY {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_DATE {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

	}
}
