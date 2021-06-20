using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using JZ.IMS.Core.Utilities;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 导入Excel基本资料 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImportExcelController : BaseController
    {
        private readonly IImportDtlRepository _repository;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ImportExcelController> _localizer;

        public ImportExcelController(IImportDtlRepository repository, IHostingEnvironment hostingEnv, IMapper mapper,
            IStringLocalizer<ImportExcelController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _hostingEnv = hostingEnv;
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

        public class ImportDtlVM
        {
            /// <summary>
            /// 主表ID
            /// </summary>
            public decimal ID { get; set; }

            /// <summary>
            /// 明细列表
            /// </summary>
            public List<ImportDtlListModel> ImportDtlList { get; set; }

            /// <summary>
            /// 是否存在模板
            /// </summary>
            public bool IsExistDemo { get; set; } = false;
        }

        /// <summary>
        /// 查询查询表信息集
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<ImportMst>>> LoadMainData([FromQuery]ImportMstRequestModel model)
        {
            ApiBaseReturn<List<ImportMst>> returnVM = new ApiBaseReturn<List<ImportMst>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(TOTABLE_NAME, :Key) > 0 or instr(DESC_CN, :Key) > 0) ";
                    }
                    if (!model.TOTABLE_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(TOTABLE_NAME, :TOTABLE_NAME) > 0) ";
                    }
                    if (!model.DESC_CN.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(DESC_CN, :DESC_CN) > 0) ";
                    }
                    if (!model.DESC_EN.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (instr(DESC_EN, :DESC_EN) > 0) ";
                    }

                    var list = (await _repository.GetListPagedEx<ImportMst>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();

                    count = await _repository.RecordCountAsyncEx<ImportMst>(conditions, model);

                    returnVM.Result = list?.ToList();
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
        /// 保存表信息数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveMainData([FromBody] ImportMstModel model)
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
                        decimal resdata = await _repository.SaveMainDataByTrans(model);
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
        /// 查询当前表对应的导入模板数据
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<ImportDtlVM>> LoadData(string table_name)
        {
            ApiBaseReturn<ImportDtlVM> returnVM = new ApiBaseReturn<ImportDtlVM>();
            decimal mst_id = 0;
            string desc_cn = string.Empty;
            List<ImportDtl> importDtlList = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && table_name.IsNullOrEmpty())
                    {
                        //throw new Exception("请传入表名称。");
                        ErrorInfo.Set(_localizer["table_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetImportMst(table_name);
                        if (resdata == null)
                        {
                            //string.Format("传入模板未定义, 请先定入需导入的模板主信息.")
                            ErrorInfo.Set(_localizer["import_mst_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            mst_id = resdata.ID;
                            desc_cn = resdata.DESC_CN;
                            importDtlList = (await _repository.GetListAsync("Where MST_ID =:MST_ID order by length(EXCEL_ITEM)，EXCEL_ITEM", new { MST_ID = resdata.ID }))?.ToList();
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {

                        
                        var viewList = new List<ImportDtlListModel>();
                        importDtlList?.ForEach(x =>
                        {
                            var item = _mapper.Map<ImportDtlListModel>(x);
                            //item.REFERENCE_SQL = string.Empty;
                            viewList.Add(item);
                        });

                        returnVM.Result = new ImportDtlVM
                        {
                            ID = mst_id,
                            ImportDtlList = viewList,
                        };

                        #region 判断模板是否存在

                        string sWebRootFolder = Path.Combine(AppContext.BaseDirectory, "upload", "exceltpl");
                        string sFileName = $@"{desc_cn}_模板.xlsx";
                        var path = Path.Combine(sWebRootFolder, sFileName);
                        if (System.IO.File.Exists(path) && Directory.Exists(sWebRootFolder))
                        {

                            returnVM.Result.IsExistDemo = true;
                        }

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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取导入表信息集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<ImportMst>>> GetMstTableList()
        {
            ApiBaseReturn<List<ImportMst>> returnVM = new ApiBaseReturn<List<ImportMst>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetImportMstList();
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
        /// 获取原始表对应的字段信息集
        /// </summary>
        /// <param name="table_name">表名称</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<List<ImportItemVM>> GetIntiTableColumnList(string table_name)
        {
            ApiBaseReturn<List<ImportItemVM>> returnVM = new ApiBaseReturn<List<ImportItemVM>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && table_name.IsNullOrEmpty())
                    {
                        //throw new Exception("请传入表名称。");
                        ErrorInfo.Set(_localizer["table_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = _repository.GetTemplateInfo(table_name);
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
        /// 保存模板数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] ImportDtlModel model)
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

        /// <summary>
        /// 导出Excel模板
        /// </summary>
        /// <param name="table_name">表名称</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> ExportTPL(string table_name)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            List<ImportDtl> importDtlList = null;
            string desc_cn = string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && table_name.IsNullOrEmpty())
                    {
                        //throw new Exception("请传入表名称。");
                        ErrorInfo.Set(_localizer["table_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetImportMst(table_name);
                        if (resdata == null)
                        {
                            //string.Format("传入模板未定义, 请先定入需导入的模板主信息.")
                            ErrorInfo.Set(_localizer["import_mst_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            desc_cn = resdata.DESC_CN;
                            importDtlList = (await _repository.GetListAsync("Where MST_ID =:MST_ID order by length(EXCEL_ITEM)，EXCEL_ITEM", new { MST_ID = resdata.ID }))?.ToList();
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (importDtlList != null && importDtlList.Count > 0)
                        {
                            returnVM.Result = EPPlusHelper.CreateExcelTemplate(importDtlList, desc_cn, _repository);
                            //return File(new FileStream(excelFilePath, FileMode.Open),"application/octet-stream","ExcelNameHere.xlsx");
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
        /// 导入Excel文件数据到指定表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ImportExcelFile([FromForm]string table_name)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            var excelFile = Request.Form.Files[0];
            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;
            var newFileName = string.Empty;
            string errmsg = string.Empty;
            List<ImportDtl> importDtlList = null;
            WebResponseContent content = null;
            List<ImportExcelItem> excelItem = null;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (excelFile == null || excelFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(excelFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        #region 判断后缀

                        if (!extname.ToLower().Contains("xlsx"))
                        {
                            //msg = "只允许上传xlsx格式的Excel文件."
                            ErrorInfo.Set(_localizer["file_suffix_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

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

                    if (!ErrorInfo.Status && table_name.IsNullOrEmpty())
                    {
                        //throw new Exception("请传入表名称。");
                        ErrorInfo.Set(_localizer["table_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetImportMst(table_name);
                        if (resdata == null)
                        {
                            //string.Format("传入模板未定义, 请先定入需导入的模板主信息.")
                            ErrorInfo.Set(_localizer["import_mst_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            importDtlList = (await _repository.GetListAsync("Where MST_ID =:MST_ID order by length(EXCEL_ITEM)，EXCEL_ITEM ", new { MST_ID = resdata.ID }))?.ToList();
                        }
                    }

                    #endregion

                    #region 解释Excel数据

                    if (!ErrorInfo.Status)
                    {
                        newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var pathRoot = AppContext.BaseDirectory + @"upload\exceldata\";
                        if (Directory.Exists(pathRoot) == false)
                        {
                            Directory.CreateDirectory(pathRoot);
                        }
                        filename = pathRoot + $"{newFileName}";
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            excelFile.CopyTo(fs);
                            fs.Flush();
                        }

                        content = EPPlusHelper.WriteToDataList(filename, importDtlList, _localizer);
                        if (!content.Status)
                        {
                            ErrorInfo.Set(content.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 校验Excel数据(唯一列,不为空列)

                    if (!ErrorInfo.Status && content.Data != null)
                    {
                        excelItem = content.Data;
                        //PropertyInfo[] propertyInfos = typeof(ImportExcelItem).GetProperties().ToArray();
                        //唯一列
                        var uniqueItems = importDtlList.Where(t => t.IS_UNIQUE == 1).ToList();
                        int index = -1; bool isExist = false;
                        foreach (var item in uniqueItems)
                        {
                            index = importDtlList.IndexOf(item);
                            string columnName = $"Column{index + 1}";

                            List<string> tmpList = new List<string>();
                            string val = string.Empty;
                            foreach (var exItem in excelItem)
                            {
                                val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                                if (!string.IsNullOrWhiteSpace(val))
                                {
                                    tmpList.Add(val);
                                }
                                else
                                {
                                    //"列{0}的值不能为空."
                                    errmsg = string.Format(_localizer["col_not_nullable"], columnName);
                                    ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    break;
                                }
                            }
                            var groups = tmpList.GroupBy(t => t).Where(t => t.Count() > 1).FirstOrDefault();
                            if (groups != null && !string.IsNullOrWhiteSpace(groups.ElementAt(0)))
                            {
                                //$"列{0}的值[{1}]不唯一."
                                errmsg = string.Format(_localizer["col_not_unique"], item.COLUMN_CAPTION, groups.ElementAt(0));
                                ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                break;
                            }
                        }

                        if (!ErrorInfo.Status)
                        {
                            foreach (var item in uniqueItems)
                            {
                                index = importDtlList.IndexOf(item);
                                string columnName = $"Column{index + 1}";

                                List<string> tmpList = new List<string>();
                                string val = string.Empty;
                                foreach (var exItem in excelItem)
                                {
                                    val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                                    isExist = await _repository.ItemIsExist(table_name, item.COLUMN_NAME, val);
                                    if (isExist)
                                    {
                                        //$"列{0}的值[{1}]已在数据库存在, 不唯一."
                                        errmsg = string.Format(_localizer["data_col_not_unique"], item.COLUMN_CAPTION, val);
                                        ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        break;
                                    }
                                }
                            }
                        }

                        //不为空项目校验
                        if (!ErrorInfo.Status)
                        {
                            var noNullItems = importDtlList.Where(t => t.ISNULL_ABLE == 0).ToList();
                            index = -1;
                            foreach (var item in noNullItems)
                            {
                                index = importDtlList.IndexOf(item);
                                string columnName = $"Column{index + 1}";

                                List<string> tmpList = new List<string>();
                                string val = string.Empty;
                                foreach (var exItem in excelItem)
                                {
                                    val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                                    if (string.IsNullOrWhiteSpace(val))
                                    {
                                        //"列{0}的值[{1)}]不能为空."
                                        errmsg = string.Format(_localizer["col_not_nullable"], item.COLUMN_CAPTION, val);
                                        ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region 保存数据并返回值

                    if (!ErrorInfo.Status && excelItem != null)
                    {
                        //保存
                        ImportResult resdata = await _repository.SaveImportExcelData(excelItem, importDtlList, table_name);
                        if (resdata.Result > 0)
                        {
                            returnVM.Result = resdata.Result > 0;
                        }
                        else if (resdata.Result <= 0 && resdata.ErrInfo != null)
                        {
                            //第{0}行，{1}没有找到相关信息。
                            errmsg = string.Format(_localizer["notfind_getval"], resdata.ErrInfo.Index, resdata.ErrInfo.COLUMN_CAPTION);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        /// 导入Excel模板
        /// </summary>
        /// <param name="table_name">表名称</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ImportExcelDemoFile([FromForm]string table_name)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            var excelFile = Request.Form.Files[0];
            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;
            var newFileName = string.Empty;
            string errmsg = string.Empty;
            List<ImportDtl> importDtlList = null;
            WebResponseContent content = null;
            string desc_cn = string.Empty;
            List<ImportExcelItem> excelItem = null;
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (excelFile == null || excelFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue.Parse(excelFile.ContentDisposition).FileName.Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        #region 判断后缀

                        if (!extname.ToLower().Contains("xlsx"))
                        {
                            //msg = "只允许上传xlsx格式的Excel文件."
                            ErrorInfo.Set(_localizer["file_suffix_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        #endregion

                        #region 判断大小 
                        filesize = Convert.ToDecimal(Math.Round(excelFile.Length / 1024.00, 3));
                        long mb = excelFile.Length / 1024 / 1024;//MB
                        if (mb > 10)
                        {
                            //"只允许上传小于 10MB 的文件."
                            ErrorInfo.Set(_localizer["size_10m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion

                    }

                    if (!ErrorInfo.Status && table_name.IsNullOrEmpty())
                    {
                        //throw new Exception("请传入表名称。");
                        ErrorInfo.Set(_localizer["table_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {

                        var resdata = await _repository.GetImportMst(table_name);
                        if (resdata == null)
                        {
                            //string.Format("传入模板未定义, 请先定入需导入的模板主信息.")
                            ErrorInfo.Set(_localizer["import_mst_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            desc_cn = resdata.DESC_CN;
                        }
                    }
                    #endregion

                    #region 保存文件

                    if (!ErrorInfo.Status)
                    {
                        string webPath = string.Empty;
                        string sWebRootFolder = Path.Combine(AppContext.BaseDirectory, "upload", "exceltpl");
                        if (!Directory.Exists(sWebRootFolder))
                        {
                            Directory.CreateDirectory(sWebRootFolder);
                        }
                        string sFileName = $@"{desc_cn}_up.xlsx";  //{DateTime.Now.ToString("yyyyMMddHHmm")} 
                        var path = Path.Combine(sWebRootFolder, sFileName);
                        webPath = $"/upload/exceltpl/{sFileName}";
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            file = new FileInfo(path);
                        }

                        using (FileStream fs = System.IO.File.Create(path))
                        {
                            excelFile.CopyTo(fs);
                            fs.Flush();
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

        /// <summary>
        /// 删除导入模板
        /// </summary>
        /// <param name="table_name">表名称</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DelExcel([FromBody] Table model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            string desc_cn = string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model.Table_Name.IsNullOrEmpty())
                    {
                        //throw new Exception("请传入表名称。");
                        ErrorInfo.Set(_localizer["table_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetImportMst(model.Table_Name);
                        if (resdata == null)
                        {
                            //string.Format("传入模板未定义, 请先定入需导入的模板主信息.")
                            ErrorInfo.Set(_localizer["import_mst_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            desc_cn = resdata.DESC_CN;
                        }
                    }
                    #endregion

                    #region 解释Excule数据
                    if (!ErrorInfo.Status && !String.IsNullOrWhiteSpace(model.Table_Name))
                    {
                        string sWebRootFolder = Path.Combine(AppContext.BaseDirectory, "upload", "exceltpl");
                        string sFileName = $@"{desc_cn}_up.xlsx";
                        var path = Path.Combine(sWebRootFolder, sFileName);
                        if (System.IO.File.Exists(path) && Directory.Exists(sWebRootFolder))
                        {
                            System.IO.File.Delete(path);
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

        /// <summary>
        /// 获取Excel的数据返回集合(某些导入除了要往主表查数据，还需往子表插数据，可以用这个接口返回Excel的数据进行操作)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<ImportExcelItem>>> GetExcelDataToList() 
        {

            ApiBaseReturn<List<ImportExcelItem>> returnVM = new ApiBaseReturn<List<ImportExcelItem>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                     
                    

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




        public class Table
        {
            public string Table_Name { get; set; } = null;
        }

    }
}