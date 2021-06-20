/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-19 10:21:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesQualityDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-05-19 10:21:45
	/// 
	/// </summary>
	[Table("MES_QUALITY_DETAIL")]
	public partial class MesQualityDetail
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 明细ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal DETAIL_ID {get;set;}

		/// <summary>
		/// 检验事项ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ITEM_ID {get;set;}

		/// <summary>
		/// 检验结果值
		/// </summary>
		[MaxLength(50)]
		public String RESULT_VALUE {get;set;}

        /// <summary>
        /// 检验结果
        /// </summary>
        [Required]
        [MaxLength(1)]
        public String RESULT_TYPE { get; set; } = "Y";
    }
}
