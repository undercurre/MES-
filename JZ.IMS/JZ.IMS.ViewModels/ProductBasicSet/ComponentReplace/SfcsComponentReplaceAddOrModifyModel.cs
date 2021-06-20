/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-12-09 09:51:57                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsComponentReplace                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-12-09 09:51:57
	///  更新或者新增实体
	/// </summary>
	public class SfcsComponentReplaceAddOrModifyModel
	{
		public Decimal REPLACE_COMPONENT_ID {get;set;}

		/// <summary>
		/// sfcs_runcard的ID
		/// </summary>
		public Decimal? SN_ID {get;set;}

		public Decimal? COLLECT_COMPONENT_ID {get;set;}

		public Decimal? COLLECT_OPERATION_ID {get;set;}

		public Decimal? REPLACE_OPERATION_ID {get;set;}

		public Decimal? COLLECT_DEFECT_ID {get;set;}

		public Decimal? COMPONENT_ID {get;set;}

		/// <summary>
		/// 零件名称
		/// </summary>
		public String COMPONENT_NAME {get;set;}

		/// <summary>
		/// 零件新客户料号
		/// </summary>
		public String NEW_CUSTOMER_COMPONENT_PN {get;set;}

		/// <summary>
		/// 零件旧客户料号
		/// </summary>
		public String OLD_CUSTOMER_COMPONENT_PN {get;set;}

		/// <summary>
		/// 零件新本厂料号
		/// </summary>
		public String NEW_ODM_COMPONENT_PN {get;set;}

		/// <summary>
		/// 零件旧本厂料号
		/// </summary>
		public String OLD_ODM_COMPONENT_PN {get;set;}

		/// <summary>
		/// 新零件序号
		/// </summary>
		public String NEW_CUSTOMER_COMPONENT_SN {get;set;}

		/// <summary>
		/// 旧零件序号
		/// </summary>
		public String OLD_CUSTOMER_COMPONENT_SN {get;set;}

		/// <summary>
		/// 零件新本厂序号
		/// </summary>
		public String NEW_ODM_COMPONENT_SN {get;set;}

		/// <summary>
		/// 零件旧本帮序号
		/// </summary>
		public String OLD_ODM_COMPONENT_SN {get;set;}

		/// <summary>
		/// 零件数量
		/// </summary>
		public Decimal? COMPONENT_QTY {get;set;}

		/// <summary>
		/// 替换站点ID
		/// </summary>
		public Decimal? REPLACE_SITE_ID {get;set;}

		/// <summary>
		/// 序号是否唯一
		/// </summary>
		public String SERIALIZED {get;set;}

		public Decimal? REPLACE_TYPE {get;set;}

		/// <summary>
		/// 替换原因
		/// </summary>
		public String REPLACE_REASON {get;set;}

		/// <summary>
		/// 替换备注
		/// </summary>
		public String REPLACE_REMARK {get;set;}

		/// <summary>
		/// 替换人
		/// </summary>
		public String REPLACE_BY {get;set;}

		/// <summary>
		/// 替换时间
		/// </summary>
		public DateTime? REPLACE_TIME {get;set;}

		public String DUMMY_SERIAL_NUMBER {get;set;}

		public String DUMMY_PART_NUMBER {get;set;}


	}
}
