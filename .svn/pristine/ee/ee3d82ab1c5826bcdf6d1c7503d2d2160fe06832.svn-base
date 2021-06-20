/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-29 17:59:32                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsOperationSites                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-09-29 17:59:32
	/// 站位
	/// </summary>
	[Table("SFCS_OPERATION_SITES")]
	public partial class SfcsOperationSites
	{
		

		[Key]
		public Decimal ID {get;set;}

		[Required]
		[MaxLength(50)]
		public String ENABLE_BILL_ID {get;set;}

		[Required]
		[MaxLength(50)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 站位名
		/// </summary>
		[MaxLength(40)]
		public String OPERATION_SITE_NAME { get; set; }

		/// <summary>
		/// 线体ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_LINE_ID {get;set;}		

		/// <summary>
		/// 工序ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_ID {get;set;}

		[Required]
		[MaxLength(200)]
		public String DESCRIPTION {get;set;}

		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

	}
}
