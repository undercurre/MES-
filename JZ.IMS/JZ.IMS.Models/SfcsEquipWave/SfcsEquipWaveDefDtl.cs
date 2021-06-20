/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-29 16:22:13                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipWaveDefDtl                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-29 16:22:13
	/// 
	/// </summary>
	[Table("SFCS_EQUIP_WAVE_DEF_DTL")]
	public partial class SfcsEquipWaveDefDtl
	{
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// MES_EQUIP_WAVE_DEFFECT_MST的ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal MST_ID {get;set;}

		/// <summary>
		/// 不良类型
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String TYPE {get;set;}

		/// <summary>
		/// 位号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String POINT {get;set;}

		/// <summary>
		/// 数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal COUNT {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String CREATE_USER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime CREATE_TIME {get;set;}

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
