/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：批次管理表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-08-17 15:48:19                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesBatchManagerController                                      
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
    /// 批次管理 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesBatchManagerController : BaseController
    {
        private readonly IMesBatchManagerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MesBatchManagerController> _localizer;
        private readonly IAndonCallRuleConfigRepository _getlinelist;
        private readonly IMesBatchResourcesRepository _mesBatchResources;
        private readonly IMesBatchPringRepository _mesBatchPring;


        public MesBatchManagerController(IMesBatchManagerRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MesBatchManagerController> localizer, IAndonCallRuleConfigRepository getlinelist, IMesBatchResourcesRepository mesBatchResources, IMesBatchPringRepository mesBatchPring)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _getlinelist = getlinelist;
            _mesBatchResources = mesBatchResources;
            _mesBatchPring = mesBatchPring;
        }

        public class IndexVM
        {
            /// <summary>
            /// 线体
            /// </summary>
            public List<dynamic> LINE_NAME { get; set; }

        }


        /// <summary>
        /// 首页视图（获取线体下拉框数据）
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
                            LINE_NAME = await _getlinelist.GetLINENAME(),
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
        /// 查询批次管理数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesBatchManagerListModel>>> LoadData([FromQuery] MesBatchManagerRequestModel model)
        {
            ApiBaseReturn<List<MesBatchManagerListModel>> returnVM = new ApiBaseReturn<List<MesBatchManagerListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.ID.IsNullOrWhiteSpace())
                    {
                        conditions += $"and ( ID=:ID)";
                    }
                    if (!model.LINE_ID.IsNullOrWhiteSpace())
                    {
                        conditions += $"and ( LINE_ID=:LINE_ID)";
                    }
                    if (!model.StartDate.IsNullOrWhiteSpace())
                    {
                        conditions += string.Format(@"and (PRODUCTION_TIME >= to_date('{0}','yyyy-MM-dd HH24:mi:ss'))", model.StartDate.ToString());
                    }
                    if (!model.EndDate.IsNullOrWhiteSpace())
                    {
                        conditions += string.Format(@"and (PRODUCTION_TIME < to_date('{0}','yyyy-MM-dd HH24:mi:ss'))", model.EndDate.ToString());
                    }
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(WO_NO, :Key) > 0 )";
                    }

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesBatchManagerListModel>();
                    list?.ForEach(async x =>
                    {
                        var config = new MapperConfiguration(cfg=>cfg.CreateMap<MesBatchManager,MesBatchManagerListModel>().ForMember(d=>d.OUTPUT_QTY,opt=>opt.Ignore()));
                        var item = config.CreateMapper().Map<MesBatchManagerListModel>(x);
                        //_mapper.Map;
                        dynamic productionModel = (await _repository.QueryAsyncEx<dynamic>("SELECT SUM(OUTPUT_QTY) OUTPUT_QTY FROM SMT_PRODUCTION WHERE LOC_NO=:LOC_NO", new
                        {
                            LOC_NO = item.LOC_NO
                        }))?.FirstOrDefault();
                        productionModel?.ToString();
                        item.OUTPUT_QTY = productionModel?.OUTPUT_QTY??0;//productionModel.values[0];
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
        /// 获取批次号数据(新增页面需要筛选批次号带出数据)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetMesBatchDataByLOCNO([FromQuery] MesBatchManagerRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetMesBatchDataByLOCNO(model);

                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 根据批次管理表ID查询附件表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesBatchResourcesListModel>>> GetMesBatchResourcesList([FromQuery] MesBatchResourcesRequestModel model)
        {
            ApiBaseReturn<List<MesBatchResourcesListModel>> returnVM = new ApiBaseReturn<List<MesBatchResourcesListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.BT_MANAGER_ID.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (BT_MANAGER_ID=:BT_MANAGER_ID)";
                    }
                    var list = (await _mesBatchResources.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesBatchResourcesListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesBatchResourcesListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _mesBatchResources.RecordCountAsync(conditions, model);

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
        /// 根据批次管理表ID查询周转打印表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesBatchPringListModel>>> GetMesBatchPrintList([FromQuery] MesBatchPringRequestModel model)
        {
            ApiBaseReturn<List<MesBatchPringListModel>> returnVM = new ApiBaseReturn<List<MesBatchPringListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.BT_MANAGER_ID.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (BT_MANAGER_ID=:BT_MANAGER_ID)";
                    }
                    if (!model.StartDate.IsNullOrWhiteSpace())
                    {
                        conditions += string.Format(@"and (CREATE_TIME >= to_date('{0}','yyyy-MM-dd HH24:mi:ss'))", model.StartDate.ToString());
                    }
                    if (!model.EndDate.IsNullOrWhiteSpace())
                    {
                        conditions += string.Format(@"and (CREATE_TIME < to_date('{0}','yyyy-MM-dd HH24:mi:ss'))", model.EndDate.ToString());
                    }
                    var list = (await _mesBatchPring.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesBatchPringListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesBatchPringListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _mesBatchPring.RecordCountAsync(conditions, model);

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
        ///  根据批次管理表批次号查询下方物料报表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetMesMaterialInfoList([FromQuery] MesMaterialInfoRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetMesMaterialInfoList(model);

                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        ///  根据批次管理表ID获取打印时自动带出的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetMesBatchPring(decimal id)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _repository.GetMesBatchPring(id);
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;


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
        /// 新增打印信息(点击打印时需记录打印相关信息)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> SaveMesBatchPring([FromBody] MesBatchPringModel model)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    foreach (MesBatchPringAddOrModifyModel mesBatchPringAddOrModifyModel in model.InsertRecords)
                    {
                        var exist = await _mesBatchPring.GetListAsyncEx<MesBatchPring>("WHERE CODE = :CODE", new
                        {
                            CODE = mesBatchPringAddOrModifyModel.CODE
                        });
                        if (exist != null && exist.Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["SaveMesBatchPring_data_exist"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            break;
                        }

                        //批次生产数量
                        var managerQty = await _mesBatchPring.ExecuteScalarAsync("SELECT NVL(PRODUCTION_QTY,0) QTY FROM MES_BATCH_MANAGER WHERE ID=:ID", new
                        {
                            ID = mesBatchPringAddOrModifyModel.BT_MANAGER_ID
                        });

                        //数量验证
                        var pringQty = await _mesBatchPring.ExecuteScalarAsync("SELECT SUM( NVL(QTY,0)) QTY FROM MES_BATCH_PRING WHERE BT_MANAGER_ID=:BT_MANAGER_ID", new
                        {
                            BT_MANAGER_ID = mesBatchPringAddOrModifyModel.BT_MANAGER_ID
                        });

                        if ((mesBatchPringAddOrModifyModel.QTY + pringQty) > managerQty)
                        {
                            //周转箱总数超过了批次产能数量，请注意检查!
                            ErrorInfo.Set(_localizer["SAVE_QTY_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            break;
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _mesBatchPring.SaveDataByTrans(model);
                        if (resdata != -1)
                        {
                            returnVM.Result = resdata;
                        }
                        else
                        {
                            ErrorInfo.Set(_localizer["SaveMesBatchPring_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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
        /// 获取标签打印上传文件表中周转箱条码最新的一条数据给前台(前台获取文件名生成一个txt文件)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> BackStageToPagePrintFile()
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _repository.GetPrintFilesToPage();
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;


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
        /// 保存数据(保存、修改批次管理信息，新增附件表数据)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] MesBatchManagerModel model)
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
        /// 根据批次号判断批次管理表中是否存在该批次号(新增时需要判断，不能添加重复批次号)
        /// </summary>
        /// <param name="locno">批次号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> JudgeLocNoIsExistByLocNo(string locno)
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
                        if (!locno.IsNullOrWhiteSpace())
                        {
                            result = await _repository.JudgeLocNoIsExistByLocNo(locno);
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
        /// 删除附件表信息
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteMesBatchResourcesById(decimal id)
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
                        var count = await _mesBatchResources.DeleteAsync(id);
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





        /*  删除和添加或修改视图功能暂时不需要，先注释
        

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



    }
}