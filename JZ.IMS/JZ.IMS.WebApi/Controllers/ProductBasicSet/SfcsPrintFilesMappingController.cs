/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 10:41:48                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsPrintFilesMappingController                                      
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
    /// 产品与打印标签关系绑定
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsPrintFilesMappingController : BaseController
    {
        private readonly ISfcsPrintFilesMappingRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsProductPalletController> _localizer;
        private readonly ISfcsProductPalletRepository _pprepository;
        private readonly ISfcsModelRepository _modelrepository;

        public SfcsPrintFilesMappingController(ISfcsPrintFilesMappingRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsProductPalletController> localizer, ISfcsProductPalletRepository pprepository,ISfcsModelRepository modelrepository)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _pprepository = pprepository;
            _modelrepository = modelrepository;

        }

        public class IndexVM
        {
            /// <summary>
            /// 客户类型
            /// </summary>
            public List<dynamic> CustomerList { get; set; }
            ///// <summary>
            ///// 机种
            ///// </summary>
            //public List<dynamic> ModelList { get; set; }
            /// <summary>
            /// 工序
            /// </summary>
            public List<dynamic> OperationsList { get; set; }
            /// <summary>
            /// 文件名
            /// </summary>
            public List<dynamic> PrintFileList { get; set; }
            /// <summary>
            /// Y/N
            /// </summary>
            public List<string>  EnableList{ get; set; }
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
                            CustomerList = await _repository.GetListByTable(" SC.ID, SC.CUSTOMER,SC.NATIONALITY ", " SFCS_CUSTOMERS SC ", " AND ENABLED='Y' ORDER BY CUSTOMER ASC"),
                           // ModelList = await _repository.GetListByTable(" SM.ID,SM.MODEL ", " SFCS_MODEL SM ", " ORDER BY SM.MODEL ASC "),
                            OperationsList = await _repository.GetListByTable(" SO.ID,SO.OPERATION_NAME,SO.DESCRIPTION ", " SFCS_OPERATIONS SO ", " AND ENABLED='Y' ORDER BY SO.OPERATION_NAME "),
                            PrintFileList = await _repository.GetListByTable(" ID, FILE_NAME,DESCRIPTION,LABEL_TYPE ", "SFCS_PRINT_FILES", " AND ENABLED='Y' ORDER BY FILE_NAME "),
                            EnableList = new List<string>() {"Y","N" },
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
        /// 机种数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsModelListModel>>> LoadModelData([FromQuery]SfcsModelRequestModel model)
        {
            ApiBaseReturn<List<SfcsModelListModel>> returnVM = new ApiBaseReturn<List<SfcsModelListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    string conditions = " WHERE ID > 0 ";
                    if (model.ID>0)
                    {
                        conditions += $" and ID=:ID  ";
                    }
                    if (!model.DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $" and instr(DESCRIPTION, :DESCRIPTION) > 0  ";
                    }
                    if (!model.MODELName.IsNullOrWhiteSpace())
                    {
                        conditions += $" and instr(MODEL,:MODELName) > 0 ";
                    }
                    int count = 0;
                    var list = (await _modelrepository.GetListPagedAsync(model.Page, model.Limit, conditions, " MODEL ASC ", model)).ToList();
                    var viewList = new List<SfcsModelListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsModelListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _modelrepository.RecordCountAsync(conditions, model);

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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadData([FromQuery]SfcsPrintFilesMappingRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    var result= await _repository.GetSfcsPrintFilesMappingList(model);
                    returnVM.Result = result.data;
                    returnVM.TotalCount =result.count;
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
        /// 双击获取数据(只传ID即可)
        /// 获取标签样式图片使用 SfcsPrintFilesController -> GetPathByType(查图片:Photo)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsPrintFilesMappingListModel>>> DoubleClickData([FromQuery]SfcsPrintFilesMappingRequestModel model)
        {
            ApiBaseReturn<List<SfcsPrintFilesMappingListModel>> returnVM = new ApiBaseReturn<List<SfcsPrintFilesMappingListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0)";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsPrintFilesMappingListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsPrintFilesMappingListModel>(x);
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsPrintFilesMappingModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (model.UpdateRecords!=null)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            if(item.PART_NO== "000000")
                            {
                                continue;
                            }
                            //检查料号
                            if (item.PART_NO!=null&&!item.PART_NO.IsNullOrWhiteSpace())
                            {
                                if (await _pprepository.ItemIsByPartNo(item.PART_NO)==false)
                                {
                                    string msg = string.Format(_localizer["partnoexist_error"], item.PART_NO);
                                    ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
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