/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：收藏夹表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-28 14:22:27                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISysFavoritesController                                      
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
    ///  收藏夹 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SysFavoritesController : BaseController
    {
        private readonly ISysFavoritesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SysFavoritesController> _localizer;

        public SysFavoritesController(ISysFavoritesRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SysFavoritesController> localizer)
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
        [Authorize]
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SysFavoritesListModel>>> LoadData([FromQuery]SysFavoritesRequestModel model)
        {
            ApiBaseReturn<List<SysFavoritesListModel>> returnVM = new ApiBaseReturn<List<SysFavoritesListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.LoadData(model);

                    returnVM.Result = resdata.data;
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SysFavoritesModel model)
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize]
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

    }
}