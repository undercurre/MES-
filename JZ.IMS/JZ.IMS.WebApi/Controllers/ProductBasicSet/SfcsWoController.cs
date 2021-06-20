/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 16:44:04                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsWoController                                      
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
    /// 工单设定控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsWoController : BaseController
	{
		private readonly ISfcsWoRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SfcsWoController> _localizer;
		
		public SfcsWoController(ISfcsWoRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SfcsWoController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}

        public class IndexVM
        {
            ///// <summary>
            ///// 机种下拉数据
            ///// </summary>
            //public List<dynamic> ModelList { get; set; }
            /// <summary>
            /// 制程设定下拉数据
            /// </summary>
            public List<dynamic> RoutesList { get; set; }
            /// <summary>
            /// 工序下拉数据
            /// </summary>
            public List<dynamic> OperationsList { get; set; }
            /// <summary>
            /// 厂部下拉数据
            /// </summary>
            public List<dynamic> PlantCodeList { get; set; }
            /// <summary>
            /// 工单类别下拉数据
            /// </summary>
            public List<dynamic> WoClassificationList { get; set; }
            /// <summary>
            /// 制造群下拉数据
            /// </summary>
            public List<dynamic> BuCodeList { get; set; }
            /// <summary>
            /// 工单类型下拉数据
            /// </summary>
            public List<dynamic> WoTypeList { get; set; }
            /// <summary>
            /// 工单生产阶段下拉数据
            /// </summary>
            public List<dynamic> ProductStageList { get; set; }
            /// <summary>
            /// 工单状态下拉数据
            /// </summary>
            public List<dynamic> WoStatusList { get; set; }
            /// <summary>
            /// 制造类别下拉数据
            /// </summary>
            public List<dynamic> ManufactureTypeList { get; set; }
            /// <summary>
            /// Y/N
            /// </summary>
            public List<string> EnabledTypeList { get; set; }
        }

        /// <summary>
        /// 首页视图
        /// 能否编辑在于工单状态为look_up=1,meaning=Not Input的时候
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
                            //ModelList = await _repository.GetListByTable("  SM.ID,SM.MODEL ", "SFCS_MODEL SM ", " And Enabled='Y' "),
                            RoutesList = await _repository.GetListByTable("  SR.ID,SR.ROUTE_NAME ", " SFCS_ROUTES SR ", " AND SR.ENABLED = 'Y'"),
                            OperationsList = await _repository.GetListByTable("  SO.ID,SO.OPERATION_NAME ", " SFCS_OPERATIONS SO ", ""),
                            PlantCodeList = await _repository.GetListByTable("  SP.ID,SP.LOOKUP_CODE,SP.MEANING ", " SFCS_PARAMETERS SP ", " AND SP.LOOKUP_TYPE='PLANT_CODE' ORDER BY LOOKUP_TYPE"),
                            WoClassificationList = await _repository.GetListByTable("  ID,LOOKUP_CODE,MEANING ", " SFCS_PARAMETERS ", " AND LOOKUP_TYPE='WO_CLASSIFICATION' AND ENABLED='Y' ORDER BY LOOKUP_TYPE"),
                            BuCodeList = await _repository.GetListByTable("  ID,LOOKUP_CODE,MEANING ", " SFCS_PARAMETERS ", " And LOOKUP_TYPE='BU_CODE' AND ENABLED='Y' ORDER BY LOOKUP_TYPE "),
                            WoTypeList = await _repository.GetListByTable("  ID,LOOKUP_CODE,MEANING ", " SFCS_PARAMETERS ", " And LOOKUP_TYPE='WO_TYPE' AND ENABLED='Y' ORDER BY LOOKUP_TYPE "),
                            ProductStageList = await _repository.GetListByTable("  ID,LOOKUP_CODE,MEANING ", " SFCS_PARAMETERS ", " And LOOKUP_TYPE='PRODUCT_STAGE' AND ENABLED='Y' ORDER BY LOOKUP_TYPE "),
                            WoStatusList = await _repository.GetListByTable("  ID,LOOKUP_CODE,MEANING ", " SFCS_PARAMETERS ", " And LOOKUP_TYPE='WO_STATUS' AND ENABLED='Y' ORDER BY LOOKUP_TYPE "),
                            ManufactureTypeList = await _repository.GetListByTable("  ID,LOOKUP_CODE,MEANING ", " SFCS_PARAMETERS ", " And LOOKUP_TYPE='MANUFACTURE_TYPE' AND ENABLED='Y' ORDER BY LOOKUP_TYPE "),
                            EnabledTypeList = new List<string> { "Y","N"},

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
        /// 工单查询 搜索按钮对应的处理也是这个方法
        /// 制程详细配置请使用SfcsRoutes控制器 -》/api/SfcsRoutes/LoadRouteConfig即可
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsWoListModel>>> LoadData([FromQuery]SfcsWoRequestModel model)
        {
            ApiBaseReturn<List<SfcsWoListModel>> returnVM = new ApiBaseReturn<List<SfcsWoListModel>>();
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
                    if (!model.WO_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(WO_NO, :WO_NO) > 0 ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsWoListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsWoListModel>(x);
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
        /// 获取工单设定带MODEL中文
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadDataEX([FromQuery]SfcsWoRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.TotalCount = 0;
                    returnVM.Result = null;
                    var result = await _repository.GetWOList(model);
                    returnVM.Result = result.data;
                    returnVM.TotalCount = result.count;

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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsWoModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        if (model.UpdateRecords!=null)
                        {
                            foreach (var item in model.UpdateRecords)
                            {
                                if (item.TARGET_QTY<=0&&!ErrorInfo.Status)
                                {
                                    ErrorInfo.Set(_localizer["target_qty_null_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                if (item.TARGET_QTY>0&&item.TARGET_QTY!=null&&!ErrorInfo.Status)
                                {
                                    
                                    if (item.TARGET_QTY < item.INPUT_QTY)
                                    {
                                        //工单数量不能小于已投产数量{0}。
                                        ErrorInfo.Set(string.Format(_localizer["input_qty_err"],item.INPUT_QTY), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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

        /// <summary>
        /// 查询开工工单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<TableDataModel>> GetProductionWO([FromQuery] SfcsProductionRequestModel model)
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();
            if (!ErrorInfo.Status)
            {

                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repository.GetProductionWO(model);

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