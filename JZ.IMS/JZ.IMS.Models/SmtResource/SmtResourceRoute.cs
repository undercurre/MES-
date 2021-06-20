/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-02-27 14:08:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtResourceRoute                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-02-27 14:08:53
	/// 
	/// </summary>
	[Table("SMT_RESOURCE_ROUTE")]
	public partial class SmtResourceRoute
	{
		/// <summary>
		/// 主键
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
		/// 名称(ID)
		/// </summary>
		[MaxLength(22)]
		public Decimal? OBJECT_ID {get;set;}

		/// <summary>
		/// 当前工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? CURRENT_OPERATION {get;set;}

		/// <summary>
		/// 下一道工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? NEXT_OPERATION {get;set;}

		/// <summary>
		/// 排序
		/// </summary>
		[MaxLength(22)]
		public Decimal? ORDER_NO {get;set;}

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
