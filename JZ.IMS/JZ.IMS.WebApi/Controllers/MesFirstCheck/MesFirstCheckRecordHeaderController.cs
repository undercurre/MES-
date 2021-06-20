/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-25 15:16:15                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesFirstCheckRecordHeaderController                                      
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
    /// 首件停机解锁控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesFirstCheckRecordHeaderController : BaseController
    {
        private readonly IMesFirstCheckRecordHeaderRepository _repository;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MesFirstCheckRecordHeaderController> _localizer;

        public MesFirstCheckRecordHeaderController(IMesFirstCheckRecordHeaderRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MesFirstCheckRecordHeaderController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }


        public class StatutsAndContent
        {
            /// <summary>
            /// 解锁状态(Y/N)
            /// </summary>
            public String status { get; set; }
            /// <summary>
            /// 解锁报告
            /// </summary>
            public String Content { get; set; }
        }

        public class FirstCheckRecord
        {
            /// <summary>
            /// 首件测试的ID
            /// </summary>
            public string HeaderId { get; set; }
            /// <summary>
            /// 用户名
            /// </summary>
            public string User { get; set; }
            /// <summary>
            /// 解锁
            /// </summary>
            public string Status { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public string Content { get; set; }
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
        /// 查询首付测试记录
        /// 搜索按钮对应的处理也是这个方法
        /// 参数ID:工单号
        /// LINE_ID:产线ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        //[Authorize]
        public async Task<ApiBaseReturn<List<MesFirstCheckRecordHeaderListModel>>> LoadFirstCheckRecordList([FromQuery]MesFirstCheckRecordHeaderRequestModel model)
        {
            ApiBaseReturn<List<MesFirstCheckRecordHeaderListModel>> returnVM = new ApiBaseReturn<List<MesFirstCheckRecordHeaderListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE  CHECK_TYPE = '首件' ";

                    if (model.LINE_ID > 0)
                    {
                        conditions += " AND A.LINE_ID = :LINE_ID";
                    }
                    var list = (await _repository.GetListPagedAsynclData(model.Page, model.Limit, conditions, "ORDER BY PRODUCT_DATE desc", model)).ToList();
                    var viewList = new List<MesFirstCheckRecordHeaderListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesFirstCheckRecordHeaderListModel>(x);
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
        /// 查询首付测试明细
        /// 搜索按钮对应的处理也是这个方法
        /// 参数ID:首件测试记录ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesFirstCheckRecordDetailListModel>>> LoadFirstCheckRecordDetail([FromQuery]MesFirstCheckRecordHeaderRequestModel model)
        {
            ApiBaseReturn<List<MesFirstCheckRecordDetailListModel>> returnVM = new ApiBaseReturn<List<MesFirstCheckRecordDetailListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE  1=1 ";
                    if (!model.ID.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND HID=:ID ";
                    }
                    var list = (await _repository.GetListPagedEx<MesFirstCheckRecordDetail>(model.Page, model.Limit, conditions, "ID DESC", model)).ToList();
                    var viewList = new List<MesFirstCheckRecordDetailListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesFirstCheckRecordDetailListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsyncEx<MesFirstCheckRecordDetail>(conditions, model);
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
        /// 获取解锁状态(Y/N)和解锁报告
        /// 如果查不到解锁状态(N)，解锁报告为空
        /// 参数ID:首件测试记录ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>o.status,o.content</returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<object>> GetStatusContent(string id)
        {
            ApiBaseReturn<object> returnVM = new ApiBaseReturn<object>();
            returnVM.Result = new { ID = "", STATUS = "N", CONTENT = "" };
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        if (!id.IsNullOrWhiteSpace())
                        {
                            var result = await _repository.GetStatusContent(id);
                            if (result != null)
                            {
                                returnVM.Result = result;
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData(FirstCheckRecord model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status&&model!=null)
                    {
                        if (model.HeaderId.IsNullOrWhiteSpace() ||model.Content.Trim().IsNullOrWhiteSpace() || model.Status.IsNullOrWhiteSpace())
                        {
                            //Messenger.Error("请您选择不良检测报告，并填写解锁报告！");
                            ErrorInfo.Set(_localizer["noeixt_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            returnVM.Result = false;
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveData(model.HeaderId, model.User,model.Status,model.Content);

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