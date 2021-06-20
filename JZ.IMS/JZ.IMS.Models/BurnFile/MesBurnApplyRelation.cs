/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-23 15:07:44                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBurnApplyRelation                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-07-23 15:07:44
	/// 
	/// </summary>
	[Table("MES_BURN_APPLY_RELATION")]
	public partial class MesBurnApplyRelation
	{
		/// <summary>
		/// 主键 
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 文件申请主键
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal APPLY_ID {get;set;}

		/// <summary>
		/// 烧录文件主键
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BURN_FILE_ID {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[MaxLength(22)]
		public Decimal? STATUS {get;set;}

		/// <summary>
		/// 用户名字
		/// </summary>
		[MaxLength(255)]
		public String USER_NAME {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? MODIFY_TIME {get;set;}

		


	}
}
