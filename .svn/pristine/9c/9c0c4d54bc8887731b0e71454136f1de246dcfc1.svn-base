using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface IKanbanRepository : IDisposable
	{
		#region PCBA/SMT共用
		/// <summary>
		/// 获取每小时产能数据
		/// </summary>
		/// <param name="lineId">产线ID</param>
		/// <param name="topCount">查询数据量</param>
		/// <param name="lineType">产线类型 PCBA/SMT </param>
		/// <returns></returns>
		Task<IEnumerable<HourYidldModel>> GetHourYidldDataAsync(int lineId, int topCount, string lineType);

		/// <summary>
		/// 获取抽检良率
		/// </summary>
		/// <param name="lineId">产线id</param>
		/// <param name="wo_no">工单号</param>
		/// <returns></returns>
		Task<KanBanSpotCheckModel> GetKanbanSpotCheckDataAsync(int lineId, string wo_no, int topCount);

		Task<IEnumerable<dynamic>> GetKanbanPassRateDataAsync(int lineId, string lineType);
		#endregion

		#region 产线看板

		/// <summary>
		/// 检测线体是否存在
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<bool> CheckLineAsync(int lineId);

		/// <summary>
		/// 获取产线的工单信息
		/// </summary>
		/// <param name="lineId">产线id</param>
		/// <returns></returns>
		Task<IEnumerable<KanbanWoModel>> GetKanbanWoDataAsync(int lineId);

		/// <summary>
		/// 获取产线的站点统计信息
		/// </summary>
		/// <param name="lineId">产线id</param>
		/// <returns></returns>
		Task<IEnumerable<KanbanSiteModel>> GetKanbanSiteDataAsync(int lineId, string where="");

		/// <summary>
		/// 获取产线FCT工序的合格率
		/// </summary>
		/// <param name="lineId">产线id</param>
		/// <returns></returns>
		Task<IEnumerable<KanbanPassRateModel>> GetKanbanPassRateDataAsync(int lineId);

		/// <summary>
		/// 获取产线的呼叫信息
		/// </summary>
		/// <param name="lineId">产线id</param>
		/// <param name="day">最近今天的呼叫数据</param>
		/// <param name="topCount">查询前X条</param>
		/// <returns></returns>
		Task<IEnumerable<AndonCallModel>> GetCallDataAsync(int lineId, int day = 3, int topCount = 5);

		/// <summary>
		/// 获取产线TOP不良代码信息
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="topCount"></param>
		/// <returns></returns>
		Task<IEnumerable<TopDefectModel>> GetTopDefectDataAsync(int lineId, int topCount = 5);

		/// <summary>
		/// 产线看板的排产的完成率
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<List<KanbanWorkingPassRateModel>> GetKanbanWorkingPassRateDataAsync(int lineId);

		/// <summary>
		/// 获取产线最近X小时的每小时产能
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="topCount"></param>
		/// <returns></returns>
		Task<IEnumerable<HourYidldModel>> GetKanbanHourYidldDataAsync(int lineId, int topCount = 7);

		/// <summary>
		/// 获取产线最近X小时的每小时产能（优化）
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="topCount"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		//Task<IEnumerable<HourYidldModel>> GetKanbanHourYidldDataNewAsync(int lineId, int topCount, string date);

		/// <summary>
		/// 产线看板的标准产能
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<IEnumerable<SysWorkShiftDetailModel>> GetWorkShiftDetailDataAsync(int lineId);

		/// <summary>
		/// 产线看板不良品信息
		/// </summary>
		/// <param name="wo_no"></param>
		/// <returns></returns>
		Task<IEnumerable<KanbanDefectMessageModel>> GetKanbanDefectMsgDataAsync(string wo_no);
		#endregion

		#region 自动化线看板
		/// <summary>
		/// 检查自动化线体是否存在
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<bool> CheckSmtLineAsync(int lineId);

		/// <summary>
		/// 获取产线的工单信息
		/// </summary>
		/// <param name="lineId">产线id</param>
		/// <returns></returns>
		Task<IEnumerable<SmtKanbanWoModel>> GetSmtKanbanWoDataAsync(int lineId);

		/// <summary>
		/// 自动化线看板的AOI的直通率
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<IEnumerable<SmtKanbanAoiPassRateModel>> GetSmtKanbanAoiPassRateDataAsync(int lineId);

		/// <summary>
		/// 自动化线看板的首件的直通率
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<IEnumerable<SmtKanbanFirstPassRateModel>> GetSmtKanbanFirstPassRateDataAsync(int lineId);

		/// <summary>
		/// 自动化线看板-低水位预警
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<IEnumerable<SmtKanbanRestPcbModel>> GetSmtKanbanRestPcbDataAsync(int lineId, int topCount);

		/// <summary>
		/// 自动化线看板的SPI的直通率
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<IEnumerable<SmtKanbanSpiPassRateModel>> GetSmtKanbanSpiPassRateDataAsync(int lineId);

		/// <summary>
		/// 自动化线看板的排产的完成率
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<IEnumerable<SmtKanbanWorkingPassRateModel>> GetSmtKanbanWorkingPassRateDataAsync(int lineId);

		/// <summary>
		/// 获取自动化线最近X小时的每小时产能
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="topCount"></param>
		/// <returns></returns>
		Task<IEnumerable<SmtKanbanHourYidldModel>> GetSmtKanbanHourYidldDataAsync(int lineId, int topCount);

		/// <summary>
		/// 获取自动化线最近X小时的每小时产能（优化）
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="topCount"></param>
		/// <param name="date"></param>
		/// <returns></returns>
		//Task<IEnumerable<SmtKanbanHourYidldModel>> GetSmtKanbanHourYidldDataNewAsync(int lineId, int topCount, string date);

		/// <summary>
		/// 自动化线看板的标准产能
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<IEnumerable<SysWorkShiftDetailModel>> GetSmtWorkShiftDetailDataAsync(int lineId);
		#endregion

		#region 设备点检看板
		/// <summary>
		/// 获取设备列表
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<SfcsEquipmentListModel>> GetEquipmentList(decimal LineId);

		/// <summary>
		/// 获取设备维修记录
		/// </summary>
		/// <param name="topCount"></param>
		/// <param name="equipId"></param>
		/// <returns></returns>
		Task<IEnumerable<SfcsEquipRepairHead>> GetEquipRepairHeadList(int topCount, decimal equipId);

		/// <summary>
		/// 获取设备点检记录
		/// </summary>
		/// <param name="topCount"></param>
		/// <param name="equipId"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		Task<IEnumerable<SfcsEquipKeepHead>> GetEquipKeepHeadList(int topCount, decimal equipId, int type);
		#endregion

		/// <summary>
		/// 看板的异常报告
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="lineType"></param>
		/// <returns></returns>
		Task<IEnumerable<MesMonitoringReportListModel>> GetMonitoringReportDataAsync(int lineId, string lineType, int topCount);

		/// <summary>
		/// 获取当前工单的时间完成数
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<WOPassVM> GetWoPassTotal(int lineId);

		/// <summary>
		/// 获取每小时产能
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<List<dynamic>> GetWoHourPass(int lineId);

		/// <summary>
		/// 获取今日工单完成
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<List<dynamic>> GetWoToDayPass(int lineId);

		/// <summary>
		/// 站点统计
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<List<dynamic>> GetSiteStatistics(int lineId);

		/// <summary>
		/// 获取今日的工单汇总
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<List<dynamic>> GetWoToDayALL(int lineId);

		/// <summary>
		/// 直通率
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<decimal> GetWoToRate(int lineId);

		/// <summary>
		/// 最近5天的产能
		/// </summary>
		/// <param name="lineId"></param>
		/// <returns></returns>
		Task<List<dynamic>> Top5Prouduct(int lineId);
		#region AOI/SPI集成看板
		/// <summary>
		/// 获取看板数据
		/// </summary>
		/// <returns></returns>
		Task<List<AoiAndSpiReportListModel>> GetAoiSpiDataAsync(string organizeId, string floor);

		#endregion

		#region  线体看板

		/// <summary>
		/// 根据用户ID，获取用户的所有组织
		/// </summary>
		/// <param name="user_id"></param>
		/// <returns></returns>
		Task<IEnumerable<BoardEntryListModel>> GetOrganizeList(decimal user_id);

		/// <summary>
		/// 根据组织ID，获取到隶属该组织的所有线别
		/// </summary>
		/// <param name="organizeId"></param>
		/// <returns></returns>
		Task<IEnumerable<BoardEntryListModel>> GetLines(string organizeId);

		/// <summary>
		/// 根据组织ID，获取到AOI/SPI的楼层信息
		/// </summary>
		/// <param name="organizeId"></param>
		/// <returns></returns>
		Task<IEnumerable<AoiAndSpiReportListModel>> GetAoiSpiFloorData(string organizeId);

		#endregion

		#region AI&RI集成看板

		Task<List<AiAndRiReportListModel>> GetAiRiDataAsync(string organizeId, string floor, List<decimal> idList = null);

		#endregion

		/// <summary>
		/// Aoi品质柏拉图
		/// </summary>
		/// <param name="WO_NO"></param>
		/// <returns></returns>
		Task<TableDataModel> GetAoiReportData(AoiAndSpiReportListModel model);

		/// <summary>
		/// spi品质柏拉图
		/// </summary>
		/// <param name="WO_NO"></param>
		/// <returns></returns>
		Task<TableDataModel> GetSpiReportData(AoiAndSpiReportListModel model);

		/// <summary>
		/// avi品质柏拉图
		/// </summary>
		/// <param name="WO_NO"></param>
		/// <returns></returns>
		Task<TableDataModel> GetAviReportData(AoiAndSpiReportListModel model);

		/// <summary>
		/// Ict报表柏拉图
		/// </summary>
		/// <param name="WO_NO"></param>
		/// <returns></returns>
		Task<TableDataModel> GetIctReport(string WO_NO);

		/// <summary>
		/// fct报表柏拉图
		/// </summary>
		/// <param name="WO_NO"></param>
		/// <returns></returns>
		Task<TableDataModel> GetFctReport(string WO_NO);

		/// <summary>
		/// 红胶,锡膏作业报表
		/// </summary>
		/// <param name="Resource_No"></param>
		/// <param name="startTime"></param>
		/// <returns></returns>
		Task<TableDataModel> GetResourceRuncardReport(PageModel page,string Resource_No, string code, DateTime? startTime = null);

		/// <summary>
		/// QC报表
		/// </summary>
		/// <returns></returns>
		Task<dynamic> GetQCReport(int lineId, string WO_NO, DateTime? date);

		#region 流程卡报表
		/// <summary>
		/// 根据SN获取产品流水信息
		/// </summary>
		/// <param name="sn">产品流水号</param>
		/// <returns></returns>
		Task<RuncardReportListModel> GetRuncardInfoBySn(string sn, string type);
		#endregion

		/// <summary>
		/// 根据条件获取相应的查询列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetSiteStatisticsConditionList(StatisticsConditionRequestModel model);

		/// <summary>
		/// 获取站点统计数据表格
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<SiteStatisticsTableListModel>> GetSiteStatisticsDataTable(StatisticalReportRequestModel model);

		/// <summary>
		/// 获取站点统计明细数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<SiteStatisticsDetailListModel>> GetSiteStatisticsDetail(StatisticalReportRequestModel model);

		/// <summary>
		/// 获取站点统计每小时明细数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<SiteStatisticsDetailListModel>> GetSiteStatisticsHourDetail(StatisticalReportRequestModel model);

		/// <summary>
		/// 站点统计报表获取detail report
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<SiteStatisticsReportDetailListModel>> GetSiteStatisticsReportDetail(ReportDetailRequestModel model);

		/// <summary>
		/// 获取在制品报表数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<WipStatisticsListModel> GetWipStatisticsList(StatisticalReportRequestModel model);

		/// <summary>
		/// 在制品报表获取detail report
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<WipStatisticsReportDetailListModel>> GetWipStatisticsReportDetail(ReportDetailRequestModel model);

		/// <summary>
		/// 获取不良维修报表数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<DefectReportListModel>> GetDefectReportList(DefectReportRequestModel model);

		/// <summary>
		/// 小时产能报表
		/// </summary>
		/// <returns></returns>
		Task<dynamic> GetKanbanHourReport(int lineId, string WO_NO, DateTime? startDate, DateTime? endDate, PageModel model);

		/// <summary>
		/// 生产报表
		/// </summary>
		/// <returns></returns>
		Task<dynamic> GetProductionReport(int lineId, string WO_NO, int type, string lineType, DateTime? startDate, DateTime? endDate, PageModel model);
		
		#region SMT产线看板

		/// <summary>
		/// 排产计划完成
		/// </summary>
		/// <returns></returns>
		Task<List<ProducePlanInfoListModel>> TopDayProducePlan(ProducePlanInfoRequestModel mdoel); 

		Task<HourYieldListModel> GetRefershHourYield(HourYieldRequestModel mdoel);

		Task<List<SmtAOIDefectInfoListModel>> SmtAOIDefectInfo(HourYieldRequestModel mdoel);

        #endregion

        #region SMT车间看板

        /// <summary>
        /// SMT周计划
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        Task<List<ProducePlanInfoWeekListModel>> GetProducePlanInfoWeek(int user_id);

        /// <summary>
        /// 全部线体的自动化线看板的AOI的直通率
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        Task<List<SmtKanbanAoiPassRateModel>> GetSmtKanbanAoiPassRateByAllLines(int user_id);

        /// <summary>
        /// SMT今日计划达成率
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        Task<List<PlanAchievementRateListModel>> GetPlanAchievementRate(int user_id);

        /// <summary>
        /// SMT线体任务跟踪
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        Task<List<TaskTrackBySMTLineListModel>> GetTaskTrackBySMTLine(int user_id);

		/// <summary>
		/// SMT线体效率对比
		/// </summary>
		/// <param name="user_id"></param>
		/// <returns></returns>
		Task<List<EfficiencyBySMTLineListModel>> GetEfficiencyBySMTLine(int user_id);

		#endregion

	}
}
