/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-19 20:26:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBurnSnDown                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-19 20:26:53
	/// 
	/// </summary>
	[Table("MES_BURN_SN_DOWN")]
	public partial class MesBurnSnDown
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 下载的ID(关联MES_BURN_FILE_DOWN)
		/// </summary>
		[MaxLength(22)]
		public Decimal? DOWN_ID {get;set;}

		/// <summary>
		/// 下载编号(关联MES_BURN_FILE_DOWN)
		/// </summary>
		[MaxLength(30)]
		public String DOWN_NO {get;set;}

		/// <summary>
		/// SN流水号
		/// </summary>
		[MaxLength(50)]
		public String SN {get;set;}

		/// <summary>
		/// 用户
		/// </summary>
		[MaxLength(50)]
		public String USER_NAME {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? MODIFY_TIME {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(255)]
		public String ATTRIBUTE6 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(255)]
		public String ATTRIBUTE7 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(255)]
		public String ATTRIBUTE8 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(255)]
		public String ATTRIBUTE9 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		[MaxLength(255)]
		public String ATTRIBUTE10 {get;set;}

		/// <summary>
		/// 申请编号
		/// </summary>
		[MaxLength(30)]
		public String APPLY_NO {get;set;}


	}
}
