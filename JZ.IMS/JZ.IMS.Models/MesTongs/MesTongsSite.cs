/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：工装与工位绑定                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-13 13:57:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsSite                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-01-13 13:57:38
	/// 工装与工位绑定
	/// </summary>
	[Table("MES_TONGS_SITE")]
	public partial class MesTongsSite
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 夹具ID(MES_TONGS_INFO)
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_ID {get;set;}

		/// <summary>
		/// 站点ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SITE_ID {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? LINE_ID {get;set;}

		/// <summary>
		/// 工序ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? OPERATION_ID {get;set;}

		/// <summary>
		/// 绑定时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 绑定人
		/// </summary>
		[Required]
		[MaxLength(20)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(20)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 是:Y 否:N
		/// </summary>
		[Required]
		[MaxLength(2)]
		public String IS_USING {get;set;}


	}
}
