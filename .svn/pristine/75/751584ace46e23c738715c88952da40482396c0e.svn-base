/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 11:58:48                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsCustomersController                                      
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
    /// 客户设定 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsCustomersController : BaseController
    {
        private readonly ISfcsCustomersRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsCustomersController> _localizer;

        public SfcsCustomersController(ISfcsCustomersRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsCustomersController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            ///// <summary>
            ///// 获取父阶客户
            ///// </summary>
            //public List<IDNAME> ParentCustom { get; set; }
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
                            //ParentCustom = await _repository.GetParentCustom(),
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
        /// <remarks>
        /// Key范围: 客户名称/国家/电话/移动电话号码  
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadData([FromQuery] SfcsCustomersRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var list = await _repository.LoadData( model);

                    returnVM.Result = list.data;
                    returnVM.TotalCount = list.count;

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
        ///  获取父阶客户
        ///  传Page和Limit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetParentCustom([FromQuery] SfcsCustomersRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _repository.GetParentCustom(model);
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
        /// 导出数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery] SfcsCustomersRequestModel model)
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
        /// 当前ID是否已被使用 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ItemIsByUsed(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            bool result = false;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (id > 0)
                        {
                            result = await _repository.ItemIsByUsed(id);
                        }
                        returnVM.Result = result;
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsCustomersModel model)
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
                    if (ex.Message != null && ex.Message.IndexOf("SFCS_CUSTOMERS_UK") != -1)
                    {
                        //客户名称已经存在，请注意检查!
                        ErrorInfo.Set(_localizer["SFCS_CUSTOMERS_UK"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else { 
                        ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }

                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 添加或修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> AddOrModify()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = true;

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

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id">要删除的记录的ID</param>
        ///// <returns>JSON格式的响应结果</returns>
        //[HttpPost]
        //[Authorize("Permission")]
        //public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        //{
        //    ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 删除并返回

        //            if (!ErrorInfo.Status && id <= 0)
        //            {
        //                returnVM.Result = false;
        //                //通用提示类的本地化问题处理
        //                string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
        //                    ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
        //                ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //            }
        //            if (!ErrorInfo.Status)
        //            {
        //                var count = await _repository.DeleteAsync(id);
        //                if (count > 0)
        //                {
        //                    returnVM.Result = true;
        //                }
        //                else
        //                {
        //                    //失败
        //                    returnVM.Result = false;
        //                    //通用提示类的本地化问题处理
        //                    string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
        //                        ResultCodeAddMsgKeys.CommonExceptionMsg);
        //                    ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //                }
        //            }
        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
        //        }
        //    }

        //    #region 如果出现错误，则写错误日志并返回错误内容

        //    WriteLog(ref returnVM);

        //    #endregion

        //    return returnVM;
        //}

    }
}