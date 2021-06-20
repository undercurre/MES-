/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-18 11:07:21                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtFeederRepairController                                      
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
    /// 飞达维修控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtFeederRepairController : BaseController
    {
        private readonly ISmtFeederRepairRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtFeederRepairController> _localizer;

        public SmtFeederRepairController(ISmtFeederRepairRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtFeederRepairController> localizer)
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
        /// 查询飞达维修信息
        /// </summary>
        /// <param name="feeder">飞达编号</param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SmtFeederRepairReturnModel>> LoadData(string feeder)
        {
            ApiBaseReturn<SmtFeederRepairReturnModel> returnVM = new ApiBaseReturn<SmtFeederRepairReturnModel>();
            SmtFeederRepairReturnModel returnModel = new SmtFeederRepairReturnModel();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && feeder.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入料架！");
                        ErrorInfo.Set(_localizer["feeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var tmpdata = await _repository.GetFeederInfo(feeder);
                        if (tmpdata == null)
                        {
                            //throw new Exception("料架未注册！");
                            ErrorInfo.Set(_localizer["feeder_noexist_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnModel.FEEDER_ID = tmpdata.ID;
                            var resdata = await _repository.GetFeeder2RepairList(new SmtFeederRepairRequestModel { Key = feeder });
                            if (resdata.data == null || resdata.count == 0)
                            {
                                //throw new Exception("该料架没有不良记录，不需要维修！");
                                ErrorInfo.Set(_localizer["no_defect_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status && returnModel.FEEDER_ID > 0)
                    {
                        returnModel.DefectList = await _repository.GetDefectList(feeder);
                        returnModel.RepairList = await _repository.GetRepairList(returnModel.FEEDER_ID ?? 0);
                        //本月維修次數
                        returnModel.RepairCountByMonth = await _repository.GetRepairCountByMonth(returnModel.FEEDER_ID ?? 0);
                        returnModel.RepairTotalCount = await _repository.GetRepairTotalCount(returnModel.FEEDER_ID ?? 0);

                        returnVM.Result = returnModel;
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
        /// 获取报修飞达列表
        /// </summary>
        /// <param name="model">其中Key为对应的过滤条件</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<IDNAME>>> GetFeeder2RepairList([FromQuery]SmtFeederRepairRequestModel model)
        {
            ApiBaseReturn<List<IDNAME>> returnVM = new ApiBaseReturn<List<IDNAME>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetFeeder2RepairList(model);
                        returnVM.Result = resdata.data;
                        returnVM.TotalCount = resdata.count;
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
        /// 获取根本原因列表
        /// </summary>
        /// <param name="model">其中Key为对应的过滤条件</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<CodeName>>> GetReasonList([FromQuery]SmtFeederRepairRequestModel model)
        {
            ApiBaseReturn<List<CodeName>> returnVM = new ApiBaseReturn<List<CodeName>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetReasonList(model);
                        returnVM.Result = resdata.data;
                        returnVM.TotalCount = resdata.count;
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
        /// 获取损坏部件列表
        /// </summary>
        /// <param name="model">其中Key为对应的过滤条件</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<CodeName>>> GetDamagePartList([FromQuery]SmtFeederRepairRequestModel model)
        {
            ApiBaseReturn<List<CodeName>> returnVM = new ApiBaseReturn<List<CodeName>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetDamagePartList(model);
                        returnVM.Result = resdata.data;
                        returnVM.TotalCount = resdata.count;
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
        /// 获取检查項目列表
        /// </summary>
        /// <param name="model">其中Key为对应的过滤条件</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<CodeName>>> GetRepairItemsList([FromQuery]SmtFeederRepairRequestModel model)
        {
            ApiBaseReturn<List<CodeName>> returnVM = new ApiBaseReturn<List<CodeName>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetRepairItemsList(model);
                        returnVM.Result = resdata.data;
                        returnVM.TotalCount = resdata.count;
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
        /// 获取维修结果列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<IDNAME>>> GetResultList()
        {
            ApiBaseReturn<List<IDNAME>> returnVM = new ApiBaseReturn<List<IDNAME>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetResultList();
                        returnVM.Result = resdata;
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
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtFeederRepairAddOrModifyModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            decimal feeder_id = 0;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.FEEDER.IsNullOrEmpty())
                    {
                        //throw new Exception("请输入料架！");
                        ErrorInfo.Set(_localizer["feeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && model.REASON_CODE.IsNullOrEmpty())
                    {
                        //throw new Exception("根本原因不能为空！");
                        ErrorInfo.Set(_localizer["reason_code_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && model.DEFECT_CODE.IsNullOrEmpty())
                    {
                        //throw new Exception("请勾选料架不良信息！");
                        ErrorInfo.Set(_localizer["defect_code_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.DAMAGE_PART.IsNullOrEmpty())
                    {
                        //throw new Exception("损坏部件不能为空！");
                        ErrorInfo.Set(_localizer["damage_part_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    //if (!ErrorInfo.Status && model.REPAIR_ITEM.IsNullOrEmpty())
                    //{
                    //    //throw new Exception("检查项目不能为空！");
                    //    ErrorInfo.Set(_localizer["repair_item_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //}

                    if (!ErrorInfo.Status && model.METHOD.IsNullOrEmpty())
                    {
                        //throw new Exception("请填写维修方法！");
                        ErrorInfo.Set(_localizer["method_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && (model.RESULT != 2 && model.RESULT != 4))
                    {
                        //throw new Exception("维修结果不能正确！");
                        ErrorInfo.Set(_localizer["result_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var tmpdata = await _repository.GetFeederInfo(model.FEEDER);
                        if (tmpdata == null)
                        {
                            //throw new Exception("料架未注册！");
                            ErrorInfo.Set(_localizer["feeder_noexist_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            feeder_id = tmpdata.ID;
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model, feeder_id);
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