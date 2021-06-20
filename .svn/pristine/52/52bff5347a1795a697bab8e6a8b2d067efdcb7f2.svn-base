/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-16 10:01:53                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SfcsEquipmentThrow                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-16 10:01:53
	/// 
	/// </summary>
	[Table("SFCS_EQUIPMENT_THROW")]
	public partial class SfcsEquipmentThrow
	{
		[Key]
		public Decimal ID {get;set;}


        /// <summary>
		/// 线体ID
		/// </summary>
		[Required]
        [MaxLength(22)]
        public Decimal LINE_ID { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        [Required]
		[MaxLength(50)]
		public String ORGANIZE_ID {get;set;}

        /// <summary>
        /// 组织ID
        /// </summary>
        [Required]
        [MaxLength(50)]
        public String WO_NO { get; set; }

        /// <summary>
        /// 抛料日期
        /// </summary>
        [Required]
		[MaxLength(7)]
		public DateTime THROW_DATE {get;set;}

		/// <summary>
		/// 抛料开始时间
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String START_TIME {get;set;}

		/// <summary>
		/// 抛料结束时间
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String END_TIME {get;set;}

		/// <summary>
		/// 产品料号
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 订单数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal ORDER_QTY {get;set;}

		/// <summary>
		/// 生产数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal TARGET_QTY {get;set;}

		/// <summary>
		/// 站点ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal SITE_ID {get;set;}

		/// <summary>
		/// 设备ID
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal EQUIPMENT_ID {get;set;}

		/// <summary>
		/// 抛料数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal THROW_QTY {get;set;}

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
		/// 线体
		/// </summary>
        [NotMapped]
        public String LINE_NAME { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        [NotMapped]
        public String TIME_SLOT { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        [NotMapped]
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [NotMapped]
        public String EQUIPMENT_NAME { get; set; }

        /// <summary>
        /// 抛料率
        /// </summary>
        [NotMapped]
        public String THROW_RATE { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public String MODEL { get; set; }
    }
}
