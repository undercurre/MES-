using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using JZ.IMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JZ.IMS.WebApi.Validation;
using FluentValidation.Results;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using JZ.IMS.Models;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 菜单管理控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuController : BaseController
    {
        private readonly IMenuService _service;
        private readonly IStringLocalizer<MenuController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuController(IStringLocalizer<MenuController> localizer, IMenuService service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取菜单列表首页
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
        /// 获取菜单列表数据
        /// </summary>
        /// <param name="model">列表查询模型</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<string> LoadData([FromQuery]MenuRequestModel model)
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
        /// 获取菜单树形列表数据
        /// </summary>
        /// <param name="model">列表查询模型</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<string> LoadData2Tree([FromQuery]MenuRequestModel model)
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
                        model.Page = 1;
                        model.Limit = 10000;
                        var result = _service.LoadData(model);
                        List<Sys_Menu> menuList = ((IEnumerable<Sys_Menu>)result.data).ToList();
                        List<MenuTree> menuTree = menuList?.Where(t => t.Parent_Id == 0)
                            .Select(t => new MenuTree()
                            {
                                Id = t.Id,
                                Parent_Id = t.Parent_Id,
                                Menu_Code = t.Menu_Code,
                                Menu_Name = t.Menu_Name,
                                MENU_TYPE = t.MENU_TYPE,
                                Icon_Url = t.Icon_Url,
                                Link_Url = t.Link_Url,
                                Sort = t.Sort,
                                ENABLED = t.ENABLED,
                                Is_System = t.Is_System,

                                Add_Time = t.Add_Time,
                                Spread = t.Spread,
                                Target = t.Target,
                                Add_Manager_Id = t.Add_Manager_Id,
                                Modify_Manager_Id = t.Modify_Manager_Id,
                                Modify_Time = t.Modify_Time,
                                MENU_EN = t.MENU_EN,
                                COLUMNS=t.COLUMNS,

                                children = GetChildren(menuList, t.Id),
                            }).ToList();


                        if (menuTree != null)
                        {
                            returnVM.Result = JsonHelper.ObjectToJSON(menuTree);
                            returnVM.TotalCount = result.count;
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

        private List<MenuTree> GetChildren(List<Sys_Menu> menuList, decimal parent_Id)
        {
            return menuList?.Where(t => t.Parent_Id == parent_Id)
                            .Select(t => new MenuTree()
                            {
                                Id = t.Id,
                                Parent_Id = t.Parent_Id,
                                Menu_Code = t.Menu_Code,
                                Menu_Name = t.Menu_Name,
                                MENU_TYPE = t.MENU_TYPE,
                                Icon_Url = t.Icon_Url,
                                Link_Url = t.Link_Url,
                                Sort = t.Sort,
                                ENABLED = t.ENABLED,
                                Is_System = t.Is_System,

                                Add_Time = t.Add_Time,
                                Spread = t.Spread,
                                Target = t.Target,
                                Add_Manager_Id = t.Add_Manager_Id,
                                Modify_Manager_Id = t.Modify_Manager_Id,
                                Modify_Time = t.Modify_Time,
                                MENU_EN = t.MENU_EN,
                                COLUMNS=t.COLUMNS,

                                children = GetChildren(menuList, t.Id),
                            }).ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        public string LoadDataSub([FromQuery]MenuRequestModel model)
        {
            return JsonHelper.ObjectToJSON(_service.LoadDataSub(model.parentid));
        }

        /// <summary>
        /// 菜单新增或编辑时获取授权及上级菜单列表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<string> AddOrModify()
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var menuList = _service.GetChildListByParentId(0);
                        if (menuList != null)
                        {
                            returnVM.Result = JsonHelper.ObjectToJSON(menuList);
                            returnVM.TotalCount = 1;
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
        /// 获取子菜单数据(新增或编辑)
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<string> AddOrModifySub(int parentid)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var menuList = _service.GetMenuListById(parentid);
                        if (menuList != null)
                        {
                            returnVM.Result = JsonHelper.ObjectToJSON(menuList);
                            returnVM.TotalCount = 1;
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
        /// 保存菜单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody]MenuAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    MenuValidation validationRules = new MenuValidation(_localizer);
                    ValidationResult results = validationRules.Validate(item);
                    if (!results.IsValid)
                    {
                        ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 保存菜单并返回

                    if (!ErrorInfo.Status)
                    {
                        item.MENU_TYPE = 1;
                        var resultData = await _service.AddOrModifyAsync(item);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySubSave([FromBody]MenuAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    MenuValidation validationRules = new MenuValidation(_localizer);
                    ValidationResult results = validationRules.Validate(item);
                    if (!results.IsValid)
                    {
                        ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 保存按钮并返回

                    if (!ErrorInfo.Status)
                    {
                        item.MENU_TYPE = 2;
                        var resultData = await _service.AddOrModifyAsync(item);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">菜单id</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> Delete(decimal id)
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
                        var resultData = await _service.DeleteIdsAsync(new[] { id });
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
        /// 删除按钮
        /// </summary>
        /// <param name="id">按钮id</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteSub(decimal id)
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
                        var resultData = await _service.DeleteSubAsync(id);
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
        /// 修改菜单状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeDisplayStatus([FromBody]ChangeStatusModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_localizer);
                    ValidationResult results = validationRules.Validate(item);
                    if (!results.IsValid)
                    {
                        ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 保存按钮并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.ChangeDisplayStatusAsync(item);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        ///  路由地址是否存在
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> IsExistsLinkUrl([FromQuery]MenuAddOrModifyModel item)
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
                        var resultData = await _service.IsExistsLinkUrlAsync(item);
                        if (resultData != null)
                        {
                            returnVM.Result = resultData.Data;
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
        /// 通过上级ID获取菜单列表
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<string> LoadDataWithParentId([FromQuery]int ParentId = -1)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = _service.GetChildListByParentId(ParentId);
                        if (resultData != null)
                        {
                            returnVM.Result = JsonHelper.ObjectToJSON(resultData);
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

    /// <summary>
    /// 菜单树
    /// </summary>
    public class MenuTree : Sys_Menu
    {
        public List<MenuTree> children { get; set; }
    }
}