using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using FluentValidation.Results;
using JZ.IMS.IRepository;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.Extensions.Localization;
using JZ.IMS.Models;
using JZ.IMS.Core.Extensions;
using System.Net;
using System.IO;
using System.Text;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// (QA/OQA检验)抽检报告管理 控制器  
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesSpotcheckHeaderController : BaseController
    {
        private readonly IMesSpotcheckHeaderService _service;
        private readonly IMesSpotcheckHeaderRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MesSpotcheckHeaderController> _localizer;

        public MesSpotcheckHeaderController(IMesSpotcheckHeaderService service, IMesSpotcheckHeaderRepository repository,
            IHttpContextAccessor httpContextAccessor, IStringLocalizer<MesSpotcheckHeaderController> localizer)
        {
            _localizer = localizer;
            _service = service;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// QA/OQA检验 首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<List<LineModel>> Index()
        {
            ApiBaseReturn<List<LineModel>> returnVM = new ApiBaseReturn<List<LineModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetLineDataAll();
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
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery] MesSpotcheckHeaderRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetHeaderDataList(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                    returnVM.TotalCount = await _repository.GetHeaderDataCountEx(model);

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
        /// 查询质检列表
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadDataTwo([FromQuery] MesSpotcheckHeaderRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetHeaderDataTwoList(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                    returnVM.TotalCount = await _repository.GetHeaderDataCountTwo(model);

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
        /// 根据抽检批次号获取抽检数据
        /// </summary>
        /// <param name="batchNo">抽检批次号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SpotcheckDetailListModel>> GetSpotcheckDetailByBatchNo(string batchNo)
        {
            ApiBaseReturn<SpotcheckDetailListModel> returnVM = new ApiBaseReturn<SpotcheckDetailListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 参数验证
                    if (string.IsNullOrEmpty(batchNo) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["BATCH_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetSpotcheckDetailByBatchNo(batchNo);
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
        /// 根据抽检批次号获取质检项目
        /// </summary>
        /// <param name="batchNo">抽检批次号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesSpotcheckIteamsListModel>>> GetSpotcheckIteamsByBatchNo(string batchNo)
        {
            ApiBaseReturn<List<MesSpotcheckIteamsListModel>> returnVM = new ApiBaseReturn<List<MesSpotcheckIteamsListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 参数验证
                    if (string.IsNullOrEmpty(batchNo) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["BATCH_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetListByTableEX<MesSpotcheckIteamsListModel>("*", "MES_SPOTCHECK_ITEAMS", " And BATCH_NO =:BATCH_NO ORDER BY ID ASC", new { BATCH_NO = batchNo });
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
        /// 根据抽检批次号获取抽检不良明细数据
        /// </summary>
        /// <param name="model">Key:抽检批次号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetSpotcheckFailDetail(PageModel model)
        {
            MesSpotcheckHeader header = null;
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 参数验证

                    if (string.IsNullOrEmpty(model.Key) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["BATCH_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        header = (await _repository.GetListByTableEX<MesSpotcheckHeader>("*", "MES_SPOTCHECK_HEADER", " AND BATCH_NO=:BATCH_NO", new { BATCH_NO = model.Key })).FirstOrDefault();
                    }

                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetSpotcheckFailDetail(model);
                        if (resdata.Count() > 0 && header != null && header.QC_TYPE == 2)
                        {
                            foreach (var item in resdata)
                            {
                                if (String.IsNullOrEmpty(item.SITE_NAME) || item.SITE_ID < 1)
                                {
                                    item.STATUS = "";
                                    item.DEFECT_CODE = "";
                                    item.DEFECT_LOC = "";
                                    item.DEFECT_DESCRIPTION = "";
                                    item.DEFECT_MSG = "";
                                }
                            }
                        }
                        returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                        returnVM.TotalCount = await _repository.GetFailDetailDataCount(model);
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
        /// 获取明细数据
        /// </summary>
        /// <param name="batch">批次号</param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetDatailData(string batch)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetDetailDataList(batch);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                    returnVM.TotalCount = resdata.Count();

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
        [Authorize]
        public ApiBaseReturn<List<LineModel>> AddOrModify()
        {
            ApiBaseReturn<List<LineModel>> returnVM = new ApiBaseReturn<List<LineModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetLineDataAll();
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
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody] MesSpotcheckHeaderAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 校验参数

                    if (!ErrorInfo.Status && !string.IsNullOrEmpty(item.BATCH_NO))
                    {
                        int status = _repository.GetStatusByBatch(item.BATCH_NO);
                        if (status != 0)
                        {
                            ErrorInfo.Set(_localizer["update_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        if (string.IsNullOrEmpty(item.BATCH_NO))
                            item.ORDER_NO = _repository.GetOrderNo(item.LINE_ID, item.WO_NO);

                        item.STATUS = 0;
                        item.SAMP_STANDART = 0;
                        item.FAIL_QTY = 0;
                        item.RESULT = null;

                        //item.CHECKER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
                        var resultData = await _service.AddOrModifyAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 通过ID删除记录
        /// </summary>
        /// <param name="Id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(string Id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    int status = _repository.GetStatusByBatch(Id);

                    #region 校验参数

                    if (!ErrorInfo.Status && status != 0)
                    {
                        ErrorInfo.Set(_localizer["delete_status_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 删除并返回

                    if (!ErrorInfo.Status)
                    {
                        int i = await _repository.DeleteSpotCheck(Id);
                        if (i == 0)
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(_localizer["delete_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result = true;
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
        /// 根据工单号获取产品信息
        /// </summary>
        /// <param name="wo_no">工单号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetPartDataByWoNo(string wo_no)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetPartDataByWoNo(wo_no);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);

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
        /// 获取当前线别在线工单
        /// </summary>
        /// <param name="line_id">线别ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetWoNoByLineId(decimal line_id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetWoNoByLineId(line_id);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);

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

        ///// <summary>
        ///// 确认抽检单
        ///// </summary>
        ///// <param name="result">抽检结果</param>
        ///// <param name="batch">批次号</param>
        ///// <param name="userName">用户名称</param>
        ///// <returns></returns>
        //[HttpGet]
        //[Authorize]
        //public async Task<string> ConfirmSpotCheck(int result, string batch, string userName)
        //{
        //	BaseResult res = new BaseResult();
        //	int status = _repository.GetStatusByBatch(batch);
        //	if (status != 0)
        //	{
        //		res.ResultCode = 1;
        //		res.ResultMsg = "确认失败，当前抽检单不是初始状态！";
        //	}

        //	int i = await _repository.ConfirmSpotCheck(result, batch, userName);

        //	if (i == 0)
        //	{
        //		res.ResultCode = 1;
        //		res.ResultMsg = "确认失败，未找到相应数据，请刷新重试！";
        //	}
        //	else
        //	{
        //		res.ResultCode = 0;
        //		res.ResultMsg = "确认成功！";
        //	}
        //	return JsonHelper.ObjectToJSON(res);
        //}

        /// <summary>
        /// 审核抽检单
        /// </summary>
        /// <param name="model">审核抽检模型</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AuditSpotCheck([FromBody] AuditSpotModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    int status = _repository.GetStatusByBatch(model.Batch);

                    #region 校验参数

                    if (!ErrorInfo.Status && status != 0)
                    {
                        //审核失败，当前抽检单不是初始状态！
                        ErrorInfo.Set(_localizer["audit_status_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 审核并返回

                    if (!ErrorInfo.Status)
                    {
                        int resdata = await _repository.AuditSpotCheck(model.ResultStatus, model.Batch, model.AuditUser);
                        if (resdata == 0)
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(_localizer["audit_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result = true;
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
        /// 更新抽检项目数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> UpdateSpotCheckIteamsData([FromBody] MesSpotcheckIteamsRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (model.BATCH_NO.IsNullOrWhiteSpace() && model.BATCH_NO.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["BATCH_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        //检查抽检单是否审核 状态：0 新增； 2 确认；3审核；
                        int status = _repository.GetStatusByBatch(model.BATCH_NO);
                        if (status == 3)
                        {
                            ErrorInfo.Set(_localizer["STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.UpdateSpotCheckIteamsData(model) > 0 ? true : false;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 保存QC过程检验的抽检明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveFailDetailData([FromBody] FailDetailRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    Decimal? qc_type = null;

                    if (model.SITE_ID < 1 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SITES_INFO_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        SfcsOperationSites sfcsOperationSites = _repository.QueryEx<SfcsOperationSites>("SELECT * FROM SFCS_OPERATION_SITES WHERE ID = :ID", new { ID = model.SITE_ID }).FirstOrDefault();
                        if (sfcsOperationSites == null)
                        {
                            ErrorInfo.Set(_localizer["SITES_INFO_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    if (string.IsNullOrEmpty(model.BATCH_NO) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["BATCH_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        MesSpotcheckHeader header = (await _repository.GetListByTableEX<MesSpotcheckHeader>("*", "MES_SPOTCHECK_HEADER", " AND BATCH_NO=:BATCH_NO", new { BATCH_NO = model.BATCH_NO })).FirstOrDefault();
                        if (header == null)
                        {
                            ErrorInfo.Set(_localizer["BATCH_NO_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (header.STATUS == 3)
                        {
                            //检查抽检单是否审核 状态：0 新增； 2 确认；3审核；
                            ErrorInfo.Set(_localizer["STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (header.QC_TYPE != 1 && header.QC_TYPE != 2)
                        {
                            //只有完工检验和终检检验的单据才能进行检验
                            ErrorInfo.Set(_localizer["QC_TYPE_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (header.ALL_QTY == header.CHECK_QTY && header.QC_TYPE == 2 && !string.IsNullOrEmpty(model.SN))
                        {
                            decimal snQty = _repository.QueryEx<decimal>("SELECT COUNT(0) SNQTY FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO= :BATCH_NO AND SN= :SN ", new { BATCH_NO = model.BATCH_NO, SN = model.SN }).FirstOrDefault();
                            if (snQty <= 0)
                            {
                                ErrorInfo.Set(_localizer["SN_QTY_FULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        qc_type = header == null ? null : header.QC_TYPE;
                    }
                    if (string.IsNullOrEmpty(model.SN) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SN_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        //根据产品批次号和工单号检查当前SN
                        String sQuery = @"SELECT NVL(COUNT(1) ,0) FROM SFCS_RUNCARD SR,SFCS_WO SW,MES_SPOTCHECK_HEADER SH WHERE SR.WO_ID = SW.ID AND SW.WO_NO = SH.WO_NO AND SN = :SN AND SH.BATCH_NO = :BATCH_NO AND SH.WO_NO = :WO_NO ";
                        decimal qty = _repository.QueryEx<decimal>(sQuery, new { SN = model.SN, BATCH_NO = model.BATCH_NO, WO_NO = model.WO_NO }).FirstOrDefault();
                        if (qty <= 0)
                        {
                            ErrorInfo.Set(_localizer["SN_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    if (model.STATUS != GlobalVariables.PassStatus && model.STATUS != GlobalVariables.FailStatus && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["STATUS_VALUE_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SaveFailDetailData(model, qc_type) > 0 ? true : false;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 根据抽检批次号和SN删除质检明细数据
        /// </summary>
        /// <param name="batch_no">抽检批次号</param>
        /// <param name="sn">产品流水号</param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DeleteSpotCheckDetailByBatchNo(string batch_no, string sn)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    //状态：0 新增； 2 确认；3审核；
                    int status = _repository.GetStatusByBatch(batch_no);
                    if (status == 3)
                    {
                        ErrorInfo.Set(_localizer["STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值
                    returnVM.Result = await _repository.DeleteSpotCheckDetailByBatchNo(batch_no, sn) > 0 ? true : false;
                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #region 明细

        /// <summary>
        /// 选择不良信息视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<SelectDefectResult> SelectDefectIndex()
        {
            ApiBaseReturn<SelectDefectResult> returnVM = new ApiBaseReturn<SelectDefectResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new SelectDefectResult
                        {
                            TypeList = _repository.GetParametersByType("DEFECT_TYPE"),
                            ClassList = _repository.GetParametersByType("DEFECT_CLASS"),
                            CategoryList = _repository.GetParametersByType("DEFECT_CATEGORY"),
                            LevelList = _repository.GetParametersByType("DEFECT_LEVEL_CODE"),
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
        /// 获取不良信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadDefectData([FromQuery] SfcsDefectConfigRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.LoadDefectData(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                    //returnVM.TotalCount = resdata.Count();

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
        /// 保存明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrEditDetail([FromBody] MesSpotcheckDetailModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    int status = _repository.GetStatusByBatch(model.BATCH_NO);

                    #region 校验参数

                    if (!ErrorInfo.Status && status != 0)
                    {
                        //操作失败，当前抽检单不是初始状态！
                        ErrorInfo.Set(_localizer["detail_status_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        //model.CREATOR = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
                        model.CREATE_TIME = DateTime.Now;
                        int i = await _repository.AddOrEditDetail(model);
                        if (i == 0)
                        {
                            returnVM.Result = false;
                            //"操作失败，请刷新后重试！";
                            ErrorInfo.Set(_localizer["detail_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result = true;
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
        /// 删除明细数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteDetail([FromBody] DeleteDetailModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    int status = _repository.GetStatusByBatch(model.Batch);

                    #region 校验参数

                    if (!ErrorInfo.Status && status != 0)
                    {
                        //删除失败，当前抽检单不是初始状态，无法删除明细数据！
                        ErrorInfo.Set(_localizer["delete_detail_status_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 删除明细并返回

                    if (!ErrorInfo.Status)
                    {
                        int i = await _repository.DeleteDetail(model.ID, model.Batch);
                        if (i == 0)
                        {
                            returnVM.Result = false;
                            //"操作失败，请刷新后重试！";
                            ErrorInfo.Set(_localizer["detail_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result = true;
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

        #endregion

        #region 内部方法

        /// <summary>
        /// 审核抽检模型
        /// </summary>
        public class AuditSpotModel
        {
            /// <summary>
            /// 抽检结果
            /// </summary>
            public int ResultStatus { get; set; }

            /// <summary>
            /// 批次号
            /// </summary>
            public string Batch { get; set; }

            /// <summary>
            /// 审核人
            /// </summary>
            public string AuditUser { get; set; }
        }

        /// <summary>
        /// 选择不良信息返回集
        /// </summary>
        public class SelectDefectResult
        {
            /// <summary>
            /// 类型
            /// </summary>
            public List<SfcsParameters> TypeList { get; set; }

            /// <summary>
            /// 种类
            /// </summary>
            public List<SfcsParameters> ClassList { get; set; }

            /// <summary>
            /// 类别
            /// </summary>
            public List<SfcsParameters> CategoryList { get; set; }

            /// <summary>
            /// 等级
            /// </summary>
            public List<SfcsParameters> LevelList { get; set; }
        }

        /// <summary>
        /// 删除明细模型 
        /// </summary>
        public class DeleteDetailModel
        {
            /// <summary>
            /// 明细ID
            /// </summary>
            public decimal ID { get; set; }

            /// <summary>
            /// 批次
            /// </summary>
            public string Batch { get; set; }
        }

        #endregion
    }
}