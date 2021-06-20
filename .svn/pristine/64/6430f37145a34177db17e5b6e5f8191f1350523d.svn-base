/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：组织架构表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-05-05 11:05:54                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISysOrganizeController                                      
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
    /// 组织架构 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SysOrganizeController : BaseController
    {
        private readonly ISysOrganizeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SysOrganizeController> _localizer;

        public SysOrganizeController(ISysOrganizeRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SysOrganizeController> localizer)
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
        public async Task<ApiBaseReturn<List<SysOrganizeListModel>>> LoadData([FromQuery]SysOrganizeRequestModel model)
        {
            ApiBaseReturn<List<SysOrganizeListModel>> returnVM = new ApiBaseReturn<List<SysOrganizeListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.ORGANIZE_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(ORGANIZE_NAME, :Key) > 0) ";
                    }
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (ENABLED =:ENABLED) ";
                    }
                    if (model.ID != null && model.ID > 0)
                    {
                        conditions += $"and (ID =:ID) ";
                    }
                    if (model.ORGANIZE_TYPE_ID != null && model.ORGANIZE_TYPE_ID > 0)
                    {
                        conditions += $"and (ORGANIZE_TYPE_ID =:ORGANIZE_TYPE_ID) ";
                    }
                    if (model.PARENT_ORGANIZE_ID != null && model.PARENT_ORGANIZE_ID > 0)
                    {
                        conditions += $"and (PARENT_ORGANIZE_ID =:PARENT_ORGANIZE_ID) ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SysOrganizeListModel>();
                    if (list != null && list.Count > 0)
                    {
                        var OrganizeTypeIdList = list.Select(t => t.ORGANIZE_TYPE_ID).Distinct().ToList();
                        var OrganizeTypeList = await _repository.GetListByTableEX<SysOrganizeType>("ID,ORGANIZE_TYPE_NAME", "SYS_ORGANIZE_TYPE",
                            " and id in :ids", new { ids = OrganizeTypeIdList.ToArray() });

                        var ParentIdList = list.Where(t => t.PARENT_ORGANIZE_ID != 0).Select(t => t.PARENT_ORGANIZE_ID).Distinct().ToList();
                        List<SysOrganize> ParentList = null;
                        if (ParentIdList != null && ParentIdList.Count >0) {
                            ParentList = await _repository.GetListByTableEX<SysOrganize>("ID,ORGANIZE_NAME", "SYS_ORGANIZE",
                                " and id in :ids", new { ids = ParentIdList.ToArray() });
                        }
                        list?.ForEach(x =>
                        {
                            var item = _mapper.Map<SysOrganizeListModel>(x);

                            item.ORGANIZE_TYPE_NAME = OrganizeTypeList.Where(t => t.ID == x.PARENT_ORGANIZE_ID)
                                                                      .Select(t => t.ORGANIZE_TYPE_NAME).FirstOrDefault() ?? string.Empty;
                            if (ParentList != null)
                            {
                               item.PARENT_ORGANIZE_NAME = ParentList.Where(t => t.ID == x.PARENT_ORGANIZE_ID)
                                                                                                      .Select(t => t.ORGANIZE_NAME).FirstOrDefault() ?? string.Empty;
                            }
                            viewList.Add(item);
                        });
                    }
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
        /// 查询组织架构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SysOrganizeTree>>> LoadTreeData()
        {
            ApiBaseReturn<List<SysOrganizeTree>> returnVM = new ApiBaseReturn<List<SysOrganizeTree>>();
            string menuIds = string.Empty;
            List<SysOrganizeTree> organizeTree = null;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        List<SysOrganize> sysOrgList = (await _repository.GetListAsync())?.ToList();
                        if (sysOrgList != null)
                        {
                            organizeTree = sysOrgList?.Where(t => t.PARENT_ORGANIZE_ID == 0)
                               .Select(t => new SysOrganizeTree()
                               {
                                   ID = t.ID,
                                   ORGANIZE_NAME = t.ORGANIZE_NAME,
                                   ORGANIZE_TYPE_ID = t.ORGANIZE_TYPE_ID,
                                   PARENT_ORGANIZE_ID = t.PARENT_ORGANIZE_ID,

                                   children = GetChildrenTree(sysOrgList, t.ID),
                               }).ToList();
                        }

                        returnVM.Result = organizeTree;
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
        /// <param name="sysOrgList"></param>
        /// <param name="parent_Id"></param>
        /// <returns></returns>
        private List<SysOrganizeTree> GetChildrenTree(List<SysOrganize> sysOrgList, decimal parent_Id)
        {
            return sysOrgList?.Where(s => s.PARENT_ORGANIZE_ID == parent_Id)
                            .Select(s => new SysOrganizeTree()
                            {
                                ID = s.ID,
                                ORGANIZE_NAME = s.ORGANIZE_NAME,
                                ORGANIZE_TYPE_ID = s.ORGANIZE_TYPE_ID,
                                PARENT_ORGANIZE_ID = s.PARENT_ORGANIZE_ID,

                                children = GetChildrenTree(sysOrgList, s.ID),
                            }).ToList();
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
        /// 保存 组织架构 数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SysOrganizeModel model)
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status && id <= 0)
                    {
                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
                            ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var count = await _repository.DeleteAsync(id);
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                ResultCodeAddMsgKeys.CommonExceptionMsg);
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
        /// 查询 组织架构人员关联数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SysUserOrganizeListModel>>> LoadUserOrganize([FromQuery]SysUserOrganizeRequestModel model)
        {
            ApiBaseReturn<List<SysUserOrganizeListModel>> returnVM = new ApiBaseReturn<List<SysUserOrganizeListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _repository.LoadUserOrganize(model);

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
        /// 保存 组织架构人员关联数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveUserOrganize([FromBody] SysUserOrganizeModel model)
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
                        decimal resdata = await _repository.SaveUserOrganize(model);
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