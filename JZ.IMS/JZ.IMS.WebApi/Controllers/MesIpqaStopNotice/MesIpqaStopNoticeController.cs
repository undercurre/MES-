/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：制程品质异常停线通知单表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-11-02 11:09:31                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesIpqaStopNoticeController                                      
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
using System.Reflection;
using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using JZ.IMS.Core;
using JZ.IMS.WebApi.Public;

namespace JZ.IMS.WebApi.Controllers  
{
    /// <summary>
    /// 品质停线通知单 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class MesIpqaStopNoticeController : BaseController
	{
		private readonly IMesIpqaStopNoticeRepository _repository;
		private readonly ISysDepartmentRepository _depRepository;
        private readonly ISfcsParametersRepository _parameRepository;
		private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<MesIpqaStopNoticeController> _localizer;
		
		public MesIpqaStopNoticeController(IMesIpqaStopNoticeRepository repository, ISysDepartmentRepository depRepository, ISfcsParametersRepository parameRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<MesIpqaStopNoticeController> localizer)
		{
			_repository = repository;
            _depRepository = depRepository;
            _parameRepository = parameRepository;
            _mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}

		/// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
		[AllowAnonymous]
        public async Task<ApiBaseReturn<List<MesIpqaStopNoticeListModel>>> LoadData([FromQuery]MesIpqaStopNoticeRequestModel model)
        {
            ApiBaseReturn<List<MesIpqaStopNoticeListModel>> returnVM = new ApiBaseReturn<List<MesIpqaStopNoticeListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _repository.LoadDataPagedList(model);
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

        ///// <summary>
        ///// 当前ID是否已被使用 
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns></returns>
        //[HttpGet]
        //[Authorize]
        //public async Task<ApiBaseReturn<bool>> ItemIsByUsed(decimal id)
        //{
        //    ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
        //    bool result = false;

        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 设置返回值

        //            if (!ErrorInfo.Status)
        //            {
        //                if (id > 0)
        //                {
        //                    result = await _repository.ItemIsByUsed(id);
        //                }
        //                returnVM.Result = result;
        //                returnVM.TotalCount = 1;
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

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
		[AllowAnonymous]
        public async Task<ApiBaseReturn<decimal>> SaveData([FromBody] MesIpqaStopNoticeModel model)
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
                        if (resdata > 0)
                        {
                            returnVM.Result = resdata;
                        }
                        else
                        {
                            returnVM.Result = 0;
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
		[AllowAnonymous]
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
		[AllowAnonymous]
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

                    #region 验证数据
                    if (!ErrorInfo.Status)
                    {
                        var iStatus = await _repository.GetBillStatus(id);
                        if (iStatus > (decimal)MesIpqaStopNoticeStatus.Edit)
                        {
                            //非拟制状态不能删除。
                            ErrorInfo.Set(_localizer["delete_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion


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
        /// 查询线体数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<List<MesIpqaStopNoticeLinesResultModel>>> GetLinesList([FromQuery] MesIpqaStopNoticeLinesRequestModel model)
        {
            ApiBaseReturn<List<MesIpqaStopNoticeLinesResultModel>> returnVM = new ApiBaseReturn<List<MesIpqaStopNoticeLinesResultModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _repository.GetLinesList(model.ORGANIZE_ID, "SMT");
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
        /// 查询部门数据
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<List<SysDepartmentListModel>>> GetDepartmentList()
        {
            ApiBaseReturn<List<SysDepartmentListModel>> returnVM = new ApiBaseReturn<List<SysDepartmentListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 AND ENABLED = 'Y' ";
                    var list = (await _depRepository.GetListAsync(conditions)).ToList();
                    var viewList = new List<SysDepartmentListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SysDepartmentListModel>(x);
                        viewList.Add(item);
                    });

                    count = await _depRepository.RecordCountAsync(conditions);

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
		/// 获取当前线别在线工单
		/// </summary>
		/// <param name="line_id">线别ID（必填）</param>
		/// <returns></returns>
		[HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<string>> GetWoNoByLineId(decimal line_id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && line_id <= 0)
                    {
                        // 线别ID不能为空。
                        ErrorInfo.Set(_localizer["LINE_ID_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.GetWoNoByLineId(line_id);

                        returnVM.Result = result;
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
		/// 根据工单号获取产品信息
		/// </summary>
		/// <param name="wo_no">工单号</param>
		/// <returns></returns>
		[HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<PartModel>> GetPartDataByWoNo(string wo_no)
        {
            ApiBaseReturn<PartModel> returnVM = new ApiBaseReturn<PartModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && wo_no.IsNullOrWhiteSpace())
                    {
                        // 工单号不能为空。
                        ErrorInfo.Set(_localizer["WO_NO_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.GetPartDataByWoNo(wo_no);
                        var viewModel = _mapper.Map<PartModel>(result);

                        returnVM.Result = viewModel;
                        returnVM.TotalCount = viewModel == null ? 0 : 1;
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
        /// 查询原因分析数据
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<List<SfcsParametersListModel>>> GetAnalysisOpinionList()
        {
            ApiBaseReturn<List<SfcsParametersListModel>> returnVM = new ApiBaseReturn<List<SfcsParametersListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 AND ENABLED = 'Y' AND LOOKUP_TYPE = 'IPQA_STOP_REASON' ";
                    
                    var list = (await _parameRepository.GetListAsync(conditions)).ToList();
                    var viewList = new List<SfcsParametersListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsParametersListModel>(x);
                        viewList.Add(item);
                    });

                    count = await _parameRepository.RecordCountAsync(conditions);

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
		/// 审核
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<bool>> AuditBill([FromBody] MesIpqaStopNoticeAuditBillRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.ID <= 0)
                    {
                        // ID不能为空。
                        ErrorInfo.Set(_localizer["ID_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model.AUDIT_USER))
                    {
                        // 审核人不能为空。
                        ErrorInfo.Set(_localizer["AUDIT_USER_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = new BaseResult();
                        var billStatus = await _repository.GetBillStatus(model.ID);
                        if (!ErrorInfo.Status && billStatus != (decimal)MesIpqaStopNoticeStatus.Edit)
                        {
                            // 单据不是拟制状态, 不允许审核。
                            ErrorInfo.Set(_localizer["AuditBill_Status_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.AuditBill(model);
                        if (resdata > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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
		/// 批准
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<bool>> ApprovalBill([FromBody] MesIpqaStopNoticeApprovalBillRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.ID <= 0)
                    {
                        // ID不能为空。
                        ErrorInfo.Set(_localizer["ID_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model.APPROVAL_USER))
                    {
                        // 批准人不能为空。
                        ErrorInfo.Set(_localizer["APPROVAL_USER_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var result = new BaseResult();
                        var billStatus = await _repository.GetBillStatus(model.ID);
                        if (!ErrorInfo.Status && billStatus != (decimal)MesIpqaStopNoticeStatus.Audit)
                        {
                            // 单据不是审核状态, 不允许批准。
                            ErrorInfo.Set(_localizer["ApprovalBill_Status_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.ApprovalBill(model);
                        if (resdata > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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

    /// <summary>
    /// 停线通知单状态枚举
    /// </summary>
    public enum MesIpqaStopNoticeStatus
    {
        /// <summary>
        /// 拟制
        /// </summary>
        Edit = 0,

        /// <summary>
        /// 已审核
        /// </summary>
        Audit = 1,

        /// <summary>
        /// 已批准
        /// </summary>
        Approval = 2
    }
}