using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels.ProductBasicSet
{
   public class HiReelInfoVM
    {
        /// <summary>
        /// 线体ID
        /// </summary>
        public string OperationLineId { get; set; }

        /// <summary>
		/// 工单号
		/// </summary>
		public string WoNo { get; set; }

        /// <summary>
        /// 料卷
        /// </summary>
        public string ReelId { get; set;}
        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; }
    }
}
