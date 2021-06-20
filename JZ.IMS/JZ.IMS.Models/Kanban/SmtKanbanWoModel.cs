/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动化线看板-工单信息                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 10:27:00                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtKanbanWoModel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-28 10:27:00
	/// 自动化线看板-工单信息
	/// </summary>
	public partial class SmtKanbanWoModel
	{
		/// <summary>
		/// 线体ID
		/// </summary>
		public Decimal OPERATION_LINE_ID { get; set; }

		/// <summary>
		/// 线体名称
		/// </summary>
		public String OPERATION_LINE_NAME { get; set; }

		/// <summary>
		/// 工单号
		/// </summary>
		public String WO_NO { get; set; }

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO { get; set; }

		/// <summary>
		/// 机型
		/// </summary>
		public String MODEL { get; set; }

		/// <summary>
		/// 总量
		/// </summary>
		public Decimal TARGET_QTY { get; set; }

		/// <summary>
		/// 完成数量
		/// </summary>
		public Decimal OUTPUT_QTY { get; set; }

		/// <summary>
		/// 达成率
		/// </summary>
		public Decimal YIELD { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime UPDATE_TIME { get; set; }

		/// <summary>
		/// 连板数
		/// </summary>
		public Decimal MULTI_NO { get; set; }

		/// <summary>
		/// 批号
		/// </summary>
		public String BATCH_NO { get; set; }

		/// <summary>
		/// 板底/板面
		/// </summary>
		public Decimal PCB_SIDE { get; set; }

		/// <summary>
		/// 开工表ID
		/// </summary>
		public Decimal PLACEMENT_MST_ID { get; set; }


	}
}
