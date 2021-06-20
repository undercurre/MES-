/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动化线看板的AOI的直通率                                                 
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 10:39:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtKanbanAoiPassRateModel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-28 10:39:45
	/// 自动化线看板的AOI的直通率
	/// </summary>
	public class SmtKanbanAoiPassRateModel
	{
		/// <summary>
		/// 线体ID
		/// </summary>
		public Decimal OPERATION_LINE_ID {get;set;}

		/// <summary>
		/// 线体名称
		/// </summary>
		public String OPERATION_LINE_NAME {get;set;}

		/// <summary>
		/// 合格数量
		/// </summary>
		public Decimal PASS {get;set;}

		/// <summary>
		/// 总数
		/// </summary>
		public Decimal TOTAL {get;set;}

		/// <summary>
		/// 合格率
		/// </summary>
		public Decimal RATE {get;set;}


	}
}
