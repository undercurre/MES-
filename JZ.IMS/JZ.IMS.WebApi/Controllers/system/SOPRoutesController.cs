using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using JZ.IMS.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JZ.IMS.ViewModels.SOPRoutes;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using JZ.IMS.Core.Extensions;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using JZ.IMS.IRepository;
using JZ.IMS.Models.SOP;
using System.Net;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.WebApi.Validation;
using JZ.IMS.WebApi.Common;
using System.IO;
using System.Text;
using Spire.Xls;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 工艺路线控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SOPRoutesController : BaseController
    {

        private readonly ISOPRoutesService _service;
        private readonly ISfcsOperationsService _operationsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly ISOP_ROUTESRepository _repository;
        private readonly IStringLocalizer<SOPRoutesController> _localizer;
        private readonly IStringLocalizer<MenuController> _menuLocalizer;
        public SOPRoutesController(ISOPRoutesService service, ISfcsOperationsService operationsService, IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnv, ISOP_ROUTESRepository repository, IStringLocalizer<SOPRoutesController> localizer,
            IStringLocalizer<MenuController> menuLocalizer)
        {
            _localizer = localizer;
            _menuLocalizer = menuLocalizer;
            _service = service;
            _operationsService = operationsService;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnv = hostingEnv;
            _repository = repository;
        }

        /// <summary>
        /// 工艺路线首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> Index()
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
                        returnVM.TotalCount = 1;
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
        /// 工艺路线列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery] SOPRoutesRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.LoadData(model);
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(resdata.data);
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
        /// 工艺路线明细主表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<DetailResult>> AddOrModify(int id = 0)
        {
            ApiBaseReturn<DetailResult> returnVM = new ApiBaseReturn<DetailResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    DetailResult result = new DetailResult();

                    SOP_ROUTES item = await _service.GetMainAsync(id);
                    result.ID = id;
                    if (item != null)
                    {
                        result.STATUS = item.STATUS;
                        result.PART_NO = item.PART_NO;
                        result.ROUTE_NAME = item.ROUTE_NAME;
                        result.DESCRIPTION = item.DESCRIPTION;
                    }

                    if (id > 0)
                    {
                        var resdata = await _repository.LoadResourceProductData(Convert.ToDecimal(id));
                        if (resdata != null)
                        {
                            result.m_ResourceID = resdata.ID;
                            result.m_RESOURCE_URL = resdata.RESOURCE_URL;
                        }
                    }

                    returnVM.Result = result;

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
        /// 预览/编辑 资源 
        /// </summary>
        /// <param name="mst_id">主表ID</param>
        /// <param name="id">资源图片ID</param>
        /// <param name="operid">工序ID</param>
        /// <param name="imgurl">图片URL</param>
        /// <param name="isedit">0"预览图片",1"编辑图片</param>
        /// <param name="isPart">是否零件</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<PreviewSOPVM>> PreviewSOP(string mst_id, string id, string operid, string imgurl, string isedit, string isPart)
        {
            ApiBaseReturn<PreviewSOPVM> returnVM = new ApiBaseReturn<PreviewSOPVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    PreviewSOPVM result = new PreviewSOPVM();
                    result.id = id;
                    result.operid = operid;
                    result.imgurl = imgurl;
                    result.isedit = isedit;
                    result.isPart = isPart;
                    result.mstId = mst_id;
                    result.RESOURCE_MSG = "";

                    if (!id.IsNullOrWhiteSpace())
                    {
                        SOP_OPERATIONS_ROUTES_RESOURCE resdata = await _repository.LoadResourceByID(Convert.ToDecimal(id));
                        result.RESOURCE_MSG = resdata.RESOURCE_MSG ?? string.Empty;

                        result.PartInfo = await _repository.GetSourcePartBySourceId(id);
                    }
                    if (result.PartInfo == null)
                        result.PartInfo = new SopOperationsRoutesPartListModel();

                    returnVM.Result = result;

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
        /// 添加工序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> AddOperations()
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
        /// 加载作业图列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<string>> LoadResource([FromQuery] MenuRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (string.IsNullOrWhiteSpace(model.parentid))
                    {
                        model.parentid = "0";
                    }
                    var resdata = await _service.LoadResource(Convert.ToDecimal(model.parentid));
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
        /// 零件图:资源列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<string>> LoadResourceCmpData([FromQuery] MenuRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (string.IsNullOrWhiteSpace(model.parentid))
                    {
                        model.parentid = "0";
                    }
                    var resdata = await _repository.LoadResourceCmpData(Convert.ToDecimal(model.parentid));
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
        /// 产品图:资源列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<string>> LoadResourceProductData([FromQuery] MenuRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (string.IsNullOrWhiteSpace(model.parentid))
                    {
                        model.parentid = "0";
                    }
                    var resdata = await _service.LoadResource(Convert.ToDecimal(model.parentid));
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
        /// 工序子表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<string>> LoadDtlData(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.LoadDtlData(id);
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(resdata.data);
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
        /// 工序列表
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetOperationList([FromQuery] PageModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var result = await _operationsService.GetEnabledListsync(model);
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(result.data);
                    returnVM.TotalCount = result.count;

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
        /// 保存单据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> AddOrModifySave([FromBody] SOPRoutesAddOrModifyModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (BillIsChecked(item.ID))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;

                        returnVM.Result = "-1";
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.AddOrModifyAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = resultData.ResultData;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = "-1";
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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

                    if (BillIsChecked(id))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;

                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.DeleteAsync(id);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 删除工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteSub(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (BillIsChecked(id))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;

                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.DeleteSubAsync(id);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteResource(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (BillIsChecked(id))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;

                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        await _repository.DeleteResourcePart(id);
                        var resultData = await _repository.DeleteResource(id);
                        if (resultData > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            var result = new BaseResult();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = ResultCodeAddMsgKeys.CommonExceptionMsg;

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

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AuditByIdAsync([FromBody] ChangeStatusModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_menuLocalizer);
                        ValidationResult results = validationRules.Validate(item);
                        if (!results.IsValid)
                        {
                            ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 更改审核状态并返回

                    if (!ErrorInfo.Status)
                    {
                        item.Status = true;
                        item.OperatorDatetime = DateTime.Now;
                        var resultData = await _service.AuditByIdAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 取消审核
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> UnAuditByIdAsync([FromBody] ChangeStatusModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_menuLocalizer);
                        ValidationResult results = validationRules.Validate(item);
                        if (!results.IsValid)
                        {
                            ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 更改审核状态并返回

                    if (!ErrorInfo.Status)
                    {
                        item.Status = false;
                        item.OperatorDatetime = DateTime.Now;
                        var resultData = await _service.AuditByIdAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 是否存在相同的名称的料号
        /// </summary>
        /// <param name="item"></param>
        /// <remarks>
        /// 参数: 主键ID, 料号:PART_NO 必传
        /// </remarks>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<bool>> IsExistsName([FromQuery] SOPRoutesAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var result = await _service.IsExistsNameAsync(item);
                    returnVM.Result = result.Data;

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
        /// 保存工序(图片上传时) 
        /// </summary>
        /// <remarks>
        /// 说明:
        /// 返回工序ID: 保存成功: id>0, 失败: -1; 
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public ApiBaseReturn<int> AddOperOfUploadImage([FromBody] AddOperOfUploadImageModel model)
        {
            ApiBaseReturn<int> returnVM = new ApiBaseReturn<int>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (BillIsChecked(Convert.ToDecimal(model.routid)))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;
                        returnVM.Result = -1;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 保存工序并返回工序ID

                    if (!ErrorInfo.Status)
                    {
                        //保存工序
                        decimal new_id = 0;
                        if (model.id == "0")
                        {
                            new_id = _repository.Get_Detail_SEQID();
                            if (new_id > 0)
                            {
                                var obj = new SOP_OPERATIONS_ROUTES
                                {
                                    ID = new_id,
                                    ROUTE_ID = Convert.ToDecimal(model.routid),
                                    ORDER_NO = new_id,
                                    CURRENT_OPERATION_ID = Convert.ToDecimal(model.operid),
                                    NEXT_OPERATION_ID = 0,
                                    PREVIOUS_OPERATION_ID = 0,
                                };

                                _repository.InsertDetail(obj);
                                model.id = Convert.ToString(new_id);
                            }
                        }
                        returnVM.Result = Convert.ToInt32(model.id);
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
        /// 图片上传
        /// </summary>
        /// <param name="mst_id">关联ID</param>
        /// <param name="category">资源类别(0：产品图，1：作业图，2：零件图)</param>
        /// <param name="resource_id">原有资料id(产品类型使用)</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public ApiBaseReturn<UploadImageResult> UploadImage([FromForm] decimal mst_id, [FromForm] string category, [FromForm] decimal resource_id)
        {
            ApiBaseReturn<UploadImageResult> returnVM = new ApiBaseReturn<UploadImageResult>();

            var imgFile = Request.Form.Files[0];
            var resource_name = string.Empty;
            long size = 0;
            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (imgFile == null || imgFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(imgFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        resource_name = filename;

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(imgFile.Length / 1024.00, 3));
                        long mb = imgFile.Length / 1024 / 1024; // MB
                        if (mb > 50)
                        {
                            //return Json(new { code = 1, msg = "只允许上传小于 5MB 的图片.", });
                            ErrorInfo.Set(_localizer["upload_size_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        #endregion
                    }

                    #endregion

                    #region 保存文件并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var filenameNew = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var path = $"/upload/sopfile/"; //+ DateTime.Now.ToString("yyyyMM");
                        string dir = @"upload\sopfile\";
                        var pathWebRoot = AppContext.BaseDirectory + dir;
                        if (!Directory.Exists(pathWebRoot))
                        {
                            Directory.CreateDirectory(pathWebRoot);
                        }
                        filename = pathWebRoot + $"{filenameNew}";
                        size += imgFile.Length;
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            imgFile.CopyTo(fs);
                            fs.Flush();
                        }

                        //保存资源
                        decimal res_id = 0;
                        if (mst_id > 0 && resource_id == 0)
                        {
                            res_id = _repository.Get_Resource_SEQID();
                            if (res_id > 0)
                            {
                                var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                {
                                    ID = res_id,
                                    MST_ID = mst_id,
                                    ORDER_NO = res_id,
                                    RESOURCE_TYPE = 0,
                                    RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                    RESOURCE_URL_THUMB = "",
                                    RESOURCE_NAME = resource_name,
                                    RESOURCE_SIZE = filesize,
                                    RESOURCES_CATEGORY = Convert.ToDecimal(category),
                                };
                                _repository.InsertResource(res_entity);
                            }
                        }
                        //更新产品图 
                        else if (resource_id > 0 && category == "0")
                        {
                            res_id = resource_id;
                            var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                            {
                                ID = resource_id,
                                MST_ID = mst_id,
                                ORDER_NO = resource_id,
                                RESOURCE_TYPE = 0,
                                RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                RESOURCE_URL_THUMB = "",
                                RESOURCE_NAME = resource_name,
                                RESOURCE_SIZE = filesize,
                                RESOURCES_CATEGORY = Convert.ToDecimal(category),
                            };
                            _repository.UpdateResourceByID(res_entity);
                        }

                        UploadImageResult imgResult = new UploadImageResult
                        {
                            resource_url = path + $"{filenameNew}",
                            mst_id = mst_id,
                            resource_id = res_id
                        };
                        returnVM.Result = imgResult;
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
        /// 获取文件系统信息
        /// </summary>
        /// <param name="description">规格</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<DocumentListViewModel>>> GetDocumentSystemData([FromQuery] string description)
        {
            ApiBaseReturn<List<DocumentListViewModel>> returnVM = new ApiBaseReturn<List<DocumentListViewModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetDocmentSystemData(specs: description);
                    if (resdata.status == 0 && resdata.data.list.Count() > 0)
                    {
                        returnVM.Result = resdata.data.list;
                        returnVM.TotalCount = resdata.data.list.Count;
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
        /// 文控系统
        /// </summary>
        /// <param name="mst_id">关联ID</param>
        /// <param name="category">资源类别(0：产品图，1：作业图，2：零件图)</param>
        /// <param name="resource_id">原有资料id(产品类型使用)</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<UploadImageResult>>> UploadDocumentSystem([FromBody] List<DocumentSystenRequest> modelList)
        {
            ApiBaseReturn<List<UploadImageResult>> returnVM = new ApiBaseReturn<List<UploadImageResult>>();
            List<UploadImageResult> imageResultList = new List<UploadImageResult>();
            var resource_name = string.Empty;
            long size = 0;
            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存文件并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        foreach (var model in modelList)
                        {
                            //文控根路径
                            var rootPath = (await _repository.GetListByTableEX<SfcsParameters>("MEANING,DESCRIPTION", "SFCS_PARAMETERS", " AND LOOKUP_TYPE='MES_UT_DOCUMENT' AND ENABLED='Y' AND MEANING='MEANING'"))?.FirstOrDefault();
                            var fullOrign = Path.Combine(rootPath == null ? "" : rootPath.DESCRIPTION.Trim(), model.spath, model.filname);
                            var filenameNew = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                            var path = $"/upload/sopfile/"; //+ DateTime.Now.ToString("yyyyMM");
                            string dir = @"upload\sopfile\";
                            var pathWebRoot = AppContext.BaseDirectory + dir;
                            if (!Directory.Exists(pathWebRoot))
                            {
                                Directory.CreateDirectory(pathWebRoot);
                            }
                            filename = pathWebRoot + $"{filenameNew}";

                            using (FileStream fs = System.IO.File.OpenRead(fullOrign))
                            {
                                using (FileStream cfs = System.IO.File.Create(filename))
                                {
                                    fs.CopyToAsync(cfs);
                                }
                            }

                            //保存资源
                            decimal res_id = 0;
                            if (model.mst_id > 0 && model.resource_id == 0)
                            {
                                res_id = _repository.Get_Resource_SEQID();
                                if (res_id > 0)
                                {
                                    var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                    {
                                        ID = res_id,
                                        MST_ID = model.mst_id,
                                        ORDER_NO = res_id,
                                        RESOURCE_TYPE = 0,
                                        RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                        RESOURCE_URL_THUMB = "",
                                        RESOURCE_NAME = resource_name,
                                        RESOURCE_SIZE = filesize,
                                        RESOURCES_CATEGORY = Convert.ToDecimal(model.category),
                                    };
                                    _repository.InsertResource(res_entity);
                                }
                            }
                            //更新产品图 
                            else if (model.resource_id > 0 && model.category == "0")
                            {
                                res_id = model.resource_id;
                                var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                {
                                    ID = model.resource_id,
                                    MST_ID = model.mst_id,
                                    ORDER_NO = model.resource_id,
                                    RESOURCE_TYPE = 0,
                                    RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                    RESOURCE_URL_THUMB = "",
                                    RESOURCE_NAME = resource_name,
                                    RESOURCE_SIZE = filesize,
                                    RESOURCES_CATEGORY = Convert.ToDecimal(model.category),
                                };
                                _repository.UpdateResourceByID(res_entity);
                            }

                            UploadImageResult imgResult = new UploadImageResult
                            {
                                resource_url = path + $"{filenameNew}",
                                mst_id = model.mst_id,
                                resource_id = res_id
                            };
                            imageResultList.Add(imgResult);
                        }

                        returnVM.Result = imageResultList;
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
        /// 上传资料转换PDA
        /// </summary>
        /// <param name="mst_id">关联ID</param>
        /// <param name="category">资源类别(0：产品图，1：作业图，2：零件图)</param>
        /// <param name="resource_id">原有资料id(产品类型使用)</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<UploadImageResult>> UploadFileToPDF([FromForm] decimal mst_id, [FromForm] string category, [FromForm] decimal resource_id)
        {
            ApiBaseReturn<UploadImageResult> returnVM = new ApiBaseReturn<UploadImageResult>();

            var imgFile = Request.Form.Files[0];
            var resource_name = string.Empty;
            long size = 0;
            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (imgFile == null || imgFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(imgFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        resource_name = filename;

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(imgFile.Length / 1024.00, 3));
                        long mb = imgFile.Length / 1024 / 1024; // MB
                        if (mb > 50)
                        {
                            //return Json(new { code = 1, msg = "只允许上传小于 5MB 的图片.", });
                            ErrorInfo.Set(_localizer["upload_size_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        #endregion
                    }

                    #endregion

                    #region 保存文件并设置返回值
                    List<SOP_OPERATIONS_ROUTES_RESOURCE> sopList = new List<SOP_OPERATIONS_ROUTES_RESOURCE>();
                    if (!ErrorInfo.Status)
                    {
                        var filenameNew = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var path = $"/upload/sopfile/"; //+ DateTime.Now.ToString("yyyyMM");
                        string dir = @"upload\sopfile\";
                        var pathWebRoot = AppContext.BaseDirectory + dir;
                        if (!Directory.Exists(pathWebRoot))
                        {
                            Directory.CreateDirectory(pathWebRoot);
                        }
                        filename = pathWebRoot + $"{filenameNew}";
                        size += imgFile.Length;
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            imgFile.CopyTo(fs);
                            fs.Flush();
                        }

                        //转pdf
                        decimal res_id = 0;
                        var isPDFType = false;

                        if (mst_id > 0 && resource_id == 0)
                        {
                            res_id = _repository.Get_Resource_SEQID();
                        }
                        else if (resource_id > 0 && category == "0")
                        {
                            res_id = resource_id;
                        }
                        var extensionName = Path.GetExtension(resource_name);
                        var pdfName = "";
                        var destinationPDFPath = "";

                        if (extensionName.ToUpper().Contains("PPT") || extensionName.ToUpper().Contains("PPTX")
                            || extensionName.ToUpper().Contains("DOC") || extensionName.ToUpper().Contains("DOCX")
                            || extensionName.ToUpper().Contains("XLS") || extensionName.ToUpper().Contains("XLSX") || extensionName.ToUpper().Contains("CSV"))
                        {

                            pdfName = res_id + ".pdf";
                            var pdfPath = System.IO.Path.Combine(pathWebRoot, $"{pdfName}");
                            var from = System.IO.Path.Combine(pathWebRoot, $"{filenameNew}");

                            if (mst_id > 0 && resource_id == 0)
                            {
                                if (res_id > 0)
                                {
                                    if (extensionName.ToUpper().Contains("XLS") || extensionName.ToUpper().Contains("XLSX") || extensionName.ToUpper().Contains("CSV"))
                                    {
                                        #region excel逻辑
                                        Workbook workbook = new Workbook();
                                        workbook.LoadFromFile(from);
                                        foreach (Worksheet item in workbook.Worksheets)
                                        {
                                            decimal id = _repository.Get_Resource_SEQID();
                                            pdfName = id + ".pdf";
                                            pdfPath = System.IO.Path.Combine(pathWebRoot, $"{pdfName}");
                                            var result = OfficeToPDF.ExcelToPdf(item, pdfPath);
                                            destinationPDFPath = result ? pdfPath : "";
                                            isPDFType = result;
                                            var pdfURL = isPDFType ? System.IO.Path.Combine(path, $"{pdfName}") : "";
                                            //保存资源
                                            var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                            {
                                                ID = id,
                                                MST_ID = mst_id,
                                                ORDER_NO = id,
                                                RESOURCE_TYPE = 0,
                                                RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                                RESOUTCE_PDF_URL = pdfURL,
                                                RESOURCE_URL_THUMB = "",
                                                RESOURCE_NAME = resource_name,
                                                RESOURCE_SIZE = filesize,
                                                RESOURCES_CATEGORY = Convert.ToDecimal(category),
                                            };
                                            sopList.Add(res_entity);
                                        }
                                        foreach (var item in sopList)
                                        {
                                            _repository.InsertResource(item);
                                        }
                                        #endregion
                                    } else if (extensionName.ToUpper().Contains("PPT") || extensionName.ToUpper().Contains("PPTX")|| extensionName.ToUpper().Contains("DOC") || extensionName.ToUpper().Contains("DOCX")) 
                                    {
                                        decimal id = _repository.Get_Resource_SEQID();
                                        pdfName = id + ".pdf";
                                        pdfPath = System.IO.Path.Combine(pathWebRoot, $"{pdfName}");
                                        var result = OfficeToPDF.ProcessOfficeToPDF(filename, pdfPath, extensionName.ToUpper());
                                        destinationPDFPath = result ? pdfPath : "";
                                        isPDFType = result;
                                        var pdfURL = isPDFType ? System.IO.Path.Combine(path, $"{pdfName}") : "";
                                        //保存资源
                                        var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                        {
                                            ID = id,
                                            MST_ID = mst_id,
                                            ORDER_NO = id,
                                            RESOURCE_TYPE = 0,
                                            RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                            RESOUTCE_PDF_URL = pdfURL,
                                            RESOURCE_URL_THUMB = "",
                                            RESOURCE_NAME = resource_name,
                                            RESOURCE_SIZE = filesize,
                                            RESOURCES_CATEGORY = Convert.ToDecimal(category),
                                        };
                                        sopList.Add(res_entity);
                                    }
                                    foreach (var item in sopList)
                                    {
                                        _repository.InsertResource(item);
                                    }
                                }
                            }
                            //更新产品图 
                            if (resource_id > 0 && category == "0")
                            {
                                var result = OfficeToPDF.ProcessOfficeToPDF(from, pdfPath, extensionName);
                                destinationPDFPath = result ? pdfPath : "";
                                isPDFType = result;
                                var pdfURL = isPDFType ? System.IO.Path.Combine(path, $"{pdfName}") : "";

                                var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                {
                                    ID = resource_id,
                                    MST_ID = mst_id,
                                    ORDER_NO = resource_id,
                                    RESOURCE_TYPE = 0,
                                    RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                    RESOUTCE_PDF_URL = pdfURL,
                                    RESOURCE_URL_THUMB = "",
                                    RESOURCE_NAME = resource_name,
                                    RESOURCE_SIZE = filesize,
                                    RESOURCES_CATEGORY = Convert.ToDecimal(category),
                                };
                                _repository.UpdateResourceByID(res_entity);
                            }

                        }
                        else
                        {
                            //保存资源
                            if (mst_id > 0 && resource_id == 0)
                            {
                                res_id = _repository.Get_Resource_SEQID();
                                if (res_id > 0)
                                {
                                    var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                    {
                                        ID = res_id,
                                        MST_ID = mst_id,
                                        ORDER_NO = res_id,
                                        RESOURCE_TYPE = 0,
                                        RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                        RESOURCE_URL_THUMB = "",
                                        RESOURCE_NAME = resource_name,
                                        RESOURCE_SIZE = filesize,
                                        RESOURCES_CATEGORY = Convert.ToDecimal(category),
                                    };
                                    _repository.InsertResource(res_entity);
                                }
                            }
                            //更新产品图 
                            else if (resource_id > 0 && category == "0")
                            {
                                res_id = resource_id;
                                var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                {
                                    ID = resource_id,
                                    MST_ID = mst_id,
                                    ORDER_NO = resource_id,
                                    RESOURCE_TYPE = 0,
                                    RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                    RESOURCE_URL_THUMB = "",
                                    RESOURCE_NAME = resource_name,
                                    RESOURCE_SIZE = filesize,
                                    RESOURCES_CATEGORY = Convert.ToDecimal(category),
                                };
                                _repository.UpdateResourceByID(res_entity);
                            }
                        }
                        var resourceUrl = path + (isPDFType ? $"{pdfName}" : $"{filenameNew}");
                        UploadImageResult imgResult = new UploadImageResult
                        {
                            resource_url = resourceUrl,
                            mst_id = mst_id,
                            resource_id = res_id
                        };
                        returnVM.Result = imgResult;
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
        /// 保存图片说明  
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> UpdateMsgInfo([FromBody] UpdateMsgInfoModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (model.Resource == null)
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonFailNoDataCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonFailNoDataMsg;

                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && (model.PartInfo != null && model.PartInfo.PART_NO != null))
                    {
                        //partInfo.CREATEUSER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
                        model.PartInfo.CREATEDATE = DateTime.Now;
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.UnpdateResourceMsg(model.Resource, model.PartInfo);
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
        /// SOP复制 执行方法
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SOPCopySave([FromBody] SOPCopyRequestModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _service.SOPCopyAsync(item);
                        if (result.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else
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

        /// <summary>
        /// 根据零件料号获取零件信息
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetPartByPartNo(string partNo)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetPartByPartNo(partNo);
                    returnVM.Result = JsonHelper.ObjectToJSON(data);

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
        /// 下载图片到服务器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="docname"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        private SOP_OPERATIONS_ROUTES_RESOURCE DownloadFile(string url, string docname)
        {
            Stream stream = null;
            SOP_OPERATIONS_ROUTES_RESOURCE res_entity = null;
            try
            {
                string fileurl = $@"/upload/sopfile/{docname}";
                string filename = _hostingEnv.WebRootPath + fileurl;
                HttpWebRequest ReqData = (HttpWebRequest)WebRequest.Create(url);
                ReqData.Method = "GET";
                HttpWebResponse HWR = ReqData.GetResponse() as HttpWebResponse;
                var len = HWR.ContentLength;
                stream = HWR.GetResponseStream();

                decimal filesize = Convert.ToDecimal(Math.Round(len / 1024.00, 3));
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                img.Save(filename);

                res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE();
                res_entity.RESOURCE_TYPE = 0;
                res_entity.RESOURCE_URL = fileurl;
                res_entity.RESOURCE_NAME = docname;
                res_entity.RESOURCE_SIZE = filesize;

                return res_entity;
            }
            catch (Exception ex)
            {
                return res_entity;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        #region 测试类
        /// <summary>
        /// 转pdf测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> UpdateTest()
        {
            bool result = false;
            try
            {

                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        #endregion

        #region 内部方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">单据ID</param>
        /// <returns></returns>
        private bool BillIsChecked(decimal id)
        {
            return _repository.GetDisplayStatusById(id);
        }

        /// <summary>
        /// 明细返回类
        /// </summary>
        public class DetailResult
        {
            /// <summary>
            /// 主键
            /// </summary>
            public decimal ID { get; set; }

            /// <summary>
            /// 料号
            /// </summary>
            public string PART_NO { get; set; }

            /// <summary>
            /// 制程名称
            /// </summary>
            public string ROUTE_NAME { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string DESCRIPTION { get; set; }

            /// <summary>
            /// 状态(0:待审核;1:已审核;)
            /// </summary>
            public decimal STATUS { get; set; }

            /// <summary>
            /// 主表资料图片ID
            /// </summary>
            public decimal m_ResourceID { get; set; }

            /// <summary>
            /// 主表资料图片URL
            /// </summary>
            public string m_RESOURCE_URL { get; set; }
        }

        /// <summary>
        /// 更新资源信息模型
        /// </summary>
        public class UpdateMsgInfoModel
        {
            /// <summary>
            /// 图片资源
            /// </summary>
            public SOP_OPERATIONS_ROUTES_RESOURCE Resource { get; set; }

            /// <summary>
            ///  零件信息
            /// </summary>
            public SopOperationsRoutesPartAddOrModifyModel PartInfo { get; set; } = null;
        }

        /// <summary>
        /// 图片上传前保存工序的模型
        /// </summary>
        public class AddOperOfUploadImageModel
        {
            /// <summary>
            /// 制程(工艺线路主表主键)id
            /// </summary>
            public string routid { get; set; }

            /// <summary>
            /// 工序表主键ID
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// 当前工序id
            /// </summary>
            public string operid { get; set; }
        }

        /// <summary>
        /// 图片上传模型
        /// </summary>
        public class UploadImageModel
        {
            /// <summary>
            /// 关联ID
            /// </summary>
            public decimal mst_id { get; set; }

            /// <summary>
            /// 资源类别(0：产品图，1：作业图，2：零件图)
            /// </summary>
            public string category { get; set; }

            /// <summary>
            /// 原有资料id(产品类型使用)
            /// </summary>
            public decimal resource_id { get; set; }
        }

        /// <summary>
        /// 图片上传返回值
        /// </summary>
        public class UploadImageResult
        {
            /// <summary>
            /// 图片资料地址
            /// </summary>
            public string resource_url { get; set; }

            /// <summary>
            /// 关联id
            /// </summary>
            public decimal mst_id { get; set; }

            /// <summary>
            /// 图片资料id
            /// </summary>
            public decimal resource_id { get; set; }
        }



        #endregion
    }
}