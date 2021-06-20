/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-10 19:51:17                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsPrintFiles                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-10 19:51:17
	/// 
	/// </summary>
	[Table("SFCS_PRINT_FILES")]
	public partial class SfcsPrintFiles
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 文件名
		/// </summary>
		[MaxLength(100)]
		public String FILE_NAME {get;set;}

		/// <summary>
		/// 上传文件
		/// </summary>
		[MaxLength(4000)]
		public Byte[] FILE_CONTENT {get;set;}

		/// <summary>
		/// 标签类型
		/// </summary>
		[MaxLength(22)]
		public Decimal? LABEL_TYPE {get;set;}

		/// <summary>
		/// 标签图片
		/// </summary>
		[MaxLength(4000)]
		public Byte[] LABEL_IMAGE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(500)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 文件类型
		/// </summary>
		[MaxLength(10)]
		public String FILE_TYPE {get;set;}

		/// <summary>
		/// 源文件名
		/// </summary>
		[MaxLength(100)]
		public String ORIGINAL_FILE_NAME {get;set;}

		/// <summary>
		/// 版本日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? FILE_VERSION_DATE {get;set;}


	}
}
