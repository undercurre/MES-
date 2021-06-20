/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：员工技能分值表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-23 18:43:32                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SopEmpTrainGrade                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-23 18:43:32
	/// 员工技能分值表
	/// </summary>
	[Table("SYS_EMPLOYEE_TRAIN_GRADE")]
	public partial class SysEmployeeTrainGrade
	{
		/// <summary>
		/// 员工工号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String USER_ID { get; set; }

		/// <summary>
		/// 技能
		/// </summary>
		[Required]
		[MaxLength(200)]
		public String TRAIN_NAME { get; set; }

		/// <summary>
		/// 分值
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal GRADE { get; set; }
	}
}
