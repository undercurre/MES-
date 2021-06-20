/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：拆拼板的详细信息                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-23 10:01:32                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtMultipanelDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-23 10:01:32
	/// 拆拼板的详细信息
	/// </summary>
	[Table("SMT_MULTIPANEL_DETAIL")]
	public partial class SmtMultipanelDetail
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 拆拼板的头部信息
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MULT_HEADER_ID {get;set;}

		/// <summary>
		/// 连板SN
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SN {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATETIME {get;set;}
		/// <summary>
		/// 镭雕机打印SN的顺序
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TASK_NO { get; set; }
		/// <summary>
		/// 镭雕打印SN的状态码1:成功,0：失败
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TASK_STATUS { get; set; }
		/// <summary>
		/// 镭雕打印SN的状态描述
		/// </summary>
		[Required]
		[MaxLength(500)]
		public String TASK_MSG { get; set; }
		/// <summary>
		/// 镭雕打印任务ID
		/// </summary>
		[Required]
		[MaxLength(500)]
		public String TASK_ID { get; set; }

	}
}
