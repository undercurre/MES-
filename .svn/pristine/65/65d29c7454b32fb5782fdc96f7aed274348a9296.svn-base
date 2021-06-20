/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具基本信息表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-20 17:39:29                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsInfo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-20 17:39:29
	/// 夹具基本信息表
	/// </summary>
	[Table("MES_TONGS_INFO")]
	public partial class MesTongsInfo
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 夹具编码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CODE { get; set; }

		/// <summary>
		/// 夹具类别（0：工装，1：ICT针床，2:FCT针床）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_TYPE { get; set; }

		/// <summary>
		/// 部门
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DEPARTMENT { get; set; }

		/// <summary>
		/// 来源（0：自制，1：外购，2：转移）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SOURCES { get; set; }

		/// <summary>
		/// 夹具状态，0：待入库，1：存储中，2：借出，3：使用中，4：保养中，5：维修中，6：已报废
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS { get; set; }

		/// <summary>
		/// 储位ID
		/// </summary>
		[MaxLength(22)]
		public Decimal? STORE_ID { get; set; }

		/// <summary>
		/// 注册人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER { get; set; }

		/// <summary>
		/// 注册时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_DATE { get; set; }

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(30)]
		public String UPDATE_USER { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_DATE { get; set; }

		/// <summary>
		/// 激活（Y:已激活，N：未激活）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ACTIVE { get; set; }

		/// <summary>
		/// 是否有效（Y:有效，N：无效）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED { get; set; }

		/// <summary>
		/// 组织架构ID
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ORGANIZE_ID { get; set; }
	}
}
