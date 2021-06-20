using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JZ.IMS.Models {
	[Table("ANDON_CALL_PERSON_CONFIG")]
	public class AndonCallPersonConfig {
		/// <summary>
		/// z主键
		/// </summary>
		[Key]
		public decimal ID { get; set; }

		/// <summary>
		/// ANDON_CALL_HANDLE_CONFIG.ID
		/// </summary>		
		[Required]
		public decimal MST_ID { get; set; }

		/// <summary>
		/// 通知接收人ID
		/// </summary>
		[Required]
		public decimal USER_ID { get; set; }
	}
}
