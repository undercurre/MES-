/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：镭雕任务下达表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-28 17:10:46                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsLaserTask                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-01-28 17:10:46
	/// 镭雕任务下达表
	/// </summary>
	[Table("SFCS_LASER_TASK")]
	public partial class SfcsLaserTask
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 关联的id
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TYPE_ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(20)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 规格
		/// </summary>
		[MaxLength(300)]
		public String PART_DESC {get;set;}

		/// <summary>
		/// 任务类型（1：工单流水号范围；2:导入SN；3:中转码）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String TASK_TYPE {get;set;}

		/// <summary>
		/// 打印总数
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRINT_TOTAL {get;set;}

		/// <summary>
		/// 已打印数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PRINT_QTY {get;set;}

		/// <summary>
		/// 打印状态（1：未打印；2:已打印）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String PRINT_STATUS {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(255)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(255)]
		public String UPDATE_USER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 镭雕机编号
		/// </summary>
		[MaxLength(50)]
		public String MACHINE_CODE {get;set;}


	}
}
