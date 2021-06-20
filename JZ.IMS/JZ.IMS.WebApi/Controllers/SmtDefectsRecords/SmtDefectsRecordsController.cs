/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-10-15 10:41:26                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtDefectsRecordsController                                      
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
	public class SmtDefectsRecordsController : BaseController
	{
		private readonly ISmtDefectsRecordsRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SmtDefectsRecordsController> _localizer;
        private readonly ISfcsParametersService _serviceParameters;
        private readonly ISfcsEquipmentLinesService _serviceLines;
        private readonly ISfcsDefectConfigRepository _serviceDeectConfig;

        public SmtDefectsRecordsController(ISmtDefectsRecordsRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SmtDefectsRecordsController> localizer, ISfcsParametersService serviceParameters, ISfcsEquipmentLinesService serviceLines,
            ISfcsDefectConfigRepository serviceDeectConfig)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
            _serviceParameters = serviceParameters;
            _localizer = localizer;
            _serviceLines = serviceLines;
            _serviceDeectConfig = serviceDeectConfig;

        }

		/// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiBaseReturn<IndexResult> Index()
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
                            LinesList = _serviceLines.GetSMTLinesList(),
                            DefectConfigList =  _serviceDeectConfig.GetDefectConfig()
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
        public async Task<ApiBaseReturn<List<SmtDefectsRecordsListModel>>> LoadData([FromQuery]SmtDefectsRecordsRequestModel model)
        {
            ApiBaseReturn<List<SmtDefectsRecordsListModel>> returnVM = new ApiBaseReturn<List<SmtDefectsRecordsListModel>>();
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
                    if (!model.LINE_ID.IsNullOrWhiteSpace() && model.LINE_ID > 0)
                    {
                        conditions += $" and LINE_ID=:LINE_ID";
                    }
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $" and PART_NO LIKE '%'||:PART_NO||'%'";
                    }
                    if (!model.WO_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $" and WO_NO LIKE '%'||:WO_NO||'%'";
                    }
                    if (!model.STATUS.IsNullOrWhiteSpace())
                    {
                        conditions += $" and STATUS = :STATUS";
                    }
                    if (model.START_TIME != null)
                    {
                        conditions += $" and REPAIR_TIME>=:START_TIME";
                    }
                    if (model.END_TIME != null)
                    {
                        conditions += $" and REPAIR_TIME<=:END_TIME";
                    }

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var lineList = _serviceLines.GetVMesLinesList();
                    var defectList = _serviceDeectConfig.GetDefectConfig();  
                    var viewList = new List<SmtDefectsRecordsListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SmtDefectsRecordsListModel>(x);
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
        public async Task<ApiBaseReturn<decimal>> SaveData([FromBody] SmtDefectsRecordsModel model)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
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
                        returnVM.Result = resdata;
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
        /// 取消审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="examineUser">维修人</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<bool>> CancelCheck(int id,string examineUser)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var check = await _repository.CancelCheck(id, examineUser);
                    returnVM.Result = check > 0;

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
        /// 审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="examineUser">审核人</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<bool>> Check(int id, string examineUser)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var repair = await _repository.Check(id, examineUser);
                    returnVM.Result = repair > 0;

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
        /// 获取线体工单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<SmtWo>> GetWoInfoByLine(string lineId)
        {
            ApiBaseReturn<SmtWo> returnVM = new ApiBaseReturn<SmtWo>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = await _repository.GetWoInfoByLine(lineId);
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
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        public async Task<ApiBaseReturn<dynamic>> LoadReportData([FromQuery]SmtDefectsRecordsRequestModel model)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    string conditions = " WHERE A.ID > 0 ";
                    if (!model.LINE_ID.IsNullOrWhiteSpace() && model.LINE_ID > 0)
                    {
                        conditions += $" and LINE_ID=:LINE_ID";
                    }
                    if (!model.WORK_CLASS.IsNullOrWhiteSpace())
                    {
                        conditions += $" and WORK_CLASS=:WORK_CLASS";
                    }

                    var list = await _repository.GetReportDefectsRecordsList(conditions, model);

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


        /// <summary>SaveDataByTrans
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        public async Task<ApiBaseReturn<dynamic>> GetReportDefectsRecordDtlList([FromQuery] SmtDefectsRecordDtlRequestModel model)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var list = await _repository.GetReportDefectsRecordDtlList(model);

                    returnVM.Result = list;
                    returnVM.TotalCount = 0;

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
        /// 明细保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiBaseReturn<bool>> SaveData_DTL([FromBody] SmtDefectsRecordDtlModel model)
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
                        decimal resdata = await _repository.SaveReportDefectsRecordDtl(model);
                        returnVM.Result = resdata==-1?false:true;
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

        }
    }
}