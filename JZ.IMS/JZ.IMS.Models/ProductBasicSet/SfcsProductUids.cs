/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-31 18:04:38                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsProductUids                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-31 18:04:38
	/// 
	/// </summary>
	[Table("SFCS_PRODUCT_UIDS")]
	public partial class SfcsProductUids
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		[MaxLength(22)]
		public Decimal? VERSION {get;set;}

		[MaxLength(50)]
		public String ENABLE_BILL_ID {get;set;}

		[MaxLength(50)]
		public String DISABLE_BILL_ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String PART_NO {get;set;}

		/// <summary>
		/// UID种类
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal UID_ID {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		[Required]
		[MaxLength(300)]
		public String DATA_FORMAT {get;set;}

		/// <summary>
		/// 收集数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal UID_QTY {get;set;}

		/// <summary>
		/// 是否唯一序列
		/// </summary>
		[MaxLength(1)]
		public String SERIALIZED {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 是否返工清除
		/// </summary>
		[MaxLength(1)]
		public String REWORK_REMOVE_FLAG {get;set;}

		/// <summary>
		/// 是否排序
		/// </summary>
		[MaxLength(1)]
		public String ORDERED {get;set;}

		/// <summary>
		/// 是否EDI
		/// </summary>
		[MaxLength(1)]
		public String EDI_FLAG {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(30)]
		public String ATTRIBUTE5 {get;set;}

		/// <summary>
		/// 采集工序
		/// </summary>
		[MaxLength(22)]
		public Decimal? COLLECT_OPERATION_ID {get;set;}
		/// <summary>
		/// 解邦工序
		/// </summary>
		public Decimal? BREAK_OPERATION_ID { get; set; }


	}
}
