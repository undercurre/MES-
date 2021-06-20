using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class SiteStatisticsListModel { }
    /// <summary>
    /// 站点统计报表
    /// </summary>
    public class SiteStatisticsTableListModel
    {
        public Decimal OPERATION_ID { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// PASS
        /// </summary>
        public Decimal? PASS { get; set; }

        /// <summary>
        /// FAIL
        /// </summary>
        public Decimal? FAIL { get; set; }

        /// <summary>
        /// REPASS
        /// </summary>
        public Decimal? REPASS { get; set; }

        /// <summary>
        /// REFAIL
        /// </summary>
        public Decimal? REFAIL { get; set; }

        /// <summary>
        /// YIELD
        /// </summary>
        public Decimal? YIELD { get; set; }

        /// <summary>
        /// TOTAL
        /// </summary>
        public Decimal? TOTAL { get; set; }

    }

    /// <summary>
    /// 统计报表查询实体（通用）
    /// </summary>
    public class StatisticalReportRequestModel
    {

        /// <summary>
        /// 全部
        /// </summary>
        public bool ALL { get; set; } = true;

        /// <summary>
        /// 机种
        /// </summary>
        public List<String> MODEL { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public List<String> PART_NO { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public List<String> WO_NO { get; set; }

        ///// <summary>
        ///// RMA单据号
        ///// </summary>
        //public String RMA_NO { get; set; }

        /// <summary>
        /// 线别ID
        /// </summary>
        public List<String> LINE_ID { get; set; }

        /// <summary>
        /// 制程ID
        /// </summary>
        public List<String> ROUTE_ID { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime BEGIN_TIME { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime END_TIME { get; set; }

        /// <summary>
        /// 排除RMA工单 （站点统计报表）
        /// </summary>
        public bool WITHOUTRMAWO { get; set; } = true;

        /// <summary>
        /// 是否获取每小时明细数据（只有查询站点统计报表明细数据时使用）
        /// </summary>
        public bool HOURDETAIL { get; set; } = false;

        /// <summary>
        /// 是否获取在制品报表产品统计数据
        /// </summary>
        public bool ISPRODUCT { get; set; } = false;

        /// <summary>
        /// 是否获取在制品报表超时数据
        /// </summary>
        public bool ISLAGTIME { get; set; } = false;

        /// <summary>
        /// 超时统计 0: 小于24H; 1: 24至48H; 2: 48至72H; 3: 大于72H; 
        /// 查询在制品超时数据时必填
        /// </summary>
        public int LAGTIME { get; set; } = -1;


    }

    /// <summary>
    /// 
    /// </summary>
    public class StatisticsConditionRequestModel : PageModel
    {
        /// <summary>
        /// 必填：1 机种 2 料号 3 工单 4 线别 5 制程 6 维修员 7 流水号 8 楼层
        /// </summary>
        public int TYPE { get; set; } = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public class WhereListModel
    {
        /// <summary>
        /// 类型: 1 机种 2 料号 3 工单 4 线别 5 制程
        /// </summary>
        public int TYPE { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public String VIEWNAME { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public String FIELDNAME { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public String WHERE { get; set; } = "";
    }

    /// <summary>
    /// 明细
    /// </summary>
    public class SiteStatisticsDetailListModel
    {
        /// <summary>
        /// 工序
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 作业时间
        /// </summary>
        public DateTime WORK_TIME { get; set; }

        /// <summary>
        /// 作业站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 机种
        /// </summary>
        public String MODEL { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// PASS
        /// </summary>
        public Decimal? PASS { get; set; }

        /// <summary>
        /// FAIL
        /// </summary>
        public Decimal? FAIL { get; set; }

        /// <summary>
        /// REPASS
        /// </summary>
        public Decimal? REPASS { get; set; }

        /// <summary>
        /// REFAIL
        /// </summary>
        public Decimal? REFAIL { get; set; }

        /// <summary>
        /// YIELD
        /// </summary>
        public Decimal? YIELD { get; set; }

    }

    /// <summary>
    /// 在制品报表
    /// </summary>
    public class WipStatisticsListModel
    {
        /// <summary>
        /// 数据表格
        /// </summary>
        public List<WipStatisticsDataTableListModel> DATATABLE { get; set; } = new List<WipStatisticsDataTableListModel>();

        /// <summary>
        /// 产品统计
        /// </summary>
        public List<WipStatisticsProductListModel> DATAPRODUCT { get; set; } = new List<WipStatisticsProductListModel>();

        /// <summary>
        /// 超时明细
        /// </summary>
        public List<WipStatisticsDetailListModel> DATADETAILS { get; set; } = new List<WipStatisticsDetailListModel>();
    }

    /// <summary>
    /// 在制品报表数据表格
    /// </summary>
    public class WipStatisticsDataTableListModel
    {

        public Decimal OPERATION_ID { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? QTY { get; set; }
    }

    /// <summary>
    /// 在制品报表产品统计
    /// </summary>
    public class WipStatisticsProductListModel
    {
        /// <summary>
        /// 机种
        /// </summary>
        public String MODEL { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public String PART_NO { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? QTY { get; set; }
    }

    /// <summary>
    /// 在制品报表数据超时明细
    /// </summary>
    public class WipStatisticsDetailListModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public String PART_NO { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 机种
        /// </summary>
        public String MODEL { get; set; }

        /// <summary>
        /// 卡通号
        /// </summary>
        public String CARTON_NO { get; set; }

        /// <summary>
        /// 栈板号
        /// </summary>
        public String PALLET_NO { get; set; }

        /// <summary>
        /// 延迟时间（h）
        /// </summary>
        public String OPERATION_TIME { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public String STATUS { get; set; }
    }

    /// <summary>
    /// 不良维修报表
    /// </summary>
    public class DefectReportRequestModel
    {
        /// <summary>
        /// 全部
        /// </summary>
        public bool ALL { get; set; } = true;

        /// <summary>
        /// 机种
        /// </summary>
        public List<String> MODEL { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public List<String> PART_NO { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public List<String> WO_NO { get; set; }

        /// <summary>
        /// 维修员
        /// </summary>
        public List<String> USERS { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public List<String> SN { get; set; }

        /// <summary>
        /// 楼层
        /// </summary>
        public List<String> FLOOR { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime BEGIN_TIME { get; set; } = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime END_TIME { get; set; } = DateTime.Now;

        /// <summary>
        /// 1: 系统厂;2: 主板厂;
        /// </summary>
        public int PLANTTYPE { get; set; } = 0;

        /// <summary>
        /// 1: WIP报表; 2: 维修报表; 3: 坏件报表;
        /// </summary>
        public int REPORTTYPE { get; set; } = 3;//默认3

    }

    /// <summary>
    /// 不良维修报表
    /// </summary>
    public class DefectReportListModel
    {

        /// <summary>
        /// 流水ID
        /// </summary>
        public Decimal SN_ID { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// PSN
        /// </summary>
        public String PSN { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public String STATUS { get; set; }

        /// <summary>
        /// 当前站点
        /// </summary>
        public String CURRENT_SITE { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public String PART_NO { get; set; }

        /// <summary>
        /// 机种
        /// </summary>
        public String MODEL { get; set; }

        /// <summary>
        /// 投产时间
        /// </summary>
        public String INPUT_TIME { get; set; }

        /// <summary>
        /// 维修站点
        /// </summary>
        public String REPAIR_SITE { get; set; }

        /// <summary>
        /// 维修工序
        /// </summary>
        public String REPAIR_OPERATION_NAME { get; set; }

        /// <summary>
        /// 不良代码
        /// </summary>
        public String DEFECT_CODE { get; set; }

        /// <summary>
        /// 不良代码英文描述
        /// </summary>
        public String DEFECT_EN_DESCRIPTION { get; set; }

        /// <summary>
        /// 不良代码中文描述
        /// </summary>
        public String DEFECT_CN_DESCRIPTION { get; set; }

        /// <summary>
        /// 不良站点
        /// </summary>
        public String DEFECT_SITE { get; set; }

        /// <summary>
        /// 不良发生时间
        /// </summary>
        public String DEFECT_TIME_TEXT { get; set; }

        /// <summary>
        /// 维修时间
        /// </summary>
        public String REPAIR_TIME_TEXT { get; set; }

        /// <summary>
        /// Lead Time(单位：天)
        /// </summary>
        public String LEAD_TIME { get; set; }

        /// <summary>
        /// 维修员
        /// </summary>
        public String REPAIRER { get; set; }

        /// <summary>
        /// 产线刷不良人员
        /// </summary>
        public String DEFECT_OPERATOR { get; set; }

        /// <summary>
        /// Defect Description
        /// </summary>
        public String DEFECT_DESCRIPTION { get; set; }

        /// <summary>
        /// 维修代码
        /// </summary>
        public String REASON_CODE { get; set; }

        /// <summary>
        /// 维修代码英文描述
        /// </summary>
        public String REASON_EN_DESCRIPTION { get; set; }

        /// <summary>
        /// 维修代码中文描述
        /// </summary>
        public String REASON_CN_DESCRIPTION { get; set; }

        /// <summary>
        /// Debug
        /// </summary>
        public String RESPONSER { get; set; }

        /// <summary>
        /// 不良位置
        /// </summary>
        public String LOCATION { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        public String ASSEMBLY_KIND { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public String REMARK { get; set; }

        /// <summary>
        /// 根本原因分类
        /// </summary>
        public String ROOT_CAUSE_CATEGORY { get; set; }

        /// <summary>
        /// 根本原因分类描述
        /// </summary>
        public String CAUSE_CATEGORY_DESCRIPTION { get; set; }

        /// <summary>
        /// 坏料
        /// </summary>
        public String BAD_PART_NO { get; set; }

        /// <summary>
        /// 替换料device数据值
        /// </summary>
        public String REPLACED_PN_DEVICE_VALUE { get; set; }

        /// <summary>
        /// 更换物料料卷
        /// </summary>
        public String REEL_ID { get; set; }

        /// <summary>
        /// 进维修室时间
        /// </summary>
        public String REPAIR_IN_TIME_TEXT { get; set; }

        /// <summary>
        /// 刷进维修室作业员
        /// </summary>
        public String REPAIR_IN_OPERATOR { get; set; }

        /// <summary>
        /// 出维修室时间
        /// </summary>
        public String REPAIR_OUT_TIME_TEXT { get; set; }

        /// <summary>
        /// 刷出维修室作业员
        /// </summary>
        public String REPAIR_OUT_OPERATOR { get; set; }

        /// <summary>
        /// Action Code
        /// </summary>
        public String ACTION_CODE { get; set; }

        /// <summary>
        /// Action Code Description
        /// </summary>
        public String ACTION_EN_DESCRIPTION { get; set; }

        /// <summary>
        /// TTF
        /// </summary>
        public String TTF { get; set; }

        /// <summary>
        /// 日期代码
        /// </summary>
        public String DATE_CODE { get; set; }

        /// <summary>
        /// 采集不良详细信息
        /// </summary>
        public String COLLECT_DEFECT_DETAIL { get; set; }

        /// <summary>
        /// 测试治具
        /// </summary>
        public String FIXTURE { get; set; }

        /// <summary>
        /// 测试机台
        /// </summary>
        public String MACHINE { get; set; }

        /// <summary>
        /// 测试用时
        /// </summary>
        public String TEST_TIME_COST { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        public String CREATE_TIME { get; set; }

        /// <summary>
        /// 工单Lead Time(单位：天)
        /// </summary>
        public String WO_LEAD_TIME { get; set; }

        /// <summary>
        /// 二次返修分析
        /// </summary>
        public String RE_REPAIR_MARK { get; set; }

        /// <summary>
        /// PCB_DATECODE
        /// </summary>
        public String PCB_DATECODE { get; set; }

        /// <summary>
        /// PCB_VENDOR
        /// </summary>
        public String PCB_VENDOR { get; set; }

        /// <summary>
        /// 更换物料LOT_CODE
        /// </summary>
        public String LOT_CODE { get; set; }

        /// <summary>
        /// COLLECT_DEFECT_DETAIL_ID
        /// </summary>
        public String COLLECT_DEFECT_DETAIL_ID { get; set; }

        /// <summary>
        /// SMT或已替换料卷编号
        /// </summary>
        public String SMT_REELCODE { get; set; }

        /// <summary>
        /// 更换物料MAKER
        /// </summary>
        public String MAKER_NAME { get; set; }

        /// <summary>
        /// 更换前REEL CODE
        /// </summary>
        public String OLD_REEL_CODE { get; set; }

        /// <summary>
        /// 更换前物料DATE CODE
        /// </summary>
        public String OLD_DATE_CODE { get; set; }

        /// <summary>
        /// 更换前物料LOT CODE
        /// </summary>
        public String OLD_LOT_CODE { get; set; }

        /// <summary>
        /// 更换前物料MAKER
        /// </summary>
        public String OLD_MAKER { get; set; }

    }

    /// <summary>
    /// 旧物料信息
    /// </summary>
    public class DefectOldMakerListModel 
    {
        /// <summary>
        /// 
        /// </summary>
        public String CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String LOT_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String DATE_CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String NAME { get; set; }
    }

    /// <summary>
    /// ReportDetail
    /// </summary>
    public class ReportDetailRequestModel
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public int CONDITION { get; set; } = 0;

        /// <summary>
        /// 工序ID （必填）
        /// </summary>
        public Decimal OPERATION_ID { get; set; }

        /// <summary>
        /// 全部
        /// </summary>
        public bool ALL { get; set; } = true;

        /// <summary>
        /// 机种
        /// </summary>
        public List<String> MODEL { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public List<String> PART_NO { get; set; }

        /// <summary>
        /// 排除RMA工单 （站点统计报表,只有料号查询时才能使用）
        /// </summary>
        public bool WITHOUTRMAWO { get; set; } = true;

        /// <summary>
        /// 工单号
        /// </summary>
        public List<String> WO_NO { get; set; }

        /// <summary>
        /// 线别ID
        /// </summary>
        public List<String> LINE_ID { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime BEGIN_TIME { get; set; } = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime END_TIME { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否获取在制品报表超时数据
        /// </summary>
        public bool ISLAGTIME { get; set; } = false;

        /// <summary>
        /// 超时统计 0: 小于24H; 1: 24至48H; 2: 48至72H; 3: 大于72H; 
        /// 查询在制品超时数据时必填
        /// </summary>
        public int LAGTIME { get; set; } = -1;
    }

    /// <summary>
    /// 站点统计ReportDetail
    /// </summary>
    public class SiteStatisticsReportDetailListModel
    {
        /// <summary>
        /// 机种
        /// </summary>
        public String MODEL { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public String PART_NO { get; set; }

        /// <summary>
        /// 作业站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 作业时间
        /// </summary>
        public DateTime WORK_TIME { get; set; }

        /// <summary>
        /// PASS
        /// </summary>
        public Decimal? PASS { get; set; }

        /// <summary>
        /// FAIL
        /// </summary>
        public Decimal? FAIL { get; set; }

        /// <summary>
        /// REPASS
        /// </summary>
        public Decimal? REPASS { get; set; }

        /// <summary>
        /// REFAIL
        /// </summary>
        public Decimal? REFAIL { get; set; }

        /// <summary>
        /// YIELD
        /// </summary>
        public Decimal? YIELD { get; set; }
    }

    /// <summary>
    /// 在制品ReportDetail
    /// </summary>
    public class WipStatisticsReportDetailListModel
    {
        /// <summary>
        /// 产品流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public String STATUS { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 卡通号（箱号）
        /// </summary>
        public String CARTON_NO { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public String PART_NO { get; set; }

        /// <summary>
        /// 机种
        /// </summary>
        public String MODEL { get; set; }

        /// <summary>
        /// 作业时间
        /// </summary>
        public DateTime WORK_TIME { get; set; }
    }

}
