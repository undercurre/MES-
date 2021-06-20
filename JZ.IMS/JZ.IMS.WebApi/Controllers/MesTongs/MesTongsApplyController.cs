/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-11 10:19:01                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtFeederController                                      
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
using JZ.IMS.WebApi.Validation;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using JZ.IMS.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using JZ.IMS.IRepository.MesTongs;
using JZ.IMS.IServices.MesTongs;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 夹具申请 控制器 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesTongsApplyController : BaseController
    {
        private readonly IMesTongsApplyRepository _repository;
        private readonly ISfcsParametersRepository _parametersRepository;
        private readonly ISfcsParametersService _serviceParameters;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsEquipContentConfController> _sfcsEquiplocalizer;
       private readonly IMesTongsApplyService _service;

        public MesTongsApplyController( IHttpContextAccessor httpContextAccessor, ISfcsParametersRepository parametersRepository, 
            IMesTongsApplyRepository repository, ISfcsParametersService serviceParameters, IStringLocalizer<SfcsEquipContentConfController> sfcsEquiplocalizer, IMesTongsApplyService service)
        {
            _httpContextAccessor = httpContextAccessor;
            _parametersRepository = parametersRepository;
            _repository = repository;
            _serviceParameters = serviceParameters;
            _sfcsEquiplocalizer = sfcsEquiplocalizer;
            _service = service;
        }


        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<List<SfcsDepartment>>> Index()
        {
            ApiBaseReturn<List<SfcsDepartment>> returnVM = new ApiBaseReturn<List<SfcsDepartment>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _parametersRepository.GetDepartmentList();
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
        /// 获取夹具类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsParameters>>> GetTongsTypeList()
        {
            ApiBaseReturn<List<SfcsParameters>> returnVM = new ApiBaseReturn<List<SfcsParameters>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        var data =await Task.Run( ()=> {return _parametersRepository.GetListByType("MES_TONGS_TYPE"); });
                        returnVM.Result = data;
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
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery]MesTongsApplyRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //model.ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                        var data = new TableDataModel();
                        data.data = await _repository.GetTongsApplyData(model);
                        returnVM.TotalCount = await _repository.GetTongsApplyDataCount(model);
                        returnVM.Result = JsonHelper.ObjectToJSON(data);
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
        /// 查询申请信息以及对应产品信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadDataAndPart([FromQuery]MesTongsApplyRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        // model.ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                        var data = new TableDataModel();

                        data.data = await _repository.GetTongsApplyAndPartData(model);
                        returnVM.TotalCount = await _repository.GetTongsApplyAndPartDataCount(model);
                        returnVM.Result = JsonHelper.ObjectToJSON(data);
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
        /// 添加或修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsDepartment>>> AddOrModify()
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
            //return View();
        }

        /// <summary>
        /// 添加或修改的相关操作
        /// 如果ID为0，用户名传CREATE_USER(即创建)
        /// 如果ID不为0，UPDATE_USER(即修改)
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> AddOrModifySave([FromForm]MesTongsApplyListModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //item.ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                        //if (item.ID == 0)
                        //    item.CREATE_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                        //else
                        //    item.UPDATE_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                        item.SURPLUS_QTY = item.QTY;
                        BaseResult result = new BaseResult();
                        result = await _repository.AddOrModify(item);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 通过ID删除记录
        /// </summary>
        /// <param name="Id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> DeleteOneById(decimal Id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        BaseResult result = new BaseResult();
                        result = await _repository.DeleteById(Id);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 通过ID更改激活状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> ChangeEnabled([FromForm]ChangeStatusModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        var result = new BaseResult();
                        ChangeStatusModelValidation validationRules = new ChangeStatusModelValidation(_sfcsEquiplocalizer);
                        ValidationResult results = validationRules.Validate(item);
                        if (results.IsValid)
                        {
                            result = await _service.ChangeEnableStatusAsync(item);
                        }
                        else
                        {
                            result.ResultCode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
                            result.ResultMsg = results.ToString("||");
                        }
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 根据ID获取夹具申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetTongsApplyById(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetTongsApplyById(id));
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
        /// 根据夹具申请ID获取对应产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetTongsApplyPartData(decimal id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetTongsApplyPartData(id));
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
        /// 根据夹具申请ID获取对应已入库夹具信息
        /// </summary>
        /// <param name="organizeId">组织ID</param>
        /// <param name="applyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetTongsDataByApplyId(decimal applyId, string organizeId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //var organizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                        returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetTongsDataByApplyId(organizeId, applyId));
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
        /// 审核夹具申请信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Id">LoginName</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> AuditData(decimal Id, string user)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //var user = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                        BaseResult result = new BaseResult();
                        result = await _repository.AuditData(Id, user);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
    }
}