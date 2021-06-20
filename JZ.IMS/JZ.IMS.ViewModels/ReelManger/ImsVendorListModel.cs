/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-21 15:57:13                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：ImsVendor                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-21 15:57:13
	///  列表显示实体
	/// </summary>
	public class ImsVendorListModel
	{
		public Decimal ID {get;set;}

		public String CODE {get;set;}

		public String NAME {get;set;}

		public String UNIFORM_NUMBER {get;set;}

		public String HTTP_CODE {get;set;}

		public String CONTACT_CODE {get;set;}

		public String BONDED_TYPE {get;set;}

		public String BONDED_KIND {get;set;}

		public String DESCRIPTION {get;set;}

		public String COMPANY_ID {get;set;}

		public String SPOTDEALER_FLAG {get;set;}

		public String ENABLED {get;set;}


	}
	/// <summary>
	/// 
	/// </summary>
    public class VendorListModel
    {
		/// <summary>
		/// 供应商ID
		/// </summary>
		public String ID { get; set; }
		/// <summary>
		/// 供应商编码
		/// </summary>
		public String CODE { get; set; }
		/// <summary>
		/// 供应商描述
		/// </summary>
		public String DESCRIPTION { get; set; }
		/// <summary>
		/// 供应商编码-供应商描述
		/// </summary>
		public String VENDOR_INFO { get; set; }
    }
}
