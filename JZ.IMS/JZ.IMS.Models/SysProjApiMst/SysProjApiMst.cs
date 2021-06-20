/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-14 09:32:34                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysProjApiMst                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-09-14 09:32:34
	/// 
	/// </summary>
	[Table("SYS_PROJ_API_MST")]
	public partial class SysProjApiMst
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 自定义ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal RID {get;set;}

		/// <summary>
		/// 项目版本号
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String PROJ_VERTION {get;set;}

		/// <summary>
		/// 项目
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String PROJ {get;set;}

		/// <summary>
		/// 功能名
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String ACTION {get;set;}

		/// <summary>
		/// 功能描述
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 请求地址
		/// </summary>
		[Required]
		[MaxLength(400)]
		public String URL {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(100)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 是否可用
		/// </summary>
		[MaxLength(1)]
		public String ENABLED { get; set; }

		/// <summary>
		/// 参数
		/// </summary>
		public List<SysProjApiParm> parms { get; set; }
	}
}
