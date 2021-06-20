/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认配置表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-24 17:23:47                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesProductionPreConf                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-24 17:23:47
	/// 产前确认配置表
	/// </summary>
	[Table("MES_PRODUCTION_PRE_CONF")]
	public partial class MesProductionPreConf
	{
		/// <summary>
		/// 唯一标识符
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 确认项目
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CONTENT_TYPE {get;set;}

		/// <summary>
		/// 确认内容
		/// </summary>
		[Required]
		[MaxLength(1000)]
		public String CONTENT {get;set;}

		/// <summary>
		/// 确认标准
		/// </summary>
		[MaxLength(1000)]
		public String CONFIRM_CONTENT {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人员
		/// </summary>
		[MaxLength(10)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 工厂类别
		/// </summary>
		[MaxLength(22)]
		public Decimal? CLASS_TYPE {get;set;}

		/// <summary>
		/// 是否有效
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
