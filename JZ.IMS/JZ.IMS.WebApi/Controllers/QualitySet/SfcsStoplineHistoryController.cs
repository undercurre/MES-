/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-17 11:14:59                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsStoplineHistoryController                                      
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
    /// 停线解锁分析 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsStoplineHistoryController : BaseController
    {
        private readonly ISfcsStoplineHistoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsStoplineHistoryController> _localizer;

        public SfcsStoplineHistoryController(ISfcsStoplineHistoryRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsStoplineHistoryController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 
        /// </summary>
        public class IndexVM
        {
            /// <summary>
            /// 停线管控模式列表
            /// </summary>
            public List<dynamic> ModeList { get; set; }

            /// <summary>
            /// 异常类型列表
            /// </summary>
            public List<dynamic> IssueTypeList { get; set; }

            /// <summary>
            /// 维护状态列表
            /// </summary>
            public List<IDNAME> MaintainStatus { get; set; }
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
                            ModeList = await _repository.GetListByTable("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", "And LOOKUP_TYPE='STOPLINE_MODE' And Enabled='Y' order by LOOKUP_CODE"),
                            IssueTypeList = await _repository.GetListByTable("LOOKUP_CODE,MEANING,CHINESE", "SFCS_PARAMETERS", "And LOOKUP_TYPE='STOPLINE_ISSUE_TYPE' And Enabled='Y' order by LOOKUP_CODE"),
                            MaintainStatus = new List<IDNAME>
                            {
                                new IDNAME{ ID ="0", NAME = "Open"},
                                new IDNAME{ ID ="2", NAME = "Closed"},
                            },
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsStoplineHistoryListModel>>> LoadData([FromQuery]SfcsStoplineHistoryRequestModel model)
        {
            ApiBaseReturn<List<SfcsStoplineHistoryListModel>> returnVM = new ApiBaseReturn<List<SfcsStoplineHistoryListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.LoadData(model);
                    returnVM.Result = resdata?.data;
                    returnVM.TotalCount = resdata?.count ?? 0;

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
        /// 获取停线维护记录
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SfcsStoplineMaintainHistory>> GetStopLineMaintain(decimal id)
        {
            ApiBaseReturn<SfcsStoplineMaintainHistory> returnVM = new ApiBaseReturn<SfcsStoplineMaintainHistory>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (id > 0)
                        {
                            string conditions = "WHERE STOPLINE_HISTORY_ID =:id";
                            returnVM.Result = await _repository.GetAsyncEx<SfcsStoplineMaintainHistory>(conditions, new { id });
                            returnVM.TotalCount = 1;
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsStoplineMaintainHistoryAddOrModifyModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            bool hasMaintainHistory = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.STOPLINE_HISTORY_ID <= 0)
                    {
                        //维护的数据不完整，请确认。
                        ErrorInfo.Set(_localizer["Err_MaintainInfoIncomplete"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.MAINTAIN_STATUS < 0)
                    {
                        //维护的数据不完整，请确认。
                        ErrorInfo.Set(_localizer["Err_MaintainInfoIncomplete"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.ROOT_CAUSE.IsNullOrWhiteSpace())
                    {
                        //维护的数据不完整，请确认。
                        ErrorInfo.Set(_localizer["Err_MaintainInfoIncomplete"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.SOLUTION.IsNullOrWhiteSpace())
                    {
                        //维护的数据不完整，请确认。
                        ErrorInfo.Set(_localizer["Err_MaintainInfoIncomplete"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.RESPONSIBILITY.IsNullOrWhiteSpace())
                    {
                        //维护的数据不完整，请确认。
                        ErrorInfo.Set(_localizer["Err_MaintainInfoIncomplete"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        string conditions = "WHERE STOPLINE_HISTORY_ID =:STOPLINE_HISTORY_ID";
                        var maintainHistory = await _repository.GetAsyncEx<SfcsStoplineMaintainHistory>(conditions, new { model.STOPLINE_HISTORY_ID });
                        hasMaintainHistory = (maintainHistory != null);
                        if (hasMaintainHistory)
                        {
                            //比對前後維護狀態
                            if (maintainHistory.MAINTAIN_STATUS > model.MAINTAIN_STATUS)
                            {
                                //不能维护比当前状态更前的状态，请确认。
                                ErrorInfo.Set(_localizer["Err_MaintainStatusWrong"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model, hasMaintainHistory);
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
        /// 获取停线不良统计
        /// </summary>
        /// <param name="headerID">头ID</param>
        /// <remarks>
        ///  不良代码:DEFECT_CODE， 不良描述:DEFECT_DESCRIPTION， 数量:COUNT 
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetStopLineDefectStatistics(decimal headerID)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (headerID > 0)
                        {
                            returnVM.Result = await _repository.GetStopLineDefectStatistics(headerID);
                            returnVM.TotalCount = returnVM.Result?.Count ?? 0;
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
        /// 获取停线所有流水号
        /// </summary>
        /// <param name="headerID">头ID</param>
        /// <param name="PART_NO">料号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<StopLineDefectDetail>>> GetStopLineDefectDetail(decimal headerID, string PART_NO)
        {
            SfcsPn pn = null;
            ApiBaseReturn<List<StopLineDefectDetail>> returnVM = new ApiBaseReturn<List<StopLineDefectDetail>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && PART_NO.IsNullOrWhiteSpace())
                    {
                        //维护的数据不完整，请确认。
                        ErrorInfo.Set(_localizer["Err_PART_NO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        pn = await _repository.GetAsyncEx<SfcsPn>("Where PART_NO =:PART_NO", new { PART_NO });
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (headerID > 0  && pn!= null)
                        {
                            returnVM.Result = await _repository.GetStopLineDefectDetail(headerID, pn.CLASSIFICATION);
                            returnVM.TotalCount = returnVM.Result?.Count ?? 0;
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
        /// 获取产品收集不良详细信息
        /// </summary>
        /// <param name="collect_defect_detail_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsCollectDefectsDetail>>> GetCollectDefectsDetail(decimal collect_defect_detail_id)
        {
            ApiBaseReturn<List<SfcsCollectDefectsDetail>> returnVM = new ApiBaseReturn<List<SfcsCollectDefectsDetail>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    
                    if (!ErrorInfo.Status)
                    {
                        if (collect_defect_detail_id > 0)
                        {
                            string condition = "WHERE COLLECT_DEFECT_DETAIL_ID =:collect_defect_detail_id";
                            var resdata = await _repository.GetListAsyncEx<SfcsCollectDefectsDetail>(condition, new { collect_defect_detail_id });

                            returnVM.Result = resdata?.ToList();
                            returnVM.TotalCount = returnVM.Result?.Count ?? 0;
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