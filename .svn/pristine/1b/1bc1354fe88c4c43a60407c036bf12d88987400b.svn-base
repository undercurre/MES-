using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
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
    /// 钢网清洗控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtStencilCleanController : BaseController
    {
        private readonly ISmtStencilStoreRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtStencilCleanController> _localizer;

        public SmtStencilCleanController(ISmtStencilStoreRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtStencilCleanController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 钢网清洗首页视图
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
        /// 获取钢网编号相关信息
        /// </summary>
        /// <param name="stencil_no">钢网编号</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SmtStencilCleanModel>> GetStencilInfo([FromQuery]string stencil_no)
        {
            ApiBaseReturn<SmtStencilCleanModel> returnVM = new ApiBaseReturn<SmtStencilCleanModel>();
            returnVM.Result = new SmtStencilCleanModel();

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
                            returnVM.Result.LAST_CLEAN_TIME = resdata.LAST_CLEAN_TIME;
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
                            returnVM.Result.UnUsed = false;
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
        /// 获取钢网清洗历史信息列表
        /// </summary>
        /// <param name="stencil_no">钢网编号</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtStencilCleanHistory>>> LoadData([FromQuery]string stencil_no)
        {
            ApiBaseReturn<List<SmtStencilCleanHistory>> returnVM = new ApiBaseReturn<List<SmtStencilCleanHistory>>();

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
                        returnVM.Result = await _repository.GetStencilCleanHistoryList(stencil_no);
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<BoolResult>> SaveData([FromBody] SmtStencilCleanSaveModel model)
        {
            ApiBaseReturn<BoolResult> returnVM = new ApiBaseReturn<BoolResult>();
            returnVM.Result = new BoolResult();

            CheckScrapResult result = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.STENCIL_NO.IsNullOrWhiteSpace())
                    {
                        //throw new Exception("请输入钢网编号！");
                        ErrorInfo.Set(_localizer["stencil_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.WorkNo.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入清洗及检查人！");
                        ErrorInfo.Set(_localizer["WorkNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);

                    }else
                    {
                        var tmpdata = await _repository.GetSysManager(model.WorkNo);
                        if (tmpdata == null)
                        {
                            //string.Format("用户{0}不存在，请输入正确员工号！", userEmpno) 
                            ErrorInfo.Set(string.Format(_localizer["workno_nothing_error"], model.WorkNo), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && model.InspectResult.IsNullOrWhiteSpace())
                    {
                        //throw new Exception("当前清洗结果不能为空！");
                        ErrorInfo.Set(_localizer["InspectResult_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && (model.TENSION_A <= 0 || model.TENSION_B <= 0 || model.TENSION_C <= 0 || model.TENSION_D <= 0
                        || model.TENSION_E <= 0))
                    {
                        //throw new Exception("张力点异常，不能保存！");
                        ErrorInfo.Set(_localizer["tension_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        result = await CheckStencilCleanInput(model.STENCIL_NO);
                        if (result.Error)
                        {
                            ErrorInfo.Set(result.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.StencilCleanSave(model);
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

        #region 内部方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stencil_no"></param>
        /// <returns></returns>
        private async Task<CheckScrapResult> CheckStencilCleanInput(string stencil_no)
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
            }
            return result;
        }

        #endregion
    }
}