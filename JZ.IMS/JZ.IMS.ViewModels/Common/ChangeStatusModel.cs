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

namespace JZ.IMS.ViewModels
{
    public class ChangeStatusModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        /// 修改后的状态
        /// </summary>
        public Boolean Status { get; set; } 

		/// <summary>
		/// 操作员
		/// </summary>
		public string Operator { get; set; }

		/// <summary>
		/// 操作日期
		/// </summary>
		public DateTime? OperatorDatetime { get; set; }
	}
}
