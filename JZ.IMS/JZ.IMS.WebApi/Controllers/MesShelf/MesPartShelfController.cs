/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2021-01-27 11:50:30                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesPartShelfController                                      
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
    /// 备料间 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesPartShelfController : BaseController
    {
        private readonly IMesPartShelfRepository _repository;
        private readonly IMesPartShelfConfigRepository _partShelfConfigRepository;
        private readonly IMesPartCheckDetailRepository _partCheckDetailRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MesPartShelfController> _localizer;

        public MesPartShelfController(IMesPartShelfRepository repository, IMesPartCheckDetailRepository partCheckDetailRepository, IErpU9Repository erpU9Repository, IMesPartShelfConfigRepository partShelfConfigRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MesPartShelfController> localizer)
        {
            _repository = repository;
            _partShelfConfigRepository = partShelfConfigRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _partCheckDetailRepository = partCheckDetailRepository;
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

        #region 查询料架配置数据
        /// <summary>
        /// 查询料架配置数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        //[HttpGet]
        //[Authorize]
        //public async Task<ApiBaseReturn<List<MesPartShelfConfigListModel>>> LoadConfigData([FromQuery] MesPartShelfConfigRequestModel model)
        //{
        //    ApiBaseReturn<List<MesPartShelfConfigListModel>> returnVM = new ApiBaseReturn<List<MesPartShelfConfigListModel>>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 设置返回值

        //            int count = 0;
        //            string conditions = " WHERE 1=1 ";

        //            if (!model.SHELF_CODE.IsNullOrEmpty())
        //            {
        //                conditions += $" AND SHELF_CODE=:SHELF_CODE ";
        //            }

        //            if (!model.SHELF_NAME.IsNullOrEmpty())
        //            {
        //                conditions += $" AND ( INSTR(SHELF_NAME,:SHELF_NAME)>0) ";
        //            }

        //            var list = (await _partShelfConfigRepository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
        //            var orgainizeList = (await _partShelfConfigRepository.GetListByTableEX<SysOrganize>("*", "SYS_ORGANIZE"))?.ToList();
        //            var viewList = new List<MesPartShelfConfigListModel>();
        //            list?.ForEach(x =>
        //            {
        //                var config = new MapperConfiguration(cfg => cfg.CreateMap<MesPartShelfConfig, MesPartShelfConfigListModel>().ForMember(d => d.ORGANIZE_NAME, opt => opt.Ignore()));
        //                var item = config.CreateMapper().Map<MesPartShelfConfigListModel>(x);
        //                item.ORGANIZE_NAME = orgainizeList == null ? "" : orgainizeList.FirstOrDefault(c => c.ID == item.ORGANIZE_ID)?.ORGANIZE_NAME;
        //                //item.ENABLED = (item.ENABLED == "Y");
        //                viewList.Add(item);
        //            });

        //            count = await _partShelfConfigRepository.RecordCountAsync(conditions, model);

        //            returnVM.Result = viewList;
        //            returnVM.TotalCount = count;

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
        #endregion

        /// <summary>
        /// 查询上下架情况
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        public async Task<ApiBaseReturn<List<MesPartShelfResponseModel>>> LoadData([FromQuery] MesPartShelfRequestModel model)
        {
            ApiBaseReturn<List<MesPartShelfResponseModel>> returnVM = new ApiBaseReturn<List<MesPartShelfResponseModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                if (!ErrorInfo.Status)
                    {
                        var resultDataModel = await _repository.GetShelfByWONO(model);
                        if (resultDataModel.code != -1)
                        {
                            returnVM.Result = resultDataModel.data;
                        }
                        else
                        {
                            //获取数据异常!
                            ErrorInfo.Set(_localizer["error_get_data_exception"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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
        /// 保存上下架数据
        /// 新增前端生成:料架编号:SHELF_CODE(SFCE-YYYYMMDDHHMMSS)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] MesPartShelfModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    String isCode = "";

                    #region 检查参数
                    List<MesPartShelfAddOrModifyModel> partShelfModelList = new List<MesPartShelfAddOrModifyModel>();
                    if (!model.InsertRecords.IsNullOrWhiteSpace() && model.InsertRecords.Count() > 0)
                    {
                        partShelfModelList.AddRange(model.InsertRecords);

                    }
                    if (!model.UpdateRecords.IsNullOrWhiteSpace() && model.InsertRecords.Count() > 0)
                    {
                        partShelfModelList.AddRange(model.UpdateRecords);
                    }

                    foreach (var item in partShelfModelList)
                    {
                        isCode = await _repository.ImsReelByUsed(item.CODE) ? GlobalVariables.EnableY : await _repository.CartonNoByUsed(item.CODE) ? GlobalVariables.EnableN : "";
                        if (String.IsNullOrEmpty(isCode))
                        {
                            ErrorInfo.Set(_localizer["error_reelcode_not_exist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            if (model.InsertRecords.Count>0|| model.UpdateRecords.Count>0)
                            {
                                //条码是否使用过
                                if ((await _repository.PartShelfByUsed(item.CODE)))
                                    ErrorInfo.Set(_localizer["error_reelcode_not_usered"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model, isCode);
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
        /// 保存料架数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveConfigData([FromBody] MesPartShelfConfigModel model)
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
                        decimal resdata = await _partShelfConfigRepository.SaveDataByTrans(model);
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
        /// 获取生产领料信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesCheckMaterialResponseModel>>> GetPickingListData([FromQuery] MesCheckMaterialRequestModel model)
        {
            ApiBaseReturn<List<MesCheckMaterialResponseModel>> returnVM = new ApiBaseReturn<List<MesCheckMaterialResponseModel>>();
            try
            {
                if (String.IsNullOrWhiteSpace(model.WoNo))
                {
                    //工单不能为空，请注意检查!
                    ErrorInfo.Set(_localizer["error_wono_empty"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                if (!ErrorInfo.Status)
                {
                    var resultData = await _repository.GetPickingListData(model);
                    if (resultData.code != -1)
                    {
                        returnVM.Result = resultData?.data ?? "";
                        returnVM.TotalCount = resultData?.count ?? 0;
                    }
                    else
                    {
                        //获取数据异常!
                        ErrorInfo.Set(_localizer["error_get_data_exception"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 核对物料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> CheckPickingByReelCode([FromQuery] MesCheckMaterialRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            try
            {
                #region 验证参数

                String isCode = "";
                if (!ErrorInfo.Status && String.IsNullOrWhiteSpace(model.WoNo))
                {
                    //工单不能为空，请重新输入!
                    ErrorInfo.Set(_localizer["error_wono_empty"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                else
                {
                    //验证工单合法性
                    if (!(await _repository.SfcsWoByUsed(model.WoNo)))
                        ErrorInfo.Set(_localizer["error_wono_not_exist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }

                if (!ErrorInfo.Status && String.IsNullOrWhiteSpace(model.REELCODE))
                {
                    //条码不能为空，请重新输入!
                    ErrorInfo.Set(_localizer["error_reelcode_empty"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                else
                {
                    isCode = await _repository.ImsReelByUsed(model.REELCODE) ? GlobalVariables.EnableY : await _repository.CartonNoByUsed(model.REELCODE) ? GlobalVariables.EnableN : "";
                    if (String.IsNullOrEmpty(isCode))
                    {
                        ErrorInfo.Set(_localizer["error_reelcode_not_exist"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {//条码是否使用过
                        if ((await _repository.PartDetailByUsed(model.REELCODE)))
                            ErrorInfo.Set(_localizer["error_reelcode_not_usered"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                }

                #endregion
                #region 返回值
                if (!ErrorInfo.Status)
                {
                    var resultData = await _repository.CheckPickingByReelCode(model, isCode);
                    returnVM.Result = resultData.code != -1;
                    returnVM.ResultInfo = resultData.msg;
                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
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
        public async Task<ApiBaseReturn<List<MesPartCheckDetailListModel>>> LoadPartCheckDetailData([FromQuery] MesPartCheckDetailRequestModel model)
        {
            ApiBaseReturn<List<MesPartCheckDetailListModel>> returnVM = new ApiBaseReturn<List<MesPartCheckDetailListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.HEADER_ID.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND HEADER_ID=:HEADER_ID ";
                    }

                    if (!model.REEL_CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND REEL_CODE=:REEL_CODE ";
                    }

                    var list = (await _partCheckDetailRepository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesPartCheckDetailListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesPartCheckDetailListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _partCheckDetailRepository.RecordCountAsync(conditions, model);

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
        /// 通过工单获取物料储位
        /// </summary>
        /// <param name="model">工单号</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesPartShelfResponseModel>>> GetShelfByWONO([FromQuery] MesPartShelfRequestModel model)
        {
            ApiBaseReturn<List<MesPartShelfResponseModel>> returnVM = new ApiBaseReturn<List<MesPartShelfResponseModel>>();
            try
            {

                if (!ErrorInfo.Status && String.IsNullOrEmpty(model.WO_NO))
                {
                    //工单不能为空
                    ErrorInfo.Set(_localizer["error_wono_empty"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }

                if (!ErrorInfo.Status)
                {
                    var resultDataModel = await _repository.GetShelfByWONO(model);
                    if (resultDataModel.code != -1)
                    {
                        returnVM.Result = resultDataModel.data;
                    }
                    else
                    {
                        //获取数据异常!
                        ErrorInfo.Set(_localizer["error_get_data_exception"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

    }
}