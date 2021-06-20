/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 10:34:23                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBurnFileApplyHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-07-22 10:34:23
	/// 
	/// </summary>
	[Table("MES_BURN_FILE_APPLY_HISTORY")]
	public partial class MesBurnFileApplyHistory
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 申请编号，供扫码采集使用
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String APPLY_NO {get;set;}

		/// <summary>
		/// 生产料号
		/// </summary>
		[MaxLength(30)]
		public String PART_CODE {get;set;}

		/// <summary>
		/// 工单
		/// </summary>
		[MaxLength(30)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 操作员
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
		/// 历史记录时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? LOG_TIME {get;set;}

		/// <summary>
		/// 历史记录人员
		/// </summary>
		[MaxLength(50)]
		public String LOG_USER {get;set;}

		


	}
}
