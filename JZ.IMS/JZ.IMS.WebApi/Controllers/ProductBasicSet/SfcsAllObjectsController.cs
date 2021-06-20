/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-31 16:22:05                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsAllObjectsController                                      
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
    /// 采集类型配置 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsAllObjectsController : BaseController
    {
        private readonly ISfcsAllObjectsRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ShareResourceController> _localizer;

        public SfcsAllObjectsController(ISfcsAllObjectsRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
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
            /// 获取采集类型种类
            /// </summary>
            /// <returns></returns>
            public List<CodeName> BjectCategoryList{ get; set; }
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
                            BjectCategoryList = await _repository.GetBjectCategory(),
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
        public async Task<ApiBaseReturn<List<SfcsAllObjectsListModel>>> LoadData([FromQuery]SfcsAllObjectsRequestModel model)
        {
            ApiBaseReturn<List<SfcsAllObjectsListModel>> returnVM = new ApiBaseReturn<List<SfcsAllObjectsListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.OBJECT_CATEGORY != null && model.OBJECT_CATEGORY > 0)
                    {
                        conditions += $"and OBJECT_CATEGORY = :OBJECT_CATEGORY ";
                    }
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(OBJECT_NAME, :Key) > 0 or instr(OBJECT_MARK, :Key) > 0 or instr(DESCRIPTION, :Key) > 0)";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsAllObjectsListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsAllObjectsListModel>(x);
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
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SfcsAllObjectsRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _repository.GetExportData(model);
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsAllObjectsModel model)
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
                    if (!msg.IsNullOrWhiteSpace() && msg.IndexOf("SFCS_OBJECTS_UNIQUE_NAME") != -1)
                    {
                        ErrorInfo.Set(_localizer["objects_name_repeat_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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

    }
}