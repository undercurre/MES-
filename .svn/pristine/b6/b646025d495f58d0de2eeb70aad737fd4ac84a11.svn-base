/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认详细表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-25 10:28:35                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesProductionPreDtl                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-25 10:28:35
	/// 产前确认详细表
	/// </summary>
	[Table("MES_PRODUCTION_PRE_DTL")]
	public partial class MesProductionPreDtl
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 配置表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CONF_ID {get;set;}

		/// <summary>
		/// 主表ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MST_ID {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String CREATOR {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATIME {get;set;}

		/// <summary>
		/// 判断结果 Y：正确，N：错误
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String RESULT {get;set;}

		/// <summary>
		/// 判断描述
		/// </summary>
		[MaxLength(1000)]
		public String DESCRIPTION {get;set;}


	}
}
