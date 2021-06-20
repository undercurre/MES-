/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-08 08:52:34                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsRoutesController                                      
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
    /// 制程名称设定和制程设定控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsRoutesController : BaseController
    {
        private readonly ISfcsRoutesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsRoutesController> _localizer;

        public SfcsRoutesController(ISfcsRoutesRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsRoutesController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 厂部下拉数据
            /// </summary>
            public List<dynamic> PlantList { get; set; }
            /// <summary>
            /// 类型下拉数据
            /// </summary>
            public List<dynamic> TypeList { get; set; }
            /// <summary>
            /// 维修工序下拉数据
            /// </summary>
            public List<dynamic> RepairOperationList { get; set; }
            /// <summary>
            /// 当前工序下拉数据
            /// </summary>
            public List<dynamic> CurrentOperationList { get; set; }
            /// <summary>
            /// 获取没有返工工序
            /// </summary>
            public List<dynamic> GetNoSetNoRoute{get; set;}
            /// <summary>
            /// 获取开始和结束
            /// </summary>
            public List<dynamic> GetStartEnd { get; set; }
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
                            PlantList = await _repository.GetListByTable("  LOOKUP_CODE, MEANING，CHINESE ", "SFCS_PARAMETERS ", " And LOOKUP_TYPE = 'PLANT_CODE' "),
                            TypeList = await _repository.GetListByTable(" LOOKUP_CODE, MEANING，CHINESE ", "SFCS_PARAMETERS", " And LOOKUP_TYPE = 'ROUTE_TYPE' "),
                            CurrentOperationList = await _repository.GetListByTable("", "SFCS_OPERATIONS", " AND  OPERATION_CLASS=1 AND ID!=100 AND ID!=999  ORDER BY OPERATION_NAME  "),
                            RepairOperationList = await _repository.GetListByTable("", "SFCS_OPERATIONS", " AND OPERATION_CLASS=2 ORDER BY OPERATION_NAME  "),
                            GetNoSetNoRoute = await _repository.GetListByTable(" SO.* ", "SFCS_OPERATIONS SO ", " AND OPERATION_CLASS=3 "),
                            GetStartEnd = await _repository.GetListByTable(" OP.ID,OP.DESCRIPTION ", " SFCS_OPERATIONS OP ", " AND (ID=100 OR ID=999) "),
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
        /// 制程名称查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsRoutesListModel>>> LoadData([FromQuery]SfcsRoutesRequestModel model)
        {
            ApiBaseReturn<List<SfcsRoutesListModel>> returnVM = new ApiBaseReturn<List<SfcsRoutesListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(PART_NO, :PART_NO) > 0 ";
                    }
                    if (!model.ROUTE_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(UPPER(ROUTE_NAME), UPPER(:ROUTE_NAME)) > 0 ";
                    }
                    if (model.ROUTE_CLASS > 0)
                    {
                        conditions += $"and instr(ROUTE_CLASS, :ROUTE_CLASS) > 0 ";
                    }
                    if (model.ROUTE_TYPE > 0)
                    {
                        conditions += $"and instr(ROUTE_TYPE, :ROUTE_TYPE) > 0 ";
                    }
                    if (!model.DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(DESCRIPTION, :DESCRIPTION) > 0 ";
                    }
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(ENABLED, :ENABLED) > 0 ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Route_Name desc", model)).ToList();
                    var viewList = new List<SfcsRoutesListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsRoutesListModel>(x);
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
        /// 制程设定查询数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsRouteConfig>>> LoadRouteConfig([FromQuery]SfcsRouteConfigRequestModel model)
        {
            ApiBaseReturn<List<SfcsRouteConfig>> returnVM = new ApiBaseReturn<List<SfcsRouteConfig>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.ROUTE_ID > 0)
                    {
                        conditions += $"and ROUTE_ID=:ROUTE_ID ";
                    }
                    var list = (await _repository.GetListPagedEx<SfcsRouteConfig>(model.Page, model.Limit, conditions, "ORDER_NO ASC", model)).ToList();
                    var viewList = new List<SfcsRouteConfig>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsRouteConfig>(x);
                    //item.ENABLED = (item.ENABLED == "Y");
                    viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsyncEx<SfcsRouteConfig>(conditions, model);

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
        /// 制程名称验证数据文档
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<string> GetValString([FromQuery]string model)
        {
            string msg = "";
            msg += @"1.if (routes.ROUTE_TYPE(类型) != GlobalVariables.RMA_ROUTE(2)){if(当前行.REWORK_OPERATION_ID==GlobalVariables.NoRoute(1)){if (当前行.REPAIR_OPERATION_ID != GlobalVariables.NoneRepair)
					{errMsg = '没有返工工序，无需设置维修工序；';}}}
                      2.当行返工序号order_on> 后面的当前工序就会报错:返工工序选择错误，返工工序只能是当前工序或之前的工序，请确认。
                      3.当选的返工序号只能显示前面和当行当前工序，cs是会连同后面当前工序也显示，是不对的
                      4.检查每行不能出现相同的当前工序会。提示:制程中出现重复工序：{0}，请检查。
                      5.返工序号下拉里面在加入的时候最好带上一个order_no
                      6.每插入一行取一个product_operation_code使用接口
                      7.第一行的pervious_operation_id=100,最后一行next_operation_id=999 
                      8.第一行order_no为0 后面每行每次加10  
                      9.没有对应的返工工序(OPERATION_CLASS:3)
";
            return msg;
        }

        /// <summary>
        /// 在切换当前工序和维修工序 返回工序的时候，调用
        /// false不存在可以使用，true的时候不给添加或者修改
        /// </summary>
        /// <param name="operationCode">工序id</param>
        /// <param name="routeID">制程ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> CheckWIPExistedAPI(decimal operationCode, decimal routeID)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = true;
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (!ErrorInfo.Status)
                    {
                        if (operationCode > 0 && routeID > 0)
                        {
                            returnVM.Result = await _repository.CheckWIPExisted(operationCode, routeID);
                        }
                        else
                        {
                            ErrorInfo.Set(_localizer["params_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
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
        /// 制程名称保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsRoutesModel model)
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
        /// 制程设定保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> RoutesConfigSaveData([FromBody] SfcsRouteConfigModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #region 出现重复工序
                    List<List<SfcsRouteConfigAddOrModifyModel>> routeConfig = null;
                    if (model.InsertRecords != null || model.UpdateRecords != null)
                    {
                        routeConfig = new List<List<SfcsRouteConfigAddOrModifyModel>>();
                    }
                    //插入
                    if (model.InsertRecords != null && !ErrorInfo.Status)
                    {
                        routeConfig.Add(model.InsertRecords);
                    }
                    //更新
                    if (model.UpdateRecords != null && !ErrorInfo.Status)
                    {
                        routeConfig.Add(model.UpdateRecords);
                    }
                    if (routeConfig != null)
                    {
                        foreach (var templist in routeConfig)
                        {
                            //制程中出现重复工序：{0}，请检查。
                            // 檢查制程中是否含有相同的工序
                            List<SfcsRouteConfigAddOrModifyModel> temp = templist.OrderBy(c => c.CURRENT_OPERATION_ID).ToList();
                            for (int i = 0; i < temp.ToArray().Length - 1; i++)
                            {
                                if (((temp[i].CURRENT_OPERATION_ID == temp[i + 1].CURRENT_OPERATION_ID)) && !ErrorInfo.Status)
                                {
                                    string operationName = "";
                                    SfcsOperationsListModel operationFirst = await _repository.GetOperationDataTable(temp[i].CURRENT_OPERATION_ID);
                                    if (operationFirst != null)
                                    {
                                        operationName = operationFirst.OPERATION_NAME;
                                    }
                                    ErrorInfo.Set(string.Format(_localizer["OPERATION_REPEATED_ERR"], operationName), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }

                            foreach (var item in templist)
                            {

                                #region 檢查修改工序中是否有WIP
                                if (!ErrorInfo.Status)
                                {
                                    bool wipExisted = await _repository.CheckWIPExisted(item.CURRENT_OPERATION_ID, templist[0].ROUTE_ID);
                                    if (wipExisted)
                                    {
                                        string operationName = (await _repository.GetOperationDataTable(item.CURRENT_OPERATION_ID)).OPERATION_NAME;
                                        ErrorInfo.Set(string.Format(_localizer["currentdeleteopeartionhaswip_err"], operationName), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                                #endregion

                                #region 验证返工工序
                                if (!ErrorInfo.Status)
                                {
                                    var routes = await _repository.GetRoutesList(item.ROUTE_ID);
                                    new SfcsRoutesListModel();
                                    if (routes != null)
                                    {
                                        if (routes.ROUTE_TYPE != GlobalVariables.RMA_ROUTE)
                                        {
                                            if (item.REWORK_OPERATION_ID == GlobalVariables.NoRoute)
                                            {
                                                if (item.REPAIR_OPERATION_ID != GlobalVariables.NoneRepair)
                                                {
                                                    ErrorInfo.Set(_localizer["dontsetrework_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                }
                                            }
                                        }

                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    #endregion

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.RoutesConfigSaveData(model);
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
        /// 制程设定保存数据
        /// 只传新增数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> RoutesConfigSaveDataOfNew([FromBody] SfcsRouteConfigModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #region 工序验证
                    List<List<SfcsRouteConfigAddOrModifyModel>> routeConfig = null;
                    if (model.InsertRecords != null || model.UpdateRecords != null)
                    {
                        routeConfig = new List<List<SfcsRouteConfigAddOrModifyModel>>();
                    }
                    //插入
                    if (model.InsertRecords != null && !ErrorInfo.Status)
                    {
                        routeConfig.Add(model.InsertRecords);
                    }
                    
                    if (routeConfig != null&&routeConfig.Count>0)
                    {
                        foreach (var templist in routeConfig)
                        {
                            //检查排序 和前后工序是否对称
                            List<SfcsRouteConfigAddOrModifyModel> temp = templist.OrderBy(c => c.ORDER_NO).ToList();
                            int orderNo = 0;
                            decimal previousOperationId = 100;
                            decimal currentOperationId =0 ;
                            decimal nextOperationId = 0;
                            List<decimal> reworkOperationList = new List<decimal>();
                            //没有返工工序的默认值
                            decimal reWorkDefaultValue = 1;
                            reworkOperationList.Add(reWorkDefaultValue);
                            //存在重复工序
                            bool isRepeat = temp.GroupBy(i => i.CURRENT_OPERATION_ID).Where(g => g.Count() > 1).Count() > 0;
                            if (isRepeat)
                                throw new Exception(_localizer["OPERATION_REPEATED_ERR"]);

                            for (int i = 0;  i< temp.Count; i++)
                            {
                                //处理排序
                                if (temp[i].ORDER_NO == orderNo)
                                {
                                    //当第一条数据时
                                    if (temp[i].ORDER_NO == 0&& previousOperationId==temp[i].PREVIOUS_OPERATION_ID)
                                    {
                                        currentOperationId = temp[i].CURRENT_OPERATION_ID;
                                        nextOperationId = temp[i].NEXT_OPERATION_ID;
                                    }
                                    else
                                    {
                                        //上一个工序等于下一个工序时
                                        if (nextOperationId == temp[i].CURRENT_OPERATION_ID)
                                        {
                                            //下一个工序存起来
                                            nextOperationId = temp[i].NEXT_OPERATION_ID;
                                        }
                                        else
                                        {
                                            throw new Exception(_localizer["ERROR_OPERATION_NEXT_ID"]);
                                        }
                                    }

                                    //处理返工逻辑
                                    reworkOperationList.Add(temp[i].CURRENT_OPERATION_ID);
                                    if (temp[i].REWORK_OPERATION_ID != reWorkDefaultValue)
                                    {
                                        if (reworkOperationList.Count(c => c == temp[i].REWORK_OPERATION_ID)<=0)
                                        {
                                            //返工工序只能返回当前或者之前的工序，请注意检查!
                                            throw new Exception(_localizer["ERROR_OPERATION_REWORK"]);
                                        }
                                    }

                                    orderNo = orderNo + 10;
                                }
                                else
                                {
                                    throw new Exception(_localizer["ERROR_OPERATION_ORDER_NO"]);
                                }
                                
                            }


                            //制程中出现重复工序：{0}，请检查。
                            // 檢查制程中是否含有相同的工序
                             temp = templist.OrderBy(c => c.CURRENT_OPERATION_ID).ToList();
                            for (int i = 0; i < temp.ToArray().Length - 1; i++)
                            {
                                if (((temp[i].CURRENT_OPERATION_ID == temp[i + 1].CURRENT_OPERATION_ID)) && !ErrorInfo.Status)
                                {
                                    string operationName = "";
                                    SfcsOperationsListModel operationFirst = await _repository.GetOperationDataTable(temp[i].CURRENT_OPERATION_ID);
                                    if (operationFirst != null)
                                    {
                                        operationName = operationFirst.OPERATION_NAME;
                                    }
                                    ErrorInfo.Set(string.Format(_localizer["operation_repeated_err"], operationName), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }

                            foreach (var item in templist)
                            {
                                #region 檢查修改工序中是否有WIP
                                //if (!ErrorInfo.Status)
                                //{
                                //    bool wipExisted = await _repository.CheckWIPExisted(item.CURRENT_OPERATION_ID, templist[0].ROUTE_ID);
                                //    if (wipExisted)
                                //    {
                                //        string operationName = (await _repository.GetOperationDataTable(item.CURRENT_OPERATION_ID)).OPERATION_NAME;
                                //        ErrorInfo.Set(string.Format(_localizer["currentdeleteopeartionhaswip_err"], operationName), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                //    }
                                //}
                                #endregion

                                #region 验证返工工序
                                if (!ErrorInfo.Status)
                                {
                                    var routes = await _repository.GetRoutesList(item.ROUTE_ID);
                                    new SfcsRoutesListModel();
                                    if (routes != null)
                                    {
                                        if (routes.ROUTE_TYPE != GlobalVariables.RMA_ROUTE)
                                        {
                                            if (item.REWORK_OPERATION_ID == GlobalVariables.NoRoute)
                                            {
                                                if (item.REPAIR_OPERATION_ID != GlobalVariables.NoneRepair)
                                                {
                                                    ErrorInfo.Set(_localizer["OPERATION_REPEATED_ERR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                }
                                            }
                                        }

                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    #endregion

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.RoutesConfigSaveDataEx(model);
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