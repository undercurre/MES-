/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：工序                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-23 11:12:27                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsOperations                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-09-23 11:12:27
	/// 工序
	/// </summary>
	[Table("SFCS_OPERATIONS")]
	public partial class SfcsOperations
	{
		/// <summary>
		/// 工序ID
		/// </summary>
		[Key]
		public Decimal Id { get; set; }

		/// <summary>
		/// 工序名称
		/// </summary>
		[MaxLength(30)]
		public String OPERATION_NAME { get; set; }

		/// <summary>
		/// 工序描述
		/// </summary>
		[Required]
		[MaxLength(60)]
		public String DESCRIPTION { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal OPERATION_CLASS { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[NotMapped]
		public decimal OPERATION_CLASS_NAME { get; set; }

		/// <summary>
		/// 工序类别
		/// </summary>
		public decimal OPERATION_CATEGORY { get; set; }

		[NotMapped]
		public String OPERATION_CATEGORY_NAME { get; set; }

		/// <summary>
		/// 是否激活
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED { get; set; }
	}
}
