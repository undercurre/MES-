/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-05 09:21:49                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： I
* ontroller                                      
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
using JZ.IMS.IRepository;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Net.Http.Headers;
using System.IO;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 钢网注册控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtStencilConfigController : BaseController
    {
        private readonly ISmtStencilConfigRepository _repository;
        private readonly ISmtResourceRouteRepository _routeRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ShareResourceController> _localizer;

        public SmtStencilConfigController(ISmtStencilConfigRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<ShareResourceController> localizer, ISmtResourceRouteRepository routeRepository)
        {
            _repository = repository;
            _routeRepository = routeRepository;

            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;

        }

        public class IndexVM
        {
            /// <summary>
            /// 辅料名称集
            /// </summary>
            /// <returns></returns>
            public List<IDNAME> NameList { get; set; }

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
                            NameList = await _routeRepository.GetNameAsync()
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
        /// 查询所有 新字段ATTRIBUTE1(钢网长度(CM))  ATTRIBUTE1(钢网宽度(CM)) ATTRIBUTE3(钢网厚度(CM)) ATTRIBUTE5(锡膏类型)
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadData([FromQuery]SmtStencilConfigRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (model.USER_ID ?? 0) <= 0)
                    {
                        ErrorInfo.Set(_localizer["USER_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var res = await _repository.LoadData(model);

                        returnVM.Result = res?.data;
                        returnVM.TotalCount = res?.count ?? 0;
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
        /// PAD查询所有
        /// USER_ID(用户ID必填)
        /// 工单号用这个接口(api/SmtWo/LoadData),取这个字段PART_NO
        /// PART_NO(产品编号)
        /// 返回字段{STENCIL_NO--钢网编号,LOCATION --钢网储位,PRINT_COUNT--印刷次数,PCB_SIDE--板底/面}
        /// 测试数据:产品编号201223920000
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> LoadDataPDA([FromQuery] SmtStencilPDARequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (model.USER_ID ?? 0) <= 0)
                    {
                        ErrorInfo.Set(_localizer["USER_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        
                        var res = await _repository.LoadDataPDA(model);
                        returnVM.Result = res?.data;
                        returnVM.TotalCount = res?.count ?? 0;
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
            return returnVM;
        }

        /// <summary>
        /// 当前项目是否已被使用 
        /// </summary>
        /// <param name="STENCIL_NO">钢网号(必填)</param>
		/// <param name="STENCIL_ID">钢网ID(必填)</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ItemIsByUsed(string STENCIL_NO, decimal STENCIL_ID)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            bool result = false;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (!STENCIL_NO.IsNullOrWhiteSpace())
                        {
                            result = await _repository.ItemIsByUsed(STENCIL_NO, STENCIL_ID);
                        }
                        returnVM.Result = result;
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
        /// 删除
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>响应结果</returns>
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtStencilConfigModel model)
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
                    if (ex.Message != null && ex.Message.IndexOf("SMT_STENCIL_CONFIG_INX1") != -1)
                    {
                        ErrorInfo.Set(_localizer["smt_stencil_config_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
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
        /// 钢网文件上传
        /// </summary>
        /// <param name="stencil_id">钢网ID(必填)</param>
        /// <param name="userName">上传用户(必填)</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> UploadResource(decimal stencil_id, string userName)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            var imgFile = Request.Form.Files[0];
            var resource_name = string.Empty;//文件名称
            var filename = string.Empty;//文件名
            var extname = string.Empty;//上传的文件后缀
            long size = 0;
            decimal filesize = 0;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (stencil_id <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["STENCIL_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (userName.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["USERNAME_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status && (imgFile == null || imgFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["UPLOAD_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(imgFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        resource_name = filename;

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(imgFile.Length / 1024.00, 3));
                        //long mb = imgFile.Length / 1024 / 1024; // MB
                        //if (mb > 20)
                        //{
                        //    ErrorInfo.Set(_localizer["UPLOAD_SIZE_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        //}
                        #endregion
                    }

                    #endregion

                    #region 保存文件并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var filenameNew = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var path = $"/upload/stencilfile/" + DateTime.Now.ToString("yyyyMM");
                        var pathWebRoot = AppContext.BaseDirectory + path;
                        if (Directory.Exists(pathWebRoot) == false)
                        {
                            Directory.CreateDirectory(pathWebRoot);
                        }
                        filename = pathWebRoot + $"/{filenameNew}";

                        size += imgFile.Length;
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            imgFile.CopyTo(fs);
                            fs.Flush();
                        }
                        //保存资源
                        SmtStencilResourceListModel model = new SmtStencilResourceListModel();
                        model.STENCIL_ID = stencil_id;
                        model.RESOURCE_URL = path + $"/{filenameNew}";
                        model.RESOURCE_NAME = resource_name;
                        model.RESOURCE_SIZE = filesize.ToString();
                        model.UPLOAD_OPER = userName;

                        returnVM.Result = await _repository.SaveStencilResourceInfo(model) > 0 ? true : false;

                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 根据钢网的资源ID获取钢网资源数据
        /// </summary>
        /// <param name="resource_id">钢网的资源ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SmtStencilResourceListModel>> GetStencilResourceInfo(string resource_id)
        {
            ApiBaseReturn<SmtStencilResourceListModel> returnVM = new ApiBaseReturn<SmtStencilResourceListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 参数验证
                    if (string.IsNullOrEmpty(resource_id) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["RESOURCE_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetStencilResourceInfo(resource_id);

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
        /// 获取钢网资源列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetStencilResourceList(SmtStencilResourceRequestModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (model.STENCIL_ID <= 0 && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["STENCIL_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion
                    #region 获取返回值

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetStencilResourceList(model);
                        returnVM.Result = JsonHelper.ObjectToJSON(resdata);
                        returnVM.TotalCount = await _repository.GetStencilResourceListCount(model);
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
        /// 根据钢网的资源ID获取钢网资源数据
        /// </summary>
        /// <param name="resource_id">钢网的资源ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> DownloadResource(string resource_id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            returnVM.Result = string.Empty;
            if (!ErrorInfo.Status)
            {
                try
                {

                    #region 参数验证
                    if (string.IsNullOrEmpty(resource_id) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["RESOURCE_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        SmtStencilResourceListModel model = await _repository.GetStencilResourceInfo(resource_id);
                        if (model != null)
                        {
                            var arr = model.RESOURCE_URL.Split('/');
                            string path = @"upload\stencilfile" + @"\" + arr[3] + @"\" + arr[4];
                            var pathRoot = AppContext.BaseDirectory + path;

                            if (System.IO.File.Exists(pathRoot))
                            {
                                string contentType = MimeMapping.GetMimeMapping(pathRoot);

                                using (var fs = new BufferedStream( System.IO.File.OpenRead(pathRoot)))
                                {
                                    byte[] dt = new byte[fs.Length];
                                    fs.Read(dt, 0, (int)fs.Length);
                                    returnVM.Result = "data:" + contentType + ";base64," + Convert.ToBase64String(dt);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }
    }
}