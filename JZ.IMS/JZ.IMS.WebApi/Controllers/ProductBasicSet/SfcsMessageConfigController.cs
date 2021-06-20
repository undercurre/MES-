/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:46                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsMessageConfigController                                      
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
    /// 后台报错信息配置控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsMessageConfigController : BaseController
	{
		private readonly ISfcsMessageConfigRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<GeneralErrorController> _localizer;
		
		public SfcsMessageConfigController(ISfcsMessageConfigRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<GeneralErrorController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}

        public class VM
        {
            /// <summary>
            /// 种类状态
            /// </summary>
            public List<object> statusList { get; set; }
            /// <summary>
            /// Y/N
            /// </summary>
            public List<string> enabled { get; set; }
        }

		/// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task< ApiBaseReturn<VM>> Index()
        {
            ApiBaseReturn<VM> returnVM = new ApiBaseReturn<VM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new VM()
                        {
                            statusList = await _repository.GetStatusList(),
                            enabled = new List<string>() { "Y", "N" }
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
        public async Task<ApiBaseReturn<List<SfcsMessageConfigListModel>>> LoadData([FromQuery]SfcsMessageConfigRequestModel model)
        {
            ApiBaseReturn<List<SfcsMessageConfigListModel>> returnVM = new ApiBaseReturn<List<SfcsMessageConfigListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.MESSAGE_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(MESSAGE_NO, :MESSAGE_NO) > 0 ";
                    }
                    if (!model.CHINESE_MESSAGE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(CHINESE_MESSAGE, :CHINESE_MESSAGE) > 0 ";
                    }
                    if (!model.ENGLISH_MESSAGE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(ENGLISH_MESSAGE, :ENGLISH_MESSAGE) > 0 ";
                    }
                    if (model.PARAMETERS_COUNT>0)
                    {
                        conditions += $"and instr(PARAMETERS_COUNT, :PARAMETERS_COUNT) > 0 ";
                    }
                    if (!model.CATEGORY.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(CATEGORY, :CATEGORY) > 0 ";
                    }
                    if (!model.BACKGROUND_FLAG.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(BACKGROUND_FLAG, :BACKGROUND_FLAG) > 0 ";
                    }
                    if (!model.APPLICATION_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(APPLICATION_NAME, :APPLICATION_NAME) > 0 ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsMessageConfigListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsMessageConfigListModel>(x);
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
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SfcsMessageConfigRequestModel model)
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
        /// 更新数据的时候，如果是修改了MESSAGE_NO和原先的不同，调用LoadData方法同时查MESSAGE_NO，如果有相同的不给保存
        /// 如果没有修改MESSAGE_NO就直接保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsMessageConfigModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                   
                    //新增加
                    if (!ErrorInfo.Status&&model.insertRecords!=null)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            if (!ErrorInfo.Status&&item.MESSAGE_NO != null)
                            {
                               bool isResult= await _repository.ItemIsExist("SFCS_MESSAGE_CONFIG", "MESSAGE_NO", item.MESSAGE_NO);
                                if (isResult)
                                {
                                    string err = string.Format(_localizer["is_exist_name_error"],item.MESSAGE_NO);
                                    ErrorInfo.Set(err, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
                            if (!ErrorInfo.Status && item.PARAMETERS_COUNT!=null && Convert.ToInt32(item.PARAMETERS_COUNT) <= 0)
                            {
                                string msg = string.Format(_localizer["verifinumber_error"], "PARAMETERS_COUNT");
                                ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }
                    //更新
                    if (!ErrorInfo.Status&&model.updateRecords!=null)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            if (!ErrorInfo.Status && item.PARAMETERS_COUNT != null && Convert.ToInt32(item.PARAMETERS_COUNT) <= 0)
                            {
                                string msg = string.Format(_localizer["verifinumber_error"], "PARAMETERS_COUNT");
                                ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
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
	}
}