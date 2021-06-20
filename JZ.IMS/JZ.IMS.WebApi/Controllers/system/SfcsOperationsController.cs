using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.Services;
using FluentValidation.Results;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.WebApi.Validation;
using JZ.IMS.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using JZ.IMS.IRepository;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 工序配置 控制器  
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsOperationsController : BaseController
    {
        private readonly ISfcsOperationsService _service;
        private readonly ISfcsParametersService _serviceParameters;
        private readonly IStringLocalizer<MenuController> _menuLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISfcsOperationsRepository _repository;

        public SfcsOperationsController(ISfcsOperationsService service, ISfcsParametersService serviceParameters,
            IStringLocalizer<MenuController> menuLocalizer, IHttpContextAccessor httpContextAccessor, ISfcsOperationsRepository repository)
        {
            _menuLocalizer = menuLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _service = service;
            _serviceParameters = serviceParameters;
            _repository = repository;
        }

        /// <summary>
        /// 工序配置 首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<List<IDNAME>>> Index()
        {
            ApiBaseReturn<List<IDNAME>> returnVM = new ApiBaseReturn<List<IDNAME>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetListByTableEX<IDNAME>("LOOKUP_CODE AS ID, CHINESE AS NAME", "sfcs_parameters", " AND lookup_type = 'OPERATION_CLASS'");
                        returnVM.TotalCount = 1;
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
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery]SfcsOperationsRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.LoadDataAsync(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

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
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SfcsOperationsRequestModel model)
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
        /// 明细返回值
        /// </summary>
        public class DetailVM
        {
            /// <summary>
            /// 工序类型
            /// </summary>
            public List<SfcsParameters> OperationCategoryList { get; set; }

            /// <summary>
            /// 操作类型
            /// </summary>
            public List<IDNAME> KindList { get; set; }
        }

        /// <summary>
        /// 添加或修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async  Task<ApiBaseReturn<DetailVM>> AddOrModify()
        {
            ApiBaseReturn<DetailVM> returnVM = new ApiBaseReturn<DetailVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new DetailVM
                        {
                            OperationCategoryList = _serviceParameters.GetOperationCategoryList().OrderBy(c=>c.CHINESE).ToList(),
                            KindList = (await _repository.GetListByTableEX<IDNAME>("LOOKUP_CODE AS ID, CHINESE AS NAME", "sfcs_parameters", " AND lookup_type = 'OPERATION_CLASS'")).OrderBy(c=>c.NAME).ToList()
                        };
                        returnVM.TotalCount = 1;
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
        /// 添加或修改的相关操作
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody]SfcsOperationsAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(item.OPERATION_NAME))
                    {
                        // "工序名称不能为空.";
                        ErrorInfo.Set(_menuLocalizer["operation_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (item.Id == 0)
                    {
                        // 判断添加的是否为重复项
                        string condition = "WHERE OPERATION_NAME = :OPERATION_NAME";
                        List<SfcsOperations> isRepeated = _repository.GetList(condition, new { OPERATION_NAME = item.OPERATION_NAME.Trim() }).ToList();
                        if (isRepeated.Count != 0)
                        { 
                            //"该工序已经存在，请修改相应的工序。"
                            ErrorInfo.Set(_menuLocalizer["operation_name_exist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.AddOrModifyAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 通过ID更改激活状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeEnabled([FromBody]ChangeStatusModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_menuLocalizer);
                        ValidationResult results = validationRules.Validate(item);
                        if (!results.IsValid)
                        {
                            ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 更改激活状态并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.ChangeLockStatusAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取制程工序
        /// </summary>
        /// <param name="route_id"></param>
        /// <returns>{"ID":制程配置ID,"PRODUCT_OPERATION_CODE":唯一作业标识,"CURRENT_OPERATION_ID":当前工序,"OPERATION_NAME":"工序名称","DESCRIPTION":"描述"}</returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetRouteOperationByRouteID(int route_id = 0)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 设置返回值

                    if (route_id > 0)
                    {
                        returnVM.Result = await _repository.GetRouteOperationByRouteID(route_id);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_menuLocalizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
    }
}