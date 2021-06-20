/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 17:24:04                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsDefectConfigController                                      
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
    /// 不良代码设定 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsDefectConfigController : BaseController
	{
		private readonly ISfcsDefectConfigRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<ShareResourceController> _localizer;
		
		public SfcsDefectConfigController(ISfcsDefectConfigRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<ShareResourceController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}

        public class IndexVM
        {
            /// <summary>
            /// 获取不良类型
            /// </summary>
            /// <returns></returns>
            public List<CodeName> DefectType { get; set; }

            /// <summary>
            /// 获取不良种类
            /// </summary>
            /// <returns></returns>
            public List<CodeName> DefectClass { get; set; }

            /// <summary>
            /// 获取不良类别
            /// </summary>
            /// <returns></returns>
            public List<CodeName> DefectCategory { get; set; }

            /// <summary>
            /// 获取不良等级
            /// </summary>
            /// <returns></returns>
            public List<CodeName> DefectLevelCode { get; set; }

            /// <summary>
            /// 获取不良来源
            /// </summary>
            /// <returns></returns>
            public List<string> DefectSource { get; set; }
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
                            DefectType = await _repository.GetDefectType(),
                            DefectClass = await _repository.GetDefectClass(),
                            DefectCategory = await _repository.GetDefectCategory(),
                            DefectLevelCode = await _repository.GetDefectLevelCode(),
                            DefectSource = await _repository.GetDefectSource()
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
        public async Task<ApiBaseReturn<List<SfcsDefectConfigListModel>>> LoadData([FromQuery]SfcsDefectConfigRequestModel model)
        {
            ApiBaseReturn<List<SfcsDefectConfigListModel>> returnVM = new ApiBaseReturn<List<SfcsDefectConfigListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model?.DEFECT_TYPE > 0)
                        conditions += " and DEFECT_TYPE = :DEFECT_TYPE ";

                    if (model?.DEFECT_CLASS > 0)
                        conditions += " and DEFECT_CLASS = :DEFECT_CLASS ";

                    if (model?.DEFECT_CATEGORY > 0)
                        conditions += " and DEFECT_CATEGORY = :DEFECT_CATEGORY ";

                    if (model?.DEFECT_LEVEL_CODE > 0)
                        conditions += " and LEVEL_CODE = :DEFECT_LEVEL_CODE ";

                    if (!model.DEFECT_SOURCE.IsNullOrWhiteSpace())
                        conditions += " and SOURCE = :DEFECT_SOURCE ";

                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(upper(DEFECT_CODE), upper(:Key)) > 0 or instr(DEFECT_DESCRIPTION, :Key) > 0 or instr(CHINESE_DESCRIPTION, :Key) > 0)";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsDefectConfigListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsDefectConfigListModel>(x);
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
		/// 导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SfcsDefectConfigRequestModel model)
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsDefectConfigModel model)
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
                    string msg = ex.Message;
                    if (!msg.IsNullOrWhiteSpace() && msg.IndexOf("SFCS_UNIQUE_DEFECT_CONFIG_CODE") != -1)
                    {
                        ErrorInfo.Set(_localizer["SFCS_UNIQUE_DEFECT_CONFIG_CODE"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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

        #region 产线维修不良
        /// <summary>
        /// 无码维修
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<string>> GetSfscEquipHeadDataAsync(string WO_NO, int Page, int Limit)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            PageModel pageModel = new PageModel();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    pageModel.Page = Page;
                    pageModel.Limit = Limit;
                    var data = await _repository.GetSfscEquipHeadDataAsync(WO_NO, pageModel);
                    returnVM.Result = JsonHelper.ObjectToJSON(data.data);
                    returnVM.TotalCount = data.count;
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
        /// 有码维修
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<string>> GetSfscEquipHeadCodeDataAsync(string WO_NO, int Page, int Limit)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            PageModel pageModel = new PageModel();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    pageModel.Page = Page;
                    pageModel.Limit = Limit;
                    var data = await _repository.GetSfscEquipHeadCodeDataAsync(WO_NO, pageModel);
                    returnVM.Result = JsonHelper.ObjectToJSON(data.data);
                    returnVM.TotalCount = data.count;
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
        #endregion

    }
}