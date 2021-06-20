/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-05 09:34:53                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtStencilPartController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using JZ.IMS.ViewModels.SmtStencil;
using Dapper;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 钢网产品对照控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtStencilPartController : BaseController
    {
        private readonly ISmtStencilPartRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtStencilPartController> _localizer;

        public SmtStencilPartController(ISmtStencilPartRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtStencilPartController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 产品集
            /// </summary>
            /// <returns></returns>
            public List<CodeName> PartList { get; set; }

            /// <summary>
            /// 面板集
            /// </summary>
            /// <returns></returns>
            public List<CodeName> PCBSideList { get; set; }
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
                            PartList = await _repository.GetPartList(),
                            PCBSideList = await _repository.GetPCBSideList(),
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
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtStencilPartListModel>>> LoadData([FromQuery] SmtStencilPartRequestModel model)
        {
            ApiBaseReturn<List<SmtStencilPartListModel>> returnVM = new ApiBaseReturn<List<SmtStencilPartListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE 1=1 ";
                    if (!model.STENCIL_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(STENCIL_NO, :STENCIL_NO) > 0) ";
                    }

                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (PART_NO =:PART_NO) ";
                    }

                    if (!model.PCB_SIDE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (PCB_SIDE =:PCB_SIDE) ";
                    }

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "CREATE_TIME desc", model)).ToList();
                    var viewList = new List<SmtStencilPartListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SmtStencilPartListModel>(x);
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
        /// 产品编号搜索
        /// NAME:产品名称 
        /// CODE:产品编号 
        /// DESCRIPTION:产品规格
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> PartNoLoadData([FromQuery] IMSPARTRequest model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE 1=1 ";
                    if (!model.CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(CODE,:CODE) >0) ";
                    }

                    if (!model.NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(NAME ,:NAME) > 0 ) ";
                    }

                    if (!model.DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(DESCRIPTION ,:DESCRIPTION) > 0 ) ";
                    }

                    var list = (await _repository.GetListPagedEx<ImsPartEntity>(model.Page, model.Limit, conditions, " CODE  desc ,NAME desc ", model)).ToList();
                    var result = list.Select(c => new { c.ID, c.NAME, c.CODE, c.DESCRIPTION }).ToList<dynamic>();
                    count = await _repository.RecordCountAsyncEx<ImsPartEntity>(conditions, model);
                    returnVM.Result = result;
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
		/// 导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery] SmtStencilPartRequestModel model)
        {

            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值
                    var result = await _repository.GetExportData(model);
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
            return returnVM;
        }

        /// <summary>
        ///  获取产品规格
        /// </summary>
        /// <param name="PART_NO">产品编号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetPNModel(string PART_NO)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(PART_NO))
                    {
                        ErrorInfo.Set(_localizer["PART_NO_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 获取并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetPNModel(PART_NO);
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
        ///  获取钢网位号(查询传LOC值)
        /// </summary>
        /// <param name="LOC">位号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetStencilLOC(string LOC)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            returnVM.Result = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    #endregion

                    #region 获取并返回

                    if (!ErrorInfo.Status)
                    {
                        string condition = "";
                        if (!LOC.IsNullOrWhiteSpace())
                        {
                            condition += " AND instr(VALUE,:VALUE)>0 ";
                        }
                        returnVM.Result = (await _repository.GetListByTableEX<dynamic>(" CODE , VALUE ", "SMT_LOOKUP", " AND TYPE = 'STENCIL_LOC' AND ENABLED='Y' " + condition, new
                        {
                            VALUE = LOC
                        })).ToList();

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
        /// <param name="id">主键ID</param>
        /// <returns>响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(string id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status && id.IsNullOrWhiteSpace())
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtStencilPartModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.insertRecords != null && model.insertRecords.Where(t => string.IsNullOrWhiteSpace(t.STENCIL_NO)).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["STENCIL_NO_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.updateRecords != null && model.updateRecords.Where(t => string.IsNullOrWhiteSpace(t.STENCIL_NO)).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["STENCIL_NO_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.insertRecords != null && model.insertRecords.Where(t => string.IsNullOrWhiteSpace(t.PART_NO)).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["PART_NO_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.updateRecords != null && model.updateRecords.Where(t => string.IsNullOrWhiteSpace(t.PART_NO)).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["PART_NO_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.insertRecords != null && model.insertRecords.Where(t => string.IsNullOrWhiteSpace(t.PCB_SIDE)).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["PCB_SIDE_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.updateRecords != null && model.updateRecords.Where(t => string.IsNullOrWhiteSpace(t.PCB_SIDE)).Count() > 0)
                    {
                        ErrorInfo.Set(_localizer["PCB_SIDE_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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

                    if (ex.Message != null && ex.Message.IndexOf("SMT_STENCIL_PART_UINX2") != -1)
                    {
                        //钢网编号、产品编号同时已经存在，请注意检查 
                        ErrorInfo.Set(_localizer["EXIST_SAME_TIME2"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else if (ex.Message != null && ex.Message.IndexOf("SMT_STENCIL_PART_UINX1") != -1)
                    {
                        //钢网编号、产品编号和正(反面)同时已经存在，请注意检查 
                        ErrorInfo.Set(_localizer["EXIST_SAME_TIME"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }

                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
    }
}