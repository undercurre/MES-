using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 获取物料视图信息
    /// </summary>
    public class ReelInfoViewModel
    {
        public Decimal ID { get; set; }

        public Decimal? VERSION { get; set; }

        public String CODE { get; set; }

        public Decimal? BOX_ID { get; set; }

        public Decimal? VENDOR_ID { get; set; }

        public String VENDOR_CODE { get; set; }

        public Decimal PART_ID { get; set; }

        public String PART_NO { get; set; }

        public String PART_NAME { get; set; }

        public String PART_DESC { get; set; }

        public Decimal? MAKER_PART_ID { get; set; }

        public String MAKER_PART_NO { get; set; }

        public String DATE_CODE { get; set; }

        public String LOT_CODE { get; set; }

        public String REVISION { get; set; }

        public Decimal? PARENT_ID { get; set; }

        public String MSD_LEVEL { get; set; }

        public String ESD_FLAG { get; set; }

        public String SELF_GENERATE { get; set; }

        public String REGION { get; set; }

        public String SOFTWARE { get; set; }

        public String MAKER_CODE { get; set; }

        public String MAKER_NAME { get; set; }

        public String MAKER_DESC { get; set; }

        public String FIRMWARE { get; set; }

        public Decimal? CASE_QTY { get; set; }

        public String IQC_FLAG { get; set; }

        public String DESCRIPTION { get; set; }

        public String COO { get; set; }

        public String CUSTOMER_PN { get; set; }

        public String REFERENCE { get; set; }

        public Decimal? ORIGINAL_QUANTITY { get; set; }

        public String VENDOR_NAME { get; set; }
    }
}
