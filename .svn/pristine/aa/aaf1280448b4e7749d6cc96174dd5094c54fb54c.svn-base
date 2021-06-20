/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-10 18:09:05                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtStencilStoreController                                      
*└──────────────────────────────────────────────────────────────┘
*/

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
using JZ.IMS.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 钢网存储/变更储位控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtStencilStoreController : BaseController
    {
        private readonly ISmtStencilStoreRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtStencilStoreController> _localizer;

        public SmtStencilStoreController(ISmtStencilStoreRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtStencilStoreController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 钢网存储首页视图
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
        /// 变更储位首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> ChangeLocationIndex()
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
        /// 钢网存储查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtStencilStoreListModel>>> LoadData([FromQuery]SmtStencilStoreRequestModel model)
        {
            ApiBaseReturn<List<SmtStencilStoreListModel>> returnVM = new ApiBaseReturn<List<SmtStencilStoreListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var list = (await _repository.Loadata(model)).ToList();

                    var count = await _repository.GetTotalCount(model);

                    returnVM.Result = list;
                    returnVM.TotalCount = count;

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
        /// 变更储位之钢网编号查询
        /// </summary>
        /// <param name="stencil_no">钢网编号</param>
        /// <returns>返回钢网储位</returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> ChangeLocationLoadData([FromQuery]string stencil_no)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        CheckScrapResult result = await GetStencilStoreMsgByStencilNo(stencil_no);
                        if (result.Error)
                        {
                            //"钢网没有存储过，没有原储位，不能使用更换储位功能！"
                            ErrorInfo.Set(result.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result = result.LOCATION;
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
        /// 钢网存储保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtStencilStoreModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            string errMsg = string.Empty;
            decimal stencil_id = 0;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        CheckResult result = await CheckStencilStoreInput(model);
                        if (result.Error)
                        {
                            ErrorInfo.Set(result.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            stencil_id = result.ID;
                        }
                    }

                    //判斷網板是否有清洗過
                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.CheckStencilCleaned(model.STENCIL_NO);
                        //throw new Exception(string.Format("{0}钢网尚未清洗不能入柜，请先清洗!", stencilNo));
                        if (!resdata)
                        {
                            ErrorInfo.Set(_localizer["noclear_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    //判斷網板是否已經維護過入柜信息
                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetStencilStoreBySTENCIL_ID(stencil_id);
                        if (resdata != null)
                        {
                            if (resdata.STATUS == GlobalVariables.StencilTaken)
                            {
                                //throw new Exception(string.Format("{0}钢网状态领用中，请使用归入功能!", stencilNo));
                                errMsg = string.Format(_localizer["stencil_state_error"], model.STENCIL_NO);
                                ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model, stencil_id);
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
        /// 报废出柜
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ScrapStencilStore([FromBody] ScrapStencilStoreModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            string errMsg = string.Empty;
            CheckScrapResult result = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        result = await CheckScrapStenciStorelInput(model.STENCIL_NO);
                        if (result.Error)
                        {
                            ErrorInfo.Set(result.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 报废出柜并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.ScrapStencilStore(result.STENCIL_ID, result.LOCATION, model.UserName);
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
        /// 变更储位保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeLocationSave([FromBody] ChangeLocationModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            string errMsg = string.Empty;
            CheckScrapResult result = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        result = await CheckChangeLocationInput(model.UserName, model.STENCIL_NO, model.NewLocation);
                        if (result.Error)
                        {
                            ErrorInfo.Set(result.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 变更储位并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.ChangeLocationSave(result.STENCIL_ID, model.NewLocation, result.LOCATION, model.UserName);
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
        /// 
        /// </summary>
        /// <param name="stencil_no"></param>
        /// <returns></returns>
        private async Task<CheckScrapResult> CheckScrapStenciStorelInput(string stencil_no)
        {
            CheckScrapResult result = new CheckScrapResult();

            if (!result.Error && stencil_no.IsNullOrEmpty())
            {
                //throw new Exception("请输入钢网编号！");
                result.Set(_localizer["stencil_no_error"]);
            }

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

            if (!result.Error)
            {
                var resdata = await _repository.GetStencilStoreBySTENCIL_ID(stencil.ID);
                //throw new Exception(string.Format("{0}钢网尚未入柜，无法报废", stencilNo));
                if (resdata == null)
                {
                    result.Set(string.Format(_localizer["stencil_nostore_error"], stencil_no));
                }
                else
                {
                    result.LOCATION = resdata.LOCATION;
                    if (resdata.STATUS == GlobalVariables.StencilOnline)
                    {
                        //this.ShowErrorMessage(string.Format("{0}钢网正在线上使用状态，不能做报废!", stencilNo));
                        result.Set(string.Format(_localizer["StencilOnline_error"], stencil_no));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<CheckResult> CheckStencilStoreInput(SmtStencilStoreModel model)
        {
            CheckResult result = new CheckResult();

            if (!result.Error && model.STENCIL_NO.IsNullOrEmpty())
            {
                //throw new Exception("请输入钢网编号！");
                result.Set(_localizer["stencil_no_error"]);
            }

            if (!result.Error && model.LOCATION.IsNullOrEmpty())
            {
                //throw new Exception("请输入储位！");
                result.Set(_localizer["location_error"]);
            }

            if (!result.Error && model.UserName.IsNullOrEmpty())
            {
                //throw new Exception("用户不能为空。");
                result.Set(_localizer["UserName_null"]);
            }

            if (!result.Error)
            {
                var stencil = await _repository.GetStencil(model.STENCIL_NO);
                if (stencil == null)
                {
                    //throw new Exception(string.Format("{0}钢网没有注册，请检查!", stencilNo));
                    result.Set(string.Format(_localizer["stencil_check_error"], model.STENCIL_NO));
                }
                else
                {
                    result.ID = stencil.ID;
                    if (await CheckLocationExist(model.UserName, model.LOCATION, stencil.ID))
                    {
                        //throw new Exception("这个储位上面已经有存放钢网，请换其它储位!");
                        result.Set(_localizer["location_check_error"]);
                    }
                }
            }

            return result;
        }

        private async Task<bool> CheckLocationExist(string user_name, string location,  decimal stencil_id)
        {
            bool result = false;
            SmtStencilStore store = await _repository.GetLocationInfo(location);
            if (store != null)
            {
                //如果發現輸入的儲位是當前網板佔據的，則不用處理
                if (store.STENCIL_ID == stencil_id)
                {
                    return false;
                }
                //如果發現該儲位對應的網板信息已經被刪除,則自動清除儲位信息
                var stencil = await _repository.GetStencilByID(stencil_id);
                if (stencil == null)
                {
                    await _repository.DeleteStencilLocation(user_name, location, stencil_id);
                    return false;
                }
                return true;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stencilNo"></param>
        /// <returns></returns>
        private async Task<CheckScrapResult> GetStencilStoreMsgByStencilNo(string stencilNo)
        {
            CheckScrapResult result = new CheckScrapResult();

            var stencil = await _repository.GetStencil(stencilNo);
            if (stencil == null)
            {
                //throw new Exception(string.Format("{0}钢网没有注册，请输入正确的钢网编号!", stencilNo));
                result.Set(string.Format(_localizer["stencil_config_error"], stencilNo));
            }
            else
            {
                result.ID = stencil.ID;
            }

            if (!result.Error)
            {
                var resdata = await _repository.GetStencilStoreBySTENCIL_ID(stencil.ID);

                if (resdata == null)
                {
                    //this.ShowErrorMessage(string.Format("{0}钢网尚未入柜!", stencilNo));
                    result.Set(string.Format(_localizer["change_nostore_error"], stencilNo));
                }
                else
                {
                    result.LOCATION = resdata.LOCATION;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="stencilNo"></param>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        private async Task<CheckScrapResult> CheckChangeLocationInput(string user_name, string stencilNo, string newLocation)
        {
            CheckScrapResult result = new CheckScrapResult();

            if (!result.Error && stencilNo.IsNullOrEmpty())
            {
                //throw new Exception("请输入钢网编号！");
                result.Set(_localizer["stencil_no_error"]);
            }

            if (!result.Error && newLocation.IsNullOrEmpty())
            {
                //throw new Exception("请输入新储位编号！");
                result.Set(_localizer["newlocation_error"]);
            }

            if (!result.Error)
            {
                var stencil = await _repository.GetStencil(stencilNo);
                if (stencil == null)
                {
                    //throw new Exception(string.Format("{0}钢网没有注册，请检查!", stencilNo));
                    result.Set(string.Format(_localizer["stencil_check_error"], stencilNo));
                }
                else
                {
                    result.STENCIL_ID = stencil.ID;
                }
            }

            if (!result.Error)
            {
                var resdata = await _repository.GetStencilStoreBySTENCIL_ID(result.STENCIL_ID);
                if (resdata == null)
                {
                    //this.ShowErrorMessage(string.Format("{0}钢网尚未入柜!", stencilNo));
                    result.Set(string.Format(_localizer["change_nostore_error"], stencilNo));
                }
                else
                {
                    result.LOCATION = resdata.LOCATION;
                }
            }

            if (!result.Error && await CheckLocationExist(user_name, newLocation, result.STENCIL_ID))
            {
                //throw new Exception("这个储位上面已经有存放钢网，请换其它储位!");
                result.Set(_localizer["location_check_error"]);
            }

            return result;
        }
    }
}