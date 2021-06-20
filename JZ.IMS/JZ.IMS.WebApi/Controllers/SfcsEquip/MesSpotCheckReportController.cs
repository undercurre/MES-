using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository.MesSpotCheckReport;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers.SfcsEquip
{

    /// <summary>
    /// 抽检报表控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesSpotCheckReportController : BaseController
    {
        private readonly IMesSpotCheckReportRepository _repository;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportExcelController> _localizer;

        public MesSpotCheckReportController(IMesSpotCheckReportRepository repository, IHostingEnvironment hostingEnv, IMapper mapper,
            IStringLocalizer<ImportExcelController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _hostingEnv = hostingEnv;
            _localizer = localizer;
        }

        /// <summary>
        /// 不良品明细报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DefectDetailReport()
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
        /// 获取抽检不良明细报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetCheckFailReportData([FromQuery]MesCheckReportRequestModel model)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    var list = await _repository.GetCheckFailReportData(model);
                    var data = new TableDataModel()
                    {
                        count = list.Count(),
                        data = list
                    };

                    returnVM.Result = data;
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
        /// 获取抽检合格率汇总报表数据(日)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetCheckPassRateSumDayData([FromQuery]MesCheckReportRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //model.ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    var list = await _repository.GetCheckPassRateSumDayData(model);
                    var data = new TableDataModel()
                    {
                        count = list.Count(),
                        data = list
                    };

                    returnVM.Result = JsonHelper.ObjectToJSON(data);
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion

            }
            return returnVM;
        }

        /// <summary>
        /// 获取抽检合格率汇总报表数据(月)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetCheckPassRateSumMonthData([FromQuery]MesCheckReportRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //model.ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    var list = await _repository.GetCheckPassRateSumMonthData(model);
                    var data = new TableDataModel()
                    {
                        count = list.Count(),
                        data = list
                    };

                    returnVM.Result = JsonHelper.ObjectToJSON(data);
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
        /// 获取提前一个月的日期数据
        /// </summary>
        /// <param name="beginDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<string> GetMonthDayData(DateTime beginDate)
        {
            ApiBaseReturn<string> returnVm = new ApiBaseReturn<string>();

            try
            {
                List<string> list = new List<string>();
                DateTime endDate = beginDate.AddMonths(1);

                while (beginDate < endDate)
                {
                    list.Add(beginDate.ToString("MM月dd日"));
                    beginDate = beginDate.AddDays(1);
                }

                returnVm.Result = JsonHelper.ObjectToJSON(list);
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }
            return returnVm;
        }

    }
}
