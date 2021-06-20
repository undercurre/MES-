/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-02 10:47:11                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsProductComponentsController                                      
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
    /// 产品零件维护控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsProductComponentsController : BaseController
    {
        private readonly ISfcsProductComponentsRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsProductComponentsController> _localizer;

        public SfcsProductComponentsController(ISfcsProductComponentsRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsProductComponentsController> localizer)
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
            /// 零件名称
            /// </summary>
            public List<dynamic> ComponentName { get; set; }

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
                            ComponentName = await _repository.GetListByTable("ID, OBJECT_NAME, DESCRIPTION", "SFCS_ALL_OBJECTS", "AND  OBJECT_CATEGORY=1AND ISACTIVE = 'Y' ORDER BY OBJECT_NAME ASC "),
                            Operation = await _repository.GetListByTable("ID, OPERATION_NAME, DESCRIPTION", "SFCS_OPERATIONS", "AND OPERATION_CLASS =1 AND ID != 100 and ID != 999 ORDER BY OPERATION_NAME ASC  "),
                            Attachment = await _repository.GetListByTable("ID, OBJECT_NAME, DESCRIPTION", "SFCS_ALL_OBJECTS", "AND OBJECT_CATEGORY =3 AND ISACTIVE = 'Y' ORDER BY OBJECT_NAME ASC "),
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
        /// 查询零件组件(工序:COLLECT_OPERATION_ID,料号:PART_NO)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsProductComponentsListModel>>> LoadData([FromQuery]SfcsProductComponentsRequestModel model)
        {
            ApiBaseReturn<List<SfcsProductComponentsListModel>> returnVM = new ApiBaseReturn<List<SfcsProductComponentsListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.PART_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(PART_NO, :PART_NO) > 0 ";
                    }
                    if (!string.IsNullOrEmpty(model.COMPONENT_ID.ToString()) && model.COMPONENT_ID > 0)
                    {
                        conditions += $"and instr(COMPONENT_ID, :COMPONENT_ID) > 0 ";
                    }
                    if (!model.ODM_COMPONENT_PN.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(ODM_COMPONENT_PN, :ODM_COMPONENT_PN) > 0 ";
                    }
                    if (!model.CUSTOMER_COMPONENT_PN.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(CUSTOMER_COMPONENT_PN, :CUSTOMER_COMPONENT_PN) > 0 ";
                    }
                    if (!model.DATA_FORMAT.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(DATA_FORMAT, :DATA_FORMAT) > 0 ";
                    }
                    if (model.COMPONENT_QTY > 0)
                    {
                        conditions += $"and instr(COMPONENT_QTY, :COMPONENT_QTY) > 0 ";
                    }
                    if (model.COLLECT_OPERATION_ID > 0)
                    {
                        conditions += $"and instr(COLLECT_OPERATION_ID, :COLLECT_OPERATION_ID) > 0 ";
                    }

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsProductComponentsListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsProductComponentsListModel>(x);
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
        public async Task<ApiBaseReturn<List<SfcsProductAttachments>>> GetComponentsAattachments([FromQuery]SfcsProductAttachmentsRequestModel model)
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
                        conditions += $"and PRODUCT_OBJECT_ID=:PRODUCT_OBJECT_ID";
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
        /// 获取替代料维护
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsSubstituteComponents>>> GetSubstituteComponents([FromQuery]SfcsSubstituteComponentsRequestModel model)
        {
            ApiBaseReturn<List<SfcsSubstituteComponents>> returnVM = new ApiBaseReturn<List<SfcsSubstituteComponents>>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 设置返回值
                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!ErrorInfo.Status && !model.PRODUCT_COMPONENT_ID.IsNullOrWhiteSpace())
                    {
                        conditions += $"and PRODUCT_COMPONENT_ID=:PRODUCT_COMPONENT_ID";
                        var list = (await _repository.GetListPagedEx<SfcsSubstituteComponents>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                        var viewList = new List<SfcsSubstituteComponents>();
                        list?.ForEach(x =>
                        {
                            var item = _mapper.Map<SfcsSubstituteComponents>(x);
                            //item.ENABLED = (item.ENABLED == "Y");
                            viewList.Add(item);
                        });

                        count = await _repository.RecordCountAsyncEx<SfcsSubstituteComponents>(conditions, model);

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
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsProductComponentsModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status)
                    {
                        #region 零件参数难
                        List<List<SfcsProductComponentsAddOrModifyModel>> productResources = null;
                        if (model.InsertComponents != null || model.UpdateComponents != null)
                        {
                            productResources = new List<List<SfcsProductComponentsAddOrModifyModel>>();
                        }
                        //插入
                        if (model.InsertComponents != null && !ErrorInfo.Status)
                        {
                            productResources.Add(model.InsertComponents);
                        }
                        //更新
                        if (model.UpdateComponents != null && !ErrorInfo.Status)
                        {
                            productResources.Add(model.UpdateComponents);
                        }
                        if (productResources != null)
                        {
                            foreach (var templist in productResources)
                            {
                                foreach (var item in templist)
                                {
                                    //if (item.DATA_FORMAT.Length == 0 && !ErrorInfo.Status)
                                    //{
                                    //    //零件格式不能为空
                                    //    ErrorInfo.Set(_localizer["partformatnull_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    //}
                                    if (item.PART_NO.ToString().IsNullOrWhiteSpace() && !ErrorInfo.Status)
                                    {
                                        //请输入料号
                                        ErrorInfo.Set(_localizer["partnonull_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    else
                                    {
                                        ////输入料号不存在
                                        //if (await _repository.ItemIsByPartNo(item.PART_NO) == false)
                                        //{
                                        //    string msg = string.Format(_localizer["partnoexist_error"], item.PART_NO);
                                        //    ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        //}
                                    }
                                    if ((item.COLLECT_OPERATION_ID <= 0 || item.COLLECT_OPERATION_ID == null) && !ErrorInfo.Status)
                                    {
                                        //请选择采集工序。
                                        ErrorInfo.Set(_localizer["collectionprocess_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    //int charCCount = item.DATA_FORMAT.Count(f => f.Equals('C'));
                                    //int charACount = item.DATA_FORMAT.Count(f => f.Equals('A'));
                                    //if ((item.DATA_FORMAT.Length == charCCount || item.DATA_FORMAT.Length == charACount || item.DATA_FORMAT.Length == charCCount + charACount) && !ErrorInfo.Status)
                                    //{
                                    //    ErrorInfo.Set(_localizer["dataform_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    //}
                                }
                            }
                        }
                        #endregion

                        #region 替代料维护
                        List<List<SfcsSubstituteComponentsAddOrModifyModel>> SubstituteComponents = null;
                        if (model.InsertSubstitute != null || model.UpdateSubstitute != null)
                        {
                            SubstituteComponents = new List<List<SfcsSubstituteComponentsAddOrModifyModel>>();
                        }
                        //插入
                        if (model.InsertSubstitute != null && !ErrorInfo.Status)
                        {
                            SubstituteComponents.Add(model.InsertSubstitute);
                        }
                        //更新
                        if (model.UpdateSubstitute != null && !ErrorInfo.Status)
                        {
                            SubstituteComponents.Add(model.UpdateSubstitute);
                        }

                        if (SubstituteComponents != null)
                        {
                            foreach (var templist in SubstituteComponents)
                            {
                                foreach (var item in templist)
                                {
                                    if (item.PRODUCT_COMPONENT_ID <= 0 && !ErrorInfo.Status)
                                    {
                                        string msg = string.Format(_localizer["fieldnonull_error"], "PRODUCT_OBJECT_ID");
                                        ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    if (item.SUBSTITUTE_COMP_PN.ToString().IsNullOrWhiteSpace() && !ErrorInfo.Status)
                                    {
                                        //请输入料号
                                        ErrorInfo.Set(_localizer["partnonull_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    else
                                    {
                                        //if (await _repository.ItemIsByPartNo(item.SUBSTITUTE_COMP_PN) == false)
                                        //{
                                        //    string msg = string.Format(_localizer["partnoexist_error"], item.SUBSTITUTE_COMP_PN);
                                        //    ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        //}
                                    }
                                    if (item.DATA_FORMAT.Length == 0 && !ErrorInfo.Status)
                                    {
                                        //零件格式不能为空
                                        // ErrorInfo.Set(_localizer["partformatnull_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    int charCCount = item.DATA_FORMAT.Count(f => f.Equals('C'));
                                    int charACount = item.DATA_FORMAT.Count(f => f.Equals('A'));
                                    if ((item.DATA_FORMAT.Length == charCCount || item.DATA_FORMAT.Length == charACount || item.DATA_FORMAT.Length == charCCount + charACount) && !ErrorInfo.Status)
                                    {
                                        ErrorInfo.Set(_localizer["dataform_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                            }
                        }




                        #endregion

                        #region 附件表的参数验证
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
                                    if (item.PRODUCT_OBJECT_ID <= 0 && !ErrorInfo.Status)
                                    {
                                        string msg = string.Format(_localizer["fieldnonull_error"], "PRODUCT_OBJECT_ID");
                                        ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    int charCCount = item.DATA_FORMAT.Count(f => f.Equals('C'));
                                    int charACount = item.DATA_FORMAT.Count(f => f.Equals('A'));
                                    //if ((item.DATA_FORMAT.Length == charCCount || item.DATA_FORMAT.Length == charACount || item.DATA_FORMAT.Length == charCCount + charACount) && !ErrorInfo.Status)
                                    //{
                                    //    ErrorInfo.Set(_localizer["dataform_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    //}
                                    if (item.FIX_VALUE.IsNullOrEmpty()&&item.DATA_FORMAT.IsNullOrEmpty())
                                    {
                                        //formatnull_error
                                        ErrorInfo.Set(_localizer["formatnull_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
                    if (ex.Message != null && ex.Message.IndexOf("SFCS_PRODUCT_COMPONENTS_UINX1") != -1)
                    {
                        ErrorInfo.Set(_localizer["components_uinx1"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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