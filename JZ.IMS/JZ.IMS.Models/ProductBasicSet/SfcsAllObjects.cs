/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-31 16:22:05                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsAllObjects                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-31 16:22:05
	/// 
	/// </summary>
	[Table("SFCS_ALL_OBJECTS")]
	public partial class SfcsAllObjects
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
		/// 采集类型名称
		/// </summary>
		[MaxLength(30)]
		public String OBJECT_NAME {get;set;}

		/// <summary>
		/// 标记信息
		/// </summary>
		[MaxLength(30)]
		public String OBJECT_MARK {get;set;}

		/// <summary>
		/// 采集类型种类
		/// </summary>
		[MaxLength(22)]
		public Decimal? OBJECT_CATEGORY {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		[MaxLength(1)]
		public String ISACTIVE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(60)]
		public String DESCRIPTION {get;set;}

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


	}
}
