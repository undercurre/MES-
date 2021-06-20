/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-17 08:55:48                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtFeederMaintainController                                      
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
    /// 飞达维护
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtFeederMaintainController : BaseController
    {
        private readonly ISmtFeederMaintainRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtFeederMaintainController> _localizer;

        public SmtFeederMaintainController(ISmtFeederMaintainRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtFeederMaintainController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> Index()
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
        /// 料架编号FEEDER 返回数据
        /// 同时判断是否存在料架号
        /// </summary>
        /// <param name="FEEDER">FEEDER</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtFeederMaintainListModel>>> GetFeederInfo([FromQuery] SmtFeederMaintainRequestModel model)
        {
            ApiBaseReturn<List<SmtFeederMaintainListModel>> returnVM = new ApiBaseReturn<List<SmtFeederMaintainListModel>>();

            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 检查参数

                    if (!ErrorInfo.Status && model.FEEDER.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入料架编号！");
                        ErrorInfo.Set(_localizer["feeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        //查是否存在料架编号
                        var tmpdata = await _repository.ItemIsByFeeder(model.FEEDER);
                        if (tmpdata == null)
                        {
                            string msg = string.Format(_localizer["feeder_noeixt_error"], model.FEEDER);
                            ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        //返回维护明细
                        if (!ErrorInfo.Status)
                        {
                            var resdata = await _repository.GetFedderMaintainList(new SmtFeederIDModel()
                            {
                                FEEDER_ID = tmpdata.ID,
                                Page = model.Page,
                                Limit = model.Limit
                            });
                            returnVM.Result = resdata.data;
                            returnVM.TotalCount = resdata.count;
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
        /// 查料架编号是否已经存在,存在就为返回数据，不存在就返回错误信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> QueryByNewFeeder([FromQuery]string feeder)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            returnVM.Result = true;
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (!ErrorInfo.Status && feeder.IsNullOrWhiteSpace())
                    {
                        // throw new Exception("请输入新料架编号！！");
                        ErrorInfo.Set(_localizer["feeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var tmpdata = await _repository.ItemIsByFeeder(feeder);
                        if (tmpdata == null)
                        {
                            string msg = string.Format(_localizer["feeder_noeixt_error"], feeder);
                            ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result = tmpdata;
                        }
                    }
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
        /// <param name="model">
        /// 料架编号:FEEDER_ID
        /// 维护类型(已保养、已校正、已保养+校正 分别对应数字1,2,3):MAINTAIN_KIND
        /// 维护人对应用户名:MAINTAIN_BY
        /// 描述:DESCRIPTION
        /// 其他字段可以不传</param>
        /// <returns></returns>
        [HttpPost]
        // [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtFeederMaintainModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status && model.insertRecords[0].FEEDER_ID.ToString().IsNullOrWhiteSpace())
                    {
                        //throw new Exception("请输入料架编号！");
                        ErrorInfo.Set(_localizer["feeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        //查是否存在料架编号
                        var tmpdata = await _repository.GetSmtFeederByFeederID(model.insertRecords[0].FEEDER_ID);
                        if (tmpdata == null)
                        {
                            string msg = string.Format(_localizer["feeder_noeixt_error"], model.insertRecords[0].FEEDER_ID);
                            ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model.insertRecords[0]);
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
    }
}