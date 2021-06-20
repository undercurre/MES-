using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using FluentValidation.Results;
using JZ.IMS.IRepository;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Validation;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Public;
using JZ.IMS.Models;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using JZ.IMS.ViewModels.OfflineMaterials;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 离线备料 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OfflineMaterialsController : BaseController
    {
        private readonly IMesOffLineReelRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<OfflineMaterialsController> _localizer;

        public OfflineMaterialsController(IMesOffLineReelRepository repository, IHttpContextAccessor httpContextAccessor, IStringLocalizer<OfflineMaterialsController> localizer)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize("Permission")]
        //[AllowAnonymous]
        public async Task<ApiBaseReturn<List<dynamic>>> Index()
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    //returnVM.Result = await Task.Run(() => { return _repository.GetParametersByType("PART_TYPE"); });

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
        /// 查询数据
        /// 必传飞达(FEEDER)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesOffLineReel>>> LoadData([FromQuery] OfflineMaterialsRequestModel model)
        {
            ApiBaseReturn<List<MesOffLineReel>> returnVM = new ApiBaseReturn<List<MesOffLineReel>>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 检验

                    #endregion

                    #region 设置返回值


                    if (!ErrorInfo.Status)
                    {
                        string conditions = @" WHERE 1=1 AND STATUS=1 ";
                        if (!model.FEED_ID.IsNullOrWhiteSpace())
                        {
                            conditions += " AND FEEDER=:FEED_ID ";
                        }

                        if (!model.FEED_TYPE.IsNullOrWhiteSpace())
                        {
                            conditions += " AND FEEDER_TYPE=:FEED_TYPE ";
                        }

                        var list = (await _repository.GetListPagedEx<MesOffLineReel>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                        int count = 0;
                        count = await _repository.RecordCountAsync(conditions, model);

                        returnVM.Result = list;
                        returnVM.TotalCount = count;
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
        /// 删除只传ID
        /// 只要使用del参数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DELOffLineReel([FromBody] MesOffLineReelModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 检验

                    #endregion

                    #region 设置返回值


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
        /// 检测离线备料
        /// 组装线就不用传飞达和飞达类型(料糟)
        /// FEED_ID:FD1489
        /// FEED_TYPE:1
        /// REEL_ID:20500888194700E84
        /// LINE_ID:180607
        /// USERNAME:ADMIN
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> CheckOfflineMaterials([FromBody] OfflineMaterialsRequestModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 检验
                    if (model.FEED_ID.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(string.Format(_localizer["PARAMS_NOT_NULL"], "FEED_ID"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.REEL_ID.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(string.Format(_localizer["PARAMS_NOT_NULL"], "REEL_ID"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if ((model.LINE_ID ?? 0) <= 0)
                    {
                        ErrorInfo.Set(string.Format(_localizer["PARAMS_NOT_NULL"], "LINE_ID"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    //判断当前线别有没有离线备料事项
                    if (!ErrorInfo.Status)
                    {
                        string lineType = string.Empty;

                        var smtLine = (await _repository.GetListByTableEX<SmtLines>("*", "SMT_LINES", " And ID=:LINE_ID", new { LINE_ID = model.LINE_ID })).FirstOrDefault();
                        if (smtLine != null)
                        {
                            lineType = smtLine.PLANT;
                        }
                        else
                        {
                            var operationLines = (await _repository.GetListByTableEX<SfcsOperationLines>("*", "SFCS_OPERATION_LINES", " And ID=:LINE_ID", new { LINE_ID = model.LINE_ID })).FirstOrDefault();
                            lineType = operationLines.PLANT_CODE.ToString();
                        }
                        if (lineType.IsNullOrEmpty())
                        {
                            //找不到对应的线别类型
                            ErrorInfo.Set(_localizer["CANNOT_FIND_LINETYPE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (!ErrorInfo.Status)
                        {
                            //查有没有离线备料配置
                            if (await _repository.IsCheckConfig(lineType))
                            {
                                //查产前确认主表
                                var productPre = (await _repository.GetProductPreMst(model.LINE_ID.ToString())).FirstOrDefault();
                                if (productPre != null)
                                {
                                    //查子项含有备料事项
                                    var productDtl = (await _repository.IsCheckProductPreDlt(productPre.ID)).FirstOrDefault();
                                    if (productDtl != null)
                                    {
                                        #region  BOM比对
                                        var imsPart = (await _repository.GetImsPart(model.REEL_ID)).FirstOrDefault();
                                        var bom = await _repository.GetBom2(productPre.PART_NO);
                                        var result = bom.Where(c => c.PART_CODE.Contains(imsPart.CODE)).ToList();
                                        if (result != null && result.Count() > 0)
                                        {
                                            //写入离线备料
                                            returnVM.Result = await SaveOffLineReel(model, productPre);
                                            var allPart = await _repository.GetALLPartNo(productPre.MST_NO);
                                            if (allPart.Count() == bom.Count())
                                            {
                                                ItemModel item = new ItemModel();
                                                item.Pre_Mst_No = productPre.MST_NO;
                                                item.Conf_ID = productDtl.CONF_ID.ToString();
                                                var data = await UpdateProductPreItem(item);
                                                if (!data.Result)
                                                {
                                                    //更改 物料准备项 判断结果失败.
                                                    ErrorInfo.Set(_localizer["FAILED_TO_UPDATE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            //没有找到对应的物料条码
                                            ErrorInfo.Set(_localizer["NO_FOUND_BARCODE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //写入离线备料
                                        returnVM.Result = await SaveOffLineReel(model, productPre);
                                    }
                                }
                                else
                                {
                                    //请新增产前确认记录，谢谢！
                                    ErrorInfo.Set(_localizer["PLEASE_ADD_RECORD"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
                            else
                            {
                                //查产前确认主表
                                var productPre = (await _repository.GetProductPreMst(model.LINE_ID.ToString())).FirstOrDefault();
                                if (productPre != null)
                                {
                                    //查子项含有备料事项
                                    var productDtl = (await _repository.IsCheckProductPreDlt(productPre.ID)).FirstOrDefault();
                                    if (productDtl != null)
                                    {
                                        #region  BOM比对
                                        var imsPart = (await _repository.GetImsPart(model.REEL_ID)).FirstOrDefault();
                                        var bom = await _repository.GetBom2(productPre.PART_NO);
                                        var result = bom.Where(c => c.PART_CODE.Contains(imsPart.CODE)).ToList();
                                        if (result != null && result.Count() > 0)
                                        {
                                            //写入离线备料
                                            returnVM.Result = await SaveOffLineReel(model, productPre);
                                            var allPart = await _repository.GetALLPartNo(productPre.MST_NO);
                                            if (allPart.Count() == bom.Count())
                                            {
                                                ItemModel item = new ItemModel();
                                                item.Pre_Mst_No = productPre.MST_NO;
                                                item.Conf_ID = productDtl.CONF_ID.ToString();
                                                var data = await UpdateProductPreItem(item);
                                                if (!data.Result)
                                                {
                                                    //更改 物料准备项 判断结果失败.
                                                    ErrorInfo.Set(_localizer["FAILED_TO_UPDATE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //没有找到对应的物料条码
                                            ErrorInfo.Set(_localizer["NO_FOUND_BARCODE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //写入离线备料
                                        returnVM.Result = await SaveOffLineReel(model, productPre);
                                    }
                                }
                                else
                                {
                                    //写入离线备料
                                    returnVM.Result = await SaveOffLineReel(model, productPre);
                                }
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

        public class ItemModel
        {
            /// <summary>
            /// DTLID
            /// </summary>
            public Decimal? ID { get; set; } = null;

            /// <summary>
            /// 产前配置ID
            /// </summary>
            public string Conf_ID { get; set; } = null;

            /// <summary>
            /// PRE_MST_NO 产前确认编码
            /// </summary>
            /// <returns></returns>
            public string Pre_Mst_No { get; set; } = null;
        }

        /// <summary>
        /// 保存更新产前确认子项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> UpdateProductPreItem([FromBody] ItemModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        if (model.ID <= 0)
                        {
                            var mst = (await _repository.GetListByTableEX<MesProductionPreMst>("*", "MES_PRODUCTION_PRE_MST", " AND MST_No=:MST_No ",
                                      new { MST_No = model.Pre_Mst_No })).FirstOrDefault();
                            if (mst != null)
                            {
                                var dtl = (await _repository.GetListByTableEX<MesProductionPreDtl>("*", "MES_PRODUCTION_PRE_DTL", " AND MST_ID=:MST_ID AND CONF_ID=:CONF_ID",
                                        new { MST_ID = mst.ID, CONF_ID = model.Conf_ID })).FirstOrDefault();
                                model.ID = dtl.ID;
                            }

                        }
                        MesProductionPreMstModel mstModel = new MesProductionPreMstModel();
                        MesProductionPreDtlAddOrModifyModel DtlModify = new MesProductionPreDtlAddOrModifyModel();
                        DtlModify.ID = model.ID ?? 0;
                        mstModel.UpdateRecords = new List<MesProductionPreDtlAddOrModifyModel>();
                        mstModel.UpdateRecords.Add(DtlModify);
                        decimal resdata = await _repository.SaveDTLItme(mstModel);
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
        /// 离线卸料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> OfflineUnloading([FromBody] OfflineUnloadingModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.LINE_ID <= 0)
                        ErrorInfo.Set(string.Format(_localizer["PARAMS_NOT_NULL"], "LineID"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);

                    //FEEDER_REEL_INFORMATION
                    if (!ErrorInfo.Status && model.KEY.IsNullOrEmpty())
                        ErrorInfo.Set(_localizer["FEEDER_REEL_INFORMATION"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.SaveDataOfflineUnloading(model);
                        if (result != -1)
                        {
                            returnVM.Result = true;
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


        private async Task<bool> SaveOffLineReel(OfflineMaterialsRequestModel model, MesProductionPreMst productPre)
        {
            try

            {
                MesOffLineReelModel reelList = new MesOffLineReelModel();
                MesOffLineReelAddOrModifyModel insertModel = new MesOffLineReelAddOrModifyModel();
                insertModel.FEEDER = model.FEED_ID;
                insertModel.LINE_ID = model.LINE_ID ?? 0;
                insertModel.STATUS = 1;//状态，1：待用，2：已用，3：失效
                insertModel.PREPARE_USER = model.UserName;
                insertModel.PREPARE_TIME = DateTime.Now;
                insertModel.PRE_MST_NO = productPre == null ? "" : productPre.MST_NO;
                insertModel.FEEDER_TYPE = model.FEED_TYPE;
                insertModel.REEL_ID = model.REEL_ID;
                reelList.InsertRecords = new List<MesOffLineReelAddOrModifyModel>();
                reelList.InsertRecords.Add(insertModel);
                var data = (await _repository.SaveDataByTrans(reelList));
                if (data > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


    }
}