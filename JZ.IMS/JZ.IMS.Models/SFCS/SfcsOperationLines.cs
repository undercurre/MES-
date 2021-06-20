/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产线线体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-09-23 10:14:20                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsOperationLines                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-09-23 10:14:20
	/// 产线线体
	/// </summary>
	[Table("SFCS_OPERATION_LINES")]
	public partial class SfcsOperationLines
	{
		/// <summary>
		/// 线体的物理位置中文描述（from SFCS_PARAMETERS）
		/// </summary>					
		[NotMapped]
		public string LINE_NAME_INCN { get; set; }

		/// <summary>
		/// 厂别代码的中文描述（from SFCS_PARAMETERS）
		/// </summary>					
		[NotMapped]
		public string PLANT_CODE_INCN { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Key]
		public decimal Id { get; set; }

		/// <summary>
		/// 线体名称
		/// </summary>
		[MaxLength(40)]
		public string OPERATION_LINE_NAME { get; set; }

		/// <summary>
		/// 物料位置
		/// </summary>		
		public decimal PHYSICAL_LOCATION { get; set; }

		/// <summary>
		/// 线别序号
		/// </summary>
		public string LINE { get; set; }

		/// <summary>
		/// 厂别代码
		/// </summary>
		public decimal PLANT_CODE { get; set; }

		/// <summary>
		/// 激活状态
		/// </summary>
		[MaxLength(1)]
		public string ENABLED { get; set; }

		/// <summary>
		/// 组织ID
		/// </summary>
		public Decimal ORGANIZE_ID { get; set; }
	}
}
