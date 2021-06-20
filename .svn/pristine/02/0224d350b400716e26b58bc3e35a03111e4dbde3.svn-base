/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限表                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-03-07 16:50:56                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：RolePermission                                     
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
	/// 角色权限表
	/// </summary>
	public partial class Sys_Role_Permission
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public decimal Id { get; set; }

		/// <summary>
		/// 角色主键
		/// </summary>
		[Required]
		[MaxLength(10)]
		public decimal Role_Id { get; set; }

		/// <summary>
		/// 菜单主键
		/// </summary>
		[Required]
		[MaxLength(10)]
		public decimal Menu_Id { get; set; }
	}
}
