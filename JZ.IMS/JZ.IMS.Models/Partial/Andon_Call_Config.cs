using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JZ.IMS.Models {
	public partial class Andon_Call_Config {
		/// <summary>
		/// 呼叫类型代码对应的中文描述（为了与响应实体建立映射关系而添加，该字段来自其它表）
		/// </summary>
		[NotMapped]
		public String TYPE_CODE_INCN { get; set; }

		/// <summary>
		/// 线体ID对应的中文描述
		/// </summary>
		[NotMapped]
		public String LINE_ID_INCN { get; set; }

		/// <summary>
		/// 注意：这里是 工序ID 对应的中文描述
		/// </summary>
		[NotMapped]
		public String SITE_ID_INCN { get; set; }

		/// <summary>
		/// 用户ID
		/// </summary>
		[NotMapped]
		public String USER_ID { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[NotMapped]
		public String USER_NAME { get; set; }
	}
}
