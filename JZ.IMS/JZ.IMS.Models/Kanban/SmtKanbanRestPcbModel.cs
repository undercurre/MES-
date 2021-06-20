/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动化线看板-可打板剩余数                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 10:38:11                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtKanbanRestPcbModel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-28 10:38:11
	/// 自动化线看板-低水位预警
	/// </summary>
	public class SmtKanbanRestPcbModel
	{
		/// <summary>
		/// 线别ID
		/// </summary>
		public Decimal OPERATION_LINE_ID {get;set;}

		/// <summary>
		/// 位置
		/// </summary>
		public String LOCATION {get;set;}

		public String SUB_SLOT {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO {get;set;}

		/// <summary>
		/// 使用数量
		/// </summary>
		public Decimal USED_QTY {get;set;}

		/// <summary>
		/// 剩余数量
		/// </summary>
		public Decimal REST_PCB_COUNT {get;set;}

		/// <summary>
		/// 排序
		/// </summary>
		public Decimal ROWNO {get;set;}


	}
}
