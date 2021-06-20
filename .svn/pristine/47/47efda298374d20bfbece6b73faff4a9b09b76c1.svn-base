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
    /// 钢网保养控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtStencilMaintainController : BaseController
    {
        private readonly ISmtStencilStoreRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtStencilTakeController> _localizer;

        public SmtStencilMaintainController(ISmtStencilStoreRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtStencilTakeController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 获取状态列表
            /// </summary>
            /// <returns></returns>
            public List<IDNAME> StatusList { get; set; }
        }

        /// <summary>
        /// 钢网保养首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
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
                        returnVM.Result = new IndexVM
                        {
                            StatusList = await _repository.GetStatus(),
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
        /// 获取钢网相关信息
        /// </summary>
        /// <param name="stencil_no">钢网编号</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SmtStencilMaintainReturnModel>> GetStencilInfo([FromQuery]string stencil_no)
        {
            ApiBaseReturn<SmtStencilMaintainReturnModel> returnVM = new ApiBaseReturn<SmtStencilMaintainReturnModel>();
            CheckScrapResult checkResult = null;

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
                        checkResult = await CheckStencilMaintainStatus(stencil_no);
                        if (checkResult.Error)
                        {
                            ErrorInfo.Set(checkResult.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new SmtStencilMaintainReturnModel
                        {
                            STATUS = checkResult.STATUS ?? GlobalVariables.StencilUsed,
                            IsRun = checkResult.IsRun
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
        /// 获取钢网保养历史信息列表
        /// </summary>
        /// <param name="stencil_no">钢网编号</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtStencilMaintainHistory>>> LoadData([FromQuery]string stencil_no)
        {
            ApiBaseReturn<List<SmtStencilMaintainHistory>> returnVM = new ApiBaseReturn<List<SmtStencilMaintainHistory>>();

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
                        returnVM.Result = await _repository.GetStencilMaintainHistoryList(stencil_no);
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
        public async Task<ApiBaseReturn<BoolResult>> SaveData([FromBody] SmtStencilMaintainModel model)
        {
            ApiBaseReturn<BoolResult> returnVM = new ApiBaseReturn<BoolResult>();
            returnVM.Result = new BoolResult();
            decimal stencil_id = 0;
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

                    if (!ErrorInfo.Status && model.Remark.IsNullOrWhiteSpace())
                    {
                        //throw new Exception("请输入保养备注！");
                        ErrorInfo.Set(_localizer["Remark_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);

                    }

                    if (!ErrorInfo.Status && model.ResultStatus != 1 && model.ResultStatus != 2)
                    {
                        //throw new Exception("维护后的状态不正确！");
                        ErrorInfo.Set(_localizer["ResultStatus_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var stencil = await _repository.GetStencil(model.STENCIL_NO);
                        if (stencil == null)
                        {
                            //throw new Exception(string.Format("{0}钢网没有注册，请检查!", stencilNo));
                            ErrorInfo.Set(_localizer["stencil_check_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            stencil_id = stencil.ID;
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        
                        decimal resdata =  await _repository.StencilMaintainSave(model, stencil_id);
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

        #region 钢网使用次数
        /// <summary>
        /// Avi品质柏拉图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<string>> GetStencilUseDataAsync(string STENCIL_NO, int Page, int Limit)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetStencilUseData(STENCIL_NO, new PageModel { Page = Page, Limit = Limit });
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

        #region 内部方法

        /// <summary>
        /// 检查钢网的维修状态是否能报废
        /// </summary>
        /// <param name="stencil_no"></param>
        /// <returns></returns>
        private async Task<CheckScrapResult> CheckStencilMaintainStatus(string stencil_no)
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

                //从注册表判断是否报废
                if (stencil.MAX_USED_FLAG == GlobalVariables.EnableN)
                {
                    //Messenger.Show(string.Format("钢网{0}已经报废不能进行维护保养！", stencilNumber));
                    string errmsg = string.Format(_localizer["scrapped_error"], stencil_no);
                    result.Set(string.Format(errmsg, stencil_no));
                }
            }

            if (!result.Error)
            {
                //检查钢网是否投入使用
                var runtime = await _repository.GetStencilRuntime(stencil_no);
                if (runtime != null)
                {
                    result.IsRun = true;
                    result.STATUS = runtime.STATUS;
                    if (runtime.STATUS == GlobalVariables.StencilScrapped)
                    {
                        //Messenger.Show(string.Format("{0}钢网已经报废不能进行维护保养！", stencilNumber));
                        result.Set(string.Format(_localizer["scrapped_error"], stencil_no));
                    }
                }
            }

            return result;
        }

        #endregion
    }
}