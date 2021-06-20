/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 10:16:13                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesBurnFileApplyController                                      
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
using System.Net.Http.Headers;
using System.IO;
using MySqlX.XDevAPI.Common;
using JZ.IMS.ViewModels.BurnFile;
using Google.Protobuf.WellKnownTypes;
using JZ.IMS.ViewModels.ATEDevice;

namespace JZ.IMS.WebApi.Controllers
{

    /// <summary>
    /// ATE设备 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class ATEDeviceController : BaseController
    {
        private readonly IAteDeviceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ISfcsProductConfigRepository _sfcsProductConfigRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ATEDeviceController> _localizer;



        public ATEDeviceController(IAteDeviceRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ISfcsProductConfigRepository sfcsProductConfigRepository,
            IStringLocalizer<ATEDeviceController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _sfcsProductConfigRepository = sfcsProductConfigRepository;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {

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
            returnVM.Result = false;
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
        /// 检测产品条码SN状态的接口
        /// </summary>
        /// <param name="SN">SN条码</param>
        /// <returns>SN 状态</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ATEDeviceListModel>> CheckSn([FromBody] ATECheckSN model)
        {
            ApiBaseReturn<ATEDeviceListModel> returnVM = new ApiBaseReturn<ATEDeviceListModel>();
            returnVM.Result = new ATEDeviceListModel() { RES = false };
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (model.SN.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SN_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.SiteID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["OPERATIONID_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (model.TYPE == "CheckSn")
                        {

                            var runCardModel = (await _repository.GetListByTableEX<SfcsRuncard>("*", "SFCS_RUNCARD", " AND SN=:SN AND ROWNUM=1 ", new { SN = model.SN })).FirstOrDefault();
                            if (runCardModel == null)
                            {
                                #region 判断sn是否存在runcard_range表里面
                                var woNO = await _repository.GetWONOBySN(model.SN);
                                if (!woNO.IsNullOrWhiteSpace())
                                {
                                    //通过工序查找工序
                                    var woModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND WO_NO=:WO_NO AND ROWNUM=1 ", new { WO_NO = woNO })).FirstOrDefault();
                                    if (woModel != null)
                                    {
                                        #region 判断是不是第一个工序
                                        woModel.ROUTE_ID = woModel.ROUTE_ID > 0 ? woModel.ROUTE_ID : _sfcsProductConfigRepository.GetRouteIdByPartNo(woModel.PART_NO);
                                        var currentOperationModel = (await _repository.GetListByTableEX<SfcsRouteConfig>("CURRENT_OPERATION_ID", "SFCS_ROUTE_CONFIG", " AND ROUTE_ID =:ROUTE_ID and ROwNUM=1 ORDER BY ORDER_NO ASC ", new { ROUTE_ID = woModel.ROUTE_ID })).FirstOrDefault();
                                        if (currentOperationModel != null && currentOperationModel.CURRENT_OPERATION_ID > 0)
                                        {
                                            var sites = (await _repository.GetListByTableEX<SfcsOperationSites>("*", "SFCS_OPERATION_SITES", "AND ID=:ID", new { ID = model.SiteID })).FirstOrDefault();
                                            if (sites == null) sites.OPERATION_ID = 0;
                                            if (currentOperationModel.CURRENT_OPERATION_ID == sites.OPERATION_ID)
                                                returnVM.Result.RES = true;
                                        }
                                        else
                                        {
                                            //当前工序与实际工序不一致，请注意检查
                                            ErrorInfo.Set(_localizer["Current_Process_NOT_SAME"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        }

                                        #endregion
                                    }

                                }
                                else
                                {
                                    //没有找到对应的SN
                                    ErrorInfo.Set(_localizer["SN_NOT_FOUND"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                #endregion
                            }
                            else
                            {
                                //检查SN状态和制程是否漏刷 PROCESS_TEST_REQUEST
                                var data = await _repository.CheckSNAndRoute(model.SN, model.SiteID);
                                if (data.code == 1)
                                {
                                    //SN状态有误，请注意检查！ The SN status is wrong, please check!
                                    ErrorInfo.Set(_localizer["THE_SN_WRONG"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                else
                                {
                                    returnVM.Result.RES = true;
                                }
                            }

                            //过站检测成功! Pass site detection succeeded
                            if (returnVM.Result.RES)
                            {
                                returnVM.Result.Reason = _localizer["PASS_SITE_SUCCEEDED"];
                            }
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
        /// 处理SN过站的接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<TableDataModel>> ComCommonOperation([FromBody] ATECheckSN model)
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 参数验证
                    if (model.SN.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SN_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.SiteID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["OPERATIONID_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion


                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var sites = (await _repository.GetListByTableEX<SfcsOperationSites>("*", "SFCS_OPERATION_SITES", "AND ID=:ID", new { ID = model.SiteID })).FirstOrDefault();
                        if (sites == null) sites.OPERATION_ID = 0;
                        var data = await _repository.ProcessMultiOperation(model.SN, sites.OPERATION_ID ?? 0, model.SiteID);
                        returnVM.Result = data;
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

        [HttpGet]
        public async Task<dynamic> test(string sn)
        {
            //var t = await _repository.GetWONOBySN(sn);
            var obj = await _repository.CheckSNAndRoute(sn, 12323);
            return 1;
        }
    }

}