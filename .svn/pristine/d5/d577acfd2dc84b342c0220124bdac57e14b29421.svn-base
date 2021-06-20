/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫内容配置 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-08-12 11:10:38                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IAndonCallContentConfigController                                      
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
    /// 异常内容配置 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class AndonCallContentConfigController : BaseController
    {
        private readonly IAndonCallContentConfigRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<AndonCallContentConfigController> _localizer;

        public AndonCallContentConfigController(IAndonCallContentConfigRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<AndonCallContentConfigController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }


        public class IndexVM
        {
            /// <summary>
            /// 异常种类
            /// </summary>
            public List<dynamic> CALL_CATEGORY_CODE { get; set; }

            /// <summary>
            /// 异常类型
            /// </summary>
            public List<dynamic> CALL_TYPE_CODE { get; set; }

        }

        /// <summary>
        /// 首页视图(获取异常种类、异常类型下拉框值)
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
                        returnVM.Result = new IndexVM()
                        {
                            CALL_CATEGORY_CODE = await _repository.GetListByTable("LOOKUP_CODE,DESCRIPTION", "SFCS_PARAMETERS", "and LOOKUP_TYPE='ALARM_CATEGORY' and ENABLED='Y'"),
                            CALL_TYPE_CODE = await _repository.GetListByTable("LOOKUP_CODE,DESCRIPTION", "SFCS_PARAMETERS", "and LOOKUP_TYPE='ALARM_TYPE' and ENABLED='Y'"),
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
        /// 获取异常种类、异常类型下拉框值
        /// 异常种类:0:安灯 1:停线管控
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<ParamsOfCallCode>> GetParamsOfCallCode()
        {
            ApiBaseReturn<ParamsOfCallCode> returnVM = new ApiBaseReturn<ParamsOfCallCode>();
            var paramsOfCallCode = new ParamsOfCallCode();
            paramsOfCallCode.ANDON_ERROR = new CallCodeModel();
            paramsOfCallCode.STOP_LINE_ERROR = new CallCodeModel();
            returnVM.Result = paramsOfCallCode;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var cALLCategoryCList = await _repository.GetListByTableEX<SfcsParameters>("LOOKUP_CODE,DESCRIPTION", "SFCS_PARAMETERS", " and LOOKUP_TYPE='ALARM_CATEGORY' and ENABLED='Y'");
                        var typeCodeItemList = await _repository.GetListByTableEX<TypeCodeItem>("NAME,LOOKUP_CODE,DESCRIPTION", "SFCS_PARAMETERS", "and LOOKUP_TYPE='ALARM_TYPE' and ENABLED='Y'");
                        if (cALLCategoryCList == null || cALLCategoryCList.Count <= 0 || typeCodeItemList == null || typeCodeItemList.Count <= 0)
                        {
                            return returnVM;
                        }
                        paramsOfCallCode.ANDON_ERROR.CATEGORY_CODE = cALLCategoryCList.FirstOrDefault(c => c.LOOKUP_CODE == 0).LOOKUP_CODE;
                        paramsOfCallCode.STOP_LINE_ERROR.CATEGORY_CODE = cALLCategoryCList.FirstOrDefault(c => c.LOOKUP_CODE == 1).LOOKUP_CODE;

                        paramsOfCallCode.ANDON_ERROR.TYPE_CODE_List = typeCodeItemList.Where(c => Convert.ToDecimal(c.NAME) == 0)?.ToList();
                        paramsOfCallCode.STOP_LINE_ERROR.TYPE_CODE_List = typeCodeItemList.Where(c => Convert.ToDecimal(c.NAME) == 1)?.ToList();
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
        public async Task<ApiBaseReturn<List<AndonCallContentConfigListModel>>> LoadData([FromQuery] AndonCallContentConfigRequestModel model)
        {
            ApiBaseReturn<List<AndonCallContentConfigListModel>> returnVM = new ApiBaseReturn<List<AndonCallContentConfigListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.ID.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (ID =:ID)";
                    }
                    if (!model.CALL_TITLE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(CALL_TITLE, :CALL_TITLE) > 0 )";
                    }
                    if (!model.CALL_CATEGORY_CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (CALL_CATEGORY_CODE =:CALL_CATEGORY_CODE)";
                    }
                    if (!model.CALL_TYPE_CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (CALL_TYPE_CODE =:CALL_TYPE_CODE)";
                    }
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (ENABLED =:ENABLED)";
                    }
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(DESCRIPTION, :Key) > 0 or instr(CHINESE, :Key) > 0)";
                    }

                    var ALLCategoryCList = await _repository.GetListByTableEX<SfcsParameters>("LOOKUP_CODE,DESCRIPTION", "SFCS_PARAMETERS", " and LOOKUP_TYPE='ALARM_CATEGORY' and ENABLED='Y'");
                    var typeCodeItemList = await _repository.GetListByTableEX<SfcsParameters>("NAME,LOOKUP_CODE,DESCRIPTION", "SFCS_PARAMETERS", "and LOOKUP_TYPE='ALARM_TYPE' and ENABLED='Y'");
                    if (ALLCategoryCList == null || ALLCategoryCList.Count <= 0 || typeCodeItemList == null || typeCodeItemList.Count <= 0)
                    {
                        return returnVM;
                    }

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<AndonCallContentConfigListModel>();

                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<AndonCallContentConfigListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        item.CALL_CATEGORY_NAME = ALLCategoryCList.Where(c => c.LOOKUP_CODE == Convert.ToDecimal(item.CALL_CATEGORY_CODE)).FirstOrDefault()?.DESCRIPTION;
                        item.CALL_TYPE_NAME = typeCodeItemList.Where(c => c.NAME == item.CALL_CATEGORY_CODE&& c.LOOKUP_CODE == Convert.ToDecimal(item.CALL_TYPE_CODE)).FirstOrDefault()?.DESCRIPTION;
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsync(conditions, model);
                    returnVM.Result = viewList;
                    returnVM.TotalCount = count;

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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] AndonCallContentConfigModel model)
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

            WriteLog(ref returnVM);

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
        public async Task<ApiBaseReturn<bool>> ChangeEnabled([FromBody] ChangeStatusModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
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


        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery] AndonCallContentConfigRequestModel model)
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

        #region 内部类
        public class ParamsOfCallCode
        {
            public CallCodeModel ANDON_ERROR { get; set; }
            public CallCodeModel STOP_LINE_ERROR { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class CallCodeModel
        {
            public decimal CATEGORY_CODE { get; set; }
            public List<TypeCodeItem> TYPE_CODE_List { get; set; }
        }
        /// <summary>
        /// 详细的项
        /// </summary>
        public class TypeCodeItem
        {
            /// <summary>
            /// 异常类型
            /// </summary>
            public string NAME { get; set; }
            public decimal LOOKUP_CODE { get; set; }
            public string DESCRIPTION { get; set; }
        }
        #endregion







    }

}