/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：批次管理文件上传表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-17 17:11:24                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBatchResources                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-17 17:11:24
	/// 批次管理文件上传表
	/// </summary>
	[Table("MES_BATCH_RESOURCES")]
	public partial class MesBatchResources
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 批次管理表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal BT_MANAGER_ID {get;set;}

		/// <summary>
		/// 线别名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String LINE_NAME {get;set;}

		/// <summary>
		/// 文件类型
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal RESOURCE_TYPE {get;set;}

		/// <summary>
		/// 文件路径
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String RESOURCES_URL {get;set;}

		/// <summary>
		/// 文件名称
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String RESOURCE_NAME {get;set;}

		/// <summary>
		/// 文件大小
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal RESOURCE_SIZE {get;set;}

		/// <summary>
		/// 用户
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String UPLOAD_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}


	}
}
