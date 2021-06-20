/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接收者分组主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-12 14:28:29                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesMessageReceiverGroup                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-12 14:28:29
	/// 接收者分组主表
	/// </summary>
	[Table("MES_MESSAGE_RECEIVER_GROUP")]
	public partial class MesMessageReceiverGroup
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}

		/// <summary>
		/// 部门ID（默认为0  已无用）
		/// </summary>
		[MaxLength(50)]
		public String DEP_ID {get;set;}

		/// <summary>
		/// 分组名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String GROUP_NAME {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否激活（Y:是 N: 否）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 最后修改人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 最后修改时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime UPDATE_TIME {get;set;}


	}
}
