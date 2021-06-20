/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 11:58:55                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductMultiRuncard                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-03 11:58:55
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_MULTI_RUNCARD")]
	public partial class SfcsProductMultiRuncard
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_UNITAGE {get;set;}

		/// <summary>
		/// 连板工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINK_OPERATION_CODE {get;set;}

		/// <summary>
		/// 拆板工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BREAK_OPERATION_CODE {get;set;}

		[Required]
		[MaxLength(1)]
		public String WHOLE_FAIL_FLAG {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
