/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-21 14:43:44                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtWoController                                      
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
    /// 工单管理控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SmtWoController : BaseController
	{
		private readonly ISmtWoRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SmtWoController> _localizer;
		
		public SmtWoController(ISmtWoRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SmtWoController> localizer)
		{
			_repository = repository;
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
        public async Task<ApiBaseReturn<List<SmtWoListModel>>> LoadData([FromQuery]SmtWoRequestModel model)
        {
            ApiBaseReturn<List<SmtWoListModel>> returnVM = new ApiBaseReturn<List<SmtWoListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.WO_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND (INSTR(WO_NO, :WO_NO)) > 0 ";
                    }
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND (INSTR(PART_NO, :PART_NO)) > 0 ";
                    }
                    if (!model.MODEL1.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND (INSTR(MODEL, :MODEL1)) > 0 ";
                    }
                    if (!model.DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND (INSTR(DESCRIPTION, :DESCRIPTION)) > 0 ";
                    }
                    if (!model.ATTRIBUTE1.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND (INSTR(ATTRIBUTE1, :ATTRIBUTE1)) > 0 ";
                    }
                    if (!model.ATTRIBUTE3.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND (INSTR(ATTRIBUTE3, :ATTRIBUTE3)) > 0 ";
                    }
                    if (!model.ATTRIBUTE4.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND (INSTR(ATTRIBUTE4, :ATTRIBUTE4)) > 0 ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SmtWoListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SmtWoListModel>(x);
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
        /// 根据工单号获取数据
        /// </summary>
        /// <param name="wo_no">工单号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SmtWoListModel>> GetSmtWoByNo(String wo_no)
        {
            ApiBaseReturn<SmtWoListModel> returnVM = new ApiBaseReturn<SmtWoListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!String.IsNullOrEmpty(wo_no))
                    {
                        returnVM.Result = (await _repository.GetListByTableEX<SmtWoListModel>("*", "SMT_WO", "AND WO_NO = :WO_NO ORDER BY ID DESC", new { WO_NO = wo_no }))?.FirstOrDefault();
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
        /// 获取SMT在生产的工单信息
        /// </summary>
        /// <param name="lineID"></param>
        /// <param name="stationID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetSMTWoInfo(string lineID, string stationID = "")
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!String.IsNullOrEmpty(lineID))
                    {
                        string condition = (stationID.IsNullOrEmpty() ? "" : " AND STATION_ID=:STATION_ID ");
                        string sQuery = $@" SELECT T.BATCH_NO,to_char(T.LINE_ID),T.WO_NO,T.PCB_PN,to_char(T.PCB_SIDE) PCB_SIDE,TO_CHAR(T.START_TIME,'yyyy-mm-dd hh24:mi:ss'),to_char(T.STATION_ID),to_char(NVL(SPH.STANDARD_CAPACITY,0)) STANDARD_CAPACITY,
                                to_char(ROUND((CASE WHEN NVL(SPH.STANDARD_CAPACITY,0) IN(0)  THEN 0 ELSE NVL(WO.TARGET_QTY,0)/NVL(SPH.STANDARD_CAPACITY,0) END),2)) WORKING_HOURS,
                                TO_CHAR((T.START_TIME+(ROUND((CASE WHEN NVL(SPH.STANDARD_CAPACITY,0) IN(0)  THEN 0 ELSE NVL(WO.TARGET_QTY,0)/NVL(SPH.STANDARD_CAPACITY,0) END),2)/24)),'yyyy-mm-dd hh24:mi:ss') FISHEDTIME
                                    FROM ( 
                                            SELECT PRO.BATCH_NO,PRO.LINE_ID,PRO.WO_NO,PRO.PCB_PN,PRO.PCB_SIDE,PRO.START_TIME,PRO.STATION_ID,PRO.PLACEMENT_MST_ID 
                                            FROM  SMT_PRODUCTION PRO WHERE FINISHED = 'N'  AND LINE_ID =:LINE_ID {condition} AND ROWNUM=1 
                                          ) T
                                LEFT JOIN SFCS_WO  WO ON WO.WO_NO=T.WO_NO
                                LEFT JOIN SMT_PLACEMENT_HEADER  SPH ON SPH.ID=T.PLACEMENT_MST_ID
                                ORDER BY T.START_TIME ASC ";

                        returnVM.Result = (await _repository.QueryAsyncEx<dynamic>(sQuery, new { LINE_ID = lineID, STATION_ID = stationID }))?.FirstOrDefault();
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
        /// 获取机台列表
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetStation(decimal lineId = 0)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (lineId > 0)
                    {
                        string sQuery = @"SELECT ID,SMT_NAME,DESCRIPTION FROM SMT_STATION WHERE ID IN(SELECT STATION_ID FROM SMT_ROUTE WHERE LINE_ID = :LINE_ID AND ENABLED = 'Y')";

                        returnVM.Result = (await _repository.QueryAsyncEx<dynamic>(sQuery, new { LINE_ID = lineId }))?.FirstOrDefault();
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