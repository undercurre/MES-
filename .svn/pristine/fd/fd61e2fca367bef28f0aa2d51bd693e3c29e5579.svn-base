/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：不良报工表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-26 14:37:35                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISfcsDefectReportWorkController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.IRepository;
using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using System;
using System.Reflection;

namespace JZ.IMS.Admin.Controllers
{
    /// <summary>
    /// 不良统计报表控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsDefectReportWorkController : BaseController
	{
        private readonly ISfcsDefectReportWorkRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISfcsOperationLinesRepository _lineRepository;

        public SfcsDefectReportWorkController(ISfcsDefectReportWorkRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,ISfcsOperationLinesRepository lineRepository)
		{
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _lineRepository = lineRepository;
        }

        /// <summary>
        /// 获取产线
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<List<AllLinesModel>> GetLinesList()
		{
            ApiBaseReturn<List<AllLinesModel>> returnVM = new ApiBaseReturn<List<AllLinesModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    returnVM.Result = _lineRepository.GetLinesList(orginizeId, "PCBA");
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
		/// 查询所有数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[HttpGet]
        [Authorize]
		public async Task<ApiBaseReturn<List<SfcsDefectReportWorkListModel>>> LoadData([FromQuery]SfcsDefectReportWorkRequestModel model)
		{
            ApiBaseReturn<List<SfcsDefectReportWorkListModel>> returnVM = new ApiBaseReturn<List<SfcsDefectReportWorkListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var orginizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    string conditions = " 1 = 1";
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0 or instr(Mobile, :Key) > 0 )";
                    }
                    //工单
                    if (!model.WO_NO.IsNullOrWhiteSpace())
                    {
                        conditions += " AND WO_NO = :WO_NO";
                    }
                    //线别
                    if (model.LINE_ID > 0)
                    {
                        conditions += " AND T.LINE_ID = :LINE_ID";
                    }
                    //组合条件
                    if (!model.COM_CONDITION.IsNullOrWhiteSpace())
                    {
                        conditions += " AND (T.DEFECT_CODE = :COM_CONDITION OR T.LOC = :COM_CONDITION OR C.DEFECT_DESCRIPTION = :COM_CONDITION)";
                    }
                    var list = (await _repository.GetDataPagedAsync(model.Page, model.Limit, conditions, "ORDER BY WO_NO,QTY desc", model)).ToList();
                    var viewList = new List<SfcsDefectReportWorkListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsDefectReportWorkListModel>(x);
                        //item.Role_Name = _roleRepository.GetNameById(x.ROLE_ID);
                        //item.ENABLED = (item.ENABLED == "Y");
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
	}
}