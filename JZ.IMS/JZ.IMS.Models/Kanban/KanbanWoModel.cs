/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：Manager改变锁定状态实体                                                    
*│　作    者：Admin                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/1/2 12:52:32                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.ViewModels.Manager                                   
*│　类    名： ManagerChangeLockStatusModel                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.Models
{
	public partial class KanbanWoModel
	{
		/// <summary>
		/// 产线ID
		/// </summary>
		public decimal OPERATION_LINE_ID { get; set; }

		/// <summary>
		/// 产线名称
		/// </summary>
		public string OPERATION_LINE_NAME { get; set; }

		/// <summary>
		/// 工单ID
		/// </summary>
		public decimal WO_ID { get; set; }

		/// <summary>
		/// 工单号
		/// </summary>
		public string WO_NO { get; set; }

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO { get; set; }

		/// <summary>
		/// 机种
		/// </summary>
		public String MODEL { get; set; }

		/// <summary>
		/// 工单总量
		/// </summary>
		public decimal TARGET_QTY { get; set; }

		/// <summary>
		/// 完成数量
		/// </summary>
		public decimal OUTPUT_QTY { get; set; }

		/// <summary>
		/// 工单产能
		/// </summary>
		public decimal YIELD { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime UPDATE_TIME { get; set; }

		/// <summary>
		/// 制程ID
		/// </summary>
		public decimal ROUTE_ID { get; set; }

		/// <summary>
		/// 连板数
		/// </summary>
		public Decimal MULTI_NO { get; set; }

		/// <summary>
		/// 剩余X大板的余料时预警
		/// </summary>
		public Decimal HIPTS_ALARM { get; set; }
		
	}
}
