/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-14 10:13:19                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysProjApiParm                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-09-14 10:13:19
	/// 
	/// </summary>
	[Table("SYS_PROJ_API_PARM")]
	public partial class SysProjApiParm
	{
		[Required]
		[MaxLength(22)]
		public Decimal MST_RID { get;set;}

		/// <summary>
		/// 参数名
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String PARM_NAME {get;set;}

		/// <summary>
		/// 参数名
		/// </summary>
		[MaxLength(200)]
		public String PARM_VALUE { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否必填,Y/N
		/// </summary>
		[Required]
		[MaxLength(2)]
		public String REQUIRED {get;set;}


	}
}
