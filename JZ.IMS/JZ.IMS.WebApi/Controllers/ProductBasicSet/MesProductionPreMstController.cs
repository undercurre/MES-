/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认主表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-25 09:05:16                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesProductionPreMstController                                      
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
    /// 产前确认管理 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesProductionPreMstController : BaseController
    {
        private readonly IMesProductionPreMstRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MenuController> _localizer;

        public MesProductionPreMstController(IMesProductionPreMstRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MenuController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 线体列表
            /// </summary>
            public List<IDNAME> LineList { get; set; }
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
        public async Task<ApiBaseReturn<List<MesProductionPreMstListModel>>> LoadData([FromQuery]MesProductionPreMstRequestModel model)
        {
            ApiBaseReturn<List<MesProductionPreMstListModel>> returnVM = new ApiBaseReturn<List<MesProductionPreMstListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if ((model.ID ?? 0) > 0)
                    {
                        conditions += $"and (ID =:ID) ";
                    }
                    if ((model.LINE_ID ?? 0) > 0)
                    {
                        conditions += $"and (LINE_ID =:LINE_ID) ";
                    }
                    if (!model.WO_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(WO_NO, :WO_NO) > 0) ";
                    }
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(PART_NO, :PART_NO) > 0) ";
                    }
                    if (!model.PART_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(PART_NAME, :PART_NAME) > 0) ";
                    }
                    if ((model.MODEL_ID ?? 0) > 0)
                    {
                        conditions += $"and (MODEL_ID =:MODEL_ID) ";
                    }
                    if ((model.CUSTOMER_ID ?? 0) > 0)
                    {
                        conditions += $"and (CUSTOMER_ID =:CUSTOMER_ID) ";
                    }
                    if (!model.BEGIN_TIME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (CREATIME >= to_date(:BEGIN_TIME,'yyyy-mm-dd')) ";
                    }
                    if (!model.END_TIME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (CREATIME <= to_date(:END_TIME,'yyyy-mm-dd')) ";  //'yyyy-mm-dd HH24:MI:SS' 
                    }

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesProductionPreMstListModel>();
                    var lineList = await _repository.GetLineList();
                    var custid = list.Select(t => t.CUSTOMER_ID).Distinct().ToList();
                    List<SfcsCustomers> customer = null;
                    if (custid.Count > 0)
                    {
                        customer = (await _repository.GetListAsyncEx<SfcsCustomers>("where ID in :ID", new { ID = custid.ToArray() }))?.ToList();
                    }
                    
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesProductionPreMstListModel>(x);

                        item.LINE_NAME = lineList.Where(t => t.ID == x.LINE_ID.ToString()).Select(t => t.NAME).FirstOrDefault();
                        item.CUSTOMER_NAME = customer?.Where(t => t.ID == x.CUSTOMER_ID).Select(t => t.CUSTOMER).FirstOrDefault()?? string.Empty;

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
        /// 获取工单信息
        /// </summary>
        /// <param name="wo_no">工单号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtWoV>>> GetWoInfo(string wo_no)
        {
            ApiBaseReturn<List<SmtWoV>> returnVM = new ApiBaseReturn<List<SmtWoV>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status && !wo_no.IsNullOrWhiteSpace())
                    {
                        returnVM.Result = (await _repository.GetListAsyncEx<SmtWoV>("Where WO_NO=:wo_no", new { wo_no }))?.ToList();
                        returnVM.TotalCount = returnVM.Result?.Count ?? 0;
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
        /// 获取明细信息
        /// </summary>
        /// <param name="id">主表id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesProductionPreDtlListModel>>> GetDetailInfo(decimal id)
        {
            ApiBaseReturn<List<MesProductionPreDtlListModel>> returnVM = new ApiBaseReturn<List<MesProductionPreDtlListModel>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status && id > 0)
                    {
                        returnVM.Result = await _repository.GetProductionPreDtlList(id);
                        returnVM.TotalCount = returnVM.Result?.Count ?? 0;
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] MesProductionPreMstModel model)
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
        /// 删除
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteBill(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && id <= 0)
                    {
                        ErrorInfo.Set(_localizer["id_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var mesProductionPreMst = await _repository.GetAsync(id);
                        if (mesProductionPreMst.END_STATUS == "Y")
                        {
                            ErrorInfo.Set(_localizer["status_not_can_delete"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _repository.DeleteByTrans(id);
                        if (resultData == 1)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(_localizer["delete_fail_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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