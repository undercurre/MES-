/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：组织部门表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-02 14:29:09                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysDepartment                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-02 14:29:09
	/// 组织部门表
	/// </summary>
	[Table("SYS_DEPARTMENT")]
	public partial class SysDepartment
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}

		/// <summary>
		/// 部门名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String DEP_NAME {get;set;}

		/// <summary>
		/// 启用（Y/N）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal ORDER_ID {get;set;}

		/// <summary>
		/// 是否面向客户管理
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE5 {get;set;}


	}
}
