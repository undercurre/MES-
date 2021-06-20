/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 10:34:52                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsSubstituteComponents                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-03 10:34:52
	///  更新或者新增实体
	/// </summary>
	public class SfcsSubstituteComponentsAddOrModifyModel
	{
		/// <summary>
		/// 主键
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 产品零件
		/// </summary>
		public Decimal PRODUCT_COMPONENT_ID {get;set;}

		/// <summary>
		/// 替代料本厂料号
		/// </summary>
		public String SUBSTITUTE_COMP_PN {get;set;}

		/// <summary>
		/// 替代料客户料号
		/// </summary>
		public String CUSTOMER_COMPONENT_PN {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		public Decimal COMPONENT_QTY {get;set;}

		/// <summary>
		/// 开始日期
		/// </summary>
		public DateTime? BEGIN_DATE {get;set;}

		/// <summary>
		/// 结束日期
		/// </summary>
		public DateTime? END_DATE {get;set;}

		/// <summary>
		/// 是否唯一序列
		/// </summary>
		public String SERIALIZED {get;set;}

		/// <summary>
		/// 返工是否自动清除
		/// </summary>
		public String REWORK_REMOVE_FLAG {get;set;}

		/// <summary>
		/// 是否EDI
		/// </summary>
		public String EDI_FLAG {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED {get;set;}

		/// <summary>
		/// 是否测试零件
		/// </summary>
		public String DEVICE_FLAG {get;set;}

		/// <summary>
		/// 测试零件标准使用次数
		/// </summary>
		public Decimal? STANDARD_USE_COUNT {get;set;}

		/// <summary>
		/// 零件描述
		/// </summary>
		public String ATTRIBUTE2 { get; set; }

	}
}
