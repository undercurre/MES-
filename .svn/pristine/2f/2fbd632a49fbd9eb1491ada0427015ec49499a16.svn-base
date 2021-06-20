/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：看板控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-13 11:41:58                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesKanbanController                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-12-13 11:41:58
	/// 看板控制器
	/// </summary>
	[Table("MES_KANBAN_CONTROLLER")]
	public partial class MesKanbanControllerModel
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 车间类型(SMT/PCBA)
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String LINE_TYPE {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal LINE_ID {get;set;}

		/// <summary>
		/// 是否启用人工控制
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED {get;set;}

		/// <summary>
		/// 修改人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String MODIFIER {get;set;}

		/// <summary>
		/// 修改时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime MODIFY_TIME {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		[MaxLength(50)]
		public String DESCRIPTION {get;set;}


	}
}
