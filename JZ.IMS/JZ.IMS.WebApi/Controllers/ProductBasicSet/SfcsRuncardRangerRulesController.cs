/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-11 10:06:08                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsRuncardRangerRulesController                                      
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
using JZ.IMS.WebApi.Common;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 流水号范围规则设定 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsRuncardRangerRulesController : BaseController
    {
        private readonly ISfcsRuncardRangerRulesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsRuncardRangerRulesController> _localizer;

        public SfcsRuncardRangerRulesController(ISfcsRuncardRangerRulesRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsRuncardRangerRulesController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 
        /// </summary>
        public class IndexVM
        {
            /// <summary>
            /// 客户列表
            /// </summary>
            public List<dynamic> CustomList { get; set; }

            /// <summary>
            /// 进制列表
            /// </summary>
            public List<dynamic> DigitalList { get; set; }
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
                            CustomList = await _repository.GetListByTable("ID, CUSTOMER", "SFCS_CUSTOMERS", "Order by CUSTOMER desc "),
                            DigitalList = await _repository.GetListByTable("LOOKUP_CODE,MEANING,DESCRIPTION", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'RADIX_TYPE' ORDER BY MEANING"),
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
        public async Task<ApiBaseReturn<IEnumerable<SfcsRuncardRangerRulesListModel>>> LoadData([FromQuery]SfcsRuncardRangerRulesRequestModel model)
        {
            ApiBaseReturn<IEnumerable<SfcsRuncardRangerRulesListModel>> returnVM = new ApiBaseReturn<IEnumerable<SfcsRuncardRangerRulesListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.LoadData(model);
                    returnVM.TotalCount = await _repository.LoadDataCount(model);

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
        /// 校验用户录入数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> VerifyRangerRulesData([FromQuery]SfcsRuncardRangerRulesEditModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            #region 变量定义
            //const decimal SetupByPlatform = 0;
            const decimal SetupByFamily = 0;
            const decimal SetupByCustomer = 1;
            const decimal SetupByPartNO = 2;
            #endregion

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        if (model.RangerRuleType == SetupByFamily)
                        {
                            if (model.PRODUCT_FAMILY_ID < 1)
                            {
                                //请选择一个设定流水范围的条件。
                                ErrorInfo.Set(_localizer["Err_NotSelectRuncardRangerRuleSetby"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            // 一個PRODUCT_FAMILY只能配置一筆
                            var tmpdata = await _repository.ItemIsExist("SFCS_RUNCARD_RANGER_RULES", "RULE_TYPE = '" + model.RULE_TYPE + "' AND PRODUCT_FAMILY_ID", model.PRODUCT_FAMILY_ID.ToString());
                            if (tmpdata)
                            {
                                //此流水号范围规则已存在，请检查。
                                ErrorInfo.Set(_localizer["Err_RuncardRangerRuleDuplicate"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else if (model.RangerRuleType == SetupByCustomer)
                        {
                            if (model.CUSTOMER_ID == null || model.CUSTOMER_ID < 0)
                            {
                                ErrorInfo.Set(_localizer["Err_NotSelectRuncardRangerRuleSetby"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            // 一個Customer只能配置一筆
                            var tmpdata = await _repository.ItemIsExist("SFCS_RUNCARD_RANGER_RULES", "RULE_TYPE = '" + model.RULE_TYPE + "' AND CUSTOMER_ID", model.CUSTOMER_ID.ToString());
                            if (tmpdata)
                            {
                                //此流水号范围规则已存在，请检查。
                                ErrorInfo.Set(_localizer["Err_RuncardRangerRuleDuplicate"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else if (model.RangerRuleType == SetupByPartNO)
                        {
                            if (model.PART_NO.IsNullOrWhiteSpace())
                            {
                                //请录入成品料号。
                                ErrorInfo.Set(_localizer["Err_PART_NO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            // 检查料号
                            var tmpdata = await _repository.ItemIsExist("SFCS_PN", "PART_NO", model.PART_NO);
                            if (!tmpdata&& model.PART_NO != "000000")
                            {
                                //料号{0}在系统中不存在，请检查。
                                string errrmsg = string.Format(_localizer["Err_PN_Not_Found"], model.PART_NO);
                                ErrorInfo.Set(errrmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            //一個Part NO只能配置一筆
                            tmpdata = await _repository.ItemIsExist("SFCS_RUNCARD_RANGER_RULES", "RULE_TYPE = '" + model.RULE_TYPE + "' AND PART_NO", model.PART_NO);
                            if (tmpdata)
                            {
                                //此流水号范围规则已存在，请检查。
                                ErrorInfo.Set(_localizer["Err_RuncardRangerRuleDuplicate"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        //else if (model.RangerRuleType == SetupByPlatform)
                        //{
                        //    if (model.PLATFORM_ID == null || model.PLATFORM_ID < 0)
                        //    {
                        //        //请选择一个设定流水范围的条件。
                        //        ErrorInfo.Set(_localizer["Err_NotSelectRuncardRangerRuleSetby"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        //    }
                        //    // 一個Platform只能配置一筆
                        //    var tmpdata = await _repository.ItemIsExist("SFCS_RUNCARD_RANGER_RULES", "RULE_TYPE = '" + model.RULE_TYPE + "' AND PLATFORM_ID", model.PLATFORM_ID.ToString());
                        //    if (tmpdata)
                        //    {
                        //        //此流水号范围规则已存在，请检查。
                        //        ErrorInfo.Set(_localizer["Err_RuncardRangerRuleDuplicate"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        //    }
                        //}
                    }

                    #endregion

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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsRuncardRangerRulesAddOrModifyModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            string errmsg = string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.FIX_HEADER.IsNullOrWhiteSpace() && model.FIX_TAIL.IsNullOrWhiteSpace())
                    {
                        //throw new MESException(Properties.Resources.Err_NotInputColumn, this.colFIX_HEADER.Caption + "OR" + this.colFIX_TAIL.Caption);
                        //errmsg = string.Format("请输入前导符或结束符。");
                        ErrorInfo.Set(_localizer["Err_NotInput_FIX_HEADER_TAIL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && !model.FIX_HEADER.IsNullOrWhiteSpace())
                    {
                        model.FIX_HEADER = model.FIX_HEADER.Trim();
                    }
                    if (!ErrorInfo.Status && !model.FIX_TAIL.IsNullOrWhiteSpace())
                    {
                        model.FIX_TAIL = model.FIX_TAIL.Trim();
                    }
                    if (!ErrorInfo.Status && model.RANGE_START_CODE.IsNullOrWhiteSpace())
                    {
                        //"请输入流水范围开始字符。"
                        ErrorInfo.Set(_localizer["Err_Not_RANGE_START_CODE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        model.RANGE_START_CODE = model.RANGE_START_CODE.Trim();
                    }

                    if (!ErrorInfo.Status && model.RANGE_LENGTH != model.RANGE_START_CODE.Length)
                    {
                        //起始码字符长度与规定的长度不一致，请检查。
                        ErrorInfo.Set(_localizer["Err_RangeLengthRangeStartCodeNotMatch"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.DIGITAL == null && model.DIGITAL <= 0)
                    {
                        //"请输入进制。"
                        ErrorInfo.Set(_localizer["Err_NotInput_DIGITAL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        string conditions = "WHERE LOOKUP_TYPE='RADIX_EXCLUSIVE' AND MEANING=:MEANING ORDER BY LOOKUP_TYPE";
                        var radixExclusiveRow = await _repository.GetAsyncEx<SfcsParameters>(conditions, new { MEANING = model.DIGITAL_Caption });
                        //除36進制外其余進制必須設定RADIX_EXCLUSIVE
                        if (radixExclusiveRow == null)
                        {
                            if (model.DIGITAL != 1)
                            {
                                //"找不到进制排除字符 {0}。"
                                errmsg = string.Format(_localizer["Err_CanNotFoundTheSettingOfRadixExclusive"], model.DIGITAL);
                                ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else
                        {
                            model.EXCLUSIVE_CHAR = radixExclusiveRow.DESCRIPTION;
                        }

                        conditions = "WHERE LOOKUP_TYPE='RADIX_TYPE' AND LOOKUP_CODE=:DIGITAL ORDER BY LOOKUP_TYPE";
                        var radixTypeRow = await _repository.GetAsyncEx<SfcsParameters>(conditions, new { model.DIGITAL });
                        if (radixTypeRow != null)
                        {
                            string expression = "^[" + radixTypeRow.DESCRIPTION + "]+$";
                            if (!SFCSExpression.IsMatch(model.RANGE_START_CODE, expression))
                            {
                                //<Range Start Code>不能匹配格式，不允许存在{0}字符。
                                errmsg = string.Format(_localizer["Err_RangeStartCodeFormatNotMatch"], radixExclusiveRow.DESCRIPTION);
                                ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

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

    }
}