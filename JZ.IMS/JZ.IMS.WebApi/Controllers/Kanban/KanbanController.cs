using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.WebApi.Common;
using JZ.IMS.Core.Extensions;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 看板管理 控制器  
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class KanbanController : BaseController
    {
        private readonly IKanbanService _service;
        private readonly IKanbanRepository _repository;
        private readonly IMesKanbanControllerRepository _repositoryIMesKanbanController;
        private readonly IMesChangeLineRecordRepository _repositoryChangeLineRecord;
        private readonly IMesChangeLineRecordResultRepository _repositoryChangeLineRecordResult;
        private readonly ISfcsEquipmentLinesRepository _linesRepository;
        private readonly ISysProjApiMstRepository _repositoryApi;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<KanbanController> _localizer;

        public KanbanController(IKanbanService service, IKanbanRepository repository, IMesKanbanControllerRepository repositoryIMesKanbanController,
            IMapper mapper, IMesChangeLineRecordRepository repositoryChangeLineRecord, IMesChangeLineRecordResultRepository repositoryChangeLineRecordResult
            , ISysProjApiMstRepository repositoryApi, ISfcsEquipmentLinesRepository linesRepository, IStringLocalizer<KanbanController> localizer)
        {
            _service = service;
            _repository = repository;
            _repositoryIMesKanbanController = repositoryIMesKanbanController;
            _mapper = mapper;
            _linesRepository = linesRepository;

            _repositoryChangeLineRecord = repositoryChangeLineRecord;
            _repositoryChangeLineRecordResult = repositoryChangeLineRecordResult;
            _repositoryChangeLineRecordResult = repositoryChangeLineRecordResult;
            _repositoryApi = repositoryApi;
            _localizer = localizer;
        }

        #region 获取配置信息
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> Index(string ORGANIZE_ID)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var LineList = _linesRepository.GetLinesList(ORGANIZE_ID, "PCBA");
                    returnVM.Result = JsonHelper.ObjectToJSON(new { LineList });
                    returnVM.TotalCount = 0;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region 产线看板

        /// <summary>
        /// 产线看板页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> ProductLine()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = true;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }
            return returnVM;
        }

        /// <summary>
        /// 检测线体是否存在
        /// </summary>
        /// <param name="lineId">线体id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> CheckLine(int lineId)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = false;
                    var resdata = await _service.CheckLineAsync(lineId);
                    if (resdata.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                    {
                        returnVM.Result = true;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取产线的工单信息
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetKanbanWoData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetKanbanWoDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取抽检良率
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="wo_no">工单号</param>
        /// <param name="topCount">前几条</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetKanbanSpotCheckData(int lineId, string wo_no, int topCount)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetKanbanSpotCheckDataAsync(lineId, wo_no, topCount);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取产线的站点统计信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetKanbanSiteData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetKanbanSiteDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取产线FCT工序的合格率
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetKanbanPassRateData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetKanbanPassRateDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取产线的呼叫信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="day">最近今天的呼叫数据</param>
        /// <param name="topCount">查询前X条</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetCallData(int lineId, int day, int topCount)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetCallDataAsync(lineId, day, topCount);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取产线TOP不良代码信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="topCount">查询前X条</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetTopDefectData(int lineId, int topCount)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetTopDefectDataAsync(lineId, topCount);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取产线最近X小时的每小时产能
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="topCount">查询前X条</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetKanbanHourYidldData(int lineId, int topCount)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetKanbanHourYidldDataAsync(lineId, topCount);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 产线看板的排产的完成率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetKanbanWorkingPassRateData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetKanbanWorkingPassRateDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }


        /// <summary>
        /// 产线看板的标准产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetWorkShiftDetailData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetWorkShiftDetailDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 产线看板的异常报告
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetMonitoringReportData(int lineId, int topCount = 8)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetMonitoringReportDataAsync(lineId, "PCBA", topCount);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 看板的换线记录
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topDay"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetChangeLineRecordData(int lineId, int topDay = 7)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetChangeLineRecordDataAsync(lineId, "PCBA", topDay);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 产线看板不良品信息
        /// </summary>
        /// <param name="wo_no"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetKanbanDefectMsgData(string wo_no)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetKanbanDefectMsgDataAsync(wo_no);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        #endregion

        #region 自动化线看板

        /// <summary>
        /// SmtLine(自动化线)看板页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> SmtLine()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = true;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }
            return returnVM;
        }

        /// <summary>
        /// 检查自动化线体是否存在
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        public async Task<ApiBaseReturn<bool>> CheckSmtLine(int lineId)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = false;
                    var resdata = await _service.CheckSmtLineAsync(lineId);
                    if (resdata.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                    {
                        returnVM.Result = true;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取产线的工单信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtKanbanWoData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetSmtKanbanWoDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 自动化线看板的AOI的直通率
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtKanbanAoiPassRateData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetSmtKanbanAoiPassRateDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 自动化线看板的首件的直通率
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtKanbanFirstPassRateData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetSmtKanbanFirstPassRateDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 自动化线看板-低水位预警
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtKanbanRestPcbData(int lineId, int topCount = 8)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetSmtKanbanRestPcbDataAsync(lineId, topCount);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 自动化线看板的SPI的直通率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtKanbanSpiPassRateData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetSmtKanbanSpiPassRateDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 自动化线看板的排产的完成率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtKanbanWorkingPassRateData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetSmtKanbanWorkingPassRateDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取自动化线最近X小时的每小时产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtKanbanHourYidldData(int lineId, int topCount = 8)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetSmtKanbanHourYidldDataAsync(lineId, topCount);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 自动化线看板的标准产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtWorkShiftDetailData(int lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetSmtWorkShiftDetailDataAsync(lineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 自动化线看板的异常报告
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtMonitoringReportData(int lineId, int topCount = 8)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetMonitoringReportDataAsync(lineId, "SMT", topCount);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 自动化线看板的异常报告
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topDay"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSmtChangeLineRecordData(int lineId, int topDay = 7)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetChangeLineRecordDataAsync(lineId, "SMT", topDay);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        #endregion

        #region 设备点检看板

        /// <summary>
        /// 设备点检看板看板页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> EquipCheck()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = true;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }
            return returnVM;
        }

        /// <summary>
        ///  获取设备点检信息
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="LineId">产线id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetEquipCheckData(int topCount, decimal LineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetEquipCheckDataAsync(topCount, LineId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取设备维修信息
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="equipId">设备id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetEquipRepairHeadData(int topCount, decimal equipId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetEquipRepairHeadDataAsync(topCount, equipId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取设备点检报表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetEquipKeepHeadData(int topCount, decimal equipId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetEquipKeepHeadDataAsync(topCount, equipId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        #endregion

        #region 产线上料看板

        /// <summary>
        /// 产线上料看板页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> HiMaterial()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = true;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }
            return returnVM;
        }

        #endregion

        #region 美联生产看板

        /// <summary>
        /// 获取当前工单的时间完成
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<WOPassVM>> GetWoPassTotal(int lineId)
        {
            ApiBaseReturn<WOPassVM> returnVM = new ApiBaseReturn<WOPassVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetWoPassTotal(lineId);

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
        /// 获取每小时产能
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetWoHourPass(int lineId)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetWoHourPass(lineId);

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
        /// 获取今日工单完成
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetWoToDayPass(int lineId)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetWoToDayPass(lineId);

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
        /// 获取今日工单汇总
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetWoToDayALL(int lineId)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetWoToDayALL(lineId);

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
        /// 直通率
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> GetWoToRate(int lineId)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetWoToRate(lineId);

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
        /// 最近5天的产能
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> Top5Prouduct(int lineId)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.Top5Prouduct(lineId);

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
        /// 站点统计
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetSiteStatistics(int lineId)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetSiteStatistics(lineId);

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

        #endregion

        #region  看板入口

        /// <summary>
        /// 看板入口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<bool> BoardEntry()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = true;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }
            return returnVM;
        }

        /// <summary>
		/// 根据用户ID，获取用户的所有组织
		/// </summary>
		/// <param name="user_id"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetOrganizeList(decimal user_id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetOrganizeList(user_id));

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
        /// 根据组织ID，获取到隶属该组织的所有线别
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetLines(string organizeId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetLines(organizeId));

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
        /// 根据组织ID，获取到AOI/SPI的楼层信息
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetAoiSpiFloorData(string organizeId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetAoiSpiFloorData(organizeId));

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

        #endregion

        #region AOI/SPI集成看板

        [HttpGet]
        [Authorize]
        public ApiBaseReturn<bool> AoiAndSpiReport(string ORGANIZE_ID, string FLOOR)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = true;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }
            return returnVM;
        }

        /// <summary>
        /// 获取看板数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetAoiSpiDataAsync(string organizeId, string floor)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetAoiSpiDataAsync(organizeId, floor));

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

        #endregion

        #region AI/RI集成看板

        [HttpGet]
        [Authorize]
        public ApiBaseReturn<bool> AiAndRiReport(string ORGANIZE_ID, string FLOOR)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = true;

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
        /// 获取AI&RI看板数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetAiAndRiData(string organizeId, string floor, decimal lineId = 0)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    List<decimal> ids = new List<decimal>();
                    if (lineId != 0)
                    {
                        ids.Add(lineId);
                    }

                    var data = await _repository.GetAiRiDataAsync(organizeId, floor, ids.Count > 0 ? ids : null);
                    foreach (var item in data)
                    {
                        if (item != null)
                        {
                            //获取换线时间数据
                            item.CHANGE_LINE_RESULT = (await GetChangeLineRecordDataAsync(int.Parse(item.LINE_ID.ToString()), "SMT", 5)).data;
                        }
                    }

                    returnVM.Result = JsonHelper.ObjectToJSON(data);

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

        #endregion

        #region 内部方法

        /// <summary>
        /// 看板的换线记录
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="lineType"></param>
        /// <param name="topDay"></param>
        /// <returns></returns>
        private async Task<TableDataModel> GetChangeLineRecordDataAsync(int lineId, string lineType, int topDay)
        {
            int beginMinute = 0;
            int endMinute = 1440;

            if (lineType == "PCBA")
            {
                beginMinute = 480;
                endMinute = 1260;
            }

            DateTime date = DateTime.Parse(DateTime.Now.AddDays(-topDay).ToString("yyyy-MM-dd"));
            object whereConditions = "WHERE LINE_TYPE = :LINE_TYPE AND LINE_ID = :LINE_ID AND (PRE_START_TIME >= :TIME " +
                " OR PRE_END_TIME >= :TIME OR NEXT_START_TIME >= :TIME OR NEXT_END_TIME >= :TIME) ORDER BY PRE_END_TIME ";

            string conditions = "WHERE ENABLED = 'Y' AND LINE_TYPE = :LINE_TYPE AND LINE_ID = :LINE_ID";
            var data = new List<MesChangeLineRecord>();
            if (await _repositoryIMesKanbanController.GetRecordCountAsync(conditions, new { LINE_ID = lineId, LINE_TYPE = lineType }) == 0)
            {
                data = (await _repositoryChangeLineRecord.GetListAsync(whereConditions.ToString(), new { LINE_ID = lineId, LINE_TYPE = lineType, TIME = date })).ToList();
            }
            else
            {
                var dataResult = (await _repositoryChangeLineRecordResult.GetListAsync(whereConditions.ToString(), new { LINE_ID = lineId, LINE_TYPE = lineType, TIME = date })).ToList();
                dataResult?.ForEach(x =>
                {
                    var item = _mapper.Map<MesChangeLineRecord>(x);
                    data.Add(item);
                });
            }

            //数组下标值含义：0：为Y轴下标，1：X轴开始值，2：X轴结束值，3：值，4：0表示工作，1表示换线
            List<decimal[]> ctList = new List<decimal[]>();

            var timeList = GetListTimeByDay(topDay);

            int type = 1;//当天结束时，换线状态  1表示工作状态，2表示换线状态
            for (int j = 0; j < timeList.Count; j++)
            {
                bool isCurDate = false;//表示当天是否有换线数据
                for (int i = 0; i < data.Count(); i++)
                {
                    //上个工单结束时间日期等于Y轴日期
                    if (timeList[j] == DateTime.Parse(data[i].PRE_END_TIME.ToString("yyyy-MM-dd")))
                    {
                        isCurDate = true;

                        //判断上个工单开始时间跟结束时间是否为同一天
                        if (data[i].PRE_START_TIME.Date.Equals(data[i].PRE_END_TIME.Date))
                        {
                            int sM = GetMinutes(data[i].PRE_START_TIME);
                            int eM = GetMinutes(data[i].PRE_END_TIME);
                            ctList.Add(new decimal[] { j, sM, eM, eM - sM, 0 });//上个工单开始时间到结束时间的工作时间
                        }
                        else
                        {
                            int eM = GetMinutes(data[i].PRE_END_TIME);
                            ctList.Add(new decimal[] { j, beginMinute, eM, eM - beginMinute, 0 });//上个工单当天的工作时间
                        }

                        //判断上个工单结束时间跟下个工单开始时间是否为同一天
                        if (data[i].PRE_END_TIME.Date.Equals((data[i].NEXT_START_TIME ?? DateTime.Now).Date))
                        {
                            int sM2 = GetMinutes(data[i].PRE_END_TIME);
                            int eM2 = GetMinutes(data[i].NEXT_START_TIME ?? DateTime.Now);
                            ctList.Add(new decimal[] { j, sM2, eM2, eM2 - sM2, 1 });//上个工单跟下个工单的换线时间

                            //判断下个工单开始时间跟结束时间是否为同一天(不为空情况下)
                            if (data[i].NEXT_START_TIME != null)
                            {
                                if (!(data[i].NEXT_START_TIME ?? DateTime.Now).Date.Equals((data[i].NEXT_END_TIME ?? DateTime.Now).Date))
                                {
                                    int eM3 = GetMinutes(data[i].NEXT_START_TIME ?? DateTime.Now);
                                    ctList.Add(new decimal[] { j, eM3, endMinute, endMinute - eM3, 0 });//不为同一天时，则增加开始时间到当天结束的工作时间段
                                    type = 1;
                                }
                            }
                        }
                        else
                        {
                            int sM2 = GetMinutes(data[i].PRE_END_TIME);
                            ctList.Add(new decimal[] { j, sM2, endMinute, endMinute - sM2, 1 });//上个工单跟下个工单的换线时间
                            type = 2;
                        }

                        continue;
                    }

                    //上个工单开始时间日期等于Y轴日期
                    if (timeList[j] == DateTime.Parse(data[i].PRE_START_TIME.ToString("yyyy-MM-dd")))
                    {
                        //当上个工单开始时间跟结束时间不为同一天时
                        if (!data[i].PRE_START_TIME.Date.Equals(data[i].PRE_END_TIME.Date))
                        {
                            isCurDate = true;
                            if (type == 2)
                            {
                                int sM = GetMinutes(data[i].PRE_START_TIME);
                                ctList.Add(new decimal[] { j, beginMinute, sM, sM - beginMinute, 1 });//换线时间
                                ctList.Add(new decimal[] { j, sM, endMinute, endMinute - sM, 0 });//工作时间
                                type = 1;
                            }
                        }
                    }

                    //下个工单开始时间日期等于Y轴日期
                    if (timeList[j] == DateTime.Parse((data[i].NEXT_START_TIME ?? DateTime.Now).ToString("yyyy-MM-dd")))
                    {
                        //当上个工单结束时间跟下个工单开始时间不为同一天时
                        if (!data[i].PRE_END_TIME.Date.Equals((data[i].NEXT_START_TIME ?? DateTime.Now).Date))
                        {
                            isCurDate = true;
                            int sM = GetMinutes((data[i].NEXT_START_TIME ?? DateTime.Now));
                            ctList.Add(new decimal[] { j, beginMinute, sM, sM - beginMinute, 1 });//换线时间
                            ctList.Add(new decimal[] { j, sM, endMinute, endMinute - sM, 0 });//工作时间
                            type = 1;
                        }
                    }
                }

                if (!isCurDate)
                {
                    if (type == 1)
                        ctList.Add(new decimal[] { j, beginMinute, endMinute, endMinute - beginMinute, 0 });//当天整天为工作时间
                    else
                        ctList.Add(new decimal[] { j, beginMinute, endMinute, endMinute - beginMinute, 1 });//当天整天为换线时间
                }
            }

            if (data.Count > 0)
                data[0].TimeValue = ctList;

            return new TableDataModel
            {
                count = 1,
                data = data
            };
        }

        private List<DateTime> GetListTimeByDay(int topDay)
        {
            List<DateTime> list = new List<DateTime>();

            for (int i = topDay; i > 0; i--)
            {
                list.Add(DateTime.Parse((DateTime.Now.AddDays(-i)).ToString("yyyy-MM-dd")));
            }

            return list;
        }

        private int GetMinutes(DateTime time)
        {
            return time.Hour * 60 + time.Minute;
        }

        #endregion

        #region Spi品质柏拉图
        /// <summary>
        /// Spi品质柏拉图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetSpiDataAsync([FromQuery] AoiAndSpiReportListModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetSpiReportData(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(data.data);
                    returnVM.TotalCount = data.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region Aoi品质柏拉图
        /// <summary>
        /// Aoi品质柏拉图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetAoiDataAsync([FromQuery] AoiAndSpiReportListModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetAoiReportData(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(data.data);
                    returnVM.TotalCount = data.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region Avi品质柏拉图
        /// <summary>
        /// Avi品质柏拉图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetAviDataAsync([FromQuery] AoiAndSpiReportListModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetAviReportData(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(data.data);
                    returnVM.TotalCount = data.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region Ict品质柏拉图
        /// <summary>
        /// Ict品质柏拉图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetIctDataAsync(string WO_NO)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetIctReport(WO_NO);
                    returnVM.Result = JsonHelper.ObjectToJSON(data.data);
                    returnVM.TotalCount = data.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region Fct品质柏拉图
        /// <summary>
        /// Fct品质柏拉图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetFctDataAsync(string WO_NO)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetFctReport(WO_NO);
                    returnVM.Result = JsonHelper.ObjectToJSON(data.data);
                    returnVM.TotalCount = data.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region 红胶,锡膏作业报表
        /// <summary>
        /// 红胶,锡膏作业报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetResourceRuncardReportAsync(int Limit, int Page, string resourceNo, string code, DateTime? startTime = null)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var data = await _repository.GetResourceRuncardReport(new PageModel { Page = Page, Limit = Limit }, resourceNo, code, startTime);
                    returnVM.Result = JsonHelper.ObjectToJSON(data.data);
                    returnVM.TotalCount = data.count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region QC报表
        /// <summary>
        /// QC报表
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="WO_NO"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetQCReportAsync(int lineId, string WO_NO, DateTime? date)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var data = await _repository.GetQCReport(lineId, WO_NO, date);
                    returnVM.Result = JsonHelper.ObjectToJSON(data);
                    returnVM.TotalCount = 0;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region 流程卡报表
        /// <summary>
        /// 流程卡报表(默认是SN)
        /// </summary>
        /// <param name="sn">产品流水号</param>
        /// <param name="type">类型:SN,UID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<RuncardReportListModel>> GetRuncardInfoBySn(string sn, string type = "SN")
        {
            ApiBaseReturn<RuncardReportListModel> returnVM = new ApiBaseReturn<RuncardReportListModel>();
            returnVM.Result = null;
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 参数验证
                    //if (!string.IsNullOrEmpty(sn) && !ErrorInfo.Status)
                    //{
                    //    ErrorInfo.Set(_localizer["BATCH_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //}
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status && !string.IsNullOrEmpty(sn))
                    {
                        returnVM.Result = await _repository.GetRuncardInfoBySn(sn, type);

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
        #endregion

        #region 站点统计报表
        /// <summary>
        /// 根据条件获取相应的查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<TableDataModel>> GetSiteStatisticsConditionList(StatisticsConditionRequestModel model)
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();
            returnVM.Result = null;
            try
            {
                #region 设置返回值
                if (!ErrorInfo.Status)
                {
                    returnVM.Result = model == null ? null : await _repository.GetSiteStatisticsConditionList(model);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取站点统计数据表格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<SiteStatisticsTableListModel>>> GetSiteStatisticsDataTable(StatisticalReportRequestModel model)
        {
            ApiBaseReturn<List<SiteStatisticsTableListModel>> returnVM = new ApiBaseReturn<List<SiteStatisticsTableListModel>>();

            try
            {
                #region 设置返回值
                returnVM.Result = model == null ? null : await _repository.GetSiteStatisticsDataTable(model);
                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取站点统计明细和每小时明细数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<SiteStatisticsDetailListModel>>> GetSiteStatisticsDetail(StatisticalReportRequestModel model)
        {
            ApiBaseReturn<List<SiteStatisticsDetailListModel>> returnVM = new ApiBaseReturn<List<SiteStatisticsDetailListModel>>();

            try
            {
                #region 设置返回值
                if (model != null)
                {
                    if (model.HOURDETAIL)
                    {

                        returnVM.Result = await _repository.GetSiteStatisticsHourDetail(model);
                    }
                    else
                    {
                        returnVM.Result = await _repository.GetSiteStatisticsDetail(model);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 站点统计报表获取detail report
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<SiteStatisticsReportDetailListModel>>> GetSiteStatisticsReportDetail(ReportDetailRequestModel model)
        {
            ApiBaseReturn<List<SiteStatisticsReportDetailListModel>> returnVM = new ApiBaseReturn<List<SiteStatisticsReportDetailListModel>>();

            try
            {
                #region 设置返回值

                returnVM.Result = model == null ? null : await _repository.GetSiteStatisticsReportDetail(model);

                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
        #endregion

        #region 在制品报表
        /// <summary>
        /// 获取在制品报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<WipStatisticsListModel>> GetWipStatisticsList(StatisticalReportRequestModel model)
        {
            ApiBaseReturn<WipStatisticsListModel> returnVM = new ApiBaseReturn<WipStatisticsListModel>();

            try
            {
                if (!model.ALL && (model.MODEL.Count() == 0 || model.MODEL == null) && (model.PART_NO.Count() == 0 || model.PART_NO == null) && (model.WO_NO.Count() == 0 || model.WO_NO == null) && (model.ROUTE_ID.Count() == 0 || model.ROUTE_ID == null) && (model.LINE_ID.Count() == 0 || model.LINE_ID == null))
                {
                    ErrorInfo.Set(_localizer["PARAMETER_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                if (model.ISLAGTIME && (model.LAGTIME < 0 || model.LAGTIME > 3) && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizer["LAGTIME_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }

                #region 设置返回值
                if (!ErrorInfo.Status)
                {
                    returnVM.Result = model == null ? null : await _repository.GetWipStatisticsList(model);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 在制品报表获取detail report
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<WipStatisticsReportDetailListModel>>> GetWipStatisticsReportDetail(ReportDetailRequestModel model)
        {
            ApiBaseReturn<List<WipStatisticsReportDetailListModel>> returnVM = new ApiBaseReturn<List<WipStatisticsReportDetailListModel>>();

            try
            {

                if (!model.ALL && model.OPERATION_ID == 0 && (model.MODEL.Count() == 0 || model.MODEL == null) && (model.PART_NO.Count() == 0 || model.PART_NO == null) && (model.WO_NO.Count() == 0 || model.WO_NO == null) && (model.LINE_ID.Count() == 0 || model.LINE_ID == null))
                {
                    ErrorInfo.Set(_localizer["PARAMETER_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                #region 设置返回值

                returnVM.Result = model == null ? null : await _repository.GetWipStatisticsReportDetail(model);

                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
        #endregion

        #region 不良维修报表
        /// <summary>
        /// 获取不良维修报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<DefectReportListModel>>> GetDefectReportList(DefectReportRequestModel model)
        {
            ApiBaseReturn<List<DefectReportListModel>> returnVM = new ApiBaseReturn<List<DefectReportListModel>>();

            try
            {
                if (!model.ALL && (model.MODEL.Count() == 0 || model.MODEL == null) && (model.PART_NO.Count() == 0 || model.PART_NO == null) && (model.WO_NO.Count() == 0 || model.WO_NO == null) && (model.USERS.Count() == 0 || model.USERS == null) && (model.SN.Count() == 0 || model.SN == null) && (model.FLOOR.Count() == 0 || model.FLOOR == null))
                {
                    ErrorInfo.Set(_localizer["PARAMETER_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                #region 设置返回值
                if (!ErrorInfo.Status)
                {
                    returnVM.Result = model == null ? null : await _repository.GetDefectReportList(model);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
        #endregion

        #region 小时产能报表
        /// <summary>
        /// 小时产能
        /// </summary>
        /// <param name="LINE_ID"></param>
        /// <param name="WO_NO"></param>
        /// <param name="START_TIME"></param>
        /// <param name="END_TIME"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<string>> GetKanbanHourReportAsync(int Limit, int Page, int LINE_ID = 0, string WO_NO = "", DateTime? START_TIME = null, DateTime? END_TIME = null)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var data = await _repository.GetKanbanHourReport(LINE_ID, WO_NO, START_TIME, END_TIME, new PageModel { Page = Page, Limit = Limit });
                    returnVM.Result = JsonHelper.ObjectToJSON(data);
                    returnVM.TotalCount = 0;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region 生产报表
        /// <summary>
        /// 小时产能
        /// </summary>
        /// <param name="LINE_ID"></param>
        /// <param name="WO_NO"></param>
        /// <param name="START_TIME"></param>
        /// <param name="END_TIME"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<string>> GetProductionReportAsync(int Limit, int Page, int type, string lineType, int LINE_ID = 0, string WO_NO = "", DateTime? START_TIME = null, DateTime? END_TIME = null)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var data = await _repository.GetProductionReport(LINE_ID, WO_NO, type, lineType, START_TIME, END_TIME, new PageModel { Page = Page, Limit = Limit });
                    returnVM.Result = JsonHelper.ObjectToJSON(data);
                    returnVM.TotalCount = 0;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }
        #endregion

        #region SMT产线看板

        /// <summary>
        /// 瑞晶-SMT产线看板-排产计划完成
        /// </summary>
        /// <param name="mdoel"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<ProducePlanInfoListModel>>> TopDayProducePlan([FromQuery] ProducePlanInfoRequestModel mdoel)
        {
            ApiBaseReturn<List<ProducePlanInfoListModel>> returnVM = new ApiBaseReturn<List<ProducePlanInfoListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.TopDayProducePlan(mdoel);

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
        /// 瑞晶-SMT产线看板-每小时产能
        /// </summary>
        /// <param name="mdoel"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<HourYieldListModel>> GetRefershHourYield([FromQuery] HourYieldRequestModel mdoel)
        {
            ApiBaseReturn<HourYieldListModel> returnVM = new ApiBaseReturn<HourYieldListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!String.IsNullOrEmpty(mdoel.WO_NO))
                    {
                        SfcsWo swModel = (await _repositoryApi.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = mdoel.WO_NO })).FirstOrDefault();
                        if (swModel == null || swModel.ID <= 1)
                        {
                            ErrorInfo.Set(_localizer["WO_INFO_ERR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        if (!ErrorInfo.Status)
                        {
                            mdoel.WO_ID = swModel.ID;
                            returnVM.Result = await _repository.GetRefershHourYield(mdoel);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["WO_INFO_ERR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        /// 瑞晶-SMT产线看板-AOI不良
        /// </summary>
        /// <param name="mdoel"></param>
        /// <returns>ResultInfo: AOI 不良总数</returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtAOIDefectInfoListModel>>> SmtAOIDefectInfo([FromQuery] HourYieldRequestModel mdoel)
        {
            ApiBaseReturn<List<SmtAOIDefectInfoListModel>> returnVM = new ApiBaseReturn<List<SmtAOIDefectInfoListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!String.IsNullOrEmpty(mdoel.WO_NO))
                    {
                        SfcsWo swModel = (await _repositoryApi.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = mdoel.WO_NO })).FirstOrDefault();
                        if (swModel == null || swModel.ID <= 1)
                        {
                            ErrorInfo.Set(_localizer["WO_INFO_ERR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        if (!ErrorInfo.Status)
                        {
                            mdoel.WO_ID = swModel.ID;
                            List<SmtAOIDefectInfoListModel> list = new List<SmtAOIDefectInfoListModel>();
                            var dlist = await _repository.SmtAOIDefectInfo(mdoel);
                            decimal total = dlist != null && dlist.Count() > 0 ? dlist.Sum(m => m.DEFECT_QTY) : 0;
                            decimal previous = 0;
                            for (int i = 0; i < dlist.Count; i++)
                            {
                                SmtAOIDefectInfoListModel model = new SmtAOIDefectInfoListModel();
                                model = dlist[i];
                                previous += model.DEFECT_QTY;
                                if (0.Equals(total))
                                    continue;

                                model.DEFECT_PERCENTAGE = Math.Round((previous / total) * 100, 2, MidpointRounding.AwayFromZero);
                                list.Add(model);
                            }
                            returnVM.Result = list;

                            SfcsSiteStatistics sModel = (await _repositoryApi.GetListByTableEX<SfcsSiteStatistics>("SUM(PASS) PASS,SUM(FAIL) FAIL", " SFCS_SITE_STATISTICS SS, SFCS_OPERATION_SITES OS", " AND SS.OPERATION_SITE_ID = OS.ID AND SS.WO_ID = :WO_ID AND OS.OPERATION_LINE_ID = :LINE_ID ", new { WO_ID = swModel.ID, LINE_ID = mdoel.LINE_ID })).FirstOrDefault();
                            if (sModel != null && sModel.PASS != null)
                            {
                                decimal pass = Convert.ToDecimal(sModel.PASS);
                                total = pass + Convert.ToDecimal(sModel.FAIL);
                                if (0.Equals(total))
                                {
                                    returnVM.ResultInfo = "0.00%";
                                }
                                else { returnVM.ResultInfo = Math.Round((pass / total) * 100, 2, MidpointRounding.AwayFromZero).ToString() + "%"; }
                            }
                            else
                            {
                                returnVM.ResultInfo = "0.00%";
                            }
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["WO_INFO_ERR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        /// 瑞晶-SMT产线看板-获取钢网刮刀辅料
        /// </summary>
        /// <param name="mdoel"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SMTOnlineDataListModel>> GetSMTOnlineDataByWo([FromQuery] HourYieldRequestModel mdoel)
        {
            ApiBaseReturn<SMTOnlineDataListModel> returnVM = new ApiBaseReturn<SMTOnlineDataListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    SMTOnlineDataListModel data = new SMTOnlineDataListModel();
                    var resource = await _repositoryIMesKanbanController.QueryAsyncEx<String>("SELECT RESOURCE_NO FROM SMT_RESOURCE_WO WHERE IS_USING = 'Y' AND WO_NO = :WO_NO AND LINE_ID = :LINE_ID ", mdoel);
                    data.RESOURCE = resource != null && resource.Count() > 0 ? resource[0] : "";

                    var stencil = await _repositoryIMesKanbanController.QueryAsyncEx<String>("SELECT STENCIL_NO FROM SMT_STENCIL_WO WHERE IS_USING = 'Y' AND WO_NO = :WO_NO AND LINE_ID = :LINE_ID ", mdoel);
                    data.STENCIL = stencil != null && stencil.Count() > 0 ? stencil[0] : "";

                    var scraper = await _repositoryIMesKanbanController.QueryAsyncEx<String>("SELECT SCRAPER_NO FROM SMT_SCRAPER_WO WHERE IS_USING = 'Y' AND WO_NO = :WO_NO AND LINE_ID = :LINE_ID ", mdoel);
                    if (scraper != null)
                    {
                        for (int i = 0; i < scraper.Count(); i++)
                        {
                            String scraper_no = scraper[i].ToString().Trim();
                            if (scraper_no.Contains("F"))
                            {
                                //前刮刀
                                data.SCRAPER_ONE = scraper_no;
                            }
                            else if (scraper_no.Contains("R"))
                            {
                                //后刮刀
                                data.SCRAPER_TWO = scraper_no.ToString();
                            }
                            else
                            {
                                if ((String.IsNullOrEmpty(data.SCRAPER_ONE) && String.IsNullOrEmpty(data.SCRAPER_TWO)) || (String.IsNullOrEmpty(data.SCRAPER_ONE) && !String.IsNullOrEmpty(data.SCRAPER_TWO)))
                                {
                                    data.SCRAPER_ONE = scraper_no;
                                }
                                else if (!String.IsNullOrEmpty(data.SCRAPER_ONE) && String.IsNullOrEmpty(data.SCRAPER_TWO))
                                {
                                    data.SCRAPER_TWO = scraper_no;
                                }
                            }
                        }
                    }

                    returnVM.Result = data;

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

        #endregion

        #region SMT车间看板

        /// <summary>
        /// SMT周计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<ProducePlanInfoWeekListModel>>> GetProducePlanInfoWeek(int user_id)
        {
            ApiBaseReturn<List<ProducePlanInfoWeekListModel>> returnVM = new ApiBaseReturn<List<ProducePlanInfoWeekListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetProducePlanInfoWeek(user_id);

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
        /// 全部线体的自动化线看板的AOI的直通率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtKanbanAoiPassRateModel>>> GetSmtKanbanAoiPassRateByAllLines(int user_id)
        {
            ApiBaseReturn<List<SmtKanbanAoiPassRateModel>> returnVM = new ApiBaseReturn<List<SmtKanbanAoiPassRateModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetSmtKanbanAoiPassRateByAllLines(user_id);

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
        /// SMT今日计划达成率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<PlanAchievementRateListModel>>> GetPlanAchievementRate(int user_id)
        {
            ApiBaseReturn<List<PlanAchievementRateListModel>> returnVM = new ApiBaseReturn<List<PlanAchievementRateListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetPlanAchievementRate(user_id);

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
        /// SMT今日线体任务跟踪
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<TaskTrackBySMTLineListModel>>> GetTaskTrackBySMTLine(int user_id)
        {
            ApiBaseReturn<List<TaskTrackBySMTLineListModel>> returnVM = new ApiBaseReturn<List<TaskTrackBySMTLineListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetTaskTrackBySMTLine(user_id);

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
        /// SMT线体效率对比(本周)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<EfficiencyBySMTLineListModel>>> GetEfficiencyBySMTLine(int user_id)
        {
            ApiBaseReturn<List<EfficiencyBySMTLineListModel>> returnVM = new ApiBaseReturn<List<EfficiencyBySMTLineListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetEfficiencyBySMTLine(user_id);

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

        #endregion

    }
}