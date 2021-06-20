/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-06 14:36:26                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsProductResourcesController                                      
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
using JZ.IMS.ViewModels.ProductBasicSet.ComponentReplace;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 产品设定-产品资料维护
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsProductResourcesController : BaseController
    {
        private readonly ISfcsProductResourcesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsProductResourcesController> _localizer;

        public SfcsProductResourcesController(ISfcsProductResourcesRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsProductResourcesController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 资料名称
            /// </summary>
            public List<dynamic> SourceList { get; set; }
            /// <summary>
            /// 采集序号
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
                            SourceList = await _repository.GetListByTable("*", " SFCS_ALL_OBJECTS SAO ", " AND OBJECT_CATEGORY =5 AND ISACTIVE='Y' "),
                            Operation = await _repository.GetListByTable("ID, OPERATION_NAME, DESCRIPTION", "SFCS_OPERATIONS", "AND OPERATION_CLASS =1 AND ID != 100 and ID != 999 order by description  "),
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
        public async Task<ApiBaseReturn<List<SfcsProductResourcesListModel>>> LoadData([FromQuery] SfcsProductResourcesRequestModel model)
        {
            ApiBaseReturn<List<SfcsProductResourcesListModel>> returnVM = new ApiBaseReturn<List<SfcsProductResourcesListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"AND INSTR(PART_NO, :PART_NO) > 0 ";
                    }
                    if (!model.RESOURCE_ID.IsNullOrWhiteSpace() && model.RESOURCE_ID > 0)
                    {
                        conditions += $"AND RESOURCE_ID=:RESOURCE_ID ";
                    }
                    if (!model.COLLECT_OPERATION_ID.IsNullOrWhiteSpace() && model.COLLECT_OPERATION_ID > 0)
                    {
                        conditions += $"AND COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsProductResourcesListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsProductResourcesListModel>(x);
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
        /// 获取附件表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsProductAttachments>>> GetAattachments([FromQuery] SfcsProductAttachmentsRequestModel model)
        {
            ApiBaseReturn<List<SfcsProductAttachments>> returnVM = new ApiBaseReturn<List<SfcsProductAttachments>>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 设置返回值
                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!ErrorInfo.Status && !model.PRODUCT_OBJECT_ID.IsNullOrWhiteSpace())
                    {
                        conditions += $"AND PRODUCT_OBJECT_ID=:PRODUCT_OBJECT_ID";
                        var list = (await _repository.GetListPagedEx<SfcsProductAttachments>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                        var viewList = new List<SfcsProductAttachments>();
                        list?.ForEach(x =>
                        {
                            var item = _mapper.Map<SfcsProductAttachments>(x);
                            //item.ENABLED = (item.ENABLED == "Y");
                            viewList.Add(item);
                        });

                        count = await _repository.RecordCountAsyncEx<SfcsProductAttachments>(conditions, model);

                        returnVM.Result = viewList;
                        returnVM.TotalCount = count;
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
        /// 保存之前:资源名不能为空 采集工序不能为空
        /// 数字验证(资料维护数量 1-500, 附件维护数量 1-1000)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsProductResourcesModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status)
                    {

                        #region 产品资料维护
                        List<List<SfcsProductResourcesAddOrModifyModel>> productResources = null;
                        if (model.InsertRecords != null || model.UpdateRecords != null)
                        {
                            productResources = new List<List<SfcsProductResourcesAddOrModifyModel>>();
                        }
                        //插入
                        if (model.InsertRecords != null && !ErrorInfo.Status)
                        {
                            productResources.Add(model.InsertRecords);
                        }
                        //更新
                        if (model.UpdateRecords != null && !ErrorInfo.Status)
                        {
                            productResources.Add(model.UpdateRecords);
                        }

                        if (productResources != null)
                        {
                            foreach (var templist in productResources)
                            {
                                foreach (var item in templist)
                                {
                                    //if (item.DATA_FORMAT.Length == 0 && !ErrorInfo.Status)
                                    //{
                                    //    //格式不能为空
                                    //    ErrorInfo.Set(_localizer["partformatnull_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    //}
                                    if (item.DATA_FORMAT.IsNullOrEmpty() && item.FIXED_VALUE.IsNullOrEmpty())
                                    {
                                        //格式限定和固定值不能同时为空  FORMAT_FIX_CANNOT_EMPTY
                                        ErrorInfo.Set(_localizer["FORMAT_FIX_CANNOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    if (item.PART_NO.ToString().IsNullOrWhiteSpace() && !ErrorInfo.Status)
                                    {
                                        //请输入料号
                                        ErrorInfo.Set(_localizer["partnonull_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    else
                                    {
                                        //输入料号不存在
                                        if (await _repository.ItemIsByPartNo(item.PART_NO) == false)
                                        {
                                            string msg = string.Format(_localizer["partnoexist_error"], item.PART_NO);
                                            ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        }
                                    }
                                    if ((item.COLLECT_OPERATION_ID <= 0 || item.COLLECT_OPERATION_ID == null) && !ErrorInfo.Status)
                                    {
                                        //请选择采集工序。
                                        ErrorInfo.Set(_localizer["collectionprocess_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    if ((item.RESOURCE_QTY < 0 || item.RESOURCE_QTY > 500) && !ErrorInfo.Status)
                                    {
                                        //资料维护数量
                                        ErrorInfo.Set(_localizer["resourceqty_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 附件
                        List<List<SfcsProductAttachmentsAddOrModifyModel>> productAttachments = null;
                        if (model.InsertAttachments != null || model.UpdateAttachments != null)
                        {
                            productAttachments = new List<List<SfcsProductAttachmentsAddOrModifyModel>>();
                        }
                        //插入
                        if (model.InsertAttachments != null && !ErrorInfo.Status)
                        {
                            productAttachments.Add(model.InsertAttachments);
                        }
                        //更新
                        if (model.UpdateAttachments != null && !ErrorInfo.Status)
                        {
                            productAttachments.Add(model.UpdateAttachments);
                        }

                        if (productAttachments != null)
                        {
                            foreach (var templist in productAttachments)
                            {
                                foreach (var item in templist)
                                {
                                    if (item.ATTACHMENT_ID <= 0 && !ErrorInfo.Status)
                                    {
                                        //附件不能为空
                                        ErrorInfo.Set(_localizer["Attachment_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    if ((item.ATTACHMENT_QTY < 0 || item.ATTACHMENT_QTY > 1000) && !ErrorInfo.Status)
                                    {
                                        //附件维护数量:1-1000
                                        ErrorInfo.Set(_localizer["Attachmentqty_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                            }
                        }
                        #endregion

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
        /// 获取资源替换信息
        /// NEW_NO(新料号)
        /// OLD_NO(旧料号)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetResourcesReplace([FromQuery] string partNo="")
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();

            try
            {
                string conditions = "";

                if (!partNo.IsNullOrWhiteSpace())
                {
                    conditions += " AND OLD_NO=:OLDNO ";
                }
                
                returnVM.Result= (await _repository.GetListByTableEX<dynamic>("REPLACE_RESOURCES_ID,NEW_NO,OLD_NO,REPLACE_BY,REPLACE_TIME", "SFCS_RESOURCES_REPLACE", conditions,new {
                    OLDNO= partNo
                }))?.ToList();
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }
            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 产品资源替换保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveResourcesReplace([FromBody] SfcsReplaceModel<CommReplaceViewModel> model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检验参数
                    var count = 0;

                    if (!ErrorInfo.Status && model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        count = model.InsertRecords.Count(c => c.OldNo.IsNullOrWhiteSpace() || c.NewNo.IsNullOrWhiteSpace());
                    }

                    if (!ErrorInfo.Status && count == 0 && model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        count = model.InsertRecords.Count(c => c.OldNo.IsNullOrWhiteSpace() || c.NewNo.IsNullOrWhiteSpace());
                    }
                    if (count > 0)
                    {
                        //请输入新旧料号信息。
                        throw new Exception(_localizer["Please_NewResources_InforMation"]);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.SaveDataByOldResources(model);
                        returnVM.Result = result == 1 ? true : false;
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