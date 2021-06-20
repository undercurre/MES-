/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：看板-每小时产能记录表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-25 16:55:23                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IMesKanbanHourYidldController                                      
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
using AutoMapper;
using JZ.IMS.IRepository;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using System.Reflection;

namespace JZ.IMS.Admin.Controllers
{
    /// <summary>
    /// 首小时产量和人均产能报表控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesKanbanHourYidldController : BaseController
	{
        private readonly IMesKanbanHourYidldRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISfcsOperationLinesRepository _lineRepository;
        public MesKanbanHourYidldController(IMesKanbanHourYidldRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ISfcsOperationLinesRepository lineRepository)
		{
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _lineRepository = lineRepository;
        }

        /// <summary>
        /// 查询所有首小时产量数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        //[Authorize]
		public async Task<ApiBaseReturn<List<MesKanbanHourYidldListModel>>> LoadData([FromQuery]MesKanbanHourYidldRequestModel model)
		{
            ApiBaseReturn<List<MesKanbanHourYidldListModel>> returnVM = new ApiBaseReturn<List<MesKanbanHourYidldListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    string conditions = " 1 = 1";//"where Is_Delete='N' ";//未删除的

                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0 or instr(Mobile, :Key) > 0 )";
                    }
                    //工作时间
                    var selectedDates = new List<DateTime?>();
                    if (model.BEGIN_TIME.Year == 1 && model.END_TIME.Year == 1)
                    {
                        conditions += " AND SUBSTR(TO_CHAR(A.WORK_TIME,'yyyy/MM/dd HH:mi:ss'),12,8) = '08:00:00'";
                    }

                    if (model.BEGIN_TIME.Year > 1 && model.END_TIME.Year > 1)
                    {
                        for (var date = model.BEGIN_TIME; date <= model.END_TIME; date = date.AddDays(1))
                        {
                            selectedDates.Add(date);
                        }
                    }

                    if (selectedDates.Count > 0)
                    {
                        string tion = string.Empty;
                        conditions += " AND (";
                        for (int i = 0; i < selectedDates.Count; i++)
                        {
                            if (i < selectedDates.Count - 1)
                            {
                                tion += "TO_CHAR(A.WORK_TIME,'yyyy/mm/dd hh24:mi:ss') = '" + selectedDates[i].Value.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " 08:00:00' or ";
                                continue;
                            }
                            if (i < selectedDates.Count)
                            {
                                tion += "TO_CHAR(A.WORK_TIME,'yyyy/mm/dd hh24:mi:ss') = '" + selectedDates[i].Value.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " 08:00:00'";
                            }
                        }
                        conditions += tion + ")";
                    }

                    //线别
                    if (model.LINE_ID > 0)
                    {
                        conditions += " AND A.LINE_ID = :LINE_ID";
                    }
                    //工单
                    if (!model.WO_NO.IsNullOrWhiteSpace())
                    {
                        conditions += " AND A.WO_NO = :WO_NO";
                    }
                    //料号
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += " AND PART_NO = :PART_NO";
                    }
                    //报告内容
                    if (!model.REPORT_CONTENT.IsNullOrWhiteSpace())
                    {
                        conditions += " AND E.REPORT_CONTENT =:REPORT_CONTENT";
                    }

                    var list = (await _repository.GetDataPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesKanbanHourYidldListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesKanbanHourYidldListModel>(x);
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
                    ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;

        }


        /// <summary>
        /// 查询所有人均产值数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesKanbanHourYidldListModel>>> LoadDataReport([FromQuery]MesKanbanHourYidldRequestModel model)
        {
            ApiBaseReturn<List<MesKanbanHourYidldListModel>> reutrnVM = new ApiBaseReturn<List<MesKanbanHourYidldListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    model.BEGIN_TIME = Convert.ToDateTime(model.BEGIN_TIME.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                    model.END_TIME = Convert.ToDateTime(model.END_TIME.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                    string conditions = " WHERE 1 = 1 AND LINE_TYPE= 'PCBA'";//"where Is_Delete='N' ";//未删除的

                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0 or instr(Mobile, :Key) > 0 )";
                    }
                    //线别
                    if (model.LINE_ID > 0)
                    {
                        conditions += " AND LINE_ID = :LINE_ID";
                    }
                    //工单
                    if (!model.WO_NO.IsNullOrWhiteSpace())
                    {
                        conditions += " AND WO_NO LIKE  '%' ||:WO_NO || '%'";
                    }
                    //料号
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += " AND PART_NO LIKE  '%' ||:PART_NO || '%'";
                    }
                    if (model.BEGIN_TIME.Year > 1)
                    {
                        conditions += " AND TO_CHAR(WORK_TIME,'yyyy/MM/dd') BETWEEN '" + model.BEGIN_TIME.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "' AND '" + model.END_TIME.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "'";
                    }
                    var list = (await _repository.GetReportListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var priceList = _repository.GetPriceListAsync(model.Page, model.Limit, conditions, model);
                    var viewList = new List<MesKanbanHourYidldListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesKanbanHourYidldListModel>(x);
                        var price = priceList.SingleOrDefault(m => m.Code == item.PART_NO);
                        if (price != null)
                        {
                            item.PRICE = price.Price.ToString();
                            item.TOTAL_PRICE = (item.OUTPUT_QTY * price.Price).ToString();
                            item.AVERAGE_PRICE = ((item.OUTPUT_QTY * price.Price) / Convert.ToDecimal(item.REPORT_NUM == "0" ? "1" : item.REPORT_NUM)).ToString();
                        }
                        else
                        {
                            item.PRICE = "0";
                            item.TOTAL_PRICE = "0";
                            item.AVERAGE_PRICE = "0";
                        }
                        viewList.Add(item);
                    });

                    var data = new TableDataModel
                    {
                        //TODO：model如新增参数，则需在此方法也增加传入参数
                        count = await _repository.RecordReportCountAsync(conditions, model),
                        data = viewList,
                    };

                    reutrnVM.Result = data.data;
                    reutrnVM.TotalCount = data.count;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
                }

                WriteLog(ref reutrnVM);
            }
            
            return reutrnVM;
        }
	}
}