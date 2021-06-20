/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 14:53:45                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsRuncardRangerController                                      
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
using JZ.IMS.Core.Utilities;
using System.Text.RegularExpressions;
using JZ.IMS.WebApi.Common;
using System.Text;
using RadixConvertPublic = JZ.IMS.WebApi.Common.RadixConvertPublic;
using System.Net.Http.Headers;
using System.IO;
using Aspose.Cells;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 工单流水号范围设定 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsRuncardRangerController : BaseController
    {
        private readonly ISfcsRuncardRangerRepository _repository;
        private readonly IMapper _mapper;
        private readonly ISfcsProductConfigRepository _sfcsProductConfigRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISfcsPrintTasksRepository _sfcsPrintTasksRepository;
        private readonly IMesTongsInfoRepository _repositoryt;
        private readonly IStringLocalizer<SfcsRuncardRangerRulesController> _localizer;

        public SfcsRuncardRangerController(ISfcsRuncardRangerRepository repository,
            ISfcsPrintTasksRepository sfcsPrintTasksRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ISfcsProductConfigRepository sfcsProductConfigRepository, IMesTongsInfoRepository repositoryt,
            IStringLocalizer<SfcsRuncardRangerRulesController> localizer)
        {
            _repository = repository;
            _sfcsPrintTasksRepository = sfcsPrintTasksRepository;
            _mapper = mapper;
            _sfcsProductConfigRepository = sfcsProductConfigRepository;
            _httpContextAccessor = httpContextAccessor;
            _repositoryt = repositoryt;
            _localizer = localizer;
        }

        /// <summary>
        /// 
        /// </summary>
        public class IndexVM
        {
            /// <summary>
            /// 状态列表
            /// </summary>
            public List<dynamic> StatusList { get; set; }

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
                            DigitalList = await _repository.GetListByTable("LOOKUP_CODE,MEANING,DESCRIPTION", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'RADIX_TYPE' AND ENABLED='Y' ORDER BY MEANING"),
                            StatusList = await _repository.GetListByTable("LOOKUP_CODE,MEANING,DESCRIPTION", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'RANGER_STATUS' ORDER BY LOOKUP_CODE"),
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
        public async Task<ApiBaseReturn<List<dynamic>>> LoadData([FromQuery] SfcsRuncardRangerRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.LoadData(model);
                    returnVM.Result = resdata?.data;
                    returnVM.TotalCount = resdata?.count ?? 0;

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
        /// 获取工单流水号范围 数据
        /// </summary>
        /// <param name="wo_no">工单号</param>
        /// <param name="rule_type">规则类型 0: 流水号规则 1: 箱号规则 2: 栈板号规则</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SfcsRuncardRanger>> GetRuncardRangerData([FromQuery] string wo_no)
        {
            ApiBaseReturn<SfcsRuncardRanger> returnVM = new ApiBaseReturn<SfcsRuncardRanger>();
            SfcsRuncardRanger runcardRanger = new SfcsRuncardRanger();

            #region 定义变量

            string errmsg = string.Empty;
            SfcsWo sfcsWo = null;
            string conditions = string.Empty;
            decimal routeID;
            string snformat = string.Empty;
            decimal quantity = 0;
            decimal platformID = 0;
            string platform = null;
            decimal familyId = 0;
            string familyName = "";
            SfcsPn partNumberRow = null;
            SfcsCustomers customer = null;
            SfcsProductFamily productFamilyRow = null;

            #endregion

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && wo_no.IsNullOrWhiteSpace())
                    {
                        //请录入工单号。
                        ErrorInfo.Set(_localizer["Err_WoOrderNO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        conditions = "WHERE WO_NO =:WO_NO";
                        sfcsWo = await _repository.GetAsyncEx<SfcsWo>(conditions, new { WO_NO = wo_no });
                        if (sfcsWo == null)
                        {
                            //工单{0}不存在，请确认。
                            errmsg = string.Format(_localizer["Err_WO_Not_Found"], wo_no);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            runcardRanger.WO_ID = sfcsWo.ID;
                            returnVM.Result = runcardRanger;
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        //獲得產品基本信息
                        string partNumber = sfcsWo.PART_NO;
                        partNumberRow = await _repository.GetAsyncEx<SfcsPn>("WHERE PART_NO =:PART_NO", new { PART_NO = partNumber });
                        if (partNumberRow == null)
                        {
                            //产品料号信息不存在，请确认。
                            errmsg = string.Format(_localizer["Err_PN_Not_Found"], partNumber);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (!partNumberRow.IsNullOrWhiteSpace() && !partNumberRow.FAMILY_ID.IsNullOrEmpty() && partNumberRow.FAMILY_ID > 0)
                        {
                            productFamilyRow = await _repository.GetAsyncEx<SfcsProductFamily>("WHERE ID =:ID", new { ID = partNumberRow.FAMILY_ID });
                            familyId = productFamilyRow.IsNullOrWhiteSpace() ? 0 : Convert.ToDecimal(productFamilyRow.ID);
                            familyName = productFamilyRow.IsNullOrWhiteSpace() ? "" : productFamilyRow.FAMILY_NAME;
                        }
                    }

                    //if (!ErrorInfo.Status)
                    //{
                    //    //獲得客户信息
                    //    customer = await _repository.GetAsyncEx<SfcsCustomers>("WHERE ID =:ID", new { ID = partNumberRow.CUSTOMER_ID });
                    //    if (customer == null)
                    //    {
                    //        //客户信息不存在，请确认。
                    //        ErrorInfo.Set(_localizer["Err_Customer_Not_Found"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //    }
                    //}

                    if (!ErrorInfo.Status)
                    {
                        //獲得產品基本信息
                        string partNumber = sfcsWo.PART_NO;
                        decimal woID = sfcsWo.ID;
                        decimal plantCode = sfcsWo.PLANT_CODE;

                        sfcsWo.ROUTE_ID = sfcsWo.ROUTE_ID > 0 ? sfcsWo.ROUTE_ID : _sfcsProductConfigRepository.GetRouteIdByPartNo(sfcsWo.PART_NO);
                        routeID = sfcsWo.ROUTE_ID;
                        decimal woTargetQty = await GetWorkOrderTargetQTY(woID);
                        decimal rangeSetQty = await GetRuncardRangerQTY(woID);

                        //判断目标量与当前设定量
                        //if (woTargetQty <= rangeSetQty)
                        //{
                        //	throw new MESException(Properties.Resources.Err_Ranger_QTY_Full, woOrderNO);
                        //}
                        //配置是否启用？号
                        bool isEnable = true;
                        var parameterModle = await _repository.GetListByTableEX<SfcsParameters>("MEANING", "SFCS_PARAMETERS", " AND LOOKUP_TYPE='MES_PRODUCTION_CONFIG_DEFUALT' AND ENABLED='Y' ");
                        isEnable = parameterModle != null && parameterModle.Count > 0 ? Convert.ToBoolean(parameterModle.FirstOrDefault().MEANING.ToUpper()) : isEnable;
                        if (isEnable)
                        {
                            snformat = await GetSNFormat(partNumber, false);
                        }
                        else
                        {
                            snformat = "?";
                        }

                        if (woTargetQty - rangeSetQty > 0)
                        {
                            quantity = woTargetQty - rangeSetQty;
                        }
                        // 自動創建流水號範圍
                        await AutoCreateRuncardRanger(runcardRanger, sfcsWo.WO_NO, partNumber, familyId, familyName, platformID, platform, quantity, snformat, customer, GlobalVariables.RangerSN);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = runcardRanger;
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
        /// 
        /// </summary>
        public class PrintSnRequestModel
        {
            /// <summary>
            /// 流水号范围对象
            /// </summary>
            public List<SfcsRuncardRanger> modelList { get; set; }
            /// <summary>
            /// 用户名称
            /// </summary>
            public string UserName { get; set; }
        }

        /// <summary>
        /// 打印产生的流水号(弃用)  
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回打印任务ID</returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> PrintSnRanger([FromBody] SfcsRuncardRanger model)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (model == null)
                    {
                        throw new Exception(_localizer["Err_GetRuncardRanger"]);
                    }
                    model = await _repository.GetAsync(model.ID);
                    if (model.PRINTED == "Y")
                    {
                        throw new Exception(_localizer["Err_RangerhavedPrinte"]);
                    }
                    #endregion

                    #region 保存并返回
                    List<string> snList = await GenerateRangerSN(model);
                    if (snList == null || snList.Count <= 0)
                    {
                        throw new Exception(_localizer["Err_GetSnData"]);
                    }
                    String printMappSql = @"select SPF.* from SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES  SPF 
                        where SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 1";
                    var sfcsPnlist = await _repository.QueryAsyncEx<SfcsPn>("select SP.* from SFCS_PN SP, SFCS_WO SW where SP.PART_NO = SW.PART_NO AND SW.ID = :ID",
                        new
                        {
                            ID = model.WO_ID
                        });
                    SfcsPn sfcsPn = sfcsPnlist.FirstOrDefault();
                    String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
                    SfcsPrintFiles sfcsPrintFiles = null;
                    List<SfcsPrintFiles> sfcsPrintMapplist = null;
                    sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByPn,
                        new
                        {
                            PART_NO = sfcsPn.PART_NO
                        });

                    if (sfcsPrintMapplist == null)
                    {
                        String printMappSqlByModel = printMappSql + " AND SPFM.MODEL_ID = :MODEL_ID";
                        sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByModel,
                        new
                        {
                            MODEL_ID = sfcsPn.MODEL_ID
                        });
                    }
                    if (sfcsPrintMapplist == null)
                    {
                        String printMappSqlByFamilly = printMappSql + " AND SPFM.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID";
                        sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByFamilly,
                        new
                        {
                            PRODUCT_FAMILY_ID = sfcsPn.FAMILY_ID
                        });
                    }
                    if (sfcsPrintMapplist == null)
                    {
                        String printMappSqlByCustor = printMappSql + " AND SPFM.CUSTOMER_ID = :CUSTOMER_ID";

                        sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByCustor,
                        new
                        {
                            CUSTOMER_ID = sfcsPn.CUSTOMER_ID
                        });
                    }
                    //默认产品条码模板
                    if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                    {
                        sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByPn,
                        new
                        {
                            PART_NO = "000000"
                        });
                    }
                    if (sfcsPrintMapplist != null && sfcsPrintMapplist.Count > 0)
                    {
                        sfcsPrintFiles = sfcsPrintMapplist.FirstOrDefault();
                    }
                    else
                    {
                        throw new Exception(_localizer["Err_SetProductPrintFile"]);
                    }
                    //获取产品打印附加值
                    String detail = "", detailValue = "", header = "";
                    detail = _repository.QueryEx<String>("SELECT CONFIG_VALUE FROM SFCS_PRODUCT_CONFIG WHERE PART_NO =:PART_NO AND CONFIG_TYPE =:CONFIG_TYPE AND ENABLED='Y'",
                                new { PART_NO = sfcsPn.PART_NO, CONFIG_TYPE = GlobalVariables.SNPrintData }).FirstOrDefault();
                    if (detail != null && detail != "")
                    {
                        var detailArr = detail.Split("|");
                        for (int i = 0; i < detailArr.Length; i++)
                        {
                            header += "," + GlobalVariables.PrintHeader + (i + 1);
                            detailValue += "," + detailArr[i];
                        }
                    }
                    ImsPart imsPart = _repository.QueryEx<ImsPart>("SELECT * FROM IMS_PART WHERE CODE = :CODE",
                        new { CODE = sfcsPn.PART_NO }).FirstOrDefault();

                    StringBuilder stringBuilder = new StringBuilder();
                    DateTime dateTime = DateTime.Now;
                    stringBuilder.AppendLine(String.Format("PN,PN_NAME,MODEL,SN,CREATE_TIME,QR_NO{0}", header));
                    foreach (String sn in snList)
                    {
                        String qrNo = String.Format("{0}|{1}|{2}|{3}|{4}", imsPart.CODE, imsPart.NAME, imsPart.DESCRIPTION, sn, detail);
                        stringBuilder.AppendLine(String.Format("{0},{1},{2},{3},{4},{5}{6}", imsPart.CODE, imsPart.NAME, imsPart.DESCRIPTION, sn, dateTime.ToString(), qrNo, detailValue));
                    }
                    decimal printTaskId = await _sfcsPrintTasksRepository.GetSEQID();
                    bool result = await _sfcsPrintTasksRepository.InsertPrintTask(printTaskId, sfcsPrintFiles.ID, "MES", stringBuilder.ToString(), "", sfcsPn.PART_NO);
                    if (result)
                    {
                        returnVM.Result = printTaskId;
                    }
                    else
                    {
                        throw new Exception(_localizer["Err_SetProductPrintFile"]);
                    }
                    await _repository.UpdatePrint(model.ID);
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
        /// 打印产生的流水号(批量)  
        /// </summary>
        /// <param name="printRequestModel"></param>
        /// <returns>返回打印任务ID</returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<PrintReturnModel>> PrintSnRangerEx([FromBody] PrintSnRequestModel printRequestModel)
        {
            ApiBaseReturn<PrintReturnModel> returnVM = new ApiBaseReturn<PrintReturnModel>();
            PrintReturnModel printReturnModel = new PrintReturnModel();
            //任务ID
            List<decimal> taskIdList = new List<decimal>();
            //消息列
            List<String> msgList = new List<string>();
            printReturnModel.TaskIdList = taskIdList;
            printReturnModel.MsgList = msgList;
            returnVM.Result = printReturnModel;

            var sfcsRuncardModel = new SfcsRuncardRanger();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (printRequestModel == null || printRequestModel.modelList == null || printRequestModel.modelList.Count <= 0)
                    {
                        throw new Exception(_localizer["Err_GetRuncardRanger"]);
                    }

                    #endregion

                    #region 保存并返回

                    foreach (var model in printRequestModel.modelList)
                    {
                        sfcsRuncardModel = await _repository.GetAsync(model.ID);
                        //获取工单
                        var woNoModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND ID=:ID ", new { id = model.WO_ID }))?.FirstOrDefault();
                        var woNo = woNoModel == null ? "" : woNoModel.WO_NO;
                        var part_no = woNoModel == null ? "" : woNoModel.PART_NO;
                        if (sfcsRuncardModel != null && sfcsRuncardModel.PRINTED == "Y")
                        {
                            //throw new Exception(_localizer["Err_RangerhavedPrinte"]);
                            msgList.Add(string.Format(_localizer["Err_RangerhavedPrinteEx"], woNo, sfcsRuncardModel.SN_BEGIN));
                            continue;
                        }
                        List<string> snList = await GenerateRangerSN(model);
                        if (snList == null || snList.Count <= 0)
                        {
                            //throw new Exception(_localizer["Err_GetSnData"]);
                            //工单:{0}对应的流水号:{1},计算SN异常!
                            msgList.Add(string.Format(_localizer["Err_GetSnDataEx"], woNo, sfcsRuncardModel.SN_BEGIN));
                            continue;
                        }
                        SfcsPrintTasks printTasks = await GetPrintFiles(model.WO_ID, snList);

                        bool result = await _sfcsPrintTasksRepository.InsertPrintTask(printTasks.ID, (decimal)printTasks.PRINT_FILE_ID, printRequestModel.UserName, printTasks.PRINT_DATA, woNo, printTasks.PART_NO);
                        if (result)
                        {
                            taskIdList.Add(printTasks.ID);
                        }
                        else
                        {
                            //更新数据异常位于:工单:{0}对应的流水号:{1}
                            msgList.Add(string.Format(_localizer["Err_Update_TaskId"], woNo, model.SN_BEGIN));
                            continue;
                        }
                        await _repository.UpdatePrint(model.ID);

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
        /// 校验数据并计算流水号范围等信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<SfcsRuncardRanger>> GetCalculateRanger([FromBody] SfcsRuncardRanger model)
        {
            ApiBaseReturn<SfcsRuncardRanger> returnVM = new ApiBaseReturn<SfcsRuncardRanger>();
            string conditions = string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.WO_ID <= 0)
                    {
                        //请录入工单号。
                        ErrorInfo.Set(_localizer["Err_WoOrderNO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && (model.RANGE == null || model.RANGE <= 0))
                    {
                        //变化位数不能小于1， 请录入正确的变化位数。
                        ErrorInfo.Set(_localizer["Err_RANGE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await GetRADIX_EXCLUSIVE(Convert.ToString(model.DIGITAL));
                        //除36進制外其余進制必須設定RADIX_EXCLUSIVE
                        if (resdata == null)
                        {
                            if ((decimal)model.DIGITAL != 1)
                            {
                                //字典参数配置不完整。
                                ErrorInfo.Set(_localizer["Err_ParametersNotExisted"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else
                        {
                            model.EXCLUSIVE_CHAR = resdata.DESCRIPTION;
                        }
                    }

                    #endregion

                    #region 计算流水号范围

                    if (!ErrorInfo.Status)
                    {
                        conditions = "WHERE ID =:ID";
                        SfcsWo sfcsWo = await _repository.GetAsyncEx<SfcsWo>(conditions, new { ID = model.WO_ID });
                        if (sfcsWo == null)
                        {
                            //工单不存在，请确认。
                            ErrorInfo.Set(_localizer["Err_WO_ID_Not_Found"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        //产品基本信息
                        string partNumber = sfcsWo.PART_NO;
                        SfcsPn partNumberRow = await _repository.GetAsyncEx<SfcsPn>("WHERE PART_NO =:PART_NO", new { PART_NO = partNumber });

                        //配置是否启用？号
                        string snformat = string.Empty;
                        bool isEnable = true;
                        var parameterModle = await _repository.GetListByTableEX<SfcsParameters>("MEANING", "SFCS_PARAMETERS", " AND LOOKUP_TYPE='MES_PRODUCTION_CONFIG_DEFUALT' AND ENABLED='Y' ");
                        isEnable = parameterModle != null && parameterModle.Count > 0 ? Convert.ToBoolean(parameterModle.FirstOrDefault().MEANING.ToUpper()) : isEnable;
                        if (isEnable)
                        {
                            snformat = await GetSNFormat(partNumber, true, model.SN_BEGIN);
                        }
                        else
                        {
                            snformat = "?";
                        }

                        await CalculateRanger(model, snformat);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = model;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 通过Excel导入SN到数据库并进行数据校验 (关联表IMPORT_RUNCARD_SN)
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="sn"></param>
        /// <param name="sn2"></param>
        /// <param name="imei1"></param>
        /// <param name="imei2"></param>
        /// <param name="meid"></param>
        /// <param name="mac"></param>
        /// <param name="bt"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<ResultMsg>> SaveExcelData([FromForm] String userName, [FromForm] String sn, [FromForm] String sn2, [FromForm] String imei1, [FromForm] String imei2, [FromForm] String meid,[FromForm] String mac, [FromForm] String bt)
        {
            ApiBaseReturn<ResultMsg> returnVM = new ApiBaseReturn<ResultMsg>();
            ResultMsg resultVM = new ResultMsg();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;
            var newFileName = string.Empty;
            string errmsg = string.Empty;
            SaveExcelDataRequestModel model = new SaveExcelDataRequestModel() { USER_NAME = userName, SN_CONFIG_VALUE = sn, SN2_CONFIG_VALUE = sn2, IMEI1_CONFIG_VALUE = imei1, IMEI2_CONFIG_VALUE = imei2, MEID_CONFIG_VALUE = meid, MAC_CONFIG_VALUE = mac, BT_CONFIG_VALUE = bt };
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        try
                        {
                            excelFile = Request.Form.Files[0];
                        }
                        catch (Exception)
                        {
                            //未找到上传的文件,请重新上传。
                            ErrorInfo.Set(_localizer["nofind_file"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && (excelFile == null || excelFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        source_filename = ContentDispositionHeaderValue
                                     .Parse(excelFile.ContentDisposition)
                                     .FileName
                                     .Trim('"');
                        extname = source_filename.Substring(source_filename.LastIndexOf("."), source_filename.Length - source_filename.LastIndexOf("."));

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(excelFile.Length / 1024.00, 3));
                        long mb = excelFile.Length / 1024 / 1024; // MB
                        if (mb > 10)
                        {
                            //"只允许上传小于 10MB 的文件."
                            ErrorInfo.Set(_localizer["size_10m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion
                    }

                    #endregion

                    #region 保存文件并解释

                    if (!ErrorInfo.Status)
                    {
                        newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var pathRoot = AppContext.BaseDirectory + @"upload\CustomerSN\";
                        if (Directory.Exists(pathRoot) == false)
                        {
                            Directory.CreateDirectory(pathRoot);
                        }
                        save_filename = pathRoot + $"{newFileName}";
                        using (FileStream fs = System.IO.File.Create(save_filename))
                        {
                            excelFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        resultVM = await LoadRuncardRangerExcelFile(save_filename, model);
                        if (resultVM.Code == 0)
                        {
                            ErrorInfo.Set(resultVM.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = resultVM;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 导出SN的数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<string>>> ExportSNRangerData([FromQuery] SfcsRuncardRangerRequestModel model)
        {
            ApiBaseReturn<List<string>> returnVM = new ApiBaseReturn<List<string>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    string condition = " ";

                    if (!string.IsNullOrWhiteSpace(model.WO_NO))
                    {
                        condition += $" and INSTR(SO.WO_NO,:WO_NO)>0 ";
                    }
                    if (!string.IsNullOrWhiteSpace(model.SN_BEGIN))
                    {
                        condition += $" and INSTR(RR.SN_BEGIN,:SN_BEGIN)>0 ";
                    }

                    //获取流水号范围信息
                    var sfcsRuncardRangerList = await _repository.GetListByTableEX<SfcsRuncardRanger>("*", "SFCS_RUNCARD_RANGER RR, SFCS_WO SO", " AND RR.WO_ID = SO.ID " + condition
                        , new { WO_NO = model.WO_NO, SN_BEGIN = model.SN_BEGIN });

                    //SN结果集
                    List<string> snList = new List<string>();

                    if (sfcsRuncardRangerList.Count > 0)
                    {
                        foreach (var item in sfcsRuncardRangerList)
                        {
                            var generateRangerSNList = (await GenerateRangerSN(item));
                            if (generateRangerSNList.Count > 0)
                            {
                                //查询的结果集放到SNList
                                snList.AddRange(generateRangerSNList);
                            }
                        }
                    }
                    returnVM.Result = snList;

                    #endregion


                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsRuncardRanger model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            string conditions = string.Empty;
            SfcsWo sfcsWo = null;
            string snformat = string.Empty;
            var rulesModel = new SfcsRuncardRangerRules();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.WO_ID <= 0)
                    {
                        //请录入工单号。
                        ErrorInfo.Set(_localizer["Err_WoOrderNO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.RANGER_RULE_ID <= 0)
                    {
                        //请设置条码管理规则，请注意检查!
                        ErrorInfo.Set(_localizer["Err_RangleRule"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        rulesModel = (await _repository.GetListByTableEX<SfcsRuncardRangerRules>("*", "SFCS_RUNCARD_RANGER_RULES", " AND ID=:ID", new { ID = model.RANGER_RULE_ID }))?.FirstOrDefault();
                    }

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await GetRADIX_EXCLUSIVE(Convert.ToString(model.DIGITAL));
                        //除36進制外其余進制必須設定RADIX_EXCLUSIVE
                        if (resdata == null)
                        {
                            if ((decimal)model.DIGITAL != 1)
                            {
                                //字典参数配置不完整。
                                ErrorInfo.Set(_localizer["Err_ParametersNotExisted"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else
                        {
                            model.EXCLUSIVE_CHAR = resdata.DESCRIPTION;
                        }
                    }
                    if (!ErrorInfo.Status)
                    {
                        conditions = "WHERE ID =:ID";
                        sfcsWo = await _repository.GetAsyncEx<SfcsWo>(conditions, new { ID = model.WO_ID });
                        if (sfcsWo == null)
                        {
                            //工单不存在，请确认。
                            ErrorInfo.Set(_localizer["Err_WO_ID_Not_Found"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        //产品基本信息
                        string partNumber = sfcsWo.PART_NO;
                        //SfcsPn partNumberRow = await _repository.GetAsyncEx<SfcsPn>("WHERE PART_NO =:PART_NO", new { PART_NO = partNumber });

                        //配置是否启用？号
                        bool isEnable = true;
                        var parameterModle = await _repository.GetListByTableEX<SfcsParameters>("MEANING", "SFCS_PARAMETERS", " AND LOOKUP_TYPE='MES_PRODUCTION_CONFIG_DEFUALT' AND ENABLED='Y' ");
                        isEnable = parameterModle != null && parameterModle.Count > 0 ? Convert.ToBoolean(parameterModle.FirstOrDefault().MEANING.ToUpper()) : isEnable;
                        if (isEnable)
                        {
                            snformat = await GetSNFormat(partNumber, true, model.SN_BEGIN);
                        }
                        else
                        {
                            snformat = "?";
                        }



                        await CalculateRanger(model, snformat, rulesModel);
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
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 拼板单码打印
        /// </summary>
        /// <param name="printPuzzleCode"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> PrintPuzzleSingleCode(PrintPuzzleCodeRequestModel printPuzzleCode)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if ((printPuzzleCode.Id.IsNullOrEmpty() || printPuzzleCode.Id <= 0) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["RUNCARD_RANGER_ID_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    SfcsRuncardRanger model = (await _repository.GetListByTableEX<SfcsRuncardRanger>("*", "SFCS_RUNCARD_RANGER", " And ID=:ID", new { ID = printPuzzleCode.Id })).FirstOrDefault();
                    if (model == null && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["RUNCARD_RANGER_DATA_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else if (model.PRINTED == "Y" && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["Err_RangerhavedPrinte"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (printPuzzleCode.BoardQty <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["BOARD_QTY_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    decimal remainder = model.QUANTITY / printPuzzleCode.BoardQty;//board_qty 主码需要打印的数量 printQty 每个板对应的sn数量  printQty - 1 =每个主码它的子码数量是  
                    int printQty = Convert.ToInt32(remainder);
                    if (remainder != printQty && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SN_QTY_NOT_ENOUGH"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    SfcsWo woNoModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND ID=:ID ", new { id = model.WO_ID })).FirstOrDefault();
                    if (woNoModel == null)
                    {
                        ErrorInfo.Set(_localizer["Err_WO_ID_Not_Found"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        List<string> snList = await GenerateRangerSN(model);
                        if (snList != null && snList.Count() >= model.QUANTITY)
                        {
                            int i = 0;
                            List<string> snHeaderList = new List<string>();
                            SfcsWoRgMultiHeader headerModel = null;
                            List<SfcsWoRgMultiHeader> headerList = new List<SfcsWoRgMultiHeader>();
                            List<SfcsWoRgMultiDetail> detailList = new List<SfcsWoRgMultiDetail>();
                            foreach (var sn in snList)
                            {
                                if (i == 0 || i == printQty)
                                {
                                    i = 0;
                                    headerModel = new SfcsWoRgMultiHeader();
                                    //添加主码数据
                                    headerModel.ID = await _repository.GetSEQID("SFCS_WO_RG_MULTI_HEADER_SEQ");
                                    headerModel.WO_RANGER_ID = printPuzzleCode.Id;
                                    headerModel.STATUS = 0;
                                    headerModel.SN = sn;
                                    headerModel.WO_NO = woNoModel.WO_NO;
                                    headerModel.SPLICING_QTY = printPuzzleCode.BoardQty;
                                    headerModel.CREATE_USER = printPuzzleCode.UserName;
                                    snHeaderList.Add(sn);
                                    headerList.Add(headerModel);
                                }
                                else
                                {
                                    //添加子码数据
                                    SfcsWoRgMultiDetail detailModel = new SfcsWoRgMultiDetail();
                                    detailModel.ID = await _repository.GetSEQID("SFCS_WO_RG_MULTI_DETAIL_SEQ");
                                    detailModel.SN = sn;
                                    detailModel.MST_ID = headerModel.ID;
                                    detailModel.CREATE_USER = printPuzzleCode.UserName;
                                    detailList.Add(detailModel);
                                }
                                i++;
                            }
                            //根据主码生成主码的打印数据并返回前端进行打印
                            SfcsPrintTasks printTasks = await GetPrintFiles(model.WO_ID, snHeaderList);
                            printTasks.WO_NO = woNoModel.WO_NO;
                            printTasks.OPERATOR = printPuzzleCode.UserName;
                            returnVM.Result = await _repository.PrintPuzzleSingleCode(printTasks, headerList, detailList) == true ? printTasks.ID : 0;

                            if (returnVM.Result > 0) { await _repository.UpdatePrint(model.ID); } //修改流水号范围数据为已打印

                        }
                        else
                        {
                            ErrorInfo.Set(_localizer["SN_QTY_NOT_ENOUGH"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 拼板余码打印
        /// </summary>
        /// <param name="sn">主码SN</param>
        /// <param name="UserName">操作用户</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> PrintPuzzleRemainingCodeBySN(String sn, String UserName)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (sn.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SN_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    SfcsWoRgMultiHeader headerModel = null;
                    if (!ErrorInfo.Status)
                    {
                        headerModel = (await _repository.GetListByTableEX<SfcsWoRgMultiHeader>("*", "SFCS_WO_RG_MULTI_HEADER", " AND SN=:SN ", new { SN = sn })).FirstOrDefault();
                        if (headerModel == null && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["GET_PRINT_DATA_FAIL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (headerModel.STATUS > 0 && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["HEADER_STATUS_PRINT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    List<SfcsWoRgMultiDetail> detailList = null;
                    if (!ErrorInfo.Status)
                    {
                        detailList = await _repository.GetListByTableEX<SfcsWoRgMultiDetail>("*", "SFCS_WO_RG_MULTI_DETAIL", " AND MST_ID=:MST_ID ", new { MST_ID = headerModel.ID });
                        if ((detailList == null || detailList.Count() < 1) && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["GET_PRINT_DATA_FAIL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    SfcsWo woNoModel = null;
                    if (!ErrorInfo.Status)
                    {
                        woNoModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND WO_NO=:WO_NO ", new { WO_NO = headerModel.WO_NO })).FirstOrDefault();
                        if (woNoModel == null && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["Err_WO_ID_Not_Found"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        List<string> snList = new List<string>();
                        foreach (var item in detailList)
                        {
                            snList.Add(item.SN);
                        }

                        SfcsPrintTasks printTasks = await GetPrintFiles(woNoModel.ID, snList);
                        printTasks.WO_NO = woNoModel.WO_NO;
                        printTasks.OPERATOR = UserName;
                        headerModel.UPDATE_USER = UserName;
                        returnVM.Result = await _repository.PrintPuzzleRemainingCodeBySN(printTasks, headerModel) == true ? printTasks.ID : 0;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #region 导入特殊SN相关

        ///// <summary>
        ///// 加载导入特殊SN汇总数据
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Authorize]
        //private async Task<ApiBaseReturn<List<dynamic>>> LoadImportRuncardSnSummaryData([FromQuery] ImportRuncardSnRequestModel model)
        //{
        //    ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 参数验证
        //            if (model.HEADER_ID > 1 && !ErrorInfo.Status)
        //            {
        //                ErrorInfo.Set(_localizer["Err_UserNotEixst"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //            }
        //            if (String.IsNullOrEmpty(model.USER_NAME) && !ErrorInfo.Status)
        //            {
        //                ErrorInfo.Set(_localizer["Err_UserNotEixst"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //            }
        //            #endregion

        //            #region 设置返回值

        //            var resdata = await _repository.LoadImportRuncardSnSummaryData(model);
        //            returnVM.Result = resdata?.data;
        //            returnVM.TotalCount = resdata?.count ?? 0;

        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
        //        }
        //    }

        //    #region 如果出现错误，则写错误日志并返回错误内容

        //    WriteLog(ref returnVM);

        //    #endregion

        //    return returnVM;
        //}

        ///// <summary>
        ///// 根据id加载导入特殊SN数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Authorize]
        //private async Task<ApiBaseReturn<ImportRuncardSn>> LoadImportRuncardSnByid(int id)
        //{
        //    ApiBaseReturn<ImportRuncardSn> returnVM = new ApiBaseReturn<ImportRuncardSn>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 设置返回值

        //            returnVM.Result = (await _repository.GetListByTableEX<ImportRuncardSn>("*", "IMPORT_RUNCARD_SN", " AND ID = :ID", new { ID = id }))?.FirstOrDefault();

        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
        //        }
        //    }

        //    #region 如果出现错误，则写错误日志并返回错误内容

        //    WriteLog(ref returnVM);

        //    #endregion

        //    return returnVM;
        //}

        /// <summary>
        /// 加载导入特殊SN主表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadImportRuncardHeaderData([FromQuery] ImportRuncardHeaderRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 设置返回值

                    var resdata = await _repository.LoadImportRuncardHeaderData(model);
                    returnVM.Result = resdata?.data;
                    returnVM.TotalCount = resdata?.count ?? 0;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 加载导入特殊SN数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadImportRuncardSnData([FromQuery] ImportRuncardSnRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 参数验证
                    if (model.HEADER_ID < 1 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["Err_UserNotEixst"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    //if (String.IsNullOrEmpty(model.USER_NAME) && !ErrorInfo.Status)
                    //{
                    //    ErrorInfo.Set(_localizer["Err_UserNotEixst"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //}
                    #endregion

                    #region 设置返回值

                    var resdata = await _repository.LoadImportRuncardSnData(model);
                    returnVM.Result = resdata?.data;
                    returnVM.TotalCount = resdata?.count ?? 0;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 保存导入特殊SN数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveImportRuncardSnDataByTrans([FromBody] ImportRuncardSnModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    //if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    //{
                    //    SfcsWo wo = await _repository.GetAsyncEx<SfcsWo>("WHERE WO_NO =:WO_NO", new { WO_NO = model.InsertRecords[0].WO_NO });

                    //    foreach (var item in model.InsertRecords)
                    //    {
                    //        if (wo.IsNullOrWhiteSpace()) { continue; }
                    //        item.SN = await GetSNByWoId(wo.ID);
                    //        if (item.SN.IsNullOrEmpty()) { continue; }
                    //    }
                    //}

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveImportRuncardSnDataByTrans(model);
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
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 根据主表批量删除导入SN数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveImportRuncardHeaderByTrans([FromBody] ImportRuncardHeaderModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveImportRuncardHeaderByTrans(model);
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
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 打印导入的流水号(批量)  
        /// </summary>
        /// <param name="printRequestModel"></param>
        /// <returns>返回打印任务ID</returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<PrintReturnModel>> PrintImportRuncardSN([FromBody] PrintImportRuncardSNRequestModel printRequestModel)
        {
            ApiBaseReturn<PrintReturnModel> returnVM = new ApiBaseReturn<PrintReturnModel>();
            PrintReturnModel printReturnModel = new PrintReturnModel();
            //任务ID
            List<decimal> taskIdList = new List<decimal>();
            //消息列
            List<String> msgList = new List<string>();
            printReturnModel.TaskIdList = taskIdList;
            printReturnModel.MsgList = msgList;
            returnVM.Result = printReturnModel;
            ImportRuncardHeaderListModel runcardHeaderModel = new ImportRuncardHeaderListModel();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (printRequestModel == null || printRequestModel.modelList == null || printRequestModel.modelList.Count <= 0)
                    {
                        throw new Exception(_localizer["Err_PrintImportRuncardSN"]);
                    }

                    #endregion

                    #region 保存并返回
                    foreach (var item in printRequestModel.modelList)
                    {
                        runcardHeaderModel = (await _repository.GetListByTableEX<ImportRuncardHeaderListModel>("*", "IMPORT_RUNCARD_HEADER", " AND ID=:ID ", new { ID = item.ID }))?.FirstOrDefault();
                        //获取工单
                        var woNoModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND WO_NO=:WO_NO ", new { WO_NO = runcardHeaderModel.WO_NO }))?.FirstOrDefault();
                        var woNo = woNoModel == null ? "" : woNoModel.WO_NO;
                        var part_no = woNoModel == null ? "" : woNoModel.PART_NO;
                        if (runcardHeaderModel != null && runcardHeaderModel.PRINTED == "Y")
                        {
                            msgList.Add(string.Format(_localizer["Err_PrintedImportWO"], runcardHeaderModel.WO_NO));
                            continue;
                        }
                        List<string> snList = (await _repository.GetListByTableEX<String>("SN", "IMPORT_RUNCARD_SN", " AND HEADER_ID =:HEADER_ID ", new { HEADER_ID = runcardHeaderModel.ID }));
                        if (snList == null || snList.Count <= 0)
                        {
                            msgList.Add(string.Format(_localizer["Err_GetImportSnData"], woNo));
                            continue;
                        }
                        SfcsPrintTasks printTasks = await GetPrintFiles(woNoModel.ID, snList);

                        bool result = await _sfcsPrintTasksRepository.InsertPrintTask(printTasks.ID, (decimal)printTasks.PRINT_FILE_ID, printRequestModel.UserName, printTasks.PRINT_DATA, woNo, printTasks.PART_NO);
                        if (result)
                        {
                            taskIdList.Add(printTasks.ID);
                        }
                        else
                        {
                            //更新数据异常位于:工单:{0}对应的流水号:{1}
                            msgList.Add(string.Format(_localizer["Err_Update_TaskId"], woNo, snList[0]));
                            continue;
                        }
                        await _repository.UpdatePrintImportRuncard(runcardHeaderModel.ID);

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

        public class PrintImportRuncardSNRequestModel
        {
            /// <summary>
            /// 导入列表
            /// </summary>
            public List<ImportRuncardHeaderListModel> modelList { get; set; }
            /// <summary>
            /// 用户名称
            /// </summary>
            public string UserName { get; set; }
        }
        #endregion

        #region 镭雕任务下达

        /// <summary>
        /// 镭雕任务下达数据列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadLaserTaskData([FromQuery] SfcsLaserTaskRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 设置返回值

                    var resdata = await _repository.LoadLaserTaskData(model);
                    returnVM.Result = resdata?.data;
                    returnVM.TotalCount = resdata?.count ?? 0;

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 镭雕任务下达
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> LaserTaskReleaseByType(LaserTaskReleaseRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    SfcsLaserTaskModel taskRelease = new SfcsLaserTaskModel();
                    taskRelease.InsertRecords = new List<SfcsLaserTaskAddOrModifyModel>();

                    #region 参数验证

                    if (model.USER_NAME.IsNullOrEmpty() && !ErrorInfo.Status) { throw new Exception("Err_UserNotEixst"); }
                    if (model.ID_LIST == null && !ErrorInfo.Status)
                    {
                        throw new Exception("ERR_TYPE_ID");
                    }

                    //生成镭雕任务下达数据
                    foreach (int id in model.ID_LIST)
                    {
                        SfcsLaserTaskAddOrModifyModel addTaskRelease = new SfcsLaserTaskAddOrModifyModel();
                        addTaskRelease.PRINT_QTY = 0;
                        addTaskRelease.CREATE_USER = model.USER_NAME;
                        addTaskRelease.MACHINE_CODE = model.MACHINE_CODE;
                        addTaskRelease.ENABLED = "N";

                        if (model.TASK_TYPE == 1)
                        {
                            addTaskRelease.TASK_TYPE = "1";

                            SfcsRuncardRanger runcardRanger = await _repository.GetAsync("SELECT * FROM SFCS_RUNCARD_RANGER WHERE ID = :ID", new { ID = id });
                            if (runcardRanger.IsNullOrWhiteSpace()) { throw new Exception("ERR_TYPE_ID"); }
                            addTaskRelease.TYPE_ID = runcardRanger.ID;
                            addTaskRelease.PRINT_TOTAL = Convert.ToInt32(runcardRanger.QUANTITY);

                            SfcsWo swModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND ID = :ID", new { ID = runcardRanger.WO_ID }))?.FirstOrDefault();
                            if (swModel.IsNullOrWhiteSpace()) { throw new Exception("Err_WO_ID_Not_Found"); }
                            addTaskRelease.WO_NO = swModel.WO_NO;
                            addTaskRelease.PART_NO = swModel.PART_NO;

                        }
                        else if (model.TASK_TYPE == 2)
                        {
                            addTaskRelease.TASK_TYPE = "2";

                            ImportRuncardHeader importRuncardHeader = (await _repository.GetListByTableEX<ImportRuncardHeader>("*", "IMPORT_RUNCARD_HEADER", " AND ID = :ID", new { ID = id }))?.FirstOrDefault();
                            if (importRuncardHeader.IsNullOrWhiteSpace() || importRuncardHeader.WO_NO.IsNullOrEmpty()) { throw new Exception("ERR_TYPE_ID"); }
                            addTaskRelease.TYPE_ID = importRuncardHeader.ID;
                            addTaskRelease.PRINT_TOTAL = importRuncardHeader.SN_QTY;
                            addTaskRelease.WO_NO = importRuncardHeader.WO_NO;
                            SfcsWo swModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND WO_NO = :WO_NO", new { WO_NO = addTaskRelease.WO_NO }))?.FirstOrDefault();
                            addTaskRelease.PART_NO = swModel != null ? swModel.PART_NO : "";

                        }
                        else
                        {
                            throw new Exception("ERR_TASK_TYPE");
                        }

                        ImsPart resData = addTaskRelease.PART_NO.IsNullOrEmpty() ? null : await _repositoryt.GetPartByPartNo(addTaskRelease.PART_NO);//根据料号获取产品信息
                        addTaskRelease.PART_DESC = resData != null ? resData.DESCRIPTION : "";

                        bool r = await _repository.ItemIsByLaserTask(addTaskRelease.TYPE_ID, addTaskRelease.TASK_TYPE);
                        if (r) { throw new Exception("LASER_TASK_RELEASE_ERR"); }
                        taskRelease.InsertRecords.Add(addTaskRelease);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveLaserTaskDataByTrans(taskRelease);
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
        /// 更新镭雕任务状态(批量)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> UpdateLaserTaskStatus(UpdateLaserTaskStatusRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    if (model.USER_NAME.IsNullOrEmpty() && !ErrorInfo.Status) { throw new Exception("Err_UserNotEixst"); }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.UpdateLaserTaskStatus(model);
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
        /// 删除镭雕任务(批量)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DelectLaserTask(UpdateLaserTaskStatusRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证

                    if (model.USER_NAME.IsNullOrEmpty() && !ErrorInfo.Status) { throw new Exception("Err_UserNotEixst"); }
                    List<LaserTaskStatusRequestModel> laserTaskStatusRequestModels = model.STATUS_LIST;
                    if (laserTaskStatusRequestModels == null && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["Err_PleasSelectData"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    decimal count = await _repository.GetSfcsLaserTask(laserTaskStatusRequestModels);
                    if (count > 0)
                    {
                        ErrorInfo.Set(_localizer["Err_SelectDataIsNoValite"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.DeleLaserTasks(laserTaskStatusRequestModels);
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
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #endregion

        #region 工单投入查询报表
        /// <summary>
        /// 工单投入查询报表
        /// </summary>
        /// <param name="WoNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<InputSNListModel>> LoadInputSNDataByWoNo(String WoNo)
        {
            ApiBaseReturn<InputSNListModel> returnVM = new ApiBaseReturn<InputSNListModel>();

            #region 设置返回值

            try
            {
                #region 设置返回值

                returnVM.Result = new InputSNListModel();
                returnVM.Result.WO_NO = WoNo.Trim();
                if (!String.IsNullOrEmpty(returnVM.Result.WO_NO))
                {
                    SfcsWo wo = await _repository.GetAsyncEx<SfcsWo>("WHERE WO_NO =:WO_NO", new { WO_NO = returnVM.Result.WO_NO });
                    if (wo != null)
                    {
                        List<String> IsSNList = new List<string>();
                        List<String> rangerSNList = new List<string>();
                        List<SfcsRuncardRanger> runcardRangerList = await _repository.GetListByTableEX<SfcsRuncardRanger>("RA.*", "SFCS_RUNCARD_RANGER RA,SFCS_RUNCARD_RANGER_RULES RU", " AND RA.RANGER_RULE_ID = RU.ID AND RU.RULE_TYPE = :RULE_TYPE AND RA.WO_ID = :WO_ID ORDER BY RA.ID DESC ", new { WO_ID = wo.ID, RULE_TYPE = GlobalVariables.RangerSN });
                        if (runcardRangerList != null && runcardRangerList.Count > 0)
                        {
                            foreach (var item in runcardRangerList)
                            {
                                List<String> logSNList = null;
                                List<String> runcardSNList = null;
                                if (!String.IsNullOrEmpty(item.FIX_HEADER) || !String.IsNullOrEmpty(item.FIX_TAIL))
                                {
                                    runcardSNList = await _repository.GetListByTableEX<String>("SN", "SFCS_RUNCARD", " AND SN LIKE :SN ORDER BY SN ASC ", new { SN = item.FIX_HEADER + "%" + item.FIX_TAIL });
                                    logSNList = await _repository.GetListByTableEX<String>("SN", "IMPORT_RUNCARD_SN", " AND SN LIKE :SN ORDER BY SN ASC ", new { SN = item.FIX_HEADER + "%" + item.FIX_TAIL });
                                }

                                if (logSNList == null) { logSNList = new List<String>(); }
                                if (runcardSNList == null) { runcardSNList = new List<String>(); }

                                runcardSNList = runcardSNList.Concat(logSNList).ToList();//合并
                                IsSNList = IsSNList.Concat(runcardSNList).ToList();
                                List<String> rangerSN = (await GenerateRangerSN(item));//当前范围所有的SN
                                rangerSNList = rangerSNList.Concat(rangerSN).ToList();
                            }

                            returnVM.Result.IsNotSNList = rangerSNList.Except(IsSNList).ToList();
                            returnVM.Result.IsSNList = rangerSNList.Except(returnVM.Result.IsNotSNList).ToList();
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format(_localizer["Err_WO_Not_Found"], WoNo));
                    }
                }
                else
                {
                    throw new Exception("Err_WoOrderNO");
                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #endregion

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
        #endregion

        #region 内部方法

        ///// <summary>
        ///// 解释文件
        ///// </summary>
        ///// <param name="planFilePath">路径</param>
        ///// <param name="userName">用户</param>
        ///// <returns></returns>
        //private async Task<ResultMsg> LoadRuncardRangerExcelFile(string planFilePath, string userName)
        //{
        //    ResultMsg result = new ResultMsg();
        //    object tmpValue;
        //    List<String> strMsgList = new List<String>();
        //    List<String> strStoreList = new List<string>();
        //    List<String> strNOList = new List<string>();
        //    List<Dictionary<string, string>> snList = new List<Dictionary<string, string>>();

        //    try
        //    {
        //        Workbook workbook = new Workbook(planFilePath);
        //        Worksheet sheet = workbook.Worksheets[0];

        //        string noIndex = "A"; //A --序号
        //        string woNoIndex = "B"; //B --工单
        //        string snIndex = "C"; //C --SN
        //        int startRow = 2;//开始的行

        //        //事务开启
        //        await _repository.StartTransaction();

        //        for (int i = startRow; i < sheet.Cells.Rows.Count + 1; i++)
        //        {
        //            ImportRuncardSnAddOrModifyModel snExcelModel = new ImportRuncardSnAddOrModifyModel() { CREATE_BY = userName, UPDATE_BY = userName };

        //            //工单号
        //            tmpValue = sheet.Cells[woNoIndex + i].Value;
        //            if (tmpValue != null) snExcelModel.WO_NO = tmpValue.ToString().Trim();

        //            //SN号处如果在导入数据里面不重复，就添加进数据库
        //            tmpValue = sheet.Cells[snIndex + i].Value;
        //            if (tmpValue != null && !snList.Any(c => c.Equals(tmpValue.ToString().Trim())))
        //            {
        //                snExcelModel.SN = tmpValue.ToString().Trim();
        //                var snDictionary = new Dictionary<string, string>();
        //                snDictionary.Add(i.ToString(), snExcelModel.SN);
        //                snList.Add(snDictionary);
        //            }
        //            else
        //            {
        //                //如果为空或者已经有重复的话就跳过
        //                continue;
        //            }

        //            var resultCode = await _repository.UpdateCustomerSNData(snExcelModel);
        //            if (resultCode == GlobalVariables.FailNUM)
        //            {
        //                //第{0}行数据异常，请注意检查! The data in row {0} is abnormal, please check!   
        //                strMsgList.Add(string.Format(_localizer["Err_Data_Row_Abnormal"], i.ToString()));
        //            }
        //        }

        //        //事务提交
        //        await _repository.CommitTransaction();
        //        if (strMsgList != null && strMsgList.Count() > 0)
        //        {
        //            result.Set(String.Join(';', strMsgList));
        //            result.Code = 1;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        await _repository.RollbackTransaction();
        //        result.Set(ex.Message);
        //    }
        //    finally
        //    {
        //        await _repository.CloseTransaction();
        //    }

        //    return result;
        //}

        /// <summary>
        /// 解释文件
        /// </summary>
        /// <param name="planFilePath">路径</param>
        /// <param name="userName">用户</param>
        /// <param name="model">用户</param>
        /// <returns></returns>
        private async Task<ResultMsg> LoadRuncardRangerExcelFile(string planFilePath, SaveExcelDataRequestModel model)
        {
            ResultMsg result = new ResultMsg();
            List<String> strMsgList = new List<String>();
            List<String> snList = new List<String>();

            try
            {
                Workbook workbook = new Workbook(planFilePath);
                Cells cells = workbook.Worksheets[0].Cells;
                System.Data.DataTable table = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);

                //事务开启
                await _repository.StartTransaction();

                if (table != null && table.Rows.Count > 0)
                {
                    decimal header_id = 0; int sn_qty = 0, i = 0;
                    List<String> runcardSnList = new List<String>();
                    runcardSnList = (await _repository.GetListByTableEX<ImportRuncardSn>("SN", "IMPORT_RUNCARD_SN", ""))?.Select(c => c.SN).ToList();

                    foreach (System.Data.DataRow row in table.Rows)
                    {
                        ImportRuncardSnAddOrModifyModel snExcelModel = new ImportRuncardSnAddOrModifyModel() { CREATE_BY = model.USER_NAME, UPDATE_BY = model.USER_NAME };
                        if (!row["WO_NO"].ToString().Trim().IsNullOrEmpty())
                        {
                            i++;

                            snExcelModel.WO_NO = row["WO_NO"].ToString().Trim();
                            if (header_id <= 0)
                            {
                                header_id = await _repository.AddImportRuncardHeader(snExcelModel);
                                if (header_id < 1) { throw new Exception("SAVE_IMPORTRUNCARDSN_HEADER_ERR"); }
                            }

                            snExcelModel.SN = row["SN"].ToString().Trim();
                            if (snExcelModel.SN.IsNullOrEmpty())
                            {
                                //SN为空，按工单号获取SN导入
                                SfcsWo wo = await _repository.GetAsyncEx<SfcsWo>("WHERE WO_NO =:WO_NO", new { WO_NO = snExcelModel.WO_NO });
                                if (wo.IsNullOrWhiteSpace()) { continue; }
                                snExcelModel.SN = await GetSNByWoId(wo.ID);
                                if (snExcelModel.SN.IsNullOrEmpty()) { continue; }

                            }

                            if (!snList.Exists(m => m == snExcelModel.SN) && !runcardSnList.Exists(m => m == snExcelModel.SN))
                            {
                                snList.Add(snExcelModel.SN);
                            }
                            else
                            {
                                //如果重复的话就跳过
                                continue;
                            }


                            if (!String.IsNullOrEmpty(model.SN_CONFIG_VALUE) && !FormatChecker.FormatCheck(snExcelModel.SN, model.SN_CONFIG_VALUE.Trim()))
                            {
                                throw new Exception(string.Format(_localizer["Err_SNFormat"], snExcelModel.SN, model.SN_CONFIG_VALUE.Trim()));
                            }

                            snExcelModel.SN2 = row["SN2"].ToString().Trim();
                            if (!String.IsNullOrEmpty(snExcelModel.SN2) && !String.IsNullOrEmpty(model.SN2_CONFIG_VALUE) && !FormatChecker.FormatCheck(snExcelModel.SN2, model.SN2_CONFIG_VALUE.Trim()))
                            {
                                throw new Exception(string.Format(_localizer["Err_SNFormat"], "2" + snExcelModel.SN2, model.SN2_CONFIG_VALUE.Trim()));
                            }

                            snExcelModel.MAIN_CARD_IMEI = row["MAIN_CARD_IMEI"].ToString().Trim();
                            if (!String.IsNullOrEmpty(snExcelModel.MAIN_CARD_IMEI) && !String.IsNullOrEmpty(model.IMEI1_CONFIG_VALUE) && !FormatChecker.FormatCheck(snExcelModel.MAIN_CARD_IMEI, model.IMEI1_CONFIG_VALUE.Trim()))
                            {
                                throw new Exception(string.Format("IMEI1" + _localizer["Err_DateFormat"], snExcelModel.MAIN_CARD_IMEI, model.IMEI1_CONFIG_VALUE.Trim()));
                            }

                            snExcelModel.MINOR_CARD_IMEI = row["MINOR_CARD_IMEI"].ToString().Trim();
                            if (!String.IsNullOrEmpty(snExcelModel.MINOR_CARD_IMEI) && !String.IsNullOrEmpty(model.IMEI2_CONFIG_VALUE) && !FormatChecker.FormatCheck(snExcelModel.MINOR_CARD_IMEI, model.IMEI2_CONFIG_VALUE.Trim()))
                            {
                                throw new Exception(string.Format("IMEI2" + _localizer["Err_DateFormat"], snExcelModel.MINOR_CARD_IMEI, model.IMEI2_CONFIG_VALUE.Trim()));
                            }

                            snExcelModel.BT = row["BT"].ToString().Trim();
                            if (!String.IsNullOrEmpty(snExcelModel.BT) && !String.IsNullOrEmpty(model.BT_CONFIG_VALUE) && !FormatChecker.FormatCheck(snExcelModel.BT, model.BT_CONFIG_VALUE.Trim()))
                            {
                                throw new Exception(string.Format("BT" + _localizer["Err_DateFormat"], snExcelModel.BT, model.BT_CONFIG_VALUE.Trim()));
                            }

                            snExcelModel.MAC = row["MAC"].ToString().Trim();
                            if (!String.IsNullOrEmpty(snExcelModel.MAC) && !String.IsNullOrEmpty(model.MAC_CONFIG_VALUE) && !FormatChecker.FormatCheck(snExcelModel.MAC, model.MAC_CONFIG_VALUE.Trim()))
                            {
                                throw new Exception(string.Format("MAC" + _localizer["Err_DateFormat"], snExcelModel.MAC, model.MAC_CONFIG_VALUE.Trim()));
                            }

                            snExcelModel.MEID = row["MEID"].ToString().Trim();
                            if (!String.IsNullOrEmpty(snExcelModel.MEID) && !String.IsNullOrEmpty(model.MEID_CONFIG_VALUE) && !FormatChecker.FormatCheck(snExcelModel.MEID, model.MEID_CONFIG_VALUE.Trim()))
                            {
                                throw new Exception(string.Format("MEID" + _localizer["Err_DateFormat"], snExcelModel.MEID, model.MEID_CONFIG_VALUE.Trim()));
                            }

                            snExcelModel.PART_TYPE = row["PART_TYPE"].ToString().Trim();
                            snExcelModel.PART_PN = row["PART_PN"].ToString().Trim();
                            snExcelModel.PART_EAN = row["PART_EAN"].ToString().Trim();
                            snExcelModel.CUSTOMER = row["CUSTOMER"].ToString().Trim();
                            snExcelModel.CUSTOMER_MODEL = row["CUSTOMER_MODEL"].ToString().Trim();
                            snExcelModel.CUSTOMER_CODE = row["CUSTOMER_CODE"].ToString().Trim();
                            snExcelModel.SUPPLY_LOCATION = row["SUPPLY_LOCATION"].ToString().Trim();
                            snExcelModel.DELIVERY_LOCATION = row["DELIVERY_LOCATION"].ToString().Trim();
                            snExcelModel.HARDWARE_VERSION = row["HARDWARE_VERSION"].ToString().Trim();
                            snExcelModel.SOFTWARE_VERSION = row["SOFTWARE_VERSION"].ToString().Trim();
                            snExcelModel.COLOUR = row["COLOUR"].ToString().Trim();
                            snExcelModel.CUSTOMER_BATCH_NO = row["CUSTOMER_BATCH_NO"].ToString().Trim();
                            snExcelModel.OUTER_BOX_NET_WEIGHT = row["OUTER_BOX_NET_WEIGHT"].ToString().Trim();
                            snExcelModel.OUTER_BOX_GROSS_WEIGHT = row["OUTER_BOX_GROSS_WEIGHT"].ToString().Trim();
                            snExcelModel.OUTER_BOX_QTY = row["OUTER_BOX_QTY"].ToString().Trim();

                            var resultCode = await _repository.UpdateCustomerSNData(snExcelModel, header_id);
                            if (resultCode == GlobalVariables.FailNUM)
                            {
                                //第{0}行数据异常，请注意检查! The data in row {0} is abnormal, please check!   
                                strMsgList.Add(string.Format(_localizer["Err_Data_Row_Abnormal"], i.ToString()));
                            }
                            else
                            {
                                sn_qty++;
                            }
                        }
                    }

                    await _repository.upSNHeaderByQty(header_id, sn_qty);

                }

                //事务提交
                await _repository.CommitTransaction();
                if (strMsgList != null && strMsgList.Count() > 0)
                {
                    result.Set(String.Join(';', strMsgList));
                    result.Code = 1;
                }

            }
            catch (Exception ex)
            {
                await _repository.RollbackTransaction();
                result.Set(ex.Message);
            }
            finally
            {
                await _repository.CloseTransaction();
            }

            return result;
        }

        /// <summary>
        /// 解释文件
        /// </summary>
        /// <param name="planFilePath">路径</param>
        /// <param name="userName">用户</param>
        /// <returns></returns>
        private async Task<ResultMsg> LoadRuncardRangerExcelFileEx(string planFilePath, string userName)
        {
            ResultMsg result = new ResultMsg();
            object tmpValue;
            List<String> strMsgList = new List<String>();
            List<String> strStoreList = new List<string>();
            List<String> strNOList = new List<string>();
            List<Dictionary<string, string>> snList = new List<Dictionary<string, string>>();

            try
            {
                Workbook workbook = new Workbook(planFilePath);
                Worksheet sheet = workbook.Worksheets[0];

                string woNoIndex = "A"; //A --工单
                string mainCardIndex = "B"; //主卡IMEI
                string minorCardIndex = "C"; //副卡IMEI
                string snIndex = "D"; //D --SN
                string btIndex = "E"; //E --BT
                string macIndex = "F"; //F --MAC
                string meidIndex = "G"; //G --MEID
                int startRow = 2;//开始的行
                decimal header_id = 0;
                int sn_qty = 0;

                //事务开启
                await _repository.StartTransaction();

                for (int i = startRow; i < sheet.Cells.Rows.Count + 1; i++)
                {
                    ImportRuncardSnAddOrModifyModel snExcelModel = new ImportRuncardSnAddOrModifyModel() { CREATE_BY = userName, UPDATE_BY = userName };

                    //工单号
                    tmpValue = sheet.Cells[woNoIndex + i].Value;
                    if (tmpValue.IsNullOrEmpty()) { continue; }
                    snExcelModel.WO_NO = tmpValue.ToString().Trim();
                    if (header_id <= 0)
                    {
                        header_id = await _repository.AddImportRuncardHeader(snExcelModel);
                        if (header_id < 1) { throw new Exception("SAVE_IMPORTRUNCARDSN_HEADER_ERR"); }
                    }

                    //主卡IMEI
                    tmpValue = sheet.Cells[mainCardIndex + i].Value;
                    if (tmpValue != null) snExcelModel.MAIN_CARD_IMEI = tmpValue.ToString().Trim();

                    //副卡IMEI
                    tmpValue = sheet.Cells[minorCardIndex + i].Value;
                    if (tmpValue != null) snExcelModel.MINOR_CARD_IMEI = tmpValue.ToString().Trim();

                    //SN字段如果填了SN按填入的SN导入，如果没有填入SN就根据工单生成SN导入
                    tmpValue = sheet.Cells[snIndex + i].Value;
                    if (tmpValue.IsNullOrEmpty())
                    {
                        //SN为空，按工单号获取SN导入
                        SfcsWo wo = await _repository.GetAsyncEx<SfcsWo>("WHERE WO_NO =:WO_NO", new { WO_NO = snExcelModel.WO_NO });
                        if (wo.IsNullOrWhiteSpace()) { continue; }
                        snExcelModel.SN = await GetSNByWoId(wo.ID);
                        if (snExcelModel.SN.IsNullOrEmpty()) { continue; }
                        var snDictionary = new Dictionary<string, string>();
                        snDictionary.Add(i.ToString(), snExcelModel.SN);
                        snList.Add(snDictionary);
                    }
                    else if (snList.Any(c => c.Equals(tmpValue.ToString().Trim())))
                    {
                        //如果重复的话就跳过
                        continue;
                    }
                    else
                    {
                        //SN不为空，按SN导入
                        snExcelModel.SN = tmpValue.ToString().Trim();
                        var snDictionary = new Dictionary<string, string>();
                        snDictionary.Add(i.ToString(), snExcelModel.SN);
                        snList.Add(snDictionary);
                    }

                    //BT
                    tmpValue = sheet.Cells[btIndex + i].Value;
                    if (tmpValue != null) snExcelModel.BT = tmpValue.ToString().Trim();

                    //MAC
                    tmpValue = sheet.Cells[macIndex + i].Value;
                    if (tmpValue != null) snExcelModel.MAC = tmpValue.ToString().Trim();

                    //MEID
                    tmpValue = sheet.Cells[meidIndex + i].Value;
                    if (tmpValue != null) snExcelModel.MEID = tmpValue.ToString().Trim();

                    var resultCode = await _repository.UpdateCustomerSNData(snExcelModel, header_id);
                    if (resultCode == GlobalVariables.FailNUM)
                    {
                        //第{0}行数据异常，请注意检查! The data in row {0} is abnormal, please check!   
                        strMsgList.Add(string.Format(_localizer["Err_Data_Row_Abnormal"], i.ToString()));
                    }
                    else
                    {
                        sn_qty++;
                    }
                }

                await _repository.upSNHeaderByQty(header_id, sn_qty);

                //事务提交
                await _repository.CommitTransaction();
                if (strMsgList != null && strMsgList.Count() > 0)
                {
                    result.Set(String.Join(';', strMsgList));
                    result.Code = 1;
                }

            }
            catch (Exception ex)
            {
                await _repository.RollbackTransaction();
                result.Set(ex.Message);
            }
            finally
            {
                await _repository.CloseTransaction();
            }

            return result;
        }

        /// <summary>
        /// 根据工单在流水号范围里获取一个SN
        /// </summary>
        /// <param name="wo_id"></param>
        /// <returns></returns>
        private async Task<String> GetSNByWoId(decimal wo_id)
        {
            String sn = "";
            List<String> snList = new List<string>();
            List<SfcsRuncardRanger> runcardRangerList = await _repository.GetListByTableEX<SfcsRuncardRanger>("RA.*", "SFCS_RUNCARD_RANGER RA,SFCS_RUNCARD_RANGER_RULES RU", " AND RA.RANGER_RULE_ID = RU.ID AND RU.RULE_TYPE = :RULE_TYPE AND RA.WO_ID = :WO_ID ORDER BY RA.ID DESC ", new { WO_ID = wo_id, RULE_TYPE = GlobalVariables.RangerSN });
            if (runcardRangerList.Count > 0)
            {
                foreach (var item in runcardRangerList)
                {
                    if (item.PRINTED != "Y")
                    {
                        List<String> runcardSNList = await _repository.GetListByTableEX<String>("SN", "SFCS_RUNCARD", " AND SN LIKE :SN ORDER BY SN ASC ", new { SN = item.FIX_HEADER + "%" + item.FIX_TAIL });
                        List<String> logSNList = await _repository.GetListByTableEX<String>("SN", "IMPORT_RUNCARD_SN", " AND SN LIKE :SN ORDER BY SN ASC ", new { SN = item.FIX_HEADER + "%" + item.FIX_TAIL });
                        if (runcardSNList.Count() < item.QUANTITY)
                        {
                            List<String> rangerSNList = (await GenerateRangerSN(item));
                            if (rangerSNList.Count > 0)
                            {
                                snList = rangerSNList.Except(runcardSNList).ToList();
                                foreach (var snitem in snList)
                                {
                                    if (logSNList.Where(m => m == snitem).FirstOrDefault().IsNullOrEmpty()) { sn = snitem; }
                                    if (!sn.IsNullOrEmpty()) { break; }
                                }
                                if (!sn.IsNullOrEmpty()) { break; }
                            }
                        }
                    }
                }
            }
            return sn;
        }

        /// <summary>
        /// 计算出流水号范围中的流水号信息
        /// </summary>
        /// <param name="sfcsRuncardRanger"></param>
        /// <returns></returns>
        private async Task<List<string>> GenerateRangerSN(SfcsRuncardRanger sfcsRuncardRanger)
        {
            List<string> SerialNumberList = new List<string>();
            var sfcsParameterslist = await _repository.QueryAsyncEx<SfcsParameters>("SELECT SP.* FROM SFCS_PARAMETERS SP WHERE LOOKUP_TYPE = :LOOKUP_TYPE AND LOOKUP_CODE = :LOOKUP_CODE",
                 new
                 {
                     LOOKUP_TYPE = "RADIX_TYPE",
                     LOOKUP_CODE = sfcsRuncardRanger.DIGITAL
                 });
            if (sfcsParameterslist == null || sfcsParameterslist.Count() <= 0)
            {
                return null;
            }
            string standardDigits = sfcsParameterslist.FirstOrDefault().DESCRIPTION;
            SerialNumberList.Add(sfcsRuncardRanger.SN_BEGIN);
            string snBeginRange = sfcsRuncardRanger.SN_BEGIN.Substring(
                (int)(sfcsRuncardRanger.HEADER_LENGTH), (int)sfcsRuncardRanger.RANGE);
            for (int i = 1; i < sfcsRuncardRanger.QUANTITY; i++)
            {
                // calculate sn from 2nd to the last
                string snRange = RadixConvertPublic.RadixInc(snBeginRange, standardDigits, i).PadLeft(snBeginRange.Length, '0').Trim();
                string sn = (sfcsRuncardRanger.FIX_HEADER.IsNullOrEmpty() ? "" : sfcsRuncardRanger.FIX_HEADER.ToString().Trim()) +
                    snRange + (sfcsRuncardRanger.FIX_TAIL.IsNullOrEmpty() ? "" : sfcsRuncardRanger.FIX_TAIL.ToString().Trim());

                // add new sn into list
                SerialNumberList.Add(sn);
            }
            return SerialNumberList;
        }

        /// <summary>
        /// 獲取工單已設定的流水號數量
        /// </summary>
        /// <returns></returns>
        private async Task<decimal> GetRuncardRangerQTY(decimal workOrderID)
        {
            decimal result = 0;
            List<SfcsRuncardRanger> runcardRanger = (await _repository.GetListAsync("WHERE WO_ID =:WO_ID", new { WO_ID = workOrderID }))?.ToList();
            if (runcardRanger != null)
            {
                result = runcardRanger.Sum(t => t.QUANTITY);
            }
            return result;
        }

        /// <summary>
        /// 獲取當前ID已設定的流水號數量
        /// </summary>
        /// <param name="rangerID"></param>
        /// <param name="workOrderID"></param>
        /// <returns></returns>
        private async Task<decimal> GetRuncardRangerQTY(decimal rangerID, decimal workOrderID)
        {
            string conditions = "WHERE ID =:ID AND WO_ID =:WO_ID";
            SfcsRuncardRanger runcardRanger = await _repository.GetAsyncEx<SfcsRuncardRanger>(conditions, new { ID = rangerID, WO_ID = workOrderID });
            return runcardRanger?.QUANTITY ?? 0;
        }

        /// <summary>
        /// 獲取工單目標產量
        /// </summary>
        /// <returns></returns>
        private async Task<decimal> GetWorkOrderTargetQTY(decimal workOrderID)
        {
            string conditions = "WHERE ID =:ID";
            SfcsWo woRow = await _repository.GetAsyncEx<SfcsWo>(conditions, new { ID = workOrderID });
            return woRow?.TARGET_QTY ?? 0;
        }

        /// <summary>
		/// 取得SN Format
		/// </summary>
		/// <param name="partNumber">成品料号</param>
        /// <param name="checkSNBeginFormat">是否校验开始流水号</param>
        /// <param name="snBegin">开始流水号</param>
		private async Task<string> GetSNFormat(string partNumber, bool checkSNBeginFormat = true, string snBegin = "")
        {
            string result = string.Empty;
            string errmsg = string.Empty;
            string conditions = "WHERE PART_NO =:PART_NO AND CONFIG_TYPE =:CONFIG_TYPE";
            SfcsProductConfig configRow = await _repository.GetAsyncEx<SfcsProductConfig>(conditions, new
            {
                PART_NO = partNumber,
                CONFIG_TYPE = GlobalVariables.SNFormat
            });
            if (configRow == null)
            {
                configRow = await _repository.GetAsyncEx<SfcsProductConfig>(conditions, new
                {
                    PART_NO = "000000",
                    CONFIG_TYPE = GlobalVariables.SNFormat
                });
            }
            if (configRow == null || configRow.CONFIG_VALUE.IsNullOrWhiteSpace())
            {
                //料号{0}没有设定流水号格式。
                errmsg = string.Format(_localizer["Err_NoFomat"], partNumber);
                throw new Exception(errmsg);
            }

            if (checkSNBeginFormat)
            {
                if (!FormatChecker.FormatCheck(snBegin, configRow.CONFIG_VALUE.Trim()))
                {
                    //流水号{0}格式{1}不匹配。
                    errmsg = string.Format(_localizer["Err_SNFormat"], snBegin, configRow.CONFIG_VALUE);
                    throw new Exception(errmsg);
                }
            }
            result = configRow.CONFIG_VALUE.Trim();
            return result;
        }

        /// <summary>
        /// 获取 排除字符表 RADIX_EXCLUSIVE 
        /// </summary>
        /// <param name="digital">进制代码</param>
        /// <returns></returns>
        private async Task<SfcsParameters> GetRADIX_EXCLUSIVE(string digital)
        {
            var DigitalList = await _repository.GetListByTableEX<IDNAME>("LOOKUP_CODE as ID,MEANING as NAME", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'RADIX_TYPE' AND ENABLED='Y' ORDER BY MEANING");
            string meaning = DigitalList.Where(t => t.ID == digital).Select(t => t.NAME).FirstOrDefault() ?? string.Empty;
            string conditions = "WHERE LOOKUP_TYPE = 'RADIX_EXCLUSIVE' AND MEANING=:MEANING";
            return (await _repository.GetAsyncEx<SfcsParameters>(conditions, new { MEANING = meaning }));
        }

        /// <summary>
        /// 获取Sn打印文件的id
        /// </summary>
        /// <returns></returns>
        private async Task<SfcsPrintTasks> GetPrintFiles(decimal wo_id, List<string> snList)
        {
            SfcsPrintTasks printTasks = new SfcsPrintTasks();

            String printMappSql = @"select SPF.* from SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES  SPF 
                        where SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 1";
            var sfcsPnlist = await _repository.QueryAsyncEx<SfcsPn>("select SP.* from SFCS_PN SP, SFCS_WO SW where SP.PART_NO = SW.PART_NO AND SW.ID = :ID", new { ID = wo_id });
            SfcsPn sfcsPn = sfcsPnlist.FirstOrDefault();
            String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
            SfcsPrintFiles sfcsPrintFiles = null;
            List<SfcsPrintFiles> sfcsPrintMapplist = null;
            sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByPn, new { PART_NO = sfcsPn.PART_NO });
            if (sfcsPrintMapplist == null)
            {
                String printMappSqlByModel = printMappSql + " AND SPFM.MODEL_ID = :MODEL_ID";
                sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByModel,
                new
                {
                    MODEL_ID = sfcsPn.MODEL_ID
                });
            }
            if (sfcsPrintMapplist == null)
            {
                String printMappSqlByFamilly = printMappSql + " AND SPFM.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID";
                sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByFamilly,
                new
                {
                    PRODUCT_FAMILY_ID = sfcsPn.FAMILY_ID
                });
            }
            if (sfcsPrintMapplist == null)
            {
                String printMappSqlByCustor = printMappSql + " AND SPFM.CUSTOMER_ID = :CUSTOMER_ID";

                sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByCustor,
                new
                {
                    CUSTOMER_ID = sfcsPn.CUSTOMER_ID
                });
            }
            //默认产品条码模板
            if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
            {
                sfcsPrintMapplist = await _repository.QueryAsyncEx<SfcsPrintFiles>(printMappSqlByPn,
                new
                {
                    PART_NO = "000000"
                });
            }
            if (sfcsPrintMapplist != null && sfcsPrintMapplist.Count > 0)
            {
                sfcsPrintFiles = sfcsPrintMapplist.FirstOrDefault();
            }
            else
            {
                throw new Exception(_localizer["Err_SetProductPrintFile"]);
            }
            ImsPart imsPart = _repository.QueryEx<ImsPart>("SELECT * FROM IMS_PART WHERE CODE = :CODE",
                new { CODE = sfcsPn.PART_NO }).FirstOrDefault();

            //获取产品打印附加值 SELECT LOOKUP_CODE FROM SFCS_PARAMETERS SP WHERE SP.LOOKUP_TYPE = 'PRODUCT_CONFIG_TYPE' AND SP.ENABLED = 'Y' AND SP.CHINESE = '产品打印附加值'
            //SELECT CONFIG_VALUE FROM SFCS_PRODUCT_CONFIG WHERE PART_NO = '' AND CONFIG_TYPE ='200' AND ENABLED='Y'
            String detail = "", detailValue = "", header = "";
            detail = _repository.QueryEx<String>("SELECT CONFIG_VALUE FROM SFCS_PRODUCT_CONFIG WHERE PART_NO =:PART_NO AND CONFIG_TYPE =:CONFIG_TYPE AND ENABLED='Y'",
                        new { PART_NO = sfcsPn.PART_NO, CONFIG_TYPE = GlobalVariables.SNPrintData }).FirstOrDefault();
            if (!detail.IsNullOrEmpty())
            {
                var detailArr = detail.Split("|");
                for (int i = 0; i < detailArr.Length; i++)
                {
                    header += "," + GlobalVariables.PrintHeader + (i + 1);
                    detailValue += "," + detailArr[i];
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            DateTime dateTime = DateTime.Now;
            stringBuilder.AppendLine(String.Format("PN,PN_NAME,MODEL,SN,CREATE_TIME,QR_NO{0}", header));
            foreach (String sn in snList)
            {
                String qrNo = String.Format("{0}|{1}|{2}|{3}|{4}", imsPart.CODE, imsPart.NAME, imsPart.DESCRIPTION, sn, detail);
                stringBuilder.AppendLine(String.Format("{0},{1},{2},{3},{4},{5}{6}", imsPart.CODE, imsPart.NAME, imsPart.DESCRIPTION, sn, dateTime.ToString(), qrNo, detailValue));
            }

            printTasks.PART_NO = sfcsPn.PART_NO;
            printTasks.PRINT_FILE_ID = sfcsPrintFiles.ID;
            printTasks.PRINT_DATA = stringBuilder.ToString();
            printTasks.ID = await _sfcsPrintTasksRepository.GetSEQID();
            return printTasks;
        }

        #region 自动创建范围

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runcardRangerInfo"></param>
        /// <param name="wo_no">工单号</param>
        /// <param name="partNumber">料号</param>
        /// <param name="familyId">产品系列id</param>
        /// <param name="familyName">产品系列名称</param>
        /// <param name="platformID"></param>
        /// <param name="platform"></param>
        /// <param name="quantity"></param>
        /// <param name="snformat"></param>
        /// <param name="customer"></param>
        /// <param name="rule_type">规则类型 0: 流水号规则 1: 箱号规则 2: 栈板号规则</param>
        /// <param name="salesOrderNumber"></param>
        /// <returns></returns>
        private async Task<SfcsRuncardRanger> AutoCreateRuncardRanger(SfcsRuncardRanger runcardRangerInfo, string wo_no, string partNumber, decimal familyId, string familyName, decimal platformID, string platform, decimal quantity, string snformat, SfcsCustomers customer, int rule_type, string salesOrderNumber = "")
        {
            var result = runcardRangerInfo;
            string conditions = string.Empty;
            try
            {
                //先工单再到销售订单再到成品料号再到产品系列最后客户
                string WoNoFound = string.Empty;
                string salesOrderNumFound = string.Empty;
                string partNumberFound = string.Empty;
                string familyIDFound = string.Empty;
                string platformIDFound = string.Empty;
                string customerIDFound = string.Empty;
                string totalRuleFound = string.Empty;
                int foundCount = 0;

                SfcsRuncardRangerRules runcardRangerRule = null;
                conditions = "WHERE WO_NO =:WO_NO AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                SfcsRuncardRangerRules runcardRangerRuleByWoNo = await _repository.GetAsyncEx<SfcsRuncardRangerRules>(conditions, new { WO_NO = wo_no, RULE_TYPE = rule_type });
                if (runcardRangerRuleByWoNo != null)
                {
                    runcardRangerRule = runcardRangerRuleByWoNo;
                }

                if (runcardRangerRule == null && !salesOrderNumber.IsNullOrEmpty())
                {
                    conditions = "WHERE SALES_ORDER =:SALES_ORDER AND Enabled ='Y'";
                    SfcsRuncardRangerRules runcardRangerRuleBySalesOrder = await _repository.GetAsyncEx<SfcsRuncardRangerRules>(conditions, new { SALES_ORDER = salesOrderNumber });
                    if (runcardRangerRuleBySalesOrder != null)
                    {
                        salesOrderNumFound = "SalesOrderNumber#" + salesOrderNumber + " ";
                        totalRuleFound = string.Concat(totalRuleFound, partNumberFound);
                        foundCount = ++foundCount;
                        runcardRangerRule = runcardRangerRuleBySalesOrder;
                    }
                }

                if (runcardRangerRule == null && !partNumber.IsNullOrEmpty())
                {
                    conditions = "WHERE PART_NO =:PART_NO AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    SfcsRuncardRangerRules runcardRangerRuleByPN = await _repository.GetAsyncEx<SfcsRuncardRangerRules>(conditions, new { PART_NO = partNumber, RULE_TYPE = rule_type });
                    if (runcardRangerRuleByPN != null)
                    {
                        partNumberFound = "PartNumber#" + partNumber + " ";
                        totalRuleFound = string.Concat(totalRuleFound, partNumberFound);
                        foundCount = ++foundCount;
                        runcardRangerRule = runcardRangerRuleByPN;
                    }
                }

                if (runcardRangerRule == null && familyId > 0)
                {
                    conditions = "WHERE PLATFORM_ID =:PRODUCT_FAMILY_ID AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    SfcsRuncardRangerRules runcardRangerRuleByFamily = await _repository.GetAsyncEx<SfcsRuncardRangerRules>(conditions, new { PRODUCT_FAMILY_ID = familyId, RULE_TYPE = rule_type });
                    if (runcardRangerRuleByFamily != null)
                    {
                        familyIDFound = "ProductFamily#" + familyName + " ";
                        totalRuleFound = string.Concat(totalRuleFound, familyIDFound);
                        foundCount = ++foundCount;
                        runcardRangerRule = runcardRangerRuleByFamily;
                    }
                }

                if (runcardRangerRule == null && platformID > 0)
                {
                    conditions = "WHERE PLATFORM_ID =:PLATFORM_ID AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    SfcsRuncardRangerRules runcardRangerRuleByPlatform = await _repository.GetAsyncEx<SfcsRuncardRangerRules>(conditions, new { PLATFORM_ID = platformID, RULE_TYPE = rule_type });
                    if (runcardRangerRuleByPlatform != null)
                    {
                        platformIDFound = "PlatformName#" + platform + " ";
                        totalRuleFound = string.Concat(totalRuleFound, platformIDFound);
                        foundCount = ++foundCount;
                        runcardRangerRule = runcardRangerRuleByPlatform;
                    }
                }

                if (runcardRangerRule == null && customer != null)
                {
                    conditions = "WHERE CUSTOMER_ID =:CUSTOMER_ID AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    SfcsRuncardRangerRules runcardRangerRuleByCustomer = await _repository.GetAsyncEx<SfcsRuncardRangerRules>(conditions,
                        new { CUSTOMER_ID = customer.ID, RULE_TYPE = rule_type });
                    if (runcardRangerRuleByCustomer == null)
                    {
                        conditions = "WHERE CUSTOMER_ID =:CUSTOMER_ID AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                        runcardRangerRuleByCustomer = await _repository.GetAsyncEx<SfcsRuncardRangerRules>(conditions, new { CUSTOMER_ID = 127537, RULE_TYPE = rule_type });

                    }
                    if (runcardRangerRuleByCustomer != null)
                    {
                        customerIDFound = "CustomerName#" + customer.CUSTOMER + " ";
                        totalRuleFound = string.Concat(totalRuleFound, customerIDFound);
                        foundCount = ++foundCount;
                        runcardRangerRule = runcardRangerRuleByCustomer;
                    }
                }

                if (runcardRangerRule == null)
                {
                    //寻找默认料号000000的配置
                    conditions = "WHERE PART_NO =:PART_NO AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    SfcsRuncardRangerRules runcardRangerRuleByPN = await _repository.GetAsyncEx<SfcsRuncardRangerRules>(conditions, new { PART_NO = "000000", RULE_TYPE = rule_type });
                    if (runcardRangerRuleByPN != null)
                    {
                        partNumberFound = "PartNumber#" + partNumber + " ";
                        totalRuleFound = string.Concat(totalRuleFound, partNumberFound);
                        foundCount = ++foundCount;
                        runcardRangerRule = runcardRangerRuleByPN;
                    }
                }

                //// 范围选定遵循从细到广的原则
                //if (runcardRangerRuleByWoNo != null)
                //{
                //    runcardRangerRule = runcardRangerRuleByWoNo;
                //}
                //else if (runcardRangerRuleBySalesOrder != null)
                //{
                //    runcardRangerRule = runcardRangerRuleBySalesOrder;
                //}
                //else if (runcardRangerRuleByPN != null)
                //{
                //    runcardRangerRule = runcardRangerRuleByWoNo;
                //}
                //else if (runcardRangerRuleByPN != null)
                //{
                //    runcardRangerRule = runcardRangerRuleByPN;
                //}
                //else if (runcardRangerRuleByFamily != null)
                //{
                //    runcardRangerRule = runcardRangerRuleByFamily;
                //}
                //else if (runcardRangerRuleByPlatform != null)
                //{
                //    runcardRangerRule = runcardRangerRuleByPlatform;
                //}
                //else if (runcardRangerRuleByCustomer != null)
                //{
                //    runcardRangerRule = runcardRangerRuleByCustomer;
                //}

                if (runcardRangerRule == null)
                {
                    // 没有找到自动生成规则，需手动设定范围
                    return null;
                }
                else
                {
                    // 辨析規則中的變化編碼
                    string fixHeader = await IdentitySpecialCodes(wo_no, partNumber, runcardRangerRule.FIX_HEADER == null ? "" : runcardRangerRule.FIX_HEADER, runcardRangerRule == null ? 0 : runcardRangerRule.ID);
                    string fixTail = string.Empty;
                    if (runcardRangerRule.FIX_TAIL != null)
                    {
                        fixTail = await IdentitySpecialCodes(wo_no, partNumber, runcardRangerRule.FIX_TAIL, runcardRangerRule == null ? 0 : runcardRangerRule.ID);
                    }
                    if (fixHeader == null && fixTail == null)
                    {
                        //"固定头和固定尾不能同时为空!"
                        throw new Exception(_localizer["Err_FIX_HEADER_TAIL"]);
                    }

                    conditions = "WHERE LOOKUP_TYPE =:LOOKUP_TYPE AND LOOKUP_CODE =:LOOKUP_CODE ";
                    SfcsParameters row = await _repository.GetAsyncEx<SfcsParameters>(conditions,
                        new { LOOKUP_TYPE = "RADIX_TYPE", LOOKUP_CODE = runcardRangerRule.DIGITAL });
                    if (row == null)
                    {
                        //"进制类型设定有误，请联系管理员处理。"
                        throw new Exception(_localizer["Err_RADIX_TYPE"]);
                    }
                    string radixString = row.DESCRIPTION;

                    string snBegin = string.Empty;
                    //conditions = @"WHERE RANGER_RULE_ID = :RANGER_RULE_ID AND DIGITAL = :DIGITAL AND RANGE = :RANGE ";
                    conditions = @"WHERE  DIGITAL = :DIGITAL AND RANGE = :RANGE ";
                    if (!fixHeader.IsNullOrWhiteSpace())
                    {
                        conditions += " AND FIX_HEADER = :FIX_HEADER ";
                    }
                    if (!fixTail.IsNullOrWhiteSpace())
                    {
                        conditions += " AND FIX_TAIL = :FIX_TAIL ";
                    }
                    var isAsc = true;
                    isAsc = runcardRangerRule != null && runcardRangerRule.SORT_TYPE == 2 ? false : true;
                    if (isAsc)
                        conditions += " ORDER BY SN_END DESC ";

                    SfcsRuncardRanger runcardRanger = new SfcsRuncardRanger();
                    if (isAsc)
                    {
                        runcardRanger = await _repository.GetAsyncEx<SfcsRuncardRanger>(conditions, new
                        {
                            //RANGER_RULE_ID = runcardRangerRule.ID,
                            DIGITAL = runcardRangerRule.DIGITAL,
                            RANGE = runcardRangerRule.RANGE_LENGTH,
                            FIX_HEADER = fixHeader,
                            FIX_TAIL = fixTail
                        });
                    }
                    else
                    {
                        runcardRanger = (await _repository.GetListByTableEX<SfcsRuncardRanger>("*", "SFCS_RUNCARD_RANGER",
                           " AND SN_BEGIN IN ( SELECT MIN(SN_BEGIN) SN_BEGIN FROM SFCS_RUNCARD_RANGER " + conditions + ")", new
                           {
                               DIGITAL = runcardRangerRule.DIGITAL,
                               RANGE = runcardRangerRule.RANGE_LENGTH,
                               FIX_HEADER = fixHeader,
                               FIX_TAIL = fixTail
                           }))?.FirstOrDefault();
                    }

                    if (runcardRanger == null)
                    {
                        // 第一段範圍，依照指定的開始碼產生SN Begin
                        snBegin = (fixHeader + runcardRangerRule.RANGE_START_CODE + fixTail).Trim();
                        //mark by shanki.f not need to check
                        /*
                        if (this.customerID == GlobalVariables.InspurCustomer)
                        {
                            // 到舊系統獲取最後一個設定的SN
                            string lastSNInOldSFCS = DataSynchronManager.GetOldSFCSInspurLastSN();
                            snBegin = RadixConvertPublic.RadixInc(lastSNInOldSFCS, radixString, 1);
                        }
                        */
                    }
                    else
                    {
                        int rangeLength = (int)runcardRangerRule.RANGE_LENGTH;
                        string lastSNCode = isAsc ? runcardRanger.SN_END.Substring(fixHeader.Length, rangeLength) : runcardRanger.SN_BEGIN.Substring(fixHeader.Length, rangeLength);
                        RadixConvertPublic._localizer = _localizer;
                        string snBeginCode = RadixConvertPublic.RadixInc(lastSNCode, radixString, 1, isAsc).PadLeft(rangeLength, '0');
                        snBegin = fixHeader + snBeginCode + fixTail;
                    }

                    result.RANGER_RULE_ID = runcardRangerRule.ID;
                    result.DIGITAL = runcardRangerRule.DIGITAL ?? 0;
                    result.RANGE = runcardRangerRule.RANGE_LENGTH;
                    result.TAIL_LENGTH = fixTail.Length;
                    result.QUANTITY = quantity;
                    result.SN_BEGIN = snBegin;
                    result.STATUS = 1;

                    await CalculateRanger(result, snformat, runcardRangerRule);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        /// <summary>
        /// 辨析特殊代码
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        private async Task<string> IdentitySpecialCodes(string wo_no, string part_no, string sourceString, Decimal? rangerRuleId = 0)
        {
            string verifiedString = sourceString;

            // 獲取特殊字符並解析
            Regex regex = new Regex(@"(?<=\<)[^\<^\>]+(?=\>)");
            MatchCollection matchCollection = regex.Matches(sourceString);
            for (int i = 0; i < matchCollection.Count; i++)
            {
                //处理成品料号的规则
                if (matchCollection[i].Value.Contains("PN"))
                {
                    var specialCodesSplitList = matchCollection[i].Value.Replace("PN", "").Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    if (specialCodesSplitList.Length == 2)
                    {
                        //查规则的成品料号
                        //var ruleModel= (await _repository.GetListByTableEX<SfcsRuncardRangerRules>("*", "SFCS_RUNCARD_RANGER_RULES"," AND ID=:ID",new { ID= rangerRuleId }))?.FirstOrDefault();
                        if (!part_no.IsNullOrWhiteSpace())
                        {
                            //规则处理:<PN(0,2)> 截取0位的2个长度
                            int startIndex = Convert.ToInt32(specialCodesSplitList[0]) - 1;
                            int length = Convert.ToInt32(specialCodesSplitList[1]);
                            verifiedString = verifiedString.Replace("<" + matchCollection[i].Value.ToString() + ">", "") + "" + part_no.Substring(startIndex, length);
                        }
                        else
                        {
                            verifiedString = verifiedString.Replace("<" + matchCollection[i].Value.ToString() + ">", "");
                        }
                    }
                    break;
                }

                //处理工单的规则
                if (matchCollection[i].Value.Contains("WO"))
                {
                    var specialCodesSplitList = matchCollection[i].Value.Replace("WO", "").Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    if (specialCodesSplitList.Length == 2)
                    {
                        //查规则的成品料号
                        //var ruleModel = (await _repository.GetListByTableEX<SfcsRuncardRangerRules>("*", "SFCS_RUNCARD_RANGER_RULES", " AND ID=:ID", new { ID = rangerRuleId }))?.FirstOrDefault();
                        if (!wo_no.IsNullOrWhiteSpace())
                        {
                            //规则处理:<WO(0,2)> 截取0位的2个长度
                            int startIndex = Convert.ToInt32(specialCodesSplitList[0]) - 1;
                            int length = Convert.ToInt32(specialCodesSplitList[1]);
                            verifiedString = verifiedString.Replace("<" + matchCollection[i].Value.ToString() + ">", "") + "" + wo_no.Substring(startIndex, length);
                        }
                        else
                        {
                            verifiedString = verifiedString.Replace("<" + matchCollection[i].Value.ToString() + ">", "");
                        }
                    }
                    break;
                }

                switch (matchCollection[i].Value)
                {
                    case "YYWW":
                        string currentYYWW = await _repository.GetYYWW();
                        verifiedString = verifiedString.Replace("<YYWW>", currentYYWW);
                        break;
                    case "YYIW":
                        string currentYYIW = await _repository.GetYYIW();
                        verifiedString = verifiedString.Replace("<YYIW>", currentYYIW);
                        break;
                    case "IYIW":
                        // Oracle Work Week Format
                        string currentIYIW = await _repository.GetIYIW();
                        verifiedString = verifiedString.Replace("<IYIW>", currentIYIW);
                        break;
                    case "YIW":
                        // Intel Work Week Format
                        /* Ues IYIW and adjust +1 or not manually
                        string currentYIW = await _repository.GetYIW();
                        decimal decimalVal = System.Convert.ToDecimal(currentYIW) + 1;
                        currentYIW = System.Convert.ToString(decimalVal);*/
                        string currentYIW = await _repository.Get_Intel_YIW();
                        verifiedString = verifiedString.Replace("<YIW>", currentYIW);
                        break;
                    case "IW":
                        // Oracle Work Week only Format
                        string currentIW = await _repository.GetIW();
                        verifiedString = verifiedString.Replace("<IW>", currentIW);
                        break;
                    case "Y":
                        string currentY = await _repository.GetY();
                        verifiedString = verifiedString.Replace("<Y>", currentY);
                        break;
                    case "YY":
                        string currentYY = await _repository.GetYY();
                        verifiedString = verifiedString.Replace("<YY>", currentYY);
                        break;
                    case "YYYY":
                        string currentYYYY = await _repository.GetYYYY();
                        verifiedString = verifiedString.Replace("<YYYY>", currentYYYY);
                        break;
                    case "MM":
                        string currentMM = await _repository.GetMM();
                        verifiedString = verifiedString.Replace("<MM>", currentMM);
                        break;
                    case "DD":
                        string currentDD = await _repository.GetDD();
                        verifiedString = verifiedString.Replace("<DD>", currentDD);
                        break;
                    case "YYYYMMDD":
                        string currentYYYYMMDD = await _repository.GetYYYYMMDD();
                        verifiedString = verifiedString.Replace("<YYYYMMDD>", currentYYYYMMDD);
                        break;
                    case "YYMMDD":
                        string currentYYMMDD = await _repository.GetYYMMDD();
                        verifiedString = verifiedString.Replace("<YYMMDD>", currentYYMMDD);
                        break;
                    case "YYYYMM":
                        string currentYYYYMM = await _repository.GetYYYYMM();
                        verifiedString = verifiedString.Replace("<YYYYMM>", currentYYYYMM);
                        break;
                    case "YYMM":
                        string currentYYMM = await _repository.GetYYMM();
                        verifiedString = verifiedString.Replace("<YYMM>", currentYYMM);
                        break;
                    case "MMDD":
                        string currentMMDD = await _repository.GetMMDD();
                        verifiedString = verifiedString.Replace("<MMDD>", currentMMDD);
                        break;
                    case "WW":
                        string currentWW = await _repository.GetWW();
                        verifiedString = verifiedString.Replace("<WW>", currentWW);
                        break;
                    case "WWD":
                        String currentWWD = await _repository.GetWWD();
                        Decimal current = Decimal.Parse(currentWWD) - 1;
                        if (current % 10 == 0)
                        {
                            current = current + 7;
                        }
                        verifiedString = verifiedString.Replace("<WWD>", current.ToString());
                        break;
                    default:
                        break;
                }
            }
            return verifiedString;
        }

        /// <summary>
        /// 判斷輸入是否完整并變換焦點
        /// </summary>
        /// <param name="runcardRanger"></param>
        /// <param name="snformat"></param>
        /// <returns></returns>
		private bool SetFocus(SfcsRuncardRanger runcardRanger, string snformat)
        {
            if (runcardRanger.DIGITAL == -1)
            {
                return false;
            }

            if (runcardRanger.QUANTITY == 0)
            {
                return false;
            }

            if (runcardRanger.SN_BEGIN.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (runcardRanger.QUANTITY != 1 && runcardRanger.RANGE == 0)
            {
                return false;
            }

            if (snformat.IsNullOrEmpty())
            {
                //输入{0}后请按回车确认。
                //Messenger.Hint(Properties.Resources.Msg_End_With_Enter);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 計算流水號范圍
        /// </summary>
        /// <param name="runcardRanger"></param>
        /// <param name="snformat"></param>
		private async Task<bool> CalculateRanger(SfcsRuncardRanger runcardRanger, string snformat, SfcsRuncardRangerRules runcardRangerRule = null)
        {
            string errmsg = string.Empty;
            int snLength = runcardRanger.SN_BEGIN.Trim().Length;
            if (snLength == 0)
            {
                return false;
            }
            int rangeLength = (int)runcardRanger.RANGE;
            int tailLength = (int)runcardRanger.TAIL_LENGTH;
            int headerLength;
            if (SetFocus(runcardRanger, snformat))
            {
                if (rangeLength > snLength)
                {
                    //所选{0}数量{1}超出最大允许范围，请确认。
                    errmsg = string.Format(_localizer["Msg_QTY_Overflow"], "变化位数", rangeLength);//runcardRanger.RANGE
                    throw new Exception(errmsg);
                }

                if (tailLength > snLength)
                {
                    errmsg = string.Format(_localizer["Msg_QTY_Overflow"], "固定尾位数", tailLength); //runcardRanger.TAIL_LENGTH
                    throw new Exception(errmsg);
                }

                if (rangeLength + tailLength > snLength)
                {
                    errmsg = string.Format(_localizer["Msg_QTY_Overflow"], "变化位数 + 固定尾位数", rangeLength + tailLength);
                    throw new Exception(errmsg);
                }

                headerLength = snLength - rangeLength - tailLength;
                runcardRanger.HEADER_LENGTH = headerLength;
                runcardRanger.FIX_TAIL = runcardRanger.SN_BEGIN.Substring(snLength - tailLength);
                runcardRanger.FIX_HEADER = runcardRanger.SN_BEGIN.Substring(0, headerLength);

                string snBeginRange = "";
                if (rangeLength > 0)
                {
                    if (rangeLength == snLength)
                    {
                        snBeginRange = runcardRanger.SN_BEGIN;
                    }
                    else
                    {
                        snBeginRange = runcardRanger.SN_BEGIN.Substring(headerLength, rangeLength);
                    }

                    if (!DataChecker.IsAlphanumeric(snBeginRange))
                    {
                        //流水号的变化字符中{0}含有非法字符，请确认。
                        errmsg = string.Format(_localizer["Err_Illegal_Range"], snBeginRange);
                        throw new Exception(errmsg);
                    }
                }
                string conditions = " WHERE LOOKUP_TYPE = 'RADIX_TYPE' and LOOKUP_CODE=:DIGITAL";
                SfcsParameters row = await _repository.GetAsyncEx<SfcsParameters>(conditions, new { runcardRanger.DIGITAL });
                string radixString = row?.DESCRIPTION ?? string.Empty;
                //默认是升序的
                var isAsc = true;
                isAsc = runcardRangerRule != null && runcardRangerRule.SORT_TYPE == 2 ? false : true;
                RadixConvertPublic._localizer = _localizer;
                string snEndRange = RadixConvertPublic.RadixInc(snBeginRange, radixString, (int)runcardRanger.QUANTITY - 1, isAsc).PadLeft(rangeLength, '0');

                runcardRanger.SN_END = runcardRanger.FIX_HEADER + snEndRange + runcardRanger.FIX_TAIL;

                if (!FormatChecker.FormatCheck(runcardRanger.SN_END, snformat))
                {
                    //流水号{0}格式{1}不匹配。
                    errmsg = string.Format(_localizer["Err_SNFormat"], runcardRanger.SN_END, snformat);
                    throw new Exception(errmsg);
                }

                //把开始流水号范围变成结束流水号范围
                if (!isAsc)
                {
                    string temp = runcardRanger.SN_BEGIN;
                    runcardRanger.SN_BEGIN = runcardRanger.SN_END;
                    runcardRanger.SN_END = temp;
                }

                // 確認流水號是否已存在
                await ConfirmRangerExisted(runcardRanger, runcardRanger.SN_BEGIN, runcardRanger.SN_END, headerLength, tailLength);
            }
            return true;
        }

        /// <summary>
        /// 確認流水號是否已存在
        /// </summary>
        /// <param name="runcardRanger"></param>
        /// <param name="snBegin"></param>
        /// <param name="snEnd"></param>
        /// <param name="headerLength"></param>
        /// <param name="tailLength"></param>
		private async Task ConfirmRangerExisted(SfcsRuncardRanger runcardRanger, string snBegin, string snEnd, int headerLength, int tailLength)
        {
            decimal rowID = runcardRanger.ID;
            string errmsg = string.Empty;
            string snBeginExist;
            string snEndExist;
            string conditions = @"WHERE :SN BETWEEN SN_BEGIN AND SN_END 
                                  AND LENGTH(:SN) = LENGTH(SN_END)  
                                  AND (FIX_HEADER = SUBSTR(:SN,1,HEADER_LENGTH) OR FIX_HEADER IS NULL) 
                                  AND (FIX_TAIL = SUBSTR(:SN, LENGTH(:SN)-TAIL_LENGTH+1, TAIL_LENGTH) OR FIX_TAIL IS NULL) ";

            List<SfcsRuncardRanger> runcardRangerTableSNBegin = (await _repository.GetListAsyncEx<SfcsRuncardRanger>(conditions, new { SN = snBegin })).ToList();

            List<SfcsRuncardRanger> runcardRangerTableSNEnd = (await _repository.GetListAsyncEx<SfcsRuncardRanger>(conditions, new { SN = snEnd })).ToList();

            conditions = @"WHERE LENGTH(:SN) = LENGTH(SN_END) 
                               AND (FIX_HEADER = SUBSTR(:SN, 1, HEADER_LENGTH) OR FIX_HEADER IS NULL)
                               AND (FIX_TAIL = SUBSTR(:SN, LENGTH(:SN) - TAIL_LENGTH + 1, TAIL_LENGTH) OR FIX_TAIL IS NULL) ";
            List<SfcsRuncardRanger> runcardRangerTableSNInclude = (await _repository.GetListAsyncEx<SfcsRuncardRanger>(conditions, new { SN = snBegin })).ToList();

            // 確保Begin SN不在已有的範圍中
            foreach (var runcardRangerRow in runcardRangerTableSNBegin)
            {
                if (rowID != runcardRangerRow.ID)
                {
                    snBeginExist = runcardRangerRow.SN_BEGIN;
                    snEndExist = runcardRangerRow.SN_END;
                    //流水号{0}已包含在范围{1}内。
                    errmsg = string.Format(_localizer["Err_Ranger_Existed"], snBegin, snBeginExist + "-" + snEndExist);
                    throw new Exception(errmsg);
                }
            }

            // 確保End SN不在已有的範圍中
            foreach (var runcardRangerRow in runcardRangerTableSNEnd)
            {
                if (rowID != runcardRangerRow.ID)
                {

                    snBeginExist = runcardRangerRow.SN_BEGIN;
                    snEndExist = runcardRangerRow.SN_END;
                    //流水号{0}已包含在范围{1}内。
                    errmsg = string.Format(_localizer["Err_Ranger_Existed"], snEnd, snBeginExist + "-" + snEndExist);
                    throw new Exception(errmsg);
                }
            }

            // 避免新設定的範圍包含已有的範圍
            foreach (var runcardRangerRow in runcardRangerTableSNInclude)
            {
                if (rowID != runcardRangerRow.ID)
                {
                    if (string.Compare(runcardRangerRow.SN_BEGIN, snBegin) >= 0 &&
                        string.Compare(runcardRangerRow.SN_END, snEnd) <= 0)
                    {
                        snBeginExist = runcardRangerRow.SN_BEGIN;
                        snEndExist = runcardRangerRow.SN_END;
                        //流水号{0}已包含在范围{1}内。
                        errmsg = string.Format(_localizer["Err_Ranger_Existed"], "", snBeginExist + "-" + snEndExist);
                        throw new Exception(errmsg);
                    }
                }
            }

            //檢查RuncardReplace是否存在
            await CheckRuncardReplaceExisted(snBegin, snEnd, headerLength, tailLength);
        }

        /// <summary>
        /// 檢查RuncardReplace重複SN
        /// </summary>
        /// <param name="snBegin"></param>
        /// <param name="snEnd"></param>
        /// <param name="headerLength"></param>
        /// <param name="tailLength"></param>
        private async Task CheckRuncardReplaceExisted(string snBegin, string snEnd, int headerLength, int tailLength)
        {
            string conditions = @"WHERE NEW_SN BETWEEN :SN_BEGIN AND :SN_END
                AND LENGTH (NEW_SN) = LENGTH (:SN_BEGIN)
                AND (SUBSTR (NEW_SN,1,:HEADER_LENGTH) = SUBSTR(:SN_BEGIN,1,:HEADER_LENGTH) OR :HEADER_LENGTH=0)
                AND (SUBSTR(NEW_SN,(LENGTH(NEW_SN)-:TAIL_LENGTH+1),:TAIL_LENGTH) = SUBSTR(:SN_BEGIN,(LENGTH(:SN_BEGIN)-:TAIL_LENGTH+1),:TAIL_LENGTH) OR :TAIL_LENGTH=0)";
            List<SfcsRuncardReplace> runcardReplaceTable = (await _repository.GetListAsyncEx<SfcsRuncardReplace>(conditions, new
            {
                SN_BEGIN = snBegin,
                SN_END = snEnd,
                HEADER_LENGTH = headerLength,
                TAIL_LENGTH = tailLength
            })).ToList();

            if (runcardReplaceTable.Count != 0)
            {
                string replaceSN = null;

                foreach (var row in runcardReplaceTable)
                {
                    replaceSN += row.NEW_SN + GlobalVariables.comma;
                }
                //流水号{0}已被用作替换流水号，不能作为新范围设定。
                string errmsg = string.Format(_localizer["Err_SNIsReplace"], replaceSN);
                throw new Exception(errmsg);
            }
        }

        #endregion

        /// <summary>
        /// 返回类
        /// </summary>
        public class ResultMsg
        {
            /// <summary>
            /// 
            /// </summary>
            public int Code { get; set; } = 1;

            /// <summary>
            /// 
            /// </summary>
            public string ErrorMessage { get; set; }

            public void Set(string msg)
            {
                Code = 0;
                ErrorMessage = msg;
            }
        }

        #endregion
    }
}