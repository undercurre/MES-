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
using JZ.IMS.ViewModels.SmtLineSet;
using System.IO;
using JZ.IMS.ViewModels.BomVsPlacement;
using System.Net.Http.Headers;
using JZ.IMS.WebApi.Controllers.BomVsPlacement;
using JZ.IMS.WebApi.Common;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 料单管理 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtPlacementHeaderController : BaseController
    {
        private readonly ISmtPlacementHeaderRepository _repository;
        private readonly IBomVsPlacementRepository _bomVsPlacementRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<BomVsPlacementController> _localizer;
        private BomVsPlacementService _service;

        public SmtPlacementHeaderController(ISmtPlacementHeaderRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<BomVsPlacementController> localizer, IBomVsPlacementRepository bomVsPlacementRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _bomVsPlacementRepository = bomVsPlacementRepository;
        }

        public class IndexVM
        {
            /// <summary>
            /// 机台
            /// </summary>
            public List<dynamic> SmtStation { get; set; }
            /// <summary>
            /// 贴片机组
            /// </summary>
            public List<string> machineList { get; set; }
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
                            SmtStation = await _repository.GetListByTable("ID, SMT_NAME", "SMT_STATION ", " AND ENABLED = 'Y'"),
                            machineList = new List<string>() { "PANASONIC", "PANASONIC_CM", "AI", "RI", "HI", "YAMAHA", "SAMSUNG", "SIEMENS" },
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
        /// 查询料单数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtPlacementHeaderListModel>>> LoadData([FromQuery]SmtPlacementHeaderRequestModel model)
        {
            ApiBaseReturn<List<SmtPlacementHeaderListModel>> returnVM = new ApiBaseReturn<List<SmtPlacementHeaderListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetSmtPlacementHeaderList(model);

                    returnVM.Result = resdata?.data;
                    returnVM.TotalCount = resdata?.count ?? 0;

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
        /// 明细列表
        /// </summary>
        /// <param name="mst_id">主表ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtPlacementDetail>>> GetDetailData(decimal mst_id)
        {
            ApiBaseReturn<List<SmtPlacementDetail>> returnVM = new ApiBaseReturn<List<SmtPlacementDetail>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        string condition = $" WHERE MST_ID =:MST_ID ";

                        var result = await _repository.GetListAsyncEx<SmtPlacementDetail>(condition, new { MST_ID = mst_id });

                        returnVM.Result = result?.ToList();
                        returnVM.TotalCount = result.Count();
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

        public class EditVM
        {
            /// <summary>
            /// 机台
            /// </summary>
            public List<dynamic> SmtStation { get; set; }

            /// <summary>
            /// 板型
            /// </summary>
            public List<dynamic> PCBSide { get; set; }

            /// <summary>
            /// 料单主表
            /// </summary>
            public SmtPlacementHeader PlacementHeader { get; set; }

            /// <summary>
            /// 料单明细
            /// </summary>
            public List<SmtPlacementDetail> PlacementDetail { get; set; }
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        /// <param name="mst_id">主表ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<EditVM>> EditView(decimal mst_id)
        {
            ApiBaseReturn<EditVM> returnVM = new ApiBaseReturn<EditVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new EditVM
                        {
                            SmtStation = await _repository.GetListByTable("ID, SMT_NAME", "SMT_STATION ", "AND ENABLED = 'Y'"),
                            PCBSide = await _repository.GetListByTable("CODE,CN_DESC,VALUE", "SMT_LOOKUP ", "AND TYPE='PCB_SIDE'"),
                        };

                        if (mst_id > 0)
                        {
                            returnVM.Result.PlacementHeader = await _repository.GetAsync(mst_id);

                            var result = await _repository.GetListAsyncEx<SmtPlacementDetail>("WHERE MST_ID =:MST_ID ", new { MST_ID = mst_id });
                            returnVM.Result.PlacementDetail = result?.ToList();
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
        /// 料单编辑之保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtPlacementSaveModel model)
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
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        public class AIPlacementVM
        {
            /// <summary>
            /// 机台
            /// </summary>
            public List<dynamic> SmtStation { get; set; }

            /// <summary>
            /// 板型
            /// </summary>
            public List<IDNAME> PcbSide { get; set; }
        }

        /// <summary>
        /// 料单上传视图
        /// 获取西门子机台Type='SIEMENS'
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<AIPlacementVM>> ImportAIPlacementView([FromQuery]string Type = "AI")
        {
            ApiBaseReturn<AIPlacementVM> returnVM = new ApiBaseReturn<AIPlacementVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {

                        var pcbSide = _repository.QueryEx<IDNAME>("SELECT CODE AS ID,CN_DESC AS NAME FROM SMT_LOOKUP WHERE TYPE = 'PCB_SIDE' AND ENABLED = 'Y' ORDER BY CODE");

                        if (Type== "SIEMENS")
                        {
                            returnVM.Result = new AIPlacementVM
                            {
                                PcbSide = pcbSide,
                                SmtStation = await _repository.GetStationListBySIEMENS(Type),
                            };
                        }
                        else
                        {
                            returnVM.Result = new AIPlacementVM
                            {
                                PcbSide = pcbSide,
                                SmtStation = (await _repository.GetStationList(Type))?.ToList<dynamic>(),
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

        public class ImportAIFileReturnVM<T>
        {
            /// <summary>
            /// BOM信息列表
            /// </summary>
            public List<BOMData> BomList { get; set; }

            /// <summary>
            /// FE料单信息
            /// </summary>
            public AIRIPlacementVM<T> AIRIPlacement { get; set; }
        }

        /// <summary>
        ///  AI料单文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>> ImportAIPlacementFile()
        {
            ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>> returnVM = new ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>> ();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
            ImportAIFileReturnVM<List<AIRIPlacement>> resdata = new ImportAIFileReturnVM<List<AIRIPlacement>>();
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
                        _service = new BomVsPlacementService(_localizer);
                        var tmpdata = _service.LoadAIRIPlacement(save_filename);
                        if (tmpdata != null)
                        {
                            resdata.AIRIPlacement = tmpdata;
                        }
                    }

                    if (!ErrorInfo.Status && resdata.AIRIPlacement != null)
                    {
                        //Bom信息
                        _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                        await _service.LoadBom(resdata.AIRIPlacement.Part_NO, BomType.AI组件);
                        if (_service.IsError)
                        {
                            ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            resdata.BomList = _service.BomInfo;
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (resdata != null)
                    {
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

        public class SamsungModel
        {
            /// <summary>
            /// 成品料号
            /// </summary>
            public string PartNO { get; set; } = null;
            /// <summary>
            /// 拼板数
            /// </summary>
            public int MultiNo { get; set; } = 1;


        }
        /// <summary>
        ///  三星料单上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>> ImportPlacementTxtFile([FromForm]SamsungModel model)
        {
            ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>> returnVM = new ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
            ImportAIFileReturnVM<List<AIRIPlacement>> resdata = new ImportAIFileReturnVM<List<AIRIPlacement>>();
            decimal filesize = 0;
            var newFileName = string.Empty;
            string errmsg = string.Empty;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status && model.PartNO.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["NoPartNO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

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

                    if (!ErrorInfo.Status)
                    {
                        source_filename = ContentDispositionHeaderValue
                                     .Parse(excelFile.ContentDisposition)
                                     .FileName
                                     .Trim('"');
                        extname = source_filename.Substring(source_filename.LastIndexOf("."), source_filename.Length - source_filename.LastIndexOf("."));

                        #region 判断后缀

                        //if (!extname.ToLower().Contains("txt"))
                        //{
                        //    //msg = "只允许上传xlsx格式的Excel文件."
                        //    ErrorInfo.Set(_localizer["incorrectness_placement_file"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
                        _service = new BomVsPlacementService(_localizer);
                        var tmpdata = _service.LoadGalaxyPlacementExcel(save_filename, source_filename, model.PartNO,model.MultiNo);
                        if (tmpdata != null)
                        {
                            resdata.AIRIPlacement = tmpdata;
                        }
                    }

                    if (!ErrorInfo.Status && resdata.AIRIPlacement != null)
                    {
                        //Bom信息
                        _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                        await _service.LoadBomEX(resdata.AIRIPlacement.Part_NO, BomType.三星组件);
                        if (_service.IsError)
                        {
                            ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            resdata.BomList = _service.BomInfo;
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (resdata != null)
                    {
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
        ///  三星料单上传(txt文件)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>> ImportPlacementByTxtFile([FromForm] SamsungModel model)
        {
            ImportAIFileReturnVM<List<AIRIPlacement>> resdata = new ImportAIFileReturnVM<List<AIRIPlacement>>();
            ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>> returnVM = new ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>();
            IFormFile txtFile = Request.Form.Files[0];
            var save_filename = string.Empty;//文件完整路径
            var source_filename = string.Empty;//文件名称
            var extname = string.Empty;//上传的文件后缀
            decimal filesize = 0;//文件大小
            var newFileName = string.Empty;//新的文件名

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status && model.PartNO.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["NoPartNO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && (txtFile == null || txtFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["UPLOAD_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        source_filename = ContentDispositionHeaderValue
                                     .Parse(txtFile.ContentDisposition)
                                     .FileName
                                     .Trim('"');
                        extname = source_filename.Substring(source_filename.LastIndexOf("."), source_filename.Length - source_filename.LastIndexOf("."));

                        #region 判断后缀

                        if (!extname.ToLower().Contains("txt"))
                        {
                            ErrorInfo.Set(_localizer["file_suffix_error_txt"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(txtFile.Length / 1024.00, 3));
                        long mb = txtFile.Length / 1024 / 1024; // MB
                        if (mb > 10)
                        {
                            ErrorInfo.Set(_localizer["size_10m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        #endregion
                    }

                    #endregion

                    #region 保存文件并解析

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
                            txtFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        _service = new BomVsPlacementService(_localizer);
                        var tmpdata = _service.LoadGalaxyPlacementTxt(save_filename, source_filename, model.PartNO);
                        if (tmpdata != null)
                        {
                            resdata.AIRIPlacement = tmpdata;
                        }
                    }

                    if (!ErrorInfo.Status && resdata.AIRIPlacement != null)
                    {
                        //Bom信息
                        _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                        await _service.LoadBomEX(resdata.AIRIPlacement.Part_NO, BomType.三星组件);
                        if (_service.IsError)
                        {
                            ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            resdata.BomList = _service.BomInfo;
                        }
                    }
                    #endregion

                    #region 设置返回值

                    if (resdata != null)
                    {
                        returnVM.Result = resdata;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        ///  RI料单文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>> ImportRIPlacementFile()
        {
            ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>> returnVM = new ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
            ImportAIFileReturnVM<List<AIRIPlacement>> resdata = new ImportAIFileReturnVM<List<AIRIPlacement>>();
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
                        _service = new BomVsPlacementService(_localizer);
                        var tmpdata = _service.LoadRIPlacement(save_filename);
                        if (tmpdata != null)
                        {
                            resdata.AIRIPlacement = tmpdata;
                        }
                    }

                    if (!ErrorInfo.Status && resdata.AIRIPlacement != null)
                    {
                        //Bom信息
                        _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                        await _service.LoadBom(resdata.AIRIPlacement.Part_NO, BomType.AI组件);
                        if (_service.IsError)
                        {
                            ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            resdata.BomList = _service.BomInfo;
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (resdata != null)
                    {
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

        public class SiemenModel
        {
            /// <summary>
            /// 成品料号
            /// </summary>
            public string PartNO { get; set; } = null;

            /// <summary>
            /// 机台ID数组
            /// </summary>
            public string StationIdArrary { get; set; } = null;

            /// <summary>
            /// 拼板数
            /// </summary>
            public int MultiNo { get; set; } = 1;

        }

        /// <summary>
        /// 西门子料单上传 
        /// TYPE:SIEMENS
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ImportAIFileReturnVM<SmtPlacementByLine>>> ImportSiemensPlacementFile([FromForm] SiemenModel model)
        {
            ApiBaseReturn<ImportAIFileReturnVM<SmtPlacementByLine>> returnVM = new ApiBaseReturn<ImportAIFileReturnVM<SmtPlacementByLine>>();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
            ImportAIFileReturnVM<SmtPlacementByLine> resdata = new ImportAIFileReturnVM<SmtPlacementByLine>();
            decimal filesize = 0;
            var newFileName = string.Empty;
            string errmsg = string.Empty;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.PartNO.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["NoPartNO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && (model.StationIdArrary.IsNullOrEmpty()||model.StationIdArrary.Split(',', StringSplitOptions.RemoveEmptyEntries).Length<=0))
                    {
                        ErrorInfo.Set(_localizer["NoStationID"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

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

                    if (!ErrorInfo.Status)
                    {
                        source_filename = ContentDispositionHeaderValue
                                     .Parse(excelFile.ContentDisposition)
                                     .FileName
                                     .Trim('"');
                        extname = source_filename.Substring(source_filename.LastIndexOf("."), source_filename.Length - source_filename.LastIndexOf("."));

                        #region 判断后缀

                        //if (!extname.ToLower().Contains("txt"))
                        //{
                        //    //msg = "只允许上传xlsx格式的Excel文件."
                        //    ErrorInfo.Set(_localizer["incorrectness_placement_file"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
                        _service = new BomVsPlacementService(_bomVsPlacementRepository,_localizer);
                        var stationIdArrary = new List<string>(model.StationIdArrary.Split("," , StringSplitOptions.RemoveEmptyEntries));//返回值包括含有空字符串的数组元素
                        var tmpdata =await _service.LoadSiemensPlacementExcel(save_filename, source_filename, model.PartNO, stationIdArrary,model.MultiNo);
                        if (tmpdata != null)
                        {
                            resdata.AIRIPlacement = tmpdata;
                        }
                    }

                    if (!ErrorInfo.Status && resdata.AIRIPlacement != null)
                    {
                        //Bom信息
                        _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                        await _service.LoadBomEX(resdata.AIRIPlacement.Part_NO, BomType.西门子组件);
                        if (_service.IsError)
                        {
                            ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            resdata.BomList = _service.BomInfo;
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (resdata != null)
                    {
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
        /// AI料单上传之料单BOM比对
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<CompareResult>> AIPlacementCompareByBom([FromBody] AIPlacementCompareModel model)
        {
            ApiBaseReturn<CompareResult> returnVM = new ApiBaseReturn<CompareResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.Part_NO.IsNullOrEmpty())
                    {
                        //throw new Exception("请录入成品料号。");
                        ErrorInfo.Set(_localizer["product_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.Placement.IsNullOrEmpty())
                    {
                        //throw new Exception("请录入料单。");
                        ErrorInfo.Set(_localizer["Placement_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.PlacementList == null || model.PlacementList.Count == 0)
                    {
                        //throw new Exception("请选择料单数据列表。");
                        ErrorInfo.Set(_localizer["DataList_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.Type.IsNullOrEmpty())
                    {
                        //throw new Exception("传入设备类型。");
                        ErrorInfo.Set(_localizer["Machine_Type"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 检查参数并返回

                    if (!ErrorInfo.Status)
                    {
                        BomType bomType = BomType.AI组件;
                        switch (model.Type)
                        {
                            case "AI": bomType = BomType.AI组件 ; break;
                            case "SAMSUNG": bomType = BomType.三星组件; break;
                            case "PANASONIC": bomType = BomType.SMT组件; break;
                            case "PANASONIC_CM": bomType = BomType.SMT组件; break;
                            case "RI": bomType = BomType.RI组件; break;
                            case "HI": bomType = BomType.HI组件; break;
                            case "YAMAHA": bomType = BomType.SMT组件; break;
                            case "SIEMENS": bomType = BomType.西门子组件; break;
                        }
                        _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                        var resBom = await _service.LoadBom(model.Part_NO, bomType);
                        if (_service.IsError)
                        {
                            ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        CompareByBomModel compareByBomModel = new CompareByBomModel()
                        {
                            BOMDataList = (List<BOMData>)_service.BomInfo,
                            PlacementList = model.PlacementList,
                        };
                        if (bomType==BomType.西门子组件)
                        {
                            returnVM.Result = _service.CompareNoLocationByBom(compareByBomModel);
                        }
                        else
                        {
                            returnVM.Result = _service.CompareByBom(compareByBomModel);
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
        /// AI料单上传之保存数据(注,先要作BOM比对,再作保存)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AIPlacementSaveData([FromBody] PlacementSaveModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (model.InsertRecords != null)
                    {
                        foreach (var item in model.InsertRecords)
                        {

                            if (!ErrorInfo.Status && item.Part_NO.IsNullOrEmpty())
                            {
                                //throw new Exception("请录入成品料号。");
                                ErrorInfo.Set(_localizer["product_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status && item.Placement.IsNullOrEmpty())
                            {
                                //throw new Exception("请录入料单。");
                                ErrorInfo.Set(_localizer["Placement_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status && item.PCB_SIDE == 0)
                            {
                                //throw new Exception("请选择板型。");
                                ErrorInfo.Set(_localizer["PCB_SIDE_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status && item.Stations == null || item.Stations.Count == 0)
                            {
                                //throw new Exception("请选择机台。");
                                ErrorInfo.Set(_localizer["Stations_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status && item.DataList == null || item.DataList.Count == 0)
                            {
                                //throw new Exception("请选择料单数据列表。");
                                ErrorInfo.Set(_localizer["DataList_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    #endregion

                    #region 检查参数

                    //if (!ErrorInfo.Status)
                    //{
                    //    _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                    //    var resBom = await _service.LoadBom(model.Part_NO, BomType.AI组件);
                    //    if (_service.IsError)
                    //    {
                    //        ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //    }

                    //    CompareByBomModel compareByBomModel = new CompareByBomModel()
                    //    {
                    //        BOMDataList = (List<BOMData>)_service.BomInfo,
                    //         PlacementList = model.PlacementList,
                    //    };
                    //    var tmpdata = _service.CompareByBom(compareByBomModel);
                    //    if (tmpdata.Result == false && compareByBomModel.BOMDataList != null)
                    //    {
                    //        ErrorInfo.Set(tmpdata.ResultMsg.ToString(), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //    }
                    //}

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status&& model.InsertRecords != null)
                    {
                        var resdata = await _repository.AIPlacementSave(model);
                        if (resdata.result != -1)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            if (resdata.StationIsConfig == 0)
                            {
                                //"请先配置机台序列号！"
                                ErrorInfo.Set(_localizer["StationNoConfig_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (resdata.UseReel != null)
                            {
                                //"{0}料号的料卷{1}正在机台{2}料站{3}制件,不能重传料单.请把该料卷卸下再重传料单。"
                                string errmsg = string.Format(_localizer["Reelid_IsUse_error"], resdata.UseReel.REEL_ID, resdata.UseReel.PART_NO,
                                    resdata.UseReel.StationName, resdata.UseReel.LOCATION);
                                ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        /// Siemens料单上传之保存数据(注,先要作BOM比对,再作保存)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SiemensPlacementSaveData([FromBody] PlacementSaveModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (model.InsertRecords != null)
                    {
                        foreach (var item in model.InsertRecords)
                        {

                            if (!ErrorInfo.Status && item.Part_NO.IsNullOrEmpty())
                            {
                                //throw new Exception("请录入成品料号。");
                                ErrorInfo.Set(_localizer["product_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status && item.Placement.IsNullOrEmpty())
                            {
                                //throw new Exception("请录入料单。");
                                ErrorInfo.Set(_localizer["Placement_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status && item.PCB_SIDE == 0)
                            {
                                //throw new Exception("请选择板型。");
                                ErrorInfo.Set(_localizer["PCB_SIDE_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status && item.Stations == null || item.Stations.Count == 0)
                            {
                                //throw new Exception("请选择机台。");
                                ErrorInfo.Set(_localizer["Stations_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status && item.DataList == null || item.DataList.Count == 0)
                            {
                                //throw new Exception("请选择料单数据列表。");
                                ErrorInfo.Set(_localizer["DataList_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    #endregion

                    #region 检查参数

                    //if (!ErrorInfo.Status)
                    //{
                    //    _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                    //    var resBom = await _service.LoadBom(model.Part_NO, BomType.AI组件);
                    //    if (_service.IsError)
                    //    {
                    //        ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //    }

                    //    CompareByBomModel compareByBomModel = new CompareByBomModel()
                    //    {
                    //        BOMDataList = (List<BOMData>)_service.BomInfo,
                    //         PlacementList = model.PlacementList,
                    //    };
                    //    var tmpdata = _service.CompareByBom(compareByBomModel);
                    //    if (tmpdata.Result == false && compareByBomModel.BOMDataList != null)
                    //    {
                    //        ErrorInfo.Set(tmpdata.ResultMsg.ToString(), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //    }
                    //}

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status && model.InsertRecords != null)
                    {
                        var resdata = await _repository.SiemensPlacementSave(model);
                        if (resdata.result != -1)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
                            if (resdata.StationIsConfig == 0)
                            {
                                //"请先配置机台序列号！"
                                ErrorInfo.Set(_localizer["StationNoConfig_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (resdata.UseReel != null)
                            {
                                //"{0}料号的料卷{1}正在机台{2}料站{3}制件,不能重传料单.请把该料卷卸下再重传料单。"
                                string errmsg = string.Format(_localizer["Reelid_IsUse_error"], resdata.UseReel.REEL_ID, resdata.UseReel.PART_NO,
                                    resdata.UseReel.StationName, resdata.UseReel.LOCATION);
                                ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        ///  雅马哈料单上传(.csv文件)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>> ImportYMHPlacementFile([FromForm] SamsungModel model)
        {
            ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>> returnVM = new ApiBaseReturn<ImportAIFileReturnVM<List<AIRIPlacement>>>();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
            ImportAIFileReturnVM<List<AIRIPlacement>> resdata = new ImportAIFileReturnVM<List<AIRIPlacement>>();
            decimal filesize = 0;
            var newFileName = string.Empty;
            string errmsg = string.Empty;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status && model.PartNO.IsNullOrWhiteSpace())
                    {
                        ErrorInfo.Set(_localizer["NoPartNO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

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

                    if (!ErrorInfo.Status)
                    {
                        source_filename = ContentDispositionHeaderValue
                                     .Parse(excelFile.ContentDisposition)
                                     .FileName
                                     .Trim('"');
                        extname = source_filename.Substring(source_filename.LastIndexOf("."), source_filename.Length - source_filename.LastIndexOf("."));

                        #region 判断后缀

                        //if (extname.ToLower().Contains(".xlsx") || extname.ToLower().Contains(".csv"))
                        //{
                        //    //msg = "只允许上传xlsx格式的Excel文件."
                        //    ErrorInfo.Set(_localizer["incorrectness_placement_file"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
                        _service = new BomVsPlacementService(_localizer);
                        var tmpdata = _service.LoadYMHPlacementExcel(save_filename, source_filename, model.PartNO, model.MultiNo);
                        if (tmpdata != null)
                        {
                            resdata.AIRIPlacement = tmpdata;
                        }
                    }

                    //if (!ErrorInfo.Status && resdata.AIRIPlacement != null)
                    //{
                    //    //Bom信息
                    //    _service = new BomVsPlacementService(_bomVsPlacementRepository, _localizer);
                    //    await _service.LoadBomEX(resdata.AIRIPlacement.Part_NO, BomType.雅码哈);
                    //    if (_service.IsError)
                    //    {
                    //        ErrorInfo.Set(_service.ErrMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //    }
                    //    else
                    //    {
                    //        resdata.BomList = _service.BomInfo;
                    //    }
                    //}

                    #endregion

                    #region 设置返回值

                    if (resdata != null)
                    {
                        returnVM.Result = resdata;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

    }
}