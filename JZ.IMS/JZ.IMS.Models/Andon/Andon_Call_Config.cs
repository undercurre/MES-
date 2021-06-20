using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models {
	public partial class Andon_Call_Config {
		/// <summary>
		/// 表ID
		/// </summary>
		[Key]
		public decimal ID { get; set; }

		/// <summary>
		/// 
		/// </summary>		

		public decimal VERSION { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MaxLength(50)]
		public string ENABLE_BILL_ID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[MaxLength(50)]
		public string DISABLE_BILL_ID { get; set; }

		/// <summary>
		/// 线体ID
		/// </summary>
		[Required]
		public decimal OPERATION_LINE_ID { get; set; }

		/// <summary>
		/// 注意：这里是 - 工序ID
		/// </summary>
		[Required]
		public decimal OPERATION_SITE_ID { get; set; }

		/// <summary>
		/// 呼叫类型代码
		/// </summary>
		[Required]
		public string CALL_TYPE_CODE { get; set; }

		/// <summary>
		/// 是否激活
		/// </summary>
		[MaxLength(1)]
		[Required]
		public string ENABLED { get; set; }		
	}
}
