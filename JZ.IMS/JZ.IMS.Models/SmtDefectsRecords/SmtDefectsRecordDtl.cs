/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动化不良维修记录                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-12 13:47:12                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtDefectsRecordDtl                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-12 13:47:12
	/// 自动化不良维修记录
	/// </summary>
	[Table("SMT_DEFECTS_RECORD_DTL")]
	public partial class SmtDefectsRecordDtl
	{
		[Key]
		public Decimal ID {get;set;}

		[Required]
		[MaxLength(22)]
		public Decimal MST_ID {get;set;}

		/// <summary>
		/// 产品流水号
		/// </summary>
		[MaxLength(100)]
		public String SN {get;set;}

		/// <summary>
		/// 不良位号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String LOCATION {get;set;}

		/// <summary>
		/// 不良代码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String DEFECT_CODE {get;set;}

		/// <summary>
		/// 不良现象
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String DEFECT_DES {get;set;}

		/// <summary>
		/// 不良描述
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String DEFECT_REMARK {get;set;}

		/// <summary>
		/// 维修状态Y/N
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String IS_OK {get;set;}

		/// <summary>
		/// 维修人员
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String REPAIR_USER {get;set;}

		/// <summary>
		/// 维修时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime REPAIR_TIME {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
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
}
