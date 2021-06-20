/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 10:37:42                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBurnFileDownHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-07-22 10:37:42
	/// 
	/// </summary>
	[Table("MES_BURN_FILE_DOWN_HISTORY")]
	public partial class MesBurnFileDownHistory
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 主表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MST_ID {get;set;}

		/// <summary>
		/// 文件名
		/// </summary>
		[MaxLength(50)]
		public String FILE_NAME {get;set;}

		/// <summary>
		/// 文件大小
		/// </summary>
		[MaxLength(50)]
		public String FILE_LEN {get;set;}

		/// <summary>
		/// 文件类型(文件扩展名)
		/// </summary>
		[MaxLength(255)]
		public String FILE_TYPE {get;set;}

		/// <summary>
		/// 文件创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? FILE_TIME {get;set;}

		


	}
}
