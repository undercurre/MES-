/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：组织类型表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-05-05 09:15:05                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISysOrganizeTypeController                                      
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
    /// 组织类型 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SysOrganizeTypeController : BaseController
	{
		private readonly ISysOrganizeTypeRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SysOrganizeTypeController> _localizer;
		
		public SysOrganizeTypeController(ISysOrganizeTypeRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SysOrganizeTypeController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
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
        public async Task<ApiBaseReturn<List<SysOrganizeTypeListModel>>> LoadData([FromQuery]SysOrganizeTypeRequestModel model)
        {
            ApiBaseReturn<List<SysOrganizeTypeListModel>> returnVM = new ApiBaseReturn<List<SysOrganizeTypeListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.ORGANIZE_TYPE_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"AND INSTR(ORGANIZE_TYPE_NAME, :ORGANIZE_TYPE_NAME) > 0 ";
                    }
                    if (!model.ORGANIZE_TYPE_CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $"AND INSTR(ORGANIZE_TYPE_CODE, :ORGANIZE_TYPE_CODE) > 0 ";
                    }
                    if (model.ORDER_ID>0)
                    {
                        conditions += $"AND (ORDER_ID=:ORDER_ID) ";
                    }
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"AND (ENABLED=:ENABLED)  ";
                    }
                    if (!model.REMARK.IsNullOrWhiteSpace())
                    {
                        conditions += $"AND (INSTR(REMARK, :REMARK) > 0 ";
                    }
                    if (model.OrderID>0)
                    {
                        conditions += $" AND ORDER_ID>:ORDERID ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "ORDER_ID ASC", model)).ToList();
                    var viewList = new List<SysOrganizeTypeListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SysOrganizeTypeListModel>(x);
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
        /// 保存数据，删除之前先调用ItemIsByUsed判断是否存在，存在不给删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SysOrganizeTypeModel model)
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