/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-08 09:05:26                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsRoutes                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-08 09:05:26
	/// 
	/// </summary>
	[Table("SFCS_ROUTES")]
	public partial class SfcsRoutes
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(20)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 制程名称
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String ROUTE_NAME {get;set;}

		/// <summary>
		/// 厂部
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ROUTE_CLASS {get;set;}

		/// <summary>
		/// 类型
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ROUTE_TYPE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(60)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
