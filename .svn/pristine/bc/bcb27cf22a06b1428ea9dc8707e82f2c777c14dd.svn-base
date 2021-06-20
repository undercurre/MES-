/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-22 19:19:39                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesQualityItems                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-22 19:19:39
	/// 
	/// </summary>
	[Table("MES_QUALITY_ITEMS")]
	public partial class MesQualityItems
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
		/// 检验类别名称
		/// </summary>
		[NotMapped]
		[MaxLength(100)]
		public string CHECK_TYPE_NAME { get; set; }

		/// <summary>
		/// 检验项目
		/// </summary>
		[Required]
		[MaxLength(500)]
		public String CHECK_ITEM { get; set; }

		/// <summary>
		/// 检验描述
		/// </summary>
		[MaxLength(100)]
		public String CHECK_DESC {get;set;}

		/// <summary>
		/// 有无量化标准，1：有量化，2：无量化
		/// </summary>
		[Required]
		[MaxLength(2)]
		public String QUANTIZE_TYPE {get;set;}

		/// <summary>
		/// 是否可空
		/// </summary>
		[Required]
		[MaxLength(2)]
		public String ISEMPTY {get;set;}

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
		[MaxLength(2)]
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
		/// 组织架构
		/// </summary>
		[NotMapped]
        public String ORGANIZE_NAME { get; set; }
    }
}
