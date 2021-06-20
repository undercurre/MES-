/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-11 14:14:44                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtStencilCleanHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-11 14:14:44
	/// 
	/// </summary>
	[Table("SMT_STENCIL_CLEAN_HISTORY")]
	public partial class SmtStencilCleanHistory
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 钢网编号
		/// </summary>
		[MaxLength(30)]
		public String STENCIL_NO {get;set;}

		/// <summary>
		/// 操作产线
		/// </summary>
		[MaxLength(40)]
		public String OPERATION_LINE_NAME {get;set;}

		/// <summary>
		/// 列印次数
		/// </summary>
		[MaxLength(22)]
		public Decimal? PRINT_COUNT {get;set;}

		/// <summary>
		/// 清洗及检查人
		/// </summary>
		[MaxLength(20)]
		public String CLEAN_USER {get;set;}

		/// <summary>
		/// 稽核人
		/// </summary>
		[MaxLength(20)]
		public String INSPECT_USER {get;set;}

		/// <summary>
		/// 稽核结果
		/// </summary>
		[MaxLength(200)]
		public String INSPECT_RESULT {get;set;}

		/// <summary>
		/// 上一次清洗时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CLEAN_TIME {get;set;}

		/// <summary>
		/// 下一次清洗时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? NEXT_CLEAN_TIME {get;set;}

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

		/// <summary>
		/// 张点力 上
		/// </summary>
		[MaxLength(22)]
		public Decimal? TENSION_A {get;set;}

		/// <summary>
		/// 张点力 下
		/// </summary>
		[MaxLength(22)]
		public Decimal? TENSION_B {get;set;}

		/// <summary>
		/// 张点力 左
		/// </summary>
		[MaxLength(22)]
		public Decimal? TENSION_C {get;set;}

		/// <summary>
		/// 张点力 右
		/// </summary>
		[MaxLength(22)]
		public Decimal? TENSION_D {get;set;}

		/// <summary>
		/// 张点力 中
		/// </summary>
		[MaxLength(22)]
		public Decimal? TENSION_E {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal STENCIL_TYPE {get;set;}


	}
}
