using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JZ.IMS.WebApi.Controllers.Common
{
    /// <summary>
    /// 公共查询参数控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISfcsOperationLinesRepository _lineRepository;
        private readonly ISfcsParametersRepository _partmetersRepository;
        public CommonController(IMapper mapper, IHttpContextAccessor httpContextAccessor, 
            ISfcsOperationLinesRepository lineRepository, ISfcsParametersRepository partmetersRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _lineRepository = lineRepository;
            _partmetersRepository = partmetersRepository;
        }

        /// <summary>
        /// 获取全部产线
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiBaseReturn<List<AllLinesModel>> GetLinesList()
        {
            ApiBaseReturn<List<AllLinesModel>> returnVM = new ApiBaseReturn<List<AllLinesModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    returnVM.Result = _lineRepository.GetLinesList(orginizeId, "PCBA");
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
        /// 获取当前时间范围
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiBaseReturn<String> GetDateNow()
        {
            ApiBaseReturn<String> returnVM = new ApiBaseReturn<String>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    returnVM.Result = DateTime.Now.ToString("yyyy-MM-dd",System.Globalization.DateTimeFormatInfo.InvariantInfo) + " ~ " + DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
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
        /// 获取产线（通过产线类型）
        /// </summary>
        /// <param name="lineType">产线类型</param>
        /// <returns></returns>
        [HttpGet]
        public ApiBaseReturn<List<AllLinesModel>> GetLinesListToType(string lineType)
        {
            ApiBaseReturn<List<AllLinesModel>> returnVM = new ApiBaseReturn<List<AllLinesModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    returnVM.Result = _lineRepository.GetLinesList(orginizeId, lineType);
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
        /// 首五件检验类别数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiBaseReturn<List<SfcsParameters>> GetListByType(string lineType)
        {
            ApiBaseReturn<List<SfcsParameters>> returnVM = new ApiBaseReturn<List<SfcsParameters>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    returnVM.Result = _partmetersRepository.GetListByType("FIRST_CHECK_ITEM_TYPE"); ;
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
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiBaseReturn<List<SfcsDepartment>> GetDepartmentList()
        {
            ApiBaseReturn<List<SfcsDepartment>> returnVM = new ApiBaseReturn<List<SfcsDepartment>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    returnVM.Result = _partmetersRepository.GetDepartmentList(); ;
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
