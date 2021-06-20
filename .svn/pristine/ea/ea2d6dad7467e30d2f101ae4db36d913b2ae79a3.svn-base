using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.Models;
using System.Text;
using FluentValidation.Results;
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using AutoMapper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using JZ.IMS.WebApi.Public;
using JZ.IMS.WebApi.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using JZ.IMS.IRepository;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 角色管理控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManagerRoleController : BaseController
    {
        private readonly IManagerRoleService _service;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IMenuService _menuService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ManagerRoleController> _localizer;
        private readonly IManagerRoleRepository _repository;

        public ManagerRoleController(IManagerRoleService service, IRolePermissionService rolePermissionService, IMenuService menuService,
            IHttpContextAccessor httpContextAccessor, IStringLocalizer<ManagerRoleController> localizer, IManagerRoleRepository repository)
        {
            _service = service;
            _rolePermissionService = rolePermissionService;
            _menuService = menuService;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _repository = repository;
        }

        /// <summary>
        /// 获取角色列表首页
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
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = true;
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
        /// 获取角色列表数据
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<string> LoadData([FromQuery]ManagerRoleRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model != null && (model.Limit == 0 || model.Page == 0))
                    {
                        ErrorInfo.Set(_localizer["pageparam_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = _service.LoadData(model);
                        if (resultData != null)
                        {

                            returnVM.Result = JsonHelper.ObjectToJSON(resultData.data);
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
        /// 获取角色的菜单列表数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<List<MenuNavTree>> LoadMenusByRoleId(decimal roleId)
        {
            ApiBaseReturn<List<MenuNavTree>> returnVM = new ApiBaseReturn<List<MenuNavTree>>();
            List<MenuNavTree> menuTree = null;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var menuList = _service.GetMenusByRoleId(roleId);
                        if (menuList != null)
                        {
                            menuTree = menuList?.Where(t => t.Parent_Id == 0).OrderBy(t => t.Sort)
                               .Select(t => new MenuNavTree()
                               {
                                   Id = t.Id,
                                   Parent_Id = t.Parent_Id,
                                   Menu_Code = t.Menu_Code,
                                   Menu_Name = t.Menu_Name,
                                   Icon_Url = t.Icon_Url,
                                   Link_Url = t.Link_Url,
                                   Sort = t.Sort,
                                   Spread = t.Spread,
                                   Target = t.Target,
                                   MENU_EN = t.MENU_EN,
                                   ENABLED = t.ENABLED,
                                   COLUMNS=t.COLUMNS,
                                   children = GetChildren(menuList, t.Id),
                               }).ToList();

                            returnVM.Result = menuTree;
                            returnVM.TotalCount = menuList.Count;
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
        /// 通过角色ID获取角色的按钮列表z
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<List<Sys_Menu>> GetAllButtonByRoleId(decimal roleId)
        {
            ApiBaseReturn<List<Sys_Menu>> returnVM = new ApiBaseReturn<List<Sys_Menu>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetAllButtonByRoleId(roleId);
                        returnVM.TotalCount = returnVM.Result?.Count ?? 0;
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
        /// 子菜单
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="parent_Id"></param>
        /// <returns></returns>
        private List<MenuNavTree> GetChildren(List<MenuNavView> menuList, decimal parent_Id)
        {
            return menuList?.Where(s => s.Parent_Id == parent_Id).OrderBy(s => s.Sort)
            .Select(s => new MenuNavTree()
            {
                Id = s.Id,
                Parent_Id = s.Parent_Id,
                Menu_Code = s.Menu_Code,
                Menu_Name = s.Menu_Name,
                Icon_Url = s.Icon_Url,
                Link_Url = s.Link_Url,
                Sort = s.Sort,
                Spread = s.Spread,
                Target = s.Target,
                MENU_EN = s.MENU_EN,
                ENABLED = s.ENABLED,
                COLUMNS=s.COLUMNS,
                children = GetChildren(menuList, s.Id),
            }).ToList();
        }

        /// <summary>
        /// 角色新增或编辑时, 获取[授权]及[已授权的菜单主键数组]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<RoleTreeData> AddOrModify(int id)
        {
            ApiBaseReturn<RoleTreeData> returnVM = new ApiBaseReturn<RoleTreeData>();
            string menuIds = string.Empty;
            List<RoleTree> menuTree = null;
            //menuList
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        List<Sys_Menu> menuList = _menuService.LoadAllData();
                        if (menuList != null)
                        {
                            menuTree = menuList?.Where(t => t.Parent_Id == 0)
                               .Select(t => new RoleTree()
                               {
                                   Id = t.Id,
                                   Parent_Id = t.Parent_Id,
                                   Menu_Name = t.Menu_Name,

                                   children = GetChildrenRoleTree(menuList, t.Id),
                               }).ToList();
                        }

                        if (id > 0)
                        {
                            menuIds = _rolePermissionService.GetIdsByRoleId(id).ArrayToString();
                        }

                        returnVM.Result = new RoleTreeData()
                        {
                            RoleTreeList = menuTree,
                            MenuIds = menuIds,
                        };

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
        /// 
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="parent_Id"></param>
        /// <returns></returns>
        private List<RoleTree> GetChildrenRoleTree(List<Sys_Menu> menuList, decimal parent_Id)
        {
            return menuList?.Where(s => s.Parent_Id == parent_Id)
                            .Select(s => new RoleTree()
                            {
                                Id = s.Id,
                                Parent_Id = s.Parent_Id,
                                Menu_Name = s.Menu_Name,
                                MENU_EN = s.MENU_EN,

                                children = GetChildrenRoleTree(menuList, s.Id),
                            }).ToList();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody]ManagerRoleAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    ManagerRoleValidation validationRules = new ManagerRoleValidation(_localizer);
                    ValidationResult results = validationRules.Validate(item);
                    if (!results.IsValid)
                    {
                        ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.AddOrModify(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> Delete(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = _service.DeleteIds(new[] { id });
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
    }

    /// <summary>
    /// 菜单树
    /// </summary>
    public class RoleTree
    {
        /// <summary>
        /// 主键
        /// </summary>
        public decimal Id { get; set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public decimal Parent_Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public String Menu_Name { get; set; }

        /// <summary>
		/// 英文名称
		/// </summary>
		public string MENU_EN { get; set; }

        public List<RoleTree> children { get; set; }
    }

    /// <summary>
    /// 菜单树返回集
    /// </summary>
    public class RoleTreeData
    {
        public List<RoleTree> RoleTreeList { get; set; }

        /// <summary>
        /// 已授权的菜单主键数组
        /// </summary>
        public string MenuIds { get; set; }

    }
}