using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 工单完工模型
    /// </summary>
    public class WOPassVM
    {
        /// <summary>
        /// 当前工单的时间完成数
        /// </summary>
        public decimal PassTotal { get; set; }

        /// <summary>
        /// 当前工单的总数
        /// </summary>
        public decimal TargetQty { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HourYieldRequestModel 
    {
        /// <summary>
        /// 线体id
        /// </summary>
        public int LINE_ID { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public Decimal WO_ID { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class HourYieldListModel
    {
        /// <summary>
        /// 标准产能
        /// </summary>
        public int STANDARD_CAPACITY { get; set; } = 0;

        public List<HourYieldDataListModel> YieldData { get; set; } = new List<HourYieldDataListModel>();

    }

    /// <summary>
    /// 
    /// </summary>
    public class HourYieldDataListModel
    {
        
        /// <summary>
        /// 时间
        /// </summary>
        public String WORK_HOUR { get; set; } = "";

        /// <summary>
        /// 每小时产能
        /// </summary>
        public int OUTPUT_QTY { get; set; } = 0;

        /// <summary>
        /// AOI不良
        /// </summary>
        public int AOI_FAIL { get; set; } = 0;

    }

    /// <summary>
    /// 
    /// </summary>
    public class SmtAOIDefectInfoListModel
    {
        /// <summary>
        /// 不良描述
        /// </summary>
        public String DEFECT_DESCRIPTION { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal DEFECT_QTY { get; set; } = 0;

        /// <summary>
        /// 百分比
        /// </summary>
        public decimal DEFECT_PERCENTAGE { get; set; } = 0;

    }

    /// <summary>
    /// 
    /// </summary>
    public class SMTOnlineDataListModel
    {
        /// <summary>
        /// 钢网
        /// </summary>
        public String STENCIL { get; set; } = "";

        /// <summary>
        /// 辅料
        /// </summary>
        public String RESOURCE { get; set; } = "";

        /// <summary>
        /// 刮刀
        /// </summary>
        public String SCRAPER_ONE { get; set; } = "";

        /// <summary>
        /// 刮刀
        /// </summary>
        public String SCRAPER_TWO { get; set; } = "";

    }


}
