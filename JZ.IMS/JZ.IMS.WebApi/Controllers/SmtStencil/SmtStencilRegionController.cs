/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-05 09:34:54                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtStencilRegionController                                      
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
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Public;
using System.Reflection;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 使用次数区间控制器 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtStencilRegionController : BaseController
    {
        private readonly ISmtStencilRegionRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtStencilRegionController> _localizer;

        public SmtStencilRegionController(ISmtStencilRegionRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtStencilRegionController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 钢网类型
            /// </summary>
            /// <returns></returns>
            public List<CodeName> StencilTypeList { get; set; }

            /// <summary>
            /// 区间状态集
            /// </summary>
            /// <returns></returns>
            public List<CodeName> StatusList { get; set; }
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
                            StencilTypeList = await _repository.GetStencilTypeList(),
                            StatusList = await _repository.GetStatusList(),
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
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtStencilRegionListModel>>> LoadData([FromQuery]SmtStencilRegionRequestModel model)
        {
            ApiBaseReturn<List<SmtStencilRegionListModel>> returnVM = new ApiBaseReturn<List<SmtStencilRegionListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID>0 ";

                    if (!string.IsNullOrWhiteSpace(model.Key))
                    {
                        conditions += $" AND STENCIL_ID =:Key";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SmtStencilRegionListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SmtStencilRegionListModel>(x);
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
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SmtStencilRegionRequestModel model)
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
        /// 添加或修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> AddOrModify()
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
            return returnVM;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status && id <= 0)
                    {
                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
                            ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var count = await _repository.DeleteAsync(id);
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                ResultCodeAddMsgKeys.CommonExceptionMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtStencilRegionModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.insertRecords != null && model.insertRecords.Where(t => t.STENCIL_ID < 0).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["STENCIL_ID_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.updateRecords != null && model.updateRecords.Where(t => t.STENCIL_ID == 0).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["STENCIL_ID_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.insertRecords != null && model.insertRecords.Where(t => t.BETWEEN_STATUS <= 0).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["BETWEEN_STATUS_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.updateRecords != null && model.updateRecords.Where(t => t.BETWEEN_STATUS <= 0).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["BETWEEN_STATUS_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.insertRecords != null && model.insertRecords.Where(t => t.OUTSIDE_STATUS <= 0).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["OUTSIDE_STATUS_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.updateRecords != null && model.updateRecords.Where(t => t.OUTSIDE_STATUS <= 0).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["OUTSIDE_STATUS_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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

    }
}