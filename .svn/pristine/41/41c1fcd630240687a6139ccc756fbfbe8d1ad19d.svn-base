/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：首五件检验事项                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-11 14:51:23                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesFirstCheckItems                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-11 14:51:23
	/// 首五件检验事项
	/// </summary>
	[Table("MES_FIRST_CHECK_ITEMS")]
	public partial class MesFirstCheckItems
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}

		/// <summary>
		/// 检验类别
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CHECK_TYPE {get;set; }

		/// <summary>
		/// 检验项目
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String CHECK_ITEM {get;set;}

		/// <summary>
		/// 检验依据
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String CHECK_GIST {get;set;}

		/// <summary>
		/// 排序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORDER_NO {get;set;}

		/// <summary>
		/// 是否有效（Y/N）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime UPDATE_TIME {get;set;}

		/// <summary>
		/// 最后更新人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 属性1
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE1 {get;set;}

		/// <summary>
		/// 属性2
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE2 {get;set;}

		/// <summary>
		/// 属性3
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE3 {get;set;}

		/// <summary>
		/// 属性4
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE4 {get;set;}

		/// <summary>
		/// 属性5
		/// </summary>
		[MaxLength(100)]
		public String ATTRIBUTE5 {get;set; }

		/// <summary>
		/// 检验类别名称
		/// </summary>
		[NotMapped]
		public String CHECK_TYPE_NAME { get; set; }
	}
}
