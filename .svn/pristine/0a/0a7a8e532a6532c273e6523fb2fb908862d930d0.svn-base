/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-08 15:21:13                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsCustomersComplaint                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-08 15:21:13
	/// 
	/// </summary>
	[Table("SFCS_CUSTOMERS_COMPLAINT")]
	public partial class SfcsCustomersComplaint
	{
		[Key]
		public Decimal ID {get;set;}

		[Required]
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal CUSTOMERS_ID {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		[MaxLength(50)]
		public String WO_NO {get;set;}

		[Required]
		[MaxLength(50)]
		public String PART_NO {get;set;}

		[Required]
		[MaxLength(50)]
		public String MODEL {get;set;}

		[Required]
		[MaxLength(7)]
		public DateTime PRODUCT_DATE {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal REWORK_QTY {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal RETURN_QTY {get;set;}

		[Required]
		[MaxLength(400)]
		public String CONTENT {get;set;}

		[MaxLength(400)]
		public String CAUSE {get;set;}

		[MaxLength(400)]
		public String INTERIM_MEASURES {get;set;}

		[MaxLength(400)]
		public String LONG_MEASURES {get;set;}

		[MaxLength(400)]
		public String REMARKS {get;set;}

		[Required]
		[MaxLength(7)]
		public DateTime COMPLAINT_DATE {get;set;}

		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		[Required]
		[MaxLength(50)]
		public String CREATE_USER {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE5 {get;set;}


	}

	public class ComplaintFile {
		public int ID { get; set; }
		public string name { get; set; }
		public string url { get; set; }
	}
}
