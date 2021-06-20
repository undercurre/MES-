/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-12 14:21:57                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtStencilOperationHistoryController                                      
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

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 钢网领用/钢网归还控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtStencilTakeController : BaseController
    {
        private readonly ISmtStencilStoreRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtStencilTakeController> _localizer;

        public SmtStencilTakeController(ISmtStencilStoreRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtStencilTakeController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 钢网领用首页视图
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
        /// 钢网归还首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> ReturnIndex()
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

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 钢网领用之获取钢网编号信息
        /// </summary>
        /// <param name="stencil_no">钢网编号</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SmtStencilTakeModel>> LoadData([FromQuery]string stencil_no)
        {
            ApiBaseReturn<SmtStencilTakeModel> returnVM = new ApiBaseReturn<SmtStencilTakeModel>();
            returnVM.Result = new SmtStencilTakeModel();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && stencil_no.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入钢网编号！");
                        ErrorInfo.Set(_localizer["stencil_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetStencil2Enabled(stencil_no);
                        if (resdata == null)
                        {
                            //string.Format("{0}钢网在系未注册或未激活使用", stencilNo)
                            ErrorInfo.Set(_localizer["stencil_config_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result.TENSION_CONTROL_FLAG = resdata.TENSION_CONTROL_FLAG;
                            returnVM.Result.TENSION_CONTROL_VALUE = resdata.TENSION_CONTROL_VALUE;
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetStencilRuntime(stencil_no);
                        if (resdata != null)
                        {
                            returnVM.Result.PrintCount = resdata.PRINT_COUNT ?? 0;
                        }

                        var tmpdata = await _repository.GetStencilCleanHistory(stencil_no);
                        if (tmpdata != null)
                        {
                            returnVM.Result.TENSION_A = tmpdata.TENSION_A ?? 0;
                            returnVM.Result.TENSION_B = tmpdata.TENSION_B ?? 0;
                            returnVM.Result.TENSION_C = tmpdata.TENSION_C ?? 0;
                            returnVM.Result.TENSION_D = tmpdata.TENSION_D ?? 0;
                            returnVM.Result.TENSION_E = tmpdata.TENSION_E ?? 0;
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
        /// 钢网领用之保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<BoolResult>> SaveData([FromBody] SmtStencilTakeSaveModel model)
        {
            ApiBaseReturn<BoolResult> returnVM = new ApiBaseReturn<BoolResult>();
            returnVM.Result = new BoolResult();

            CheckScrapResult result = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.STENCIL_NO.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入钢网编号！");
                        ErrorInfo.Set(_localizer["stencil_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.WorkNo.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入领用者工号！");
                        ErrorInfo.Set(_localizer["WorkNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.InspectResult != "OK")
                    {
                        //throw new Exception("当前清洗结果为异常，不能领用！");
                        ErrorInfo.Set(_localizer["InspectResult_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && (model.TENSION_A <= 0 || model.TENSION_B <= 0 || model.TENSION_C <= 0 || model.TENSION_D <= 0
                        || model.TENSION_E <= 0))
                    {
                        //throw new Exception("张力点异常，不能领用！");
                        ErrorInfo.Set(_localizer["tension_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        result = await CheckStencilTakeInput(model.STENCIL_NO);
                        if (result.Error)
                        {
                            ErrorInfo.Set(result.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            DateTime nowTime = DateTime.Now;
                            if (result.UPDATE_TIME == null)
                            {
                                if (result.CREATE_TIME.Value.AddDays(180) <= nowTime)
                                {
                                    // ("钢网存储超过180天，请注意要清洗！");
                                    returnVM.Result.PromptMessage = _localizer["out180_err"];
                                }
                            }
                            else if (result.UPDATE_TIME.Value.AddDays(180) <= nowTime)
                            {
                                returnVM.Result.PromptMessage = _localizer["out180_err"];
                            }
                        }
                    }


                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.StencilTakeSave(model, result);
                        if (resdata != -1)
                        {
                            returnVM.Result.Result = true;
                        }
                        else
                        {
                            returnVM.Result.Result = false;
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
        /// 钢网归还之获取钢网储位
        /// </summary>
        /// <param name="stencil_no">钢网编号</param>
        /// <returns>返回钢网储位</returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> ReturnLoadData([FromQuery]string stencil_no)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && stencil_no.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入钢网编号！");
                        ErrorInfo.Set(_localizer["stencil_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetStencilLocation(stencil_no);
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
        /// 钢网归还之保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<BoolResult>> ReturnSaveData([FromBody] SmtStencilReturnSaveModel model)
        {
            ApiBaseReturn<BoolResult> returnVM = new ApiBaseReturn<BoolResult>();
            returnVM.Result = new BoolResult();

            CheckScrapResult result = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.STENCIL_NO.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入钢网编号！");
                        ErrorInfo.Set(_localizer["stencil_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.LOCATION.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入归还储位！");
                        ErrorInfo.Set(_localizer["location_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.WorkNo.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入归还者！");
                        ErrorInfo.Set(_localizer["Return_WorkNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        result = await CheckReturnStencilInput(model.STENCIL_NO);
                        if (result.Error)
                        {
                            ErrorInfo.Set(result.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.StencilReturnSave(model, result);
                        if (resdata != -1)
                        {
                            returnVM.Result.Result = true;
                        }
                        else
                        {
                            returnVM.Result.Result = false;
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
        /// 
        /// </summary>
        /// <param name="stencil_no"></param>
        /// <returns></returns>
        private async Task<CheckScrapResult> CheckStencilTakeInput(string stencil_no)
        {
            CheckScrapResult result = new CheckScrapResult();

            var stencil = await _repository.GetStencil(stencil_no);
            if (!result.Error && stencil == null)
            {
                //throw new Exception(string.Format("{0}钢网没有注册，请检查!", stencilNo));
                result.Set(string.Format(_localizer["stencil_check_error"], stencil_no));
            }
            else
            {
                result.STENCIL_ID = stencil.ID;
                result.ID = stencil.ID;
            }

            if (!result.Error)
            {
                var resdata = await _repository.GetStencilStoreBySTENCIL_ID(result.ID);
                if (resdata == null)
                {
                    //this.ShowErrorMessage(string.Format("{0}钢网尚未入柜!", stencilNo));
                    result.Set(string.Format(_localizer["stencil_nostore_error"], stencil_no));
                }
                else
                {
                    result.LOCATION = resdata.LOCATION;
                    result.CREATE_TIME = resdata.CREATE_TIME;
                    resdata.UPDATE_TIME = resdata.UPDATE_TIME;
                    if (resdata.STATUS != GlobalVariables.StencilStored)
                    {
                        //this.ShowErrorMessage(string.Format("{0}钢网不是储存状态，不能执行领用!", stencilNo));
                        result.Set(string.Format(_localizer["stencil_take_error"], stencil_no));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stencil_no"></param>
        /// <returns></returns>
        private async Task<CheckScrapResult> CheckReturnStencilInput(string stencil_no)
        {
            CheckScrapResult result = new CheckScrapResult();

            var stencil = await _repository.GetStencil(stencil_no);
            if (!result.Error && stencil == null)
            {
                //throw new Exception(string.Format("{0}钢网没有注册，请检查!", stencilNo));
                result.Set(string.Format(_localizer["stencil_check_error"], stencil_no));
            }
            else
            {
                result.STENCIL_ID = stencil.ID;
                result.ID = stencil.ID;
            }

            if (!result.Error)
            {
                var resdata = await _repository.GetStencilStoreBySTENCIL_ID(result.ID);
                if (resdata == null)
                {
                    //this.ShowErrorMessage(string.Format("{0}钢网尚未入柜!", stencilNo));
                    result.Set(string.Format(_localizer["stencil_nostore_error"], stencil_no));
                }
                else
                {
                    if (!result.Error && resdata.STATUS == GlobalVariables.StencilStored)
                    {
                        //this.ShowErrorMessage("钢网已经归还，不需要再作归还动作!");
                        result.Set(string.Format(_localizer["stencil_returned_error"], stencil_no));
                    }
                    if (!result.Error && resdata.STATUS == GlobalVariables.StencilOnline)
                    {
                        //this.ShowErrorMessage(string.Format("{0}钢网正在线上使用状态，不能做归还!", stencilNo));
                        result.Set(string.Format(_localizer["stencil_online_error"], stencil_no));
                    }
                    if (!result.Error)
                    {
                        var tmpdata = await _repository.StencilIsCleanedBefore(resdata.STENCIL_ID ?? 0);
                        if (!tmpdata)
                        {
                            //throw new Exception("钢网必须先清洗才可以归还！");
                            result.Set(string.Format(_localizer["stencil_clear_error"], stencil_no));
                        }
                    }
                }
            }
            return result;
        }
    }
}