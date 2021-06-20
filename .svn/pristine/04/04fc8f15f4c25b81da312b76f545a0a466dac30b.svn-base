/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：组织架构人员关联表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-06 09:12:32                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysUserOrganize                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-06 09:12:32
	/// 组织架构人员关联表
	/// </summary>
	[Table("SYS_USER_ORGANIZE")]
	public partial class SysUserOrganize
	{
		/// <summary>
		/// 唯一标识符合
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 人员ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MANAGER_ID {get;set;}

		/// <summary>
		/// 组织架构ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORGANIZE_ID {get;set;}

		/// <summary>
		/// 开始时间（创建时间）
		/// </summary>
		[MaxLength(7)]
		public DateTime? START_DATE {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(10)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 失效时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? END_DATE {get;set;}

		/// <summary>
		/// 状态,'Y':表示有效;'N':表示无效
		/// </summary>
		[MaxLength(1)]
		public String STATUS {get;set;}
	}
}
