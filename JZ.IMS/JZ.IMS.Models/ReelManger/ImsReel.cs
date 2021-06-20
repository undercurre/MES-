/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-04 15:39:22                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ImsReel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-04 15:39:22
	/// 
	/// </summary>
	[Table("IMS_REEL")]
	public partial class ImsReel
	{
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(200)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(200)]
		public String DISABLE_BILL_ID {get;set;}

		[Required]
		[MaxLength(200)]
		public String CODE {get;set;}

		[MaxLength(22)]
		public Decimal? BOX_ID {get;set;}

		[MaxLength(22)]
		public Decimal? VENDOR_ID {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal PART_ID {get;set;}

		[MaxLength(22)]
		public Decimal? MAKER_PART_ID {get;set;}

		[MaxLength(22)]
		public String DATE_CODE {get;set;}

		[MaxLength(200)]
		public String LOT_CODE {get;set;}

		[MaxLength(200)]
		public String REVISION {get;set;}

		[MaxLength(22)]
		public Decimal? PARENT_ID {get;set;}

		[MaxLength(200)]
		public String MSD_LEVEL {get;set;}

		[MaxLength(200)]
		public String ESD_FLAG {get;set;}

		[MaxLength(200)]
		public String SELF_GENERATE {get;set;}

		[MaxLength(600)]
		public String REGION {get;set;}

		[MaxLength(600)]
		public String SOFTWARE {get;set;}

		[MaxLength(600)]
		public String FIRMWARE {get;set;}

		[MaxLength(22)]
		public Decimal? CASE_QTY {get;set;}

		[MaxLength(200)]
		public String IQC_FLAG {get;set;}

		[MaxLength(2000)]
		public String DESCRIPTION {get;set;}

		[MaxLength(400)]
		public String COO {get;set;}

		[MaxLength(400)]
		public String CUSTOMER_PN {get;set;}

		[MaxLength(600)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(600)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(600)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(600)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(600)]
		public String ATTRIBUTE5 {get;set;}

		[MaxLength(22)]
		public Decimal? ORIGINAL_SIC_ID {get;set;}

		[MaxLength(22)]
		public Decimal? TO_LOCATOR_ID {get;set;}

		[MaxLength(400)]
		public String REFERENCE {get;set;}

		[MaxLength(400)]
		public String MAKER_PART_CODE {get;set;}

		[MaxLength(22)]
		public Decimal? ORIGINAL_QUANTITY {get;set;}

		//[MaxLength(200)]
		//public String CREATED_BY {get;set;}

		//[MaxLength(7)]
		//public DateTime? CREATED_DATE {get;set;}

		//[MaxLength(200)]
		//public String LAST_UPDATE_BY {get;set;}

		//[MaxLength(7)]
		//public DateTime? LAST_UPDATE_DATE {get;set;}


	}
}
