/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-21 14:43:44                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtWo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-21 14:43:44
	/// 
	/// </summary>
	[Table("SMT_WO")]
	public partial class SmtWo
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 生产工单
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String WO_NO {get;set;}

		/// <summary>
		/// 成品料号
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 机种
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String MODEL {get;set;}

		[MaxLength(200)]
		public String CLASSIFICATION {get;set;}

		/// <summary>
		/// 备注说明
		/// </summary>
		[MaxLength(2000)]
		public String DESCRIPTION {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(200)]
		public String ATTRIBUTE5 {get;set;}

		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		[MaxLength(7)]
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 项目名称
		/// </summary>
		[MaxLength(30)]
		public string PROJECT_NAME { get; set; }

		/// <summary>
		/// 分组标记
		/// </summary>
		[MaxLength(10)]
		public string PACKET_MARKING { get; set; }
	}
}
