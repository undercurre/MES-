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
	public partial class TopDefectModel
	{
		/// <summary>
		/// 主键
		/// </summary>
		public decimal ID { get; set; }

		/// <summary>
		/// 不良代码
		/// </summary>
		public string DEFECT_CODE { get; set; }

		/// <summary>
		/// 不良代码描述
		/// </summary>
		public string DEFECT_DESCRIPTION { get; set; }

		/// <summary>
		/// 不良数量
		/// </summary>
		public int DEFECT_COUNT { get; set; }

	}
}
