/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员角色                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-03-07 16:50:56                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ManagerRole                                     
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
	/// 后台管理员角色
	/// </summary>
	public partial class Sys_Manager_Role
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public decimal Id {get;set;}

		/// <summary>
		/// 角色名称
		/// </summary>
		[Required]
		[MaxLength(64)]
		public String Role_Name {get;set;}

		/// <summary>
		/// 角色类型1超管2系管
		/// </summary>
		[Required]
		[MaxLength(10)]
		public decimal Role_Type {get;set;}

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
		public String Is_Delete {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(128)]
		public String Remark {get;set;}

    }
}
