/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：连板拆板头部信息                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-23 09:58:09                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtMultipanelHeader                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-23 09:58:09
	/// 连板拆板头部信息
	/// </summary>
	[Table("SMT_MULTIPANEL_HEADER")]
	public partial class SmtMultipanelHeader
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 连板批次
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String BATCH_NO {get;set;}

		/// <summary>
		/// 连板站点ID
		/// </summary>
		[MaxLength(50)]
		public String MULT_SITE_ID {get;set;}

		/// <summary>
		/// 连板站点名称
		/// </summary>
		[MaxLength(50)]
		public String MULT_SITE_NAME {get;set;}

		/// <summary>
		/// 连板操作员
		/// </summary>
		[MaxLength(50)]
		public String MULT_OPERATOR {get;set;}

		/// <summary>
		/// 连板时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? MULT_TIME {get;set;}

		/// <summary>
		/// 拆板站点ID
		/// </summary>
		[MaxLength(50)]
		public String SPLIT_SITE_ID {get;set;}

		/// <summary>
		/// 拆板站点名称
		/// </summary>
		[MaxLength(50)]
		public String SPLIT_SITE_NAME {get;set;}

		/// <summary>
		/// 拆板操作员
		/// </summary>
		[MaxLength(50)]
		public String SPLIT_OPERATOR {get;set;}

		/// <summary>
		/// 拆板时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? SPLIT_TIME {get;set;}

		/// <summary>
		/// 是否拆板(默认为否)
		/// </summary>
		[MaxLength(10)]
		public String IS_SPLIT {get;set;}

		/// <summary>
		/// 连板数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? MULT_NUMBER {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(50)]
		public String WO_NO {get;set;}


	}
}
