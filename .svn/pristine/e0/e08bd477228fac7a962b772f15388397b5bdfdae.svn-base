/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：组织类型表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-05 09:15:05                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysOrganizeType                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-05 09:15:05
	/// 组织类型表
	/// </summary>
	[Table("SYS_ORGANIZE_TYPE")]
	public partial class SysOrganizeType
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 组织类型名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ORGANIZE_TYPE_NAME {get;set;}

		/// <summary>
		/// 组织类型代码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ORGANIZE_TYPE_CODE {get;set;}

		/// <summary>
		/// 序号
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORDER_ID {get;set;}

		/// <summary>
		/// 启用
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(500)]
		public String REMARK {get;set;}


	}
}
