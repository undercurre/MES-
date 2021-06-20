using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.IRepository;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using JZ.IMS.Core.Helper;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 刮刀作业(领用/归还)控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsScraperOperatController : BaseController
    {
        private readonly ISfcsScraperOperatRepository _repository;
        private readonly IStringLocalizer<SfcsScraperOperatController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SfcsScraperOperatController(IStringLocalizer<SfcsScraperOperatController> localizer, ISfcsScraperOperatRepository repository,
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

            /// <summary>
            /// 站点列表
            /// </summary>
            /// <returns></returns>
            public List<IDNAME> SmtLineList { get; set; }
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
                            ScraperStatusList = await _repository.GetScraperStatusAsync(),
                            SmtLineList = await _repository.GetSmtLine(),
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
            public ScraperOperatVM ScraperInfo { get; set; }

            /// <summary>
            /// 刮刀作业记录列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsScraperOperationHistory> ScraperOperatHistoryList { get; set; }
        }

        /// <summary>
        /// 查询刮刀作业记录
        /// </summary>
        /// <param name="scraperNo">刮刀号</param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<ScraperVM>> LoadScraperData(string scraperNo)
        {
            ApiBaseReturn<ScraperVM> returnVM = new ApiBaseReturn<ScraperVM>();
            List<SfcsScraperOperationHistory> operatHistoryList = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(scraperNo))
                    {
                        ErrorInfo.Set(_localizer["ScraperNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    ScraperOperatVM operatInfo = await GetScraperRuncard(scraperNo);
                    if (operatInfo.ErrCode == 1)
                    {
                        ErrorInfo.Set(operatInfo.ErrMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        operatHistoryList = await _repository.GetScraperOperationHistoryList(scraperNo);
                    }

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new ScraperVM
                        {
                            ScraperInfo = operatInfo,
                            ScraperOperatHistoryList = operatHistoryList
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsScraperOperatModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            string errMsg = string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model.SCRAPER_NO))
                    {
                        ErrorInfo.Set(_localizer["ScraperNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && (model.ActionType != (int)ScraperEnm.SCRAPER_TAKEN && model.ActionType != (int)ScraperEnm.SCRAPER_STORED))
                    {
                        ErrorInfo.Set(_localizer["ActionType_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model.WorkerNo))
                    {
                        ErrorInfo.Set(_localizer["WorkerNo_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.ActionType == (int)ScraperEnm.SCRAPER_TAKEN && model.SiteID <= 0)
                    {
                        ErrorInfo.Set(_localizer["SiteID_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        SfcsScraperRuncard runcardRow = await _repository.GetScraperRuncard(model.SCRAPER_NO);
                        if (runcardRow != null)
                        {
                            if (runcardRow.STATUS == (decimal)model.ActionType)
                            {
                                string typeName = (runcardRow.STATUS == 6) ? "领用" : "归还";
                                string acceptLanguage = GetCurMessage(_httpContextAccessor);
                                if (acceptLanguage.IndexOf("zh") == -1)
                                {
                                    typeName = (runcardRow.STATUS == 6) ? "Use" : "return";
                                }

                                errMsg = string.Format(_localizer["scraperState_error"], model.SCRAPER_NO, typeName);
                                ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            #region  领用规则

                            if (model.ActionType == (int)ScraperEnm.SCRAPER_TAKEN)
                            {
                                if (runcardRow.STATUS == (int)ScraperEnm.SCRAPER_ONLINE)
                                {
                                    //throw new Exception(string.Format("刮刀{0}已经上线，不能领用。", model.SCRAPER_NO));
                                    errMsg = string.Format(_localizer["SCRAPER_ONLINE_error"], model.SCRAPER_NO);
                                    ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }

                                if (runcardRow.STATUS == (int)ScraperEnm.SCRAPER_USE)
                                {
                                    //throw new Exception(string.Format("刮刀{0}正在使用中，不能领用。", model.SCRAPER_NO));
                                    errMsg = string.Format(_localizer["SCRAPER_USE_error"], model.SCRAPER_NO);
                                    ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }

                                if (runcardRow.STATUS == (int)ScraperEnm.SCRAPER_OFFLINE)
                                {
                                    //throw new Exception(string.Format("刮刀{0}已下线，不能领用，请先清洗归还。", model.SCRAPER_NO));
                                    errMsg = string.Format(_localizer["SCRAPER_OFFLINE_error"], model.SCRAPER_NO);
                                    ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }

                                if (runcardRow.STATUS == (int)ScraperEnm.SCRAPER_FAIL)
                                {
                                    //throw new Exception(string.Format("刮刀{0}检查不合格，不能领用。", model.SCRAPER_NO));
                                    errMsg = string.Format(_localizer["SCRAPER_FAIL_error"], model.SCRAPER_NO);
                                    ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                // 即只能是清洗或归还状态才能领用

                                //檢查刮刀是否已經存儲了180天未使用
                                SfcsScraperOperationHistory operationHistory = await _repository.GetScraperOperationHistory(model.SCRAPER_NO, model.ActionType);

                                if (operationHistory != null)
                                {
                                    DateTime nowTime = DateTime.Now;

                                    if (operationHistory.OPERATION_TIME?.AddDays(180) <= nowTime)
                                    {
                                        //throw new Exception("刮刀存储已超过180天！");
                                        errMsg = _localizer["SCRAPER_180_error"];
                                        ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                            }

                            #endregion

                            #region 归还规则

                            if (model.ActionType == (int)ScraperEnm.SCRAPER_STORED && runcardRow.STATUS != (int)ScraperEnm.SCRAPER_CLEAN)
                            {
                                //throw new Exception(string.Format("刮刀{0}尚不能归还，请先下线清洗。", model.SCRAPER_NO));
                                errMsg = string.Format(_localizer["STORED_SCLEAN_error"], model.SCRAPER_NO);
                                ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            #endregion
                        }
                    }

                    if (!ErrorInfo.Status && model.ActionType == (int)ScraperEnm.SCRAPER_TAKEN)
                    {
                        CheckBindingResult result = await CheckScraperBinding(model.SCRAPER_NO, model.SiteID);
                        if (result.Result == false)
                        {
                            //throw new Exception("操作失败，{0}，请重新选择。".FormatWith(errMsg));
                            errMsg = string.Format(_localizer["ScraperBinding_error"], result.ErrMsg);
                            ErrorInfo.Set(errMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
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

        #region 刮刀使用次数
        /// <summary>
        /// 刮刀使用次数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetScraperUseDataAsync(string SCRAPER_NO, int Page, int Limit)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetScraperUseData(SCRAPER_NO, new PageModel { Page=Page, Limit=Limit });
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
        /// 获取刮刀runcard数据
        /// </summary>
        /// <param name="scraperNo"></param>
        private async Task<ScraperOperatVM> GetScraperRuncard(string scraperNo)
        {
            ScraperOperatVM operat = new ScraperOperatVM();

            SfcsScraperRuncard runcardRow = await _repository.GetScraperRuncard(scraperNo);
            if (runcardRow == null)
            {
                operat.Status = (int)ScraperEnm.SCRAPER_STORED;
            }
            else
            {
                operat.Status = runcardRow.STATUS;

                if (runcardRow.BIND_SITE_ID != null)
                {
                    operat.SiteID = runcardRow.BIND_SITE_ID;
                    if (runcardRow.STATUS == (int)ScraperEnm.SCRAPER_CLEAN)
                    {
                        if (runcardRow.OPERATION_SITE_ID != null)
                            operat.SiteID = runcardRow.OPERATION_SITE_ID;
                    }
                }
                else
                {
                    operat.SiteID = 0;
                }
            }

            //获取刮刀注册信息
            SfcsScraperConfig configRow = await _repository.GetScraperConfig(scraperNo);
            if (configRow == null)
            {
                operat.ErrCode = 1;
                operat.ErrMessage = _localizer["scraperRow_error"];
                return operat;
            }
            else
            {
                operat.LOCATION = configRow.LOCATION;
            }
            return operat;
        }

        public class CheckBindingResult
        {
            public bool Result { get; set; } = true;

            public string ErrMsg { get; set; } = string.Empty;
        }

        /// <summary>
        /// 檢查刮刀綁定
        /// </summary>
        /// <param name="scraper_no"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        private async Task<CheckBindingResult> CheckScraperBinding(string scraper_no, decimal siteID)
        {
            CheckBindingResult result = new CheckBindingResult();
            string scraperChar = CheckScraperFrontOrRear(scraper_no);

            int scraperCharIndex = scraper_no.IndexOf(scraperChar);
            string anotherScraper = this.GetAnotherScraper(scraper_no, scraperCharIndex);

            int scraperExistCount = 0;
            int bindCount = 0;

            SfcsScraperRuncard runcardRow = await _repository.GetScraperRuncard(scraper_no);
            SfcsScraperRuncard anotherScraperRow = await _repository.GetScraperRuncard(anotherScraper);
            if (runcardRow != null)
            {
                scraperExistCount++;
            }

            if (anotherScraperRow != null)
            {
                scraperExistCount++;
            }

            List<SfcsScraperRuncard> bindSiteList = await _repository.GetScraperRuncardBySite(siteID);
            foreach (SfcsScraperRuncard row in bindSiteList)
            {
                if (row.STATUS != (int)(int)ScraperEnm.SCRAPER_STORED)
                {
                    bindCount++;
                }
            }

            //檢查兩副刮刀綁定
            if (bindCount > 3)
            {
                //errMsg = "当前站点已达到刮刀最大绑定数量";
                result.ErrMsg = _localizer["Scraper_Max_error"];
                result.Result = false;
            }
            //檢查一副刮刀綁定
            if (scraperExistCount > 1)
            {
                if (runcardRow.BIND_SITE_ID == anotherScraperRow.BIND_SITE_ID
                    && runcardRow.STATUS != (int)ScraperEnm.SCRAPER_STORED
                    && anotherScraperRow.STATUS != (int)ScraperEnm.SCRAPER_STORED)
                {
                    //errMsg = "该刮刀已被绑定到别的站点";
                    result.ErrMsg = _localizer["Scraper_OtherSite_error"];
                    result.Result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 查看是前刮刀還是後刮刀，並算出截取位數 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bCategory"></param>
        private string CheckScraperFrontOrRear(string input, bool bCategory = false)
        {
            string scraperChar = string.Empty;

            if (!bCategory) return scraperChar;

            if (input.Contains("F"))
            {
                scraperChar = "F";
            }
            else if (input.Contains("R"))
            {
                scraperChar = "R";
            }
            //else
            //{
            //    throw new Exception("刮刀使用位置错误，请检查");
            //}
            return scraperChar;
        }

        /// <summary>
        /// 傳回另一個刮刀號
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="bCategory"></param>
        /// <returns></returns>
        private string GetAnotherScraper(string input, int index, bool bCategory = false)
        {
            if (!bCategory) return input;

            string scraperAnother = string.Empty;
            string scraperFirst = input.Substring(0, index);
            string scraperEnd = input.Substring(index + 1);
            string scraperChar = input.Substring(index, 1);

            if (scraperChar == "F")
            {
                scraperAnother = scraperFirst + "R" + scraperEnd;
            }
            else if (scraperChar == "R")
            {
                scraperAnother = scraperFirst + "F" + scraperEnd;
            }
            return scraperAnother;
        }

        #endregion
    }
}