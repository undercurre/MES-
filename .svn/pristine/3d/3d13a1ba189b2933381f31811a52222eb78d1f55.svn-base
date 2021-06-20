/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-22 09:40:14                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsHoldProductHeaderController                                      
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
using System.Net.Http.Headers;
using System.IO;
using System.Collections;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 解锁单据/产品 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsUnLockProductController : BaseController
    {
        private readonly ISfcsHoldProductHeaderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsUnLockProductController> _localizer;

        public SfcsUnLockProductController(ISfcsHoldProductHeaderRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsUnLockProductController> localizer)
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
            /// 主管控条件列表
            /// </summary>
            public List<dynamic> MainCondition { get; set; }

            /// <summary>
            /// 辅助管控条件列表
            /// </summary>
            public List<dynamic> SubCondition { get; set; }

            /// <summary>
            /// 管控动作列表
            /// </summary>
            public List<dynamic> HoldAction { get; set; }
        }

        /// <summary>
        /// 解锁单据之首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
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
                            MainCondition = await _repository.GetListByTable("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'MAIN_CONDITION' ORDER BY LOOKUP_CODE"),
                            SubCondition = await _repository.GetListByTable("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'SUBSIDIARY_CONDITION' ORDER BY LOOKUP_CODE"),
                            HoldAction = await _repository.GetListByTable("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'CONTROL_ACTION' ORDER BY LOOKUP_CODE"),
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
        /// 解锁单据之查询主表数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsHoldProductHeaderListModel>>> LoadData([FromQuery]SfcsHoldProductHeaderRequestModel model)
        {
            ApiBaseReturn<List<SfcsHoldProductHeaderListModel>> returnVM = new ApiBaseReturn<List<SfcsHoldProductHeaderListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var mainCondition = await _repository.GetListByTable("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'MAIN_CONDITION' ORDER BY LOOKUP_CODE");
                    var subCondition = await _repository.GetListByTable("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'SUBSIDIARY_CONDITION' ORDER BY LOOKUP_CODE");
                    var holdAction = await _repository.GetListByTable("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'CONTROL_ACTION' ORDER BY LOOKUP_CODE");

                    #region 设置返回值

                    int count = 0;
                    string conditions = "WHERE HOLD_NUMBER NOT LIKE 'SL%' ";
                    if (!model.HOLD_NUMBER.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (HOLD_NUMBER =:HOLD_NUMBER) ";
                    }
                    if (!model.HOLD_EMPNO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (HOLD_EMPNO =:HOLD_EMPNO) ";
                    }
                    if (!model.BEGIN_TIME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (HOLD_TIME >= to_date(:BEGIN_TIME,'yyyy-mm-dd HH24:MI:SS')) ";
                    }
                    if (!model.END_TIME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (HOLD_TIME <= to_date(:END_TIME,'yyyy-mm-dd HH24:MI:SS')) ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsHoldProductHeaderListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsHoldProductHeaderListModel>(x);

                        item.MAIN_CONDITION_CAPTION = mainCondition.Where(t => t.LOOKUP_CODE == x.MAIN_CONDITION).Select(t => t.MEANING).FirstOrDefault();
                        item.SUBSIDIARY_CONDITION_CAPTION = subCondition.Where(t => t.LOOKUP_CODE == x.SUBSIDIARY_CONDITION).Select(t => t.MEANING).FirstOrDefault();
                        item.HOLD_ACTION_CAPTION = holdAction.Where(t => t.LOOKUP_CODE == x.HOLD_ACTION).Select(t => t.MEANING).FirstOrDefault();

                        viewList.Add(item); 
                    });
                    //var viewList2 = list.Select(t => new SfcsHoldProductHeaderListModel()
                    //{
                    //    ID = t.ID,
                    //    MAIN_CONDITION_CAPTION = mainCondition.Where(s => s.LOOKUP_CODE == t.MAIN_CONDITION).Select(s => s.MEANING).FirstOrDefault(),

                    //}).ToList();

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
        /// 解锁单据之获取明细数据 
        /// </summary>
        /// <param name="hold_id">主表ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsHoldProductDetailVListModel>>> GetDetailData(decimal hold_id)
        {
            ApiBaseReturn<List<SfcsHoldProductDetailVListModel>> returnVM = new ApiBaseReturn<List<SfcsHoldProductDetailVListModel>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        string conditions = "WHERE HOLD_NUMBER NOT LIKE 'SL%' and HOLD_ID = :HOLD_ID";
                        var resdata = await _repository.GetListAsyncEx<SfcsHoldProductDetailVListModel>(conditions, new { HOLD_ID = hold_id });

                        returnVM.Result = resdata?.ToList();
                        returnVM.TotalCount = resdata?.Count() ?? 0;
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
        /// 解锁单据之保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> UnLockBillSave([FromBody] UnLockBillSaveModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.ID <= 0)
                    {
                        //请录入需解锁的单据ID。
                        ErrorInfo.Set(_localizer["Err_HOLD_ID"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.HOLD_NUMBER.IsNullOrWhiteSpace())
                    {
                        //请录入解锁单据号。
                        ErrorInfo.Set(_localizer["Err_HOLD_NUMBER"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.RELEASE_CAUSE.IsNullOrWhiteSpace())
                    {
                        //请录入解锁原因。
                        ErrorInfo.Set(_localizer["Err_RELEASE_CAUSE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.User_Name.IsNullOrWhiteSpace())
                    {
                        //请录入操作人员。
                        ErrorInfo.Set(_localizer["Err_User_Name"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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

        /// <summary>
        /// 解锁产品之首页模型
        /// </summary>
        public class ProductIndexVM
        {
            /// <summary>
            /// 线别列表
            /// </summary>
            public List<dynamic> LineList { get; set; }

            /// <summary>
            /// 工序列表
            /// </summary>
            public List<dynamic> OperationList { get; set; }

            /// <summary>
            /// 站点列表
            /// </summary>
            public List<dynamic> SiteList { get; set; }
        }

        /// <summary>
        /// 解锁产品之首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<ProductIndexVM>> ProductIndex()
        {
            ApiBaseReturn<ProductIndexVM> returnVM = new ApiBaseReturn<ProductIndexVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        //排除Packing工序
                        returnVM.Result = new ProductIndexVM
                        {
                            LineList = await _repository.GetListByTable("ID,OPERATION_LINE_NAME", "SFCS_OPERATION_LINES", "ORDER BY OPERATION_LINE_NAME"),
                            OperationList = await _repository.GetListByTable("ID,OPERATION_NAME", "SFCS_OPERATIONS", "AND OPERATION_CATEGORY != 5 ORDER BY ID"),
                            SiteList = await _repository.GetListByTable("ID,OPERATION_SITE_NAME", "SFCS_OPERATION_SITES", "ORDER BY OPERATION_SITE_NAME"),
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
        /// 解锁产品之查询数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsHoldProductDetailVListModel>>> LoadProductData([FromQuery]SfcsHoldProductDetailRequestModel model)
        {
            ApiBaseReturn<List<SfcsHoldProductDetailVListModel>> returnVM = new ApiBaseReturn<List<SfcsHoldProductDetailVListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        //至少选择一个条件
                        PropertyInfo[] props = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        List<string> exclude_items = new List<string> { "Page", "Limit", "Key" };
                        var condition_prop = props.Where(t => exclude_items.IndexOf(t.Name) == -1 && !(t.GetValue(model).IsNullOrWhiteSpace())).ToList();
                        if (condition_prop?.Count == 0)
                        {
                            ErrorInfo.Set(_localizer["Err_Model_Params"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        int count = 0;
                        string conditions = "WHERE HOLD_NUMBER NOT LIKE 'SL%' ";
                        if (!model.HOLD_NUMBER.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (HOLD_NUMBER =:HOLD_NUMBER) ";
                        }
                        if (!model.SN.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (SN =:SN) ";
                        }
                        if (!model.CARTON_NO.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (CARTON_NO =:CARTON_NO) ";
                        }
                        if (!model.PALLET_NO.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (PALLET_NO =:PALLET_NO) ";
                        }
                        if (!model.COMPONENT_SN.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (COMPONENT_SN =:COMPONENT_SN) ";
                        }
                        if (!model.CUSTOMER_COMPONENT_PN.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (CUSTOMER_COMPONENT_PN =:CUSTOMER_COMPONENT_PN) ";
                        }
                        if (!model.MODEL.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (MODEL =:MODEL) ";
                        }
                        if (!model.PART_NO.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (PART_NO =:PART_NO) ";
                        }

                        if (!model.WO_NO.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (WO_NO =:WO_NO) ";
                        }

                        if (model.OPERATION_LINE_ID != null && model.OPERATION_LINE_ID > 0)
                        {
                            conditions += $"and (OPERATION_LINE_ID =:OPERATION_LINE_ID) ";
                        }

                        if (model.OPERATION_SITE_ID != null && model.OPERATION_SITE_ID > 0)
                        {
                            conditions += $"and (OPERATION_SITE_ID =:OPERATION_SITE_ID) ";
                        }

                        if (model.CURRENT_OPERATION_ID != null && model.CURRENT_OPERATION_ID > 0)
                        {
                            conditions += $"and (CURRENT_OPERATION_ID =:CURRENT_OPERATION_ID) ";
                        }

                        if (!model.BEGIN_TIME.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (CREATE_TIME >= to_date(:BEGIN_TIME,'yyyy-mm-dd HH24:MI:SS')) ";
                        }
                        if (!model.END_TIME.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (CREATE_TIME <= to_date(:END_TIME,'yyyy-mm-dd HH24:MI:SS')) ";
                        }

                        var list = (await _repository.GetListPagedEx<SfcsHoldProductDetailVListModel>(model.Page, model.Limit, conditions, "", model)).ToList();
                        count = await _repository.RecordCountAsyncEx<SfcsHoldProductDetailVListModel>(conditions, model);

                        returnVM.Result = list;
                        returnVM.TotalCount = count;
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
        /// 解锁产品之导入文件查询数据
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsHoldProductDetailVListModel>>> LoadProductData2File([FromForm]decimal? operationType)
        {
            ApiBaseReturn<List<SfcsHoldProductDetailVListModel>> returnVM = new ApiBaseReturn<List<SfcsHoldProductDetailVListModel>>();
            returnVM.Result = new List<SfcsHoldProductDetailVListModel>();

            var txtFile = Request.Form.Files[0];
            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;
            var newFileName = string.Empty;
            string conditions = string.Empty;

            const int ReleaseProductBySerialNumber = 0;
            const int ReleaseProductByComponentSerialNumber = 1;
            const int ReleaseProductByCarton = 2;
            const int ReleaseProductByPallet = 3;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (txtFile == null || txtFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(txtFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        #region 判断后缀

                        if (!extname.ToLower().Contains("txt"))
                        {
                            //msg = "只允许上传txt文件."
                            ErrorInfo.Set(_localizer["file_suffix_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(txtFile.Length / 1024.00, 3));
                        long mb = txtFile.Length / 1024 / 1024; // MB
                        if (mb > 1)
                        {
                            //"只允许上传小于 1MB 的文件."
                            ErrorInfo.Set(_localizer["size_1m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion
                    }

                    if (!ErrorInfo.Status && (operationType == null || operationType == -1))
                    {
                        //请先选择操作类型
                        ErrorInfo.Set(_localizer["Error_OperationType"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 解释txt数据

                    if (!ErrorInfo.Status)
                    {
                        newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var pathRoot = AppContext.BaseDirectory + @"upload\tmpdata\";
                        if (Directory.Exists(pathRoot) == false)
                        {
                            Directory.CreateDirectory(pathRoot);
                        }
                        filename = pathRoot + $"{newFileName}";
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            txtFile.CopyTo(fs);
                            fs.Flush();
                        }

                        if (!System.IO.File.Exists(filename))
                        {
                            ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 查询数据并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        ArrayList fileList = GetSimpleFileContent(filename);
                        ArrayList list = new ArrayList();

                        string item = null;
                        for (int i = 0; i < fileList.Count; i++)
                        {
                            // 跳過空數據
                            item = fileList[i].ToString().Trim();
                            if (!item.IsNullOrEmpty() && list.IndexOf(item) < 0)
                            {
                                list.Add(item);
                            }
                        }

                        switch (operationType)
                        {
                            case ReleaseProductBySerialNumber:
                                {
                                    // 產品流水號
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        //只帶出當前鎖定的數據
                                        var tmpdata = await _repository.GetHoldDetailViewAddByOldSN(Convert.ToString(list[i]));
                                        returnVM.Result.AddRange(tmpdata);
                                    }
                                }
                                break;
                            case ReleaseProductByComponentSerialNumber:
                                {
                                    // 零件流水號
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        // 只帶出當前鎖定的數據
                                        conditions = "WHERE COMPONENT_SN=:COMPONENT_SN AND STATUS='Y' ";
                                        var tmpdata = await _repository.GetListAsyncEx<SfcsHoldProductDetailVListModel>(conditions,
                                            new { COMPONENT_SN = Convert.ToString(list[i]) });
                                        returnVM.Result.AddRange(tmpdata);
                                    }
                                }
                                break;
                            case ReleaseProductByCarton:
                                {
                                    // 卡通號
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        // 只帶出當前鎖定的數據
                                        var runcardDataTable = await _repository.GetListAsyncEx<SfcsRuncard>("WHERE CARTON_NO=:CARTON_NO",
                                            new { CARTON_NO = Convert.ToString(list[i]) });

                                        foreach (var runcardRow in runcardDataTable)
                                        {
                                            conditions = "WHERE SN=:SN AND STATUS='Y' ";
                                            var tmpdata = await _repository.GetListAsyncEx<SfcsHoldProductDetailVListModel>(conditions,
                                                new { COMPONENT_SN = runcardRow.SN });
                                            returnVM.Result.AddRange(tmpdata);
                                        }
                                    }
                                }
                                break;
                            case ReleaseProductByPallet:
                                {
                                    // 棧板號
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        // 只帶出當前鎖定的數據
                                        var runcardDataTable = await _repository.GetListAsyncEx<SfcsRuncard>("WHERE PALLET_NO=:PALLET_NO",
                                            new { PALLET_NO = Convert.ToString(list[i]) });
                                        foreach (var runcardRow in runcardDataTable)
                                        {
                                            conditions = "WHERE SN=:SN AND STATUS='Y' ";
                                            var tmpdata = await _repository.GetListAsyncEx<SfcsHoldProductDetailVListModel>(conditions,
                                                new { runcardRow.SN });
                                            returnVM.Result.AddRange(tmpdata);
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        //導入文件後，Detail默認為解鎖狀態
                        foreach (var detail in returnVM.Result)
                        {
                            detail.STATUS = GlobalVariables.EnableN;
                        }
                        returnVM.TotalCount = returnVM.Result.Count;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
                finally
                {
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 解锁产品之解除产品锁定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<UnLockProductSaveReturn>> UnLockProductSave([FromBody] UnLockProductSaveModel model)
        {
            ApiBaseReturn<UnLockProductSaveReturn> returnVM = new ApiBaseReturn<UnLockProductSaveReturn>();
            returnVM.Result = new UnLockProductSaveReturn()
            {
                Result = false,
                ResultMsgList = new List<string>(),
            };
            string errmsg = string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (model.HoldProductList == null || model.HoldProductList.Count == 0))
                    {
                        //请录入需解锁的单据ID。
                        ErrorInfo.Set(_localizer["Err_HOLD_ID"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.HoldProductList.Where(t => t.STATUS == GlobalVariables.EnableN).Count() > 0)
                    {
                        var un_HoldProductList = model.HoldProductList.Where(t => t.STATUS == GlobalVariables.EnableN).ToList();
                        foreach (var item in un_HoldProductList)
                        {
                            //序号{0}已经是解锁状态，不能修改为锁定状态，如需锁定，请重新执行锁定作业。
                            errmsg = string.Format(_localizer["Err_CanNotChangeHoldStatus"], item.SN);
                            returnVM.Result.ResultMsgList.Add(errmsg);
                        }
                    }

                    if (!ErrorInfo.Status && model.HoldProductList.Where(t => t.STATUS == GlobalVariables.EnableY).Count() == 0)
                    {
                        //没有需要解锁的序号，请确认。 
                        ErrorInfo.Set(_localizer["Err_NoSNNeedtoRelease"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        model.HoldProductList = model.HoldProductList.Where(t => t.STATUS == GlobalVariables.EnableY).ToList();
                    }

                    if (!ErrorInfo.Status && model.RELEASE_CAUSE.IsNullOrWhiteSpace())
                    {
                        //请录入解锁原因。
                        ErrorInfo.Set(_localizer["Err_RELEASE_CAUSE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.User_Name.IsNullOrWhiteSpace())
                    {
                        //请录入操作人员。
                        ErrorInfo.Set(_localizer["Err_User_Name"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.UnLockProductSave(model);
                        if (resdata.ResultMsgList.Count > 0)
                        {
                            foreach (var item in resdata.ResultMsgList)
                            {
                                returnVM.Result.ResultMsgList.Add(item);
                            }
                        }
                        returnVM.Result.Result = resdata.Result;
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

        #region 内部方法

        /// <summary>
        /// 獲取單個文件內容
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        private ArrayList GetSimpleFileContent(string fileFullName)
        {
            if (!System.IO.File.Exists(fileFullName))
            {
                return null;
            }
            StreamReader streamReader = System.IO.File.OpenText(fileFullName);
            ArrayList list = new ArrayList();
            string oneLine = null;
            while ((oneLine = streamReader.ReadLine()) != null)
            {
                list.Add(oneLine);
            }
            streamReader.Close();
            return list;
        }

        #endregion
    }
}