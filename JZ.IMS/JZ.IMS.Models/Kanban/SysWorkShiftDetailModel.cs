/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：上班班次明细表（用于产线看板/自动化看板）                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-06 16:36:31                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SysWorkShiftDetailModel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-11-06 16:36:31
	/// 上班班次明细表（用于产线看板/自动化看板）
	/// </summary>
	public class SysWorkShiftDetailModel
	{
		/// <summary>
		/// 工作小时
		/// </summary>
		public String WORK_TIME {get;set;}

		/// <summary>
		/// 工作分钟数
		/// </summary>
		public Decimal WORKING_MINUTES {get;set;}

		/// <summary>
		/// 线体类型(SMT\PCBA)
		/// </summary>
		public String LINE_TYPE {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		public Decimal LINE_ID {get;set;}


	}
}
