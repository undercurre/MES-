/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:46                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsMessageConfig                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-30 10:44:46
	/// 
	/// </summary>
	[Table("SFCS_MESSAGE_CONFIG")]
	public partial class SfcsMessageConfig
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 消息编号
		/// </summary>
		[MaxLength(20)]
		public String MESSAGE_NO {get;set;}

		/// <summary>
		/// 消息内容(中文)
		/// </summary>
		[MaxLength(200)]
		public String CHINESE_MESSAGE {get;set;}

		/// <summary>
		/// 消息内容(英文)
		/// </summary>
		[MaxLength(200)]
		public String ENGLISH_MESSAGE {get;set;}

		/// <summary>
		/// 参数个数
		/// </summary>
		[MaxLength(22)]
		public Decimal? PARAMETERS_COUNT {get;set;}

		/// <summary>
		/// 种类
		/// </summary>
		[MaxLength(30)]
		public String CATEGORY {get;set;}

		/// <summary>
		/// 后台(pl/sql)标识
		/// </summary>
		[MaxLength(1)]
		public String BACKGROUND_FLAG {get;set;}

		/// <summary>
		/// 应用程序名称
		/// </summary>
		[MaxLength(50)]
		public String APPLICATION_NAME {get;set;}

	}
}
