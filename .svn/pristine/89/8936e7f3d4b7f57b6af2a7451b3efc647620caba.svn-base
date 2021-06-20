/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-28 10:51:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISysPdaMenusController                                      
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
using JZ.IMS.IServices;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// PDA菜单管理 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SysPdaMenusController : BaseController
    {
        private readonly ISysPdaMenusRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MenuController> _localizer;
        private readonly IManagerRoleService _service;

        public SysPdaMenusController(ISysPdaMenusRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MenuController> localizer, IManagerRoleService service)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _service = service;
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
        public async Task<ApiBaseReturn<List<SysPdaMenusListModel>>> LoadData([FromQuery]SysPdaMenusRequestModel model)
        {
            ApiBaseReturn<List<SysPdaMenusListModel>> returnVM = new ApiBaseReturn<List<SysPdaMenusListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.MENU_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $" and instr(MENU_NAME, :MENU_NAME) > 0  ";
                    }
                    if (!model.MODULE_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $" and  instr(MODULE_NAME, :MODULE_NAME) > 0  ";
                    }
                    if (!model.FORM_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $" and instr(FORM_NAME, :FORM_NAME) > 0 ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "ORDER_SEQ", model)).ToList();
                    var viewList = new List<SysPdaMenusListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SysPdaMenusListModel>(x);
                        //var roles = (_repository.GetListEx<SysPdaMenusRoles>("Where MST_ID=:ID", new { x.ID }))?.ToList();
                        //item.Roles_String = roles.ArrayToString();
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
        /// 查询对应的角色列表
        /// </summary>
        /// <param name="mst_id">子表ID</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SysPdaMenusRolesV>>> LoadDetailData(decimal mst_id)
        {
            ApiBaseReturn<List<SysPdaMenusRolesV>> returnVM = new ApiBaseReturn<List<SysPdaMenusRolesV>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (mst_id > 0)
                    {
                        string conditions = " WHERE mst_id =:mst_id ";
                        var res = (await _repository.GetListAsyncEx<SysPdaMenusRolesV>(conditions, new { mst_id }))?.ToList();

                        returnVM.Result = res;
                        returnVM.TotalCount = res?.Count ?? 0;
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
        /// 获取角色列表数据
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<List<Sys_Manager_Role>> LoadRoleData([FromQuery]ManagerRoleRequestModel model)
        {
            ApiBaseReturn<List<Sys_Manager_Role>> returnVM = new ApiBaseReturn<List<Sys_Manager_Role>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = _service.LoadData(model);
                        if (resultData != null)
                        {

                            returnVM.Result = resultData.data;
                            returnVM.TotalCount = resultData.count;
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SysPdaMenusModel model)
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
                    if (ex.Message.IndexOf("SYS_PDA_MENUS_UINX1") != -1)
                    {
                        ErrorInfo.Set(_localizer["SYS_PDA_MENUS_UINX1"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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
        /// 查询角色对应的PDA菜单列表
        /// </summary>
        /// <param name="role_id">角色ID</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SysPdaMenusOfRoleID>>> LoadPdaMenusByRole(decimal role_id)
        {
            ApiBaseReturn<List<SysPdaMenusOfRoleID>> returnVM = new ApiBaseReturn<List<SysPdaMenusOfRoleID>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (role_id > 0)
                    {
                        var res = await _repository.LoadPdaMenusByRole(role_id);

                        returnVM.Result = res;
                        returnVM.TotalCount = res?.Count ?? 0;
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
        ///保存角色对应的授权数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveData2Role([FromBody] SysPdaMenusOfRoleIDSave model)
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
                        decimal resdata = await _repository.SaveData2Role(model);
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
                    if (ex.Message.IndexOf("SYS_PDA_MENUS_ROLES_UNIQUE") != -1)
                    {
                        ErrorInfo.Set(_localizer["SYS_PDA_MENUS_ROLES_UNIQUE"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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