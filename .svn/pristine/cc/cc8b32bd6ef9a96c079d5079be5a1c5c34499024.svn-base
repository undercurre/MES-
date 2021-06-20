/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：不良详细描述                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-27 17:17:30                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesSpotcheckFailDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-11-27 17:17:30
	/// 不良详细描述
	/// </summary>
	[Table("MES_SPOTCHECK_FAIL_DETAIL")]
	public partial class MesSpotcheckFailDetail
	{
		/// <summary>
		/// 唯一标识符
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 抽检明显关联ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SPOTCHECK_DETAIL_ID {get;set;}

		/// <summary>
		/// 不良代码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String DEFECT_CODE {get;set;}

		/// <summary>
		/// 不良位号
		/// </summary>
		[MaxLength(50)]
		public String DEFECT_LOC {get;set;}

		/// <summary>
		/// 不良描述
		/// </summary>
		[MaxLength(100)]
		public String DEFECT_DESCRIPTION {get;set;}

		/// <summary>
		/// 不良现象
		/// </summary>
		[MaxLength(50)]
		public String DEFECT_MSG {get;set;}


	}
}
