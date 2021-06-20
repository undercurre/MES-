/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：镭雕记录                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-23 09:15:48                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsLaserRecord                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-11-23 09:15:48
	/// 镭雕记录
	/// </summary>
	[Table("SFCS_LASER_RECORD")]
	public partial class SfcsLaserRecord
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 机器编码
		/// </summary>
		[MaxLength(50)]
		public String MACHINE_NO {get;set;}

		/// <summary>
		/// 镭雕时间
		/// </summary>
		[MaxLength(50)]
		public String LASER_TIME {get;set;}

		/// <summary>
		/// 基板名称
		/// </summary>
		[MaxLength(50)]
		public String LOT_NO {get;set;}

		/// <summary>
		/// 连板ID（一个整版一个ID）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PANEL_ID {get;set;}

		/// <summary>
		/// SN
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String SN {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 拼版数
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MULTI_NO {get;set;}

		/// <summary>
		/// 是否已投产
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String IS_INPUT {get;set;}

		/// <summary>
		/// 投产时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? INPUT_TIME {get;set;}

		/// <summary>
		/// 是否无效（Y/N），后续SN与之前的存在重复，则之前的整版SN设置为无效
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String IS_INVALID {get;set;}

		/// <summary>
		/// 无效时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? INVALID_TIME {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE1 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE2 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE3 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE4 {get;set;}

		[MaxLength(50)]
		public String ATTRIBUTE5 {get;set;}


	}
}
