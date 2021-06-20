using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels.SOPRoutes;
using JZ.IMS.WebApi.Public;
using JZ.IMS.WebApi.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using FluentValidation.Results;
using System.Net.Http.Headers;
using JZ.IMS.Models.SOP;
using static JZ.IMS.WebApi.Controllers.SOPRoutesController;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using JZ.IMS.WebApi.Controllers.BomVsPlacement;
using JZ.IMS.WebApi.Common;

namespace JZ.IMS.WebApi.Controllers.system
{
    /// <summary>
    /// 简化版 工艺路线控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SimpleSOPRoutesController : BaseController
    {

        private readonly ISOPRoutesService _service;
        private readonly ISimpleSOP_ROUTESRepository _repository;
        private readonly ISysEmployeeService _serviceSysEmployee;
        private readonly IStringLocalizer<MenuController> _menuLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SOPRoutesController> _localizer;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly ISfcsOperationsService _operationsService;
        private readonly IBomVsPlacementRepository _bomVsPlacementRepository;
        private readonly IStringLocalizer<BomVsPlacementController> _bomlocalizer;
        private readonly ISfcsProductComponentsRepository _componentRepository;
        private readonly ISfcsProductResourcesRepository _productResourcesRepository;
        private readonly ISfcsProductCartonRepository _productCartonrepository;
        private readonly ISfcsProductUidsRepository _productUidsRepository;
        private readonly ISfcsProductPalletRepository _productPalletRepository;

        public SimpleSOPRoutesController(ISOPRoutesService service, ISimpleSOP_ROUTESRepository repository, ISysEmployeeService serviceSysEmployee,
            IStringLocalizer<MenuController> menuLocalizer, IHttpContextAccessor httpContextAccessor, IStringLocalizer<SOPRoutesController> localizer, ISfcsOperationsService operationsService, IHostingEnvironment hostingEnv, IBomVsPlacementRepository bomVsPlacementRepository,
            IStringLocalizer<BomVsPlacementController> bomlocalizer, ISfcsProductComponentsRepository componentRepository, ISfcsProductCartonRepository productCartonrepository, ISfcsProductUidsRepository productUidsRepository, ISfcsProductPalletRepository productPalletRepository, ISfcsProductResourcesRepository productResourcesRepository)
        {
            _serviceSysEmployee = serviceSysEmployee;
            _repository = repository;
            _service = service;
            _menuLocalizer = menuLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _hostingEnv = hostingEnv;
            _operationsService = operationsService;
            _bomVsPlacementRepository = bomVsPlacementRepository;
            _bomlocalizer = bomlocalizer;
            _componentRepository = componentRepository;
            _productCartonrepository = productCartonrepository;
            _productUidsRepository = productUidsRepository;
            _productPalletRepository=productPalletRepository;
            _productResourcesRepository=productResourcesRepository;
    }

        public class ViewAddOrModify
        {
            /// <summary>
            /// 主键
            /// </summary>
            public static int? ID { get; set; }
            /// <summary>
            /// 状态(0:待审核;1:已审核;)
            /// </summary>
            public static decimal? STATUS { get; set; }

            /// <summary>
            /// 料号
            /// </summary>
            public static string PART_NO { get; set; } = null;

            /// <summary>
            /// 制程名称
            /// </summary>
            public static string ROUTE_NAME { get; set; } = null;

            /// <summary>
            /// 描述
            /// </summary>
            public static string DESCRIPTION { get; set; }

            /// <summary>
            /// 版本号
            /// </summary>
            public static string ATTRIBUTE1 { get; set; }

            /// <summary>
            /// 生效日期
            /// </summary>
            public static string ATTRIBUTE2 { get; set; }

            /// <summary>
            /// 失效日期
            /// </summary>
            public static string ATTRIBUTE3 { get; set; }

            /// <summary>
            /// 主键
            /// </summary>
            public static decimal? m_ResourceID { get; set; } = null;

            /// <summary>
            /// 资源URL（原图URL/视频URL）
            /// </summary>
            public static string m_RESOURCE_URL { get; set; } = null;

        }

        /// <summary>
        /// 简易版工艺路线控制器 首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
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


        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery] SOPRoutesRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.LoadData(model);
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(resdata.data);
                    returnVM.TotalCount = resdata.count;

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
        /// 通过成品料号 获取零件料号
        /// </summary>
        /// <param name="partNo">成品料号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<string>>> LoadDataByPartNo([FromQuery] SOPRoutesRequestModel model)
        {
            ApiBaseReturn<List<string>> returnVM = new ApiBaseReturn<List<string>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetBomPNByPartNo(model);
                    if ((resdata?.count ?? 0) <= 0)
                    {
                        var _service = new BomVsPlacementService(_bomVsPlacementRepository, _bomlocalizer);
                        var resBom = await _service.LoadBom(model.PART_NO, BomType.ALL);
                        if (_service.IsError)
                        {
                            ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //再查一次数据
                            resdata = await _repository.GetBomPNByPartNo(model);
                        }
                    }

                    returnVM.Result = resdata?.data ?? "";
                    returnVM.TotalCount = resdata?.count ?? 0;

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
        public async Task<ApiBaseReturn<string>> ExportData([FromQuery] SOPRoutesRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.ExportData(model);
                        returnVM.Result = JsonHelper.ObjectToJSON(resdata);
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
        ///  添加、修改视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<DetailResult>> AddOrModify(int id = 0)
        {
            ApiBaseReturn<DetailResult> returnVM = new ApiBaseReturn<DetailResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    DetailResult result = new DetailResult();

                    SOP_ROUTES item = await _service.GetMainAsync(id);
                    result.ID = id;
                    if (item != null)
                    {
                        result.STATUS = item.STATUS;
                        result.PART_NO = item.PART_NO;
                        result.ROUTE_NAME = item.ROUTE_NAME;
                        result.DESCRIPTION = item.DESCRIPTION;
                    }

                    if (id > 0)
                    {
                        var resdata = await _repository.LoadResourceProductData(Convert.ToDecimal(id));
                        if (resdata != null)
                        {
                            result.m_ResourceID = resdata.ID;
                            result.m_RESOURCE_URL = resdata.RESOURCE_URL;
                        }
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
        ///  预览/编辑 资源
        /// </summary>
        /// <param name="mst_id">主表ID</param>
        /// <param name="id"></param>
        /// <param name="operid">工序ID</param>
        /// <param name="imgurl">图片</param>
        /// <param name="isedit">是否编辑</param>
        /// <param name="isPart">是否零件</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<PreviewSOPVM>> PreviewSOP(string mst_id, string id, string operid, string imgurl, string isedit, string isPart)
        {
            ApiBaseReturn<PreviewSOPVM> returnVM = new ApiBaseReturn<PreviewSOPVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    PreviewSOPVM result = new PreviewSOPVM();
                    result.id = id;
                    result.operid = operid;
                    result.imgurl = imgurl;
                    result.isedit = isedit;
                    result.isPart = isPart;
                    result.mstId = mst_id;
                    result.RESOURCE_MSG = "";

                    if (!id.IsNullOrWhiteSpace())
                    {
                        SOP_OPERATIONS_ROUTES_RESOURCE resdata = await _repository.LoadResourceByID(Convert.ToDecimal(id));
                        result.RESOURCE_MSG = resdata.RESOURCE_MSG ?? string.Empty;

                        result.PartInfo = await _repository.GetSourcePartBySourceId(id);
                    }
                    if (result.PartInfo == null)
                        result.PartInfo = new SopOperationsRoutesPartListModel();

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
        /// 添加工序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> AddOperations()
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

            return returnVM;
        }

        /// <summary>
        /// 加载作业图列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<string>> LoadResource([FromQuery] MenuRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (string.IsNullOrWhiteSpace(model.parentid))
                    {
                        model.parentid = "0";
                    }
                    var resdata = await _service.LoadResource(Convert.ToDecimal(model.parentid));
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

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
        /// 根据工序ID和资源名称加载作业图列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<string>> LoadResourceByIDandName([FromQuery] MenuRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.LoadResourceByIDandName(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

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
        /// 资源信息转移工序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ApiBaseReturn<bool>> UpdateResourceByParentID([FromBody] MESTransferProcess model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            #region 验证参数

            if (!ErrorInfo.Status && model.ParentId.IsNullOrWhiteSpace())
            {
                //请选择工序!注意检查!
                ErrorInfo.Set(_localizer["error_operation_id"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            if (!ErrorInfo.Status && model.ResourceId == null || model.ResourceId.Count <= 0)
            {
                //请选择转移的SOP项!
                ErrorInfo.Set(_localizer["error_sop_id"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #endregion
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _service.UpdateResourceByParentID(model);

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
        /// 零件图:资源列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadResourceCmpData([FromQuery] MenuRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (string.IsNullOrWhiteSpace(model.parentid))
                    {
                        model.parentid = "0";
                    }
                    var resdata = await _repository.LoadResourceCmpData(Convert.ToDecimal(model.parentid));
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

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
        /// 产品图:资源列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadResourceProductData([FromQuery] MenuRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (string.IsNullOrWhiteSpace(model.parentid))
                    {
                        model.parentid = "0";
                    }
                    var resdata = await _service.LoadResource(Convert.ToDecimal(model.parentid));
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
                    returnVM.TotalCount = resdata.count;

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
        /// 工序子表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partNo">料号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadDtlData(decimal id, string partNo)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.LoadDtlData(id);
                    if (resdata.count >= 0)
                    {
                        var modelList = (List<SOPOperationsView>)resdata.data;
                        
                        modelList?.ForEach(async c =>
                        {
                            //资源SOP维护
                            MenuRequestModel menuModel = new MenuRequestModel() { parentid = c.ID.ToString() };
                            var resourceModel = await _service.LoadResourceByIDandName(menuModel);
                            c.SOP = (resourceModel?.count ?? 0) > 0 ? "√" : "";

                            //采集零件
                            SfcsProductComponentsRequestModel component = new SfcsProductComponentsRequestModel() { COLLECT_OPERATION_ID = c.CURRENT_OPERATION_ID , PART_NO = partNo };
                            var componentCount = await _componentRepository.RecordCountAsync(" WHERE ID > 0 AND COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID and instr(PART_NO, :PART_NO) > 0 ", component);
                            c.Components = componentCount > 0 ? "√" : "";

                            //采集箱号维护
                            //SfcsProductCartonRequestModel cartion = new SfcsProductCartonRequestModel() { COLLECT_OPERATION_ID = c.CURRENT_OPERATION_ID, PART_NO = partNo };
                            //var cartonCount = await _productCartonrepository.RecordCountAsync(" WHERE ID > 0 AND instr(PART_NO, :PART_NO) > 0 AND  COLLECT_OPERATION_ID =:COLLECT_OPERATION_ID ", cartion);
                            //sb.AppendLine(cartonCount > 0 ? "√采集箱号维护" : "");

                            //采集资源维护
                            SfcsProductResourcesRequestModel resources = new SfcsProductResourcesRequestModel() { COLLECT_OPERATION_ID = c.CURRENT_OPERATION_ID, PART_NO = partNo };
                            var resourcesCount = await _productResourcesRepository.RecordCountAsync(" WHERE ID > 0 AND instr(PART_NO, :PART_NO) > 0 AND  COLLECT_OPERATION_ID =:COLLECT_OPERATION_ID ", resources);
                            c.Resources = resourcesCount > 0 ? "√" : "";

                            //采集UID维护
                            SfcsProductUidsRequestModel uids = new SfcsProductUidsRequestModel() { COLLECT_OPERATION_ID = c.CURRENT_OPERATION_ID, Key = partNo };
                            var uidsCount = await _productUidsRepository.RecordCountAsync(" WHERE ID > 0 AND instr(PART_NO, :Key) > 0 AND  COLLECT_OPERATION_ID =:COLLECT_OPERATION_ID ", uids);
                            c.UIDS = uidsCount > 0 ? "√" : "";

                            //采集栈板维护
                            //SfcsProductPalletRequestModel pallet = new SfcsProductPalletRequestModel() { COLLECT_OPERATION_ID = c.CURRENT_OPERATION_ID,PART_NO = partNo };
                            //var palletCount = await _productPalletRepository.RecordCountAsync(" WHERE ID > 0 AND instr(PART_NO, :PART_NO) > 0 AND  COLLECT_OPERATION_ID =:COLLECT_OPERATION_ID ", pallet);
                            //sb.AppendLine(uidsCount > 0 ? "√采集栈板维护" : "");
                        });
                    }
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(resdata.data);
                    returnVM.TotalCount = resdata.count;

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
        /// 工序列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetOperationList([FromQuery] PageModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var result = await _operationsService.GetEnabledListsync(model);
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(result.data);
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
        /// 保存单据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> AddOrModifySave([FromBody] SOPRoutesAddOrModifyModel item)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (BillIsChecked(item.ID))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;

                        returnVM.Result = "-1";
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.AddOrModifyAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = resultData.ResultData;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = "-1";
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> Delete(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (BillIsChecked(id))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;

                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.DeleteAsync(id);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 删除工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteSub(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (BillIsChecked(id))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;

                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.DeleteSubAsync(id);
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
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteResource([FromBody] List<decimal> idList)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (idList.Count <= 0)
                        return returnVM;
                    foreach (var id in idList)
                    {
                        if (BillIsChecked(id))
                        {
                            var result = new BaseResult();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                            result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;

                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            break;
                        }

                    }


                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.DeleteResourceBatch(idList);
                        if (resultData)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            var result = new BaseResult();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = ResultCodeAddMsgKeys.CommonExceptionMsg;

                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
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
		/// 审核
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AuditByIdAsync([FromBody] ChangeStatusModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_menuLocalizer);
                        ValidationResult results = validationRules.Validate(item);
                        if (!results.IsValid)
                        {
                            ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 更改审核状态并返回

                    if (!ErrorInfo.Status)
                    {
                        item.Status = true;
                        item.OperatorDatetime = DateTime.Now;
                        var resultData = await _service.AuditByIdAsync(item);
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
		/// 取消审核
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> UnAuditByIdAsync([FromBody] ChangeStatusModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_menuLocalizer);
                        ValidationResult results = validationRules.Validate(item);
                        if (!results.IsValid)
                        {
                            ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 更改审核状态并返回

                    if (!ErrorInfo.Status)
                    {
                        item.Status = false;
                        item.OperatorDatetime = DateTime.Now;
                        var resultData = await _service.AuditByIdAsync(item);
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
		/// 是否存在相同的名称的料号
		/// </summary>
		/// <param name="item"></param>
		/// <remarks>
		/// 参数: 主键ID, 料号:PART_NO 必传
		/// </remarks>
		/// <returns></returns>
		[Authorize]
        [HttpGet]
        public async Task<ApiBaseReturn<bool>> IsExistsName([FromQuery] SOPRoutesAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var result = await _service.IsExistsNameAsync(item);
                    returnVM.Result = result.Data;

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
		/// 保存工序(图片上传时) 
		/// </summary>
		/// <remarks>
		/// 说明:
		/// 返回工序ID: 保存成功: id>0, 失败: -1; 
		/// </remarks>
		/// <returns></returns>
		[HttpPost]
        [Authorize("Permission")]
        public ApiBaseReturn<int> AddOperOfUploadImage([FromBody] AddOperOfUploadImageModel model)
        {
            ApiBaseReturn<int> returnVM = new ApiBaseReturn<int>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (BillIsChecked(Convert.ToDecimal(model.routid)))
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;
                        returnVM.Result = -1;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 保存工序并返回工序ID

                    if (!ErrorInfo.Status)
                    {
                        //保存工序
                        decimal new_id = 0;
                        if (model.id == "0")
                        {
                            new_id = _repository.Get_Detail_SEQID();
                            if (new_id > 0)
                            {
                                var obj = new SOP_OPERATIONS_ROUTES
                                {
                                    ID = new_id,
                                    ROUTE_ID = Convert.ToDecimal(model.routid),
                                    ORDER_NO = new_id,
                                    CURRENT_OPERATION_ID = Convert.ToDecimal(model.operid),
                                    NEXT_OPERATION_ID = 0,
                                    PREVIOUS_OPERATION_ID = 0,
                                };

                                _repository.InsertDetail(obj);
                                model.id = Convert.ToString(new_id);
                            }
                        }
                        returnVM.Result = Convert.ToInt32(model.id);
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
		/// 图片上传
		/// </summary>
		/// <param name="mst_id">关联ID</param>
		/// <param name="category">资源类别(0：产品图，1：作业图，2：零件图)</param>
		/// <param name="resource_id">原有资料id(产品类型使用)</param>
		/// <returns></returns>
		[HttpPost]
        [Authorize("Permission")]
        public ApiBaseReturn<UploadImageResult> UploadImage([FromForm] decimal mst_id, [FromForm] string category, [FromForm] decimal resource_id)
        {
            ApiBaseReturn<UploadImageResult> returnVM = new ApiBaseReturn<UploadImageResult>();

            var imgFile = Request.Form.Files[0];
            var resource_name = string.Empty;
            long size = 0;
            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (imgFile == null || imgFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(imgFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        resource_name = filename;

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(imgFile.Length / 1024.00, 3));
                        long mb = imgFile.Length / 1024 / 1024; // MB
                        if (mb > 50)
                        {
                            //return Json(new { code = 1, msg = "只允许上传小于 5MB 的图片.", });
                            ErrorInfo.Set(_localizer["upload_size_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        #endregion
                    }

                    #endregion

                    #region 保存文件并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var filenameNew = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var path = $"/upload/sopfile/"; //+ DateTime.Now.ToString("yyyyMM");
                        string dir = @"upload\sopfile\";
                        var pathWebRoot = AppContext.BaseDirectory + dir;
                        if (!Directory.Exists(pathWebRoot))
                        {
                            Directory.CreateDirectory(pathWebRoot);
                        }
                        filename = pathWebRoot + $"{filenameNew}";
                        size += imgFile.Length;
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            imgFile.CopyTo(fs);
                            fs.Flush();
                        }

                        //保存资源
                        decimal res_id = 0;
                        if (mst_id > 0 && resource_id == 0)
                        {
                            res_id = _repository.Get_Resource_SEQID();
                            if (res_id > 0)
                            {
                                var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                                {
                                    ID = res_id,
                                    MST_ID = mst_id,
                                    ORDER_NO = res_id,
                                    RESOURCE_TYPE = 0,
                                    RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                    RESOURCE_URL_THUMB = "",
                                    RESOURCE_NAME = resource_name,
                                    RESOURCE_SIZE = filesize,
                                    RESOURCES_CATEGORY = Convert.ToDecimal(category),
                                };
                                _repository.InsertResource(res_entity);
                            }
                        }
                        //更新产品图 
                        else if (resource_id > 0 && category == "0")
                        {
                            res_id = resource_id;
                            var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
                            {
                                ID = resource_id,
                                MST_ID = mst_id,
                                ORDER_NO = resource_id,
                                RESOURCE_TYPE = 0,
                                RESOURCE_URL = System.IO.Path.Combine(path, $"{filenameNew}"),
                                RESOURCE_URL_THUMB = "",
                                RESOURCE_NAME = resource_name,
                                RESOURCE_SIZE = filesize,
                                RESOURCES_CATEGORY = Convert.ToDecimal(category),
                            };
                            _repository.UpdateResourceByID(res_entity);
                        }

                        UploadImageResult imgResult = new UploadImageResult
                        {
                            resource_url = path + $"{filenameNew}",
                            mst_id = mst_id,
                            resource_id = res_id
                        };
                        returnVM.Result = imgResult;
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
		/// 保存图片说明  
		/// </summary>
		/// <returns></returns>
		[HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> UpdateMsgInfo([FromBody] UpdateMsgInfoModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (model.Resource == null)
                    {
                        var result = new BaseResult();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonFailNoDataCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonFailNoDataMsg;

                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && (model.PartInfo != null && model.PartInfo.PART_NO != null))
                    {
                        //partInfo.CREATEUSER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
                        model.PartInfo.CREATEDATE = DateTime.Now;
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.UnpdateResourceMsg(model.Resource, model.PartInfo);
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
        public async Task<ApiBaseReturn<bool>> SOPCopySave([FromBody] SOPCopyRequestModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _service.SOPCopyAsync(item);
                        if (result.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
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
		/// 新的SOP复制 执行方法（2020-09-01）
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SOPCopySaveNew([FromBody] SOPCopyRequestModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _service.SOPCopyAsyncNEW(item);
                        if (result.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
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
		/// 批量新的SOP复制 执行方法（2020-12-09）
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<string>>> BatchSOPCopySaveNew([FromBody] BatchSOPCopyRequest SopModelList)
        {
            ApiBaseReturn<List<string>> returnVM = new ApiBaseReturn<List<string>>();
            //记录异常消息
            List<string> msgList = new List<string>();
            returnVM.Result = msgList;


            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status && SopModelList != null && SopModelList.modelList != null && SopModelList.modelList.Count > 0)
                    {
                        foreach (var item in SopModelList.modelList)
                        {
                            var result = await _service.SOPCopyAsyncNEW(item);
                            if (result.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                            {
                                string exceptionMsg = "新料号:" + item.PART_NO_NEW + "出现异常，异常原因:" + result.ResultMsg;
                                msgList.Add(exceptionMsg);
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
        /// 根据零件料号获取零件信息
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetPartByPartNo(string partNo)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var data = await _repository.GetPartByPartNo(partNo);
                    returnVM.Result = JsonHelper.ObjectToJSON(data);

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }
            return returnVM;
        }

        /// <summary>
        /// 获取零件信息(未使用工艺路线的料号)
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<TableDataModel>> GetNewPart(int pageIndex = 1, int pageSize = 10, string partNo = "")
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();
            returnVM.Result = new TableDataModel();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetPartByPartNo(pageIndex, pageSize, partNo);
                    returnVM.TotalCount = returnVM.Result.count;
                    if (returnVM.TotalCount <= 0)
                    {

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
        /// 下载图片到服务器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="docname"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        private SOP_OPERATIONS_ROUTES_RESOURCE DownloadFile(string url, string docname)
        {
            Stream stream = null;
            SOP_OPERATIONS_ROUTES_RESOURCE res_entity = null;
            try
            {
                string fileurl = $@"/upload/sopfile/{docname}";
                string filename = _hostingEnv.WebRootPath + fileurl;
                HttpWebRequest ReqData = (HttpWebRequest)WebRequest.Create(url);
                ReqData.Method = "GET";
                HttpWebResponse HWR = ReqData.GetResponse() as HttpWebResponse;
                var len = HWR.ContentLength;
                stream = HWR.GetResponseStream();

                decimal filesize = Convert.ToDecimal(Math.Round(len / 1024.00, 3));
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                img.Save(filename);

                res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE();
                res_entity.RESOURCE_TYPE = 0;
                res_entity.RESOURCE_URL = fileurl;
                res_entity.RESOURCE_NAME = docname;
                res_entity.RESOURCE_SIZE = filesize;

                return res_entity;
            }
            catch (Exception ex)
            {
                return res_entity;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

        /// <summary>
        /// 保存资源排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> SaveRoutesOrderNo([FromBody] List<SOP_OPERATIONS_ROUTES_RESOURCE> list)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        BaseResult result = await _repository.SaveRoutesOrderNo(list);
                        returnVM.Result = JsonHelper.ObjectToJSON(result);
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



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id">单据ID</param>
        ///// <returns></returns>
        private bool BillIsChecked(decimal id)
        {
            return _repository.GetDisplayStatusById(id);
        }

        #region 内部类
        /// <summary>
        /// 批量复制的内部类
        /// </summary>
        public class BatchSOPCopyRequest
        {
            public List<SOPCopyRequestModel> modelList { get; set; }
        }
        #endregion

    }
}