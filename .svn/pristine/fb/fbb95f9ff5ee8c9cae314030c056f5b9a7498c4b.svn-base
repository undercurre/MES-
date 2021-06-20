/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具操作记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-24 09:30:52                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesTongsOperationHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-24 09:30:52
	/// 夹具操作记录表
	/// </summary>
	[Table("MES_TONGS_OPERATION_HISTORY")]
	public partial class MesTongsOperationHistory
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 夹具ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TONGS_ID { get; set; }

		/// <summary>
		/// 操作类型：0、注册；1、入库；2、激活；3、变更储位；4、领用；5、使用（预留）；6、开始保养；7、结束保养；8、维修；
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal OPERATION_TYPE { get; set; }

		/// <summary>
		/// 操作前状态
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRE_STATUS { get; set; }

		/// <summary>
		/// 操作后状态
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LAST_STATUS { get; set; }

		/// <summary>
		/// 储位信息
		/// </summary>
		[MaxLength(22)]
		public Decimal? STORE_ID { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(200)]
		public String REMARK { get; set; }

		/// <summary>
		/// 操作人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER { get; set; }

		/// <summary>
		/// 操作时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_DATE { get; set; }


	}
}
