/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-02 10:58:38                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsProductCartonController                                      
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
    /// 产品卡通维护 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsProductCartonController : BaseController
    {
        private readonly ISfcsProductCartonRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ShareResourceController> _localizer;

        public SfcsProductCartonController(ISfcsProductCartonRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<ShareResourceController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 
        /// </summary>
        public class IndexVM
        {
            /// <summary>
            /// 采集工序
            /// </summary>
            public List<dynamic> Operation { get; set; }

            /// <summary>
            /// 附件名称
            /// </summary>
            public List<dynamic> Attachment { get; set; }
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
                        returnVM.Result = new IndexVM()
                        {
                            Operation = await _repository.GetListByTable("ID, OPERATION_NAME, DESCRIPTION", "SFCS_OPERATIONS", "AND OPERATION_CLASS =1 AND ID != 100 and ID != 999 order by DESCRIPTION desc "),
                            Attachment = await _repository.GetListByTable("ID, OBJECT_NAME, DESCRIPTION", "SFCS_ALL_OBJECTS", "AND OBJECT_CATEGORY =3 AND ISACTIVE = 'Y'"),
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsProductCartonListModel>>> LoadData([FromQuery]SfcsProductCartonRequestModel model)
        {
            ApiBaseReturn<List<SfcsProductCartonListModel>> returnVM = new ApiBaseReturn<List<SfcsProductCartonListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.COLLECT_OPERATION_ID != null && model.COLLECT_OPERATION_ID > 0)
                    {
                        conditions += $"and COLLECT_OPERATION_ID =:COLLECT_OPERATION_ID ";
                    }
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(PART_NO, :PART_NO) > 0  ";
                    }
                    if (!model.FORMAT.IsNullOrWhiteSpace())
                    {
                        conditions += $"and  instr(FORMAT, :FORMAT) > 0 ";
                    }
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(PART_NO, :Key) > 0 or instr(FORMAT, :Key) > 0) ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsProductCartonListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsProductCartonListModel>(x);
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
        /// 查询附件列表
        /// </summary>
        /// <param name="mst_id">主表ID</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsProductAttachmentsListModel>>> GetAttachmentList(decimal mst_id)
        {
            ApiBaseReturn<List<SfcsProductAttachmentsListModel>> returnVM = new ApiBaseReturn<List<SfcsProductAttachmentsListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (!ErrorInfo.Status && mst_id <= 0)
                    {
                        ErrorInfo.Set(_localizer["master_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE PRODUCT_OBJECT_ID =:mst_id ";

                    var list = (await _repository.GetListAsyncEx<SfcsProductAttachments>(conditions, new { mst_id }))?.ToList();
                    var viewList = new List<SfcsProductAttachmentsListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsProductAttachmentsListModel>(x);
                        viewList.Add(item);
                    });

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
        /// 料号是否存在 
        /// </summary>
        /// <param name="part_no">料号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> PartNOIsExist(string part_no)
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
                        if (!part_no.IsNullOrWhiteSpace())
                        {
                            result = await _repository.ItemIsExist("SFCS_PN", "PART_NO", part_no);
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsProductCartonModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.InsertRecords?.Count > 0)
                    {
                        if (model.InsertRecords.Where(t => t.PART_NO.IsNullOrWhiteSpace()).Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["part_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (model.InsertRecords.Where(t => (t.COLLECT_OPERATION_ID ?? 0) <= 0).Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["collect_operation_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && model.UpdateRecords?.Count > 0)
                    {
                        if (model.UpdateRecords.Where(t => t.PART_NO.IsNullOrWhiteSpace()).Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["part_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (model.UpdateRecords.Where(t => (t.COLLECT_OPERATION_ID ?? 0) <= 0).Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["collect_operation_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
                    string msg = ex.Message;
                    if (!msg.IsNullOrWhiteSpace() && msg.IndexOf("SFCS_PRODUCT_CARTON_UINX1") != -1)
                    {
                        ErrorInfo.Set(_localizer["part_no_repeat_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else
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
        /// 添加或修改附件视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> AddOrModifyAttachment()
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
        /// 保存附件数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveAttachmentData([FromBody] SfcsProductAttachmentsModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.InsertRecords?.Count > 0)
                    {
                        if (model.InsertRecords.Where(t => t.PRODUCT_OBJECT_ID <= 0).Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["master_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (model.InsertRecords.Where(t => t.ATTACHMENT_ID <= 0).Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["attachment_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && model.UpdateRecords?.Count > 0)
                    {
                        if (model.UpdateRecords.Where(t => t.PRODUCT_OBJECT_ID <= 0).Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["master_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (model.UpdateRecords.Where(t => t.ATTACHMENT_ID <= 0).Count() > 0)
                        {
                            ErrorInfo.Set(_localizer["attachment_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveAttachmentData(model);
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

    }
}