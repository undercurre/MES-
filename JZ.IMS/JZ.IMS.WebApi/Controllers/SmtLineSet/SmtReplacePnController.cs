/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-23 09:03:32                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtReplacePnController                                      
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
using JZ.IMS.ViewModels.SmtLineSet;

namespace JZ.IMS.WebApi.Controllers  
{
    /// <summary>
    /// 贴片防错替代料设定控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtReplacePnController : BaseController
    {
        private readonly ISmtReplacePnRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtReplacePnController> _localizer;

        public SmtReplacePnController(ISmtReplacePnRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtReplacePnController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            public List<object> StatusList { get; set; }
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
        /// 获取供应商下拉表,需要查询请使用KEY值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<object>>> GetVendorCode([FromQuery]IDNameRequestModel model)
        {
            ApiBaseReturn<List<object>> returnVM = new ApiBaseReturn<List<object>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    int count = 0;
                    var result = await _repository.GetVendorCode(model);
                    if (result!=null)
                    {
                        count = result.count;
                    }
                    returnVM.Result = result.data;
                    returnVM.TotalCount = count;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容
            WriteLog(ref returnVM);
            #endregion

            return returnVM;
        }

        /// <summary>
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadData([FromQuery]SmtReplacePnRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var result = await _repository.GetSmtReplaceList(model);
                    returnVM.Result = result.data;
                    returnVM.TotalCount = result.count;
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
        /// 导出数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SmtReplacePnRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    
                    var res = await _repository.GetExportData(model);
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;

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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtReplacePnModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    //if (!ErrorInfo.Status&&model.insertRecords!= null&&model.insertRecords.Count>0)
                    //{
                    //    foreach (var item in model.insertRecords)
                    //    {
                    //        if (item.WO_NO.IsNullOrWhiteSpace() && item.PCB_PN.IsNullOrWhiteSpace() && item.COMPONENT_PN.IsNullOrWhiteSpace() && item.REPLACE_PN.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    //        {
                    //            //工单和成品料号不能同时为空，请注意检查！ 
                    //            ErrorInfo.Set(_localizer["ORDER_PN_CANNOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);

                    //        }
                    //        else
                    //        {
                    //            //生产工单 成品料号 当前料号 替代料号同决定唯一
                    //            if (model.insertRecords.Where(m => m.WO_NO == item.WO_NO && m.PCB_PN == item.PCB_PN && m.COMPONENT_PN == item.COMPONENT_PN && m.REPLACE_PN == item.REPLACE_PN).Count() > 1)
                    //            {
                    //                ErrorInfo.Set(_localizer["DATA_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //            }
                    //            else
                    //            {
                    //                string selectReplacePN = "SELECT COUNT(1) FROM SMT_REPLACE_PN WHERE WO_NO=:WO_NO AND PCB_PN=:PCB_PN AND COMPONENT_PN=:COMPONENT_PN AND REPLACE_PN=:REPLACE_PN";
                    //                int qty = _repository.QueryEx<int>(selectReplacePN, new { WO_NO = item.WO_NO, PCB_PN = item.PCB_PN, COMPONENT_PN = item.COMPONENT_PN, REPLACE_PN = item.REPLACE_PN }).FirstOrDefault();
                    //                if (qty > 0)
                    //                {
                    //                    ErrorInfo.Set(_localizer["DATA_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    //if (!ErrorInfo.Status&&model.updateRecords != null && model.updateRecords.Count > 0 )
                    //{
                    //    foreach (var item in model.updateRecords)
                    //    {
                    //        if (item.WO_NO.IsNullOrWhiteSpace() && item.PCB_PN.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    //        {
                    //            ErrorInfo.Set(_localizer["ORDER_PN_CANNOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //        }
                    //    }
                    //}
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model);
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