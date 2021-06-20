/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-02-26 11:58:40                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： I
* ontroller                                      
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
    /// 辅料类型管理控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtResourceCategoryController : BaseController
    {
        private readonly ISmtResourceCategoryRepository _repository;
        private readonly ISmtResourceRouteRepository _routeRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsEquipContentConfController> _localizer;

        public SmtResourceCategoryController(ISmtResourceCategoryRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
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
            /// 辅料特性列表
            /// </summary>
            /// <returns></returns>
            public List<IDNAME> PropertyList { get; set; }
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
                            PropertyList = await _routeRepository.GetPropertyAsync()
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
        /// <remarks>
        /// 返回数据: 辅料类型集, 字段说明: 
        /// 辅料类型ID: Decimal  ID           辅料名称(ID): Decimal OBJECT_ID 
        /// 供应商ID: Decimal VENDOR_ID       辅料种类代码: Decimal  CATEGORY_ID 
        /// 辅料料号: String CATEGORY_NAME    有效期天数: Decimal VALID_TIME 
        /// 暴露小时数: Decimal EXPOSE_TIME   特性: String PROPERTY_FLAG 
        /// 是否激活: String ENABLED  描述备注: String DESCRIPTION 
        /// 辅料品牌: String BRAND  辅料型号: String CATEGORY_MODEL 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery]SmtResourceCategoryRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.OBJECT_ID != null && model.OBJECT_ID > 0)
                    {
                        conditions += $"and OBJECT_ID = :OBJECT_ID ";
                    }
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(CATEGORY_NAME, :Key) > 0 or instr(CATEGORY_ID, :Key) > 0)";
                    }
                    if (!model.BRAND.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(BRAND, :Key) > 0 or instr(BRAND, :Key) > 0)";
                    }
                    if (!model.CATEGORY_MODEL.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(CATEGORY_MODEL, :Key) > 0 or instr(CATEGORY_MODEL, :Key) > 0)";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, " CATEGORY_NAME ", model)).ToList();
                    var viewList = new List<SmtResourceCategoryListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SmtResourceCategoryListModel>(x);
                        //item.Role_Name = _roleRepository.GetNameById(x.ROLE_ID);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    //TODO：总记录数
                    count = await _repository.RecordCountAsync(conditions, model);

                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(viewList);
                    returnVM.TotalCount = count;

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
		/// 导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SmtResourceCategoryRequestModel model)
        {

            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

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
        /// 当前辅料种类代码是否已被使用 
        /// </summary>
        /// <param name="CATEGORY_ID">辅料分类代码</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ItemIsByUsed(decimal CATEGORY_ID)
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
                        if (CATEGORY_ID > 0)
                        {
                            result = await _repository.ItemIsByUsed(CATEGORY_ID);
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtResourceCategoryModel model)
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
        /// 保存单个操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody]SmtResourceCategoryAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    SmtResourceCategory model;

                    #region 检查参数

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        if (item.ID == 0)
                        {
                            //TODO ADD
                            model = _mapper.Map<SmtResourceCategory>(item);
                            model.ID = await Task.Run(() => { return _repository.GetSEQ_ID(); });
                            if (await _repository.InsertAsync(model) > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                            }
                        }
                        else
                        {
                            //TODO Modify
                            model = _repository.Get(item.ID);
                            if (model != null)
                            {
                                _mapper.Map(item, model);
                                if (await _repository.UpdateAsync(model) > 0)
                                {
                                    returnVM.Result = true;
                                }
                                else
                                {
                                    returnVM.Result = false;
                                }
                            }
                            else
                            {
                                returnVM.Result = false;
                                //通用提示类的本地化问题处理
                                string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonFailNoDataCode,
                                    ResultCodeAddMsgKeys.CommonFailNoDataMsg);
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
        /// 通过ID更改激活状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeEnabled([FromBody]ChangeStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        ChangeStatusModelValidation validationRules = new ChangeStatusModelValidation(_localizer);
                        ValidationResult results = validationRules.Validate(model);
                        if (!results.IsValid)
                        {
                            ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 更改激活状态并返回

                    if (!ErrorInfo.Status)
                    {
                        //判断状态是否发生变化，没有则修改，有则返回状态已变化无法更改状态的提示
                        var isLock = await _repository.GetEnableStatus(model.Id);
                        if (isLock != model.Status)
                        {
                            var count = await _repository.ChangeEnableStatus(model.Id, model.Status);
                            if (count > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                                //通用提示类的本地化问题处理
                                string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                    ResultCodeAddMsgKeys.CommonExceptionMsg);
                                ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonDataStatusChangeCode,
                                ResultCodeAddMsgKeys.CommonDataStatusChangeMsg);
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