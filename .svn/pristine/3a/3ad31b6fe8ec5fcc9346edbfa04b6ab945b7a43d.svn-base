/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：员工信息表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-23 23:28:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysEmployee                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-23 23:28:38
	/// 员工信息表
	/// </summary>
	[Table("SYS_EMPLOYEE")]
	public partial class SysEmployee
	{
		/// <summary>
		/// 员工工号
		/// </summary>
		[Key]
		public String USER_ID {get;set;}

		/// <summary>
		/// 员工姓名
		/// </summary>
		[MaxLength(200)]
		public String USER_NAME {get;set;}

		/// <summary>
		/// 员工照片
		/// </summary>
		[Required]
		[MaxLength(4000)]
		public Byte[] PHOTO {get;set;}


	}
}
