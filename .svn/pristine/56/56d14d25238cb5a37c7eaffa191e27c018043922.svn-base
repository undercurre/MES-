/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-12 16:35:20                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtFeederDefectReasonController                                      
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
    /// 失效原因设定控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController] 
	public class SmtFeederDefectReasonController : BaseController
	{
		private readonly ISmtFeederDefectReasonRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SmtFeederDefectReasonController> _localizer;
		
		public SmtFeederDefectReasonController(ISmtFeederDefectReasonRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SmtFeederDefectReasonController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}


        public class IndexVM
        {
            /// <summary>
            /// 是否状态列表
            /// </summary>
            /// <returns></returns>
            public List<string> StatusList { get; set; }

            /// <summary>
            /// 失效代码列表
            /// </summary>
            /// <returns></returns>
            public List<IDNAME> CodeList { get; set; }

       
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
                            StatusList = _repository.GetEnableStatusList(),
                            CodeList = await _repository.GetEnableCodeList()
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
        /// <remarks>
        /// 字键说明:
        /// 主键ID: ID
        /// 失效代码(CODE):string CODE
        /// 描述：string DESCRIPTION
        /// 是否可用:String(Y/N) ENABLED
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtFeederDefectReasonListModel>>> LoadData([FromQuery]SmtFeederDefectReasonRequestModel model)
        {
            ApiBaseReturn<List<SmtFeederDefectReasonListModel>> returnVM = new ApiBaseReturn<List<SmtFeederDefectReasonListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (CODE=:CODE)";
                    }
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (ENABLED=:ENABLED)";
                    }
                    if (!model.DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (DESCRIPTION=:DESCRIPTION)";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SmtFeederDefectReasonListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SmtFeederDefectReasonListModel>(x);
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
        /// 当前code是否已被使用 
        /// </summary>
        /// <remarks>
        /// 字段说明:
        /// 需要传CODE字段
        /// </remarks>
        /// <param name="code">code</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ItemIsByUsed(string code)
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
                        if (!code.IsNullOrWhiteSpace())
                        {
                            result = await _repository.ItemIsByUsed(code);
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtFeederDefectReasonModel model)
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
                    if (!msg.IsNullOrWhiteSpace() && msg.IndexOf("code_is_same") != -1)
                    {
                        ErrorInfo.Set(_localizer["code_is_same"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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

        ///// <summary>
        ///// 添加或修改视图
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Authorize("Permission")]
        //public ApiBaseReturn<bool> AddOrModify()
        //{
        //    ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 设置返回值

        //            returnVM.Result = true;

        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
        //        }
        //    }

        //    #region 如果出现错误，则写错误日志并返回错误内容

        //    WriteLog(ref returnVM);

        //    #endregion

        //    return returnVM;
        //}

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        //[HttpPost]
        //[Authorize("Permission")]
        //public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        //{
        //    ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 删除并返回

        //            if (!ErrorInfo.Status && id <= 0)
        //            {
        //                returnVM.Result = false;
        //                //通用提示类的本地化问题处理
        //                string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
        //                    ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
        //                ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //            }
        //            if (!ErrorInfo.Status)
        //            {
        //                var count = await _repository.DeleteAsync(id);
        //                if (count > 0)
        //                {
        //                    returnVM.Result = true;
        //                }
        //                else
        //                {
        //                    //失败
        //                    returnVM.Result = false;
        //                    //通用提示类的本地化问题处理
        //                    string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
        //                        ResultCodeAddMsgKeys.CommonExceptionMsg);
        //                    ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //                }
        //            }
        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
        //        }
        //    }

        //    #region 如果出现错误，则写错误日志并返回错误内容

        //    WriteLog(ref returnVM);

        //    #endregion

        //    return returnVM;
        //}
		
	}
}