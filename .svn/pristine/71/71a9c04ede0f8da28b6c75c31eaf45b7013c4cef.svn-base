/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：周转箱打印表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-18 10:46:57                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBatchPring                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-18 10:46:57
	/// 周转箱打印表
	/// </summary>
	[Table("MES_BATCH_PRING")]
	public partial class MesBatchPring
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 批次管理表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BT_MANAGER_ID {get;set;}

		/// <summary>
		/// 周转条码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CODE {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QTY {get;set;}

		/// <summary>
		/// 周转箱编号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CARTON_NO {get;set;}

		/// <summary>
		/// 创建日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String CREATOR {get;set;}


	}
}
