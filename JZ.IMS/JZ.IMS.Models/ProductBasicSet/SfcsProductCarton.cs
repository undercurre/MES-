/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-02 10:58:33                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductCarton                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-02 10:58:33
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_CARTON")]
	public partial class SfcsProductCarton
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		[Required]
		[MaxLength(300)]
		public String FORMAT {get;set;}

		/// <summary>
		/// 标准容量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STANDARD_QUANTITY {get;set;}

		/// <summary>
		/// 最大容量
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAX_QUANTITY {get;set;}

		/// <summary>
		/// 最小容量
		/// </summary>
		[MaxLength(22)]
		public Decimal? MIN_QUANTITY {get;set;}

		/// <summary>
		/// 标准重量
		/// </summary>
		[MaxLength(30)]
		public String STANDARD_WEIGHT {get;set;}

		/// <summary>
		/// 最大重量
		/// </summary>
		[MaxLength(30)]
		public String MAX_WEIGHT {get;set;}

		/// <summary>
		/// 最小重量
		/// </summary>
		[MaxLength(30)]
		public String MIN_WEIGHT {get;set;}

		/// <summary>
		/// 标准体积
		/// </summary>
		[MaxLength(30)]
		public String CUBAGE {get;set;}

		/// <summary>
		/// 卡通长
		/// </summary>
		[MaxLength(30)]
		public String LENGTH {get;set;}

		/// <summary>
		/// 卡通宽
		/// </summary>
		[MaxLength(30)]
		public String WIDTH {get;set;}

		/// <summary>
		/// 卡通高
		/// </summary>
		[MaxLength(30)]
		public String HEIGHT {get;set;}

		/// <summary>
		/// 是否系统自动产生
		/// </summary>
		[MaxLength(30)]
		public String AUTO_CREATE_FLAG {get;set;}

		/// <summary>
		/// 采集工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? COLLECT_OPERATION_ID {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}


	}
}
