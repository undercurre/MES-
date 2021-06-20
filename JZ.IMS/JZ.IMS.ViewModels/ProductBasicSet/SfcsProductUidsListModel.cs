/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-31 18:04:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductUids                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-31 18:04:38
	///  列表显示实体
	/// </summary>
	public class SfcsProductUidsListModel
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO {get;set;}

		/// <summary>
		/// UID种类
		/// </summary>
		public Decimal UID_ID {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 收集数量
		/// </summary>
		public Decimal UID_QTY {get;set;}

		/// <summary>
		/// 是否唯一序列
		/// </summary>
		public String SERIALIZED {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED {get;set;}

		/// <summary>
		/// 是否返工清除
		/// </summary>
		public String REWORK_REMOVE_FLAG {get;set;}

		/// <summary>
		/// 是否排序
		/// </summary>
		public String ORDERED {get;set;}

		/// <summary>
		/// 是否EDI
		/// </summary>
		public String EDI_FLAG {get;set;}

		/// <summary>
		/// 采集工序
		/// </summary>
		public Decimal? COLLECT_OPERATION_ID {get;set;}
		/// <summary>
		/// 解邦工序
		/// </summary>
		public Decimal? BREAK_OPERATION_ID { get; set; }
	}
}
