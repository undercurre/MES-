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
using JZ.IMS.Core.Extensions;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Localization;
using JZ.IMS.Models;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Validation;
using JZ.IMS.ViewModels.MesTongs;
using JZ.IMS.IRepository;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 设备信息控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsEquipmentController : BaseController
    {
        private readonly IStringLocalizer<MenuController> _menu_localizer;
        private readonly IStringLocalizer<SfcsEquipmentController> _localizer;
        private readonly ISfcsEquipmentService _service;
        private readonly ISfcsEquipmentRepository _repository;
        private readonly ISfcsEquipmentLinesService _serviceLines;
        private readonly ISfcsParametersService _serviceParameters;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SfcsEquipmentController(IStringLocalizer<MenuController> menu_localizer, ISfcsEquipmentService service,
            ISfcsEquipmentLinesService serviceLines, ISfcsEquipmentRepository repository, ISfcsParametersService serviceParameters, IHostingEnvironment hostingEnv,
            IHttpContextAccessor httpContextAccessor, IStringLocalizer<SfcsEquipmentController> localizer)
        {
            _menu_localizer = menu_localizer;
            _localizer = localizer;
            _service = service;
            _serviceLines = serviceLines;
            _serviceParameters = serviceParameters;
            _hostingEnv = hostingEnv;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        /// <summary>
        /// 设备信息首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<IndexResult> Index()
        {
            ApiBaseReturn<IndexResult> returnVM = new ApiBaseReturn<IndexResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new IndexResult()
                        {
                            CategoryList = _serviceParameters.GetEquipmentCategoryList(),
                            LinesList = _serviceLines.GetLinesList(),
                        };
                        returnVM.TotalCount = 1;
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
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadData([FromQuery] SfcsEquipmentRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetEquipmentList(model);
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(resdata.data);
                    returnVM.TotalCount = resdata.count;

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
        /// 导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery] SfcsEquipmentRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _service.GetExportData(model);
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;

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
        public ApiBaseReturn<DetailResult> AddOrModify()
        {
            ApiBaseReturn<DetailResult> returnVM = new ApiBaseReturn<DetailResult>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = new DetailResult()
                    {
                        CategoryList = _serviceParameters.GetEquipmentCategoryList(),
                        LinesList = _serviceLines.GetLinesList(),
                        DepartmentList = _serviceParameters.GetDepartmentList(),
                    };
                    returnVM.TotalCount = 1;

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
        /// 保存数据
        /// </summary>
        /// <param name="item">保存数据模型</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody] SfcsEquipmentAddOrModifyModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.AddOrModifyAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (!msg.IsNullOrWhiteSpace() && msg.IndexOf("UNIQUE_EQUIPMENT_PRODUCT_NO") != -1)
                    {
                        ErrorInfo.Set(_localizer["UNIQUE_EQUIPMENT_PRODUCT_NO"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else
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
        /// <param name="id">设备ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.DeleteAsync(id);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 通过ID更改激活状态
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ChangeEnabled([FromBody] ChangeStatusModel item)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status)
                    {
                        ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_menu_localizer);
                        ValidationResult results = validationRules.Validate(item);
                        if (!results.IsValid)
                        {
                            ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 更改激活状态并返回

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.ChangeEnableStatusAsync(item);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 图片上传功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ApiBaseReturn<PicInfo> UploadImage([FromForm] string mst_id, [FromForm] decimal resource_id)
        {
            ApiBaseReturn<PicInfo> returnVM = new ApiBaseReturn<PicInfo>();
            var imgFile = Request.Form.Files[0];
            var resource_name = string.Empty;
            var filename = string.Empty;
            var extname = string.Empty;
            long size = 0;
            decimal filesize = 0;
            var newFileName = string.Empty;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (imgFile == null || imgFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(imgFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        resource_name = filename;

                        #region 判断后缀

                        //if (!extname.ToLower().Contains("jpg") && !extname.ToLower().Contains("png") && !extname.ToLower().Contains("gif"))
                        //{
                        //   return Json(new { code = 1, msg = "只允许上传jpg,png,gif格式的图片.", });
                        //}

                        #endregion

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(imgFile.Length / 1024.00, 3));
                        long mb = imgFile.Length / 1024 / 1024; // MB
                        if (mb > 20)
                        {
                            //"只允许上传小于 20MB 的图片."
                            ErrorInfo.Set(_localizer["upload_size_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion
                    }

                    #endregion

                    #region 保存文件并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var path = $"/upload/sfcsFile/"; // + DateTime.Now.ToString("yyyyMM");
                        string dir = @"upload\sfcsFile\";
                        var pathWebRoot = AppContext.BaseDirectory + dir;
                        if (Directory.Exists(pathWebRoot) == false)
                        {
                            Directory.CreateDirectory(pathWebRoot);
                        }
                        filename = pathWebRoot + $"{newFileName}";

                        size += imgFile.Length;
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            imgFile.CopyTo(fs);
                            fs.Flush();
                        }

                        //资源信息
                        var res_data = new PicInfo
                        {
                            mst_id = mst_id,
                            ImgUrl = path + $"{newFileName}",
                            Img_Resource_id = 0,
                            ImgName = resource_name,
                            ImgSize = filesize,
                        };

                        returnVM.Result = res_data;
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
        /// 根据设备条件获取设备信息
        /// </summary>
        /// <param name="model">设备条件对象</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetEquipmentList(SfcsEquipmentRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _service.GetEquipmentList(model);
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(resdata);

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
		/// 根据设备唯一码获取设备信息
		/// </summary>
		/// <param name="ONLY_CODE">设备唯一编码</param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetEquipment(string ONLY_CODE)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = _service.GetEquipment(ONLY_CODE);
                    returnVM.Result = JsonHelper.ObjectToJSONOfDate(resdata);

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

        #region 设备盘点
        /// <summary>
        /// 保存PDA设备盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<EquipmentInfoInnerKeepDetail>>> SavePDAEquipmentCheckData([FromBody] SaveEquipmentCheckDataRequestModel model)
        {
            SfcsEquipment equipment = null;
            SfcsEquipmentKeepHead head = null;
            SfcsEquipmentKeepDetail detail = null;
            List<EquipmentInfoInnerKeepDetail> fList = null;
            ApiBaseReturn<List<EquipmentInfoInnerKeepDetail>> returnVM = new ApiBaseReturn<List<EquipmentInfoInnerKeepDetail>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 获取所在组织架构
                    //Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    //if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    //String organize_id = "";
                    //List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    //if (idList != null && idList.Count() > 0)
                    //{
                    //    organize_id = String.Join(",", idList);
                    //}
                    //else
                    //{
                    //    throw new Exception("ORGANIZE_INFO_EMPTY");
                    //}
                    #endregion

                    if (!model.CHECK_CODE.IsNullOrEmpty() && model.EQUIPMENT_BODYMARK.IsNullOrEmpty())
                    {
                        head = (await _repository.GetListByTableEX<SfcsEquipmentKeepHead>("*", "SFCS_EQUIPMENT_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        if (!head.IsNullOrWhiteSpace())
                        {
                            returnVM.Result = await _repository.GetPDAEquipmentCheckDataByHeadID(head.ID);
                        }
                        else
                        {
                            returnVM.Result = null;
                        }
                    }
                    else
                    {
                        if (model.EQUIPMENT_BODYMARK.IsNullOrEmpty()&& !ErrorInfo.Status)
                        {
                            //设备编号不能为空
                            ErrorInfo.Set(_localizer["EQUIPMENT_BODYMARK_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            equipment = (await _repository.GetListByTableEX<SfcsEquipment>("*", "SFCS_EQUIPMENT", " AND NAME=:CODE ", new { CODE = model.EQUIPMENT_BODYMARK })).FirstOrDefault();
                            if (equipment.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (equipment.STATUS == 6)
                            {
                                //报废
                                ErrorInfo.Set(_localizer["EQUIPMENT_STATUS_6_NOT_CHECK"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (model.CHECK_CODE.IsNullOrEmpty())
                                {
                                    String check_code = _repository.QueryEx<String>("SELECT CHECK_CODE FROM SFCS_EQUIPMENT_KEEP_HEAD WHERE CHECK_STATUS != 2  ORDER BY ID ASC").FirstOrDefault();
                                    if (!String.IsNullOrEmpty(check_code)) { model.CHECK_CODE = check_code; }
                                    //fList = await _repository.GetPDATongsCheckDataByHeadID( organize_id);
                                }
                            }
                        }
                        if (!model.CHECK_CODE.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            //检查编号是否存在
                            head = (await _repository.GetListByTableEX<SfcsEquipmentKeepHead>("*", "SFCS_EQUIPMENT_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                            if (head.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (head.CHECK_STATUS != 0)
                            {
                                ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (head.ID > 0)
                                {
                                    SfcsEquipmentKeepDetail content = (await _repository.GetListByTableEX<SfcsEquipmentKeepDetail>("*", "SFCS_EQUIPMENT_KEEP_DETAIL", " AND KEEP_HEAD_ID=:KEEP_HEAD_ID AND EQUIPMENT_ID=:EQUIPMENT_ID AND EQUIPMENT_STATUS=1", new { KEEP_HEAD_ID = head.ID, EQUIPMENT_ID = equipment.ID })).FirstOrDefault();
                                    if (!content.IsNullOrWhiteSpace())
                                    {
                                        ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_REPEAT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                            }
                        }

                        int headId = 0;
                        if (!ErrorInfo.Status)
                        {
                            headId = await _repository.SavePDAEquipmentCheckData(model, equipment, head, detail);
                        }
                        if (head == null && !model.CHECK_CODE.IsNullOrEmpty())
                        {
                            head = (await _repository.GetListByTableEX<SfcsEquipmentKeepHead>("*", "SFCS_EQUIPMENT_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        }
                        if (head != null || headId > 1)
                        {
                            headId = headId > 1 ? headId : Convert.ToInt32(head.ID);
                            returnVM.Result = await _repository.GetPDAEquipmentCheckDataByHeadID( headId);
                        }
                    }
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
        /// 删除PDA设备盘点数据记录
        /// </summary>
        /// <param name="check_code">设备点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DeletePDAEquipmentCheckData(String check_code)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    if (String.IsNullOrEmpty(check_code))
                    {
                        throw new Exception("EQUIPMENT_CODE_INFO_NULL");
                    }
                    else
                    {
                        //只能能删除新增和未审核状态下的盘点单
                        SfcsEquipmentKeepHead head = (await _repository.GetListByTableEX<SfcsEquipmentKeepHead>("*", "SFCS_EQUIPMENT_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = check_code })).FirstOrDefault();
                        if (head == null)
                        {
                            throw new Exception("EQUIPMENT_CODE_INFO_NULL");
                        }
                        else if (head.CHECK_STATUS == 0 || head.CHECK_STATUS == 1)
                        {
                            returnVM.Result = await _repository.DeletePDAEquipmentCheckData(head.ID);
                        }
                        else
                        {
                            throw new Exception("CHECK_STATUS_NOT_INSERT");
                        }

                    }
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 确认PDA设备盘点数据
        /// </summary>
        /// <param name="check_code">设备点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> AuditEquipmentCheckData([FromBody] AuditEquipmentCheckDataRequestModel model)
        {
            SfcsEquipmentKeepHead head = null;
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (model.ID > 0 && !ErrorInfo.Status)
                    {
                        //检查编号是否存在
                        head = (await _repository.GetListByTableEX<SfcsEquipmentKeepHead>("*", "SFCS_EQUIPMENT_KEEP_HEAD", " AND ID=:ID", new { ID = model.ID })).FirstOrDefault();
                        if (head.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.STATUS == 1 && !ErrorInfo.Status)
                    {
                        //确认审核必须要新增
                        if (head.CHECK_STATUS != 0)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else if (model.STATUS == 2 && !ErrorInfo.Status)
                    {
                        //审核必须要未审核
                        if (head.CHECK_STATUS != 1)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_1"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_INSERT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.ConfirmPDAEquipmentCheckData(model);
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
        /// PDA设备盘点列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<EquipmentCheckListModel>>> LoadPDAEquipmentCheckList([FromQuery] EquipmentCheckRequestModel model)
        {

            ApiBaseReturn<List<EquipmentCheckListModel>> returnVM = new ApiBaseReturn<List<EquipmentCheckListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    //Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    //if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    //String organize_id = "";
                    //List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    //if (idList != null && idList.Count() > 0)
                    //{
                    //    organize_id = String.Join(",", idList);
                    //}
                    //else
                    //{
                    //    throw new Exception("ORGANIZE_INFO_EMPTY");
                    //}
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.LoadPDAEquipmentCheckList(model);
                        returnVM.TotalCount = await _repository.LoadPDAEquipmentCheckListCount(model);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取需要点检的设备类型列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<EquipmentInfoInnerKeepDetail>>> LoadPDAEquipmentCheckInfo([FromQuery] GetEquipmentInfoRequestModel model)
        {
            ApiBaseReturn<List<EquipmentInfoInnerKeepDetail>> returnVM = new ApiBaseReturn<List<EquipmentInfoInnerKeepDetail>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    #region 组织架构
                    //Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    //if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    //String organize_id = "";
                    //List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    //if (idList != null && idList.Count() > 0)
                    //{
                    //    organize_id = String.Join(",", idList);
                    //}
                    //else
                    //{
                    //    throw new Exception("ORGANIZE_INFO_EMPTY");
                    //} 
                    #endregion

                    returnVM.Result = await _repository.GetPDAEquipmentCheckDataByHeadID();

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;

        }

        #endregion

        #region 工装验证

        /// <summary>
        /// 保存PDA设备验证数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<EquipmentInfoInnerValidationDetail>>> SavePDAEquipmentValidationData([FromBody] SaveEquipmentValidationDataRequestModel model)
        {
            SfcsEquipment EQUIPMENT = null;
            SfcsEquipmentValidationHead head = null;
            SfcsEquipValidationDetail detail = null;
            List<EquipmentInfoInnerValidationDetail> fList = null;
            ApiBaseReturn<List<EquipmentInfoInnerValidationDetail>> returnVM = new ApiBaseReturn<List<EquipmentInfoInnerValidationDetail>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 获取所在组织架构
                    //Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    //if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    //String organize_id = "";
                    //List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    //if (idList != null && idList.Count() > 0)
                    //{
                    //    organize_id = String.Join(",", idList);
                    //}
                    //else
                    //{
                    //    throw new Exception("ORGANIZE_INFO_EMPTY");
                    //}
                    #endregion

                    if (!model.CHECK_CODE.IsNullOrEmpty() && model.EQUIPMENT_BODYMARK.IsNullOrEmpty())
                    {
                        head = (await _repository.GetListByTableEX<SfcsEquipmentValidationHead>("*", "SFCS_EQUIPMENT_VALIDATION_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        if (!head.IsNullOrWhiteSpace())
                        {
                            returnVM.Result = await _repository.GetPDAEquipmentValidationDataByHeadID(head.ID);
                        }
                        else
                        {
                            returnVM.Result = null;
                        }
                    }
                    else
                    {
                        if (model.EQUIPMENT_BODYMARK.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            //设备编号不能为空
                            ErrorInfo.Set(_localizer["EQUIPMENT_BODYMARK_NOT_NULL_EX"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            EQUIPMENT = (await _repository.GetListByTableEX<SfcsEquipment>("*", "SFCS_EQUIPMENT", " AND NAME=:CODE", new { CODE = model.EQUIPMENT_BODYMARK })).FirstOrDefault();
                            if (EQUIPMENT.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (EQUIPMENT.STATUS == 6)
                            {
                                //报废
                                ErrorInfo.Set(_localizer["Equipment_STATUS_6_NOT_CHECK"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (model.CHECK_CODE.IsNullOrEmpty())
                                {
                                    String check_code = _repository.QueryEx<String>("SELECT CHECK_CODE FROM SFCS_EQUIPMENT_VALIDATION_HEAD WHERE CHECK_STATUS != 2  ORDER BY ID ASC").FirstOrDefault();
                                    if (!String.IsNullOrEmpty(check_code)) { model.CHECK_CODE = check_code; }
                                    //fList = await _repository.GetPDAEquipmentCheckDataByHeadID( organize_id);
                                }
                            }
                        }
                        if (!model.CHECK_CODE.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            //检查编号是否存在
                            head = (await _repository.GetListByTableEX<SfcsEquipmentValidationHead>("*", "SFCS_EQUIPMENT_VALIDATION_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                            if (head.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (head.CHECK_STATUS != 0)
                            {
                                ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                if (head.ID > 0)
                                {
                                    SfcsEquipValidationDetail content = (await _repository.GetListByTableEX<SfcsEquipValidationDetail>("*", "SFCS_EQUIP_VALIDATION_DETAIL", " AND VALIDATION_HEAD_ID=:VALIDATION_HEAD_ID AND Equipment_ID=:Equipment_ID AND Equipment_STATUS=1", new { VALIDATION_HEAD_ID = head.ID, Equipment_ID = EQUIPMENT.ID })).FirstOrDefault();
                                    if (!content.IsNullOrWhiteSpace())
                                    {
                                        ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_REPEAT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                            }
                        }

                        int headId = 0;
                        if (!ErrorInfo.Status)
                        {
                            headId = await _repository.SavePDAEquipmentValidationData(model, EQUIPMENT, head, detail);
                        }
                        if (head == null && !model.CHECK_CODE.IsNullOrEmpty())
                        {
                            head = (await _repository.GetListByTableEX<SfcsEquipmentValidationHead>("*", "SFCS_EQUIPMENT_VALIDATION_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        }
                        if (head != null || headId > 1)
                        {
                            headId = headId > 1 ? headId : Convert.ToInt32(head.ID);
                            returnVM.Result = await _repository.GetPDAEquipmentValidationDataByHeadID(headId);
                        }
                    }
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
        /// PDA设备验证列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<EquipmentCheckListModel>>> LoadPDAEquipmentValidationList([FromQuery] EquipmentCheckRequestModel model)
        {

            ApiBaseReturn<List<EquipmentCheckListModel>> returnVM = new ApiBaseReturn<List<EquipmentCheckListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    //Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    //if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    //String organize_id = "";
                    //List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    //if (idList != null && idList.Count() > 0)
                    //{
                    //    organize_id = String.Join(",", idList);
                    //}
                    //else
                    //{
                    //    throw new Exception("ORGANIZE_INFO_EMPTY");
                    //}
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {

                        returnVM.Result = await _repository.LoadPDAEquipmentValidationList(model);
                        returnVM.TotalCount = await _repository.LoadPDAEquipmentCheckListCount(model);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取需要验证的设备类型列表
        /// </summary>AuditEquipmentCheckData
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<EquipmentInfoInnerValidationDetail>>> LoadPDAEquipmentValidationInfo([FromQuery] GetEquipmentInfoRequestModel model)
        {
            ApiBaseReturn<List<EquipmentInfoInnerValidationDetail>> returnVM = new ApiBaseReturn<List<EquipmentInfoInnerValidationDetail>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    //Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    //if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    //String organize_id = "";
                    //List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    //if (idList != null && idList.Count() > 0)
                    //{
                    //    organize_id = String.Join(",", idList);
                    //}
                    //else
                    //{
                    //    throw new Exception("ORGANIZE_INFO_EMPTY");
                    //}

                    returnVM.Result = await _repository.GetPDAEquipmentValidationDataByHeadID();

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;

        }

        /// <summary>
        /// 确认PDA设备验证
        /// </summary>
        /// <param name="check_code">设备点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> AuditEquipmentValidationData([FromBody] AuditEquipmentCheckDataRequestModel model)
        {
            SfcsEquipmentValidationHead head = null;
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    //组织架构
                    //Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.AUDIT_USER })).FirstOrDefault();
                    //if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    //String organize_id = "";
                    //List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    //if (idList != null && idList.Count() > 0)
                    //{
                    //    organize_id = String.Join(",", idList);
                    //}
                    //else
                    //{
                    //    throw new Exception("ORGANIZE_INFO_EMPTY");
                    //}

                    if (model.ID > 0 && !ErrorInfo.Status)
                    {
                        //检查编号是否存在
                        head = (await _repository.GetListByTableEX<SfcsEquipmentValidationHead>("*", "SFCS_EQUIPMENT_VALIDATION_HEAD", " AND ID=:ID", new { ID = model.ID })).FirstOrDefault();
                        if (head.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["EQUIPMENT_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.STATUS == 1 && !ErrorInfo.Status)
                    {

                        //确认审核必须要新增
                        if (head.CHECK_STATUS != 0)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //全部验证完了才可以提交
                            //var EquipmentValidationList = await _repository.GetPDAEquipmentValidationDataByHeadID(organize_id, model.ID);

                            //if (EquipmentValidationList == null || EquipmentValidationList.Count <= 0)
                            //{
                            //    ErrorInfo.Set(_localizer["Equipment_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            //}
                            //else
                            //{
                            //    if (EquipmentValidationList.Count(c => "未验证".Equals(c.VDETAIL_STATUS)) > 0)
                            //        ErrorInfo.Set(_localizer["Equipment_STATUS_NOT_COMMIT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            //}
                        }
                    }
                    else if (model.STATUS == 2 && !ErrorInfo.Status)
                    {
                        //审核必须要未审核
                        if (head.CHECK_STATUS != 1)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_1"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_INSERT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.ConfirmPDAEquipmentValidationData(model);
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
        /// 删除PDA设备验证数据记录
        /// </summary>
        /// <param name="check_code">设备点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DeletePDAEquipmentValidationData(String check_code)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    if (String.IsNullOrEmpty(check_code))
                    {
                        throw new Exception("EQUIPMENT_CODE_INFO_NULL");
                    }
                    else
                    {
                        //只能能删除新增和未审核状态下的盘点单
                        SfcsEquipmentValidationHead head = (await _repository.GetListByTableEX<SfcsEquipmentValidationHead>("*", "SFCS_EQUIPMENT_VALIDATION_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = check_code })).FirstOrDefault();
                        if (head == null)
                        {
                            throw new Exception("EQUIPMENT_CODE_INFO_NULL");
                        }
                        else if (head.CHECK_STATUS == 0 || head.CHECK_STATUS == 1)
                        {
                            returnVM.Result = await _repository.DeletePDAEquipmentValidationData(head.ID);
                        }
                        else
                        {
                            throw new Exception("CHECK_STATUS_NOT_INSERT");
                        }

                    }
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 查设备是否已经保养
        ///  true 为已经保养
        /// </summary>
        /// <param name="check_code">设备验证编号</param>
        /// <param name="hid">主表id</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> QueryPDAEquipmentValidationBy(String check_code, decimal hid)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (String.IsNullOrEmpty(check_code))
                    {
                        throw new Exception("EQUIPMENT_CODE_INFO_NULL");
                    }
                    else
                    {
                        returnVM.Result = await _repository.QueryPDAEquipmentValidationBy(check_code, hid);
                    }
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// PDA验证--保养保存
        /// Equipment_ID 设备ID--加载数据时对应字段：INFO_ID
        /// 返回保养记录ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        ////[Authorize("Permission")]
        //public async Task<ApiBaseReturn<string>> PDAMaintain([FromBody] SfcsEquipmentMaintainHistory model)
        //{
        //    ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 设置并返回

        //            if (!ErrorInfo.Status)
        //            {
        //                var resultData = await _repository.PDAMaintain(model);
        //                if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
        //                {
        //                    returnVM.Result = resultData.ResultData;
        //                }
        //                else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
        //                {
        //                    returnVM.Result = "0";
        //                    //通用提示类的本地化问题处理
        //                    string resultMsg = GetLocalSfcssage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
        //                    ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //                }
        //            }

        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorInfo.Set(ex.Sfcssage, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
        //        }
        //    }

        //    #region 如果出现错误，则写错误日志并返回错误内容

        //    if (ErrorInfo.Status)
        //    {
        //        returnVM.ErrorInfo.Set(ErrorInfo);
        //        if (ErrorInfo.ErrorType == EnumErrorType.Error)
        //        {
        //            CreateErrorLog(ErrorInfo);
        //        }
        //        ErrorInfo.Clear();
        //    }

        //    #endregion

        //    return returnVM;
        //}
        #endregion

        /// <summary>
        /// 首页返回类
        /// </summary>
        public class IndexResult
        {
            /// <summary>
            /// 设备分类
            /// </summary>
            public List<SfcsParameters> CategoryList { get; set; }

            /// <summary>
            /// 所有线别列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsEquipmentLinesModel> LinesList { get; set; }
        }

        /// <summary>
        /// 添加或修改视图返回类
        /// </summary>
        public class DetailResult
        {
            /// <summary>
            /// 设备分类
            /// </summary>
            public List<SfcsParameters> CategoryList { get; set; }

            /// <summary>
            /// 所有线别列表
            /// </summary>
            /// <returns></returns>
            public List<SfcsEquipmentLinesModel> LinesList { get; set; }

            /// <summary>
            /// 获取全部部门
            /// </summary>
            /// <returns>结果集</returns>
            public List<SfcsDepartment> DepartmentList { get; set; }
        }

        /// <summary>
        /// 图片信息
        /// </summary>
        public class PicInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string mst_id { get; set; }

            /// <summary>
            /// 图片资源ID
            /// </summary>
            public decimal Img_Resource_id { get; set; }

            /// <summary>
            /// 图片URL
            /// </summary>
            public string ImgUrl { get; set; }

            /// <summary>
            /// 图片名称
            /// </summary>
            public string ImgName { get; set; }

            /// <summary>
            /// 图片大小
            /// </summary>
            public decimal ImgSize { get; set; }
        }
    }
}