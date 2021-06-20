/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:48                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsProductConfigController                                      
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
using JZ.IMS.ViewModels.ProductBasicSet.ComponentReplace;
using JZ.IMS.Core.Utilities;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 产品配置控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsProductConfigController : BaseController
    {
        private readonly ISfcsProductConfigRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsProductConfigController> _localizer;

        public SfcsProductConfigController(ISfcsProductConfigRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsProductConfigController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class VM
        {
            /// <summary>
            /// 配置类型
            /// </summary>
           // public List<dynamic> ConfigTypeList { get; set; }
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<VM>> Index()
        {
            ApiBaseReturn<VM> returnVM = new ApiBaseReturn<VM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new VM
                        {
                            //ConfigTypeList = await _repository.GetListByTable(" LOOKUP_CODE, CHINESE, ENABLED ", "SFCS_PARAMETERS", " AND LOOKUP_TYPE = 'PRODUCT_CONFIG_TYPE' AND ENABLED = 'Y' "),
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
        /// 查配置类型的数据(如需查询请使用TypeCode,其他值不用传)
        /// 搜索按钮对应的处理也是这个方法
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetConfigTypeList([FromQuery]SfcsProductConfigRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 AND LOOKUP_TYPE = 'PRODUCT_CONFIG_TYPE' AND ENABLED = 'Y'  ";

                    if (!model.ChineseName.IsNullOrWhiteSpace())
                    {
                        conditions += $"and INSTR(UPPER(CHINESE),UPPER(:ChineseName),1,1)>0  ";
                    }

                    var list = (await _repository.GetListPagedEx<SfcsParametersListModel>(model.Page, model.Limit, conditions, "CHINESE asc", model)).ToList();
                    var viewList = new List<SfcsParametersListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsParametersListModel>(x);
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsyncEx<SfcsParametersListModel>(conditions, model);
                    returnVM.Result = viewList.Where(c=>!c.CHINESE.IsNullOrEmpty()).Select(c => new { c.CHINESE, c.LOOKUP_CODE, c.ENABLED }).ToList<object>();
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadData([FromQuery]SfcsProductConfigRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var result = await _repository.GetProductConfig(model);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }


        /// <summary>
        /// 根据料号和选择的配置类型判断是否存在重复的数据
        /// </summary>
        /// <param name="partno"></param>
        /// <param name="configtype"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ConfigTypeIsExistByPartNo(string partno,decimal? configtype)
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
                        if (!partno.IsNullOrWhiteSpace() )
                        {
                            result = await _repository.ConfigTypeIsExistByPartNo(partno,configtype??0);
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
        /// 导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SfcsProductConfigRequestModel model)
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsProductConfigModel model)
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
        /// 获取零件信息 零件规格:DESCRIPTION }
        /// </summary>
        /// <param name="partNo">成品料号</param>
        /// <param name="odmComponentPN">零件料号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<String>> GetComponentInfomation(string partNo)
        {
            ApiBaseReturn<String> returnVM = new ApiBaseReturn<String>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检验参数
                    
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status&&!partNo.IsNullOrWhiteSpace())
                    {
                      returnVM.Result= (await _repository.GetDataByOldComponents(partNo)).FirstOrDefault().DESCRIPTION;
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
        /// 获取零件替换信息
        /// REPLACE_COMPONENT_ID:主键
        /// NEW_ODM_COMPONENT_PN(新零件料号)
        /// OLD_ODM_COMPONENT_PN(旧零件料号)
        /// NEW_ODM_COMPONENT_SN 新零件编号 
        /// OLD_ODM_COMPONENT_SN 旧零件编号 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsComponentReplace>>> GetComponentsReplace([FromQuery] string newComponentSN = "",string oldComponentPN="")
        {
            ApiBaseReturn<List<SfcsComponentReplace>> returnVM = new ApiBaseReturn<List<SfcsComponentReplace>>();
            try
            {
                string conditions = "";

                if (!oldComponentPN.IsNullOrWhiteSpace())
                {
                    conditions += " AND OLD_ODM_COMPONENT_PN=:OldODMComponentPN ";
                }
                if (!newComponentSN.IsNullOrWhiteSpace())
                {
                    conditions += " AND NEW_ODM_COMPONENT_PN=:NewODMComponentSN ";
                }

                returnVM.Result = (await _repository.GetListByTableEX<SfcsComponentReplace>("REPLACE_COMPONENT_ID,NEW_ODM_COMPONENT_PN,OLD_ODM_COMPONENT_PN,NEW_ODM_COMPONENT_SN,OLD_ODM_COMPONENT_SN,REPLACE_BY,REPLACE_TIME", "SFCS_COMPONENT_REPLACE", conditions,new {
                    OldODMComponentPN = oldComponentPN,
                    NewODMComponentSN= newComponentSN
                }))?.ToList();
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }
            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 零件替换保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveComponentReplace([FromBody] SfcsReplaceModel<ComponentReplaceViewModel> model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检验参数
                    var count = 0;

                    if (!ErrorInfo.Status && model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        count = model.InsertRecords.Count(c => c.OldODMComponentPn.IsNullOrWhiteSpace() || c.NewODMComponentPn.IsNullOrWhiteSpace() || c.NewODMComponentSn.IsNullOrWhiteSpace());
                    }

                    if (!ErrorInfo.Status && count==0&& model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        count = model.InsertRecords.Count(c => c.OldODMComponentPn.IsNullOrWhiteSpace() || c.NewODMComponentPn.IsNullOrWhiteSpace() || c.NewODMComponentSn.IsNullOrWhiteSpace());
                    }
                    if (count > 0)
                    {
                        //请输入新旧零件料号、新零件编号等信息。
                        throw new Exception(_localizer["Please_NewComponents_InforMation"]);
                    }
                   
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.SaveDataByOldComponents(model);
                        returnVM.Result = result == 1 ? true : false;
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