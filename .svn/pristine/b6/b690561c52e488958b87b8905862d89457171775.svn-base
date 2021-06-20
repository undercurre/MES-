/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-10 19:51:17                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsPrintFilesController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using AutoMapper;
using JZ.IMS.Core;
using JZ.IMS.Core.Extensions;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 标签打印设计文件上传
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsPrintFilesController : BaseController
    {
        private readonly ISfcsPrintFilesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsPrintFilesController> _localizer;

        public SfcsPrintFilesController(ISfcsPrintFilesRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsPrintFilesController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 标签类型
            /// </summary>
            public List<dynamic> LableTypeList { get; set; }
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
                        returnVM.Result = new IndexVM()
                        {
                            LableTypeList = await _repository.GetListByTable("SP.*", "SFCS_PARAMETERS SP", " AND LOOKUP_TYPE='LABEL_TYPE' ORDER BY LOOKUP_TYPE"),
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<object>>> LoadData([FromQuery] SfcsPrintFilesRequestModel model)
        {
            ApiBaseReturn<List<object>> returnVM = new ApiBaseReturn<List<object>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.FILE_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $" AND INSTR(FILE_NAME, :FILE_NAME) > 0 ";
                    }
                    if (model.LABEL_TYPE > 0)
                    {
                        conditions += $" AND INSTR(LABEL_TYPE, :LABEL_TYPE) > 0 ";
                    }
                    var list = (await _repository.GetListPagedEx<SfcsPrintFilesListModel>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsPrintFilesListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsPrintFilesListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsync(conditions, model);
                    var result = viewList.Select(c => new { c.ID, c.DESCRIPTION, c.ENABLED, c.LABEL_TYPE, c.FILE_NAME,c.ORIGINAL_FILE_NAME }).ToList<object>();
                    returnVM.Result = result;
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
        /// 查找图片和文件的路径
        /// 查图片:Photo
        /// 查文件:File
        /// 主键:ID
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> GetPathByType([FromQuery] string type, string id)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            string name = string.Empty;
            string result = string.Empty;
            string path = string.Empty;
            Byte[] Files = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 返回值
                    if (!type.IsNullOrWhiteSpace() && !id.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        path = $"/upload/sfcsPrintFiles/"; // + DateTime.Now.ToString("yyyyMM");
                        string dir = @"upload\sfcsPrintFiles\";
                        var pathWebRoot = AppContext.BaseDirectory + dir;
                        if (Directory.Exists(pathWebRoot) == false)
                        {
                            Directory.CreateDirectory(pathWebRoot);
                        }
                        var prinFiles = await _repository.GetPrintFiles(id);

                        if (type == "File" && prinFiles != null && prinFiles.FILE_CONTENT != null)
                        {
                            name = @"" + prinFiles.ID + "_" + prinFiles.ORIGINAL_FILE_NAME;
                        }
                        else if (type == "Photo" && prinFiles != null && prinFiles.LABEL_IMAGE != null)
                        {
                            name = @"" + prinFiles.ID + "_Photo.jpg";
                        }

                        #region 查地址
                        if (!name.IsNullOrEmpty() && ((!System.IO.File.Exists(pathWebRoot + $"{name}") && prinFiles != null) && (prinFiles.FILE_CONTENT != null || prinFiles.LABEL_IMAGE != null)))
                        {

                            #region 返回数据
                            string fileName = pathWebRoot + $"{name}";
                            if (type == "Photo" && prinFiles.LABEL_IMAGE != null)
                            {
                                Files = (Byte[])prinFiles.LABEL_IMAGE;
                            }
                            else if (type == "File" && prinFiles.FILE_CONTENT != null)
                            {
                                Files = (Byte[])prinFiles.FILE_CONTENT;
                            }

                            FileInfo fileInfo = new FileInfo(fileName);
                            using (BinaryWriter bw = new BinaryWriter(fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                            {
                                bw.Write(Files);
                                //bw.Close();
                            }
                            fileInfo.LastWriteTime = prinFiles.FILE_VERSION_DATE ?? DateTime.MinValue;
                            fileInfo.IsReadOnly = false;
                            fileInfo.Attributes = FileAttributes.Normal;
                            #endregion
                        }
                        returnVM.Result = name.IsNullOrEmpty() ? "" : path + $"{name}";

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
        /// 查找图片和文件的路径
        /// 查图片:Photo
        /// 查文件:File
        /// 主键:ID
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetBase64PathByType([FromQuery] string type, string id)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            string name = string.Empty;
            string result = string.Empty;
            string path = string.Empty;
            Byte[] Files = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 返回值
                    if (!type.IsNullOrWhiteSpace() && !id.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {

                        var prinFiles = await _repository.GetPrintFiles(id);

                        if (type == "File" && prinFiles != null && prinFiles.FILE_CONTENT != null)
                        {
                            name = @"" + prinFiles.ID + "_" + prinFiles.ORIGINAL_FILE_NAME;
                        }
                        else if (type == "Photo" && prinFiles != null && prinFiles.LABEL_IMAGE != null)
                        {
                            name = @"" + prinFiles.ID + "_Photo.jpg";
                        }

                        #region 查地址
                        if (!name.IsNullOrEmpty() && (prinFiles.FILE_CONTENT != null || prinFiles.LABEL_IMAGE != null))
                        {

                            #region 返回数据

                            if (type == "Photo" && prinFiles.LABEL_IMAGE != null)
                            {
                                Files = (Byte[])prinFiles.LABEL_IMAGE;

                            }
                            else if (type == "File" && prinFiles.FILE_CONTENT != null)
                            {
                                Files = (Byte[])prinFiles.FILE_CONTENT;
                            }

                            #endregion
                        }

                        //根据文件成功Base64
                        if (!name.IsNullOrEmpty() && (prinFiles != null && (prinFiles.FILE_CONTENT != null || prinFiles.LABEL_IMAGE != null)))
                        {
                            #region 转BASE64

                            string contentType = MimeMapping.GetMimeMapping(name);
                            var fileType = System.IO.Path.GetExtension(name);
                            string base64Data = Files == null || Files.Length <= 0 ? "data:object;base64, " : "data:" + contentType + ";base64," + Convert.ToBase64String(Files);
                            //缓冲流处理文件
                            returnVM.Result = new { Base64 = base64Data, FileType = fileType };

                            #endregion
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
        /// 判断文件名是否重复了，重复不能使用
        /// 在修改的时候使用
        /// </summary>
        /// <param name="FileNmae">文件名字</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> IsExistFileNmae(string FileNmae)
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
                        if (!FileNmae.IsNullOrWhiteSpace())
                        {
                            result = await _repository.IsExistFileNmae(FileNmae);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
		/// 数据保存只使用updaterecord方法即可，
        /// 同时有图片上传功能(标签文件上传只能是jpg)
        /// 标签格式图对应的字段名:Photo
        /// 标签设计文件对应的字段名:File
		/// </summary>
		/// <returns></returns>
		[HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<PicInfo>> UploadImageAndSave([FromForm] SfcsPrintFilesAddOrModifyModel model)//[FromForm] string id,[FromForm] string file_name,[FromForm] decimal label_type,[FromForm] string description,[FromForm] string enabled
        {
            ApiBaseReturn<PicInfo> returnVM = new ApiBaseReturn<PicInfo>();
            var imgFile = Request.Form.Files;
            var resource_name = string.Empty;
            var filename = string.Empty;
            var photoname = string.Empty;
            long size = 0;
            decimal filesize = 0;
            var newFileName = string.Empty;
            var path = string.Empty;
            var fileexname = string.Empty;
            var photoexname = string.Empty;
            string type = "Update";
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (imgFile.Count() > 0)
                    {
                        foreach (var item in imgFile)
                        {
                            #region 验证标签图片

                            if (!ErrorInfo.Status && (item == null || item.FileName.IsNullOrEmpty()))
                            {
                                //上传失败
                                ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status)
                            {
                                if (item.Name == "File")
                                {
                                    filename = ContentDispositionHeaderValue
                                                .Parse(item.ContentDisposition)
                                                .FileName
                                                .Trim('"');
                                    fileexname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));
                                    //resource_name = filename;
                                }


                                #region 判断后缀
                                if (item.Name == "Photo")
                                {
                                    photoname = ContentDispositionHeaderValue
                                             .Parse(item.ContentDisposition)
                                             .FileName
                                             .Trim('"');
                                    photoexname = photoname.Substring(photoname.LastIndexOf("."), photoname.Length - photoname.LastIndexOf("."));
                                    if (!photoexname.ToLower().Contains("jpg"))//&& !extname.ToLower().Contains("png") && !extname.ToLower().Contains("gif")
                                    {
                                        ErrorInfo.Set(string.Format(_localizer["onlyfileformat_err"], "jpg"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }

                                    #endregion

                                    #region 判断大小

                                    filesize = Convert.ToDecimal(Math.Round(item.Length / 1024.00, 3));
                                    long mb = item.Length / 1024 / 1024; // MB
                                    if (mb > 20)
                                    {
                                        //"只允许上传小于 20MB 的图片."
                                        ErrorInfo.Set(_localizer["upload_size_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }

                    if (model != null)
                    {

                        if (model.FILE_NAME.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                        {
                            //文件名不能为空
                            ErrorInfo.Set(_localizer["no_printfilename_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                        if (model.LABEL_TYPE <= 0 && !ErrorInfo.Status)
                        {
                            //标签类型不能为空
                            ErrorInfo.Set(_localizer["nolabeltype_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                        if (model.DESCRIPTION.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                        {
                            //描述类型不能为空
                            ErrorInfo.Set(_localizer["notinputcolumn_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                    }

                    #endregion

                    #region 保存文件并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (model.ID <= 0)
                        {
                            model.ID = await _repository.Get_MES_SEQ_ID();
                            type = "Insert";
                        }
                        if (Request.Form.Files.Count() > 0)
                        {

                            foreach (var item in imgFile)
                            {
                                if (item.Name == "File")
                                {
                                    //解析原始文件名
                                    string[] fileTempArry = filename.Split('\\');
                                    using (Stream fs = item.OpenReadStream())
                                    {
                                        //.CopyTo(fs);
                                        //将文件转成2进制
                                        BinaryReader br = new BinaryReader(fs);
                                        Byte[] printFileBinaryData = br.ReadBytes((int)fs.Length);
                                        fs.Close();
                                        br.Close();
                                        model.FILE_CONTENT = printFileBinaryData;

                                        //解析原始文件名
                                        model.ORIGINAL_FILE_NAME = fileTempArry[fileTempArry.Length - 1];

                                        //解析文件类型
                                        string[] fileArry = filename.Split('.');
                                        model.FILE_TYPE = fileArry[fileArry.Length - 1];

                                        //File Version Date
                                        // FileInfo fileInfo = new FileInfo(filename);
                                        // model.FILE_VERSION_DATE = fileInfo.LastWriteTime;
                                    }
                                }
                                if (item.Name == "Photo")
                                {
                                    size += item.Length;

                                    using (Stream fs = item.OpenReadStream())
                                    {
                                        //将文件转成2进制
                                        BinaryReader br = new BinaryReader(fs);
                                        Byte[] imageBinaryData = br.ReadBytes((int)fs.Length);
                                        br.Close();
                                        model.LABEL_IMAGE = imageBinaryData;
                                        fs.Close();
                                    }

                                }
                            }
                        }
                        decimal result = await _repository.UpdateDataByTrans(model, type);
                        var res_data = new PicInfo { ErrorInfo = true, ImgUrl = "",FileName= model.ORIGINAL_FILE_NAME };
                        if (result != -1)
                        {
                            // 资源信息filesize
                            res_data = new PicInfo
                            {
                                ID=model.ID,
                                ErrorInfo = false,
                                FileName= model.ORIGINAL_FILE_NAME
                            };
                        }
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

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        //[HttpPost]
        //[Authorize("Permission")]
        //public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        //{
        //    ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 删除并返回

        //            if (!ErrorInfo.Status && id <= 0)
        //            {
        //                returnVM.Result = false;
        //                //通用提示类的本地化问题处理
        //                string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
        //                    ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
        //                ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //            }
        //            if (!ErrorInfo.Status)
        //            {
        //                var count = await _repository.DeleteAsync(id);
        //                if (count > 0)
        //                {
        //                    returnVM.Result = true;
        //                }
        //                else
        //                {
        //                    //失败
        //                    returnVM.Result = false;
        //                    //通用提示类的本地化问题处理
        //                    string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
        //                        ResultCodeAddMsgKeys.CommonExceptionMsg);
        //                    ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
        //                }
        //            }
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
        /// 图片信息
        /// </summary>
        public class PicInfo
        {
            /// <summary>
            /// ID
            /// </summary>
            public decimal? ID { get; set; }

            /// <summary>
            /// 图片URL
            /// </summary>
            public string ImgUrl { get; set; }

            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            ///错误
            /// </summary>

            public bool ErrorInfo { get; set; }
        }

        #region 20201230 标签打印配置

        /// <summary>
        /// 根据标签文件ID获取配置的SQL信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<SavePrintFilesDetailAddOrModifyModel>> GetPrintFilesDetail([FromQuery] GetPrintFilesDetailRequestModel model)
        {
            ApiBaseReturn<SavePrintFilesDetailAddOrModifyModel> returnVM = new ApiBaseReturn<SavePrintFilesDetailAddOrModifyModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    String sQuery = "";
                    if (model.PRINT_FILES_ID > 0)
                    {
                        sQuery += $" WHERE PRINT_FILES_ID = :PRINT_FILES_ID ";

                        if (model.ID > 0)
                        {
                            sQuery += $" AND ID = :ID ";
                        }
                        SfcsPrintFilesDetail detailModel = await _repository.GetAsyncEx<SfcsPrintFilesDetail>(sQuery, new { ID = model.ID, PRINT_FILES_ID = model.PRINT_FILES_ID });
                        SavePrintFilesDetailAddOrModifyModel printFilesDetail = new SavePrintFilesDetailAddOrModifyModel();
                        if (!detailModel.IsNullOrWhiteSpace())
                        {
                            printFilesDetail.ID = detailModel.ID;
                            printFilesDetail.PRINT_FILES_ID = detailModel.PRINT_FILES_ID;
                            printFilesDetail.CREATE_USER = detailModel.CREATE_USER;
                            printFilesDetail.FILE_CONTENT = System.Text.Encoding.Default.GetString(detailModel.FILE_CONTENT);
                            printFilesDetail.CREATE_TIME = detailModel.CREATE_TIME;
                        }
                        returnVM.Result = printFilesDetail;
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
        /// 保存SQL配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SavePrintFilesDetail([FromBody] SavePrintFilesDetailAddOrModifyModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    SfcsPrintFilesDetailAddOrModifyModel saveModel = new SfcsPrintFilesDetailAddOrModifyModel();
                    saveModel.ID = model.ID;
                    saveModel.PRINT_FILES_ID = model.PRINT_FILES_ID;
                    saveModel.CREATE_USER = model.CREATE_USER;

                    if (!model.FILE_CONTENT.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        //检查SQL语句是否正确  不可以传* 
                        String file_content = model.FILE_CONTENT.ToUpper();
                        String content = file_content.Replace(" ", "");
                        if (content.Length < 12)
                        {
                            throw new Exception("SQL_ERROR");
                        }
                        if (content.Substring(0, 11).IndexOf("SELECT*FROM") != -1)
                        {
                            throw new Exception("SQL_NOT_SUPPORT");
                        }
                        else if (content.IndexOf("UPDATE") != -1 || content.IndexOf("DELETE") != -1 || content.IndexOf("INSERT") != -1)
                        {
                            throw new Exception("SQL_ERROR1");
                        }
                        else if (content.IndexOf("WHERE") == -1)
                        {
                            throw new Exception("SQL_NOT_WHERE");
                        }
                        else
                        {
                            try
                            {
                                _repository.QueryEx<dynamic>(file_content);
                            }
                            catch (Exception ex)
                            {
                                if (!ex.Message.ToString().Contains("ORA-01008")) { throw new Exception("SQL_ERROR"); }
                            }
                        }

                        saveModel.FILE_CONTENT = System.Text.Encoding.Default.GetBytes(file_content);
                    }
                    else
                    {
                        throw new Exception("SQL_ERROR");
                    }
                    if (saveModel.CREATE_USER.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["USER_NAME_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (saveModel.PRINT_FILES_ID > 0 && !ErrorInfo.Status)
                    {
                        SfcsPrintFilesListModel printFiles = await _repository.GetAsyncEx<SfcsPrintFilesListModel>("WHERE ID =:ID", new { ID = saveModel.PRINT_FILES_ID });
                        if (printFiles.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["PRINT_FILES_DATA_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (printFiles.LABEL_TYPE == 1 || printFiles.LABEL_TYPE == 4)
                        {
                            // 1 是流水号 4 是周转箱
                            ErrorInfo.Set(_localizer["PRINT_LABEL_TYPE_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
                        ErrorInfo.Set(_localizer["PRINT_FILES_ID_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (saveModel.ID < 1 && !ErrorInfo.Status)
                    {
                        SfcsPrintFilesDetail detailModel = await _repository.GetAsyncEx<SfcsPrintFilesDetail>("WHERE PRINT_FILES_ID =:PRINT_FILES_ID", new { PRINT_FILES_ID = model.PRINT_FILES_ID });
                        saveModel.ID = !detailModel.IsNullOrWhiteSpace() ? detailModel.ID : 0;
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SavePrintFilesDetail(saveModel) > 0 ? true : false;
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

        #endregion

    }
}