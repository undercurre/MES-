/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产线看板-排产完成率                                               
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 20:59:35                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：KanbanWorkingPassRate                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-28 20:59:35
	/// 产线看板-排产完成率 
	/// </summary>
	public class KanbanWorkingPassRateModel
	{
		/// <summary>
		/// 线体ID
		/// </summary>
		public Decimal OPERATION_LINE_ID {get;set;}

		/// <summary>
		/// 完成数量
		/// </summary>
		public Decimal PASS {get;set;}

		/// <summary>
		/// 排产总数
		/// </summary>
		public Decimal TOTAL {get;set;}

		/// <summary>
		/// 完成率
		/// </summary>
		public Decimal RATE {get;set;}


	}
}
