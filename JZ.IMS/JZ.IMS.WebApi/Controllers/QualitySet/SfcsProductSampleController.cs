/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-16 13:37:10                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsProductSampleController                                      
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
    /// 产品抽检方案控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsProductSampleController : BaseController
    {
        private readonly ISfcsProductSampleRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsProductSampleController> _localizer;
        private const decimal AutoMode = 1;
        private const decimal ManualMode = 2;

        public SfcsProductSampleController(ISfcsProductSampleRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsProductSampleController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 料号
            /// </summary>
            public List<dynamic> PartNoList { get; set; }
            /// <summary>
            /// 抽檢比例
            /// </summary>
            public List<dynamic> SampleList { get; set; }
            /// <summary>
            /// 制程名称
            /// </summary>
            public List<dynamic> RouteList { get; set; }
            /// <summary>
            /// 抽检模式
            /// </summary>
            public List<dynamic> SampleModeList { get; set; }
            /// <summary>
            /// 抽检方案
            /// </summary>
            public List<dynamic> ProjectNameList { get; set; }
            /// <summary>
            /// 抽检比例
            /// </summary>
            public List<dynamic> SampleRatiolList { get; set; }
        }

        /// <summary>
        /// 首页视图 料号请调用 SfcsPn ->查询方法
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
                        returnVM.Result = new IndexVM()
                        {
                            //PartNoList = await _repository.GetListByTable(" SP.* ", " SFCS_PN SP ", ""),
                            SampleList = await _repository.GetListByTable(" SP.* ", " SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='SAMPLE_RATIO' ORDER BY LOOKUP_TYPE "),
                            RouteList = await _repository.GetListByTable(" SR.ID,SR.ROUTE_NAME,SR.Enabled ", " SFCS_ROUTES SR ", " AND SR.ENABLED = 'Y' ORDER BY ROUTE_NAME "),
                            SampleModeList = await _repository.GetListByTable(" SP.* ", " SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='SAMPLE_MODE'   ORDER BY LOOKUP_TYPE "),
                            ProjectNameList = await _repository.GetListByTable(" SSP.ID,SSP.PROJECT_NAME ", " SFCS_SAMPLE_PROJECTS SSP ", " And Enabled='Y' "),
                            SampleRatiolList = await _repository.GetListByTable(" SP.* ", " SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='SAMPLE_RATIO' ORDER BY LOOKUP_TYPE "),
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
        /// 获取标记工序 如果没有提示: 没有找到生效的制程。
        /// 只要传制程ID即可，其他不用传
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetOperationList([FromQuery] SfcsProductSampleRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.TotalCount = 0;
                    returnVM.Result = null;
                    if (model.ROUTE_ID > 0)
                    {
                        var result = await _repository.GetOperationList(model);
                        returnVM.Result = result.data;
                        returnVM.TotalCount = result.count;
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
        /// 获取产品抽检方案分页
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadData([FromQuery]SfcsProductSampleRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.TotalCount = 0;
                    returnVM.Result = null;
                    var result = await _repository.GetProductSampleList(model);
                    returnVM.Result = result.data;
                    returnVM.TotalCount = result.count;

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
        /// 保存数据(标记工序和抽检工序只要传Product_Operation_Code)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsProductSampleModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    List<List<SfcsProductSampleAddOrModifyModel>> productSample = null;
                    if (model.InsertRecords != null || model.UpdateRecords != null)
                    {
                        productSample = new List<List<SfcsProductSampleAddOrModifyModel>>();
                    }
                    //插入
                    if (model.InsertRecords != null && !ErrorInfo.Status)
                    {
                        productSample.Add(model.InsertRecords);
                    }
                    //更新
                    if (model.UpdateRecords != null && !ErrorInfo.Status)
                    {
                        productSample.Add(model.UpdateRecords);
                    }

                    if (productSample != null)
                    {
                        foreach (var templist in productSample)
                        {
                            foreach (var item in templist)
                            {
                                if (!ErrorInfo.Status && item.SAMPLE_MODE <= 0)
                                {
                                    ErrorInfo.Set(_localizer["Err_No_SampleMode"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                if (!ErrorInfo.Status && item.PART_NO.IsNullOrWhiteSpace())
                                {
                                    ErrorInfo.Set(_localizer["Err_PartNoIsNull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                if (!ErrorInfo.Status && (item.DELIVER_OPERATION_CODE <= 0 || item.SAMPLE_OPERATION_CODE <= 0))
                                {
                                    ErrorInfo.Set(_localizer["Err_No_DeliverOperationCode"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                if (!ErrorInfo.Status && item.SAMPLE_OPERATION_COUNT <= 0)
                                {
                                    ErrorInfo.Set(_localizer["Err_No_SampleOperationCount"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                if (!ErrorInfo.Status && item.MUST_SIGN_WITH_FAIL.IsNullOrWhiteSpace())
                                {
                                    ErrorInfo.Set(_localizer["Err_NotInputMustSignWithFailEnabled"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                if (!ErrorInfo.Status && item.ENABLED.IsNullOrWhiteSpace())
                                {
                                    ErrorInfo.Set(_localizer["Err_No_Enabled"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                                if (!ErrorInfo.Status && item.SAMPLE_MODE == AutoMode)
                                {
                                    if (!ErrorInfo.Status && item.PROJECT_ID <= 0)
                                    {
                                        ErrorInfo.Set(_localizer["Err_No_SampleProject"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    if (!ErrorInfo.Status && item.CURRENT_SAMPLE_RATIO <= 0)
                                    {
                                        ErrorInfo.Set(_localizer["Err_No_CurrentSampleRatio"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                                // 判斷deliver是否已設定
                                var productSampleDataTable = await _repository.GetListByTable(" SPS.* ", " SFCS_PRODUCT_SAMPLE SPS ", " And PART_NO=:PART_NO And DELIVER_OPERATION_CODE=:DELIVER_OPERATION_CODE ", new { PART_NO = item.PART_NO, DELIVER_OPERATION_CODE = item.DELIVER_OPERATION_CODE });
                                var viewList = new List<SfcsProductSampleListModel>();
                                productSampleDataTable?.ForEach(x =>
                                {
                                    var obj = _mapper.Map<SfcsProductSampleListModel>(x);
                                    //item.ENABLED = (item.ENABLED == "Y");
                                    viewList.Add(obj);
                                });
                                //新增加
                                if (templist == model.InsertRecords && !ErrorInfo.Status)
                                {
                                    if (viewList.Count == 1 && viewList != null)
                                    {
                                        ErrorInfo.Set(_localizer["Err_DuplicateProductSample"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                                //更新
                                if (templist == model.UpdateRecords && !ErrorInfo.Status)
                                {
                                    if (viewList.Count > 1 && viewList != null)
                                    {
                                        ErrorInfo.Set(_localizer["Err_DuplicateProductSample"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    if (viewList.Count == 1)
                                    {
                                        decimal? recordRowID = viewList.FirstOrDefault().ID;
                                        if (recordRowID != item.ID)
                                        {
                                            ErrorInfo.Set(_localizer["Err_DuplicateProductSample"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        }

                                    }
                                }
                            }
                        }
                    }
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
        /// 验证数据的文档
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<string> ValiDocement()
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = @"1.更换制程的时候，先调用获取标记工序 如果没有数据，提示:没有找到生效的制程。
                                        2.初启SAMPLE_OPERATION_COUNT=1，DELIVER_COUNT=0，SAMPLE_PASS_COUNT=0，SAMPLE_FAIL_COUNT=0，ENABLED='Y',MUST_SIGN_WITH_FAIL='N'
                                        3. 标记工序选择最后一个，提示:当前工序为制程中最后一道工序，不能设定为标记工序。
                                        4.(标记工序ItemIndex+1+连续抽检工序个数+1)>标记工序的总数就提示:当前设定的连续抽检工序数量已超出整个制程所允许的上限，请确认。";

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