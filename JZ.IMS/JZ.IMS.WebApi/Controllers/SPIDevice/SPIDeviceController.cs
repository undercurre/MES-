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
    /// SPI设备 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SPIDeviceController : BaseController
    {
        private readonly ISPIDeviceRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SPIDeviceController> _localizer;



        public SPIDeviceController(ISPIDeviceRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SPIDeviceController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
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
        [Authorize(GlobalVariables.Permission)]
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
        /// 读取SPI的接口信息
        /// </summary>
        /// <param name="SN">SN条码</param>
        /// <returns>SN 状态</returns>
        [HttpPost]
        [Authorize(GlobalVariables.Permission)]
        public async Task<ApiBaseReturn<bool>> ReadDataSPI([FromBody]SPIModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            //returnVM.Result=new ATEDeviceListModel() {  RES=false};
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    if (model.IPAddress.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["IPAddress_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.Port.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["Port_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.DataBaseName.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["DataBaseName_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.Account.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["Account_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.PWD.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["PWD_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result= await _repository.GetAOIData(model);
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