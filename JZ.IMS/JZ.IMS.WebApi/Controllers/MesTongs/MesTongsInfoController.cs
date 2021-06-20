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
using Microsoft.AspNetCore.Hosting;
using JZ.IMS.Models;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.ViewModels.MesTongs;
using Microsoft.Extensions.Localization;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 夹具信息管理 控制器  
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesTongsInfoController : BaseController
    {
        private readonly IMesTongsInfoRepository _repository;
        private readonly ISfcsParametersService _serviceParameters;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MesTongsInfoController> _localizer;

        public MesTongsInfoController(IMesTongsInfoRepository repository, ISfcsParametersService serviceParameters, IHttpContextAccessor httpContextAccessor, IStringLocalizer<MesTongsInfoController> localizer)
        {
            _repository = repository;
            _serviceParameters = serviceParameters;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 夹具信息管理 首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<List<SfcsDepartment>> Index()
        {
            ApiBaseReturn<List<SfcsDepartment>> returnVM = new ApiBaseReturn<List<SfcsDepartment>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _serviceParameters.GetDepartmentList();
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
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery] MesTongsInfoRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetTongsData(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                    returnVM.TotalCount = await _repository.GetTongsDataCount(model);

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
		/// 导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<IEnumerable<dynamic>>> ExportData([FromQuery] MesTongsInfoRequestModel model)
        {
            ApiBaseReturn<IEnumerable<dynamic>> returnVM = new ApiBaseReturn<IEnumerable<dynamic>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值
                    var result = await _repository.GetExportData(model);
                    returnVM.Result = result;
                    returnVM.TotalCount = await _repository.GetTongsDataCount(model);
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
        /// 查询工装与工位信息
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<QuertyTongsSiteViewModel>>> GetTongsSiteByCodeAsync([FromQuery] MesTongsSiteRequestModel model)
        {
            ApiBaseReturn<List<QuertyTongsSiteViewModel>> returnVM = new ApiBaseReturn<List<QuertyTongsSiteViewModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetTongsSiteByCodeAsync(model);
                    returnVM.Result = resdata.data;
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
        /// 新增/修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<List<SfcsDepartment>> AddOrModify(decimal id)
        {
            ApiBaseReturn<List<SfcsDepartment>> returnVM = new ApiBaseReturn<List<SfcsDepartment>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _serviceParameters.GetDepartmentList();
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
        /// 判断夹具编码是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<bool> IsExistCode(string code)
        {
            return await _repository.IsExistCode(code);
        }

        public class TempListModel
        {
            /// <summary>
            /// 实体集合
            /// </summary>
            public List<MesTongsInfoListModel> List { get; set; }
            /// <summary>
            /// 用户
            /// </summary>
            public string LoginName { get; set; } = null;
            /// <summary>
            /// 组织ID
            /// </summary>
            public string ORGANIZE_ID { get; set; } = null;

        }

        /// <summary>
        /// 申请入库（批量新增）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> ApplyGoStore([FromBody] TempListModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var user = model.LoginName ?? string.Empty;
                        var organizeId = model.ORGANIZE_ID ?? string.Empty;
                        var result = await _repository.ApplyGoStore(model.List, user, organizeId);
                        returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// 申请入库
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="LoginName">用户</param>
        /// <param name="ORGANIZE_ID">组织ID</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> ApplyGoStoreByModel([FromBody] MesTongsInfoListModel model, string LoginName, string ORGANIZE_ID)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ApplyGoStoreByModel(model, LoginName, ORGANIZE_ID);
                        returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// 保存数据
        /// 产品系列说明{*PRODUCT_FAMILY_ID(产品系列传),*ID，*ENABLED(是否激活)}
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody] MesTongsInfoListModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        //if (model.ID == 0)
                        //	model.CREATE_USER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
                        //else
                        //	model.UPDATE_USER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;

                        var resultData = await _repository.AddOrModify(model);
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
        /// 根据ID获取夹具信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetTongsById(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetTongsById(id);
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
        /// 根据夹具ID获取夹具操作记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetTongsOperationData(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetTongsOperationData(id);
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
        /// 根据夹具ID获取保养/激活记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetMaintainData(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetMaintainData(id);
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
        /// 根据夹具ID获取维修记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetRepairData(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetRepairData(id);
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
        /// 根据保养主表ID获取保养明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetMaintainDetailData(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetMaintainDetailData(id);
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
        /// 获取夹具已经是维修状态但还未做维修动作的维修数据
        /// </summary>
        /// <param name="tongsId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetRepairByTongsId(decimal tongsId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetRepairByTongsId(tongsId);
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
        /// 根据料号获取产品信息
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

                    var resdata = await _repository.GetPartByPartNo(partNo);
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
        /// 夹具入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> EnterStore([FromBody] TongsItemModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.EnterStore(model.TongsID, model.StoreID, model.UserName);
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
        /// 变更储位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeStore([FromBody] TongsItemModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 变更并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.ChangeStore(model.TongsID, model.StoreID, model.UserName);
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
        /// 获取激活事项列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetActiveItemsData(int tongsType)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetMaintainItemsData(1, tongsType);
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
        /// 获取保养事项列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetMaintainItemsData(int tongsType)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetMaintainItemsData(0, tongsType);
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
        /// 开始激活
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<string>> BeginActive([FromBody] TongsSetItemModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.BeginActive(model.TongsID, model.UserName);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = resultData.ResultData;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = resultData.ResultData;
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
        /// 结束激活
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> EndActive([FromBody] MesTongsMaintainHistory model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置并返回

                    if (!ErrorInfo.Status)
                    {
                        //model.MAINTAIN_USER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
                        var resultData = await _repository.EndActive(model);
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
        /// 领用夹具
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> BorrowTongs([FromBody] TongsSetItemModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.BorrowTongs(model.TongsID, model.UserName);
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
        /// 永久借出夹具
        /// </summary>
        /// <param name="tongsId">夹具ID</param>
        /// <param name="LoginName">用户名</param>
        /// <param name="remark">备注</param>
        /// <returns>True 为成功，False 为其他为异常</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> PermanentLendTongs(decimal tongsId, string remark, string LoginName = null)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置并返回

                    if (!ErrorInfo.Status)
                    {
                        string user = LoginName ?? string.Empty;
                        var resultData = await _repository.PermanentLendTongs(tongsId, user, remark);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = "True";
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = "False";
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
        /// 开始保养
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<string>> BeginMaintain([FromBody] TongsSetItemModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.BeginMaintain(model.TongsID, model.UserName);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = resultData.ResultData;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = string.Empty;
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
        /// 结束保养
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> EndMaintain([FromBody] MesTongsMaintainHistory model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.EndMaintain(model);
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
        /// 维修夹具
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> RepairTongs([FromBody] MesTongsRepair model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置并返回

                    if (!ErrorInfo.Status)
                    {
                        //model.REPAIRER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
                        var resultData = await _repository.RepairTongs(model);
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
        /// 根据工单号或产品编号获取夹具信息
        /// </summary>
        /// <param name="WoNo">工单号</param>
        /// <param name="PartNo">产品编号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<TongsStoreInfoModel>>> GetTongsInfoByWoNo(string WoNo, string PartNo)
        {
            ApiBaseReturn<List<TongsStoreInfoModel>> returnVM = new ApiBaseReturn<List<TongsStoreInfoModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetTongsInfoByWoNo(WoNo, PartNo);

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

        #region 工装盘点
        /// <summary>
        /// 保存PDA工装盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<TongsInfoInnerKeepDetail>>> SavePDATongsCheckData([FromBody] SaveTongsCheckDataRequestModel model)
        {
            MesTongsInfo Tongs = null;
            MesTongsKeepHead head = null;
            MesTongsKeepDetail detail = null;
            List<TongsInfoInnerKeepDetail> fList = null;
            ApiBaseReturn<List<TongsInfoInnerKeepDetail>> returnVM = new ApiBaseReturn<List<TongsInfoInnerKeepDetail>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 获取所在组织架构
                    Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    String organize_id = "";
                    List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    if (idList != null && idList.Count() > 0)
                    {
                        organize_id = String.Join(",", idList);
                    }
                    else
                    {
                        throw new Exception("ORGANIZE_INFO_EMPTY");
                    }
                    #endregion

                    if (!model.CHECK_CODE.IsNullOrEmpty() && model.TONGS_BODYMARK.IsNullOrEmpty())
                    {
                        head = (await _repository.GetListByTableEX<MesTongsKeepHead>("*", "MES_TONGS_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        if (!head.IsNullOrWhiteSpace())
                        {
                            returnVM.Result = await _repository.GetPDATongsCheckDataByHeadID(organize_id, head.ID);
                        }
                        else
                        {
                            returnVM.Result = null;
                        }
                    }
                    else
                    {
                        if (model.TONGS_BODYMARK.IsNullOrEmpty() && model.TONGS_STORE <= 0 && !ErrorInfo.Status)
                        {
                            //工装编号和储位不能为空
                            ErrorInfo.Set(_localizer["TONGS_BODYMARK_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            Tongs = (await _repository.GetListByTableEX<MesTongsInfo>("*", "MES_TONGS_INFO", " AND CODE=:CODE AND STORE_ID=:STORE_ID AND ORGANIZE_ID in (" + organize_id + ")", new { CODE = model.TONGS_BODYMARK, STORE_ID = model.TONGS_STORE })).FirstOrDefault();
                            if (Tongs.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (Tongs.STATUS == 6)
                            {
                                //报废
                                ErrorInfo.Set(_localizer["TONGS_STATUS_6_NOT_CHECK"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (model.CHECK_CODE.IsNullOrEmpty())
                                {
                                    String check_code = _repository.QueryEx<String>("SELECT CHECK_CODE FROM MES_TONGS_KEEP_HEAD WHERE CHECK_STATUS != 2 AND ORGANIZE_ID in (" + organize_id + ") ORDER BY ID ASC").FirstOrDefault();
                                    if (!String.IsNullOrEmpty(check_code)) { model.CHECK_CODE = check_code; }
                                    //fList = await _repository.GetPDATongsCheckDataByHeadID( organize_id);
                                }
                            }
                        }
                        if (!model.CHECK_CODE.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            //检查编号是否存在
                            head = (await _repository.GetListByTableEX<MesTongsKeepHead>("*", "MES_TONGS_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                            if (head.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (head.CHECK_STATUS != 0)
                            {
                                ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (head.ID > 0)
                                {
                                    MesTongsKeepDetail content = (await _repository.GetListByTableEX<MesTongsKeepDetail>("*", "MES_TONGS_KEEP_DETAIL", " AND KEEP_HEAD_ID=:KEEP_HEAD_ID AND TONGS_ID=:TONGS_ID AND TONGS_STATUS=1", new { KEEP_HEAD_ID = head.ID, TONGS_ID = Tongs.ID })).FirstOrDefault();
                                    if (!content.IsNullOrWhiteSpace())
                                    {
                                        ErrorInfo.Set(_localizer["TONGS_CODE_INFO_REPEAT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                            }
                        }

                        int headId = 0;
                        if (!ErrorInfo.Status)
                        {
                            headId = await _repository.SavePDATongsCheckData(model, Tongs, head, detail);
                        }
                        if (head == null && !model.CHECK_CODE.IsNullOrEmpty())
                        {
                            head = (await _repository.GetListByTableEX<MesTongsKeepHead>("*", "MES_TONGS_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        }
                        if (head != null || headId > 1)
                        {
                            headId = headId > 1 ? headId : Convert.ToInt32(head.ID);
                            returnVM.Result = await _repository.GetPDATongsCheckDataByHeadID(organize_id, headId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 删除PDA工装盘点数据记录
        /// </summary>
        /// <param name="check_code">工装点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DeletePDATongsCheckData(String check_code)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    if (String.IsNullOrEmpty(check_code))
                    {
                        throw new Exception("TONGS_CODE_INFO_NULL");
                    }
                    else
                    {
                        //只能能删除新增和未审核状态下的盘点单
                        MesTongsKeepHead head = (await _repository.GetListByTableEX<MesTongsKeepHead>("*", "MES_TONGS_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = check_code })).FirstOrDefault();
                        if (head == null)
                        {
                            throw new Exception("TONGS_CODE_INFO_NULL");
                        }
                        else if (head.CHECK_STATUS == 0 || head.CHECK_STATUS == 1)
                        {
                            returnVM.Result = await _repository.DeletePDATongsCheckData(head.ID);
                        }
                        else
                        {
                            throw new Exception("CHECK_STATUS_NOT_INSERT");
                        }

                    }
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 确认PDA工装盘点数据
        /// </summary>
        /// <param name="check_code">工装点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> AuditTongsCheckData([FromBody] AuditTongsCheckDataRequestModel model)
        {
            MesTongsKeepHead head = null;
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (model.ID > 0 && !ErrorInfo.Status)
                    {
                        //检查编号是否存在
                        head = (await _repository.GetListByTableEX<MesTongsKeepHead>("*", "MES_TONGS_KEEP_HEAD", " AND ID=:ID", new { ID = model.ID })).FirstOrDefault();
                        if (head.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.STATUS == 1 && !ErrorInfo.Status)
                    {
                        //确认审核必须要新增
                        if (head.CHECK_STATUS != 0)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else if (model.STATUS == 2 && !ErrorInfo.Status)
                    {
                        //审核必须要未审核
                        if (head.CHECK_STATUS != 1)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_1"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_INSERT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.ConfirmPDATongsCheckData(model);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// PDA工装盘点列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<TongsCheckListModel>>> LoadPDATongsCheckList([FromQuery] TongsCheckRequestModel model)
        {

            ApiBaseReturn<List<TongsCheckListModel>> returnVM = new ApiBaseReturn<List<TongsCheckListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    String organize_id = "";
                    List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    if (idList != null && idList.Count() > 0)
                    {
                        organize_id = String.Join(",", idList);
                    }
                    else
                    {
                        throw new Exception("ORGANIZE_INFO_EMPTY");
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {

                        returnVM.Result = await _repository.LoadPDATongsCheckList(model, organize_id);
                        returnVM.TotalCount = await _repository.LoadPDATongsCheckListCount(model);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取需要点检的工装类型列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<TongsInfoInnerKeepDetail>>> LoadPDATongsCheckInfo([FromQuery] GetTongsInfoRequestModel model)
        {
            ApiBaseReturn<List<TongsInfoInnerKeepDetail>> returnVM = new ApiBaseReturn<List<TongsInfoInnerKeepDetail>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    String organize_id = "";
                    List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    if (idList != null && idList.Count() > 0)
                    {
                        organize_id = String.Join(",", idList);
                    }
                    else
                    {
                        throw new Exception("ORGANIZE_INFO_EMPTY");
                    }

                    returnVM.Result = await _repository.GetPDATongsCheckDataByHeadID(organize_id);

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;

        }

        #endregion

        #region 工装验证

        /// <summary>
        /// 保存PDA工装验证数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<TongsInfoInnerValidationDetail>>> SavePDATongsValidationData([FromBody] SaveTongsValidationDataRequestModel model)
        {
            MesTongsInfo Tongs = null;
            MesTongsValidationHead head = null;
            MesTongsValidationDetail detail = null;
            List<TongsInfoInnerValidationDetail> fList = null;
            ApiBaseReturn<List<TongsInfoInnerValidationDetail>> returnVM = new ApiBaseReturn<List<TongsInfoInnerValidationDetail>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 获取所在组织架构
                    Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    String organize_id = "";
                    List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    if (idList != null && idList.Count() > 0)
                    {
                        organize_id = String.Join(",", idList);
                    }
                    else
                    {
                        throw new Exception("ORGANIZE_INFO_EMPTY");
                    }
                    #endregion

                    if (!model.CHECK_CODE.IsNullOrEmpty() && model.TONGS_BODYMARK.IsNullOrEmpty())
                    {
                        head = (await _repository.GetListByTableEX<MesTongsValidationHead>("*", "MES_TONGS_VALIDATION_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        if (!head.IsNullOrWhiteSpace())
                        {
                            var dataModel = (await _repository.GetPDATongsValidationDataByHeadID(organize_id, pageInde: model.Page, pageSize: model.Limit, headid: head.ID));
                            returnVM.Result = dataModel?.data;
                            returnVM.TotalCount = dataModel?.count ?? 0;
                        }
                        else
                        {
                            returnVM.Result = null;
                        }
                    }
                    else
                    {
                        if (model.TONGS_BODYMARK.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            //工装编号不能为空
                            ErrorInfo.Set(_localizer["TONGS_BODYMARK_NOT_NULL_EX"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            Tongs = (await _repository.GetListByTableEX<MesTongsInfo>("*", "MES_TONGS_INFO", " AND CODE=:CODE AND ORGANIZE_ID in (" + organize_id + ")", new { CODE = model.TONGS_BODYMARK })).FirstOrDefault();
                            if (Tongs.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (Tongs.STATUS == 6)
                            {
                                //报废
                                ErrorInfo.Set(_localizer["TONGS_STATUS_6_NOT_CHECK"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (model.CHECK_CODE.IsNullOrEmpty())
                                {
                                    String check_code = _repository.QueryEx<String>("SELECT CHECK_CODE FROM MES_TONGS_VALIDATION_HEAD WHERE CHECK_STATUS != 2 AND ORGANIZE_ID in (" + organize_id + ") ORDER BY ID ASC").FirstOrDefault();
                                    if (!String.IsNullOrEmpty(check_code)) { model.CHECK_CODE = check_code; }
                                    //fList = await _repository.GetPDATongsCheckDataByHeadID( organize_id);
                                }
                            }
                        }
                        if (!model.CHECK_CODE.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            //检查编号是否存在
                            head = (await _repository.GetListByTableEX<MesTongsValidationHead>("*", "MES_TONGS_VALIDATION_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                            if (head.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (head.CHECK_STATUS != 0)
                            {
                                ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (head.ID > 0)
                                {
                                    MesTongsValidationDetail content = (await _repository.GetListByTableEX<MesTongsValidationDetail>("*", "MES_TONGS_VALIDATION_DETAIL", " AND VALIDATION_HEAD_ID=:VALIDATION_HEAD_ID AND TONGS_ID=:TONGS_ID AND TONGS_STATUS=1", new { VALIDATION_HEAD_ID = head.ID, TONGS_ID = Tongs.ID })).FirstOrDefault();
                                    if (!content.IsNullOrWhiteSpace())
                                    {
                                        ErrorInfo.Set(_localizer["TONGS_CODE_INFO_VALIDATION"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                            }
                        }

                        int headId = 0;
                        if (!ErrorInfo.Status)
                        {
                            headId = await _repository.SavePDATongsValidationData(model, Tongs, head, detail);
                        }
                        if (head == null && !model.CHECK_CODE.IsNullOrEmpty())
                        {
                            head = (await _repository.GetListByTableEX<MesTongsValidationHead>("*", "MES_TONGS_VALIDATION_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        }
                        if (head != null || headId > 1)
                        {
                            headId = headId > 1 ? headId : Convert.ToInt32(head.ID);
                            var dataModel = (await _repository.GetPDATongsValidationDataByHeadID(organize_id, pageInde: model.Page, pageSize: model.Limit, headid: headId));
                            returnVM.Result = dataModel?.data;
                            returnVM.TotalCount = dataModel?.count ?? 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// PDA工装验证列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<TongsCheckListModel>>> LoadPDATongsValidationList([FromQuery] TongsCheckRequestModel model)
        {

            ApiBaseReturn<List<TongsCheckListModel>> returnVM = new ApiBaseReturn<List<TongsCheckListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    String organize_id = "";
                    List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    if (idList != null && idList.Count() > 0)
                    {
                        organize_id = String.Join(",", idList);
                    }
                    else
                    {
                        throw new Exception("ORGANIZE_INFO_EMPTY");
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {

                        returnVM.Result = await _repository.LoadPDATongsValidationList(model, organize_id);
                        returnVM.TotalCount = await _repository.LoadPDATongsCheckListCount(model);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取需要验证的工装类型列表
        /// </summary>AuditTongsCheckData
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<TongsInfoInnerValidationDetail>>> LoadPDATongsValidationInfo([FromQuery] GetTongsInfoRequestModel model)
        {
            ApiBaseReturn<List<TongsInfoInnerValidationDetail>> returnVM = new ApiBaseReturn<List<TongsInfoInnerValidationDetail>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    String organize_id = "";
                    List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    if (idList != null && idList.Count() > 0)
                    {
                        organize_id = String.Join(",", idList);
                    }
                    else
                    {
                        throw new Exception("ORGANIZE_INFO_EMPTY");
                    }
                    var dataModel = await _repository.GetPDATongsValidationDataByHeadID(organize_id, pageInde: model.Page, pageSize: model.Limit);
                    returnVM.Result = dataModel?.data;
                    returnVM.TotalCount = dataModel?.count??0;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;

        }

        /// <summary>
        /// 确认PDA工装验证
        /// </summary>
        /// <param name="check_code">工装点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> AuditTongsValidationData([FromBody] AuditTongsCheckDataRequestModel model)
        {
            MesTongsValidationHead head = null;
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    //组织架构
                    Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.AUDIT_USER })).FirstOrDefault();
                    if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    String organize_id = "";
                    List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    if (idList != null && idList.Count() > 0)
                    {
                        organize_id = String.Join(",", idList);
                    }
                    else
                    {
                        throw new Exception("ORGANIZE_INFO_EMPTY");
                    }

                    if (model.ID > 0 && !ErrorInfo.Status)
                    {
                        //检查编号是否存在
                        head = (await _repository.GetListByTableEX<MesTongsValidationHead>("*", "MES_TONGS_VALIDATION_HEAD", " AND ID=:ID", new { ID = model.ID })).FirstOrDefault();
                        if (head.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.STATUS == 1 && !ErrorInfo.Status)
                    {

                        //确认审核必须要新增
                        if (head.CHECK_STATUS != 0)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //全部验证完了才可以提交
                            var tongsValidationList = (List<TongsInfoInnerValidationDetail>)(await _repository.GetPDATongsValidationDataByHeadID(organize_id,headid:model.ID))?.data;

                            if (tongsValidationList == null || tongsValidationList.Count <= 0)
                            {
                                ErrorInfo.Set(_localizer["TONGS_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (tongsValidationList.Count(c => "未验证".Equals(c.VDETAIL_STATUS)) > 0)
                                    ErrorInfo.Set(_localizer["TONGS_STATUS_NOT_COMMIT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }
                    else if (model.STATUS == 2 && !ErrorInfo.Status)
                    {
                        //审核必须要未审核
                        if (head.CHECK_STATUS != 1)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_1"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_INSERT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.ConfirmPDATongsValidationData(model);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 删除PDA工装验证数据记录
        /// </summary>
        /// <param name="check_code">工装点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DeletePDATongsValidationData(String check_code)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    if (String.IsNullOrEmpty(check_code))
                    {
                        throw new Exception("TONGS_CODE_INFO_NULL");
                    }
                    else
                    {
                        //只能能删除新增和未审核状态下的盘点单
                        MesTongsValidationHead head = (await _repository.GetListByTableEX<MesTongsValidationHead>("*", "MES_TONGS_VALIDATION_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = check_code })).FirstOrDefault();
                        if (head == null)
                        {
                            throw new Exception("TONGS_CODE_INFO_NULL");
                        }
                        else if (head.CHECK_STATUS == 0 || head.CHECK_STATUS == 1)
                        {
                            returnVM.Result = await _repository.DeletePDATongsValidationData(head.ID);
                        }
                        else
                        {
                            throw new Exception("CHECK_STATUS_NOT_INSERT");
                        }

                    }
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 查工装是否已经保养
        ///  true 为已经保养
        /// </summary>
        /// <param name="check_code">工装验证编号</param>
        /// <param name="hid">主表id</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> QueryPDATongsValidationBy(String check_code, decimal hid)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (String.IsNullOrEmpty(check_code))
                    {
                        throw new Exception("TONGS_CODE_INFO_NULL");
                    }
                    else
                    {
                        returnVM.Result = await _repository.QueryPDATongsValidationBy(check_code, hid);
                    }
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// PDA验证--保养保存
        /// TONGS_ID 工装ID--加载数据时对应字段：INFO_ID
        /// 返回保养记录ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> PDAMaintain([FromBody] MesTongsMaintainHistory model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.PDAMaintain(model);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = resultData.ResultData;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = "0";
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
        #endregion
    }
}