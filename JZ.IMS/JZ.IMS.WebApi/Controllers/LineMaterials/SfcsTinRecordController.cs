using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using JZ.IMS.IServices.LineMaterials;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels.SfcsTinRecord;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace JZ.IMS.WebApi.Controllers.LineMaterials
{
    /// <summary>
    /// 锡条用量登记
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsTinRecordController : BaseController
    {
        private readonly ISfcsTinRecordService _service;
        private readonly ISfcsEquipmentLinesService _serviceLines;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SfcsTinRecordController(ISfcsTinRecordService service, ISfcsEquipmentLinesService serviceLines, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _serviceLines = serviceLines;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<List<SfcsEquipmentLinesModel>>> Index()
        {
            ApiBaseReturn<List<SfcsEquipmentLinesModel>> returnVM = new ApiBaseReturn<List<SfcsEquipmentLinesModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //获取所有线别数据
                    //ViewData["LineList"] = _serviceLines.GetRoHSLinesList();
                    returnVM.Result = _serviceLines.GetRoHSLinesList();

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
        public async Task<ApiBaseReturn<TableDataModel>> LoadData([FromQuery]SfcsTinRecordRequestModel model)
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //model.ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    returnVM.Result = await _service.LoadDataAsync(model);

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
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsEquipmentLinesModel>>> AddOrModify()
        {
            ApiBaseReturn<List<SfcsEquipmentLinesModel>> returnVM = new ApiBaseReturn<List<SfcsEquipmentLinesModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //获取所有线别数据
                    //ViewData["LineList"] = _serviceLines.GetRoHSLinesList();
                    returnVM.Result = _serviceLines.GetRoHSLinesList();
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
        /// 添加或修改的相关操作
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> AddOrModifySave([FromForm]SfcsTinRecordAddOrModifyModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    item.ORGANIZE_ID = item.ORGANIZE_ID ?? string.Empty;
                    BaseResult result = new BaseResult();
                    if (item.ID == 0)
                    {
                        item.CREATER = item.CREATER ?? string.Empty;
                        item.CREATE_TIME = DateTime.Now;
                    }
                    result = await _service.AddOrModifyAsync(item);

                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// 保存分析结果
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> AddResultSave([FromForm]SfcsTinRecordAddOrModifyModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    BaseResult result = new BaseResult();
                    item.AUDITOR = item.AUDITOR ?? string.Empty;
                    item.AUDIT_TIME = DateTime.Now;
                    result = await _service.AddResultAsync(item);

                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// 通过ID删除记录
        /// </summary>
        /// <param name="Id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> DeleteOneById(decimal Id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    BaseResult result = new BaseResult();
                    result = await _service.DeleteAsync(Id);
                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// 从APS系统中获取产能
        /// </summary>
        /// <param name="LINE_ID"></param>
        /// <param name="OUTPUT_DAY"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> GetApsOutput(Decimal LINE_ID, DateTime OUTPUT_DAY)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    BaseResult result = new BaseResult();
                    result = await _service.GetApsOutputAsync(LINE_ID, OUTPUT_DAY);

                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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