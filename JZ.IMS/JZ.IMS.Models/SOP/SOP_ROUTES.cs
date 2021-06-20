/**
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SOP_ROUTES                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models {
	/// <summary>
	/// Admin
	/// 工艺路线
	/// </summary>
	public partial class SOP_ROUTES {
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public decimal ID { get; set; }

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		public string PART_NO { get; set; }

		/// <summary>
		/// 制程名称
		/// </summary>
		[Required]
		public string ROUTE_NAME { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string DESCRIPTION { get; set; }

		/// <summary>
		/// 状态(0:待审核;1:已审核;)
		/// </summary>
		public decimal STATUS { get; set; }

		/// <summary>
		///创建人
		/// </summary>
		[Required]
		public string CREATER { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		public DateTime CREATE_TIME { get; set; }

		/// <summary>
		///审核人
		/// </summary>
		[Required]
		public string AUDITOR { get; set; }

		/// <summary>
		/// 审核时间
		/// </summary>
		[Required]
		public DateTime? AUDIT_TIME { get; set; }

		/// <summary>
		/// 审核后修改人
		/// </summary>
		[MaxLength(10)]
		public string REAUDITOR { get; set; }

		/// <summary>
		/// 审核后修改时间
		/// </summary>
		[MaxLength(23)]
		public DateTime? REAUDIT_TIME { get; set; }

		/// <summary>
		/// 最后更新时间(每次新增\修改数据都更新)
		/// </summary>
		[Required]
		[MaxLength(23)]
		public DateTime? LAST_UPDATE_TIME { get; set; }

		/// <summary>
		/// 版本号
		/// </summary>
		[MaxLength(30)]
		public string ATTRIBUTE1 { get; set; }

		/// <summary>
		/// 生效日期
		/// </summary>
		[MaxLength(30)]
		public string ATTRIBUTE2 { get; set; }

		/// <summary>
		/// 失效日期
		/// </summary>
		[MaxLength(30)]
		public string ATTRIBUTE3 { get; set; }

		/// <summary>
		/// 是否有效
		/// </summary>
		[MaxLength(30)]
		public string ATTRIBUTE4 { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(200)]
		public string REMARK { get; set; }
	}
}
