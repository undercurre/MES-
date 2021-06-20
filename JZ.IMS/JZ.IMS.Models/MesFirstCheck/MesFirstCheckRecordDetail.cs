/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-25 15:55:44                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesFirstCheckRecordDetail                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-25 15:55:44
	/// 
	/// </summary>
	[Table("MES_FIRST_CHECK_RECORD_DETAIL")]
	public partial class MesFirstCheckRecordDetail
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public String ID {get;set;}

		/// <summary>
		/// 首件测试编号
		/// </summary>
		[MaxLength(50)]
		public String HID {get;set;}

		/// <summary>
		/// 位号
		/// </summary>
		[MaxLength(50)]
		public String POSITION {get;set;}

		/// <summary>
		/// 物料料号
		/// </summary>
		[MaxLength(50)]
		public String PART_NO {get;set; }

		/// <summary>
		/// 父物料料号
		/// </summary>
		[MaxLength(50)]
		public String PARENT_PART_NO { get; set; }

		/// <summary>
		/// 检测结果
		/// </summary>
		[MaxLength(50)]
		public String RESULT {get;set; }

		/// <summary>
		/// 拉力值
		/// </summary>
		[MaxLength(50)]
		public String TENSION_VALUE { get; set; }

		/// <summary>
		/// 品牌
		/// </summary>
		[MaxLength(50)]
		public String BRAND_NAME { get; set; }

		/// <summary>
		/// 供应商名称
		/// </summary>
		[MaxLength(100)]
		public String VENDOR_NAME { get; set; }

		/// <summary>
		/// 测试值
		/// </summary>
		[MaxLength(50)]
		public String TEST_VALUE { get; set; }


	}
}
