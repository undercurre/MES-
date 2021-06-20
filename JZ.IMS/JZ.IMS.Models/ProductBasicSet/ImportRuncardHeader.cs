/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：客户SN表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-19 16:20:59                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ImportRuncardHeader                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-19 16:20:59
	/// 客户SN表主表
	/// </summary>
	[Table("IMPORT_RUNCARD_HEADER")]
	public partial class ImportRuncardHeader
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(20)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(10)]
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		[MaxLength(1)]
		public String ENABLE {get;set;}

		/// <summary>
		/// 导入SN的数量
		/// </summary>
		[MaxLength(22)]
		public int SN_QTY { get; set; } = 0;


	}
}
