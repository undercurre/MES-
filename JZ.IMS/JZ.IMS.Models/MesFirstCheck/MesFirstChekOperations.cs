/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：解锁记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-25 16:33:42                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesFirstChekOperations                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-25 16:33:42
	/// 解锁记录表
	/// </summary>
	[Table("MES_FIRST_CHEK_OPERATIONS")]
	public partial class MesFirstChekOperations
	{
		/// <summary>
		/// 唯一标识符
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 首件测试表头id
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CHECK_HEADER_ID {get;set;}

		/// <summary>
		/// 解锁操作员
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String UNLOCK_OPERATOR {get;set;}

		/// <summary>
		/// 解锁状态，Y：已解锁，N：未解锁
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String STATUS {get;set;}

		/// <summary>
		/// 解锁内容
		/// </summary>
		[MaxLength(2000)]
		public String CONTENT {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}


	}
}
