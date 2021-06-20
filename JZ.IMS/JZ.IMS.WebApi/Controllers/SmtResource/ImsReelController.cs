/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-04 15:39:22                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IImsReelController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using FluentValidation.Results;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Public;
using JZ.IMS.WebApi.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 物料料卷信息接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImsReelController : BaseController
    {
        private readonly IImsReelRepository _repository;
        private readonly IMstBom2DetailQtyRepository _repositorymbdq;
        private readonly IStringLocalizer<ImsReelController> _localizer;

        public ImsReelController(IImsReelRepository repository, IMstBom2DetailQtyRepository repositorymbdq, IStringLocalizer<ImsReelController> localizer)
        {
            _repository = repository;
            _repositorymbdq = repositorymbdq;
            _localizer = localizer;
        }
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetReelCode(string reelStr)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 验证参数
                    if (string.IsNullOrEmpty(reelStr))
                    {
                        returnVM.Result = "";
                        return returnVM;
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        ReelInfoViewModel reel = null;

                        #region original
                        if (reelStr.Is2DBarcode())//解析条码是不是二维码条码
                        {
                            //第一步是找IMS_RELL 表中信息，就是找物料数据是不是存在
                            reel = await GetReel(reelStr);
                            //第二步将仓库的条码数据同步到MES中
                            bool keep = await _repository.KeepVendorBarcode(reel);
                            if (!keep)
                            {
                                returnVM.ErrorInfo.Set("料卷信息同步失败!");
                                return returnVM;
                            }
                            //获取物料的信息
                            reel = await _repository.GetReelInfoViewModel(reel.CODE);
                        }
                        else
                        {
                            string reelID = BarcoderFilter.FormatBarcode(reelStr); //去除掩码
                            reel = await _repository.GetReelInfoViewModel(reelID);
                        }
                        #endregion
                        if (reel == null)
                        {
                            #region 创维条码解释

                            // reelCode = WebAPI.BarcodeFilter.FilterBarcodeSkyworth(reelCode, BarcodeTypes.ReelID);
                            //第一步是找IMS_RELL 表中信息，就是找物料数据是不是存在
                            reel = BarcoderFilter.GetSkyworthReel(reelStr);
                            //第二步将仓库的条码数据同步到MES中
                            try
                            {
                                bool keep = await _repository.KeepVendorBarcodeInWMS(reel);
                                if (!keep)
                                {
                                    returnVM.ErrorInfo.Set("物料信息同步失败!");
                                    return returnVM;
                                }
                            }
                            catch (Exception ex)
                            {

                                returnVM.ErrorInfo.Set("物料信息同步失败!异常信息" + ex.Message);
                                return returnVM;
                            }

                            //获取物料的信息
                            reel = await _repository.GetReelInfoViewModel(reel.CODE);

                            #endregion
                        }
                        returnVM.Result = reel.CODE;
                        #region original
                        //}
                        #endregion
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
        /// 获取Reel 2d CODE值
        /// </summary>
        /// <param name="reel2dCode"></param>
        /// <returns></returns>
        private async Task<ReelInfoViewModel> GetReel(string reel2dCode)
        {
            ReelInfoViewModel reel = new ReelInfoViewModel();
            string[] splitArray = reel2dCode.Split2DBarcode();
            if (splitArray.Length < 2)
            {
                reel.CODE = BarcoderFilter.FormatBarcode(reel2dCode);
                return reel;
            }

            foreach (string item in splitArray)  //解析字符串
            {
                string temp = item.Trim();

                if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_ReelID
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.CODE = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_ReelID,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.CODE == null || reel.CODE == "")
                    {
                        throw new Exception("条形码中没有流水号");
                    }
                    int len = reel.CODE.Length;
                    if (reel.CODE.StartsWith("M") == false && len >= 13)
                    {
                        //35911 170401 12345
                        string endStr = reel.CODE.Substring(len - 5 - 6);
                        reel.VENDOR_CODE = reel.CODE.Replace(endStr, "");
                        decimal vendorId = await _repository.GetVendorId(reel.VENDOR_CODE);
                        bool vendorExists = vendorId <= 0 ? false : true;
                        if (vendorExists == false)
                        {
                            reel.VENDOR_CODE = reel.CODE.Substring(0, 5);
                        }
                    }

                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_BOX
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.CODE = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_BOX,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.CODE == null || reel.CODE == "")
                    {
                        throw new Exception("条形码中没有流水号");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_MitacPN
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.PART_NO = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_MitacPN,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.PART_NO.StartsWith("P"))
                        reel.PART_NO = reel.PART_NO.Substring(1);

                    if (reel.PART_NO == null || reel.PART_NO == "")
                    {
                        throw new Exception("条形码中料号值为空");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_MakerPN
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.MAKER_PART_NO = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_MakerPN,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.MAKER_PART_NO == null || reel.MAKER_PART_NO == "")
                    {
                        throw new Exception("条形码中制造商料号值为空");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_Maker
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.MAKER_NAME = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_Maker,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.MAKER_NAME == null || reel.MAKER_NAME == "")
                    {
                        throw new Exception("条形码中制造商名称为空");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_Qty
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    string qty = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_Qty,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (qty == null || qty == "")
                    {
                        throw new Exception("条形码中数量为空");
                    }
                    decimal reelQty = 0;
                    if (!decimal.TryParse(qty, out reelQty))
                    {
                        throw new Exception("条形码中数量填写格式错误");
                    }
                    reel.ORIGINAL_QUANTITY = reelQty;
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_Date
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    string dateCode = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_Date,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (dateCode == null || dateCode == "")
                    {
                        throw new Exception("条形码中生产日期为空");
                    }
                    //dateCode = dateCode.FixDateCode();
                    reel.DATE_CODE = dateCode;
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_CoO
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.COO = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_CoO,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_Lot
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.LOT_CODE = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_Lot,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.LOT_CODE == null || reel.LOT_CODE == "")
                    {
                        throw new Exception("条形码中批次号为空");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_CustomerPN
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.CUSTOMER_PN = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_CustomerPN,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                }
                else if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_Ref
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.REFERENCE = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_Ref,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                }
            }
            if (reel.CODE == null || reel.CODE == "")
            {
                if (splitArray.Length == 1)
                {
                    reel.CODE = splitArray[0];
                }
                else
                {
                    //条形码格式错误
                    throw new Exception(string.Format("料盘条形码{0}解析错误，请检查!", reel2dCode));
                }
            }
            return reel;
        }
        /// <summary>
        /// 查询物料条码详细信息
        /// </summary>
        /// <param name="reelCode">物料解析后的条码数据</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<ReelInfoViewModel>> GetReelInfoViewModel(string reelCode)
        {
            ApiBaseReturn<ReelInfoViewModel> returnVM = new ApiBaseReturn<ReelInfoViewModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetReelInfoViewModel(reelCode).Result;
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
        /// 获取欠料情况报表数据
        /// </summary>
        /// <param name="model">Key</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<TableDataModel>> GetLackMaterialsData(PageModel model)
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _repositorymbdq.GetLackMaterialsData(model);

                    #endregion
                }
                catch (Exception ex)
                {
                    if (returnVM.Result != null) { returnVM.Result = new TableDataModel(); returnVM.Result.code = -1; returnVM.Result.msg = ex.Message; }
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 保存欠料表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Result：-1 失败;1 成功; 新增成功时Result等于欠料表ID</returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<int>> SaveDataByMstBom2Detail([FromBody] MstBom2DetailQtyAddOrModifyModel model)
        {
            ApiBaseReturn<int> returnVM = new ApiBaseReturn<int>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repositorymbdq.SaveDataByMstBom2Detail(model);
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
        /// 根据欠料表ID删除数据
        /// </summary>
        /// <param name="id">欠料表ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DeleteMstBom2DetailById(decimal id)
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
                        decimal resdata = await _repositorymbdq.DeleteMstBom2Detail(id);
                        returnVM.Result = resdata == 1 ? true : false;
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
        /// 获取供应商信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetVendorList(ImsPartRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 获取返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetVendorList(model);
                        returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                        returnVM.TotalCount = await _repository.GetVendorListCount(model);
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
        /// 获取物料信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetImsPartList(ImsPartRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 获取返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetImsPartList(model);
                        returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                        returnVM.TotalCount = await _repository.GetImsPartListCount(model);
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
        /// 保存物料条码信息并生成打印数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<ReelPrintListModel>> SaveReelPrintInfo([FromBody] ReelPrintRequestModel model)
        {
            ApiBaseReturn<ReelPrintListModel> returnVM = new ApiBaseReturn<ReelPrintListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (model.PART_ID.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["PARAMETER_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        model.PART_CODE = _repository.QueryEx<string>(string.Format("SELECT CODE FROM IMS_PART WHERE ID = '{0}'", model.PART_ID)).FirstOrDefault();
                        if (model.PART_CODE.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["PARAMETER_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    if (model.REEL_QTY <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["PARAMETER_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if ((model.VENDOR_ID == "0" || model.VENDOR_ID.IsNullOrWhiteSpace()) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["PARAMETER_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        model.VENDOR_NAME = _repository.QueryEx<string>(string.Format("SELECT NAME FROM IMS_VENDOR WHERE ENABLED ='Y' AND ID = '{0}'", model.VENDOR_ID)).FirstOrDefault();
                        if (model.VENDOR_NAME.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["PARAMETER_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    //if (model.LOT_CODE.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    //{
                    //    ErrorInfo.Set(_localizer["PARAMETER_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //}
                    if (model.DATE_CODE.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["PARAMETER_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.PRINT_QTY <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["PARAMETER_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SaveReelPrintInfo(model);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = null;
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