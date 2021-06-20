/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-03 16:03:30                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtSolderpasteBatchmappingController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 辅料作业控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtSolderpasteBatchmappingController : BaseController
    {
        private readonly ISmtSolderpasteBatchmappingRepository _repository;
        private readonly IStringLocalizer<SmtSolderpasteBatchmappingController> _localizer;

        public SmtSolderpasteBatchmappingController(ISmtSolderpasteBatchmappingRepository repository, IStringLocalizer<SmtSolderpasteBatchmappingController> localizer)
        {
            this._repository = repository;
            _localizer = localizer;
        }
        /// <summary>
        /// 获取最新批次号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetBatchNo()
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetBatchNo().Result;
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
        /// 通过冰箱的位置获取存储批次信息
        /// </summary>
        /// <param name="loc">冰箱位置</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<IEnumerable<String>>> GetBatchByLoc(String loc)
        {
            ApiBaseReturn<IEnumerable<String>> returnVM = new ApiBaseReturn<IEnumerable<String>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetBatchByLoc(loc);
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
        /// 根据查询条件查询冰箱物理位置
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<IEnumerable<object>>> GetLoction(String para)
        {
            ApiBaseReturn<IEnumerable<object>> returnVM = new ApiBaseReturn<IEnumerable<object>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetLoction(para).Result;
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
        ///  通过批次号获取辅料详细信息
        /// </summary>
        /// <param name="bathNo">批次号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<IEnumerable<object>>> GetBatchDeatil(String bathNo)
        {
            ApiBaseReturn<IEnumerable<object>> returnVM = new ApiBaseReturn<IEnumerable<object>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetBatchDeatil(bathNo).Result;
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
        /// 新增冷藏资料信息
        /// </summary>
        /// <param name="addOrModifyModel"></param>
        /// <remarks>
        /// 传入数据: 辅料数据, 字段说明: 
        /// BATCH_NO 辅料批次号               String
        /// REEL_NO 辅料条码编号（解析后）     string
        /// FRIDGE_LOC 冰箱物料位置           string
        /// OPERATOR 操作人员AD               string
        /// REMARK 备注信息                   string
        /// </remarks>
        /// <returns> true：新增正确， false :新增失败</returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> AddResource([FromBody] SmtSolderpasteBatchmappingAddOrModifyModel addOrModifyModel)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                
                try
                {
                    #region 设置返回值
                    String batchNo = addOrModifyModel.BATCH_NO;
                    String reel_no = addOrModifyModel.REEL_NO;
                    String friDgeLoc = addOrModifyModel.FRIDGE_LOC;
                    string user = addOrModifyModel.OPERATOR;
                    String remark = addOrModifyModel.REMARK;
                    if (string.IsNullOrEmpty(batchNo) || string.IsNullOrEmpty(reel_no)
                        || string.IsNullOrEmpty(friDgeLoc))
                    {
                        ///批次号，辅料条码，冰箱位置为必填项!   
                        throw new Exception(_localizer["CAN_NOT_EMPTY"]);
                    }

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result =(await _repository.AddResources(batchNo, reel_no, friDgeLoc, user, remark));
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
        /// 获取辅料当前的作业
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SmtResourcesRuncardViewModel>> GetResourceRuncardView(String reelCode)
        {
            ApiBaseReturn<SmtResourcesRuncardViewModel> returnVM = new ApiBaseReturn<SmtResourcesRuncardViewModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var data = _repository.GetResourceRuncardView(reelCode).Result;
                        if (data == null)
                        {
                            ErrorInfo.Set(string.Format(_localizer["REELCODE_NO_ERROR"], reelCode), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result = data;
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
        /// 获取辅料制程
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<IEnumerable<SmtResourceRouteOperationViewModel>>> GetResourceRouteOperationView(decimal resourceId)
        {
            ApiBaseReturn<IEnumerable<SmtResourceRouteOperationViewModel>> returnVM =
                new ApiBaseReturn<IEnumerable<SmtResourceRouteOperationViewModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetResourceRouteOperationView(resourceId).Result;
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
        /// 获取辅料历史的作业记录
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<IEnumerable<SmtResourcesRuncardViewModel>>> GetResourceRuncardLogView(String reelCode)
        {
            ApiBaseReturn<IEnumerable<SmtResourcesRuncardViewModel>> returnVM =
                new ApiBaseReturn<IEnumerable<SmtResourcesRuncardViewModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetResourceRuncardLogView(reelCode).Result;
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
        /// 处理辅料逻辑
        /// </summary>
        /// <remarks>
        /// 传入数据: 提交下一步数据, 字段说明: 
        /// resourceNo  辅料条码编号（解析后）               String
        /// nextOperationId  下一步操作ID                   decimal
        /// user 操作人员AD                                 string
        /// </remarks>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ProcessResourceRuncard([FromBody] dynamic processsResources)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    String resourceNo = processsResources.resourceNo;
                    decimal nextOperationId =processsResources.nextOperationId;
                    String user = processsResources.user;

                    _repository.CheckResourcesQty(resourceNo, nextOperationId);

                    if (!ErrorInfo.Status)
                    {
                        _repository.ProcessResourceRuncard(resourceNo, nextOperationId, user);
                        returnVM.Result = true;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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
        /// 提交辅料报废操作操作
        /// </summary>
        /// <param name="processsResources"></param>
        /// <remarks>
        /// 传入数据: 提交下一步数据, 字段说明: 
        /// resourceNo  辅料条码编号（解析后）               String
        /// user 操作人员AD                                 string
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ProcessResourceGiveOut([FromBody] dynamic processsResources)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    
                    #region 设置返回值
                    String resourceNo = processsResources.resourceNo;
                    SmtResourcesRuncardViewModel runcardViewModel = await _repository.GetResourceRuncardView(resourceNo);
                    if (runcardViewModel == null)
                    {
                        ///您输入的辅料不存在,或未进行冷藏!
                        throw new Exception(_localizer["ACCESSORIES_NOT_REFRIGERATED"]);
                    }

                    if ( runcardViewModel!=null&&Convert.ToInt32(runcardViewModel.STATUS_CODE)==3)
                    {
                        ///当前辅料状态已经为报废.
                        throw new Exception(_localizer["CURRENT_MATERIAL_SCRAPPED"]);
                    }

                    if (!ErrorInfo.Status)
                    {
                        _repository.SetResourceRuncardStatus(resourceNo, 3);
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
        /// 提交辅料用完操作
        /// </summary>
        /// <param name="processsResources"></param>
        /// <remarks>
        /// 传入数据: 提交下一步数据, 字段说明: 
        /// resourceNo  辅料条码编号（解析后）               String
        /// user 操作人员AD                                 string
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ProcessResourceFinish([FromBody] dynamic processsResources)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    
                    #region 设置返回值
                    String resourceNo = processsResources.resourceNo;
                    SmtResourcesRuncardViewModel runcardViewModel = await _repository.GetResourceRuncardView(resourceNo);
                    if (runcardViewModel == null)
                    {
                        //您输入的辅料不存在,或未进行冷藏!
                        throw new Exception(_localizer["ACCESSORIES_NOT_REFRIGERATED"]);
                    }
                    if (runcardViewModel != null && Convert.ToInt32(runcardViewModel.STATUS_CODE) == 2)
                    {
                        ///当前辅料状态已经为用完.
                        throw new Exception(_localizer["CURRENT_MATERIAL_USERD"]);
                    }
                    decimal currentOperation = (decimal)runcardViewModel.CURRENT_OPERATION;
                    if (currentOperation != 5 &&
                    currentOperation != 7)
                    {
                        //此锡膏尚未投入使用，不能执行此操作!
                        throw new Exception(_localizer["SOLDER_CANNOT_OPERATION"]);
                    }
                   
                    var routeList = await _repository.GetResourceRoute((decimal)runcardViewModel.RESOURCE_ID);
                    SmtResourceRoute smtResourceRoute = routeList.Where<SmtResourceRoute>(f => f.CURRENT_OPERATION == 10).FirstOrDefault();
                    if(smtResourceRoute == null)
                    {
                        ///当前辅料状态已经为用完.
                        throw new Exception(_localizer["CURRENT_MATERIAL_USED_ROUTE"]);
                    }
                    if (!ErrorInfo.Status)
                    {
                        _repository.SetResourceRuncardStatus(resourceNo, 2);
                        await _repository.UpdateResourceRuncard(smtResourceRoute.ID, (decimal)smtResourceRoute.CURRENT_OPERATION, resourceNo,DateTime.Now,DateTime.Now);
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
}