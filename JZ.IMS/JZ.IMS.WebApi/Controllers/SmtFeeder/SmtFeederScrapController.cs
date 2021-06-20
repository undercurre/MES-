/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-18 11:07:21                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtFeederRepairController                                      
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
    /// 飞达报废控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtFeederScrapController : BaseController
    {
        private readonly ISmtFeederRepairRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtFeederRepairController> _localizer;

        public SmtFeederScrapController(ISmtFeederRepairRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtFeederRepairController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 维修结果
            /// </summary>
            public List<IDNAME> RepairResultList { get; set; }
        }

        /// <summary>
        /// 首页视图
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
                            RepairResultList = await _repository.GetRepairResultList()
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
        /// 查询飞达信息
        /// </summary>
        /// <param name="feeder">飞达编号</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtFeederRepairListModel>>> LoadData(string feeder)
        {
            ApiBaseReturn<List<SmtFeederRepairListModel>> returnVM = new ApiBaseReturn<List<SmtFeederRepairListModel>>();
            decimal feeder_id = 0;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && feeder.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入料架！");
                        ErrorInfo.Set(_localizer["feeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var tmpdata = await _repository.GetFeederInfo(feeder);
                        if (tmpdata == null)
                        {
                            //throw new Exception("料架:{0}不存在！");
                            string msg = string.Format(_localizer["feeder_noexist_error1"], feeder);
                            ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            feeder_id = tmpdata.ID;
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status && feeder_id > 0)
                    {
                        returnVM.Result = await _repository.GetRepairList(feeder_id);
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtFeederScrapModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            decimal feeder_id = 0;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.FEEDER.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入料架！");
                        ErrorInfo.Set(_localizer["feeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && model.SCRAP_REASON.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入报废原因！");
                        ErrorInfo.Set(_localizer["scrap_reason_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var tmpdata = await _repository.GetFeederInfo(model.FEEDER);
                        if (tmpdata == null)
                        {
                            //throw new Exception("料架不存在！");
                            string msg = string.Format(_localizer["feeder_noexist_error1"], model.FEEDER);
                            ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            feeder_id = tmpdata.ID;
                            if (tmpdata.STATUS == (decimal)FeederStatusEnum.FEEDER_STATUS_SCRAPED)
                            {
                                //("料架已是报废状态，无需重复操作！", true);
                                ErrorInfo.Set(_localizer["feeder_status_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveFeederScrap(model, feeder_id);
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