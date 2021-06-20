/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-05 09:34:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtStencilPart                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-05 09:34:53
	/// 
	/// </summary>
	[Table("SMT_STENCIL_PART")]
	public partial class SmtStencilPart
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public String ID {get;set;}

		/// <summary>
		/// 钢网编号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String STENCIL_NO {get;set;}

		/// <summary>
		/// 产品编号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 正(反)面
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String PCB_SIDE {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[MaxLength(50)]
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 料号规格
		/// </summary>
		[MaxLength(50)]
		public String PN_MODEL {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(1000)]
		public string DESCRIPTION { get; set; }


	}
}
