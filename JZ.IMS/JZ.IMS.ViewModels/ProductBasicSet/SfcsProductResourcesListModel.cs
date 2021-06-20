/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-06 14:36:26                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductResources                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-06 14:36:26
	///  列表显示实体
	/// </summary>
	public class SfcsProductResourcesListModel
	{
		/// <summary>
		/// 主键
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO {get;set;}

		/// <summary>
		/// 资源名称
		/// </summary>
		public Decimal RESOURCE_ID {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 固定值
		/// </summary>
		public String FIXED_VALUE {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		public Decimal? RESOURCE_QTY {get;set;}

		public String BINDING_SITE {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED {get;set;}

		public String REPEATED {get;set;}

		/// <summary>
		/// 返工是否自动清除
		/// </summary>
		public String REWORK_REMOVE_FLAG {get;set;}

		/// <summary>
		/// 是否EDI
		/// </summary>
		public String EDI_FLAG {get;set;}

		/// <summary>
		/// 采集工序
		/// </summary>
		public Decimal? COLLECT_OPERATION_ID {get;set;}


	}
}
