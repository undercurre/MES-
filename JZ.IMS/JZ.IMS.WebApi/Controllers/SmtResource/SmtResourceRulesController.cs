/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-02-27 14:15:50                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtResourceRulesController                                      
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
    /// 辅料管控规则控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtResourceRulesController : BaseController
    {
        private readonly ISmtResourceRulesRepository _repository;
        private readonly ISmtResourceRouteRepository _routeRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsEquipContentConfController> _localizer;

        public SmtResourceRulesController(ISmtResourceRulesRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsEquipContentConfController> localizer, ISmtResourceRouteRepository routeRepository)
        {
            _repository = repository;
            _routeRepository = routeRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 辅料名称集
            /// </summary>
            /// <returns></returns>
            public List<IDNAME> NameList { get; set; }

            /// <summary>
            /// 辅料类型列表
            /// </summary>
            /// <returns></returns>
            //public List<IDNAME> CategoryList { get; set; }

            /// <summary>
            /// 辅料规则工序选择列表
            /// </summary>
            /// <returns></returns>
            public List<ResourceRulesProcess> ProcessList { get; set; }
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
                            NameList = await _routeRepository.GetNameAsync(),
                            //CategoryList = await _routeRepository.GetCategoryAsync(),
                            ProcessList = await _repository.GetProcessAsync(),
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
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <remarks>
        /// 字段说明: 
        /// 主键ID:  Decimal ID     名称(ID): Decimal  OBJECT_ID 
        /// 工序(id): Decimal ROUTE_OPERATION_ID   标准时间: Decimal STANDARD_TIME      计算流程时间(分钟): String(Y/N) STANDARD_FLAG   
        /// 检验有效期: String(Y/N) VALID_FLAG     检验暴露期: String(Y/N) EXPOSE_FLAG   是否激活: String(Y/N) ENABLED  
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> LoadData([FromQuery]SmtResourceRulesRequestModel model)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var result = await _repository.GetResourceRulesList(model);
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
                WriteLog(ref returnVM);
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 导出方法
        /// </summary>
        /// <remarks>
        /// 字段说明: 
        /// 主键ID:  Decimal ID     名称(ID): Decimal  OBJECT_ID 
        /// 工序(id): Decimal ROUTE_OPERATION_ID   标准时间: Decimal STANDARD_TIME      计算流程时间(分钟): String(Y/N) STANDARD_FLAG   
        /// 检验有效期: String(Y/N) VALID_FLAG     检验暴露期: String(Y/N) EXPOSE_FLAG   是否激活: String(Y/N) ENABLED  
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> ExportData([FromQuery]SmtResourceRulesRequestModel model)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
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
                WriteLog(ref returnVM);
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
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

                    if (!ErrorInfo.Status)
                    {
                        if (id <= 0)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
                                ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtResourceRulesModel model)
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
        /// 查询辅料回温情况
        /// </summary>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtResourceWarmVM>>> GetSmtResourceWarm()
        {
            ApiBaseReturn<List<SmtResourceWarmVM>> returnVM = new ApiBaseReturn<List<SmtResourceWarmVM>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var result = await _repository.GetSmtResourceWarm();
                    returnVM.Result = result;

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
        /// 查询辅料使用情况
        /// </summary>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtResourceUseVM>>> GetSmtResourceUse()
        {
            ApiBaseReturn<List<SmtResourceUseVM>> returnVM = new ApiBaseReturn<List<SmtResourceUseVM>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var result = await _repository.GetSmtResourceUse();
                    returnVM.Result = result;

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