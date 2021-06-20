/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-20 11:41:41                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsProductOperationMonitorController                                      
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

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 产品流程管控配置控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsProductOperationMonitorController : BaseController
    {
        private readonly ISfcsProductOperationMonitorRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsProductOperationMonitorController> _localizer;

        public SfcsProductOperationMonitorController(ISfcsProductOperationMonitorRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsProductOperationMonitorController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 监控模式
            /// </summary>
            /// <returns></returns>
            public List<dynamic> MonitorModeList { get; set; }
            /// <summary>
            /// OPERATION工序下拉框
            /// </summary>
            /// <returns></returns>
            public List<dynamic> OperationList { get; set; }
            /// <summary>
            /// 当前工序和开始工序
            /// </summary>
            /// <returns></returns>
            public List<dynamic> CurrentOperationidList { get; set; }
            /// <summary>
            /// 结束工序和返修工序(一样)
            /// </summary>
            /// <returns></returns>
            public List<dynamic> CurrentReworkList { get; set; }
            /// <summary>
            /// 下一个工序
            /// </summary>
            /// <returns></returns>
            public List<dynamic> NextOptionskList { get; set; }
            /// <summary>
            /// 单位
            /// </summary>
            /// <returns></returns>
            public List<dynamic> MonitorUnitList { get; set; }
            /// <summary>
            /// 对比模式
            /// </summary>
            /// <returns></returns>
            public List<dynamic> CompareModeList { get; set; }

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
                        returnVM.Result = new IndexVM()
                        {
                            MonitorModeList = await _repository.GetListByTable(" SP.id,SP.Lookup_Code,SP.Meaning,SP.Description,SP.Chinese ", " SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='MONITOR_MODE' and Enabled='Y' ORDER BY LOOKUP_TYPE "),
                            MonitorUnitList = await _repository.GetListByTable(" SP.ID,SP.Lookup_Code,SP.Meaning,SP.Description,SP.Chinese ", " SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='MONITOR_UNIT' and Enabled='Y' ORDER BY LOOKUP_TYPE "),
                            CompareModeList = await _repository.GetListByTable(" SP.ID,SP.Lookup_Code,SP.Meaning,SP.Description,SP.Chinese ", " SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='COMPARE_MODE' and Enabled='Y' ORDER BY LOOKUP_TYPE "),
                            OperationList = await _repository.GetListByTable(" SO.ID,SO.Description,So.Operation_Name ", " SFCS_OPERATIONS SO ", " And Enabled='Y' ")
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
        /// 获取当前工序和返工工序
        /// </summary>
        /// <param name="roundid">制程ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<IndexVM>> GetCurrentReworkOption(string roundid)
        {
            ApiBaseReturn<IndexVM> returnVM = new ApiBaseReturn<IndexVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status && !roundid.IsNullOrWhiteSpace())
                    {
                        returnVM.Result = new IndexVM()
                        {
                            CurrentOperationidList = await _repository.GetListByTable("  SRC.PRODUCT_OPERATION_CODE,SO.DESCRIPTION,SRC.ORDER_NO ", "  SFCS_ROUTE_CONFIG SRC, SFCS_OPERATIONS SO ", @" And SRC.CURRENT_OPERATION_ID=SO.ID
                                                                                        AND SO.ENABLED = 'Y'  AND SRC.ROUTE_ID =:ROUTE_ID ORDER BY SRC.ORDER_NO  ", new { ROUTE_ID = roundid }),
                            CurrentReworkList = await _repository.GetListByTable(@" SRC.PRODUCT_OPERATION_CODE,SOO.DESCRIPTION CURRENT_OPERATION,SRC.Repair_Operation_Id
                                                                                 , SOR.DESCRIPTION REPAIR_OPERATION ", " SFCS_ROUTE_CONFIG SRC, SFCS_OPERATIONS SOO,SFCS_OPERATIONS SOR ", @"　And SRC.CURRENT_OPERATION_ID = SOO.ID AND SRC.REPAIR_OPERATION_ID = SOR.ID AND SOO.ENABLED = 'Y' AND SOR.ENABLED = 'Y' AND SRC.ROUTE_ID = :ROUTE_ID  ORDER BY SRC.ORDER_NO", new { ROUTE_ID = roundid }),
                            NextOptionskList = await _repository.GetListByTable(" SRC.PRODUCT_OPERATION_CODE,SOO.DESCRIPTION CURRENT_OPERATION,SON.ID NEXT_OPERATION_ID,SON.DESCRIPTION NEXT_OPERATION ", " SFCS_ROUTE_CONFIG SRC, SFCS_OPERATIONS SOO,SFCS_OPERATIONS SON ",
                            " And SRC.CURRENT_OPERATION_ID = SOO.ID AND SRC.NEXT_OPERATION_ID = SON.ID AND SOO.ENABLED = 'Y' AND SON.ENABLED = 'Y' AND SRC.ROUTE_ID = :ROUTE_ID  ORDER BY SRC.ORDER_NO ", new { ROUTE_ID = roundid }),
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
        /// 重复接口调用和验证文档
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> GetDocumentAPI()
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();


            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = @"1.料号用户输入了先判断是否存在，请调用/api/SfcsPn/LoadData,如果存在就给操作下一步，
                                        如果不存在就不给操作下下。
                                        2.制程的接口，请使用/api/SfcsRoutes/LoadData 
                                        3.切换监控模式:
                                            OnWIPQuantityMonitor:1和
                                        RepairWIPQuantityMonitor:2
                                        中止标准设置为只读,返修工序有,存在同时有数据源
                                        开始工序名称修改为限制投入工序 结束工序名称修改为执行操作工序 返工工序名称修改为实际管控工序
                                        this.hintMemoEdit.Text = '触发中止标准时，默认按料号 + 投入工序锁定产品投入';
                                        WIPTimeMonitor:3
                                        中止标准不显示
                                        返修工序不存在 存在开始工序和结束工序
                                        this.hintMemoEdit.Text = '按单个SN管控流程耗时，触发中止标准，且到达管控的结束工序，参考锁定配置，决定是否锁 定SN。';
                                        WIPOverTimeMonitor:4
                                        中止标准设置为只读 返修工序不显示 开始工序名称修改为开始工序/限制投入工序
                                        结束工序为结束工序;
                                        4.【刷新受控工序數據源】
                                        如果是OnWIPQuantityMonitor(1) 实际管控工序  就是为nextOperationDataSource 对应的数据NextOptionskList 读取字段:NEXT_OPERATION（工序名称）
                                        如果是RepairWIPQuantityMonitor(2) 实际管控工序  就是为repairOperationDataSource 对应数据CurrentReworkList 读取的字段CURRENT_OPERATION(工序名称),:REPAIR_OPERATION(维修工序)
                                        5.[验证信息] 1.判断料号是否存在 2.如果没有找到制程配置就提示没有找到制程配置 
                                        6.新增一行时monitorMode的下拉框
                                        OnWIPQuantityMonitor RepairWIPQuantityMonitor WIPOverTimeMonitor
                                             STOP_AND_HOLD=‘Y’
                                             STOP_AND_HOLD=Pieces(1)
                                         WIPTimeMonitor
                                             STOP_AND_HOLD='N'
                                             CRITERIA_UNIT=Hour(2)
                                        LaserPrintMonitor
                                             STOP_AND_HOLD='Y'
                                             CRITERIA_UNIT=Hour(2)
                                        7.实际管控工序不用上传保存数据。
                                        8 BeginOperation = '开始工序';
                                          EndOperation = '结束工序';
                                          InputControlOperation = '限制投入工序';
                                          CurrentOperation = '执行操作工序';
                                          InChargeOperation = '实际管控工序'
                                       9.保存的时候不要传Route_ID 
                                        ";

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

        #region 查询数据
        /// <summary>
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        //[HttpGet]
        //[Authorize]
        //public async Task<ApiBaseReturn<List<SfcsProductOperationMonitorListModel>>> LoadData([FromQuery]SfcsProductOperationMonitorRequestModel model)
        //{
        //    ApiBaseReturn<List<SfcsProductOperationMonitorListModel>> returnVM = new ApiBaseReturn<List<SfcsProductOperationMonitorListModel>>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 设置返回值

        //            int count = 0;
        //            string conditions = " WHERE ID > 0 ";
        //            if (!model.Key.IsNullOrWhiteSpace())
        //            {
        //                //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0)";
        //            }
        //            var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
        //            List<SfcsOperations> operationslist = await _repository.GetListByTableEX<SfcsOperations>("  ", " SFCS_OPERATIONS ", " And Enabled='Y' ");
        //            var viewList = new List<SfcsProductOperationMonitorListModel>();
        //            list?.ForEach(x =>
        //            {
        //                var item = _mapper.Map<SfcsProductOperationMonitorListModel>(x);
        //                //item.ENABLED = (item.ENABLED == "Y");

        //                viewList.Add(item);
        //            });
        //            returnVM.Result = viewList;
        //            returnVM.TotalCount = count;

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

        #endregion

        /// <summary>
        /// 获取产品流程管控分页(制程配置总数需要统计获取回来的制程配置数量传入)
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadMonitorData([FromQuery]SfcsProductOperationMonitorRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.TotalCount = 0;
                    returnVM.Result = null;
                    if ((model.PART_NO.IsNullOrWhiteSpace() || (model.ROUTECONFIGCOUNT <= 0 || model.ROUTECONFIGCOUNT == null) || (model.ROUTE_ID <= 0 || model.ROUTE_ID == null) || (model.MONITOR_MODE <= 0) || model.MONITOR_MODE == null) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["Err_Not_Input_PN_Route_Mode"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.GetMonitorList(model);
                        returnVM.Result = result.data;
                        returnVM.TotalCount = result.count;
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
        /// 保存数据 还需要传一个ROUTE_ID制程ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsProductOperationMonitorModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 检查参数

                    List<List<SfcsProductOperationMonitorAddOrModifyModel>> productoperationmonitor = null;
                    if (model.InsertRecords != null || model.UpdateRecords != null)
                    {
                        productoperationmonitor = new List<List<SfcsProductOperationMonitorAddOrModifyModel>>();
                    }
                    //插入
                    if (model.InsertRecords != null && !ErrorInfo.Status)
                    {
                        productoperationmonitor.Add(model.InsertRecords);
                    }
                    //更新
                    if (model.UpdateRecords != null && !ErrorInfo.Status)
                    {
                        productoperationmonitor.Add(model.UpdateRecords);
                    }

                    if (productoperationmonitor != null)
                    {
                        foreach (var templist in productoperationmonitor)
                        {
                            //decimal tuOperationOrderNo = -1;
                            //SfcsRouteConfig tuOperation = null;
                            //if (!templist[0].ROUTE_ID.IsNullOrWhiteSpace())
                            //{
                            //    tuOperation = (await _repository.GetListByTableEX<SfcsRouteConfig>("SRC.* ", " SFCS_ROUTE_CONFIG SRC ", " And ROUTE_ID=:ROUTE_ID CURRENT_OPERATION_ID=550 ORDER BY ORDER_NO  ", new { ROUTE_ID = templist[0].ROUTE_ID })).FirstOrDefault();
                            //}
                            //if (tuOperation != null)
                            //{
                            //    tuOperationOrderNo = tuOperation.ORDER_NO;
                            //}
                            foreach (var item in templist)
                            {
                                switch ((int)item.MONITOR_MODE)
                                {
                                    case (int)GlobalVariables.OnWIPQuantityMonitor:
                                    case (int)GlobalVariables.RepairWIPQuantityMonitor:
                                    case (int)GlobalVariables.WIPTimeMonitor:
                                    case (int)GlobalVariables.WIPOverTimeMonitor:
                                        {
                                            if (item.BEGIN_OPERATION_CODE <= 0 && !ErrorInfo.Status)
                                            {
                                                ErrorInfo.Set(_localizer["Err_Not_Input_Begin_Operation"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                break;
                                            }
                                            if (item.END_OPERATION_CODE <= 0 && !ErrorInfo.Status)
                                            {
                                                ErrorInfo.Set(_localizer["Err_Not_Input_End_Operation"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                break;
                                            }
                                            if (item.ALARM_CRITERIA <= 0 && !ErrorInfo.Status)
                                            {
                                                ErrorInfo.Set(_localizer["Err_Not_Input_Alarm_Criteria"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                break;
                                            }
                                            if ((item.ALARM_CRITERIA > item.STOP_CRITERIA) && !ErrorInfo.Status)
                                            {
                                                ErrorInfo.Set(_localizer["Err_Alarm_Criteria_Over_Stop_Criteria"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                break;
                                            }
                                            var begin_operation = (await _repository.GetListByTableEX<SfcsRouteConfig>("SRC.* ", " SFCS_ROUTE_CONFIG SRC ", " And  PRODUCT_OPERATION_CODE=:PRODUCT_OPERATION_CODE   ORDER BY ORDER_NO  ", new { PRODUCT_OPERATION_CODE = item.BEGIN_OPERATION_CODE })).FirstOrDefault();
                                            var end_operation = (await _repository.GetListByTableEX<SfcsRouteConfig>("SRC.* ", " SFCS_ROUTE_CONFIG SRC ", " And  PRODUCT_OPERATION_CODE=:PRODUCT_OPERATION_CODE   ORDER BY ORDER_NO  ", new { PRODUCT_OPERATION_CODE = item.END_OPERATION_CODE })).FirstOrDefault();
                                            if (begin_operation != null && end_operation != null)
                                            {
                                                if (begin_operation.ORDER_NO > end_operation.ORDER_NO)
                                                {
                                                    ErrorInfo.Set(_localizer["Err_Begin_Operation_Over_End_Operation"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    //case (int)GlobalVariables.LaserPrintMonitor:
                                    //    {
                                    //        if (item.END_OPERATION_CODE <= 0&&!ErrorInfo.Status)
                                    //        {
                                    //            ErrorInfo.Set(_localizer["Err_Not_Input_End_Operation"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    //            break;
                                    //        }
                                    //        var end_operation = (await _repository.GetListByTableEX<SfcsRouteConfig>("SRC.* ", " SFCS_ROUTE_CONFIG SRC ", " And  PRODUCT_OPERATION_CODE=:PRODUCT_OPERATION_CODE And  ORDER BY ORDER_NO  ", new { PRODUCT_OPERATION_CODE = item.END_OPERATION_CODE })).FirstOrDefault();
                                    //        var pnbypartno = (await _repository.GetListByTableEX<SfcsPn>("SP.ID,SP.CLASSIFICATION,SP.CATEGORY ", " SFCS_PN SP ", " And SP.ID>0 And PART_NO=:PART_NO  ", new { PART_NO = item.PART_NO })).FirstOrDefault();
                                    //        decimal? plantCode = pnbypartno != null ? pnbypartno.CLASSIFICATION : 0 ;
                                    //        decimal? partNumberCategory = pnbypartno != null ? pnbypartno.CATEGORY : 0;
                                    //        if (plantCode == GlobalVariables.pcbCode)
                                    //        {
                                    //            // 含有T/U工序才需要比對
                                    //            if (tuOperationOrderNo != -1)
                                    //            {
                                    //                if ((partNumberCategory == GlobalVariables.SMTClassficationCode) &&
                                    //                    (end_operation.ORDER_NO > tuOperationOrderNo)&&end_operation!=null&&!ErrorInfo.Status)
                                    //                {
                                    //                    ErrorInfo.Set(_localizer["Err_SMT_PN_Over_Route"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    //                    break;
                                    //                }
                                    //                if ((partNumberCategory == GlobalVariables.PCBClassficationCode) &&
                                    //                    (end_operation.ORDER_NO <= tuOperationOrderNo) && end_operation != null && !ErrorInfo.Status)
                                    //                {
                                    //                    ErrorInfo.Set(_localizer["Err_PCB_PN_Over_Route"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    //                    break;
                                    //                }
                                    //            }
                                    //        }
                                    //        item.BEGIN_OPERATION_CODE = 0;
                                    //        break;
                                    //    }
                                    default:
                                        {
                                            if (!ErrorInfo.Status)
                                            {
                                                ErrorInfo.Set(_localizer["Err_No_Monitor_Mode"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                            }
                                            break;
                                        }
                                }
                            }
                        }
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

    }
}