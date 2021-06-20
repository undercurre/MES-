/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：看板控制器 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-13 11:41:58                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：MesKanbanControllerModifyModel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-13 11:41:58
	/// 看板控制器 更新实体
	/// </summary>
	public class MesKanbanControllerModifyModel
	{
		/// <summary>
		/// 车间类型(SMT/PCBA)
		/// </summary>
		public String LINE_TYPE { get; set; }

		/// <summary>
		/// 线体ID
		/// </summary>
		public Decimal LINE_ID { get; set; }

		/// <summary>
		/// 站点集合
		/// </summary>
		public List<KanbanSiteModel> SITE_MODELS { get; set; }

		/// <summary>
		/// 合格率集合
		/// </summary>
		public List<MesKanbanPassRateModel> PASS_RATE_MODELS { get; set; }

		/// <summary>
		/// 小时产能集合(PCBA)
		/// </summary>
		public List<HourYidldModel> HOUR_YIDLD_PCBA_MODELS { get; set; }

		/// <summary>
		/// 小时产能集合(SMT)
		/// </summary>
		public List<SmtKanbanHourYidldModel> HOUR_YIDLD_SMT_MODELS { get; set; }

		/// <summary>
		/// 换线记录
		/// </summary>
		public List<MesChangeLineRecordResult> RECORD_MODELS { get; set; }
	}
}
