/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：打印任务表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-09-29 11:49:59                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsPrintTasksController                                      
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
using System.Text;

namespace JZ.IMS.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsPrintTasksController : BaseController
    {
        private readonly ISfcsCapReportRepository _repositoryc;
        private readonly ISfcsPrintTasksRepository _repository;
        private readonly IMesTongsInfoRepository _repositoryt;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsPrintTasksController> _localizer;

        public SfcsPrintTasksController(ISfcsPrintTasksRepository repository, ISfcsCapReportRepository repositoryc, IMesTongsInfoRepository repositoryt, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsPrintTasksController> localizer)
        {
            _repositoryc = repositoryc;
            _repository = repository;
            _repositoryt = repositoryt;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 首页视图
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

        /// <summary>
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsPrintTasksListModel>>> LoadData([FromQuery] SfcsPrintTasksRequestModel model)
        {
            ApiBaseReturn<List<SfcsPrintTasksListModel>> returnVM = new ApiBaseReturn<List<SfcsPrintTasksListModel>>();
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
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsPrintTasksListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsPrintTasksListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
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
        [Authorize]
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
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsPrintTasksModel model)
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
        [Authorize("Permission")]
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
        [Authorize("Permission")]
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
        /// 根据id打印任务数据
        /// </summary>
        /// <param name="printId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SfcsPrintTasks>> GetPrintDataById(decimal printId)
        {
            ApiBaseReturn<SfcsPrintTasks> returnVM = new ApiBaseReturn<SfcsPrintTasks>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (printId <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["PRINTID_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = (await _repository.GetListByTableEX<SfcsPrintTasks>("PT.*", "SFCS_PRINT_TASKS PT", " And PT.ID=:ID", new { ID = printId })).FirstOrDefault();
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
        /// 获取打印任务列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ID 打印任务表id, FILE_NAME 文件名, CHINESE 描述(CN), OPERATOR 操作人员, CREATE_TIME 创建时间, PRINT_STATUS 打印状态0：未打印；1：已打印；2：报废 PART_NO 料号, WO_NO 工单, CARTON_NO 箱号, PALLET_NO 栈板编号, PRINT_NO 打印次数</returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<TableDataModel>> GetPrintTasksData([FromBody] GetPrintTasksRequestModel model)
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetPrintTasksData(model);

                    #endregion
                }
                catch (Exception ex)
                {
                    if (returnVM.Result != null) { returnVM.Result = new TableDataModel(); returnVM.Result.code = -1; returnVM.Result.msg = ex.Message; }
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 重复打印
        /// </summary>
        /// <param name="printRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<PrintReturnModel>> RepeatPrintTasks([FromBody] RepeatPrintTasksRequestModel printRequestModel)
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
            SfcsPrintTasks printTasksModel = new SfcsPrintTasks();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (printRequestModel == null || printRequestModel.modelList == null || printRequestModel.modelList.Count <= 0)
                    {
                        throw new Exception("PRINT_TASKS_ERROR");
                    }

                    if (String.IsNullOrEmpty(printRequestModel.UserName))
                    {
                        throw new Exception("PRINT_USER_NULL");
                    }

                    #endregion

                    #region 保存并返回
                    foreach (var item in printRequestModel.modelList)
                    {
                        printTasksModel = (await _repository.GetListByTableEX<SfcsPrintTasks>("PT.*", "SFCS_PRINT_TASKS PT", " And PT.ID=:ID", new { ID = item.ID }))?.FirstOrDefault();
                        if (printTasksModel == null) { throw new Exception("GET_PRINT_TASKS_FAIL"); }
                        else if (printTasksModel.PRINT_STATUS.Trim() == "0") { throw new Exception("PRINT_STATUS0_FAIL"); }
                        printTasksModel.PRINTER = printRequestModel.UserName;
                        Boolean result = await _repository.RepeatPrintTasks(printTasksModel);
                        if (result)
                        {
                            taskIdList.Add(printTasksModel.ID);
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
        /// 无码报工打印标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Decimal>> NoCodeReportPrint([FromBody] NoCodeReportPrintRequestModel model)
        {
            ApiBaseReturn<Decimal> returnVM = new ApiBaseReturn<Decimal>();

            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 检查参数
                    //1.先调用这个接口添加打印内容并返回ID给前端进行打印，打印成功调用CollectProducts/PostToUncodedReport提交产能报工

                    SfcsWo swModel = null;
                    Decimal order_no = -1;
                    SfcsPrintFiles sfcsPrintFiles = null;
                    SfcsPrintTasks ptModel = new SfcsPrintTasks();
                    SfcsContainerListListModel sclModel = new SfcsContainerListListModel();
                    if (model.WO_NO.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        swModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = model.WO_NO })).FirstOrDefault();
                        if (swModel == null)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //当前工单没有配置制程，请注意检查!
                            if (swModel.ROUTE_ID <= 0)
                            {
                                ErrorInfo.Set(_localizer["WO_NO_NOT_ROUTE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    if (model.USER_NAME.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.SITE_ID.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["SITE_ID_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        String no = _repository.QueryEx<String>("SELECT SRC.ORDER_NO FROM SFCS_OPERATION_SITES SOS,SFCS_ROUTE_CONFIG SRC WHERE  SRC.CURRENT_OPERATION_ID =SOS.OPERATION_ID AND SOS.ID = :ID AND SOS.ENABLED = 'Y' AND SRC.ROUTE_ID =:ROUTE_ID", new { ID = model.SITE_ID, ROUTE_ID = swModel.ROUTE_ID })?.FirstOrDefault();
                        if (no.IsNullOrEmpty())
                        {
                            ErrorInfo.Set(String.Format(_localizer["SITE_ORDER_NO_ERROR"], swModel.WO_NO), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            order_no = Convert.ToDecimal(no);
                            //使用SFCS_PACKING_CARTON_SEQ
                            Decimal sequence = _repository.QueryEx<Decimal>("SELECT SFCS_PACKING_CARTON_SEQ.NEXTVAL FROM DUAL ").FirstOrDefault();
                            //將序列轉成36進制表示
                            string result = Core.Utilities.RadixConvertPublic.RadixConvert(sequence.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);
                            //六位表示
                            string ReleasedSequence = result.PadLeft(6, '0');
                            string yymmdd = _repository.QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                            sclModel.CONTAINER_SN = "BOX" + yymmdd + ReleasedSequence;
                            model.CATRON_NO = sclModel.CONTAINER_SN;
                            sclModel.DATA_TYPE = GlobalVariables.CartonLable;
                            sclModel.PART_NO = swModel.PART_NO;
                            sclModel.SITE_ID = Convert.ToDecimal(model.SITE_ID);
                            sclModel.QUANTITY = model.REPORT_QTY;
                            sclModel.SEQUENCE = model.REPORT_QTY;
                            sclModel.FULL_FLAG = GlobalVariables.EnableY;
                        }
                    }

                    if (model.CATRON_NO.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["CATRON_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.REPORT_QTY < 1 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["QTY_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        List<SfcsRoutesSiteListModel> rList = await _repositoryc.GetRouteCapacityDataByWoId(swModel.ID, swModel.ROUTE_ID);
                        if (rList.IsNullOrWhiteSpace() || rList.Count <= 0)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_ROUTE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            if (order_no != 0 && order_no > 0)
                            {
                                //不是第一道工序
                                Decimal no = order_no - 10;
                                SfcsRoutesSiteListModel rsModel = rList.Where(m => m.ORDER_NO == no)?.FirstOrDefault();
                                SfcsRoutesSiteListModel rs2Model = rList.Where(m => m.ORDER_NO == order_no)?.FirstOrDefault();
                                if (rsModel == null || rs2Model == null)
                                {
                                    ErrorInfo.Set(_localizer["SITE_ID_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                else if ((rs2Model.PASS + model.REPORT_QTY) > rsModel.PASS)
                                {
                                    ErrorInfo.Set(_localizer["CAPACITY_NUM_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        ptModel.WO_NO = model.WO_NO;//工单号
                        ptModel.PART_NO = swModel.PART_NO;//料号
                        ptModel.OPERATOR = model.USER_NAME;//创建人员
                        ptModel.CARTON_NO = model.CATRON_NO;//箱号
                        SfcsPn sfcsPn = (await _repository.GetListByTableEX<SfcsPn>("*", "SFCS_PN", " And PART_NO=:PART_NO", new { PART_NO = swModel.PART_NO }))?.FirstOrDefault();
                        if (sfcsPn == null)
                        {
                            ErrorInfo.Set(_localizer["PART_NO_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        //查询打印模板 无码标签的LABEL_TYPE = 5
                        String printMappSql = @"SELECT SPF.* FROM SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES SPF WHERE SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 5";
                        String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
                        List<SfcsPrintFiles> sfcsPrintMapplist = _repository.QueryEx<SfcsPrintFiles>(printMappSqlByPn, new { PART_NO = swModel.PART_NO });

                        if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                        {
                            String printMappSqlByModel = printMappSql + " AND SPFM.MODEL_ID = :MODEL_ID";
                            sfcsPrintMapplist = _repository.QueryEx<SfcsPrintFiles>(printMappSqlByModel, new { MODEL_ID = sfcsPn.MODEL_ID });
                        }
                        if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                        {
                            String printMappSqlByFamilly = printMappSql + " AND SPFM.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID";
                            sfcsPrintMapplist = _repository.QueryEx<SfcsPrintFiles>(printMappSqlByFamilly, new { PRODUCT_FAMILY_ID = sfcsPn.FAMILY_ID });
                        }
                        if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                        {
                            String printMappSqlByCustor = printMappSql + " AND SPFM.CUSTOMER_ID = :CUSTOMER_ID";
                            sfcsPrintMapplist = _repository.QueryEx<SfcsPrintFiles>(printMappSqlByCustor, new { CUSTOMER_ID = sfcsPn.CUSTOMER_ID });
                        }
                        if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                        {
                            sfcsPrintMapplist = _repository.QueryEx<SfcsPrintFiles>(printMappSqlByPn, new { PART_NO = "000000" });
                        }
                        if (sfcsPrintMapplist != null && sfcsPrintMapplist.Count > 0)
                        {
                            sfcsPrintFiles = sfcsPrintMapplist.FirstOrDefault();
                        }
                        else
                        {
                            ErrorInfo.Set(_localizer["GET_TEMPLATE_FAIL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                     


                        ptModel.PRINT_FILE_ID = sfcsPrintFiles.ID;//打印文件ID

                        ImsPart resData = await _repositoryt.GetPartByPartNo(swModel.PART_NO);//根据料号获取产品信息
                        String part_name = resData != null ? resData.NAME : "";//品名
                        String part_desc = resData != null ? resData.DESCRIPTION : "";//产品规格
                        DateTime time = await _repository.GetCurrentDateTime();

                        StringBuilder stringBuilder = new StringBuilder();
                        //模板数据：工单号，料号，物料名称，规格，数量，时间，创建人，箱号
                        stringBuilder.AppendLine("WO_NO,PART_NO,PART_NAME,PART_DESC,QTY,CREATE_TIME,CREATE_NAME,CATRON_NO,QC_CODE");
                        //for (int i = 1; i <= model.REPORT_QTY; i++)
                        //{
                        String qc_code = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", ptModel.WO_NO, ptModel.PART_NO, part_name, part_desc, model.REPORT_QTY, time.ToString(), model.USER_NAME, model.CATRON_NO);
                        stringBuilder.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", ptModel.WO_NO, ptModel.PART_NO, part_name, part_desc, model.REPORT_QTY, time.ToString(), model.USER_NAME, model.CATRON_NO, qc_code));
                        //}
                        ptModel.PRINT_DATA = stringBuilder.ToString();//打印数据

                        
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SavePrintTasksData(ptModel, sclModel);
                        if (returnVM.Result <= 0)
                        {
                            ErrorInfo.Set(_localizer["ADD_PRINT_TASKS_FAIL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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

    }
}