/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-23 09:21:54                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ImportDtl                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-23 09:21:54
	/// 导入明细
	/// </summary>
	[Table("IMPORT_DTL")]
	public partial class ImportDtl
	{
		/// <summary>
		/// 主键id
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 主表id
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MST_ID {get;set;}

		/// <summary>
		/// 项目名称
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String COLUMN_NAME {get;set;}

		/// <summary>
		/// 项目标题
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String COLUMN_CAPTION {get;set;}

		/// <summary>
		/// 是否唯一(1,0)
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal IS_UNIQUE {get;set;}

		/// <summary>
		/// 是否可空值(1,0)
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ISNULL_ABLE {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(50)]
		public String UPDATE_BY {get;set;}

		/// <summary>
		/// 修改日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime UPDATE_TIME {get;set;}

		/// <summary>
		/// Excel栏位
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String EXCEL_ITEM {get;set;}

		/// <summary>
		/// 相关联SQL语句
		/// </summary>
		[MaxLength(2000)]
		public String REFERENCE_SQL {get;set;}

		/// <summary>
		/// 验证列表SQL语句
		/// </summary>
		[MaxLength(2000)]
		public String LISTVALIDATION_SQL { get; set; }
	}
}
