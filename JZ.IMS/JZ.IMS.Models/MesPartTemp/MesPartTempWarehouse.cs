using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-02 14:23:28
	/// 辅料暂存仓数据表
	/// </summary>
	[Table("MES_PART_TEMP_WAREHOUSE")]
	public partial class MesPartTempWarehouse
	{
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public Decimal ID { get; set; }

		/// <summary>
		/// 公司代码
		/// </summary>
		[MaxLength(30)]
		public String COMPANY_CODE { get; set; }

		/// <summary>
		/// 物料类型（锡条/三防漆/助悍剂）
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal PART_TYPE { get; set; }

		/// <summary>
		/// 物料编码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PART_NO { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		[Required]
		[MaxLength(22)]
		public Decimal QTY { get; set; }

		/// <summary>
		/// 单位（KG/条/桶）
		/// </summary>
		[Required]
		[MaxLength(10)]
		public String UNIT { get; set; }

		/// <summary>
		/// 是否可用（N/Y）
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String ENABLED { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(200)]
		public String REMARK { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		[Required]
		[MaxLength(30)]
		public String CREATE_USER { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[Required]
		public DateTime CREATE_TIME { get; set; }


	}
}
