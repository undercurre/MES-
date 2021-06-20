/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：首件确认事项 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-11 14:51:23                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IMesFirstCheckItemsController                                      
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
using Microsoft.AspNetCore.Http;
using JZ.IMS.Core.Extensions;
using AutoMapper;
using JZ.IMS.WebApi.Controllers;
using System.Reflection;
using JZ.IMS.WebApi.Public;
using JZ.IMS.Models;

namespace JZ.IMS.Admin.Controllers
{
    /// <summary>
    /// DIP/组装线首件项目 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MesFirstCheckItemsController : BaseController
	{
		private readonly IMesFirstCheckItemsRepository _repository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISfcsParametersRepository _partmetersRepository;
        private readonly IMapper _mapper;
        public MesFirstCheckItemsController(IMesFirstCheckItemsRepository repository, 
            IHttpContextAccessor httpContextAccessor, ISfcsParametersRepository partmetersRepository, IMapper mapper)
		{
			_repository = repository;
			_httpContextAccessor = httpContextAccessor;
			_partmetersRepository = partmetersRepository;
            _mapper = mapper;

        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<List<SfcsParameters>>> Index()
        {
            ApiBaseReturn<List<SfcsParameters>> returnVM = new ApiBaseReturn<List<SfcsParameters>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    //首五件检验类别数据
                    returnVM.Result = _partmetersRepository.GetListByType("FIRST_CHECK_ITEM_TYPE");
                    //ViewData["ItemTypeList"] = _partmetersRepository.GetListByType();
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;

        }

        /// <summary>
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// CHECK_TYPE:检验类型
        /// CHECK_ITEM:检验项目
        /// CHECK_GIST：检验依据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
		public async Task<ApiBaseReturn<List<MesFirstCheckItemsListModel>>> LoadData([FromQuery]MesFirstCheckItemsRequestModel model)
		{
            ApiBaseReturn<List<MesFirstCheckItemsListModel>> returnVM = new ApiBaseReturn<List<MesFirstCheckItemsListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                    string conditions = " WHERE ID > 0 ";//"where Is_Delete='N' ";//未删除的

                    if (model.CHECK_TYPE != 0)
                        conditions += " AND CHECK_TYPE = :CHECK_TYPE ";

                    if (!model.CHECK_ITEM.IsNullOrWhiteSpace())
                        conditions += " AND (INSTR(CHECK_ITEM,:CHECK_ITEM) >0) ";

                    if (!model.CHECK_GIST.IsNullOrWhiteSpace())
                        conditions += " AND (INSTR(CHECK_GIST,:CHECK_GIST) >0) ";

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "CHECK_TYPE,ORDER_NO", model)).ToList();
                    var viewList = new List<MesFirstCheckItemsListModel>();
                    //获取到校验类型数据
                    var typeList = _partmetersRepository.GetListByType("FIRST_CHECK_ITEM_TYPE");
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesFirstCheckItemsListModel>(x);
                        var type = typeList.FirstOrDefault(f => f.LOOKUP_CODE == item.CHECK_TYPE);
                        item.CHECK_TYPE_NAME = type?.MEANING;
                        //item.Role_Name = _roleRepository.GetNameById(x.ROLE_ID);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    var data = new TableDataModel
                    {
                        //TODO：model如新增参数，则需在此方法也增加传入参数
                        count = await _repository.RecordCountAsync(conditions, model),
                        data = viewList,
                    };

                    returnVM.Result = data.data;
                    returnVM.TotalCount = data.count;
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
		}

		/// <summary>
		/// 添加或修改的相关操作
		/// </summary>
		/// <param name="item">请求体中的数据的映射</param>
		[HttpPost]
        //[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> AddOrModifyAsync([FromForm]MesFirstCheckItemsAddOrModifyModel item)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var result = new BaseResult();
                    MesFirstCheckItems model;
                    if (item.ID == 0)
                    {
                        //TODO ADD
                        model = _mapper.Map<MesFirstCheckItems>(item);
                        model.ID = await Task.Run(() => { return _repository.GetSEQID(); });
                        //model.Is_Delete = "N";
                        //model.Add_Time = DateTime.Now;
                        if (await _repository.InsertAsync(model) > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                    else
                    {
                        //TODO Modify
                        model = _repository.Get(item.ID);
                        if (model != null)
                        {
                            model.CHECK_TYPE = item.CHECK_TYPE;
                            model.CHECK_ITEM = item.CHECK_ITEM;
                            model.CHECK_GIST = item.CHECK_GIST;
                            model.ORDER_NO = item.ORDER_NO;
                            model.REMARK = item.REMARK;
                            model.UPDATE_USER = item.UPDATE_USER;
                            model.UPDATE_TIME = item.UPDATE_TIME;
                            model.ENABLED = item.ENABLED;
                            //model.Modify_Time = DateTime.Now;
                            if (await _repository.UpdateAsync(model) > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                                ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            }
                        }
                        else
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonFailNoDataMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
		}

		/// <summary>
		/// 通过ID删除记录
		/// </summary>
		/// <param name="Id">要删除的记录的ID</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal Id)
		{
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var result = new BaseResult();
                    if (Id <= 0)
                    {
                        result.ResultCode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
                        result.ResultMsg = ResultCodeAddMsgKeys.CommonModelStateInvalidMsg;
                    }
                    else
                    {
                        var count = await _repository.DeleteAsync(Id);
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                WriteLog(ref returnVM);
            }
            return returnVM;
		}

        /// <summary>
        /// 获取可用的检验项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string GetItemsData()
		{
			string organizeId = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
			var result = _repository.GetItemsData(organizeId);
			return JsonHelper.ObjectToJSON(result);
		}
	}
}