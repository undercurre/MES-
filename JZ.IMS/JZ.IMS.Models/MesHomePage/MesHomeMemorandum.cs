/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：备忘录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-11 14:55:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesHomeMemorandum                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-11 14:55:53
	/// 备忘录表
	/// </summary>
	[Table("MES_HOME_MEMORANDUM")]
	public partial class MesHomeMemorandum
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 用户ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal USER_ID {get;set;}

		/// <summary>
		/// 日期
		/// </summary>
		[Required]
		[MaxLength(25)]
		public String TAR_DATE {get;set;}

		/// <summary>
		/// 标题
		/// </summary>
		[Required]
		[MaxLength(60)]
		public String TITLE {get;set;}

		/// <summary>
		/// 标签
		/// </summary>
		[Required]
		[MaxLength(60)]
		public String TAG {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[Required]
		[MaxLength(255)]
		public String REMARK {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(25)]
		public String CRATED_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(25)]
		public String CREATED_BY {get;set;}

		/// <summary>
		/// 显示状态 1 表示显示 0 隐藏
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 软删除标记 1 正常 0 已删除
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal FLAG {get;set;}


	}
}
