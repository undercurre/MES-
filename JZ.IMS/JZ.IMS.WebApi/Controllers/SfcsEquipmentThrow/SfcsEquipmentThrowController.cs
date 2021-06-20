/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-10-16 10:01:53                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsEquipmentThrowController                                      
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
using JZ.IMS.IServices;

namespace JZ.IMS.WebApi.Controllers  
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsEquipmentThrowController : BaseController
    {
        private readonly ISfcsEquipmentThrowRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsEquipmentThrowController> _localizer;
        private readonly ISfcsEquipmentLinesService _serviceLines;
        private readonly ISfcsDefectConfigRepository _serviceDeectConfig;
        public SfcsEquipmentThrowController(ISfcsEquipmentThrowRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsEquipmentThrowController> localizer, ISfcsParametersService serviceParameters, ISfcsEquipmentLinesService serviceLines,
            ISfcsDefectConfigRepository serviceDeectConfig)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _serviceLines = serviceLines;
            _serviceDeectConfig = serviceDeectConfig;
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiBaseReturn<IndexResult> Index(string ORGANIZE_ID)
        {
            ApiBaseReturn<IndexResult> returnVM = new ApiBaseReturn<IndexResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new IndexResult()
                        {
                            AllLinesList = _serviceLines.GetLinesList(ORGANIZE_ID,"SMT"),
                            DefectConfigList = _serviceDeectConfig.GetDefectConfig(),
                            SfcsEquipmentList = _repository.GetSfcsEquipmentList(),
                            //SfcsOperationSiteList = _repository.GetSfcsOperationSiteList()
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        public async Task<ApiBaseReturn<List<SfcsEquipmentThrowListModel>>> LoadData([FromQuery]SfcsEquipmentThrowRequestModel model)
        {
            ApiBaseReturn<List<SfcsEquipmentThrowListModel>> returnVM = new ApiBaseReturn<List<SfcsEquipmentThrowListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0)";
                    }
                    if (model.LINE_ID > 0)
                    {
                        conditions += $" and LINE_ID=:LINE_ID";
                    }
                    if (!model.WO_NO.IsNullOrEmpty())
                    {
                        conditions += $" and WO_NO=:WO_NO";
                    }
                    if (!model.PART_NO.IsNullOrEmpty())
                    {
                        conditions += $" and PART_NO=:PART_NO";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsEquipmentThrowListModel>();
                    var LinesList = _serviceLines.GetLinesList();
                    var SfcsEquipmentList = _repository.GetSfcsEquipmentList();
                    //var SfcsOperationSiteList = _repository.GetSfcsOperationSiteList();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsEquipmentThrowListModel>(x);
                        item.LINE_NAME = LinesList.FirstOrDefault(m => m.ID == x.LINE_ID).LINE_NAME;
                        //item.OPERATION_SITE_NAME = SfcsOperationSiteList.FirstOrDefault(m => m.ID == x.SITE_ID).OPERATION_SITE_NAME;
                        item.EQUIPMENT_NAME = SfcsEquipmentList.FirstOrDefault(m => m.ID == x.EQUIPMENT_ID).NAME;
                        item.TIME_SLOT = x.START_TIME + "-" + x.END_TIME;
                        item.THROW_RATE = Math.Round((x.THROW_QTY / x.TARGET_QTY) * 100, 2).ToString() + "%";
                        item.THROW_DATE = x.THROW_DATE.ToString("yyyy/MM/dd");
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsEquipmentThrowModel model)
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>	
        [HttpGet]
        public ApiBaseReturn<ReturnPartAndModel> GetModelAndPartByWoNo(string WO_NO)
        {
            ApiBaseReturn<ReturnPartAndModel> returnVM = new ApiBaseReturn<ReturnPartAndModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = _repository.GetModelAndPartByWoNo(WO_NO);
                    returnVM.Result = data;
                    returnVM.TotalCount = 1;

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
        public ApiBaseReturn<List<ReturnReportData>> LoadReportData([FromQuery]SfcsEquipmentThrowRequestModel model)
        {
            ApiBaseReturn<List<ReturnReportData>> returnVM = new ApiBaseReturn<List<ReturnReportData>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    string conditions = " WHERE A.ID > 0 ";
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0)";
                    }
                    if (model.LINE_ID > 0)
                    {
                        conditions += $" and LINE_ID=:LINE_ID";
                    }
                    var list = _repository.GetReturnReportDatas(model, conditions);

                    returnVM.Result = list;
                    returnVM.TotalCount = list.Count;

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
        /// 首页返回类
        /// </summary>
        public class IndexResult
        {
            /// <summary>
            /// 所有线别列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsEquipmentLinesModel> LinesList { get; set; }
            /// <summary>
            /// 不良信息列表
            /// </summary>
            public List<SfcsDefectConfig> DefectConfigList { get; set; }
            public List<AllLinesModel> AllLinesList { get; set; }
            public List<SfcsEquipmentList> SfcsEquipmentList { get; set; }

        }
    }
}