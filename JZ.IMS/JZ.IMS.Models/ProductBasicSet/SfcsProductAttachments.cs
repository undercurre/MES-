/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-01 09:20:16                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductAttachments                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-01 09:20:16
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_ATTACHMENTS")]
	public partial class SfcsProductAttachments
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 主表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_OBJECT_ID {get;set;}

		/// <summary>
		/// 附件名称
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ATTACHMENT_ID {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		[MaxLength(300)]
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 固定值
		/// </summary>
		[MaxLength(20)]
		public String FIX_VALUE {get;set;}

		/// <summary>
		/// 附件数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? ATTACHMENT_QTY {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
