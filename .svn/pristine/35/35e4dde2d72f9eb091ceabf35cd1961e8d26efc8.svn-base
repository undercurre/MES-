using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 流程卡报表实体
    /// </summary>
    public class RuncardReportListModel
    {
        /// <summary>
        /// 流水号ID
        /// </summary>
        public String SN_ID { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// Parent SN
        /// </summary>
        public String PARENT_SN { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public String PART_NO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String OEM_PN { get; set; }

        /// <summary>
        /// 客户料号
        /// </summary>
        public String CUSTOMER_PN { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public String CUSTOMER { get; set; }

        /// <summary>
        /// 机种
        /// </summary>
        public String MODEL { get; set; }

        /// <summary>
        /// 制程ID
        /// </summary>
        public String ROUTE_ID { get; set; }

        /// <summary>
        /// 制程名称
        /// </summary>
        public String ROUTE_NAME { get; set; }

        /// <summary>
        /// 抽检
        /// </summary>
        public String SAMPLE_FLAG { get; set; }

        /// <summary>
        /// 最后作业站
        /// </summary>
        public String CURRENT_SITE { get; set; }

        /// <summary>
        /// 卡通号
        /// </summary>
        public String CARTON_NO { get; set; }

        /// <summary>
        /// 栈板
        /// </summary>
        public String PALLET_NO { get; set; }

        /// <summary>
        /// 下一道工序
        /// </summary>
        public String WIP_ROUTE { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public String RUNCARD_STATUS { get; set; }

        /// <summary>
        /// 投产时间
        /// </summary>
        public DateTime? INPUT_TIME { get; set; }

        /// <summary>
        /// 作业记录
        /// </summary>
        public IEnumerable<OperationHistoryListModel> OPERATIONHISTORY { get; set; }

        /// <summary>
        /// 不良
        /// </summary>
        public IEnumerable<DefectHistoryListModel> DEFECTHISTORY { get; set; }

        /// <summary>
        /// 制程
        /// </summary>
        public IEnumerable<RouteHistoryListModel> ROUTEHISTORY { get; set; }

        /// <summary>
        /// 测试记录
        /// </summary>
        public IEnumerable<TestHistoryListModel> TESTHISTORY { get; set; }

        /// <summary>
        /// 零件
        /// </summary>
        public IEnumerable<ComponentHistoryListModel> COMPONENTHISTORY { get; set; }

        /// <summary>
        /// 替换零件
        /// </summary>
        public IEnumerable<CompReplaceHistoryListModel> COMPREPLACEHISTORY { get; set; }

        /// <summary>
        /// 返工前零件
        /// </summary>
        public IEnumerable<ReworkCompHistoryListModel> REWORKCOMPHISTORY { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        public IEnumerable<UidHistoryListModel> UIDHISTORY { get; set; }

        /// <summary>
        /// 流水号替换记录
        /// </summary>
        public IEnumerable<ReplaceRuncardHistoryListModel> REPLACERUNCARDHISTORY { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public IEnumerable<ResourceHistoryListModel> RESOURCEHISTORY { get; set; }

        /// <summary>
        /// RMA
        /// </summary>
        public IEnumerable<RmaHistoryListModel> RMAHISTORY { get; set; }

        /// <summary>
        /// 工单记录
        /// </summary>
        public IEnumerable<WOReplaceHistoryListModel> WOHISTORY { get; set; }

        /// <summary>
        /// 零件采集记录
        /// </summary>
        public IEnumerable<SfcsCollectComponentsListModel> COLLECTCOMPONENTS { get; set; }

        /// <summary>
        /// 资源采集记录
        /// </summary>
        public IEnumerable<SfcsCollectResourcesListModel> COLLECTRESOURCES { get; set; }

        /// <summary>
        /// 唯一标识码采集记录
        /// </summary>
        public IEnumerable<SfcsCollectUidsListModel> COLLECTUIDS { get; set; }

        /// <summary>
        /// 钢网
        /// </summary>
        public IEnumerable<StencilListModel> STENCIL { get; set; }

        /// <summary>
        /// 刮刀
        /// </summary>
        public IEnumerable<ScraperListModel> SCRAPER { get; set; }

        /// <summary>
        /// 辅料
        /// </summary>
        public IEnumerable<ResourceListModel> RESOURCE { get; set; }

        /// <summary>
        /// 料卷信息
        /// </summary>
        public IEnumerable<SNReelListModel> SMTSNREEL { get; set; }
    }

    /// <summary>
    /// 作业记录
    /// </summary>
    public class OperationHistoryListModel
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 制程名称
        /// </summary>
        public String ROUTE_NAME { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 工序描述
        /// </summary>
        public String OPERATION_DESCRIPTION { get; set; }

        /// <summary>
        /// 站点 (站点名称)
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 作业员
        /// </summary>
        public String OPERATOR { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public String DESCRIPTION { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public String OPERATION_TIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String VISIT_NUMBER { get; set; }
    }

    /// <summary>
    /// 不良
    /// </summary>
    public class DefectHistoryListModel
    {
        /// <summary>
        /// 不良代码
        /// </summary>
        public String DEFECT_CODE { get; set; }

        /// <summary>
        /// 不良描述
        /// </summary>
        public String DEFECT_DESCRIPTION { get; set; }

        /// <summary>
        /// 发生不良站点
        /// </summary>
        public String DEFECT_SITE { get; set; }

        /// <summary>
        /// 发生不良时间
        /// </summary>
        public String DEFECT_TIME { get; set; }

        /// <summary>
        /// 刷入不良作业员
        /// </summary>
        public String DEFECT_OPERATOR { get; set; }

        /// <summary>
        /// 根本原因分类
        /// </summary>
        public String ROOT_CAUSE_CATEGORY { get; set; }

        /// <summary>
        /// 原因代码
        /// </summary>
        public String REASON_CODE { get; set; }

        /// <summary>
        /// 原因描述
        /// </summary>
        public String REASON_DESCRIPTION { get; set; }

        /// <summary>
        /// 不良详细描述
        /// </summary>
        public String DEFECT_DETAIL_DESCRIPTION { get; set; }

        /// <summary>
        /// 维修站点
        /// </summary>
        public String REPAIR_SITE { get; set; }

        /// <summary>
        /// 维修作业员
        /// </summary>
        public String REPAIRER { get; set; }

        /// <summary>
        /// 检验作业员
        /// </summary>
        public String CHECK_OPERATOR { get; set; }

        /// <summary>
        /// 维修时间
        /// </summary>
        public String REPAIR_TIME { get; set; }

        /// <summary>
        /// 进维修室时间
        /// </summary>
        public String REPAIR_IN_TIME { get; set; }

        /// <summary>
        /// 刷进维修室作业员
        /// </summary>
        public String REPAIR_IN_OPERATOR { get; set; }

        /// <summary>
        /// 出维修室时间
        /// </summary>
        public String REPAIR_OUT_TIME { get; set; }

        /// <summary>
        /// 刷出维修室作业员
        /// </summary>
        public String REPAIR_OUT_OPERATOR { get; set; }

        /// <summary>
        /// 维修位置
        /// </summary>
        public String LOCATION { get; set; }

        /// <summary>
        /// NDF标识
        /// </summary>
        public String NDF_FLAG { get; set; }

        /// <summary>
        /// 是否已维修
        /// </summary>
        public String REPAIR_FLAG { get; set; }

        /// <summary>
        /// Debug
        /// </summary>
        public String RESPONSER { get; set; }

        /// <summary>
        /// 执行动作
        /// </summary>
        public String ACTION_CODE { get; set; }

        /// <summary>
        /// TTF
        /// </summary>
        public String TTF { get; set; }

        /// <summary>
        /// 坏件料号
        /// </summary>
        public String BAD_PART_NO { get; set; }

        /// <summary>
        /// 坏件供应商
        /// </summary>
        public String BAD_PART_VENDOR { get; set; }

        /// <summary>
        /// 料卷
        /// </summary>
        public String REEL_ID { get; set; }

    }

    /// <summary>
    /// 制程
    /// </summary>
    public class RouteHistoryListModel
    {
        /// <summary>
        /// 当前工位
        /// </summary>
        public String CURRENT_OPERATION { get; set; }

        /// <summary>
        /// 对应维修工位
        /// </summary>
        public String REPAIR_OPERATION { get; set; }

        /// <summary>
        /// 维修后返回工位
        /// </summary>
        public String REWORK_OPERATION { get; set; }

    }

    /// <summary>
    /// 测试记录
    /// </summary>
    public class TestHistoryListModel
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 测试站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        public String CREATE_TIME { get; set; }

        /// <summary>
        /// 测试工位
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 测试作业员
        /// </summary>
        public String TEST_OPERATOR { get; set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        public String STATUS { get; set; }

        /// <summary>
        /// 测试耗时
        /// </summary>
        public String TEST_TIME_COST { get; set; }

        /// <summary>
        /// 是否需要重测(不过站)
        /// </summary>
        public String RETEST { get; set; }

        /// <summary>
        /// 首次测试标记(含假Fail)
        /// </summary>
        public String FIRST_TEST { get; set; }

        /// <summary>
        /// 系统首次处理标记(不含假Fail)
        /// </summary>
        public String FIRST_PROCESS_OPERATION { get; set; }

        /// <summary>
        /// 治具
        /// </summary>
        public String FIXTURE { get; set; }

        /// <summary>
        /// 机台
        /// </summary>
        public String MACHINE { get; set; }

        /// <summary>
        /// 测试程序名称
        /// </summary>
        public String TEST_PROGRAM { get; set; }

        /// <summary>
        /// 测试项
        /// </summary>
        public String ITEAM { get; set; }

        /// <summary>
        /// 测试值
        /// </summary>
        public String VALUE { get; set; }
    }

    /// <summary>
    /// 零件
    /// </summary>
    public class ComponentHistoryListModel
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 零件名称
        /// </summary>
        public String COMPONENT_NAME { get; set; }

        /// <summary>
        /// 客户序号
        /// </summary>
        public String CUSTOMER_COMPONENT_SN { get; set; }

        /// <summary>
        /// 客户料号
        /// </summary>
        public String CUSTOMER_COMPONENT_PN { get; set; }

        /// <summary>
        /// 本厂料号
        /// </summary>
        public String ODM_COMPONENT_PN { get; set; }

        /// <summary>
        /// 零件编号
        /// </summary>
        public String ODM_COMPONENT_SN { get; set; }

        /// <summary>
        /// 零件数量
        /// </summary>
        public String COMPONENT_QTY { get; set; }

        /// <summary>
        /// 是否唯一
        /// </summary>
        public String SERIALIZED { get; set; }

        /// <summary>
        /// 收集站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 收集时间
        /// </summary>
        public String COLLECT_TIME { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public String COLLECT_BY { get; set; }

        /// <summary>
        /// 返工是否清除
        /// </summary>
        public String REWORK_REMOVE_FLAG { get; set; }

        /// <summary>
        /// 是否发生替换
        /// </summary>
        public String REPLACE_FLAG { get; set; }

        /// <summary>
        /// 是否EDI数据
        /// </summary>
        public String EDI_FLAG { get; set; }
    }

    /// <summary>
    /// 替换零件
    /// </summary>
    public class CompReplaceHistoryListModel
    {
        /// <summary>
        /// 零件名称
        /// </summary>
        public String COMPONENT_NAME { get; set; }

        /// <summary>
        /// 新零件序号
        /// </summary>
        public String NEW_CUSTOMER_COMPONENT_SN { get; set; }

        /// <summary>
        /// 旧零件序号
        /// </summary>
        public String OLD_CUSTOMER_COMPONENT_SN { get; set; }

        /// <summary>
        /// 替换站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 序号是否唯一
        /// </summary>
        public String SERIALIZED { get; set; }

        /// <summary>
        /// 零件数量
        /// </summary>
        public String COMPONENT_QTY { get; set; }

        /// <summary>
        /// 零件新客户料号
        /// </summary>
        public String NEW_CUSTOMER_COMPONENT_PN { get; set; }

        /// <summary>
        /// 零件旧客户料号
        /// </summary>
        public String OLD_CUSTOMER_COMPONENT_PN { get; set; }

        /// <summary>
        /// 替换时间
        /// </summary>
        public String REPLACE_TIME { get; set; }

        /// <summary>
        /// 零件新本厂料号
        /// </summary>
        public String NEW_ODM_COMPONENT_PN { get; set; }

        /// <summary>
        /// 零件旧本厂料号
        /// </summary>
        public String OLD_ODM_COMPONENT_PN { get; set; }

        /// <summary>
        /// 替换作业员
        /// </summary>
        public String REPLACE_BY { get; set; }
    }

    /// <summary>
    /// 返工前零件
    /// </summary>
    public class ReworkCompHistoryListModel
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 零件名称
        /// </summary>
        public String COMPONENT_NAME { get; set; }

        /// <summary>
        /// 客户料号
        /// </summary>
        public String CUSTOMER_COMPONENT_SN { get; set; }

        /// <summary>
        /// 客户料号
        /// </summary>
        public String CUSTOMER_COMPONENT_PN { get; set; }

        /// <summary>
        /// 本厂料号
        /// </summary>
        public String ODM_COMPONENT_PN { get; set; }

        /// <summary>
        /// 零件数量
        /// </summary>
        public String COMPONENT_QTY { get; set; }

        /// <summary>
        /// 是否序列
        /// </summary>
        public String SERIALIZED { get; set; }

        /// <summary>
        /// 作业站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public String COLLECT_TIME { get; set; }

        /// <summary>
        /// 作业员
        /// </summary>
        public String COLLECT_BY { get; set; }

        /// <summary>
        /// 返工是否清除
        /// </summary>
        public String REWORK_REMOVE_FLAG { get; set; }

        /// <summary>
        /// 是否发生替换
        /// </summary>
        public String REPLACE_FLAG { get; set; }

        /// <summary>
        /// 数据交换标识
        /// </summary>
        public String EDI_FLAG { get; set; }
    }

    /// <summary>
    /// UID
    /// </summary>
    public class UidHistoryListModel
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public String UID_NAME { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public String UID_NUMBER { get; set; }

        /// <summary>
        /// 厂部
        /// </summary>
        public String PLANT { get; set; }

        /// <summary>
        /// 采集站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 作业时间
        /// </summary>
        public String COLLECT_TIME { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public String UID_QTY { get; set; }

        /// <summary>
        /// 作业顺序
        /// </summary>
        public String ORDER_NO { get; set; }

        /// <summary>
        /// 是否序列
        /// </summary>
        public String SERIALIZED { get; set; }

        /// <summary>
        /// 返工是否自动清除
        /// </summary>
        public String REWORK_REMOVE_FLAG { get; set; }

        /// <summary>
        /// 替换标识
        /// </summary>
        public String REPLACE_FLAG { get; set; }

        /// <summary>
        /// 作业员
        /// </summary>
        public String COLLECT_BY { get; set; }
    }

    /// <summary>
    /// 流水号替换记录
    /// </summary>
    public class ReplaceRuncardHistoryListModel
    {
        /// <summary>
        /// 旧流水号
        /// </summary>
        public String OLD_SN { get; set; }

        /// <summary>
        /// 新流水号
        /// </summary>
        public String NEW_SN { get; set; }

        /// <summary>
        /// 替换站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 替换原因
        /// </summary>
        public String REPLACE_REASON { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String REPLACE_REMARK { get; set; }

        /// <summary>
        /// 替换作业员
        /// </summary>
        public String REPLACE_BY { get; set; }

        /// <summary>
        /// 替换时间
        /// </summary>
        public String REPLACE_TIME { get; set; }
    }

    /// <summary>
    /// 资源
    /// </summary>
    public class ResourceHistoryListModel
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public String OPERATION_NAME { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public String RESOURCE_NAME { get; set; }

        /// <summary>
        /// 资源数据
        /// </summary>
        public String RESOURCE_NO { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public String RESOURCE_QTY { get; set; }

        /// <summary>
        /// 作业时间
        /// </summary>
        public String COLLECT_TIME { get; set; }

        /// <summary>
        /// 返工是否自动清除
        /// </summary>
        public String REWORK_REMOVE_FLAG { get; set; }

        /// <summary>
        /// 是否替换
        /// </summary>
        public String REPLACE_FLAG { get; set; }

        /// <summary>
        /// 采集站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 作业员
        /// </summary>
        public String COLLECT_BY { get; set; }
    }

    /// <summary>
    /// RMA
    /// </summary>
    public class RmaHistoryListModel
    {
        /// <summary>
        /// 第N次RMA
        /// </summary>
        public String COUNT { get; set; }

        /// <summary>
        /// RMA号
        /// </summary>
        public String RMA_NO { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public String RETURNED_SITE { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public String RMA_TYPE { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public String OWNER { get; set; }

        /// <summary>
        /// MIC RMA号
        /// </summary>
        public String MIC_RMA_NO { get; set; }

        /// <summary>
        /// 日期代码
        /// </summary>
        public String PCB_DATECODE { get; set; }

        /// <summary>
        /// Vendor
        /// </summary>
        public String PCB_VENDER { get; set; }

        /// <summary>
        /// 客户不良描述
        /// </summary>
        public String CUSTOMER_SYMPTOM { get; set; }

        /// <summary>
        /// OSV
        /// </summary>
        public String ON_SITE_VERIFICATION { get; set; }

        /// <summary>
        /// 本厂不良描述
        /// </summary>
        public String ODM_SYMPTOM { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public String STATUS { get; set; }

        /// <summary>
        /// Level Flag
        /// </summary>
        public String LEVEL_FLAG { get; set; }

        /// <summary>
        /// 是否保修
        /// </summary>
        public String IS_WARRANTY { get; set; }

        /// <summary>
        /// 是否损坏
        /// </summary>
        public String IS_DAMAGE { get; set; }

        /// <summary>
        /// Is CND
        /// </summary>
        public String IS_CND { get; set; }

        /// <summary>
        /// 出货时间
        /// </summary>
        public String LAST_SHIP_TIME { get; set; }

        /// <summary>
        /// 收料时间
        /// </summary>
        public String COLLECT_TIME { get; set; }

        /// <summary>
        /// 收料作业员
        /// </summary>
        public String COLLECT_BY { get; set; }

        /// <summary>
        /// 投产时间
        /// </summary>
        public String INPUT_TIME { get; set; }

        /// <summary>
        /// 包装时间
        /// </summary>
        public String PACKING_TIME { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        public String ENABLED { get; set; }
    }

    /// <summary>
    /// 工单记录
    /// </summary>
    public class WOReplaceHistoryListModel
    {
        /// <summary>
        /// 原工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 原料号
        /// </summary>
        public String PART_NO { get; set; }

        /// <summary>
        /// 新工单号
        /// </summary>
        public String NEW_WO_NO { get; set; }

        /// <summary>
        /// 新料号
        /// </summary>
        public String NEW_PART_NO { get; set; }

        /// <summary>
        /// 工单替换类型
        /// </summary>
        public String WO_REPLACE_TYPE { get; set; }

        /// <summary>
        /// 操作站点
        /// </summary>
        public String OPERATION_SITE_NAME { get; set; }

        /// <summary>
        /// 替换时间
        /// </summary>
        public String REPLACE_TIME { get; set; }

        /// <summary>
        /// 替换次数
        /// </summary>
        public String REPLACE_TIMES { get; set; }
    }
}
