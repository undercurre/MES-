using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;

namespace JZ.IMS.WebApi.Controllers.system
{

    /// <summary>
    /// 简易版SOP控制器	
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SimpleSOPController : BaseController
    {
        private readonly ISOPRoutesService _service;
        private readonly ISimpleSOP_ROUTESRepository _repository;
        private readonly ISysEmployeeService _serviceSysEmployee;

        public SimpleSOPController(ISOPRoutesService service, ISimpleSOP_ROUTESRepository repository, ISysEmployeeService serviceSysEmployee)
        {
            _serviceSysEmployee = serviceSysEmployee;
            _repository = repository;
            _service = service;
        }

        public class IndexVM
        {
            /// <summary>
            /// 获取状态类型列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsEquipmentLinesModel> LinesList { get; set; }

            /// <summary>
            /// 获取状态类型列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsOperationsListModel> OperationList { get; set; }
        }

        /// <summary>
        /// 简易版SOP 首页
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

                    returnVM.Result = new IndexVM
                    {
                        LinesList = await _repository.GetLinesList(),
                        OperationList = await _repository.GetEnabledLists(),
                    };

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
		/// 产品图:资源列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadResourceProductData(decimal routeId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.LoadResourceProductData(routeId);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);

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
        /// 零件图:资源列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadResourceCmpData(decimal operationRouteId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.LoadResourceCmpData(operationRouteId);
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
        /// 加载作业图列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadResourceData(decimal operationRouteId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.LoadResourceData(operationRouteId);
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
        /// 获取员工信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadEmployeeData(string user_id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _serviceSysEmployee.LoadDataAsync(user_id);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);

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
        /// 获取工序评判标准
        /// </summary>
        /// <param name="site_id">站点ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadSkillStandard(decimal site_id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.LoadSkillStandard(site_id);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);

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
        /// 判断是否存在未处理的安灯呼叫
        /// </summary>
        /// <param name="operation_id"></param>
        /// <param name="operation_site_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> IsExistAndonCall(decimal operation_id, decimal operation_site_id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    #endregion

                    #region 设置返回值
                    returnVM.Result = (await _repository.GetCallRecord(operation_id, operation_site_id));
                    return returnVM;

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
        /// 根据线体获取站点信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetOperationSiteList(decimal lineId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    #endregion

                    #region 设置返回值
                    var data = await _repository.GetOperationSiteList(lineId);
                     returnVM.Result = JsonHelper.ObjectToJSON(data);
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
		/// 根据工序名称获取ID
		/// </summary>
		/// <param name="operationName"></param>
		/// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> GetOperationIdByName(string operationName)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    #endregion

                    #region 设置返回值
                    decimal resdata = (await _repository.GetEnabledListsync()).FirstOrDefault(f => f.OPERATION_NAME == operationName).Id;
                    returnVM.Result = resdata;
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
        /// 根据料号获取SOP配置工序
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetOperationsByPartNo(string partNo, string version, string npNo = "")
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    #endregion

                    #region 设置返回值

                    var data = await _repository.GetOperationsByPartNo(partNo, version);
                    if (data.Count == 0 && !string.IsNullOrEmpty(npNo))
                        data = await _repository.GetOperationsByPartNo(npNo, version);
                    if (data != null) returnVM.Result = JsonHelper.ObjectToJSON(data);
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
        /// 根据料号获取对应可用版本号
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSopVersionsData(string partNo, string npNo = "")
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    #endregion

                    #region 设置返回值

                    var data = await _repository.GetSopVersionsData(partNo);

                    if (data.Count == 0 && !string.IsNullOrEmpty(npNo))
                        data = await _repository.GetSopVersionsData(npNo);

                    returnVM.Result = JsonHelper.ObjectToJSON(data);

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
        /// 根据料号获取对应NP号
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetNpNoByPartNo(string partNo)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    #endregion

                    #region 设置返回值

                    returnVM.Result= await _repository.GetNpNoByPartNo(partNo);

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