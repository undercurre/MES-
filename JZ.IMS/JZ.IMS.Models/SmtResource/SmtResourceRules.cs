/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-02-27 14:15:50                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtResourceRules                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-02-27 14:15:50
	/// 
	/// </summary>
	[Table("SMT_RESOURCE_RULES")]
	public partial class SmtResourceRules
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(50)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(50)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 名称
		/// </summary>
		[MaxLength(22)]
		public Decimal? OBJECT_ID {get;set;}

		/// <summary>
		/// 类型
		/// </summary>
		[MaxLength(22)]
		public Decimal? CATEGORY_ID {get;set;}

		/// <summary>
		/// 工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? ROUTE_OPERATION_ID {get;set;}

		/// <summary>
		/// 标准时间
		/// </summary>
		[MaxLength(22)]
		public Decimal? STANDARD_TIME {get;set;}

		/// <summary>
		/// 计算流程时间(分钟)
		/// </summary>
		[MaxLength(1)]
		public String STANDARD_FLAG {get;set;}

		/// <summary>
		/// 检验有效期
		/// </summary>
		[MaxLength(1)]
		public String VALID_FLAG {get;set;}

		/// <summary>
		/// 检验暴露期
		/// </summary>
		[MaxLength(1)]
		public String EXPOSE_FLAG {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE5 {get;set;}


	}
}
