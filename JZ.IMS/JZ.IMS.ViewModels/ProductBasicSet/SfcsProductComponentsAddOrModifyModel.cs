/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 15:59:15                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductComponents                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-03 15:59:15
	///  更新或者新增实体
	/// </summary>
	public class SfcsProductComponentsAddOrModifyModel
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
		/// 零件ID
		/// </summary>
		public Decimal COMPONENT_ID {get;set;}

		/// <summary>
		/// 本厂料号(零件料号)
		/// </summary>
		public String ODM_COMPONENT_PN {get;set;}

		/// <summary>
		/// 客户零件料号
		/// </summary>
		public String CUSTOMER_COMPONENT_PN {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		public Decimal COMPONENT_QTY {get;set;}

		/// <summary>
		/// 是否唯一序列
		/// </summary>
		public String SERIALIZED {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED {get;set;}

		/// <summary>
		/// 返工是否自动清除
		/// </summary>
		public String REWORK_REMOVE_FLAG {get;set;}

		/// <summary>
		/// 是否EDI
		/// </summary>
		public String EDI_FLAG {get;set;}

		/// <summary>
		/// 是否测试零件
		/// </summary>
		public String DEVICE_FLAG {get;set;}

		/// <summary>
		/// 测试零件标准使用次数
		/// </summary>
		public Decimal? STANDARD_USE_COUNT {get;set;}

		/// <summary>
		/// 是否检查不良
		/// </summary>
		public String CHECK_DEFECT_FLAG {get;set;}
		/// <summary>
		/// 这个字段不用传
		/// </summary>
		public String COMPONENT_LOCATIONS {get;set;}

		/// <summary>
		/// 采集工序ID
		/// </summary>
		public Decimal? COLLECT_OPERATION_ID {get;set;}
		/// <summary>
		/// 是否生成SN.BOM
		/// </summary>
		public String ATTRIBUTE1 { get; set; }
		/// <summary>
		/// 零件描述
		/// </summary>

		public String ATTRIBUTE2 { get; set; }


	}
}
