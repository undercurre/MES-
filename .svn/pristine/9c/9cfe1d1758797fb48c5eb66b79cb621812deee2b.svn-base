/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：版本提交表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-11 14:54:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesCommitVersion                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-11 14:54:38
	/// 版本提交表
	/// </summary>
	[Table("MES_COMMIT_VERSION")]
	public partial class MesCommitVersion
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 标题
		/// </summary>
		[Required]
		[MaxLength(255)]
		public String TITLE {get;set;}

		/// <summary>
		/// 简评
		/// </summary>
		[Required]
		[MaxLength(520)]
		public String SORT_DESC {get;set;}

		/// <summary>
		/// 内容（富文本）
		/// </summary>
		[Required]
		[MaxLength(4000)]
		public String CONTENT {get;set;}

		/// <summary>
		/// 标签
		/// </summary>
		[Required]
		[MaxLength(25)]
		public String TAG {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(25)]
		public String CRATED_TIME {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		[Required]
		[MaxLength(25)]
		public String UPDATED_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(25)]
		public String CREATED_BY {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		[Required]
		[MaxLength(25)]
		public String UPDATED_BY {get;set;}

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
