/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具申请表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-01 11:22:17                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsApply                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-01 11:22:17
	/// 夹具申请表
	/// </summary>
	[Table("MES_TONGS_APPLY")]
	public partial class MesTongsApply
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 申请数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QTY {get;set;}

		/// <summary>
		/// 剩余入库数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SURPLUS_QTY {get;set;}

		/// <summary>
		/// 夹具类别（0：工装，1：ICT针床，2:FCT针床）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_TYPE {get;set;}

		/// <summary>
		/// 部门
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DEPARTMENT {get;set;}

		/// <summary>
		/// 来源（0：自制，1：外购，2：转移）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SOURCES {get;set;}

		/// <summary>
		/// 状态：0：初始、1：已审核、2：已完成
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_DATE {get;set;}

		/// <summary>
		/// 最后修改人
		/// </summary>
		[MaxLength(30)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 最后修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_DATE {get;set;}

		/// <summary>
		/// 审核人
		/// </summary>
		[MaxLength(30)]
		public String AUDIT_USER {get;set;}

		/// <summary>
		/// 审核时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? AUDIT_DATE {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}

		/// <summary>
		/// 组织ID
		/// </summary>
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}


	}
}
