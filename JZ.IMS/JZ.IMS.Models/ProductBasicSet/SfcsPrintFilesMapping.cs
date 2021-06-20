/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 10:41:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsPrintFilesMapping                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-14 10:41:48
	/// 
	/// </summary>
	[Table("SFCS_PRINT_FILES_MAPPING")]
	public partial class SfcsPrintFilesMapping
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 客户
		/// </summary>
		[MaxLength(22)]
		public Decimal? CUSTOMER_ID {get;set;}

		[MaxLength(22)]
		public Decimal? PRODUCT_FAMILY_ID {get;set;}

		/// <summary>
		/// Platform Name
		/// </summary>
		[MaxLength(22)]
		public Decimal? PLATFORM_ID {get;set;}

		/// <summary>
		/// JobCode
		/// </summary>
		[MaxLength(22)]
		public Decimal? JOBCODE_ID {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		[MaxLength(22)]
		public Decimal? MODEL_ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 文件名
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRINT_FILE_ID {get;set;}

		/// <summary>
		/// 打印工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? SITE_OPERATION_ID {get;set;}

		/// <summary>
		/// 自动打印
		/// </summary>
		[MaxLength(2)]
		public String AUTO_PRINT_FLAG {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
