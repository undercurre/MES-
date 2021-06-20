/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-11 10:19:01                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtFeederController                                      
*└──────────────────────────────────────────────────────────────┘
*/

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
using JZ.IMS.WebApi.Validation;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using JZ.IMS.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using System.Net.Http.Headers;
using System.IO;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Controllers.BomVsPlacement;
using JZ.IMS.Core.Utilities;
using JZ.IMS.ViewModels.BomVsPlacement;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// BOM和料单比对 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BomVsPlacementController : BaseController
    {
        private readonly IBomVsPlacementRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<BomVsPlacementController> _localizer;
        private BomVsPlacementService _service;

        public BomVsPlacementController(IBomVsPlacementRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<BomVsPlacementController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 获取料单线别
            /// </summary>
            /// <returns></returns>
            public List<CodeName> StationKind { get; set; }
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
                            StationKind = await _repository.GetStationKind(),
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

        public class ImportFileReturnVM
        {
            /// <summary>
            /// BOM信息列表
            /// </summary>
            public List<BOMData> BomList { get; set; }

            /// <summary>
            /// FE料单信息列表
            /// </summary>
            public dynamic PlacementList { get; set; }
        }

        /// <summary>
        /// 导入文件并获取BOM及FE料单数据
        /// </summary>
        /// <param name="smtType">料单类型</param>
        /// <param name="product_no">成品料号</param>
        /// <param name="user_name">用户名称</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ImportFileReturnVM>> ImportFile([FromForm]string smtType, [FromForm]string product_no, [FromForm]string user_name)
        {
            ApiBaseReturn<ImportFileReturnVM> returnVM = new ApiBaseReturn<ImportFileReturnVM>();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
            BomVsPlacementVM resdata = null;
            decimal filesize = 0;
            var newFileName = string.Empty;
            string errmsg = string.Empty;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        try
                        {
                            excelFile = Request.Form.Files[0];
                        }
                        catch (Exception)
                        {
                            //未找到上传的文件,请重新上传。
                            ErrorInfo.Set(_localizer["nofind_file"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    if (!ErrorInfo.Status && (excelFile == null || excelFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && smtType.IsNullOrEmpty())
                    {
                        //throw new Exception("请选择料单类型。");
                        ErrorInfo.Set(_localizer["smt_type_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && product_no.IsNullOrEmpty())
                    {
                        //throw new Exception("请选择成品料号。");
                        ErrorInfo.Set(_localizer["product_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        source_filename = ContentDispositionHeaderValue
                                     .Parse(excelFile.ContentDisposition)
                                     .FileName
                                     .Trim('"');
                        extname = source_filename.Substring(source_filename.LastIndexOf("."), source_filename.Length - source_filename.LastIndexOf("."));

                        #region 判断后缀

                        //if (!extname.ToLower().Contains("xlsx"))
                        //{
                        //    //msg = "只允许上传xlsx格式的Excel文件."
                        //    ErrorInfo.Set(_localizer["file_suffix_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        //}

                        #endregion

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(excelFile.Length / 1024.00, 3));
                        long mb = excelFile.Length / 1024 / 1024; // MB
                        if (mb > 10)
                        {
                            //"只允许上传小于 10MB 的文件."
                            ErrorInfo.Set(_localizer["size_10m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion
                    }

                    #endregion

                    #region 保存文件并解释

                    if (!ErrorInfo.Status)
                    {
                        newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var pathRoot = AppContext.BaseDirectory + @"upload\BOMVsPLFile\";
                        if (Directory.Exists(pathRoot) == false)
                        {
                            Directory.CreateDirectory(pathRoot);
                        }
                        save_filename = pathRoot + $"{newFileName}";
                        using (FileStream fs = System.IO.File.Create(save_filename))
                        {
                            excelFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        _service = new BomVsPlacementService(_repository, _localizer);
                        switch (smtType)
                        {
                            case "AIRI":
                                resdata = await _service.LoadImportFile(smtType, product_no, BomType.AI组件, save_filename, source_filename, user_name);
                                break;
                            case "RI":
                                resdata = await _service.LoadImportFile(smtType, product_no, BomType.RI组件, save_filename, source_filename, user_name);
                                break;
                            case "YAMAHA":
                            case "PANASONIC":
                            case "PANASONIC_CM":
                                resdata = await _service.LoadImportFile(smtType, product_no, BomType.SMT组件, save_filename, source_filename, user_name);
                                break;
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status && resdata != null)
                    {
                        if (resdata.IsError)
                        {
                            ErrorInfo.Set(resdata.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                        else
                        {
                            returnVM.Result = new ImportFileReturnVM()
                            {
                                BomList = resdata.BomList,
                                PlacementList = resdata.PlacementList
                            };
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
        /// 比对
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize("Permission")]
        public ApiBaseReturn<CompareResult> CompareByBom([FromBody] CompareByBomModel model)
        {
            ApiBaseReturn<CompareResult> returnVM = new ApiBaseReturn<CompareResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 比较并返回

                    if (!ErrorInfo.Status)
                    {
                        _service = new BomVsPlacementService(_repository, _localizer);
                        returnVM.Result = _service.CompareByBom(model);
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