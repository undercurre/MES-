using System;
using System.Collections.Generic;
using System.Linq;
using JZ.IMS.WebApi.Public;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.Core.Extensions;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using JZ.IMS.ViewModels.AssemblyOperation;
using Microsoft.Extensions.Localization;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.IServices;
using JZ.IMS.ViewModels;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 采集过站
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CollectProductsController : BaseController
    {
        private readonly ISOPRoutesService _service;
        private readonly IMesTongsInfoRepository _repositoryt;
        private readonly IMesBurnFileApplyRepository _repositoryb;
        private readonly ISfcsOperationSitesRepository _repositoryo;
        private readonly ISfcsLockProductHeaderRepository _repository;
        private readonly ISfcsRuncardRepository _sfcsRuncardRepository;
        private readonly ISfcsParametersRepository _partmetersRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISfcsCapReportRepository _repositoryc;
        private readonly ISfcsDefectReportWorkRepository _repositorydr;
        private readonly ISfcsProductConfigRepository _sfcsProductConfigRepository;
        private readonly IStringLocalizer<CollectProductsController> _localizer;

        public CollectProductsController(ISOPRoutesService service, IMesTongsInfoRepository repositoryt, IMesBurnFileApplyRepository repositoryb, ISfcsOperationSitesRepository repositoryo, ISfcsLockProductHeaderRepository repository, ISfcsRuncardRepository sfcsRuncardRepository, ISfcsParametersRepository partmetersRepository, IHttpContextAccessor httpContextAccessor, ISfcsCapReportRepository repositoryc, ISfcsDefectReportWorkRepository repositorydr, ISfcsProductConfigRepository sfcsProductConfigRepository, IStringLocalizer<CollectProductsController> localizer)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _repository = repository;
            _repositoryt = repositoryt;
            _repositoryb = repositoryb;
            _repositoryo = repositoryo;
            _partmetersRepository = partmetersRepository;
            _repositoryc = repositoryc;
            _repositorydr = repositorydr;
            _sfcsRuncardRepository = sfcsRuncardRepository;
            _sfcsProductConfigRepository = sfcsProductConfigRepository;
        }

        /// <summary>
        /// 采集过站页数据展示接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<CollectProductsModel>> GetCollectProductsData([FromBody] GetCollectProductsRequestModel model)
        {
            ApiBaseReturn<CollectProductsModel> returnVM = new ApiBaseReturn<CollectProductsModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    SfcsWo swModel = null;
                    SfcsOperationSites osModel = null;
                    CollectProductsModel cpModel = new CollectProductsModel();
                    #region 参数验证
                    if (model.WO_NO.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        swModel = (await _repositoryb.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = model.WO_NO })).FirstOrDefault();
                        if (swModel == null)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            swModel.ROUTE_ID = swModel.ROUTE_ID > 0 ? swModel.ROUTE_ID : _sfcsProductConfigRepository.GetRouteIdByPartNo(swModel.PART_NO);
                        }
                    }
                    if (model.SiteID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SITEID_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        osModel = await _repositoryo.GetAsync(model.SiteID); // 通过站点ID查找记录
                        if (osModel == null)
                        {
                            ErrorInfo.Set(_localizer["SITEID_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    if (model.History_Top <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["HISTORY_TOP_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.Defect_Top <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["DEFECT_TOP_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        #region 产品数据
                        cpModel.Wo_Id = swModel.ID;//工单id
                        cpModel.Wo_No = swModel.WO_NO;//工单号
                        cpModel.Part_No = swModel.PART_NO;//料号
                        cpModel.Route_Id = swModel.ROUTE_ID;//制程id
                        ImsPart resData = await _repositoryt.GetPartByPartNo(swModel.PART_NO);//根据料号获取产品信息
                        cpModel.Name = resData != null ? resData.NAME : "";//品名
                        cpModel.Description = resData != null ? resData.DESCRIPTION : "";//产品规格

                        String selectIOStatisticsSql = @"SELECT SO.* FROM SFCS_IO_STATISTICS SO  WHERE WO_ID = :WO_ID AND OPERATION_ID = :OPERATION_ID ";
                        List<SfcsIoStatistics> ioList = _sfcsRuncardRepository.QueryEx<SfcsIoStatistics>(selectIOStatisticsSql, new { WO_ID = swModel.ID, OPERATION_ID = osModel.OPERATION_ID });
                        //IO_TYPE : O: 完成 I :表述产品进入的数量
                        cpModel.Target_Qty = swModel.TARGET_QTY;//目标量
                        decimal? o_qty = ioList.Where(m => m.IO_TYPE == "O").ToList().Sum(m => m.QTY);//完成
                        cpModel.Completed_Qty = Convert.ToInt32(o_qty);//完成 已处理 O
                        if ((await _repositoryb.GetListByTableEX<SfcsRouteConfig>("*", "SFCS_ROUTE_CONFIG", " AND ROUTE_ID = :ROUTE_ID AND CURRENT_OPERATION_ID = :CURRENT_OPERATION_ID AND  ORDER_NO = 0 ", new { ROUTE_ID = swModel.ROUTE_ID, CURRENT_OPERATION_ID = osModel.OPERATION_ID })).FirstOrDefault() != null)
                        {
                            //第一个工序 TARGE_QTY - O 的数量表示待完成
                            cpModel.Pending_Qty = Convert.ToInt32(cpModel.Target_Qty - o_qty);
                        }
                        else
                        {
                            //非第一工序  I - O 的数量表示待完成
                            cpModel.Pending_Qty = Convert.ToInt32(ioList.Where(m => m.IO_TYPE == "I").ToList().Sum(m => m.QTY)) - cpModel.Completed_Qty;//待处理  I-O :表示待完成
                        }
                        decimal? AchievementRate = cpModel.Completed_Qty / cpModel.Target_Qty * 100;
                        cpModel.AchievementRate = Math.Round(Convert.ToDecimal(AchievementRate), 2);//达成率 保留两位小数

                        String selectSiteSql = @"SELECT SI.* FROM SFCS_SITE_STATISTICS SI WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID ";
                        List<SfcsSiteStatistics> siList = _sfcsRuncardRepository.QueryEx<SfcsSiteStatistics>(selectSiteSql, new { WO_ID = swModel.ID, OPERATION_SITE_ID = osModel.ID });
                        cpModel.Pass_Qty = siList.Sum(m => m.PASS);//良品
                        cpModel.Fail_Qty = siList.Sum(m => m.FAIL);//不良品
                        #endregion

                        #region 历史数据
                        SfcsRuncard srModel = null;
                        SfcsParameters pModel = null;
                        List<SfcsParameters> pList = _partmetersRepository.GetListByType("RUNCARD_STATUS");
                        //List<SfcsOperationHistory> HList = (await _sfcsRuncardRepository.GetListPagedEx<SfcsOperationHistory>(1, model.History_Top, " WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID", "OPERATION_TIME DESC", new { WO_ID = swModel.ID, OPERATION_SITE_ID = model.SiteID })).ToList();
                        List<SfcsOperationHistory> hList = _sfcsRuncardRepository.QueryEx<SfcsOperationHistory>("SELECT * FROM SFCS_OPERATION_HISTORY WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID ORDER BY OPERATION_TIME DESC", new { WO_ID = swModel.ID, OPERATION_SITE_ID = model.SiteID });
                        for (int i = 0; i < hList.Count; i++)
                        {
                            if (i < model.History_Top)
                            {
                                HistoricalDataModel hdModel = new HistoricalDataModel();
                                pModel = pList.Where(m => m.LOOKUP_CODE == hList[i].OPERATION_STATUS).FirstOrDefault();
                                srModel = (await _repository.GetListByTableEX<SfcsRuncard>(" SR.SN ", " SFCS_RUNCARD SR ", " AND SR.ID=:ID ", new { ID = hList[i].SN_ID })).FirstOrDefault();
                                hdModel.SN_No = srModel != null ? srModel.SN : "";
                                hdModel.Operation_Status = pModel != null ? pModel.DESCRIPTION.ToUpper() : "";
                                hdModel.Operation_Time = hList[i].OPERATION_TIME;
                                hdModel.Operator = hList[i].OPERATOR;
                                cpModel.HistoricalData.Add(hdModel);
                            }
                        }
                        #endregion

                        #region 不良数据
                        String sql = @"SELECT B.DEFECT_DESCRIPTION,COUNT(1) AS LEVEL_CODE FROM SFCS_COLLECT_DEFECTS A,SFCS_DEFECT_CONFIG B WHERE A.DEFECT_CODE = B.DEFECT_CODE AND B.ENABLED = 'Y' AND A.WO_ID =:WO_ID AND DEFECT_OPERATION_ID =:DEFECT_OPERATION_ID GROUP BY B.DEFECT_DESCRIPTION ";
                        List<SfcsDefectConfig> scList = _sfcsRuncardRepository.QueryEx<SfcsDefectConfig>(sql, new { WO_ID = swModel.ID, DEFECT_OPERATION_ID = osModel.OPERATION_ID }).OrderByDescending(m => m.LEVEL_CODE).ToList();
                        decimal FailSum = scList.Sum(t => t.LEVEL_CODE);
                        for (int i = 0; i < scList.Count; i++)
                        {
                            if (i < model.Defect_Top)
                            {
                                FailDataModel fdModel = new FailDataModel();
                                fdModel.Fail_Name = scList[i].DEFECT_DESCRIPTION;
                                decimal Percentage = scList[i].LEVEL_CODE / FailSum * 100;
                                fdModel.Percentage = Math.Round(Percentage, 2);
                                fdModel.Fail_Qty = scList[i].LEVEL_CODE;
                                cpModel.FailData.Add(fdModel);
                            }
                        }
                        #endregion

                        returnVM.Result = cpModel;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 提交无码报工 不良代码数量默认为1
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> PostToUncodedReport([FromBody] ReportDataRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    SfcsWo swModel = null;
                    Decimal order_no = -1;
                    SfcsOperationSites osModel = null;
                    //是否最后一个工序
                    bool isLastOperation = false;

                    if (model.WO_NO.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        swModel = (await _repositoryb.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = model.WO_NO })).FirstOrDefault();
                        if (swModel == null)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //当前工单没有配置制程，请注意检查!
                            if (swModel.ROUTE_ID <= 0)
                            {
                                ErrorInfo.Set(_localizer["WO_NO_NOT_ROUTE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }
                    if (model.SiteID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SITEID_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        osModel = await _repositoryo.GetAsync(model.SiteID); // 通过站点ID查找记录
                        String no = _repository.QueryEx<String>("SELECT SRC.ORDER_NO FROM SFCS_OPERATION_SITES SOS,SFCS_ROUTE_CONFIG SRC WHERE  SRC.CURRENT_OPERATION_ID =SOS.OPERATION_ID AND SOS.ID = :ID AND SOS.ENABLED = 'Y' AND SRC.ROUTE_ID =:ROUTE_ID", new { ID = model.SiteID, ROUTE_ID = swModel.ROUTE_ID })?.FirstOrDefault();
                        if (no.IsNullOrEmpty() || osModel == null)
                        {
                            ErrorInfo.Set(String.Format(_localizer["SITE_ORDER_NO_ERROR"], swModel.WO_NO), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            order_no = Convert.ToDecimal(no);
                        }
                    }
                    if (model.CapacityReportQty <= 0 && model.DefectReportQty <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["QTY_TOP_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.DefectReportQty > 0 && model.DEFECT_CODE.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["DEFECT_CODE_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else if (model.DefectReportQty > 0 && !model.DEFECT_CODE.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        //校验不良代码
                        if ((await _repositoryb.GetListByTableEX<SfcsReasonConfig>("*", "SFCS_DEFECT_CONFIG", " AND DEFECT_CODE = :DEFECT_CODE AND ENABLED = 'Y'", new { DEFECT_CODE = model.DEFECT_CODE })).FirstOrDefault() == null)
                        {
                            ErrorInfo.Set(_localizer["DEFECT_CODE_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    if (model.UserName.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["USERNAME_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #region 最后一个工序报工数量 不能大于前面工序报工数

                    if (model.CapacityReportQty > 0 && !ErrorInfo.Status)
                    {
                        List<SfcsRoutesSiteListModel> rList = await _repositoryc.GetRouteCapacityDataByWoId(swModel.ID, swModel.ROUTE_ID);
                        if (rList.IsNullOrWhiteSpace() || rList.Count <= 0)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_ROUTE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            decimal maxNo = rList.Max(m => m.ORDER_NO);
                            if (order_no == maxNo)
                            {
                                //标记最后一个工序
                                isLastOperation = true;
                            }

                            if (order_no != 0 && order_no > 0)
                            {
                                //不是第一道工序
                                decimal no = order_no - 10;
                                SfcsRoutesSiteListModel rsModel = rList.Where(m => m.ORDER_NO == no)?.FirstOrDefault();
                                SfcsRoutesSiteListModel rs2Model = rList.Where(m => m.ORDER_NO == order_no)?.FirstOrDefault();
                                if (rsModel == null || rs2Model == null)
                                {
                                    ErrorInfo.Set(_localizer["SITEID_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                else if ((rs2Model.PASS + model.CapacityReportQty + model.DefectReportQty) > rsModel.PASS)
                                {
                                    ErrorInfo.Set(_localizer["CAPACITY_NUM_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        DateTime reportTime;//报工时间
                        if (model.REPORT_TIME.IsNullOrEmpty())
                        {
                            reportTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH") + ":00:00");
                        }
                        else
                        {
                            reportTime = Convert.ToDateTime(model.REPORT_TIME);
                            reportTime = Convert.ToDateTime(reportTime.ToString("yyyy-MM-dd HH") + ":00:00");
                        }
                        //提交产能报工
                        if (model.CapacityReportQty > 0)
                        {
                           var data= await _repositoryc.PostToCapacityReport(swModel.ID, model.SiteID, model.UserName, model.CapacityReportQty, reportTime);

                            //同步OUT_PUT字段 数据
                            if (data != -1 && isLastOperation)
                            {
                                var updateOutputResult = await _repositoryc.UpdateOutputData(swModel, model.CapacityReportQty, osModel.OPERATION_LINE_ID);
                                if (!updateOutputResult)
                                {
                                    ErrorInfo.Set(_localizer["SYSTEM_FAILED_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
                        }
                        //提交不良报工
                        if (model.DefectReportQty > 0)
                        {
                            await _repositorydr.PostToDefectReport(swModel.ID, model.SiteID, model.UserName, model.DefectReportQty, reportTime, model.DEFECT_CODE, model.DEFECT_LOC);
                        }
                        returnVM.Result = true;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 撤销无码报工
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ClearUncodedReport([FromBody] ReportDataRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    SfcsWo swModel = null;
                    Decimal order_no = -1;
                    SfcsOperationSites osModel = null;
                    SfcsInboundRecordInfo inboundRecordModel = null;

                    if (model.WO_NO.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        swModel = (await _repositoryb.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = model.WO_NO })).FirstOrDefault();
                        if (swModel == null)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    if (model.SiteID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SITEID_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        osModel = await _repositoryo.GetAsync(model.SiteID); // 通过站点ID查找记录
                        String no = _repository.QueryEx<String>("SELECT SRC.ORDER_NO FROM SFCS_OPERATION_SITES SOS,SFCS_ROUTE_CONFIG SRC WHERE  SRC.CURRENT_OPERATION_ID =SOS.OPERATION_ID AND SOS.ID = :ID AND SOS.ENABLED = 'Y' AND SRC.ROUTE_ID =:ROUTE_ID", new { ID = model.SiteID, ROUTE_ID = swModel.ROUTE_ID })?.FirstOrDefault();
                        if (no.IsNullOrEmpty() || osModel == null)
                        {
                            ErrorInfo.Set(String.Format(_localizer["SITE_ORDER_NO_ERROR"], swModel.WO_NO), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            order_no = Convert.ToDecimal(no);
                        }
                    }
                    if (model.UserName.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["USERNAME_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //标识最后一站
                       bool isLastOperation = false;
                        //撤销产能报工
                        if (model.CapacityReportQty > 0 && !ErrorInfo.Status)
                        {
                            string sql = @"SELECT * FROM (SELECT * FROM SFCS_CAP_REPORT WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID ORDER BY CREATE_TIME DESC,ID DESC) WHERE ROWNUM = 1 ";
                            SfcsCapReport crModel = _repositoryc.QueryEx<SfcsCapReport>(sql, new { WO_ID = swModel.ID, OPERATION_SITE_ID = model.SiteID }).ToList().FirstOrDefault();
                            if (crModel == null)
                            {
                                ErrorInfo.Set(_localizer["CAPACITY_REPORT_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                #region 判断最后一个工序

                                if (model.CapacityReportQty > 0)
                                {
                                    List<SfcsRoutesSiteListModel> rList = await _repositoryc.GetRouteCapacityDataByWoId(swModel.ID, swModel.ROUTE_ID);
                                    if (rList.IsNullOrWhiteSpace() || rList.Count <= 0)
                                    {
                                        ErrorInfo.Set(_localizer["WO_NO_NOT_ROUTE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    else
                                    {
                                        decimal maxNo = rList.Max(m => m.ORDER_NO);
                                        if (order_no == maxNo)
                                        {
                                            //标记最后一个工序
                                            isLastOperation = true;
                                        }

                                        #region 完工入库
                                        //完工入库检测
                                        if (isLastOperation)
                                        {

                                            inboundRecordModel = (await _repository.GetListByTableEX<SfcsInboundRecordInfo>("ROW_NUMBER() OVER(ORDER BY ID DESC) ROWNO,T.*", "SFCS_INBOUND_RECORD_INFO T", " AND T.WO_ID=:WO_ID ", new { WO_ID = swModel.ID })).FirstOrDefault();
                                            if (inboundRecordModel != null && !GlobalVariables.InBoundUntreatedStatus.Equals(inboundRecordModel.STATUS))
                                            {
                                                //当前处于完工入库处理中(处理完)状态，无法撤销报工.
                                                throw new Exception(_localizer["DEFFECT_REPORT_NOT_PROCESS"]);
                                            }

                                        }
                                        #endregion

                                        await _repositoryc.ClearCapacityReport(crModel.ID, model.UserName, crModel.QTY, swModel, model.SiteID, crModel.REPORT_TIME, osModel.OPERATION_LINE_ID, isLastOperation, inboundRecordModel?.ID??0);
                                    }
                                }

                                #endregion
                            }
                        }
                        //撤销不良报工
                        if (model.DefectReportQty > 0)
                        {
                            string sql = @"SELECT * FROM (SELECT * FROM SFCS_DEFECT_REPORT_WORK WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID ORDER BY CREATE_TIME DESC) WHERE ROWNUM = 1 ";
                            SfcsDefectReportWork drModel = _repositorydr.QueryEx<SfcsDefectReportWork>(sql, new { WO_ID = swModel.ID, OPERATION_SITE_ID = model.SiteID }).ToList().FirstOrDefault();
                            if (drModel == null)
                            {
                                ErrorInfo.Set(_localizer["DEFECT_REPORT_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                await _repositorydr.ClearDefectReport(drModel.ID, model.UserName, drModel.QTY, swModel.ID, model.SiteID, drModel.REPORT_TIME);
                            }
                        }
                        returnVM.Result = true;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 站点的直通率 获取工单的直通率不用传Type或者为空 
        /// 获取今天直通率:Type="Today" 
        /// </summary>
        /// <param name="currentWoId">工单ID</param>
        /// <param name="currentOperationSiteID">站点ID</param>
        /// <returns>PASS产能</returns>
        ///  <returns>TOTAL总件数</returns>
        ///  <returns>RATE比率</returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetRefershPassRate([FromQuery] decimal currentWoId = 0, decimal currentOperationSiteID = 0, string Type = "")
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            returnVM.Result = new object();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    if (currentWoId <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["WO_ID_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        var osModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And ID=:ID ", new { ID = currentWoId }));
                        if (osModel == null)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }


                    if (currentOperationSiteID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SITEID_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        var osModel = await _repositoryo.GetAsync(currentOperationSiteID); // 通过站点ID查找记录
                        if (osModel == null)
                        {
                            ErrorInfo.Set(_localizer["SITEID_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        if (currentWoId > 0 && currentOperationSiteID > 0)
                        {
                            returnVM.Result = await _repositoryc.GetSitePassRate(currentOperationSiteID, currentWoId, Type);
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取站点的第几小时产能
        /// </summary>
        /// <param name="currentWoId">工单ID</param>
        /// <param name="currentOperationSiteID">站点ID</param>
        /// <returns>PASS产能</returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetRefershHourYieldByStationID([FromQuery] decimal currentWoId = 0, decimal currentOperationSiteID = 0)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            returnVM.Result = new object();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    if (currentWoId <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["WO_ID_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        var osModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And ID=:ID ", new { ID = currentWoId }));
                        if (osModel == null)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }


                    if (currentOperationSiteID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SITEID_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        var osModel = await _repositoryo.GetAsync(currentOperationSiteID); // 通过站点ID查找记录
                        if (osModel == null)
                        {
                            ErrorInfo.Set(_localizer["SITEID_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        if (currentWoId > 0 && currentOperationSiteID > 0)
                        {
                            returnVM.Result = await _repositoryc.GetSiteHourYield(currentOperationSiteID, currentWoId);
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 刷新TOP X 不良现象
        /// 获取今天不良现象:Type="Today" 
        /// </summary>
        /// <param name="currentWoId">工单ID</param>
        /// <param name="currentOperationSiteID">站点ID</param>
        /// <param name="Type"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetfershTopDefectByStation([FromQuery] decimal currentWoId = 0, decimal currentOperationSiteID = 0, string Type = "")
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            returnVM.Result = new object();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    if (currentWoId <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["WO_ID_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        var osModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And ID=:ID ", new { ID = currentWoId }));
                        if (osModel == null)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }


                    if (currentOperationSiteID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SITEID_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        var osModel = await _repositoryo.GetAsync(currentOperationSiteID); // 通过站点ID查找记录
                        if (osModel == null)
                        {
                            ErrorInfo.Set(_localizer["SITEID_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        if (currentWoId > 0 && currentOperationSiteID > 0)
                        {
                            returnVM.Result = await _repositoryc.GetSiteTopDefect(currentOperationSiteID, currentWoId, 5, Type);
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取工单信息 通过线体 可以切换工单
        /// 当前工单的下标  0：当前生产工单，1：上一个工单，最多前5个工单 
        /// </summary>
        /// <param name="currentOperationLineID">线体ID</param>
        /// <param name="currentWoNoIndex">下标</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetCurrentWorkOrder([FromQuery] decimal currentOperationLineID = 0, int currentWoNoIndex = 0)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            QCModel model = new QCModel();
            returnVM.Result = model;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        if (currentOperationLineID > 0)
                        {
                            var productionModle = await _repositoryc.GetProductionByIndex(currentOperationLineID, currentWoNoIndex);
                            if (productionModle != null)
                            {
                                var woModel = (await _repositoryc.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND WO_NO=:WO_NO ", new { WO_NO = productionModle.WO_NO }))?.FirstOrDefault();
                                if (woModel == null) { throw new Exception(_localizer["WO_NO_INFO_NULL"]); }
                                model.WoId = woModel.ID.ToString();
                                model.WoNo = productionModle.WO_NO;
                                model.PartNo = woModel.PART_NO;
                                model.WoTarget = woModel.TARGET_QTY ?? 0;

                                //string singelCounter = IMSConfigs.Default["singelCounter"];
                                //if (!singelCounter.IsNullOrEmpty() && Boolean.Parse(singelCounter))
                                //{
                                //    this.multNo = 1;
                                //}
                                //else
                                //{
                                //    this.multNo = productionModle.MULTI_NO;// this.currentWorkOrderDataSet.SFCS_PRODUCTION.FirstOrDefault().MULTI_NO;
                                //}
                                //机种
                                var modelNameModel = (await _repositoryc.GetListByTableEX<SfcsModel>("*", "SFCS_MODEL", " AND ID=:ID ", new { ID = woModel.MODEL_ID }))?.FirstOrDefault();
                                model.ModelName = modelNameModel == null ? "" : modelNameModel.MODEL;
                                var stationModel = (await _repositoryc.GetListByTableEX<SfcsOperationSites>("*", "SFCS_OPERATION_SITES", " AND ID=:ID ", new { ID = productionModle.STATION_ID }))?.FirstOrDefault();
                                var operationId = stationModel == null ? 0 : (stationModel.OPERATION_ID ?? 0);
                                model.CapacityReportQty = await _repositoryc.GetStandardCapacity(model.PartNo, operationId);
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #region 内部方法

        public class QCModel
        {
            /// <summary>
            /// 工单id
            /// </summary>
            public string WoId { get; set; }

            /// <summary>
            /// 工单号
            /// </summary>
            public string WoNo { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public decimal WoTarget { get; set; }

            /// <summary>
            /// 料号
            /// </summary>
            public string PartNo { get; set; }

            /// <summary>
            /// 机种
            /// </summary>
            public string ModelName { get; set; }

            /// <summary>
            /// 报工数量
            /// </summary>
            public decimal? CapacityReportQty { get; set; }

        }
        #endregion

    }
}
