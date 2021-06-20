/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 11:58:55                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsProductMultiRuncardController                                      
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
    /// 产品连板配置 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsProductMultiRuncardController : BaseController
    {
        private readonly ISfcsProductMultiRuncardRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ShareResourceController> _localizer;

        public SfcsProductMultiRuncardController(ISfcsProductMultiRuncardRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<ShareResourceController> localizer)
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
        /// 查询制程名称列表
        /// </summary>
        /// <param name="part_no">料号</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetRoutesList(string part_no)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && part_no.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["part_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ItemIsExist("SFCS_PN", "PART_NO", part_no);
                        if (result == false)
                        {
                            //输入的料号{0}不存在。
                            string errmsg = string.Format(_localizer["part_no_noexist"], part_no);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        string condition = " AND PART_NO=:PART_NO ";
                        returnVM.Result = await _repository.GetListByTable("ID, Route_Name", "SFCS_ROUTES",condition,new { PART_NO= part_no });
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
        /// 
        /// </summary>
        public class LoadDataVM
        {
            /// <summary>
            /// 制程工序列表
            /// </summary>
            public List<SfcsOperations> SfcsOperations { get; set; }

            /// <summary>
            /// 连板配置
            /// </summary>
            public SfcsProductMultiRuncard MultiRuncard { get; set; }

        }

        /// <summary>
        /// 查询数据(制程工序列表及连板配置)
        /// </summary>
        /// <param name="part_no">料号</param>
        /// <param name="route_id">制程ID</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<LoadDataVM>> LoadData(string part_no, decimal route_id)
        {
            ApiBaseReturn<LoadDataVM> returnVM = new ApiBaseReturn<LoadDataVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && part_no.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["part_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ItemIsExist("SFCS_PN", "PART_NO", part_no);
                        if (result == false)
                        {
                            //输入的料号{0}不存在。
                            string errmsg = string.Format(_localizer["part_no_noexist"], part_no);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && route_id <= 0)
                    {
                        ErrorInfo.Set(_localizer["route_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ItemIsExist("SFCS_ROUTES", "ID", route_id.ToString());
                        if (result == false)
                        {
                            //此制程不存在。
                            ErrorInfo.Set(_localizer["route_id_noexist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetRouteConfigLists(route_id);
                        if (resdata != null && resdata.Count > 0)
                        {
                            returnVM.Result = new LoadDataVM
                            {
                                SfcsOperations = resdata,
                                MultiRuncard = await _repository.GetSfcsProductMultiRuncard(part_no, route_id),
                            };
                        }
                        else
                        {
                            //制程没有配置，请确认。
                            ErrorInfo.Set(_localizer["routeconfig_noexist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsProductMultiRuncardAddOrModifyModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.PART_NO.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["part_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ItemIsExist("SFCS_PN", "PART_NO", model.PART_NO);
                        if (result == false)
                        {
                            //输入的料号{0}不存在。
                            string errmsg = string.Format(_localizer["part_no_noexist"], model.PART_NO);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && model.LINK_OPERATION_CODE <= 0)
                    {
                        ErrorInfo.Set(_localizer["link_operation_code_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.BREAK_OPERATION_CODE <= 0)
                    {
                        ErrorInfo.Set(_localizer["break_operation_code_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.ENABLED.IsNullOrWhiteSpace() && model.ENABLED != "Y" && model.ENABLED != "N")
                    {
                        ErrorInfo.Set(_localizer["enabled_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    //if (!ErrorInfo.Status && model.LINK_OPERATION_CODE == model.BREAK_OPERATION_CODE)
                    //{
                    //    ErrorInfo.Set(_localizer["link_break_some_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
                    string msg = ex.Message;
                    if (!msg.IsNullOrWhiteSpace() && msg.IndexOf("SFCS_PRODUCT_MULTI_RUNCARD_INX") != -1)
                    {
                        //ErrorInfo.Set(_localizer["sfcs_product_multi_runcard_inx"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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