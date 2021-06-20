/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：首五件检验主表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-13 09:51:01                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IMesFirstCheckInfoController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using FluentValidation.Results;
using JZ.IMS.IRepository;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Controllers;
using AutoMapper;
using System.Collections.Generic;
using System.Reflection;
using JZ.IMS.WebApi.Public;
using JZ.IMS.Models;

namespace JZ.IMS.Admin.Controllers
{
    /// <summary>
    /// DIP/组装线首件确认 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesFirstCheckInfoController : BaseController
	{
		private readonly IMesFirstCheckInfoRepository _repository;
		private readonly ISfcsOperationLinesRepository _lineRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISfcsParametersRepository _paraRepository;
		private readonly ISfcsProductionRepository _productionRepository;
		private readonly IMesFirstCheckItemsRepository _checkItemRepository;
        private readonly IMapper _mapper;

        public MesFirstCheckInfoController(IMesFirstCheckInfoRepository repository,
			ISfcsOperationLinesRepository lineRepository, IHttpContextAccessor httpContextAccessor,
			ISfcsParametersRepository paraRepository, ISfcsProductionRepository productionRepository,
			IMesFirstCheckItemsRepository checkItemRepository, IMapper mapper)
		{
			_repository = repository;
			_lineRepository = lineRepository;
			_httpContextAccessor = httpContextAccessor;
			_paraRepository = paraRepository;
			_productionRepository = productionRepository;
			_checkItemRepository = checkItemRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// LINE_ID:产线ID
        /// DEPARTMENT：部门ID
        /// STATUS：状态
        /// RESULT_STATUS：所有结果
        /// BATCH_NO：批号工单号
        /// PART_NO： 物料编码
        /// 时间范围 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
		public async Task<ApiBaseReturn<List<MesFirstCheckInfoListModel>>> LoadData([FromQuery]MesFirstCheckInfoRequestModel model)
		{
            ApiBaseReturn<List<MesFirstCheckInfoListModel>> returnVM = new ApiBaseReturn<List<MesFirstCheckInfoListModel>>();
            var ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {
                    string conditions = " WHERE ID > 0 ";//"where Is_Delete='N' ";//未删除的

                    if (!string.IsNullOrWhiteSpace(model.LINE_ID) && Convert.ToInt32(model.LINE_ID) > -1)
                        conditions += " AND LINE_ID=:LINE_ID ";

                    if (!string.IsNullOrWhiteSpace(model.DEPARTMENT) && Convert.ToInt32(model.DEPARTMENT) > -1)
                        conditions += " AND DEPARTMENT=:DEPARTMENT ";

                    if (!string.IsNullOrWhiteSpace(model.STATUS) && Convert.ToInt32(model.STATUS) > -1)
                        conditions += " AND STATUS=:STATUS ";

                    if (!string.IsNullOrWhiteSpace(model.RESULT_STATUS) && Convert.ToInt32(model.RESULT_STATUS) > -1)
                        conditions += " AND RESULT_STATUS=:RESULT_STATUS ";

                    if (!string.IsNullOrEmpty(model.BATCH_NO))
                        conditions += " AND INSTR(BATCH_NO,:BATCH_NO)>0 ";

                    if (!string.IsNullOrEmpty(model.PART))
                        conditions += " AND (INSTR(PART_NO,:PART)>0 OR INSTR(PART_NAME,:PART)>0 OR INSTR(PART_DESC,:PART)>0) ";

                    if (model.BEGIN_TIME != null)
                        conditions += " AND CREATE_TIME >= :BEGIN_TIME ";

                    if (model.END_TIME != null)
                        conditions += " AND CREATE_TIME <= :END_TIME ";

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var lines = _lineRepository.GetLinesList();
                    var depts = _paraRepository.GetDepartmentList();
                    var viewList = new List<MesFirstCheckInfoListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesFirstCheckInfoListModel>(x);

                        var line = lines.SingleOrDefault(f => f.LINE_ID == x.LINE_ID);
                        if (line != null)
                            item.LINE_NAME = line.LINE_NAME;

                        var dept = depts.SingleOrDefault(f => f.ID == x.DEPARTMENT);
                        if (dept != null)
                            item.DEPARTMENT_NAME = dept.CHINESE;

                        viewList.Add(item);
                    });

                    var data = new TableDataModel
                    {
                        //TODO：model如新增参数，则需在此方法也增加传入参数
                        count = await _repository.RecordCountAsync(conditions, model),
                        data = viewList,
                    };

                    returnVM.Result = data.data;
                    returnVM.TotalCount = data.count;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
			return returnVM;
		}


		/// <summary>
		/// 添加或修改明细获取数据
        /// 打开检验情况界面时加载
		/// </summary>
        /// <param name="mstId">MST_ID</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public ApiBaseReturn<DetilsData> AddOrModifyDetail(decimal mstId)
		{
            ApiBaseReturn<DetilsData> returnVM = new ApiBaseReturn<DetilsData>();
            if (!ErrorInfo.Status)
            {
                try

                {
                    DetilsData detilsData = new DetilsData();
                    var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    detilsData.mesFirstCheckItemsListModelsList = _checkItemRepository.GetItemsData(orginizeId);
                    detilsData.MST_ID = mstId;
                    returnVM.Result = detilsData;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            WriteLog(ref returnVM);

            return returnVM;
        }

		/// <summary>
		/// 添加或修改的保存相关操作
		/// </summary>
		/// <param name="item">请求体中的数据的映射</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> AddOrModifyAsyncSave([FromBody]MesFirstCheckInfoAddOrModifyModel item)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    MesFirstCheckInfo model;
                    if (item.ID == 0)
                    {
                        //item.CREATE_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                        item.CREATE_TIME = DateTime.Now; 
                        //TODO ADD
                        model = _mapper.Map<MesFirstCheckInfo>(item);
                        model.ID = await Task.Run(() => { return _repository.GetSEQID(); });
                        if (await _repository.InsertAsync(model) > 0)
                        { 
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                    else
                    {
                        //TODO Modify
                        //item.UPDATE_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                        item.UPDATE_TIME = DateTime.Now;
                        model = _repository.Get(item.ID);
                        if (model != null)
                        {
                            model.LINE_ID = item.LINE_ID;
                            model.DEPARTMENT = item.DEPARTMENT;
                            model.PART_NO = item.PART_NO;
                            model.PART_NAME = item.PART_NAME;
                            model.PART_DESC = item.PART_DESC;
                            model.BATCH_NO = item.BATCH_NO;
                            model.BATCH_QTY = item.BATCH_QTY;
                            model.NEW_PART = item.NEW_PART;
                            model.PRODUCT_DATE = item.PRODUCT_DATE;
                            model.UPDATE_USER = item.UPDATE_USER;
                            model.UPDATE_TIME = item.UPDATE_TIME;
                            //model.Modify_Time = DateTime.Now;
                            if (await _repository.UpdateAsync(model) > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                                ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            }
                        }
                        else
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonFailNoDataMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            WriteLog(ref returnVM);

			return returnVM;
		}

		/// <summary>
		/// 检验明细添加或修改的保存相关操作
		/// </summary>
		/// <param name="item">请求体中的数据的映射</param>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> AddOrModifyDetailSave([FromBody]MesFirstCheckDetailAddOrModifyModel item)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //if (item.ID == 0)
                        //item.CHECK_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;

                    BaseResult result = new BaseResult();
                    result = await _repository.AddOrModifyDetailSave(item);
                    if (result.ResultCode == 0)
                    {
                        returnVM.Result = true;
                    }
                    else
                    {
                        returnVM.Result = false;
                        ErrorInfo.Set(result.ResultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
        }

		/// <summary>
		/// 通过ID删除记录
		/// </summary>
		/// <param name="Id">要删除的记录的ID</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize]
		public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal Id)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (Id <= 0)
                    {
                        returnVM.Result = false;
                        ErrorInfo.Set(ResultCodeAddMsgKeys.CommonModelStateInvalidMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else
                    {
                        var count = await _repository.DeleteAsync(Id);
                        await _repository.DeleteDetailByMstId(Id);
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
        }

		/// <summary>
		/// 删除首五件明细数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        [HttpPost]
        [Authorize]
		public async Task<ApiBaseReturn<bool>> DeleteDetail(decimal id)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (id <= 0)
                    {
                        returnVM.Result = false;
                        ErrorInfo.Set(ResultCodeAddMsgKeys.CommonModelStateInvalidMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else
                    {
                        BaseResult result = new BaseResult();
                        result = await _repository.DeleteDetail(id);
                        if (result.ResultCode  == 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
		}

        /// <summary>
        /// 根据线别ID获取当前线别生产信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
		public async Task<ApiBaseReturn<SfcsProductionInfo>> GetProductionInfo(decimal lineId)
		{
            ApiBaseReturn<SfcsProductionInfo> returnVM = new ApiBaseReturn<SfcsProductionInfo>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var result = await _productionRepository.GetProductionInfo(lineId);
                    returnVM.Result = result;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
		}

        /// <summary>
        /// 获取首五件检验明细数据
        /// </summary>
        /// <param name="mstId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesFirstCheckDetailListModel>>> GetDetailData(decimal mstId)
        {
            ApiBaseReturn<List<MesFirstCheckDetailListModel>> returnVM = new ApiBaseReturn<List<MesFirstCheckDetailListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var result = await _repository.GetDetailData(mstId);
                    var data = new TableDataModel
                    {
                        count = result.Count(),
                        data = result
                    };
                    returnVM.Result = data.data;
                    returnVM.TotalCount = data.count;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
		}

        /// <summary>
        /// 获取检验明细项目信息
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
		public async Task<ApiBaseReturn<List<MesFirstCheckDetailItemListModel>>> GetDetailItemData(decimal detailId)
		{
            ApiBaseReturn<List<MesFirstCheckDetailItemListModel>> returnVM = new ApiBaseReturn<List<MesFirstCheckDetailItemListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var result = (await _repository.GetDetailItemData(detailId)).ToList();
                    var data = new TableDataModel
                    {
                        count = result.Count(),
                        data = result
                    };
                    returnVM.Result = data.data;
                    returnVM.TotalCount = data.count;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
		}

		/// <summary>
		/// 审核首五件信息
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
        [Authorize]
		public async Task<ApiBaseReturn<bool>> AuditData([FromBody]MesFirstCheckInfoAddOrModifyModel item)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    item.AUDIT_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                    var result = await _repository.AuditData(item);
                    if (result.ResultCode == 0)
                    {
                        returnVM.Result = true;
                    }
                    else
                    {
                        returnVM.Result = false;
                        ErrorInfo.Set(result.ResultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
			
			return returnVM;
		}
	}

    public class DetilsData
    {
        public List<MesFirstCheckItemsListModel> mesFirstCheckItemsListModelsList { get; set; }
        public Decimal MST_ID { get; set; }    }
}