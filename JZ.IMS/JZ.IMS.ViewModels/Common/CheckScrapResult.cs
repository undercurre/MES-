using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckScrapResult : CheckResult
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal STENCIL_ID { get; set; } = 0;

        /// <summary>
        /// 储位
        /// </summary>
        public string LOCATION { get; set; } = string.Empty;

        /// <summary>
		/// 创建时间
		/// </summary>
        public DateTime? CREATE_TIME { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UPDATE_TIME { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Decimal? STATUS { get; set; }

        /// <summary>
        /// 钢网是否投入使用, 如果没有是否投入使用, 保存时选择报废,则要提示:  钢网没有投入使用，是否需要强制报废？
        /// </summary>
        public bool IsRun { get; set; } = false;
    }
}
