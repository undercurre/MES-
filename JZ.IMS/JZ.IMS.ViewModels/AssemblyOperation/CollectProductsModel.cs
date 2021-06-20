using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels.AssemblyOperation
{
    /// <summary>
    /// 过站采集返回对象
    /// </summary>
    public class CollectProductsModel
    {
        /// <summary>
        /// 产品料号
        /// </summary>
        public string Part_No { get; set; } = "";

        /// <summary>
        /// 工单ID
        /// </summary>
        public decimal Wo_Id { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string Wo_No { get; set; } = "";

        /// <summary>
        /// 品名
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 产品规格
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// 制程ID
        /// </summary>
        public decimal Route_Id { get; set; }

        /// <summary>
        /// 目标数量
        /// </summary>
        public Decimal? Target_Qty { get; set; } = 0;

        /// <summary>
        /// 待处理
        /// </summary>
        public int Pending_Qty { get; set; } = 0;

        /// <summary>
        /// 已处理
        /// </summary>
        public int Completed_Qty { get; set; } = 0;

        /// <summary>
        /// 良品
        /// </summary>
        public decimal? Pass_Qty { get; set; } = 0;

        /// <summary>
        /// 不良品
        /// </summary>
        public decimal? Fail_Qty { get; set; } = 0;

        /// <summary>
        /// 达成率
        /// </summary>
        public decimal? AchievementRate { get; set; } = 0;

        /// <summary>
        /// 历史数据
        /// </summary>
        public List<HistoricalDataModel> HistoricalData { get; set; } = new List<HistoricalDataModel>();

        /// <summary>
        /// 不良Top数据
        /// </summary>
        public List<FailDataModel> FailData { get; set; } = new List<FailDataModel>();

    }

    /// <summary>
    /// 历史数据
    /// </summary>
    public class HistoricalDataModel
    {
        /// <summary>
        /// SN
        /// </summary>
        public string SN_No { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public string Operation_Status { get; set; } = "";

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? Operation_Time { get; set; }

        /// <summary>
        /// 作业员
        /// </summary>
        public string Operator { get; set; } = "";
    }

    /// <summary>
    /// 不良Top
    /// </summary>
    public class FailDataModel
    {
        /// <summary>
        /// 不良品名称
        /// </summary>
        public string Fail_Name { get; set; } = "";

        /// <summary>
        /// 所占百分比
        /// </summary>
        public decimal Percentage { get; set; } = 0;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Fail_Qty { get; set; } = 0;
    }

}
