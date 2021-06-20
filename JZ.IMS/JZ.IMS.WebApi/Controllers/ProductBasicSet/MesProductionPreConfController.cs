/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认配置表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-24 17:23:47                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesProductionPreConfController                                      
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
    /// 产前确认配置 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesProductionPreConfController : BaseController
    {
        private readonly IMesProductionPreConfRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MesProductionPreConfController> _localizer;

        public MesProductionPreConfController(IMesProductionPreConfRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MesProductionPreConfController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 确认项目列表
            /// </summary>
            public List<dynamic> ContentTypeList { get; set; }

            /// <summary>
            /// 工厂类别列表
            /// </summary>
            public List<dynamic> ClassTypeList { get; set; }
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
                            ContentTypeList = await _repository.GetListByTableEX<dynamic>("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'PRODUCTION_PRE_TYPE'"),
                            ClassTypeList = await _repository.GetListByTableEX<dynamic>("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'SBU_CODE'"),
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
        /// 传线别类型(class_TYPE)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesProductionPreConfListModel>>> LoadData([FromQuery]MesProductionPreConfRequestModel model)
        {
            ApiBaseReturn<List<MesProductionPreConfListModel>> returnVM = new ApiBaseReturn<List<MesProductionPreConfListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (ENABLED =:ENABLED) ";
                    }
                    if (!model.CONTENT.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(CONTENT, :CONTENT) > 0) ";
                    }
                    if (!model.CONFIRM_CONTENT.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(CONFIRM_CONTENT, :CONFIRM_CONTENT) > 0) ";
                    }
                    if (!model.CLASS_TYPE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and CLASS_TYPE=:CLASS_TYPE ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesProductionPreConfListModel>();
                    var contentTypeList = await _repository.GetListByTableEX<SfcsParameters>("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'PRODUCTION_PRE_TYPE'");
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesProductionPreConfListModel>(x);
                        item.CONTENT_TYPE_Caption = contentTypeList.Where(t => t.LOOKUP_CODE == x.CONTENT_TYPE).Select(t => t.CHINESE).FirstOrDefault();

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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] MesProductionPreConfModel model)
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

    }
}