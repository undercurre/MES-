using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.Extensions.Localization;
using AutoMapper;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 设备点检控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsEquipKeepController : BaseController
    {
        #region 对外方法

        private readonly IStringLocalizer<SfcsEquipKeepController> _localizer;
        private readonly ISfcsEquipKeepService _service;
        private readonly ISfcsEquipKeepRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISfcsParametersService _serviceParameters;
        private readonly ISfcsEquipmentLinesRepository _linesRepository;
        private readonly ISfcsParametersRepository _categoryRepository;
        private readonly IMapper _mapper;

        public SfcsEquipKeepController(ISfcsEquipKeepService service, IHttpContextAccessor httpContextAccessor,
            ISfcsEquipKeepRepository repository, ISfcsParametersService serviceParameters,
            ISfcsEquipmentLinesRepository linesRepository, ISfcsParametersRepository categoryRepository,
            IStringLocalizer<SfcsEquipKeepController> localizer, IMapper mapper)
        {
            _localizer = localizer;
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _serviceParameters = serviceParameters;
            _linesRepository = linesRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 设备点检首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<IndexResult> Index()
        {
            ApiBaseReturn<IndexResult> returnVM = new ApiBaseReturn<IndexResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        //var equipmentList = await _repository.GetEquipmentAsync();

                        returnVM.Result = new IndexResult()
                        {
                            CategoryList = _categoryRepository.GetEquipmentCategoryList(),
                            LinesList = _linesRepository.GetLinesList(),
                            //EquipmentList = equipmentList,
                        };
                        returnVM.TotalCount = 1;
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
        /// 查询列表
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery] EquipKeepRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var LineList = _linesRepository.GetLinesList();
                    var CategoryList = _categoryRepository.GetEquipmentCategoryList();
                    var resdata = await _service.LoadDataAsync(model);
                    foreach (var item in (List<EquipKeepListModel>)resdata.data)
                    {
                        item.Line_Name = LineList.Where(t => t.ID == item.STATION_ID)
                                                 .Select(t => t.LINE_NAME)
                                                 .FirstOrDefault() ?? string.Empty;

                        item.CATEGORY_Name = CategoryList.Where(t => t.LOOKUP_CODE == item.CATEGORY)
                                                 .Select(t => t.CHINESE)
                                                 .FirstOrDefault() ?? string.Empty;
                        if (item.KEEP_CHECK_TIME == DateTime.Parse("0001-01-01 00:00:00"))
                        {
                            item.KEEP_CHECK_TIME = null;
                        }
                    }

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
        /// 查询明细数据
        /// </summary>
        /// <param name="m_id"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadDtlData(decimal m_id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    decimal billStatus = 4;
                    if (m_id > 0)
                    {
                        billStatus = await GetBillStatus(m_id);
                    }

                    var tmpdata = await _repository.LoadDetailLine(m_id);
                    if (tmpdata != null)
                    {
                        foreach (var item in tmpdata)
                        {
                            item.BLLSTATUS = billStatus;
                        }
                    }
                    returnVM.Result = JsonHelper.ObjectToJSON(tmpdata);

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
        /// 查询主表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadMainData(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    SfcsEquipKeepHead item = await _repository.LoadMainAsync(id);
                    returnVM.Result = JsonHelper.ObjectToJSON(item);

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
        /// 获取点检配置列表
        /// </summary>
        /// <param name="equip_id"></param>
        /// <param name="keep_type"></param>
        /// <param name="user_name"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetKeepConfigData(decimal equip_id, decimal keep_type, string user_name)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var tmpdata = await _repository.GetKeepConfigLine(equip_id, keep_type);

                    returnVM.Result = JsonHelper.ObjectToJSON(tmpdata);

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
        /// 获取设备状态
        /// </summary>
        /// <param name="equip_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> GetEquipmentStatus(decimal equip_id)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var tmpdata = await _repository.GetEquipmentStatusByIdAsync(equip_id);
                    returnVM.Result = tmpdata;

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
        /// 添加或修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<DetailResult>> AddOrModify(decimal id = 0)
        {
            ApiBaseReturn<DetailResult> returnVM = new ApiBaseReturn<DetailResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        decimal CATEGROY = 0;
                        string ONLY_CODE = string.Empty;
                        string NAME = string.Empty;
                        List<SfcsEquipment> equipmentList = await _repository.GetEquipmentAsync();

                        EquipKeepHeadModel keepHead = new EquipKeepHeadModel()
                        {
                            ID = id,
                            STATION_ID = -1,
                            EQUIP_ID = -1,
                            KEEP_CHECK_STATUS = 4, //默认新增 
                            KEEP_TIME = DateTime.Now, //.ToString("yyyy-MM-dd HH:mm:ss")
                        };

                        SfcsEquipKeepHead item = await _repository.LoadMainAsync(id);
                        if (id > 0)
                        {
                            var cdata = equipmentList.FirstOrDefault(m => m.ID == item.EQUIP_ID);
                            if (cdata != null)
                            {
                                CATEGROY = equipmentList.FirstOrDefault(m => m.ID == item.EQUIP_ID).CATEGORY;
                                ONLY_CODE = equipmentList.FirstOrDefault(m => m.ID == item.EQUIP_ID).ONLY_CODE;
                                NAME = equipmentList.FirstOrDefault(m => m.ID == item.EQUIP_ID).NAME;
                            };

                        }
                        if (item != null)
                        {
                            keepHead = _mapper.Map<EquipKeepHeadModel>(item);

                            keepHead.CHECK_STATUS_CAPTION = GetCHECK_STATUS_CAPTION(item.KEEP_CHECK_STATUS);
                            keepHead.CATEGROY = CATEGROY.ToString();
                            keepHead.CATEGROY = NAME.ToString();
                            keepHead.KEEP_TYPE = item.KEEP_TYPE;
                            keepHead.ONLY_CODE = ONLY_CODE;


                        }

                        var keepTypeList = _serviceParameters.GetListByType("SFCS_EQUIPMENT_MAINTAIN");
                        var equipStatusList = GetEquipStatusList();

                        var lineList = _linesRepository.GetLinesList();
                        var CategoryList = _serviceParameters.GetEquipmentCategoryList();

                        returnVM.Result = new DetailResult()
                        {
                            KeepHead = keepHead,
                            LinesList = lineList,
                            EquipmentList = equipmentList,
                            KeepTypeList = keepTypeList,
                            EquipStatusList = equipStatusList,
                            CategoryTypeList = CategoryList
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
        /// 获取设备列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetEquipmentList()
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    List<SfcsEquipment> equipmentList = await _repository.GetEquipmentAsync();
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(equipmentList);

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
        /// 查询点检作业图数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadSOPData(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var item = await _repository.LoadSOPDataync(id);
                    returnVM.Result = JsonHelper.ObjectToJSON(item);

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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<decimal>> SaveData([FromBody] EquipKeepAddOrModifyModel model)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        
                        if (model.ID == 0)
                        {
                            var equipKeepHeadByEID = await _repository.GetListByTableEX<SfcsEquipKeepHead>("*", "SFCS_EQUIP_KEEP_HEAD", " AND EQUIP_ID=:EQUIP_ID AND KEEP_CHECK_STATUS=4 ", new
                            {
                                EQUIP_ID = model.EQUIP_ID
                            });
                            if ((equipKeepHeadByEID != null && equipKeepHeadByEID.Count > 0))
                            {
                                //同一设备，已经存在新增点检记录，请注意检查！
                                throw new Exception(_localizer["EXIST_EQUIP_RECORD"]);
                            }
                            model.KEEP_CODE = string.Format("EK{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
                            model.KEEP_CHECK_STATUS = 4;  //4: 新增 
                        }
                        decimal resdata = await _repository.SaveDataByTrans(model);
                        returnVM.Result = resdata;
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
        /// 删除单据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> Delete(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        var billStatus = await GetBillStatus(id);
                        if (billStatus != 4)
                        {
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonBillNotIsNewCode, ResultCodeAddMsgKeys.CommonBillNotIsNewMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 删除并返回

                    if (!ErrorInfo.Status)
                    {
                        var result = await _service.DeleteAsync(id);
                        if (result != null && result.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (result != null && result.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
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

        #region 更改状态

        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> PostToCheck([FromBody] BillStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    //if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model?.Operator))
                    //{
                    //	// "操作员不能为空.";
                    //	ErrorInfo.Set(_localizer["Operator_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //}
                    if (!string.IsNullOrEmpty(model.EquipStatus.ToString()) && (model.EquipStatus < 0 || model.EquipStatus > 4) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["EQUIP_STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var billStatus = await GetBillStatus(model.ID);
                        if (billStatus != 4)
                        {
                            // "单据不是新增状态,不允许提交审核.";
                            ErrorInfo.Set(_localizer["PostToCheck_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        model.Operator = string.Empty;
                        model.OperatorDatetime = null;
                        model.NewStatus = 0;
                        var resdata = await _repository.UpdateStatusById(model);
                        if (resdata > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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
        /// 审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> CheckBill([FromBody] BillStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model?.Operator))
                    {
                        // "操作员不能为空.";
                        ErrorInfo.Set(_localizer["Operator_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!string.IsNullOrEmpty(model.EquipStatus.ToString()) && (model.EquipStatus < 0 || model.EquipStatus > 4) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["EQUIP_STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var billStatus = await GetBillStatus(model.ID);
                        if (billStatus != 0)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            //string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonBillisCheckedCode, ResultCodeAddMsgKeys.CommonBillisCheckedMsg);
                            //单据不是待审核状态，不允许审核。
                            ErrorInfo.Set(_localizer["docheck_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        model.OperatorDatetime = DateTime.Now;
                        model.NewStatus = 1;
                        var resdata = await _repository.UpdateStatusById(model);
                        if (resdata > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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
        /// 拒绝
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> RejectBill([FromBody] BillStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model?.Operator))
                    {
                        // "操作员不能为空.";
                        ErrorInfo.Set(_localizer["Operator_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!string.IsNullOrEmpty(model.EquipStatus.ToString()) && (model.EquipStatus < 0 || model.EquipStatus > 4) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["EQUIP_STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var billStatus = await GetBillStatus(model.ID);
                        if (billStatus != 0)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            //string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonBillisNotCheckedCode, ResultCodeAddMsgKeys.CommonBillIsNotCheckedMsg);
                            //单据不是待审核状态，不允许拒绝。
                            ErrorInfo.Set(_localizer["rejectbill_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        model.OperatorDatetime = DateTime.Now;
                        model.NewStatus = 3;
                        var resdata = await _repository.UpdateStatusById(model);
                        if (resdata > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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
        /// 取消审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> UnCheckBill([FromBody] BillStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!string.IsNullOrEmpty(model.EquipStatus.ToString()) && (model.EquipStatus < 0 || model.EquipStatus > 4) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["EQUIP_STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var billStatus = await GetBillStatus(model.ID);
                        if (billStatus != 1)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonBillisNotCheckedCode, ResultCodeAddMsgKeys.CommonBillIsNotCheckedMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        model.Operator = string.Empty;
                        model.OperatorDatetime = null;
                        model.NewStatus = 0;
                        var resdata = await _repository.UpdateStatusById(model);
                        if (resdata > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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

        #region 获取配置信息
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<string>> GetConfigs(string ORGANIZE_ID)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var LineList = _linesRepository.GetLinesList(ORGANIZE_ID);
                    var ItemTypeList = _serviceParameters.GetListByType("SFCS_EQUIPMENT_MAINTAIN");
                    var equipmentList = await _repository.GetEquipmentAsync();
                    var CategoryList = _categoryRepository.GetEquipmentCategoryList();
                    returnVM.Result = JsonHelper.ObjectToJSON(new { LineList, ItemTypeList, equipmentList, CategoryList });
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

        #region 获取月数据
        /// <summary>
        /// 获取月数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<string>> GetMonthData([FromQuery] EquipKeepRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.LoadMonthDataAsync(model);
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

        #region 获取日数据
        /// <summary>
        /// 获取月数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetDayData([FromQuery] EquipKeepRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.LoadDayDataAsync(model);
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

        #endregion

        #region 内部方法

        /// <summary>
        /// 单据状态: (0：待审核，1：已审核，3：拒绝)
        /// </summary>
        /// <param name="id">单据ID</param>
        /// <returns></returns>
        private async Task<decimal> GetBillStatus(decimal id)
        {
            return await _repository.GetBillStatus(id);
        }

        /// <summary>
        /// 设备列表
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetEquipmentList(List<SfcsEquipment> equipmentList, decimal curKey)
        {
            List<SelectListItem> returnValue = new List<SelectListItem>();
            try
            {
                returnValue = equipmentList.Select(t => new SelectListItem
                {
                    Value = t.ID.ToString(),
                    Text = t.NAME,
                    Selected = (t.ID == curKey)
                }).ToList();

                //returnValue = (from d in equipmentList
                //			   select new SelectListItem
                //			   {
                //				   Value = d.ID.ToString(),
                //				   Text = d.NAME,
                //				   Selected = (d.ID == curKey)
                //			   }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        private List<SelectListItem> GetLineList(List<SfcsEquipmentLinesModel> lineList, string curKey)
        {
            List<SelectListItem> returnValue = new List<SelectListItem>();
            try
            {
                returnValue = lineList.Select(t => new SelectListItem
                {
                    Value = t.ID.ToString(),
                    Text = t.LINE_NAME,
                    Selected = (t.ID.ToString() == curKey)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// 获取保养类型列表
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetKeepTypeList()
        {
            List<SelectListItem> returnValue = new List<SelectListItem>();
            try
            {
                returnValue.Add(new SelectListItem { Value = "", Text = "请选择保养类型" });
                returnValue.Add(new SelectListItem { Value = "0", Text = "日保养" });
                returnValue.Add(new SelectListItem { Value = "1", Text = "月保养" });
                returnValue.Add(new SelectListItem { Value = "2", Text = "年保养" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// 获取设备状态列表
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> GetEquipStatusList()
        {
            List<SelectListItem> returnValue = new List<SelectListItem>();
            try
            {
                returnValue.Add(new SelectListItem { Value = "", Text = "请选择设备状态" });
                returnValue.Add(new SelectListItem { Value = "0", Text = "正常" });
                returnValue.Add(new SelectListItem { Value = "1", Text = "闲置" });
                returnValue.Add(new SelectListItem { Value = "2", Text = "待维修" });
                returnValue.Add(new SelectListItem { Value = "3", Text = "维修中" });
                returnValue.Add(new SelectListItem { Value = "4", Text = "报废" });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// 获取设备点检状态说明
        /// </summary>
        /// <param name="check_status"></param>
        /// <returns></returns>
        private string GetCHECK_STATUS_CAPTION(decimal? check_status)
        {
            string returnValue = string.Empty;
            try
            {
                if (check_status == 0)
                    returnValue = "待审核";
                else if (check_status == 1)
                    returnValue = "已审核";
                else if (check_status == 3)
                    returnValue = "拒绝";
                else if (check_status == 4)
                    returnValue = "新增";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// 首页返回类
        /// </summary>
        public class IndexResult
        {
            /// <summary>
            /// 设备分类
            /// </summary>
            public List<SfcsParameters> CategoryList { get; set; }

            /// <summary>
            /// 所有线别列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsEquipmentLinesModel> LinesList { get; set; }

            /// <summary>
            /// 设备列表
            /// </summary>
            public List<SfcsEquipment> EquipmentList { get; set; }
        }

        /// <summary>
        /// 明细返回类
        /// </summary>
        public class DetailResult
        {
            /// <summary>
            /// 主表
            /// </summary>
            public EquipKeepHeadModel KeepHead { get; set; }

            /// <summary>
            /// 线别列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsEquipmentLinesModel> LinesList { get; set; }

            /// <summary>
            /// 设备列表
            /// </summary>
            public List<SfcsEquipment> EquipmentList { get; set; }

            /// <summary>
            /// 保养类型列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsParameters> KeepTypeList { get; set; }

            /// <summary>
            /// 设备状态列表
            /// </summary>
            public List<SelectListItem> EquipStatusList { get; set; }

            /// <summary>
            /// 设备类型
            /// </summary>
            public List<SfcsParameters> CategoryTypeList { get; set; }

        }

        #endregion
    }
}