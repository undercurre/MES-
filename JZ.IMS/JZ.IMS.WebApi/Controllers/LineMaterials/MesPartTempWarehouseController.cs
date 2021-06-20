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
using JZ.IMS.WebApi.Validation;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Public;
using JZ.IMS.Models;
using System.Reflection;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 辅料库存管理 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesPartTempWarehouseController : BaseController
    {
        private readonly IMesPartTempWarehouseService _service;
        private readonly IMesPartTempWarehouseRepository _repository;
        private readonly IMesPartShelfRepository _partShelfRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsEquipContentConfController> _localizer;
        private readonly IStringLocalizer<MesPartShelfController> _partShelfLocalizer;

        public MesPartTempWarehouseController(IMesPartTempWarehouseService service, IMesPartTempWarehouseRepository repository, IMesPartShelfRepository partShelfRepository, IHttpContextAccessor httpContextAccessor, IStringLocalizer<SfcsEquipContentConfController> localizer, IStringLocalizer<MesPartShelfController> partShelfLocalizer)
        {
            _service = service;
            _repository = repository;
            _partShelfRepository=partShelfRepository;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _partShelfLocalizer=partShelfLocalizer;
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<List<SfcsParameters>>> Index()
        {
            ApiBaseReturn<List<SfcsParameters>> returnVM = new ApiBaseReturn<List<SfcsParameters>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await Task.Run(() => { return _repository.GetParametersByType("PART_TYPE"); });

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
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery]MesPartTempWarehouseRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = new TableDataModel();
                    data.data = await _repository.GetMstData(model);
                    data.count = await _repository.GetMstDataCount(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(data);
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
        /// 获取明细数据
        /// </summary>
        /// <param name="mstId">库存主表ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetDetailData(decimal mstId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetDetailData(mstId));
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
        /// 获取操作记录数据
        /// </summary>
        /// <param name="mstId">库存主表ID</param>
        /// <param name="detailId">库存明细ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetRecordData(decimal mstId, decimal? detailId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetRecordData(mstId, detailId));
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
        public class AddrModifyView
        {
            public List<SfcsParameters> PartTypeList { get; set; }
            public List<SfcsEquipmentLinesModel> LineList { get; set; }
        }

        /// <summary>
        /// 添加或修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<AddrModifyView>> AddOrModify()
        {
            ApiBaseReturn<AddrModifyView> returnVM = new ApiBaseReturn<AddrModifyView>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = new AddrModifyView()
                    {
                        PartTypeList = _repository.GetParametersByType("PART_TYPE"),
                        LineList = _repository.GetLinesList()
                    };
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
        /// 入库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsParameters>>> InputWarehouse()
        {
            ApiBaseReturn<List<SfcsParameters>> returnVM = new ApiBaseReturn<List<SfcsParameters>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = _repository.GetParametersByType("PART_TYPE");
                    //ViewData["PartUnitList"] = _repository.GetParametersByType("PART_UNIT");
                    //ViewData["LineList"] = _repository.GetLinesList();
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
        /// 出库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<AddrModifyView>> OutputWarehouse()
        {
            ApiBaseReturn<AddrModifyView> returnVM = new ApiBaseReturn<AddrModifyView>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = new AddrModifyView()
                    {
                        PartTypeList = _repository.GetParametersByType("PART_TYPE"),
                        LineList = _repository.GetLinesList()
                    };
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
        /// 添加或修改的相关操作
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> AddOrModifySave([FromForm]MesPartTempWarehouseAddOrModifyModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    BaseResult result = new BaseResult();
                    result = await _service.AddOrModifyAsync(item);

                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        public async Task<ApiBaseReturn<string>> DeleteOneById(decimal Id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    BaseResult result = new BaseResult();
                    result = await _service.DeleteAsync(Id);
                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> ChangeEnabled([FromForm]ChangeStatusModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var result = new BaseResult();
                    ChangeStatusModelValidation validationRules = new ChangeStatusModelValidation(_localizer);
                    ValidationResult results = validationRules.Validate(item);
                    if (results.IsValid)
                    {
                        result = await _service.ChangeEnableStatusAsync(item);
                    }
                    else
                    {
                        result.ResultCode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
                        result.ResultMsg = results.ToString("||");
                    }
                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// 根据物料唯一标识获取物料信息（入库）
        /// </summary>
        /// <param name="reelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetReelDataInput(string reelId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var result = await _repository.GetReelDataInput(reelId);
                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// 根据物料唯一标识获取物料信息（入库） (安卓调用)
        /// </summary>
        /// <param name="reelId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<MesPartTempReelModel>> AndroidGetReelDataInput(string reelId)
        {
            ApiBaseReturn<MesPartTempReelModel> returnVM = new ApiBaseReturn<MesPartTempReelModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var result = await _repository.GetReelDataInputByAndroid(reelId);
                    if (result == null)
                    {
                        ErrorInfo.Set("找不到此条码信息!", MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    returnVM.Result = result;
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
		/// 条码切分 执行方法
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<BaseResult>> ReelCodeSplit([FromHeader]MesPartTempReelModel item)
        {
            ApiBaseReturn<BaseResult> returnVM = new ApiBaseReturn<BaseResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ReelCodeSplitAsync(item);
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
        /// 条码切分 执行方法(webapi)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<TableDataModel>> ReelCodeSplitEx([FromBody] MesReelCodeSplitModel item)
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 验证参数
                    if (!await _partShelfRepository.ImsReelByUsed(item.CODE)&&item.IsDown>0)
                    {
                        ErrorInfo.Set(_partShelfLocalizer["error_reelcode_not_exist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ReelCodeSplitAsyncEx(item);
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
		/// SOP复制 执行方法
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<BaseResult>> ReelCodeUpdate([FromHeader]MesPartTempReelModel item)
        {
            ApiBaseReturn<BaseResult> returnVM = new ApiBaseReturn<BaseResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.ReelCodeUpdate(item);
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
        /// 根据物料唯一标识获取物料信息（出库）
        /// </summary>
        /// <param name="reelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetReelDataOutput(string reelId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.GetReelDataOutput(reelId));
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
        /// 判断物料条码是否在库存明细中存在
        /// </summary>
        /// <param name="reelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> IsExistReelDetail(string reelId)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.IsExistReelDetail(reelId));
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
        /// 判断物料编码是否已经存在库存数据
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> IsExistPartNo(string partNo)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = JsonHelper.ObjectToJSON(await _repository.IsExistPartNo(partNo));
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
        /// 根据物料编码获取下一个该出库的物料条码
        /// </summary>
        /// <param name="part_no"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetNextReelId(string partNo)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    returnVM.Result = await _repository.GetNextReelId(partNo);

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
        /// 入库操作
        /// </summary>
        /// <param name="item">数据集</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> InputWarehouseData([FromForm]MesPartTempWarehouseAddOrModifyModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    item.CREATE_USER = item.CREATE_USER ?? string.Empty;
                    var result = await _repository.InputWarehouseData(item);
                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
        /// 出库操作
        /// </summary>
        /// <param name="item">数据集</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> OutputWarehouseData([FromForm]MesPartTempWarehouseAddOrModifyModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    item.CREATE_USER = item.CREATE_USER ?? string.Empty;
                    var result = await _repository.OutputWarehouseData(item);
                    returnVM.Result = JsonHelper.ObjectToJSON(result);
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
		/// 返回解析条码
		/// </summary>
		/// <param name="reelId"></param>
		/// <returns></returns>
		[HttpGet]
        public async Task<string> GetReelCode(string reelId)
        {
            Reel reel = await _repository.GetReel(reelId);

            if (reel == null)
            {
                reel = new Reel();
                reel.CODE = "";
            }

            return reel.CODE;
        }
    }
}