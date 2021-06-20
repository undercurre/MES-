/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:48                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsPnController                                      
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
    /// 料号设定控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsPnController : BaseController
    {
        private readonly ISfcsPnRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsPnController> _localizer;

        public SfcsPnController(ISfcsPnRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsPnController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class VM
        {
            /// <summary>
            /// 客户列表
            /// </summary>
            public List<dynamic> CustomerList { get; set; }
            /// <summary>
            /// 产品系列
            /// </summary>
            public List<dynamic> ProductList { get; set; }
            /// <summary>
            /// 机种
            /// </summary>
            public List<dynamic> ModelList { get; set; }

            /// <summary>
            /// 制造单位
            /// </summary>
            public List<dynamic> ParametersList { get; set; }

            /// <summary>
            /// 厂部
            /// </summary>
            public List<dynamic> PlantcodeList { get; set; }

            /// <summary>
            /// 产品性质
            /// </summary>

            public List<dynamic> ProductKindList { get; set; }
            /// <summary>
            /// 生产阶段
            /// </summary>

            public List<dynamic> ProductStageList { get; set; }
            /// <summary>
            /// 类别
            /// </summary>
            public List<dynamic> LookupList { get; set; }
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<VM>> Index()
        {
            ApiBaseReturn<VM> returnVM = new ApiBaseReturn<VM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new VM
                        {
                            CustomerList = await _repository.GetListByTable(" ID, CUSTOMER, ENABLED ", "SFCS_CUSTOMERS"),
                            ProductList = await _repository.GetListByTable(" ID, FAMILY_NAME, ENABLED ", "SFCS_PRODUCT_FAMILY"),
                            ModelList = await _repository.GetListByTable(" ID, MODEL, ENABLED ", "SFCS_MODEL"),
                            ParametersList = await _repository.GetListByTable(" LOOKUP_CODE, MEANING  ", "SFCS_PARAMETERS", " And LOOKUP_TYPE = 'BU_CODE' "),
                            PlantcodeList = await _repository.GetListByTable(" LOOKUP_CODE, MEANING，CHINESE  ", "SFCS_PARAMETERS", " And LOOKUP_TYPE = 'PLANT_CODE' "),
                            ProductKindList = await _repository.GetListByTable(" LOOKUP_CODE, MEANING，CHINESE ", "SFCS_PARAMETERS", " And LOOKUP_TYPE = 'PRODUCT_KIND' "),
                            ProductStageList = await _repository.GetListByTable(" LOOKUP_CODE, MEANING，CHINESE ", "SFCS_PARAMETERS", " And LOOKUP_TYPE = 'PRODUCT_STAGE' "),
                            LookupList = await _repository.GetListByTable(" LOOKUP_CODE,	MEANING，CHINESE ", "SFCS_PARAMETERS", " And LOOKUP_TYPE = 'WO_CLASSIFICATION' 	AND LOOKUP_CODE NOT IN (4) ")
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
        public async Task<ApiBaseReturn<List<dynamic>>> LoadData([FromQuery]SfcsPnRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var result = await _repository.GetPNCode(model);
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
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SfcsPnRequestModel model)
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
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsPnModel model)
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
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
    }
}