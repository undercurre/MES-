/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备基本信息表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 17:48:27                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipment                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2019-10-28 17:48:27
	/// 设备基本信息表
	/// </summary>
	[Table("SFCS_EQUIPMENT")]
	public partial class SfcsEquipment
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 设备编号
		/// </summary>
		[MaxLength(50)]
		public String NAME { get; set; }

		/// <summary>
		/// 设备名称
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal CATEGORY { get; set; }

		/// <summary>
		/// 资产编码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PROPERTY_NO { get; set; }

		/// <summary>
		/// 出厂编号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PRODUCT_NO { get; set; }

		/// <summary>
		/// 设备型号
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String MODEL { get; set; }

		/// <summary>
		/// 使用部门
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal USER_PART { get; set; }

		/// <summary>
		/// 存放地点(线别ID)
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATION_ID { get; set; }

		/// <summary>
		/// 电压/功率
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String POWER { get; set; }

		/// <summary>
		/// 生产产家
		/// </summary>
		[Required]
		[MaxLength(100)]
		public String VENDOR { get; set; }

		/// <summary>
		/// 进厂日期
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime BUY_TIME { get; set; }

		/// <summary>
		/// 使用年限
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal USER_AGE { get; set; }

		/// <summary>
		/// 报废时间
		/// </summary>
		[Required]
		[MaxLength(7)]
		public DateTime END_TIME { get; set; }

		/// <summary>
		/// 0:正常； 1: 闲置； 2:维修；3：报废
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal STATUS { get; set; }

		/// <summary>
		/// 是否有效（Y:有效，N：无效）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLE { get; set; }

		/// <summary>
		/// 部门名称
		/// </summary>
		[NotMapped]
		public string USER_PART_NAME { get; set; }

		/// <summary>
		/// 图片资源ID
		/// </summary>
		[NotMapped]
		public Decimal Img_Resource_id { get; set; }

		/// <summary>
		/// 图片URL
		/// </summary>
		[NotMapped]
		public string ImgUrl { get; set; }

		/// <summary>
		/// 图片名称
		/// </summary>
		[NotMapped]
		public String ImgName { get; set; }

		/// <summary>
		/// 图片大小
		/// </summary>
		[NotMapped]
		public Decimal ImgSize { get; set; }

        /// <summary>
        /// 唯一字段
        /// </summary>
        [MaxLength(30)]
        public string ONLY_CODE { get; set; }

    }
}
