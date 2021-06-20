using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels.ProductBasicSet.ComponentReplace
{
   public class ComponentReplaceViewModel
    {
        /// <summary>
        /// 原零件料号
        /// </summary>
        public string OldODMComponentPn { get; set; }

        /// <summary>
        /// 原零件规格
        /// </summary>
        public string OldDescription { get; set; }

        /// <summary>
        /// 原零件编号
        /// </summary>
        public string OldODMComponentSn { get; set; }

        /// <summary>
        /// 新零件料号
        /// </summary>
        public string NewODMComponentPn { get; set; }

        /// <summary>
        /// 新零件规格
        /// </summary>
        public string NewDescription { get; set; }

        /// <summary>
        /// 新零件编号
        /// </summary>
        public string NewODMComponentSn { get; set; }

        /// <summary>
        /// 采集工序
        /// </summary>
        public decimal CollectOperationID { get; set; }

        /// <summary>
        /// 成品料号
        /// </summary>
        public string PartNo { get; set; }


    }
}
