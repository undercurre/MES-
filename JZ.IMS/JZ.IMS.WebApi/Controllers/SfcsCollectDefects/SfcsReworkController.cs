/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-09-14 11:46:24                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsReworkController                                      
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
    /// <summary>
    /// 返工作业 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsReworkController : BaseController
    {
        private readonly ISfcsReworkRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsReworkController> _localizer;

        public SfcsReworkController(ISfcsReworkRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsReworkController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 查询新工单列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetNewWorkNoData([FromQuery] SfcsWoNeWorkRequestModel model)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var list = await _repository.GetNewWorkNoData(model);
                    returnVM.Result = list.data;
                    returnVM.TotalCount = list.count;
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
		/// 根据新工单号查询数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SfcsReworkListModel>> GetNewReworkDataByNewNo([FromQuery] SfcsReworkRequestModel model)
        {
            ApiBaseReturn<SfcsReworkListModel> returnVM = new ApiBaseReturn<SfcsReworkListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var list = await _repository.GetNewReworkDataByNewNo(model);
                    returnVM.Result = list;
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
		/// 根据流水号查出返工作业数据信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SfcsReworkListModel>> GetReworkDataBySN([FromQuery] SfcsReworkRequestModel model)
        {
            ApiBaseReturn<SfcsReworkListModel> returnVM = new ApiBaseReturn<SfcsReworkListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var list = await _repository.GetReworkDataBySN(model);
                    returnVM.Result = list;
                    returnVM.TotalCount = 1;

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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsReworkModel model)
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

        /*
         * 
         * 
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
        public async Task<ApiBaseReturn<List<SfcsReworkListModel>>> LoadData([FromQuery]SfcsReworkRequestModel model)
        {
            ApiBaseReturn<List<SfcsReworkListModel>> returnVM = new ApiBaseReturn<List<SfcsReworkListModel>>();
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
                    var viewList = new List<SfcsReworkListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsReworkListModel>(x);
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
		     */

        #region 工单转移

        /// <summary>
        /// 获取旧工单的SN列表（分页）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<WoTransferListModel>> GetSNDataByOldWoOrCartonNo([FromQuery] SfcsWoNeWorkRequestModel model)
        {

            ApiBaseReturn<WoTransferListModel> returnVM = new ApiBaseReturn<WoTransferListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (model.TRANSFER_TYPE == 0)
                    {
                        //在制品转移：输入新工单-->带出新工单的制程-->作业员输入旧工单号--》明细内容显示输入旧工单号的SN列表（列表有复选框）-->查询选择SN-->选择新工单的工序-->点击保存
                        //（数据处理：新增转移工单记录SFCS_WO_REPLACE,修改SN(不包括不良的SN)的WO_ID ,route_ID wip_opeation_last_operation）根据选择的新工单修改数据
                        if (model.Key.IsNullOrEmpty())
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            SfcsWo swModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = model.Key }))?.FirstOrDefault();
                            if (swModel == null)
                            {
                                ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                model.WO_ID = swModel.ID;
                                model.CARTON_NO = "";
                            }
                        }
                    }
                    else if (model.TRANSFER_TYPE == 1)
                    {
                        //仓库转移：输入新工单-->作业员输入旧工单箱号-->列表显示箱号SN-->点检保存(数据处理：新增转移工单记录SFCS_WO_REPLACE,修改SFCS_RUNCARD表的WO_ID)-->打印外箱标签（标签内容根据新工单模块打印）
                        if (model.CARTON_NO.IsNullOrEmpty())
                        {
                            ErrorInfo.Set(_localizer["CARTON_NO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            model.WO_ID = -1;
                            model.SN = "";
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["TRANSFER_TYPE_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetSNDataByOldWoOrCartonNo(model);
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
        /// 保存工单转移数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<int>> SaveWoTransferData([FromBody] TransferSnRequestModel model)
        {
            ApiBaseReturn<int> returnVM = new ApiBaseReturn<int>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回
                    SaveWoTransferListModel wtModel = new SaveWoTransferListModel();//保存数据
                    wtModel.WOREPLACELIST = new List<SfcsWoReplaceListModel>();

                    if (model.TRANSFER_TYPE == 0)
                    {
                        //在制品转移
                        if ((model.CHOOSEINDEX < GlobalVariables.DecimalDefaults || model.CHOOSEINDEXVALUE < GlobalVariables.DecimalDefaults) && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["PROCESS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (String.IsNullOrEmpty(model.WO_NO) && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //获取新工单信息
                            SfcsWo swModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = model.WO_NO }))?.FirstOrDefault();
                            if (swModel == null)
                            {
                                ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                wtModel.WO_ID = swModel.ID;
                                wtModel.ROUTE_ID = swModel.ROUTE_ID;
                                model.PLANT_CODE = swModel.PLANT_CODE;
                                if (swModel.ROUTE_ID <= 0)
                                {
                                    ErrorInfo.Set(String.Format(_localizer["ROUTE_NULL"], model.WO_NO), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                else
                                {
                                    //校验新工单的制程数据
                                    List<SfcsRouteConfigListModel> routeConfigList = await _repository.GetListByTableEX<SfcsRouteConfigListModel>("*", "SFCS_ROUTE_CONFIG", " And ROUTE_ID=:ROUTE_ID ORDER BY ORDER_NO ", new { ROUTE_ID = swModel.ROUTE_ID });
                                    if (routeConfigList == null || routeConfigList.Count <= 0)
                                    {
                                        ErrorInfo.Set(String.Format(_localizer["ROUTE_OPERATING_NULL"], model.WO_NO), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    else
                                    {
                                        //校验当前制程选择的工序是不是新工单的工序
                                        SfcsRouteConfigListModel rcModel = routeConfigList.Where(m => m.ORDER_NO == model.CHOOSEINDEX && m.CURRENT_OPERATION_ID == model.CHOOSEINDEXVALUE)?.FirstOrDefault();
                                        if (rcModel == null)
                                        {
                                            ErrorInfo.Set(String.Format(_localizer["OPERATION_ERROR"], model.WO_NO), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        }
                                        else
                                        {
                                            //下一个即将进入的工序
                                            rcModel = routeConfigList.Where(m => m.ORDER_NO > rcModel.ORDER_NO)?.OrderBy(m => m.ORDER_NO).FirstOrDefault();
                                            wtModel.LAST_OPERATION = rcModel == null ? GlobalVariables.EndOperation : rcModel.CURRENT_OPERATION_ID;
                                            wtModel.WIP_OPERATION = (decimal)model.CHOOSEINDEXVALUE;
                                        }
                                    }

                                }
                            }
                        }
                        if ((model.SNLIST == null || model.SNLIST.Count < 1) && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["SNLIST_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        if (String.IsNullOrEmpty(model.OLD_WO_NO) && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //校验输入的旧工单是否存在
                            SfcsWo oswModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = model.OLD_WO_NO }))?.FirstOrDefault();
                            if (oswModel == null)
                            {
                                ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                wtModel.OLD_WO_ID = oswModel.ID;
                            }
                        }
                    }
                    else if (model.TRANSFER_TYPE == 1)
                    {
                        //仓库转移
                        wtModel.PRINTTASKS = new SfcsPrintTasksListModel();
                        if (model.CARTON_NO.IsNullOrEmpty())
                        {
                            ErrorInfo.Set(_localizer["CARTON_NO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //根据输入的箱号获取流水号数据
                            model.SNLIST = await _repository.GetListByTableEX<String>("SN", "SFCS_RUNCARD", " And CARTON_NO=:CARTON_NO ORDER BY SN ASC ", new { CARTON_NO = model.CARTON_NO });
                            if (model.SNLIST == null || model.SNLIST.Count < 1)
                            {
                                ErrorInfo.Set(_localizer["CARTON_SN_QTY0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }

                        if (String.IsNullOrEmpty(model.WO_NO) && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["WO_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //校验新工单号
                            SfcsWo swModel = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = model.WO_NO }))?.FirstOrDefault();
                            if (swModel == null)
                            {
                                ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                wtModel.WO_ID = swModel.ID;
                                model.PLANT_CODE = swModel.PLANT_CODE;
                                wtModel.PRINTTASKS.WO_NO = swModel.WO_NO;//工单号
                                wtModel.PRINTTASKS.PART_NO = swModel.PART_NO;//料号
                            }
                        }

                        if (!ErrorInfo.Status)
                        {
                            //获取转移后的打印数据 根据新工单进行打印 箱号使用原来的箱号
                            wtModel.PRINTTASKS.OPERATOR = model.USER_NAME;//创建人员
                            wtModel.PRINTTASKS.CARTON_NO = model.CARTON_NO;//箱号

                            SfcsPn sfcsPn = (await _repository.GetListByTableEX<SfcsPn>("*", "SFCS_PN", " And PART_NO=:PART_NO", new { PART_NO = wtModel.PRINTTASKS.PART_NO }))?.FirstOrDefault();
                            if (sfcsPn == null)
                            {
                                ErrorInfo.Set(_localizer["PART_NO_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                String printMappSql = @"SELECT SPF.* FROM SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES SPF WHERE SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 3";
                                String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
                                SfcsPrintFiles sfcsPrintFiles = null;
                                List<SfcsPrintFiles> sfcsPrintMapplist = _repository.QueryEx<SfcsPrintFiles>(printMappSqlByPn, new { PART_NO = sfcsPn.PART_NO });

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
                                    ErrorInfo.Set(_localizer["GET_PRINT_FILES_FAIL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }

                                if (!ErrorInfo.Status)
                                {

                                    wtModel.PRINTTASKS.PRINT_FILE_ID = sfcsPrintFiles.ID;//打印文件ID

                                    String LineNameSql = @"SELECT OL.OPERATION_LINE_NAME FROM SFCS_CONTAINER_LIST CL LEFT JOIN SFCS_OPERATION_SITES OS ON CL.SITE_ID = OS.ID LEFT JOIN SFCS_OPERATION_LINES OL ON OS.OPERATION_LINE_ID = OL.ID WHERE CL.CONTAINER_SN = :CARTON_NO ";
                                    String line_name = _repository.QueryEx<String>(LineNameSql, new { CARTON_NO = model.CARTON_NO }).FirstOrDefault();//线体名称

                                    StringBuilder snheader = new StringBuilder();
                                    StringBuilder snDetail = new StringBuilder();
                                    for (int i = 0; i < model.SNLIST.Count; i++)
                                    {
                                        string index = (i + 1).ToString();
                                        snheader.Append(String.Format(",SN{0}", index));
                                        snDetail.Append(String.Format(",{0}", model.SNLIST[i]));
                                    }
                                    StringBuilder stringBuilder = new StringBuilder();
                                    stringBuilder.AppendLine(String.Format("BOX_NO,PN,MODEL,LINE_NAME,PRODUCT_TIME,QTY,QR_NO,WO_NO{0}", snheader.ToString()));
                                    stringBuilder.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7}{8}",
                                    model.CARTON_NO, sfcsPn.PART_NO, sfcsPn.DESCRIPTION, line_name, DateTime.Now, model.SNLIST.Count, model.CARTON_NO, model.WO_NO, snDetail.ToString()));

                                    wtModel.PRINT_DATA = stringBuilder.ToString();//打印数据
                                }
                            }

                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["TRANSFER_TYPE_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (String.IsNullOrEmpty(model.USER_NAME) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["USER_NAME_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        foreach (String sn in model.SNLIST)
                        {
                            SfcsRuncardListModel runcard = (await _repository.GetListByTableEX<SfcsRuncardListModel>("*", "SFCS_RUNCARD", " And SN=:SN", new { SN = sn }))?.FirstOrDefault();
                            if (runcard == null)
                            {
                                ErrorInfo.Set(String.Format(_localizer["SN_INFO_FAIL"], sn), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                break;
                            }
                            else if (runcard.WO_ID != wtModel.OLD_WO_ID && model.TRANSFER_TYPE == 0)
                            {
                                ErrorInfo.Set(String.Format(_localizer["SN_WO_ERROR"], sn, model.WO_NO), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                break;
                            }
                            else if (runcard.WO_ID == wtModel.WO_ID)//转移和被转移的不能是同一个
                            {
                                ErrorInfo.Set(String.Format(_localizer["SAME_WO_ERROR"], sn, model.WO_NO), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                break;
                            }

                            SfcsWoReplaceListModel wrModel = new SfcsWoReplaceListModel();
                            wrModel.SN_ID = runcard.ID;
                            wrModel.NEW_WO_ID = wtModel.WO_ID;
                            wrModel.OLD_WO_ID = runcard.WO_ID;
                            wrModel.PLANT_CODE = model.PLANT_CODE;
                            wrModel.REPLACE_TYPE = 4;
                            wrModel.REPLACE_SITE_ID = runcard.CURRENT_SITE;
                            wrModel.REPLACE_BY = model.USER_NAME;
                            wtModel.WOREPLACELIST.Add(wrModel);
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SaveWoReplaceByType(wtModel, model.TRANSFER_TYPE);
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

        #endregion

    }
}