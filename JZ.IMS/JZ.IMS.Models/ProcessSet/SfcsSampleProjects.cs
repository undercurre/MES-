/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-15 11:59:54                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsSampleProjects                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-15 11:59:54
	/// 
	/// </summary>
	[Table("SFCS_SAMPLE_PROJECTS")]
	public partial class SfcsSampleProjects
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 抽检方案名称
		/// </summary>
		[MaxLength(100)]
		public String PROJECT_NAME {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(500)]
		public String DESCRIPTION {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
