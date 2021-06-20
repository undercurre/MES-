/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：组织架构表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-05 11:05:54                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysOrganize                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-05 11:05:54
	/// 组织架构表
	/// </summary>
	[Table("SYS_ORGANIZE")]
	public partial class SysOrganize
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		[MaxLength(22)]
		public decimal ID {get;set;}

		/// <summary>
		/// 组织名称
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String ORGANIZE_NAME {get;set;}

		/// <summary>
		/// 组织类型ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORGANIZE_TYPE_ID {get;set;}

		/// <summary>
		/// 上级组织ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PARENT_ORGANIZE_ID {get;set;}

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

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String MODIFY_BY {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime MODIFY_TIME {get;set;}

	}
}
