/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-06 16:59:33                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SopSkillStandard                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-11-06 16:59:33
	/// 
	/// </summary>
	[Table("SOP_SKILL_STANDARD")]
	public partial class SopSkillStandard
	{
		/// <summary>
		/// ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 工序ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal OPERATION_ID {get;set;}

		/// <summary>
		/// 技能
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String TRAIN_NAME {get;set;}

		/// <summary>
		/// 标准分值
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STANDARD {get;set;}


	}
}
