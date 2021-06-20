/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 首件/首五件确认 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-19 09:10:09                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IMesQualityInfoController                                      
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
using Microsoft.AspNetCore.Http;
using JZ.IMS.IRepository;
using AutoMapper;
using JZ.IMS.Models;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JZ.IMS.Admin.Controllers
{
    /// <summary>
    /// 首件/首五件确认 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesQualityInfoController : BaseController
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMesQualityInfoRepository _repository;
		private readonly ISfcsOperationLinesRepository _lineRepository;
		private readonly ISfcsParametersRepository _paraRepository;
		private readonly IMesQualityItemsRepository _qualityItemsRepository;
		private readonly ISfcsProductionRepository _productionRepository;
		private readonly IMapper _mapper;
        private readonly ISysOrganizeRepository _sysOrganizeRepository;
        public MesQualityInfoController(IHttpContextAccessor httpContextAccessor,
			IMesQualityInfoRepository repository, ISfcsOperationLinesRepository lineRepository,
			ISfcsParametersRepository paraRepository, ISfcsProductionRepository productionRepository,
			IMesQualityItemsRepository qualityItemsRepository, IMapper mapper, ISysOrganizeRepository sysOrganizeRepository)
		{
			this._httpContextAccessor = httpContextAccessor;
			this._repository = repository;
			this._lineRepository = lineRepository;
			this._paraRepository = paraRepository;
			this._mapper = mapper;
			this._qualityItemsRepository = qualityItemsRepository;
			this._productionRepository = productionRepository;
            this._sysOrganizeRepository = sysOrganizeRepository;

        }

		/// <summary>
		/// 首页视图
		/// </summary>
		/// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
		public ApiBaseReturn<DataList> Index()
		{
            ApiBaseReturn<DataList> returnVM = new ApiBaseReturn<DataList>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var jwtHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(tokenHeader);
                    object userName = new object();
                    jwtToken.Payload.TryGetValue(ClaimTypes.NameIdentifier, out userName);
                    //var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    DataList dataList = new DataList();
                    //获取类型
                    dataList.SfcsParameters = _paraRepository.GetListByType("MES_QUALITY_TYPE");
                    //获取产线
                    dataList.AllLinesModel = _lineRepository.GetLinesListByUser(userName.ToString());
                    //获取部门
                    dataList.SfcsDepartment = _paraRepository.GetDepartmentList();
                    //获取板型
                    dataList.SmtLookupList = _paraRepository.GetSmtLookupListByType("PCB_SIDE");
                    //获取首件类型
                    dataList.FirstItemTypeList = _paraRepository.GetListByType("FIRST_ITEM_TYPE");
                    dataList.FirstItemTypeList = dataList.FirstItemTypeList.OrderBy(s => s.LOOKUP_CODE).ToList<SfcsParameters>();
                    //获取环保状态
                    dataList.FirstEPStatusList = _paraRepository.GetListByType("FIRST_EP_STATUS");
                    dataList.FirstEPStatusList = dataList.FirstEPStatusList.OrderBy(s => s.LOOKUP_CODE).ToList<SfcsParameters>();
                    returnVM.Result = dataList;
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
		/// 查询所有
		/// 搜索按钮对应的处理也是这个方法
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[HttpGet]
        //[Authorize]
		public async Task<ApiBaseReturn<List<MesQualityInfoListModel>>> LoadData([FromQuery]MesQualityInfoRequestModel model)
		{
            ApiBaseReturn<List<MesQualityInfoListModel>> returnVM = new ApiBaseReturn<List<MesQualityInfoListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var jwtHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(tokenHeader);
                    object userName = new object();
                    jwtToken.Payload.TryGetValue(ClaimTypes.NameIdentifier, out userName);
                    // var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    string conditions = " WHERE ID > 0 ";

                    if (!string.IsNullOrWhiteSpace(model.CHECK_TYPE))
                        conditions += " AND CHECK_TYPE = :CHECK_TYPE ";

                    if (!string.IsNullOrWhiteSpace(model.LINE_ID))
                        conditions += " AND LINE_ID=:LINE_ID ";

                    if (!string.IsNullOrWhiteSpace(model.DEPARTMENT))
                        conditions += " AND DEPARTMENT=:DEPARTMENT ";

                    if (!string.IsNullOrWhiteSpace(model.STATUS))
                        conditions += " AND STATUS=:STATUS ";

                    if (model.FIRST_ITEM_TYPE >= 0)
                        conditions += " AND FIRST_ITEM_TYPE=:FIRST_ITEM_TYPE ";

                    if (model.EP_STATUS >= 0)
                        conditions += " AND EP_STATUS=:EP_STATUS ";

                    if (!string.IsNullOrWhiteSpace(model.RESULT_STATUS))
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
                    List<AllLinesModel> lines = _lineRepository.GetLinesListByUser(userName.ToString());//有重复数据
                    lines = lines.Where((x, i) => lines.FindIndex(m => m.LINE_ID == x.LINE_ID) == i).ToList();
                    var depts = _paraRepository.GetDepartmentList();
                    var checkTypes = _paraRepository.GetListByType("MES_QUALITY_TYPE");
                    var userNameList =await _paraRepository.GetListByTableEX<Sys_Manager>("NICK_NAME,USER_NAME", "SYS_MANAGER");
                    var viewList = new List<MesQualityInfoListModel>();
                    list?.ForEach(x =>
                    {
                        var organizeData = _sysOrganizeRepository.GetOrganize(Convert.ToInt32(x.ORGANIZE_ID));
                        var item = _mapper.Map<MesQualityInfoListModel>(x);
                        
                        var line = lines?.FirstOrDefault(f => f.LINE_ID == x.LINE_ID);
                        if (line != null)
                            item.LINE_NAME = line.LINE_NAME;

                        if (!String.IsNullOrEmpty(item.CREATE_USER)) 
                            item.CREATE_USER=userNameList?.FirstOrDefault(c=>item.CREATE_USER.Equals(c.USER_NAME))?.NICK_NAME;

                        if (!String.IsNullOrEmpty(item.AUDIT_USER)) 
                            item.AUDIT_USER = userNameList?.FirstOrDefault(c => item.AUDIT_USER.Equals(c.USER_NAME))?.NICK_NAME;

                        if (!String.IsNullOrEmpty(item.UPDATE_USER))
                            item.UPDATE_USER = userNameList?.FirstOrDefault(c => item.UPDATE_USER.Equals(c.USER_NAME))?.NICK_NAME;

                        var dept = depts.SingleOrDefault(f => f.ID == x.DEPARTMENT);
                        if (dept != null)
                            item.DEPARTMENT_NAME = dept.CHINESE;

                        var checkType = checkTypes.SingleOrDefault(f => f.LOOKUP_CODE == x.CHECK_TYPE);
                        if (checkType != null)
                            item.CHECK_TYPE_NAME = checkType.MEANING;
                        item.ORGANIZE_NAME = organizeData?.ORGANIZE_NAME;
                        viewList.Add(item);
                    });

                    var result = new TableDataModel
                    {
                        //TODO：model如新增参数，则需在此方法也增加传入参数
                        count = await _repository.RecordCountAsync(conditions, model),
                        data = viewList,
                    };

                    returnVM.Result = result.data;
                    returnVM.TotalCount = result.count;
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
        /// 添加或修改的相关操作
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifyAsync([FromBody]MesQualityInfoAddOrModifyModel item)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //item.ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    if (item.ID == 0)
                    {
                        //item.CREATE_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                        item.CREATE_TIME = DateTime.Now;
                    }

                    MesQualityInfo model;
                    if (item.ID == 0)
                    {
                        //TODO ADD
                        model = _mapper.Map<MesQualityInfo>(item);
                        model.UPDATE_TIME = DateTime.Now;
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
                        model = _repository.Get(item.ID);
                        //model.UPDATE_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                        if (model != null)
                        {
                            model.CHECK_TYPE = item.CHECK_TYPE;
                            model.LINE_ID = item.LINE_ID;
                            model.DEPARTMENT = item.DEPARTMENT;
                            model.PART_NO = item.PART_NO;
                            model.PART_NAME = item.PART_NAME;
                            model.PART_DESC = item.PART_DESC;
                            model.PRODUCT_DATE = item.PRODUCT_DATE;
                            model.BATCH_NO = item.BATCH_NO;
                            model.BATCH_QTY = item.BATCH_QTY;
                            model.UPDATE_USER = item.UPDATE_USER;
                            model.UPDATE_TIME = DateTime.Now;
                            model.PCB_SIDE = item.PCB_SIDE;
                            model.WORK_CLASS = item.WORK_CLASS;
                            model.WORK_SHIFTS = item.WORK_SHIFTS;
                            model.FIRST_ITEM_TYPE = item.FIRST_ITEM_TYPE;
                            model.EP_STATUS = item.EP_STATUS;
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

                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion
            }
            return returnVM;
		}

		/// <summary>
		/// 添加或修改明细视图
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        [Authorize("Permission")]
		[AllowAnonymous]
		public async Task<ApiBaseReturn<DetailListModel>> AddOrModifyDetail(decimal mstId)
		{
            ApiBaseReturn<DetailListModel> returnVM = new ApiBaseReturn<DetailListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    MesQualityInfo mesQualityInfo = _repository.Get(mstId);
                    var detailData = await _repository.GetDetailData(mstId);
                    var hasdetail = detailData.Count() > 0 ? 1 : 0;
                    decimal bill_type = mesQualityInfo?.CHECK_TYPE ?? 0;
                    DetailListModel detail = new DetailListModel
                    {
                        CheckItems = GetItemsData("", bill_type, mstId),
                        MST_ID = mstId,
                        bill_type = bill_type,
                        has_detail = hasdetail,
                        RESULT_STATUS = mesQualityInfo.RESULT_STATUS,
                        RESULT_REMARK = mesQualityInfo.RESULT_REMARK ?? string.Empty
                         
                    };

                    returnVM.Result = detail;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion
            }
            //获取到检验项目信息

			return returnVM;
		}

        private List<CheckItemsListModel> GetItemsData(string organizeId, decimal bill_type, decimal mstId)
        {
            List<CheckItemsListModel> CheckItemsList = new List<CheckItemsListModel>();
            String sQuery = @"SELECT I.ID AS ITEM_ID,I.CHECK_ITEM,I.CHECK_DESC,I.QUANTIZE_TYPE,I.ISEMPTY FROM MES_QUALITY_ITEMS I LEFT JOIN MES_QUALITY_INFO QI ON I.CHECK_TYPE = QI.CHECK_TYPE WHERE I.ENABLED = 'Y' AND I.CHECK_TYPE =:CHECK_TYPE AND QI.ID = :MST_ID";
            CheckItemsList = _repository.QueryEx<CheckItemsListModel>(sQuery, new { ORGANIZE_ID = organizeId, CHECK_TYPE = bill_type, MST_ID = mstId });
            if (CheckItemsList != null && CheckItemsList.Count() > 0)
            {
                List<MesQualityDetail> detailList = _repository.QueryEx<MesQualityDetail>("SELECT * FROM MES_QUALITY_DETAIL WHERE MST_ID = :MST_ID", new { MST_ID = mstId });
                foreach (var item in CheckItemsList)
                {
                    MesQualityDetail detail = detailList.Where(m => m.ITEM_ID == item.ITEM_ID)?.FirstOrDefault();
                    if (detail != null)
                    {
                        item.ID = detail.ID;
                        item.RESULT_VALUE = detail.RESULT_VALUE;
                        item.RESULT_TYPE = detail.RESULT_TYPE;
                    }
                }
            }
            return CheckItemsList;
        }

        /// <summary>
		/// 获取BOM信息
        /// COMPONENT_LOCATION（位号）
        /// PART_CODE(物料编号)
        /// PART_NAME(物料名称)
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        [Authorize("Permission")]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<RecordDetailListModel>> AddOrModifyBomDetail(decimal mstId)
        {
            ApiBaseReturn<RecordDetailListModel> returnVM = new ApiBaseReturn<RecordDetailListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    MesQualityInfo mesQualityInfo = _repository.Get(mstId);
                    var detailData = await _repository.GetMesFirstCheckDetailData(mstId);
                    var hasdetail = detailData.Count() > 0 ? 1 : 0;
                    decimal bill_type = mesQualityInfo?.CHECK_TYPE ?? 0;
                    RecordDetailListModel detail = new RecordDetailListModel
                    {
                        CheckItems = GetBomDetailData(mesQualityInfo.PART_NO, mesQualityInfo.ID),
                        AlternateItems = GetBomAlternateItems(mesQualityInfo.PART_NO, mesQualityInfo.ID),
                        MST_ID = mstId,
                        bill_type = bill_type,
                        has_detail = hasdetail,
                        RESULT_STATUS = mesQualityInfo.RESULT_STATUS,
                        RESULT_REMARK = mesQualityInfo.RESULT_REMARK ?? string.Empty
                    };

                    returnVM.Result = detail;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion
            }
            //获取到检验项目信息

            return returnVM;
        }

        private List<CheckBomDetailListModel> GetBomDetailData(String part_no, decimal mstId)
        {
            List<CheckBomDetailListModel> bomDetailList = new List<CheckBomDetailListModel>();
            String sQuery = @"SELECT B2.COMPONENT_LOCATION,B2.PART_CODE,B2.PART_NAME,B2.BOM_D_ID ID,B2.UNIT_QTY,B2.PART_MODEL FROM SMT_BOM1 B1,SMT_BOM2 B2 WHERE B1.PARTENT_CODE =:PARTENT_CODE AND B1.BOM_ID=B2.BOM_ID AND B2.COMPONENT_LOCATION IS NOT NULL ORDER BY B2.PART_NAME,B2.PART_CODE";
            bomDetailList = _repository.QueryEx<CheckBomDetailListModel>(sQuery, new { PARTENT_CODE = part_no });
            if (bomDetailList != null && bomDetailList.Count() > 0)
            {
                List<MesFirstCheckRecordDetail> detailList = _repository.QueryEx<MesFirstCheckRecordDetail>("SELECT * FROM MES_FIRST_CHECK_RECORD_DETAIL MFRD WHERE  MFRD.HID=TO_CHAR( :MST_ID)", new { MST_ID = mstId });
                foreach (var item in bomDetailList)
                {
                    MesFirstCheckRecordDetail detail = detailList.Where(m => m.PART_NO == item.PART_CODE)?.FirstOrDefault();
                    if (detail != null)
                    {
                        item.TENSION_VALUE = detail.TENSION_VALUE;
                        item.TEST_VALUE = detail.TEST_VALUE;
                        String brand = detail.BRAND_NAME;
                        if (String.IsNullOrEmpty(brand))
                        {
                            brand = _repository.QueryEx<String>("SELECT ATTRIBUTE2 FROM SFCS_PN WHERE PART_NO = :PART_NO", new { PART_NO = detail.PART_NO })?.FirstOrDefault();
                        }
                        item.BRAND_NAME = brand;
                        item.VENDOR_NAME = detail.VENDOR_NAME;
                        item.RESULT = detail.RESULT;
                    }
                }
            }
            return bomDetailList;
        }

        private List<CheckBomDetailListModel> GetBomAlternateItems(String part_no, decimal mstId)
        {
            List<CheckBomDetailListModel> bomDetailList = new List<CheckBomDetailListModel>();
            String sQuery = @"SELECT RP.REPLACE_PN PART_CODE,RP.COMPONENT_PN PARENT_PART_NO,BOM.PART_NAME,BOM.UNIT_QTY,BOM.PART_MODEL FROM SMT_REPLACE_PN RP LEFT JOIN SMT_BOM2 BOM ON RP.REPLACE_PN = BOM.PART_CODE WHERE RP.ENABLED = 'Y' AND BOM.COMPONENT_LOCATION IS NOT NULL AND RP.COMPONENT_PN IN (SELECT B2.PART_CODE FROM SMT_BOM1 B1,SMT_BOM2 B2 WHERE B1.PARTENT_CODE =:PARTENT_CODE AND B1.BOM_ID=B2.BOM_ID AND B2.COMPONENT_LOCATION IS NOT NULL)";
            sQuery = @"SELECT RP.REPLACE_PN PART_CODE,
        RP.COMPONENT_PN PARENT_PART_NO,
        BOM2.PART_NAME,
        BOM2.UNIT_QTY,
        BOM2.PART_MODEL,
        BOM2.BOM_ID
FROM 
    (SELECT REPLACE_PN,
        COMPONENT_PN
    FROM SMT_REPLACE_PN
    WHERE COMPONENT_PN IN 
        (SELECT B2.PART_CODE
        FROM SMT_BOM1 B1,SMT_BOM2 B2
        WHERE B1.PARTENT_CODE = :PARTENT_CODE
                AND B1.BOM_ID=B2.BOM_ID
                AND B2.COMPONENT_LOCATION IS NOT NULL)
                AND ENABLED = 'Y'
        GROUP BY  REPLACE_PN,COMPONENT_PN
        ORDER BY  REPLACE_PN ) RP
    LEFT JOIN SMT_BOM2 BOM2
    ON RP.COMPONENT_PN = BOM2.PART_CODE
WHERE BOM2.COMPONENT_LOCATION IS NOT NULL
        AND BOM2.BOM_ID = 
    (SELECT BOM_ID
    FROM SMT_BOM1
    WHERE PARTENT_CODE = :PARTENT_CODE )
ORDER BY  RP.REPLACE_PN";
            bomDetailList = _repository.QueryEx<CheckBomDetailListModel>(sQuery, new { PARTENT_CODE = part_no });
            if (bomDetailList != null && bomDetailList.Count() > 0)
            {
                List<MesFirstCheckRecordDetail> detailList = _repository.QueryEx<MesFirstCheckRecordDetail>("SELECT * FROM MES_FIRST_CHECK_RECORD_DETAIL MFRD WHERE MFRD.HID=TO_CHAR( :MST_ID) ", new { MST_ID = mstId });
                foreach (var item in bomDetailList)
                {
                    MesFirstCheckRecordDetail detail = detailList.Where(m => m.PART_NO == item.PART_CODE)?.FirstOrDefault();//本身
                    //MesFirstCheckRecordDetail parentDetail = detailList.Where(m => m.PART_NO == item.PARENT_PART_NO)?.FirstOrDefault();//父
                    if (detail != null)
                    {
                        item.TENSION_VALUE = detail.TENSION_VALUE;
                        item.TEST_VALUE = detail.TEST_VALUE;
                        String brand = detail.BRAND_NAME;
                        if (String.IsNullOrEmpty(brand))
                        {
                            brand = _repository.QueryEx<String>("SELECT ATTRIBUTE2 FROM SFCS_PN WHERE PART_NO = :PART_NO", new { PART_NO = detail.PART_NO })?.FirstOrDefault();
                        }
                        item.BRAND_NAME = brand;
                        item.VENDOR_NAME = detail.VENDOR_NAME;
                        item.RESULT = detail.RESULT;
                        //item.COMPONENT_LOCATION = parentDetail == null ? "" : parentDetail.POSITION;
                    }
                }
            }
            return bomDetailList;
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
                        ErrorInfo.Set(ResultCodeAddMsgKeys.CommonFailNoDataMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else
                    {
                        var count = await _repository.DeleteAsync(Id);
                        if (count > 0)
                        {
                            //成功
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
                }
                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion
            }
            //获取到检验项目信息

            return returnVM;
        }

        /// <summary>
        ///  添加或修改的相关操作（明细）
        /// </summary>
        /// <param name="ItemList">请求体中的数据的映射</param>
        /// <param name="MST_ID">线体ID</param>
        /// <param name="RESULT_STATUS">检验结果</param>
        /// <param name="REMARK">备注</param>
        /// <returns></returns>
        [HttpPost]
		[Authorize]
		public async Task<ApiBaseReturn<bool>> AddOrModifyDetailSave(PraData pra)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    BaseResult result = new BaseResult();
                    result = await _repository.AddOrModifyDetailSave(pra.ItemList, pra.RESULT_STATUS, pra.REMARK, pra.MST_ID);
                    if (result.ResultCode == 0)
                    {
                        returnVM.Result = true;
                    }
                    else {
                        returnVM.Result = false;
                        ErrorInfo.Set(result.ResultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion
            }
            return returnVM;

        }

        /// <summary>
        ///  添加或修改物料确认的相关操作（明细）
        /// </summary>
        /// <param name="ItemList">请求体中的数据的映射  检验结果值为Y或者N</param>
        /// <param name="MST_ID">线体ID</param>
        /// <param name="RESULT_STATUS">检验结果</param>
        /// <param name="REMARK">备注</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> AddOrModifyBomDetailSave(PraBomData pra)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    BaseResult result = new BaseResult();
                    result = await _repository.AddOrModifyBOMDetailSave(pra.ItemList, pra.RESULT_STATUS, pra.REMARK, pra.MST_ID);
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

                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion
            }
            return returnVM;

        }

        /// <summary>
        /// 获取当前生产信息
        /// </summary>
        /// <param name="lineId">产线ID</param>
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
                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion
            }
            return returnVM;
		}

		/// <summary>
		/// 获取检验明细数据
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
        [HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<List<MesQualityCheckDetailListModel>>> GetDetailData(decimal mstId)
		{
            ApiBaseReturn<List<MesQualityCheckDetailListModel>> returnVM = new ApiBaseReturn<List<MesQualityCheckDetailListModel>>();
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
                #region 如果出现错误，则写错误日志并返回错误内容

                WriteLog(ref returnVM);

                #endregion
            }
            return returnVM;
		}

        /// <summary>
        /// 获取检验明细项目信息
        /// </summary>
        /// <param name="mst_id">mst_id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
		public async Task<ApiBaseReturn<List<dynamic>>> GetDetailItemData(decimal mst_id)
		{
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var result = (await _repository.GetDetailItemData(mst_id)).ToList();
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

                #region 如果出现错误，则写错误日志并返回错误内容
                WriteLog(ref returnVM);
                #endregion
            }
            return returnVM;
		}

        /// <summary>
        /// 获取物料确认明细项目信息
        /// </summary>
        /// <returns>{"POSITION":"位号","PART_NO":"成品料号","RESULT":"检查结果","TENSION_VALUE":"拉力值","TEST_VALUE":"测试值","BRAND_NAME":"品牌","VENDOR_NAME":"供应商","PART_NAME":"物料名称","PART_DESC":"物料规格"}</returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetDetailBOMData(decimal mst_id, int type = 0)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    String parent_part_no_sql = type == 0 ? " AND (PARENT_PART_NO IS NULL OR PARENT_PART_NO = '') " : " AND PARENT_PART_NO IS NOT NULL ";
                    var result = (await _repository.GetDetailBOMData(mst_id, parent_part_no_sql)).ToList();
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

                #region 如果出现错误，则写错误日志并返回错误内容
                WriteLog(ref returnVM);
                #endregion
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
		public async Task<ApiBaseReturn<bool>> AuditData([FromBody]MesQualityInfoAddOrModifyModel item)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    object userName = new object();
                    var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var jwtHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(tokenHeader);
                    jwtToken.Payload.TryGetValue(ClaimTypes.NameIdentifier, out userName);
                    item.AUDIT_USER = userName.ToString();
                    var result = await _repository.AuditData(item);
                    if (result.ResultCode == 0)
                    {
                        returnVM.Result = true;
                    }
                    else
                    {
                        returnVM.Result = false;
                        ErrorInfo.Set(result.ResultMsg,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                #region 如果出现错误，则写错误日志并返回错误内容
                WriteLog(ref returnVM);
                #endregion
            }

            return returnVM;
		}

    }

    #region MyRegion
    public class DataList
    {
        /// <summary>
        /// 类型
        /// </summary>
        public List<SfcsParameters> SfcsParameters { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public List<SfcsDepartment> SfcsDepartment { get; set; }
        /// <summary>
        /// 产线
        /// </summary>
        public List<AllLinesModel> AllLinesModel { get; set; }
        /// <summary>
        /// 板型
        /// </summary>
        public List<SfcsParameters> SmtLookupList { get; set; }

        /// <summary>
        /// 首件类型
        /// </summary>
        public List<SfcsParameters> FirstItemTypeList { get; set; }

        /// <summary>
        /// 环保状态
        /// </summary>
        public List<SfcsParameters> FirstEPStatusList { get; set; }
    }

    public class Detail
    {
        public Decimal bill_type { get; set; }
        public Decimal MST_ID { get; set; }
        public Decimal has_detail { get; set; }
        public Decimal RESULT_STATUS { get; set; }
        public string RESULT_REMARK { get; set; }
        public List<dynamic> CheckItems { get; set; }
    }

    /// <summary>
    /// 检验项
    /// </summary>
    public class PraData
    {
        public List<MesQualityDetailAddOrModifyModel> ItemList { get; set; }
        public decimal MST_ID { get; set; }
        public decimal RESULT_STATUS { get; set; }
        public string REMARK { get; set; }
    }

    /// <summary>
    /// 物料确认
    /// </summary>
    public class PraBomData
    {
        /// <summary>
        /// 物料信息集合
        /// </summary>
        public List<MesFirstCheckRecordDetailAddOrModifyModel> ItemList { get; set; }
        public decimal MST_ID { get; set; }
        public decimal RESULT_STATUS { get; set; }
        public string REMARK { get; set; }
    }

    public class RecordDetailListModel
    {
        public Decimal bill_type { get; set; }
        public Decimal MST_ID { get; set; }
        public Decimal has_detail { get; set; }
        public Decimal RESULT_STATUS { get; set; }
        public string RESULT_REMARK { get; set; }
        public List<CheckBomDetailListModel> CheckItems { get; set; }

        /// <summary>
        /// 代替料集合
        /// </summary>
        public List<CheckBomDetailListModel> AlternateItems { get; set; }
    }

    public class CheckBomDetailListModel
    {
        public String PART_CODE { get; set; } = "";
        public String PARENT_PART_NO { get; set; } = "";
        public String COMPONENT_LOCATION { get; set; } = "";
        public String PART_NAME { get; set; } = "";
        public String ID { get; set; } = "";
        public Decimal UNIT_QTY { get; set; } = 0;
        public String PART_MODEL { get; set; } = "";
        public String TENSION_VALUE { get; set; } = "";
        public String TEST_VALUE { get; set; } = "";
        public String BRAND_NAME { get; set; } = "";
        public String VENDOR_NAME { get; set; } = "";
        public String RESULT { get; set; } = "";
    }

    #endregion
}