/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：报表自定义SQL-语句表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 16:19:07                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IReportMstController                                      
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
using Microsoft.CodeAnalysis.Classification;
using Google.Protobuf.WellKnownTypes;
using JZ.IMS.IServices;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Dapper;
using JZ.IMS.Core.Utilities;

namespace JZ.IMS.WebApi.Controllers  
{
    /// <summary>
    /// 报表管理 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportMstController : BaseController
    {
        private readonly IReportMstRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ReportMstController> _localizer;

        public ReportMstController(IReportMstRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<ReportMstController> localizer)
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<ReportMstListModel>>> LoadData([FromQuery] ReportMstRequestModel model)
        {
            ApiBaseReturn<List<ReportMstListModel>> returnVM = new ApiBaseReturn<List<ReportMstListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(SQL, :Key) > 0";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<ReportMstListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<ReportMstListModel>(x);
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsync(conditions, model);

                    returnVM.Result = viewList;
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
        /// 保存数据，Sql格式为 select * from aa
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<List<decimal>>> SaveData([FromBody] ReportMstModel model)
        {
            ApiBaseReturn<List<decimal>> returnVM = new ApiBaseReturn<List<decimal>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    foreach (var item in model.InsertRecords)
                    {
                        if (!ErrorInfo.Status && item.parms.GroupBy(p => p.PARAM_NAME).Any(p => p.Count() > 1))
                        {
                            ErrorInfo.Set(_localizer["prop_notRepeat"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    foreach (var item in model.RemoveRecords)
                    {
                        if (!ErrorInfo.Status && item.parms.GroupBy(p => p.PARAM_NAME).Any(p => p.Count() > 1))
                        {
                            ErrorInfo.Set(_localizer["prop_notRepeat"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    foreach (var item in model.UpdateRecords)
                    {
                        if (!ErrorInfo.Status && item.parms.GroupBy(p => p.PARAM_NAME).Any(p => p.Count() > 1))
                        {
                            ErrorInfo.Set(_localizer["prop_notRepeat"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var ids = await _repository.SaveDataByTrans(model);
                        returnVM.Result = ids;
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

        #region 参数相关
        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<ReportParam>>> GetParams(decimal id)
        {
            ApiBaseReturn<List<ReportParam>> returnVM = new ApiBaseReturn<List<ReportParam>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var list = (await _repository.GetParams(id)).ToList();
                    var viewList = new List<ReportParam>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<ReportParam>(x);
                        viewList.Add(item);
                    });

                    returnVM.Result = viewList;
                    returnVM.TotalCount = list.Count;

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
        #endregion

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> QuerySql(ExecuteSqlModel model)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    var reportMsgItem = await _repository.GetReportMstItem(model.Mst_ID);
                    #region 检查参数
                    if (reportMsgItem.ENABLED == "N") { 
                        ErrorInfo.Set(_localizer["notUse"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    var res = await _repository.QuerySql(model, reportMsgItem);
                    returnVM.Result = res?.data;
                    returnVM.TotalCount = res?.count ?? 0;
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