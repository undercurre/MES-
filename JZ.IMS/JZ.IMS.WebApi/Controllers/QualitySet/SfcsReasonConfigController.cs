/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-31 10:24:29                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsReasonConfigController                                      
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
    /// 品质设定 不良原因设定
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsReasonConfigController : BaseController
    {
        private readonly ISfcsReasonConfigRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MenuController> _localizer;

        public SfcsReasonConfigController(ISfcsReasonConfigRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MenuController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }


        public class IndexVM
        {
            /// <summary>
            /// 不良原因类别
            /// </summary>
            public List<object> ReasonCodeList { get; set; }
            /// <summary>
            /// 不良原因种类
            /// </summary>
            public List<object> ReasonTypeList { get; set; }
            /// <summary>
            /// 不良原因类别
            /// </summary>
            public List<object> ReasonCategoryList { get; set; }
            /// <summary>
            /// 不良原因等级
            /// </summary>
            public List<object> LeveLCodeList { get; set; }
            /// <summary>
            /// 不良原因来源
            /// </summary>
            public List<object> SourceList { get; set; }
            /// <summary>
            /// 是否激活
            /// </summary>
            public List<string> IsEnableList { get; set; }
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

                            ReasonCodeList = await _repository.GetLookUp("REASON_TYPE"),
                            ReasonTypeList = await _repository.GetLookUp("REASON_CLASS"),
                            ReasonCategoryList = await _repository.GetLookUp("REASON_CATEGORY"),
                            LeveLCodeList = await _repository.GetLookUp("REASON_LEVEL_CODE"),
                            SourceList = await _repository.GetSource(),
                            IsEnableList=new List<string>() { "Y","N"}
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
        public async Task<ApiBaseReturn<List<SfcsReasonConfigListModel>>> LoadData([FromQuery]SfcsReasonConfigRequestModel model)
        {
            ApiBaseReturn<List<SfcsReasonConfigListModel>> returnVM = new ApiBaseReturn<List<SfcsReasonConfigListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.REASON_CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(REASON_CODE, :REASON_CODE) > 0 ";
                    }
                    if (model.REASON_TYPE > 0)
                    {
                        conditions += $"and (REASON_TYPE=:REASON_TYPE) ";
                    }
                    if (model.REASON_CLASS > 0)
                    {
                        conditions += $"and (REASON_CLASS=:REASON_CLASS) ";
                    }
                    if (model.REASON_CATEGORY > 0)
                    {
                        conditions += $"and (REASON_CATEGORY=:REASON_CATEGORY)  ";
                    }
                    if (model.LEVEL_CODE > 0)
                    {
                        conditions += $"and (LEVEL_CODE=:LEVEL_CODE)  ";
                    }
                    if (!model.SOURCE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(SOURCE, :SOURCE) > 0 ";
                    }
                    if (!model.REASON_DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(REASON_DESCRIPTION, :REASON_DESCRIPTION) > 0 ";
                    }
                    if (!model.CHINESE_DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(CHINESE_DESCRIPTION, :CHINESE_DESCRIPTION) > 0 ";
                    }
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"and ENABLED=:ENABLED ";
                    }

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsReasonConfigListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsReasonConfigListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
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
		/// 导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SfcsReasonConfigRequestModel model)
        {

            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值
                    var result = await _repository.GetExportData(model);
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

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsReasonConfigModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

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
                    string msg = ex.Message;
                    if (!msg.IsNullOrWhiteSpace() && msg.IndexOf("SFCS_UNIQUE_REASON_CODE_CONFIG") != -1)
                    {
                        ErrorInfo.Set(_localizer["SFCS_UNIQUE_REASON_CODE_CONFIG"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else if (!msg.IsNullOrWhiteSpace() && msg.IndexOf("SFCS_REASON_CODE_CLASS_INX") != -1)
                    {
                        ErrorInfo.Set(_localizer["SFCS_REASON_CODE_CLASS_INX"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else
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