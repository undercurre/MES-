/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：不良报工表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-26 14:37:35                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISfcsDefectReportWorkController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.IRepository;
using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using System;
using System.Reflection;

namespace JZ.IMS.Admin.Controllers
{
    /// <summary>
    /// 波峰焊不良统计
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsEquipWaveDefController : BaseController
    {
        private readonly ISfcsEquipWaveDefMstRepository _repository;
        private readonly ISfcsEquipWaveDefDtlRepository _repository_dtl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISfcsOperationLinesRepository _lineRepository;

        public SfcsEquipWaveDefController(ISfcsEquipWaveDefMstRepository service, ISfcsEquipWaveDefDtlRepository repository_dtl, IHttpContextAccessor httpContextAccessor, ISfcsOperationLinesRepository lineRepository)
        {
            _repository = service;
            _repository_dtl = repository_dtl;
            _lineRepository = lineRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Index(string ORGANIZE_ID)
        {
            //待加入用户信息
            var LineList = _lineRepository.GetLinesList(ORGANIZE_ID, "PCBA");
            return JsonHelper.ObjectToJSON(LineList);
        }

        /// <summary>
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        public async Task<string> LoadData([FromQuery] SfcsEquipWaveDefMstRequestModel model)
        {
            string conditions = "WHERE 1 = 1";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0 or instr(Mobile, :Key) > 0 )";
            }
            var list = await _repository.LoadData(model);

            var data = new TableDataModel
            {
                //TODO：model如新增参数，则需在此方法也增加传入参数
                count = await _repository.RecordCountAsync(conditions, model),
                data = list,
            };
            return JsonHelper.ObjectToJSON(data);
        }

        /// <summary>
        /// 添加或修改的相关操作
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<bool>> AddOrModifyAsync([FromBody] SfcsEquipWaveDefMst model)
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
                        if (model.ID == 0)
                        {
                            //TODO ADD
                            model.ID = await Task.Run(() => { return _repository.GetSEQID(); });
                            model.STATUS = 0;
                            model.CREATE_TIME = DateTime.Now;
                            if (await _repository.InsertAsync(model) > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                            }
                        }
                        else
                        {
                            //TODO Modify
                            var oldModel = _repository.Get(model.ID);
                            model.CREATE_TIME = oldModel.CREATE_TIME;
                            model.CREATE_USER = oldModel.CREATE_USER;
                            model.ORGANIZE_ID = oldModel.ORGANIZE_ID;
                            if (model != null)
                            {
                                //model.Modify_Time = DateTime.Now;
                                if (await _repository.UpdateAsync(model) > 0)
                                {
                                    returnVM.Result = true;
                                }
                                else
                                {
                                    returnVM.Result = false;
                                }
                            }
                            else
                            {
                                returnVM.Result = false;

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
        /// 改善措施
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<bool>> DealAsync([FromBody] SfcsEquipWaveDefMst model)
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

                        //TODO ADD
                        model.ID = await Task.Run(() => { return _repository.GetSEQID(); });
                        model.CREATE_TIME = DateTime.Now;

                        if (await _repository.InsertAsync(model) > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                        }

                        //TODO Modify
                        var oldModel = _repository.Get(model.ID);
                        oldModel.DEAL = model.DEAL;
                        oldModel.DEAL_TIME = DateTime.Now;
                        oldModel.DEAL_USER = model.DEAL_USER;
                        oldModel.STATUS = 1;

                        if (model != null)
                        {
                            //model.Modify_Time = DateTime.Now;
                            if (await _repository.UpdateAsync(oldModel) > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                            }
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
        /// 审核
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<bool>> CheckAsync([FromBody] SfcsEquipWaveDefMst model)
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
                        //TODO Modify
                        var oldModel = _repository.Get(model.ID);
                        oldModel.CHECK_RESULT = model.CHECK_RESULT;
                        oldModel.CHECK_TIME = DateTime.Now;
                        oldModel.CHECK_USER = model.CHECK_USER;
                        oldModel.STATUS = 2;
                        if (model != null)
                        {
                            //model.Modify_Time = DateTime.Now;
                            if (await _repository.UpdateAsync(oldModel) > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                            }
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
        /// 通过ID删除记录
        /// </summary>
        /// <param name="Id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
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
                            _repository_dtl.DeleteList(" where MST_ID=" + id);
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
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> LoadData_Dtl(string MST_ID)
        {
            string conditions = "WHERE 1 = 1  AND MST_ID = :MST_ID";

            var list = (await _repository_dtl.GetListPagedAsync(1, 999, conditions, "ID DESC", new { MST_ID })).ToList();

            var data = new TableDataModel
            {
                //TODO：model如新增参数，则需在此方法也增加传入参数
                count = await _repository_dtl.RecordCountAsync(conditions, new { MST_ID }),
                data = list,
            };
            return JsonHelper.ObjectToJSON(data);
        }

        /// <summary>
        /// 添加或修改的相关操作
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<bool>> AddOrModifyAsync_Dtl([FromBody] SfcsEquipWaveDefDtl model)
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
                        if (model.ID == 0)
                        {
                            //TODO ADD
                            model.ID = await Task.Run(() => { return _repository.GetSEQID(); });
                            model.CREATE_TIME = DateTime.Now;
                            if (await _repository_dtl.InsertAsync(model) > 0)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
                            }
                        }
                        else
                        {
                            //TODO Modify
                            var oldModel = _repository_dtl.Get(model.ID);
                            model.CREATE_TIME = oldModel.CREATE_TIME;
                            model.CREATE_USER = oldModel.CREATE_USER;
                            if (model != null)
                            {
                                //model.Modify_Time = DateTime.Now;
                                if (await _repository_dtl.UpdateAsync(model) > 0)
                                {
                                    returnVM.Result = true;
                                }
                                else
                                {
                                    returnVM.Result = false;
                                }
                            }
                            else
                            {
                                returnVM.Result = false;

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
        /// 通过ID删除记录
        /// </summary>
        /// <param name="Id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        public async Task<ApiBaseReturn<bool>> DeleteOneById_Dtl(decimal id)
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
                        var count = await _repository_dtl.DeleteAsync(id);
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
    }
}