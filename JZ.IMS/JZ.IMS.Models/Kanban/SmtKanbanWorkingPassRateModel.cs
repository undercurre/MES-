/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动化线看板的排产的完成率                                                 
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 10:37:01                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtKanbanWorkingPassRateModel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-28 10:37:01
	/// 自动化线看板的排产的完成率
	/// </summary>
	public class SmtKanbanWorkingPassRateModel
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
