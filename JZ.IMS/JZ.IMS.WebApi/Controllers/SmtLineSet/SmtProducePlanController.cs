/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：生产计划表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-09-11 14:01:42                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtProducePlanController                                      
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
using System.Net.Http.Headers;
using System.IO;
using Aspose.Cells;
using System.Data;
using JZ.IMS.Core.Utilities;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// STM 生产计划 
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtProducePlanController : BaseController
    {
        private readonly ISmtProducePlanRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ShareResourceController> _localizer;
        private readonly ISmtLineConfigRepository _lineRepository;
        public SmtProducePlanController(ISmtProducePlanRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<ShareResourceController> localizer, ISmtLineConfigRepository lineRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _lineRepository = lineRepository;
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtProducePlanListModel>>> LoadData([FromQuery] SmtProducePlanRequestModel model)
        {
            ApiBaseReturn<List<SmtProducePlanListModel>> returnVM = new ApiBaseReturn<List<SmtProducePlanListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.LINE_ID > 0)
                    {
                        //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0)";
                        conditions += $" and ( LINE_ID = :LINE_ID) ";
                    }
                    if (!model.PLAN_DATE_BEGIN.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (PLAN_DATE >= to_date(:PLAN_DATE_BEGIN,'yyyy-mm-dd')) ";
                    }
                    if (!model.PLAN_DATE_END.IsNullOrWhiteSpace())
                    {
                        conditions += $"and (PLAN_DATE <= to_date(:PLAN_DATE_END,'yyyy-mm-dd')) ";  //'yyyy-mm-dd HH24:MI:SS' 
                    }

                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "ORDER_NO ", model)).ToList();
                    var viewList = new List<SmtProducePlanListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SmtProducePlanListModel>(x);
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
        ///  排产计划文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ImportAIPlanFile()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
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
                        var pathRoot = AppContext.BaseDirectory + @"upload\PlanFile\";
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
                        var res = await LoadPlanFile(save_filename);
                        if (res.Code == 0)
                        {
                            ErrorInfo.Set(res.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtProducePlanModel model)
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
        /// 添加或修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> AddOrModify()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = true;

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
        /// 删除
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status && id <= 0)
                    {
                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
                            ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var count = await _repository.DeleteAsync(id);
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                ResultCodeAddMsgKeys.CommonExceptionMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        /// 解释计划文件
        /// </summary>
        /// <param name="planFilePath"></param>
        /// <returns></returns>
        private async Task<ResultMsg> LoadPlanFile(string planFilePath)
        {
            ResultMsg result = new ResultMsg();
            object tmpValue;
            try
            {
                Workbook workbook = new Workbook(planFilePath);
                Worksheet sheet = workbook.Worksheets[0];

                string plandate1 = string.Empty; //B2
                string plandate2 = string.Empty; //K2
                string plandate3 = string.Empty; //T2
                string plandate4 = string.Empty; //AC2
                int startRow = 4;

                tmpValue = sheet.Cells["B2"].Value;
                if (tmpValue != null)
                    plandate1 = tmpValue.ToString().Trim();

                tmpValue = sheet.Cells["K2"].Value;
                if (tmpValue != null)
                    plandate2 = tmpValue.ToString().Trim();

                tmpValue = sheet.Cells["T2"].Value;
                if (tmpValue != null)
                    plandate3 = tmpValue.ToString().Trim();

                tmpValue = sheet.Cells["AC2"].Value;
                if (tmpValue != null)
                    plandate4 = tmpValue.ToString().Trim();

                List<SmtProducePlan> planlist = new List<SmtProducePlan>();

                DateTime tmp_date;
                //第一天排期
                if (DateTime.TryParse(plandate1, out tmp_date))
                {
                    await AddPlanDataForDay(startRow, sheet, planlist, plandate1, 0);
                }

                //第二天排期
                if (DateTime.TryParse(plandate2, out tmp_date))
                {
                    await AddPlanDataForDay(startRow, sheet, planlist, plandate2, 1);
                }

                //第三天排期
                if (DateTime.TryParse(plandate3, out tmp_date))
                {
                    await AddPlanDataForDay(startRow, sheet, planlist, plandate3, 2);
                }

                //第四天排期
                if (DateTime.TryParse(plandate4, out tmp_date))
                {
                    await AddPlanDataForDay(startRow, sheet, planlist, plandate4, 3);
                }

                if (planlist.Count > 0)
                {
                    await _repository.SaveDataByPlanFile(planlist);
                }
                else
                {
                    result.Set(_localizer["notfind_error"]);
                }
            }
            catch (Exception ex)
            {
                result.Set(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 导入Excel模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> ImportExcelDemoFile(int plan_type)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            var excelFile = Request.Form.Files[0];
            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;
            var newFileName = string.Empty;
            string errmsg = string.Empty;
            string desc_cn = string.Empty;
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

                        if (!extname.ToLower().Contains("xlsx") && !extname.ToLower().Contains("xls"))
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
                        string sFileName = "SmtProducePlanTPL" + extname;
                        if (plan_type == 1)
                        {
                            sFileName = "DIP生产计划导入模版" + extname;
                        }
                        if (plan_type == 0)
                        {
                            sFileName = "SMT生产计划导入模版" + extname;
                        }
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
        /// 导出计划Excel模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ApiBaseReturn<string> ExportTPL(int plan_type)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        string sWebRootFolder = Path.Combine(AppContext.BaseDirectory, "upload", "exceltpl");
                        if (!Directory.Exists(sWebRootFolder))
                        {
                            Directory.CreateDirectory(sWebRootFolder);
                        }
                        string upFileName = "SmtProducePlanTPL.xlsx";
                        string FileName = "SmtProducePlanTPL.xls";
                        if (plan_type == 1)
                        {
                            upFileName = "DIP生产计划导入模版.xlsx";
                            FileName = "DIP生产计划导入模版.xls";
                        }
                        if (plan_type == 0)
                        {
                            upFileName = "SMT生产计划导入模版.xlsx";
                            FileName = "SMT生产计划导入模版.xls";
                        }
                        var up_path = Path.Combine(sWebRootFolder, upFileName);
                        string webPath = $"/upload/exceltpl/{upFileName}";
                        FileInfo up_file = new FileInfo(up_path);
                        if (!up_file.Exists)
                        {
                            up_path = Path.Combine(sWebRootFolder, FileName);
                            webPath = $"/upload/exceltpl/{FileName}";
                            up_file = new FileInfo(up_path);
                            if (!up_file.Exists)
                            {
                                //直接返回上传的模板
                                ErrorInfo.Set("没有找到模板。", MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            }
                        }
                        //返回上传的模板
                        if (!ErrorInfo.Status)
                        {
                            returnVM.Result = webPath;
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
        /// 导出4天计划数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> ExportPlanDataToExcel([FromQuery] SmtProducePlanRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            string webfilePath = string.Empty;
            List<SmtProducePlan> planList = null;
            FileInfo up_file = null;
            string pathRoot = string.Empty;
            string newFileName = string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        string sWebRootFolder = Path.Combine(AppContext.BaseDirectory, "upload", "exceltpl");
                        if (!Directory.Exists(sWebRootFolder))
                        {
                            Directory.CreateDirectory(sWebRootFolder);
                        }
                        string upFileName = "SmtProducePlanTPL.xlsx"; //上传的模板
                        var up_path = Path.Combine(sWebRootFolder, upFileName);
                        string webPath = $"/upload/exceltpl/{upFileName}";
                        up_file = new FileInfo(up_path);
                        if (!up_file.Exists)
                        {
                            ErrorInfo.Set(_localizer["nofind_file"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (model.PLAN_DATE_BEGIN.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["begdate_nonull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    if (!ErrorInfo.Status)
                    {
                        //查询数据
                        string conditions = " WHERE ID > 0 ";
                        if (model.LINE_ID > 0)
                        {
                            //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0)";
                            conditions += $" and ( LINE_ID = :LINE_ID) ";
                        }
                        if (!model.PLAN_DATE_BEGIN.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (PLAN_DATE >= to_date(:PLAN_DATE_BEGIN,'yyyy-mm-dd')) ";
                        }
                        if (!model.PLAN_DATE_END.IsNullOrWhiteSpace())
                        {
                            conditions += $"and (PLAN_DATE <= to_date(:PLAN_DATE_END,'yyyy-mm-dd')) ";  //'yyyy-mm-dd HH24:MI:SS' 
                        }
                        planList = (await _repository.GetListPagedAsync(model.Page, 20000, conditions, "ORDER_NO ", model)).ToList();
                    }

                    if (!ErrorInfo.Status && planList?.Count > 0 && up_file != null)
                    {
                        newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + ".xlsx";
                        pathRoot = AppContext.BaseDirectory + @"upload\PlanFile\";
                        if (Directory.Exists(pathRoot) == false)
                        {
                            Directory.CreateDirectory(pathRoot);
                        }

                        if (up_file.Exists)
                        {
                            up_file.CopyTo(pathRoot + newFileName, true);
                        }
                        webfilePath = $"/upload/PlanFile/{newFileName}";
                    }

                    if (!ErrorInfo.Status && !newFileName.IsNullOrWhiteSpace())
                    {
                        //写入数据
                        await Save2PlanFile(pathRoot + newFileName, model.PLAN_DATE_BEGIN, planList);
                    }

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = webfilePath;
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
        ///  SMT排产计划文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ResultVM>> ImportSMTPlanFile()
        {
            ApiBaseReturn<ResultVM> returnVM = new ApiBaseReturn<ResultVM>();
            ResultVM resultVM = new ResultVM();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
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
                        var pathRoot = AppContext.BaseDirectory + @"upload\PlanFile\";
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
                        var res = await LoadPlanFile(save_filename, 0, resultVM);
                        if (res.Code == 0)
                        {
                            ErrorInfo.Set(res.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = resultVM;
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
        ///  DIP排产计划文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ResultVM>> ImportDIPPlanFile()
        {
            ApiBaseReturn<ResultVM> returnVM = new ApiBaseReturn<ResultVM>();
            ResultVM resultVM = new ResultVM();
            IFormFile excelFile = null;
            var save_filename = string.Empty;
            var source_filename = string.Empty;
            var extname = string.Empty;
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
                        var pathRoot = AppContext.BaseDirectory + @"upload\PlanFile\";
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
                        var res = await LoadDIPPlanFile(save_filename, resultVM);
                        if (res.Code == 0)
                        {
                            ErrorInfo.Set(res.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    returnVM.Result = resultVM;

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

        #region 内部方法

        /// <summary>
        /// 增加哪一天的计划
        /// </summary>
        /// <param name="startRow">开始行数</param>
        /// <param name="sheet"></param>
        /// <param name="planlist">返回列表</param>
        /// <param name="plandate">计划日期</param>
        /// <param name="dayidx">第几天的序号-1 ，第一天： 0 </param>
        /// <returns></returns>
        private async Task AddPlanDataForDay(int startRow, Worksheet sheet, List<SmtProducePlan> planlist, string plandate, int dayidx = 0)
        {
            string line = string.Empty;
            decimal order_quantity = 0;  //订单量 
            decimal plan_quantity = 0;  //排产量 
            decimal line_id = 0;  //线体ID 
            string wo_no = "";   //工单号

            for (int i = startRow; i < sheet.Cells.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.Cells[i, 0].Value)))
                {
                    line = Convert.ToString(sheet.Cells[i, 0].Value).Trim();
                }
                decimal.TryParse(Convert.ToString(sheet.Cells[i, 6 + 9 * dayidx].Value), out plan_quantity);
                wo_no = Convert.ToString(sheet.Cells[i, 8 + 9 * dayidx].Value);
                if (plan_quantity > 0 && !string.IsNullOrWhiteSpace(wo_no) && !string.IsNullOrWhiteSpace(plandate))
                {
                    decimal.TryParse(Convert.ToString(sheet.Cells[i, 5 + 9 * dayidx].Value), out order_quantity);
                    line_id = await _repository.GetLineByName(string.Format("SZSMT{0}", line.Replace("线", "")));  //SZSMT1 
                    if (line_id > -1)
                    {
                        planlist.Add(new SmtProducePlan
                        {
                            PLAN_DATE = Convert.ToDateTime(plandate),
                            LINE_ID = line_id,
                            ORDER_NO = Convert.ToString(sheet.Cells[i, 1 + 9 * dayidx].Value),
                            MOVEMENT = Convert.ToString(sheet.Cells[i, 2 + 9 * dayidx].Value),
                            MACHINE_TYPE = Convert.ToString(sheet.Cells[i, 3 + 9 * dayidx].Value),
                            TYPE_ID = Convert.ToString(sheet.Cells[i, 4 + 9 * dayidx].Value),
                            ORDER_QUANTITY = order_quantity,
                            PLAN_QUANTITY = plan_quantity,
                            NATIONALITY = Convert.ToString(sheet.Cells[i, 7 + 9 * dayidx].Value),
                            WO_NO = Convert.ToString(sheet.Cells[i, 8 + 9 * dayidx].Value),
                            DESCRIPTION = Convert.ToString(sheet.Cells[i, 9 + 9 * dayidx].Value),
                            CREATE_DATE = DateTime.Now.ToString(),
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 返回MSG
        /// </summary>
        private class ResultMsg
        {
            /// <summary>
            /// 
            /// </summary>
            public int Code { get; set; } = 1;

            /// <summary>
            /// 
            /// </summary>
            public string ErrorMessage { get; set; }

            public void Set(string msg)
            {
                Code = 0;
                ErrorMessage = msg;
            }
        }

        /// <summary>
        /// 写入计划文件
        /// </summary>
        /// <param name="planFilePath"></param>
        /// <param name="beg_date"></param>
        /// <param name="planList"></param>
        /// <returns></returns>
        private async Task Save2PlanFile(string planFilePath, string beg_date, List<SmtProducePlan> planList)
        {
            ResultMsg result = new ResultMsg();
            try
            {
                Workbook workbook = new Workbook(planFilePath);
                Worksheet sheet = workbook.Worksheets[0];

                DateTime plandate1 = Convert.ToDateTime(beg_date).Date; //B2
                DateTime plandate2 = plandate1.AddDays(1); //K2
                DateTime plandate3 = plandate1.AddDays(2); //T2
                DateTime plandate4 = plandate1.AddDays(3); //AC2
                int startRow = 4;

                Cell itemCell = sheet.Cells["B2"];
                itemCell.PutValue(plandate1);

                itemCell = sheet.Cells["K2"];
                itemCell.PutValue(plandate2);

                itemCell = sheet.Cells["T2"];
                itemCell.PutValue(plandate3);

                itemCell = sheet.Cells["AC2"];
                itemCell.PutValue(plandate4);

                List<SmtProducePlan> des_list = null;
                //第一天排期
                if (planList.Where(t => t.PLAN_DATE == plandate1).Count() > 0)
                {
                    des_list = planList.Where(t => t.PLAN_DATE == plandate1).ToList();
                    await WritePlanData2Day(startRow, sheet, des_list, 0);
                }

                //第二天排期
                if (planList.Where(t => t.PLAN_DATE == plandate2).Count() > 0)
                {
                    des_list = planList.Where(t => t.PLAN_DATE == plandate2).ToList();
                    await WritePlanData2Day(startRow, sheet, des_list, 1);
                }

                //第三天排期
                if (planList.Where(t => t.PLAN_DATE == plandate3).Count() > 0)
                {
                    des_list = planList.Where(t => t.PLAN_DATE == plandate3).ToList();
                    await WritePlanData2Day(startRow, sheet, des_list, 2);
                }

                //第四天排期
                if (planList.Where(t => t.PLAN_DATE == plandate4).Count() > 0)
                {
                    des_list = planList.Where(t => t.PLAN_DATE == plandate4).ToList();
                    await WritePlanData2Day(startRow, sheet, des_list, 3);
                }

                workbook.Save(planFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 解释计划文件
        /// </summary>
        /// <param name="planFilePath"></param>
        /// <param name="plan_type">计划类型(计划类型：0:SMT, 1:DIP)</param>
        /// <param name="resultVM">返回导入信息列表</param>
        /// <returns></returns>
        private async Task<ResultMsg> LoadPlanFile(string planFilePath, decimal plan_type, ResultVM resultVM)
        {
            ResultMsg result = new ResultMsg();
            object tmpValue;
            try
            {
                Workbook workbook = new Workbook(planFilePath);
                Worksheet sheet = workbook.Worksheets[0];

                string plandate1 = string.Empty; //B2
                string plandate2 = string.Empty; //K2
                string plandate3 = string.Empty; //T2
                string plandate4 = string.Empty; //AC2
                int startRow = 3;

                tmpValue = sheet.Cells["B2"].Value;
                if (tmpValue != null)
                    plandate1 = tmpValue.ToString().Trim();

                tmpValue = sheet.Cells["J2"].Value;
                if (tmpValue != null)
                    plandate2 = tmpValue.ToString().Trim();

                tmpValue = sheet.Cells["R2"].Value;
                if (tmpValue != null)
                    plandate3 = tmpValue.ToString().Trim();

                tmpValue = sheet.Cells["Z2"].Value;
                if (tmpValue != null)
                    plandate4 = tmpValue.ToString().Trim();

                List<SmtProducePlan> planlist = new List<SmtProducePlan>();

                DateTime tmp_date;
                //第一天排期
                if (DateTime.TryParse(plandate1, out tmp_date))
                {
                    await AddPlanDataForDay(startRow, sheet, planlist, plandate1, plan_type, 0, resultVM);
                }

                //第二天排期
                if (DateTime.TryParse(plandate2, out tmp_date))
                {
                    await AddPlanDataForDay(startRow, sheet, planlist, plandate2, plan_type, 1, resultVM);
                }

                //第三天排期
                if (DateTime.TryParse(plandate3, out tmp_date))
                {
                    await AddPlanDataForDay(startRow, sheet, planlist, plandate3, plan_type, 2, resultVM);
                }

                //第四天排期
                if (DateTime.TryParse(plandate4, out tmp_date))
                {
                    await AddPlanDataForDay(startRow, sheet, planlist, plandate4, plan_type, 3, resultVM);
                }

                if (planlist.Count > 0)
                {
                    await _repository.SaveDataByPlanFile(planlist);
                }
                else
                {
                    result.Set(_localizer["notfind_error"]);
                }
            }
            catch (Exception ex)
            {
                result.Set(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 增加哪一天的计划
        /// </summary>
        /// <param name="startRow">开始行数</param>
        /// <param name="sheet"></param>
        /// <param name="planlist">返回列表</param>
        /// <param name="plandate">计划日期</param>
        /// <param name="plan_type">计划类型(计划类型：0:SMT, 1:DIP)</param>
        /// <param name="dayidx">第几天的序号-1 ，第一天： 0 </param>
        /// <param name="resultVM">返回导入信息列表</param>
        /// <returns></returns>
        private async Task AddPlanDataForDay(int startRow, Worksheet sheet, List<SmtProducePlan> planlist, string plandate, decimal plan_type,
            int dayidx, ResultVM resultVM)
        {
            string line = string.Empty;
            decimal order_quantity = 0;  //订单量 
            decimal plan_quantity = 0;  //排产量 
            decimal line_id = 0;  //线体ID 
            string wo_no = "";   //工单号
            string linename;
            for (int i = startRow; i < sheet.Cells.Rows.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.Cells[i, 0].Value)))
                {
                    line = Convert.ToString(sheet.Cells[i, 0].Value).Trim();
                }
                decimal.TryParse(Convert.ToString(sheet.Cells[i, 6 + 8 * dayidx].Value), out plan_quantity);
                wo_no = Convert.ToString(sheet.Cells[i, 3 + 8 * dayidx].Value);
                if (plan_quantity > 0 && !string.IsNullOrWhiteSpace(wo_no) && !string.IsNullOrWhiteSpace(plandate))
                {
                    decimal.TryParse(Convert.ToString(sheet.Cells[i, 5 + 8 * dayidx].Value), out order_quantity);
                    //linename = plan_type == 0 ? string.Format("SZSMT{0}", line.Replace("线", "")) : string.Format("SZDIP{0}", line.Replace("线", ""));
                    line_id = await _repository.GetLineByName(line, plan_type);  //SZSMT1 
                    if (line_id > -1)
                    {
                        planlist.Add(new SmtProducePlan
                        {
                            PLAN_TYPE = plan_type,
                            PLAN_DATE = Convert.ToDateTime(plandate),
                            LINE_ID = line_id,
                            ORDER_NO = Convert.ToString(sheet.Cells[i, 1 + 8 * dayidx].Value),
                            //MOVEMENT = Convert.ToString(sheet.Cells[i, 2 + 9 * dayidx].Value),
                            MACHINE_TYPE = Convert.ToString(sheet.Cells[i, 2 + 8 * dayidx].Value),//机型
                            WO_NO = Convert.ToString(sheet.Cells[i, 3 + 8 * dayidx].Value),                 //工单
                            //TYPE_ID = Convert.ToString(sheet.Cells[i, 4 + 9 * dayidx].Value),   //类型
                            ORDER_QUANTITY = order_quantity,                                    //订单量
                            PLAN_QUANTITY = plan_quantity,                                                  //排产量
                            //NATIONALITY = Convert.ToString(sheet.Cells[i, 7 + 9 * dayidx].Value),           //国家
                            DESCRIPTION = Convert.ToString(sheet.Cells[i, 7 + 8 * dayidx].Value),           //备注
                            CREATE_DATE = DateTime.Now.ToString(),                                          //创建时间
                        });
                    }
                    else
                    {
                        //系统没有找到名称为:[{0}] 的产线.
                        string errmsg = string.Format(_localizer["nofind_linename"], line);
                        if (resultVM.MessagList.IndexOf(errmsg) == -1)
                            resultVM.MessagList.Add(errmsg);
                    }
                }
            }
        }

        /// <summary>
        /// 写入哪一天的计划
        /// </summary>
        /// <param name="startRow">开始行数</param>
        /// <param name="sheet"></param>
        /// <param name="planlist">返回列表</param>
        /// <param name="dayidx">第几天的序号-1 ，第一天： 0 </param>
        /// <returns></returns>
        private async Task WritePlanData2Day(int startRow, Worksheet sheet, List<SmtProducePlan> planlist, int dayidx = 0)
        {
            string line = string.Empty;
            List<IDNAME> lineList = await _lineRepository.GetLineList();

            var query = from pl in planlist
                        join ln in lineList on pl.LINE_ID.ToString() equals ln.ID
                        select new
                        {
                            pl.ORDER_NO,
                            pl.MOVEMENT,
                            pl.MACHINE_TYPE,
                            pl.TYPE_ID,
                            pl.ORDER_QUANTITY,
                            pl.PLAN_QUANTITY,
                            pl.NATIONALITY,
                            pl.WO_NO,
                            pl.DESCRIPTION,
                            Line_OrderBy = ln.NAME.Replace("SZSMT", "").ToString().PadLeft(2, '0'),
                            Line_Name = String.Concat(ln.NAME.Replace("SZSMT", ""), "线"),
                        };
            var querylst = query.OrderBy(t => t.Line_OrderBy).ThenBy(t => t.ORDER_NO).ToList();

            for (int i = 0; i < querylst.Count(); i++)
            {
                line = querylst[i].Line_Name ?? string.Empty;
                //if (!line.IsNullOrWhiteSpace())
                //{
                //    line = line.Replace("SZSMT", "") + "线";
                //}
                if (string.IsNullOrWhiteSpace(Convert.ToString(sheet.Cells[startRow + i, 0].Value)))
                {
                    sheet.Cells[startRow + i, 0].PutValue(line);
                }

                sheet.Cells[startRow + i, 1 + 9 * dayidx].PutValue(querylst[i].ORDER_NO);
                sheet.Cells[startRow + i, 2 + 9 * dayidx].PutValue(querylst[i].MOVEMENT);
                sheet.Cells[startRow + i, 3 + 9 * dayidx].PutValue(querylst[i].MACHINE_TYPE);
                sheet.Cells[startRow + i, 4 + 9 * dayidx].PutValue(querylst[i].TYPE_ID);

                sheet.Cells[startRow + i, 5 + 9 * dayidx].PutValue(querylst[i].ORDER_QUANTITY);
                sheet.Cells[startRow + i, 6 + 9 * dayidx].PutValue(querylst[i].PLAN_QUANTITY);
                sheet.Cells[startRow + i, 7 + 9 * dayidx].PutValue(querylst[i].NATIONALITY);
                sheet.Cells[startRow + i, 8 + 9 * dayidx].PutValue(querylst[i].WO_NO);
                sheet.Cells[startRow + i, 9 + 9 * dayidx].PutValue(querylst[i].DESCRIPTION);
            }
        }

        /// <summary>
        /// 解释DIP计划文件
        /// </summary>
        /// <param name="planFilePath"></param>
        /// <param name="resultVM">返回导入信息列表</param>
        /// <returns></returns>
        private async Task<ResultMsg> LoadDIPPlanFile(string planFilePath, ResultVM resultVM)
        {
            ResultMsg result = new ResultMsg();
            try
            {
                Workbook workbook = new Workbook(planFilePath);
                Worksheet sheet = workbook.Worksheets[0];
                List<SmtProducePlan> planlist = new List<SmtProducePlan>();

                decimal plan_quantity = 0;  //排产量 
                decimal line_id = 0;  //线体ID 
                string wo_no = "";   //工单号
                string linename = string.Empty;
                DateTime plan_date;
                for (int i = 1; i < sheet.Cells.Rows.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.Cells[i, 0].Value)))
                    {
                        linename = Convert.ToString(sheet.Cells[i, 0].Value).Trim();
                    }
                    decimal.TryParse(Convert.ToString(sheet.Cells[i, 7].Value), out plan_quantity);
                    wo_no = Convert.ToString(sheet.Cells[i, 2].Value);
                    if (plan_quantity > 0 && !string.IsNullOrWhiteSpace(wo_no))
                    {
                        line_id = await _repository.GetLineByName(linename, 1);
                        if (line_id > -1)
                        {
                            try
                            {
                                plan_date = DateTime.ParseExact(Convert.ToString(sheet.Cells[i, 1].Value), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex)
                            {
                                if (resultVM.MessagList.IndexOf(ex.Message) == -1)
                                    resultVM.MessagList.Add(ex.Message);
                                continue;
                            }

                            planlist.Add(new SmtProducePlan
                            {
                                PLAN_TYPE = 1,
                                PLAN_DATE = plan_date,
                                LINE_ID = line_id,
                                ORDER_NO = Convert.ToString(sheet.Cells[i, 3].Value),
                                MACHINE_TYPE = Convert.ToString(sheet.Cells[i, 4].Value),
                                MOVEMENT = Convert.ToString(sheet.Cells[i, 5].Value),
                                TYPE_ID = Convert.ToString(sheet.Cells[i, 6].Value),
                                ORDER_QUANTITY = null,
                                PLAN_QUANTITY = plan_quantity,
                                NATIONALITY = string.Empty,
                                WO_NO = wo_no,
                                WO_SURPLUS = Convert.ToString(sheet.Cells[i, 8].Value),
                                DELIVERY_DATE = Convert.ToString(sheet.Cells[i, 9].Value),
                                DESCRIPTION = Convert.ToString(sheet.Cells[i, 10].Value),
                                CUSTOMER_WO = Convert.ToString(sheet.Cells[i, 11].Value),
                                CUSTOMER_BOM = Convert.ToString(sheet.Cells[i, 12].Value),
                                CREATE_DATE = DateTime.Now.ToString(),
                            });
                        }
                        else
                        {
                            //系统没有找到名称为:[{0}] 的产线.
                            string errmsg = string.Format(_localizer["nofind_linename"], linename);
                            if (resultVM.MessagList.IndexOf(errmsg) == -1)
                                resultVM.MessagList.Add(errmsg);
                        }
                    }
                }

                if (planlist.Count > 0)
                {
                    await _repository.SaveDataByPlanFile(planlist);
                }
                else
                {
                    result.Set(_localizer["notfind_error"]);
                }
            }
            catch (Exception ex)
            {
                result.Set(ex.Message);
            }

            return result;
        }

        #endregion
    }
}