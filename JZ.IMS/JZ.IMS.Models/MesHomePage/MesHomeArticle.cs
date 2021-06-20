/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：文章表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-11 14:55:22                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesHomeArticle                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-11 14:55:22
	/// 文章表
	/// </summary>
	[Table("MES_HOME_ARTICLE")]
	public partial class MesHomeArticle
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 类型 0 默认值 1 通知公告 2 预警信息 3 待办事项
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TYPE {get;set;}

		/// <summary>
		/// 目标类型 0 默认值（默认是组织架构为单位目标） 1 个人
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TARGET_TYPE {get;set;}

		/// <summary>
		/// 目标 [1, 2, 3, 4, 5] 序列化 JSON 数据，查询的时候 WHERE X IN TARGET
		/// </summary>
		[Required]
		[MaxLength(1024)]
		public String TARGET {get;set;}

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


		/// <summary>
		/// 是否置顶 0 不置顶 1 置顶
		/// </summary>
		[MaxLength(22)]
		public Decimal IS_TOP { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		[MaxLength(22)]
		public Decimal SORT { get; set; }

	}
}
