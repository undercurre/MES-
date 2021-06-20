/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动化线看板-每小时产能                                                
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 11:31:40                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SmtKanbanHourYidldModel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-28 11:31:40
	/// 自动化线看板-每小时产能
	/// </summary>
	public class SmtKanbanHourYidldModel
	{
		public Decimal ID { get; set; }

		public String LINE_TYPE { get; set; }

		public Decimal OPERATION_LINE_ID { get; set; }

		public String WO_NO { get; set; }

		public Decimal? PCB_SIDE { get; set; }

		public String PART_NO { get; set; }

		public DateTime? WORK_TIME { get; set; }

		public Decimal? OUTPUT_QTY { get; set; }

		public Decimal? STANDARD_CAPACITY { get; set; }

		public Decimal? STANDARD_CAPACITY_WORK { get; set; }

		public Decimal? VALUE_MAX { get; set; }

		public Decimal? VALUE_MIN { get; set; }

		public String CURRENT_HOUR { get; set; }

		public Decimal? REPORT_ID { get; set; }

		public String REPORT_NO { get; set; }

		public Decimal? STANDARD_CAPACITY_MINUTE { get; set; }

		public Decimal? DTL_ID { get; set; }

		public Decimal? DTL_TYPE { get; set; }

		public Decimal? DTL_MINUTES { get; set; }

		public Decimal? DTL_START_MINUTE { get; set; }

		public Decimal? DTL_END_MINUTE { get; set; }

		public String REPORT_CONTENT { get; set; }

		public String REASON { get; set; }

		public Decimal? STATUS { get; set; }
	}
}
