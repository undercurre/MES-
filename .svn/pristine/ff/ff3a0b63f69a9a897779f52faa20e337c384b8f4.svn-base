/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-18 17:29:33                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IMesQualityItemsController                                      
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
using Microsoft.AspNetCore.Http;
using JZ.IMS.IRepository;
using JZ.IMS.Core.Extensions;
using AutoMapper;
using JZ.IMS.Models;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 自动化线首件控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesQualityItemsController : BaseController
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMesQualityItemsRepository _repository;
		private readonly IMapper _mapper;
		private readonly ISfcsParametersRepository _partmetersRepository;
        private readonly IStringLocalizer<MesQualityItemsController> _localizer;
        private readonly ISysOrganizeRepository _sysOrganizeRepository;
        public MesQualityItemsController(IHttpContextAccessor httpContextAccessor, 
            IMesQualityItemsRepository repository, IMapper mapper, 
            ISfcsParametersRepository partmetersRepository,IStringLocalizer<MesQualityItemsController> localizer,
            ISysOrganizeRepository sysOrganizeRepository)
		{
			this._httpContextAccessor = httpContextAccessor;
			_repository = repository;
			_mapper = mapper;
			_partmetersRepository = partmetersRepository;
            _localizer = localizer;
            _sysOrganizeRepository = sysOrganizeRepository;

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
        /// 查询自动化线首件记录
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesQualityItemsListModel>>> LoadMesQualityItemsList([FromQuery]MesQualityItemsRequestModel model)
        {
            ApiBaseReturn<List<MesQualityItemsListModel>> returnVM = new ApiBaseReturn<List<MesQualityItemsListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    int count = 0;
                    string conditions = " WHERE ID > 0 ";//"where Is_Delete='N' ";//未删除的

                    if (model.CHECK_TYPE != 0)
                        conditions += " AND CHECK_TYPE = :CHECK_TYPE ";

                    if (!model.CHECK_ITEM.IsNullOrWhiteSpace())
                        conditions += " AND (INSTR(CHECK_ITEM,:CHECK_ITEM) >0) ";

                    if (!model.CHECK_DESC.IsNullOrWhiteSpace())
                        conditions += " AND (INSTR(CHECK_DESC,:CHECK_DESC) >0) ";

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, " CHECK_TYPE,ORDER_NO ", model)).ToList();
                    var typeList = _partmetersRepository.GetListByType("MES_QUALITY_TYPE");
                    var viewList = new List<MesQualityItemsListModel>();
                    list?.ForEach(x =>
                    {
                        var organizeData = _sysOrganizeRepository.GetOrganize(Convert.ToInt32(x.ORGANIZE_ID));
                        var item = _mapper.Map<MesQualityItemsListModel>(x);

                        var typeInfo = typeList.SingleOrDefault(f => f.LOOKUP_CODE == x.CHECK_TYPE);
                        if (typeInfo != null)
                            item.CHECK_TYPE_NAME = typeInfo.MEANING;
                        item.ORGANIZE_NAME = organizeData==null?"": organizeData.ORGANIZE_NAME;
                        viewList.Add(item);
                    });
                    count = await _repository.RecordCountAsync(conditions, model);
                    returnVM.Result = viewList;
                    returnVM.TotalCount = count;
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
        /// 获取检验类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<List<SfcsParameters>> GetTypeList()
        {
            ApiBaseReturn<List<SfcsParameters>> returnVM = new ApiBaseReturn<List<SfcsParameters>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        List<SfcsParameters>  typeList =_partmetersRepository.GetListByType("MES_QUALITY_TYPE");
                        returnVM.Result = typeList;
                        returnVM.TotalCount = typeList.Count;
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
        /// <param name="item">修改的话ID要传过来 </param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody]MesQualityItemsAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status && item != null)
                    {
                        //item.ORGANIZE_ID = _httpContextAccessor.HttpContext.Session.GetString("ORGANIZE_ID") ?? string.Empty;
                        if (item.ID == 0)
                        {
                            item.CREATE_TIME = DateTime.Now;
                            item.UPDATE_TIME = DateTime.Now;
                            //item.CREATE_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                        }

                        var result = new BaseResult();
                        MesQualityItems model;
                        if (item.ID == 0)
                        {
                            //TODO ADD
                            model = _mapper.Map<MesQualityItems>(item);
                            model.ID = await Task.Run(() => { return _repository.GetSEQID(); });
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
                            model.UPDATE_TIME = DateTime.Now;
                            //model.UPDATE_USER = _httpContextAccessor.HttpContext.Session.GetString("LoginName") ?? string.Empty;
                            if (model != null)
                            {
                                model.CHECK_TYPE = item.CHECK_TYPE;
                                model.CHECK_ITEM = item.CHECK_ITEM;
                                model.CHECK_DESC = item.CHECK_DESC;
                                model.ORDER_NO = item.ORDER_NO;
                                model.REMARK = item.REMARK;
                                model.UPDATE_USER = item.UPDATE_USER;
                                model.ENABLED = item.ENABLED;
                                model.ISEMPTY = item.ISEMPTY;
                                model.QUANTIZE_TYPE = item.QUANTIZE_TYPE;
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
        /// 通过ID删除数据
        /// </summary>
        /// <param name="Id">ID </param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal Id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (Id <= 0)
                    {
                        returnVM.Result = false;
                        ErrorInfo.Set(ResultCodeAddMsgKeys.CommonFailNoDataMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else
                    {
                        var count = await _repository.DeleteAsync(Id);
                        if (count > 0)
                        {
                            //成功
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            ErrorInfo.Set(ResultCodeAddMsgKeys.CommonExceptionMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            return returnVM;
        }
    }
}