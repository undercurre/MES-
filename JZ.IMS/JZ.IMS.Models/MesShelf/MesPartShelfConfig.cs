/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-27 11:52:21                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesPartShelfConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-01-27 11:52:21
	/// 
	/// </summary>
	[Table("MES_PART_SHELF_CONFIG")]
	public partial class MesPartShelfConfig
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 编号:"SFCE-yyyy-MM-dd-HH-mm-ss"
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SHELF_CODE {get;set;}

		/// <summary>
		/// 料架名称
		/// </summary>
		[MaxLength(50)]
		public String SHELF_NAME {get;set;}

		/// <summary>
		/// 组织架构
		/// </summary>
		[MaxLength(22)]
		public Decimal? ORGANIZE_ID {get;set;}

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
		[MaxLength(10)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		[MaxLength(10)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLE {get;set;}


	}
}
