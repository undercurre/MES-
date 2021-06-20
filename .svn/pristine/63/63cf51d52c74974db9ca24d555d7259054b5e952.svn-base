/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-03 17:21:30                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISfcsScraperCleanHistoryController                                      
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
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 刮刀清洗控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsScraperCleanController : BaseController
    {
        private readonly ISfcsScraperCleanRepository _repository;
        private readonly IStringLocalizer<SfcsScraperCleanController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SfcsScraperCleanController(IStringLocalizer<SfcsScraperCleanController> localizer, ISfcsScraperCleanRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _localizer = localizer;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public class IndexVM
        {
            /// <summary>
            /// 刮刀状态列表
            /// </summary>
            /// <returns></returns>
            public List<IDNAME> ScraperStatusList { get; set; }
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
                            ScraperStatusList = await _repository.GetScraperStatusAsync()
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

        public class ScraperVM
        {
            /// <summary>
            /// 刮刀相关信息
            /// </summary>
            /// <returns></returns>
            public ScraperRuncardVM RuncardInfo { get; set; }

            /// <summary>
            /// 刮刀清洗记录列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsScraperCleanHistory> CleanHistoryList { get; set; }
        }

        /// <summary>
        /// 查询刮刀相关记录
        /// </summary>
        /// <param name="scraperNo">刮刀号</param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<ScraperVM>> LoadScraperData(string scraperNo)
        {
            ApiBaseReturn<ScraperVM> returnVM = new ApiBaseReturn<ScraperVM>();
            List<SfcsScraperCleanHistory> cleanHistoryList = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (!ErrorInfo.Status && string .IsNullOrWhiteSpace(scraperNo))
                    {
                        ErrorInfo.Set(_localizer["ScraperNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    ScraperRuncardVM runcard = await GetScraperRuncard(scraperNo);
                    if (runcard.ErrCode == 1)
                    {
                        ErrorInfo.Set(runcard.ErrMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && (runcard.Status == (decimal)ScraperEnm.SCRAPER_USE || runcard.Status == (decimal)ScraperEnm.SCRAPER_ONLINE))
                    {
                        string errmsg = string.Format(_localizer["scraperState_error"], scraperNo);
                        ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        cleanHistoryList = await _repository.GetScraperCleanHistoryList(scraperNo);
                    }
                    
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new ScraperVM
                        {
                            RuncardInfo = runcard,
                            CleanHistoryList = cleanHistoryList
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
        /// 查询刮刀清洗记录列表
        /// </summary>
        /// <param name="scraperNo">刮刀号</param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsScraperCleanHistory>>> LoadGetScraperCleanHistory(string scraperNo)
        {
            ApiBaseReturn<List<SfcsScraperCleanHistory>> returnVM = new ApiBaseReturn<List<SfcsScraperCleanHistory>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(scraperNo))
                    {
                        ErrorInfo.Set(_localizer["ScraperNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetScraperCleanHistoryList(scraperNo);
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsScraperCleanModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model.SCRAPER_NO))
                    {
                        ErrorInfo.Set(_localizer["ScraperNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model.INSPECT_RESULT))
                    {
                        ErrorInfo.Set(_localizer["INSPECT_RESULT_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

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

        #region 内部方法

        /// <summary>
        /// 获取刮刀runcard数据
        /// </summary>
        /// <param name="scraperNo"></param>
        private async Task<ScraperRuncardVM> GetScraperRuncard(string scraperNo)
        {
            ScraperRuncardVM runcard = new ScraperRuncardVM();

            SfcsScraperRuncard scraperRow = await _repository.GetScraperRuncard(scraperNo);
            if (scraperRow == null)
            {
                runcard.ErrCode = 1;
                string msg = _localizer["scraperRow_error"];
                runcard.ErrMessage = string.Format(msg, scraperNo);
                return runcard;
            }

            SfcsScraperCleanHistory cleanRow = await _repository.GetScraperCleanHistory(scraperNo);
            if (cleanRow != null && cleanRow.CLEAN_TIME != null)
            {
                runcard.LastCleanTime = cleanRow.CLEAN_TIME;
            }

            decimal tempSiteID = 0;
            if (scraperRow.OPERATION_SITE_ID != null)
            {
                tempSiteID = scraperRow.OPERATION_SITE_ID ?? 0;
            }
            else if (scraperRow.BIND_SITE_ID != null)
            {
                tempSiteID = scraperRow.BIND_SITE_ID ?? 0;
            }

            IDNAME siteRow = await _repository.GetSmtLine(tempSiteID);
            if (siteRow != null)
            {
                runcard.SiteName = siteRow.NAME;
                runcard.SiteID = Convert.ToDecimal(siteRow.ID);
            }
            runcard.Status = scraperRow.STATUS;
            runcard.PrintCount = scraperRow.PRINT_COUNT ?? 0;
            runcard.ProductCount = scraperRow.PRODUCT_PASS_COUNT ?? 0;

            return runcard;
        }


        #endregion
    }
}