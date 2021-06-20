/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 10:29:47                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBurnFileManager                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-07-22 10:29:47
	/// 
	/// </summary>
	[Table("MES_BURN_FILE_MANAGER")]
	public partial class MesBurnFileManager
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 文件管理编码
		/// </summary>
		[MaxLength(50)]
		public String CODE {get;set;}

		/// <summary>
		/// 类型(上传程序还是本地程序)
		/// </summary>
		[MaxLength(22)]
		public Decimal? TYPE {get;set;}

		/// <summary>
		/// 路径
		/// </summary>
		[MaxLength(200)]
		public String PATH {get;set;}

		/// <summary>
		/// 文件字(不含后缀)可能后缀.bin
		/// </summary>
		[MaxLength(50)]
		public String FILENAME {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String REMARK { get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		


	}
}
