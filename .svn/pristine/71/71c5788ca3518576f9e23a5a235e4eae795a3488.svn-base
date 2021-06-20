/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-27 13:55:04                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtRoute                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-27 13:55:04
	/// 线别和机台的路由表
	/// </summary>
	[Table("SMT_ROUTE")]
	public partial class SmtRoute
	{
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(200)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(200)]
		public String DISABLE_BILL_ID {get;set;}
		/// <summary>
		/// 线别的ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}
		/// <summary>
		/// 机台的ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATION_ID {get;set;}
		/// <summary>
		/// 序号
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORDER_NO {get;set;}
		/// <summary>
		/// 是否可用
		/// </summary>
		[MaxLength(4)]
		public String ENABLED {get;set;}


	}
}
