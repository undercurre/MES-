/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-01 09:20:16                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductAttachments                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-01 09:20:16
	///  更新或者新增实体
	/// </summary>
	public class SfcsProductAttachmentsAddOrModifyModel
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 主表ID(必填)
		/// </summary>
		public Decimal PRODUCT_OBJECT_ID {get;set;}

		/// <summary>
		/// 附件名称(必填)
		/// </summary>
		public Decimal ATTACHMENT_ID {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 固定值
		/// </summary>
		public String FIX_VALUE {get;set;}

		/// <summary>
		/// 附件数量
		/// </summary>
		public Decimal? ATTACHMENT_QTY {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED {get;set;}


	}
}
