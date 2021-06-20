using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels.AssemblyOperation
{
    /// <summary>
    /// 过站采集请求对象
    /// </summary>
    public class CPCheckData
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public decimal SiteID { get; set; } = 0;

        /// <summary>
        /// 工单号
        /// </summary>
        public string WO_NO { get; set; } = "";

        /// <summary>
        /// 历史数据Top数（默认: 5）
        /// </summary>
        public int History_Top { get; set; } = 5;

        /// <summary>
        /// 不良数据Top数（默认: 5）
        /// </summary>
        public int Defect_Top { get; set; } = 5;
    }

    /// <summary>
    /// 无码报工请求对象
    /// </summary>
    public class ReportData
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public decimal SiteID { get; set; } = 0;

        /// <summary>
        /// 工单号
        /// </summary>
        public string WO_NO { get; set; } = "";

        /// <summary>
        /// 产能数量
        /// </summary>
        public decimal CapacityReportQty { get; set; } = 0;

        /// <summary>
        /// 不良数据
        /// </summary>
        public decimal DefectReportQty { get; set; } = 0;

        /// <summary>
        /// 不良代码
        /// </summary>
        public string DEFECT_CODE { get; set; } = "";

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserName { get; set; } = "";//chenhx or ZX...
    }
}
