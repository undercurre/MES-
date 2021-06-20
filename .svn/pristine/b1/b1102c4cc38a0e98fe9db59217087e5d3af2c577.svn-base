/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-16 12:07:39                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsProductStoplineController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using FluentValidation.Results;
using JZ.IMS.IRepository;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Validation;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 产品停线管控规则维护 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsProductStoplineController : BaseController
    {
        private readonly ISfcsProductStoplineRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ShareResourceController> _localizer;
        private readonly IMesStoplineLinesRepository _messtoplines;
        private readonly IMesStoplineCallRepository _messtoplinecall;
        private readonly IMesStoplinePnRepository _messtoplinepn;


        public SfcsProductStoplineController(ISfcsProductStoplineRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<ShareResourceController> localizer, IMesStoplineLinesRepository messtoplines , IMesStoplineCallRepository messtoplinecall, IMesStoplinePnRepository messtoplinepn)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _messtoplines = messtoplines;
            _messtoplinecall = messtoplinecall;
            _messtoplinepn = messtoplinepn;
        }

        /// <summary>
        /// 主页下拉框
        /// </summary>
        public class IndexVM
        {
            /// <summary>
            /// 线别ID
            /// </summary>
            public List<dynamic> LINE_ID { get; set; }

            /// <summary>
            /// 管控模式
            /// </summary>
            public List<dynamic> STOPLINE_MODE { get; set; }

            /// <summary>
            /// 管控工序
            /// </summary>
            public List<dynamic> STOP_OPERATION_ID { get; set; }

            /// <summary>
            /// 线体类别
            /// </summary>
            public List<dynamic> LINE_TYPE { get; set; }

            /// <summary>
            /// 单位
            /// </summary>
            public List<dynamic> UnitList { get; set; }
        }

        /// <summary>
        /// 新增、编辑页面下拉框
        /// </summary>
        public class CallConfigVM {
            
            /// <summary>
            /// 异常种类
            /// </summary>
            public List<dynamic> CALL_CATEGORY_CODE { get; set; }

            /// <summary>
            /// 异常类型
            /// </summary>
            public List<dynamic> CALL_TYPE_CODE { get; set; }

            /// <summary>
            /// 异常标题
            /// </summary>
            public List<dynamic> CALL_TITLE { get; set; }
        }
 

   

        #region  08-24之前的代码



        /// <summary>
        /// 查询制程名称列表
        /// </summary>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetRoutesList()
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    //if (!ErrorInfo.Status && part_no.IsNullOrWhiteSpace())
                    //{
                    //    ErrorInfo.Set(_localizer["part_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //}

                    //if (!ErrorInfo.Status)
                    //{
                    //    var result = await _repository.ItemIsExist("SFCS_PN", "PART_NO", part_no);
                    //    if (result == false)
                    //    {
                    //        //输入的料号{0}不存在。
                    //        string errmsg = string.Format(_localizer["part_no_noexist"], part_no);
                    //        ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //    }
                    //}

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetListByTable("ID, Route_Name", "SFCS_ROUTES", " and 1=1  order by ROUTE_NAME desc ");
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
        /// 
        /// </summary>
        public class LoadDataVM
        {
            /// <summary>
            /// 制程工序列表
            /// </summary>
            public List<SfcsOperations> SfcsOperationsList { get; set; }

            /// <summary>
            /// 停线管控模式列表
            /// </summary>
            public List<dynamic> ModeList { get; set; }

            /// <summary>
            /// 分割单位列表
            /// </summary>
            public List<dynamic> DivisionUnitList { get; set; }

            /// <summary>
            /// 产品停线管控规则列表
            /// </summary>
            public List<SfcsProductStopline> ProductStoplineList { get; set; }
        }

        /// <summary>
        /// 查询数据(制程工序列表及产品停线管控规则配置列表)
        /// </summary>
        /// <param name="part_no">料号</param>
        /// <param name="route_id">制程ID</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<LoadDataVM>> LoadData(string part_no, decimal route_id)
        {
            ApiBaseReturn<LoadDataVM> returnVM = new ApiBaseReturn<LoadDataVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && part_no.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["part_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ItemIsExist("SFCS_PN", "PART_NO", part_no);
                        if (result == false)
                        {
                            //输入的料号{0}不存在。
                            string errmsg = string.Format(_localizer["part_no_noexist"], part_no);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && route_id <= 0)
                    {
                        ErrorInfo.Set(_localizer["route_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ItemIsExist("SFCS_ROUTES", "ID", route_id.ToString());
                        if (result == false)
                        {
                            //此制程不存在。
                            ErrorInfo.Set(_localizer["route_id_noexist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetRouteConfigLists(route_id);
                        if (resdata != null && resdata.Count > 0)
                        {
                            returnVM.Result = new LoadDataVM
                            {
                                SfcsOperationsList = resdata,
                                ModeList = await _repository.GetListByTable("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "And LOOKUP_TYPE='STOPLINE_MODE' And Enabled='Y' order by LOOKUP_CODE"),
                                DivisionUnitList = await _repository.GetListByTable("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "And LOOKUP_TYPE='MONITOR_UNIT' And Enabled='Y' order by LOOKUP_CODE"),
                                ProductStoplineList = await _repository.GetSfcsProductStoplineList(part_no, route_id),
                            };
                        }
                        else
                        {
                            //制程没有配置，请确认。
                            ErrorInfo.Set(_localizer["routeconfig_noexist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
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

        public class StoplineModeVM
        {
            /// <summary>
            /// 分割单位
            /// </summary>
            public decimal DivisionUnit { get; set; }

            /// <summary>
            /// 是否累计误测(为空时不用修改此项目)
            /// </summary>
            public string INCLUDE_NDF { get; set; } = string.Empty;
        }

        /// <summary>
        /// 修改[停线管控模式]時同時修改单位,是否累计误测 
        /// </summary>
        /// <param name="stopline_mode">停线管控模式ID</param>
        /// <remarks>
        /// 新增时的默认值: INCLUDE_NDF = "N";
        /// INPUT_CONTROL = "N";
        /// ENABLED = "Y";
        /// ALARM_CRITERIA = 1;
        /// STOP_CRITERIA = 1;
        /// DIVISION_CRITERIA = 50;
        /// DIVISION_START = 0;
        /// ALARM_INTERVAL = 1;
        /// INPUT_CONTROL_CRITERIA = 0;
        /// </remarks>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<StoplineModeVM> GetDivisionUnit(decimal stopline_mode)
        {
            ApiBaseReturn<StoplineModeVM> returnVM = new ApiBaseReturn<StoplineModeVM>();
            StoplineModeVM modeVM = new StoplineModeVM();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    switch ((int)stopline_mode)
                    {
                        case (int)GlobalVariables.FailCountInBaseCountControl:
                        case (int)GlobalVariables.NDFCountInBaseCountControl:
                        case (int)GlobalVariables.ContinuousFailCountControl:
                        case (int)GlobalVariables.ContinuousDefectCodeControl:
                        case (int)GlobalVariables.SMTComponentFailCountInBaseCountControl:
                        case (int)GlobalVariables.SMTComponentLocationFailCountInBaseCountControl:
                        case (int)GlobalVariables.SMTMachineFailCountInBaseCountControl:
                        case (int)GlobalVariables.SMTSlotFailCountInBaseCountControl:
                        case (int)GlobalVariables.SameErrorCodeInBaseCountControl:
                            {
                                modeVM.DivisionUnit = GlobalVariables.Pieces;
                                break;
                            }
                        case (int)GlobalVariables.FailCountInBaseTimeControl:
                        case (int)GlobalVariables.FailRateInBaseTimeControl:
                        case (int)GlobalVariables.NDFCountInBaseTimeControl:
                        case (int)GlobalVariables.NDFRateInBaseTimeControl:
                        case (int)GlobalVariables.LocationFailRateInBaseTimeControl:
                            {
                                modeVM.DivisionUnit = GlobalVariables.Hour;
                                break;
                            }
                    }
                    switch ((int)stopline_mode)
                    {
                        case (int)GlobalVariables.NDFCountInBaseCountControl:
                        case (int)GlobalVariables.NDFCountInBaseTimeControl:
                        case (int)GlobalVariables.NDFRateInBaseTimeControl:
                            {
                                modeVM.INCLUDE_NDF = GlobalVariables.EnableY;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                    returnVM.Result = modeVM;

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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsProductStoplineModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            SfcsPn sfcsPn = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.PART_NO.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["part_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        sfcsPn = await _repository.GetAsyncEx<SfcsPn>("Where PART_NO =:PART_NO", new { model.PART_NO });
                        if (sfcsPn == null)
                        {
                            //输入的料号{0}不存在。
                            string errmsg = string.Format(_localizer["part_no_noexist"], model.PART_NO);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && model.ROUTE_ID <= 0)
                    {
                        ErrorInfo.Set(_localizer["route_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ItemIsExist("SFCS_ROUTES", "ID", model.ROUTE_ID.ToString());
                        if (result == false)
                        {
                            //此制程不存在。
                            ErrorInfo.Set(_localizer["route_id_noexist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        await CheckBeforeSave(model, sfcsPn);
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model);
                        if (resdata != -1)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                        }
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

        #region 内部方法

        /// <summary>
        /// 取得TU工序在制程中的Order NO
        /// </summary>
        /// <returns></returns>
        private async Task<decimal> GetTUOperationOderNo(decimal route_id)
        {
            string conditions = "WHERE ROUTE_ID =:ROUTE_ID AND CURRENT_OPERATION_ID =:OPERATION_ID";
            SfcsRouteConfig routeConfig = await _repository.GetAsyncEx<SfcsRouteConfig>(conditions, new
            {
                ROUTE_ID = route_id,
                OPERATION_ID = GlobalVariables.TUOperation
            });
            return routeConfig?.ORDER_NO ?? -1;
        }

        /// <summary>
        /// 取得工序在制程中的Order No
        /// </summary>
        /// <param name="operationCode"></param>
        /// <returns></returns>
        private async Task<decimal> GetOperationOrderNo(decimal operationCode)
        {
            string conditions = "WHERE PRODUCT_OPERATION_CODE =:OPERATION_ID";
            SfcsRouteConfig routeConfig = await _repository.GetAsyncEx<SfcsRouteConfig>(conditions, new
            {
                OPERATION_ID = operationCode
            });
            return routeConfig?.ORDER_NO ?? 0;
        }

        /// <summary>
        /// 保存前檢查
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckBeforeSave(SfcsProductStoplineModel model, SfcsPn sfcsPn)
        {
            decimal tuOperationOrderNo = await GetTUOperationOderNo(model.ROUTE_ID);
            if (model.InsertRecords?.Count > 0)
            {
                foreach (var stopline in model.InsertRecords)
                {
                    await CheckItem(stopline, tuOperationOrderNo, sfcsPn);
                }
            }

            if (model.UpdateRecords?.Count > 0)
            {
                foreach (var stopline in model.UpdateRecords)
                {
                    await CheckItem(stopline, tuOperationOrderNo, sfcsPn);
                }
            }
            return true;
        }

        /// <summary>
        /// 檢查单项
        /// </summary>
        /// <param name="item"></param>
        /// <param name="tuOperationOrderNo"></param>
        /// <param name="sfcsPn"></param>
        /// <returns></returns>
        private async Task CheckItem(SfcsProductStoplineAddOrModifyModel item, decimal tuOperationOrderNo, SfcsPn sfcsPn)
        {
            decimal plantCode = sfcsPn.CLASSIFICATION;
            decimal partNumberCategory = sfcsPn.CATEGORY;
            string errmsg = string.Empty;
            if (item.STOPLINE_MODE == null)
            {
                //没有选择监控模式，请确认。
                throw new Exception(_localizer["Err_No_Monitor_Mode"]);
            }
            if (item.ALARM_CRITERIA > item.STOP_CRITERIA)
            {
                //配置的警告标准大于中止标准，请确认。
                throw new Exception(_localizer["Err_Alarm_Criteria_Over_Stop_Criteria"]);
            }
            if (item.DIVISION_START > item.DIVISION_CRITERIA)
            {
                //开始计算切入点必需少于分割标准，请确认。
                throw new Exception(_localizer["Err_Division_Start_Over_Division_Criteria"]);
            }
            if (item.STOP_OPERATION_CODE == null)
            {
                //没有配置管控的停线工序，请确认。
                throw new Exception(_localizer["Err_Not_Input_Stop_Operation"]);
            }
            if ((item.STOPLINE_MODE == GlobalVariables.NDFCountInBaseCountControl) ||
                (item.STOPLINE_MODE == GlobalVariables.NDFCountInBaseTimeControl) ||
                (item.STOPLINE_MODE == GlobalVariables.NDFRateInBaseTimeControl))
            {
                if (item.INCLUDE_NDF != GlobalVariables.EnableY)
                {
                    //NDF相关的管控必需选择累计误测数量，请确认。
                    throw new Exception(_localizer["Err_Not_Include_NDF"]);
                }
            }
            decimal stopOperationOrderNo = await GetOperationOrderNo(item.STOP_OPERATION_CODE ?? 0);

            if (plantCode == GlobalVariables.pcbCode)
            {
                // 含有T/U工序才需要比對
                if (tuOperationOrderNo != -1)
                {
                    if ((partNumberCategory == GlobalVariables.SMTClassficationCode) &&
                    (stopOperationOrderNo > tuOperationOrderNo))
                    {
                        //SMT段料号{0}流程设定只能在其实工序到T/U(含)之间，请确认。
                        errmsg = string.Format(_localizer["Err_SMT_PN_Over_Route"], sfcsPn.PART_NO);
                        throw new Exception(errmsg);
                    }
                    if ((partNumberCategory == GlobalVariables.PCBClassficationCode) && (stopOperationOrderNo <= tuOperationOrderNo))
                    {
                        //PCB段料号{0}流程设定只能在T/U(含)到制程结束工序之间，请确认。
                        errmsg = string.Format(_localizer["Err_PCB_PN_Over_Route"], sfcsPn.PART_NO);
                        throw new Exception(errmsg);
                    }
                }
            }
            if (item.INPUT_CONTROL == GlobalVariables.EnableY)
            {
                if (item.INPUT_OPERATION_CODE == null)
                {
                    //没有配置管控的投入工序，请确认。
                    throw new Exception(_localizer["Err_Not_Input_Input_Operation"]);
                }
                decimal inputOperationOrderNo = await GetOperationOrderNo(item.INPUT_OPERATION_CODE ?? 0);
                if (inputOperationOrderNo > stopOperationOrderNo)
                {
                    //投入工序不能在停线工序之后，请确认。
                    throw new Exception(_localizer["Err_Input_Operation_After_Stop_Operation"]);
                }
                if (plantCode == GlobalVariables.pcbCode)
                {
                    // 含有T/U工序才需要比對
                    if (tuOperationOrderNo != -1)
                    {
                        if ((partNumberCategory == GlobalVariables.SMTClassficationCode) && (inputOperationOrderNo > tuOperationOrderNo))
                        {
                            //SMT段料号{0}流程设定只能在其实工序到T/U(含)之间，请确认。
                            errmsg = string.Format(_localizer["Err_SMT_PN_Over_Route"], sfcsPn.PART_NO);
                            throw new Exception(errmsg);
                        }
                        if ((partNumberCategory == GlobalVariables.PCBClassficationCode) && (inputOperationOrderNo <= tuOperationOrderNo))
                        {
                            //PCB段料号{0}流程设定只能在T/U(含)到制程结束工序之间，请确认。
                            errmsg = string.Format(_localizer["Err_PCB_PN_Over_Route"], sfcsPn.PART_NO);
                            throw new Exception(errmsg);
                        }
                    }
                }
            }
        }

        #endregion

        #endregion





        #region  08-24

        /// <summary>
        /// 首页视图(获取线体、管控模式、管控工序、线体类别、单位 下拉框值)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<IndexVM>> Index()
        {
            ApiBaseReturn<IndexVM> returnVM = new ApiBaseReturn<IndexVM>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new IndexVM()
                        {
                            LINE_ID = await _repository.GetLINENAME(),
                            STOPLINE_MODE = await _repository.GetListByTable("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "and LOOKUP_TYPE='STOPLINE_MODE' and ENABLED='Y' order by CHINESE asc"),
                            STOP_OPERATION_ID = await _repository.GetOperation(),
                            LINE_TYPE = await _repository.GetListByTable("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "and LOOKUP_TYPE='MES_LINE_TYPE' and ENABLED='Y'"),
                            UnitList = await _repository.GetListByTable("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "And LOOKUP_TYPE='MONITOR_UNIT' And Enabled='Y' order by LOOKUP_CODE"),
                        };
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
        /// 编辑页面-呼叫内容下拉框视图（获取异常种类、异常类型、异常标题下拉框）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<CallConfigVM>> CallConfigIndex()
        {
            ApiBaseReturn<CallConfigVM> returnVM = new ApiBaseReturn<CallConfigVM>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new CallConfigVM()
                        {
                            CALL_CATEGORY_CODE = await _repository.GetListByTable("LOOKUP_CODE,DESCRIPTION", "SFCS_PARAMETERS", "and LOOKUP_TYPE='ALARM_CATEGORY' and ENABLED='Y'"),
                            CALL_TYPE_CODE = await _repository.GetListByTable("LOOKUP_CODE,DESCRIPTION", "SFCS_PARAMETERS", "and LOOKUP_TYPE='ALARM_TYPE' and ENABLED='Y'"),
                            CALL_TITLE = await _messtoplinecall.GetCallTitle(),
                        };
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



        #region  停线管控主表和呼叫子表





        /// <summary>
        /// 保存停线管控表、呼叫表数据-New
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveDataNew([FromBody] SfcsProductStoplineModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model);
                        if (resdata != -1)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                        }
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
        /// 查询停线管控页面数据-New
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadDataNew([FromQuery] SfcsProductStoplineRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetSfcsProductStoplinesList(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 根据主表ID获取停线管控主表和呼叫子表数据(主表编辑时需要带出主表信息和呼叫子表信息，CallID为子表ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetStopLineAndCallDataByID(decimal id)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _repository.GetStopLineAndCallDataByID(id);
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;


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
        /// 根据异常种类、类型和异常标题获取异常内容配置表(ID、异常代码CALL_CODE、内容模板CHINESE)信息
        /// </summary>
        /// <param name="code">种类</param>
        /// <param name="typecode">类型</param>
        /// <param name="title">异常标题</param>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetCallContentConfigByTypeCode(string code, string typecode, string title)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _messtoplinecall.GetCallContentConfigByTypeCode(code, typecode, title);
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;


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
        /// 通过ID更改停线管控主表激活状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeEnabled([FromBody] ChangeStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 更改激活状态并返回

                    if (!ErrorInfo.Status)
                    {
                        //判断状态是否发生变化，没有则修改，有则返回状态已变化无法更改状态的提示
                        var isLock = await _repository.GetEnableStatus(model.Id);
                        if (isLock != model.Status)
                        {
                            var count = await _repository.ChangeEnableStatus(model.Id, model.Status);
                            if (count > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                                //通用提示类的本地化问题处理
                                string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                    ResultCodeAddMsgKeys.CommonExceptionMsg);
                                ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonDataStatusChangeCode,
                                ResultCodeAddMsgKeys.CommonDataStatusChangeMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
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
        /// 删除(删除停线管控主表数据)
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status && id <= 0)
                    {
                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
                            ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var count = await _repository.DeleteAsync(id);
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                ResultCodeAddMsgKeys.CommonExceptionMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
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



        #region  停线管控线别表


        /// <summary>
        /// 查询停线管控线别表数据(页面下方展示)
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadDataStopLines([FromQuery] MesStoplineLinesRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists =await _messtoplines.LoadDataStopLines(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 查询数据(新增停线管控线别表页面时)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetStopLinesToAdd([FromQuery] MesStoplineLinesRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _messtoplines.GetStopLinesToAdd(model);
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;


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
        /// 保存数据(保存停线管控线别表数据)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveDataStopLines([FromBody] MesStoplineLinesModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _messtoplines.SaveDataByTrans(model);
                        if (resdata != -1)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                        }
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
        /// 根据ID删除停线管控线别表数据
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteStopLinesById(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status && id <= 0)
                    {
                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
                            ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var count = await _messtoplines.DeleteAsync(id);
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                ResultCodeAddMsgKeys.CommonExceptionMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
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
        /// 通过ID更改停线管控线别表数据激活状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeStopLinesEnabled([FromBody] ChangeStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 更改激活状态并返回

                    if (!ErrorInfo.Status)
                    {
                        //判断状态是否发生变化，没有则修改，有则返回状态已变化无法更改状态的提示
                        var isLock = await _messtoplines.GetEnableStatus(model.Id);
                        if (isLock != model.Status)
                        {
                            var count = await _messtoplines.ChangeEnableStatus(model.Id, model.Status);
                            if (count > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                                //通用提示类的本地化问题处理
                                string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                    ResultCodeAddMsgKeys.CommonExceptionMsg);
                                ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonDataStatusChangeCode,
                                ResultCodeAddMsgKeys.CommonDataStatusChangeMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
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

        #endregion



        #region  停线管控产品表



        /// <summary>
        /// 保存停线管控产品表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveLinePNData([FromBody] MesStoplinePnModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _messtoplinepn.SaveDataByTrans(model);
                        if (resdata != -1)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                        }
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
        /// 查询停线管控产品表数据(页面下方展示)
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadDataLinePN([FromQuery] MesStoplinePnRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var lists = await _messtoplinepn.LoadDataLinePN(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 查询数据(新增停线管控产品表页面)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetLinePNoAdd([FromQuery] MesStoplinePnRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _messtoplinepn.GetLinePNToAdd(model);
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;


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
        /// 根据ID删除停线管控产品表数据
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteLinePNById(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status && id <= 0)
                    {
                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
                            ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var count = await _messtoplinepn.DeleteAsync(id);
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                ResultCodeAddMsgKeys.CommonExceptionMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
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
        /// 通过ID更改停线管控产品表数据激活状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeLinePNEnabled([FromBody] ChangeStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 更改激活状态并返回

                    if (!ErrorInfo.Status)
                    {
                        //判断状态是否发生变化，没有则修改，有则返回状态已变化无法更改状态的提示
                        var isLock = await _messtoplinepn.GetEnableStatus(model.Id);
                        if (isLock != model.Status)
                        {
                            var count = await _messtoplinepn.ChangeEnableStatus(model.Id, model.Status);
                            if (count > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                                //通用提示类的本地化问题处理
                                string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                    ResultCodeAddMsgKeys.CommonExceptionMsg);
                                ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonDataStatusChangeCode,
                                ResultCodeAddMsgKeys.CommonDataStatusChangeMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
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







        #endregion





        #endregion



    }
}