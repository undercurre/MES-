using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels.ProductBasicSet
{
   public class ProducLineEndVM
    {
        /// <summary>
        /// 线体ID
        /// </summary>
        public decimal lineId { get; set; }

        /// <summary>
		/// 工单号
		/// </summary>
		public string woNo { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string user { get; set; }
    }

    public class ProducLineEndLIst
    {
        public List<ProducLineEndVM> ProducLineArray { get; set; }
    }
}
