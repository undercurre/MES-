/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：特殊SN表 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-24 14:15:40                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：ImportRuncardSn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-24 14:15:40
	/// 特殊SN表 列表显示实体
	/// </summary>
	public class ImportRuncardSnListModel
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
		/// SN流水号
		/// </summary>
		public String SN {get;set;}

		/// <summary>
		/// 父级流水号 暂无使用
		/// </summary>
		public String PARENT_SN {get;set;}

		/// <summary>
		/// 制程ID SFCS_ROUTES.ID
		/// </summary>
		public Decimal? ROUTE_ID {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		public String ENABLE {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		public String UPDATE_BY {get;set;}


	}
}
