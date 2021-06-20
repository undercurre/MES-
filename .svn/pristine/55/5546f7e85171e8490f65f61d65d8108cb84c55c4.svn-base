using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.Models
{
    /// <summary>
    /// 钢网清洗查询返回模型
    /// </summary>
    public class SmtStencilCleanModel
    {
		/// <summary>
		/// 打印次数
		/// </summary>
		public decimal PrintCount { get; set; } = 0;

		/// <summary>
		/// 最后清洗时间 
		/// </summary>
		public DateTime? LAST_CLEAN_TIME { get; set; }

		/// <summary>
		/// 是否张力值管控
		/// </summary>
		public string TENSION_CONTROL_FLAG { get; set; } = string.Empty;

		/// <summary>
		/// 最小张力值 
		/// </summary>
		public Decimal? TENSION_CONTROL_VALUE { get; set; }

		/// <summary>
		/// 钢网是否未使用过,注意: 未使用过要提示: 该钢网而未使用过，请问是否继续清洗?, 如果选否, 清除所有信息.
		/// </summary>
		public bool UnUsed { get; set; } = true;

		/// <summary>
		/// 张点力 上
		/// </summary>
		public Decimal TENSION_A { get; set; } = 0;

		/// <summary>
		/// 张点力 下
		/// </summary>
		public Decimal TENSION_B { get; set; } = 0;

		/// <summary>
		/// 张点力 左
		/// </summary>
		public Decimal TENSION_C { get; set; } = 0;

		/// <summary>
		/// 张点力 右
		/// </summary>
		public Decimal TENSION_D { get; set; } = 0;

		/// <summary>
		/// 张点力 中
		/// </summary>
		public Decimal TENSION_E { get; set; } = 0;
	}
}
