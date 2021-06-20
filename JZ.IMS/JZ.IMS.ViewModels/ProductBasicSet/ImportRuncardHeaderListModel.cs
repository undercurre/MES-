/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：客户SN表 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-19 16:20:59                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：ImportRuncardHeader                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-19 16:20:59
	/// 客户SN表 列表显示实体
	/// </summary>
	public class ImportRuncardHeaderListModel
	{
		/// <summary>
		/// 主键
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		public String WO_NO {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		public String ENABLE {get;set;}

		/// <summary>
		/// 导入SN的数量
		/// </summary>
		public Decimal? SN_QTY {get;set;}

		/// <summary>
		/// 是否已打印
		/// </summary>
		public String PRINTED { get; set; }

	}
}
