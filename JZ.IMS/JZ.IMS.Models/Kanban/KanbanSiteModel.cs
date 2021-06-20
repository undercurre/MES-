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
	public partial class KanbanSiteModel
	{
		/// <summary>
		/// 产线ID
		/// </summary>
		public decimal OPERATION_LINE_ID { get; set; }

		/// <summary>
		/// 工单ID
		/// </summary>
		public decimal WO_ID { get; set; }

		/// <summary>
		/// 工序ID
		/// </summary>
		public decimal OPERATION_ID { get; set; }

		/// <summary>
		/// 工序名称
		/// </summary>
		public String OPERATION_NAME { get; set; }

		/// <summary>
		/// PASS总数
		/// </summary>
		public decimal PASS { get; set; }

		/// <summary>
		/// FAIL
		/// </summary>
		public decimal FAIL { get; set; }

		/// <summary>
		/// REPASS总数
		/// </summary>
		public decimal REPASS { get; set; }

		/// <summary>
		/// REFAIL总数
		/// </summary>
		public decimal REFAIL { get; set; }
	}
}
