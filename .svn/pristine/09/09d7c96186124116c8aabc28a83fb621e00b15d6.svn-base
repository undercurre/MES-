/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-08 17:33:20                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsRouteConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-08 17:33:20
	/// 
	/// </summary>
	[Table("SFCS_ROUTE_CONFIG")]
	public partial class SfcsRouteConfig
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal ROUTE_ID {get;set;}

		/// <summary>
		/// 唯一作业标识
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRODUCT_OPERATION_CODE {get;set;}

		/// <summary>
		/// 当前工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CURRENT_OPERATION_ID {get;set;}

		/// <summary>
		/// 前一道工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PREVIOUS_OPERATION_ID {get;set;}

		/// <summary>
		/// 下一道工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal NEXT_OPERATION_ID {get;set;}

		/// <summary>
		/// 维修工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal REPAIR_OPERATION_ID {get;set;}

		/// <summary>
		/// 返工工序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal REWORK_OPERATION_ID {get;set;}

		/// <summary>
		/// 序号
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORDER_NO {get;set;}


	}
}
