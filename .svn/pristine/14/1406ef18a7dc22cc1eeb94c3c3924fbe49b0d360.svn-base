/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 10:16:13                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesBurnFileApplyController                                      
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
using MySqlX.XDevAPI.Common;
using JZ.IMS.ViewModels.BurnFile;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Internal;


namespace JZ.IMS.WebApi.Controllers
{

    /// <summary>
    /// 烧录功能 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesBurnFileApplyController : BaseController
    {
        private readonly IMesBurnFileApplyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MesBurnFileApplyController> _localizer;
        const string _extension = ".bin";
        const int _cloudFile = 2;//云服务
        const int _customerFile = 1;//客户文件
        const int _disable = 1;//禁用
        const int _notDisabled = 0;//不禁用



        public MesBurnFileApplyController(IMesBurnFileApplyRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MesBurnFileApplyController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 文件类型
            /// </summary>
            public List<dynamic> ListParams { get; set; }
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
                        var data = (await _repository.GetListByTableEX<dynamic>(" LOOKUP_CODE CODE,CHINESE VALUE ", "SFCS_PARAMETERS", " And LOOKUP_TYPE='MES_BURN_FILE_TYPE' AND ENABLED='Y' "))?.ToList();
                        returnVM.Result = new IndexVM()
                        {
                            ListParams = data
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
        /// 上传文件
        /// 会返回路径和文件名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<TableDataModel>> UploadImageAndSave([FromForm] MesBurnFileManagerAddOrModifyModel model)//[FromForm] string id,[FromForm] string file_name,[FromForm] decimal label_type,[FromForm] string description,[FromForm] string enabled
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();
            returnVM.Result = new TableDataModel();
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
            MesBurnFileManagerAddOrModifyModel burnFile = null;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (imgFile.Count() > 0)
                    {
                        foreach (var item in imgFile)
                        {
                            #region 验证

                            if (!ErrorInfo.Status && (item == null || item.FileName.IsNullOrEmpty()))
                            {
                                //上传失败
                                ErrorInfo.Set(_localizer["UPLOAD_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status)
                            {
                                filename = ContentDispositionHeaderValue
                                            .Parse(item.ContentDisposition)
                                            .FileName
                                            .Trim('"');
                                fileexname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));
                                //resource_name = filename;

                                #region 判断大小 
                                //filesize= Convert.ToDecimal(Math.Round(item.Length/1024.00,3));
                                //long mb= item.Length / 1024 / 2014;//MB
                                // if (mb>50)
                                // {
                                //     //"只允许上传小于 50MB 的图片." 
                                //ErrorInfo.Set(string.Format( _localizer["UPLOAD_SIZE_ERROR"],50), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                // }
                                #endregion

                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 保存文件并设置返回值

                    if (!ErrorInfo.Status)
                    {

                        if (Request.Form.Files.Count() > 0)
                        {
                            //newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                            string dir = @"upload\sfcsBurnFiles\" + DateTime.Now.ToString("yyyyMMdd") + @"\";
                            string filePath = Path.Combine($"/upload/sfcsBurnFiles/", DateTime.Now.ToString("yyyyMMdd"));
                            var pathWebRoot = AppContext.BaseDirectory + dir;
                            if (Directory.Exists(pathWebRoot) == false)
                            {
                                Directory.CreateDirectory(pathWebRoot);
                            }

                            foreach (var item in imgFile)
                            {

                                #region 判断有没有ID
                                bool isUpdate = false;
                                if (model.ID > 0)
                                {
                                    #region 删除旧的文件

                                    burnFile = (await _repository.GetListByTableEX<MesBurnFileManagerAddOrModifyModel>(" * ", "MES_BURN_FILE_MANAGER", " And ID=:ID ", new { ID = model.ID }))?.FirstOrDefault();
                                    if (burnFile != null && !burnFile.PATH.IsNullOrWhiteSpace() && !burnFile.FILENAME.IsNullOrWhiteSpace())
                                    {
                                        isUpdate = true;
                                        string oldaddr = Path.Combine(pathWebRoot, burnFile.ID + "_" + $"{burnFile.FILENAME}");

                                        if (System.IO.File.Exists(oldaddr))
                                        {
                                            System.IO.File.Delete(oldaddr);
                                        }
                                    }
                                    else
                                    {
                                        burnFile = new MesBurnFileManagerAddOrModifyModel();
                                        burnFile.ID = model.ID;
                                        isUpdate = false;
                                    }

                                    #endregion
                                }
                                #endregion

                                #region 保存到服务器
                                //解析原始文件名
                                string[] fileTempArry = filename.Split('\\');
                                newFileName = model.ID + "_" + (model.FILENAME.IsNullOrEmpty() ? fileTempArry[fileTempArry.Length - 1] : model.FILENAME);
                                filename = pathWebRoot + $"{newFileName}";
                                if (System.IO.File.Exists(filename))
                                {
                                    System.IO.File.Delete(filename);
                                }
                                using (FileStream st = System.IO.File.Create(filename))
                                {
                                    item.CopyTo(st);
                                    st.Flush();
                                    st.Close();
                                }
                                #endregion

                                burnFile.PATH = filePath;
                                burnFile.CODE = model.CODE;
                                string fileNameNoExtension = Path.GetFileNameWithoutExtension(newFileName);
                                burnFile.FILENAME = fileNameNoExtension;
                                burnFile.TYPE = _cloudFile;
                                MesBurnFileManagerModel managerModel = new MesBurnFileManagerModel();
                                if (model.ID > 0 && isUpdate)
                                {
                                    managerModel.UpdateRecords = new List<MesBurnFileManagerAddOrModifyModel>();
                                    managerModel.UpdateRecords.Add(burnFile);
                                }
                                else
                                {
                                    managerModel.InsertRecords = new List<MesBurnFileManagerAddOrModifyModel>();
                                    managerModel.InsertRecords.Add(burnFile);
                                }
                                var result = await _repository.SaveFileManagerDataByTrans(managerModel);
                                var res_data = new { ErrorInfo = true, path = "", filename = "" };
                                //List<string> listid = (List<string>)result.data;
                                if (result.code != -1)
                                {
                                    res_data = new { ErrorInfo = false, path = filePath, filename = fileNameNoExtension };
                                }
                                returnVM.Result.data = res_data;
                            }
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result.data = -1;
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


        // <summary>
        /// 烧录文件查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesBurnFileManagerListModel>>> FileManagerLoadData([FromQuery] MesBurnFileManagerRequestModel model)
        {
            ApiBaseReturn<List<MesBurnFileManagerListModel>> returnVM = new ApiBaseReturn<List<MesBurnFileManagerListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.ID > 0)
                    {
                        conditions += $"and ID=:ID ";
                    }
                    if (!model.CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and CODE=:CODE ";
                    }
                    if (model.TYPE != null && model.TYPE > 0)
                    {
                        conditions += $"and TYPE=:TYPE ";
                    }
                    if (!model.PATH.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(PATH,:PATH) > 0 ";
                    }
                    if (!model.FILENAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(FILENAME,:FILENAME) > 0 ";
                    }
                    if (!model.Mark.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(REMARK,:REMARK) > 0 ";
                    }
                    var list = (await _repository.GetListPagedEx<MesBurnFileManager>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesBurnFileManagerListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesBurnFileManagerListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsyncEx<MesBurnFileManager>(conditions, model);

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
        /// 文件申请 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesBurnFileApplyListModel>>> FileApplyLoadData([FromQuery] MesBurnFileApplyRequestModel model)
        {
            ApiBaseReturn<List<MesBurnFileApplyListModel>> returnVM = new ApiBaseReturn<List<MesBurnFileApplyListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.ID > 0)
                    {
                        conditions += $"and ID=:ID ";
                    }
                    if (!model.APPLY_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and APPLY_NO=:APPLY_NO ";
                    }
                    if (!model.PART_CODE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and PART_CODE=:PART_CODE ";
                    }
                    if (!model.WO_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and WO_NO=:WO_NO ";
                    }

                    var list = (await _repository.GetListPagedEx<MesBurnFileApply>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesBurnFileApplyListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesBurnFileApplyListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsyncEx<MesBurnFileApply>(conditions, model);

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


        // <summary>
        /// 烧录文件关联查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// CODE(文件编号) PATH(路径) FILENAME(文件名)
        /// APPLY_NO(申请编号) 
        /// </summary>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ApplyRelationLoadData([FromQuery] MesBurnApplyRelationRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    int count = 0;
                    var tableModel = await _repository.GetApplyRelationLoadData(model);
                    if (tableModel.code != -1)
                    {
                        count = tableModel.count;
                    }
                    returnVM.Result = tableModel.data;
                    returnVM.TotalCount = count;

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
        /// 下载文件记录 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> DownLogLoadData([FromQuery] MesBurnFileDownRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    int count = 0;

                    var tableModel = await _repository.GetDownLoadData(model);
                    if (tableModel.code != -1)
                    {
                        count = tableModel.count;
                    }

                    returnVM.Result = tableModel.data;
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
        /// 下载文件详细表 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesBurnFileDownHistoryListModel>>> DownDetailLoadData([FromQuery] MesBurnFileDownHistoryRequestModel model)
        {
            ApiBaseReturn<List<MesBurnFileDownHistoryListModel>> returnVM = new ApiBaseReturn<List<MesBurnFileDownHistoryListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.MST_ID > 0)
                    {
                        conditions += $"and MST_ID=:MST_ID ";
                    }
                    if (!model.FILE_NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and FILE_NAME=:FILE_NAME ";
                    }
                    if (!model.FILE_TYPE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and FILE_TYPE=:FILE_TYPE ";
                    }
                    if (model.StartTime != null && model.EndTime != null && model.StartTime <= model.EndTime)
                    {
                        conditions += $"and FILE_TIME>=:StartTime and FILE_TIME<:EndTime ";
                    }
                    var list = (await _repository.GetListPagedEx<MesBurnFileDownHistory>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesBurnFileDownHistoryListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<MesBurnFileDownHistoryListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsyncEx<MesBurnFileDownHistory>(conditions, model);

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
        /// 查询SN下载
        /// 统计sn和下载编号的数量(传SN,下载编号即可)返回的结束对应COUNT
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<MesBurnSnDownListModel>>> SNLoadData([FromQuery] MesBurnSnDownRequestModel model)
        {
            ApiBaseReturn<List<MesBurnSnDownListModel>> returnVM = new ApiBaseReturn<List<MesBurnSnDownListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.DOWN_ID != null && model.DOWN_ID > 0)
                    {
                        conditions += $" and DOWN_ID=:DOWN_ID ";
                    }
                    if (!model.DOWN_NO.IsNullOrWhiteSpace())
                    {
                        conditions += $"and DOWN_NO=:DOWN_NO ";
                    }
                    if (!model.SN.IsNullOrWhiteSpace())
                    {
                        conditions += $"and SN=:SN ";
                    }
                    var list = (await _repository.GetListPagedEx<MesBurnSnDown>(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<MesBurnSnDownListModel>();
                    viewList = _mapper.Map<List<MesBurnSnDownListModel>>(list);
                    // list?.ForEach(x =>
                    // {

                    //     var item = _mapper.Map<MesBurnSnDownListModel>(x);
                    //     //item.ENABLED = (item.ENABLED == "Y");
                    //     viewList.Add(item);
                    // });

                    count = await _repository.RecordCountAsyncEx<MesBurnSnDown>(conditions, model);

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


        #region 获取文件路径

        /// <summary>
        /// 获取路径下文件记录
        /// 选择工单或者SN然后关联出来 下载的内容文件
        /// 返回 序号 文件名字 完整路径 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<BurnFilePathListModel>>> GetFilesByPath([FromQuery] DownModelByNO model)
        {
            ApiBaseReturn<List<BurnFilePathListModel>> returnVM = new ApiBaseReturn<List<BurnFilePathListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (!ErrorInfo.Status && (model.Wo_No.IsNullOrWhiteSpace() && model.SN.IsNullOrWhiteSpace()))
                    {
                        //请输入工单或者SN号,谢谢!! 
                        ErrorInfo.Set(_localizer["PLEASE_ENTER_NUMBER"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (model.Wo_No.IsNullOrWhiteSpace() && !model.SN.IsNullOrWhiteSpace())
                        {
                            //SN找到工单
                           var tableData = await _repository.GetWONOBySN(model.SN);
                            if (tableData.code!=-1)
                            {
                                model.Wo_No = tableData.data;
                            }
                            
                            if (tableData.code==-1|| model.Wo_No.IsNullOrEmpty())
                            {
                                //找不到对应的工单，请注意检查!   
                                throw new Exception(_localizer["NOT_FIND_WO_NO"]);
                            }
                        }

                        //工单查找数据
                        List<MesBurnFileManager> listmodel = await _repository.GetMesBurnManagerByNo(model.Wo_No);
                        var applyModel = (await _repository.GetMesFileApplyByWONO(model.Wo_No)).FirstOrDefault();
                        List<BurnFilePathListModel> select_Files = new List<BurnFilePathListModel>();
                        string webpath = $"/upload/sfcsBurnFiles/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                        //string dir = @"upload\sfcsBurnFiles\" + DateTime.Now.ToString("yyyyMMdd") + @"\";
                        foreach (var item in listmodel)
                        {
                            if (item != null && !item.PATH.IsNullOrWhiteSpace() && !item.FILENAME.IsNullOrWhiteSpace())
                            {
                                string path = string.Empty;
                                
                                if (item.TYPE == _customerFile)
                                {
                                    //path =Path.Combine(model.PATH , model.FILENAME);
                                    path = item.PATH;
                                }
                                else if (item.TYPE == _cloudFile)
                                {
                                    string dir = item.PATH;
                                    path = AppContext.BaseDirectory + dir;
                                    if (Directory.Exists(path) == false)
                                    {
                                        Directory.CreateDirectory(path);
                                    }
                                }

                                //找路径
                                string[] files = Directory.GetFiles(path);

                                for (int i = 0; i < files.Length; i++)
                                {
                                    string fileName = Path.GetFileNameWithoutExtension(files[i]);
                                    if (fileName.Contains(item.FILENAME))
                                    {
                                        BurnFilePathListModel pathModel = new BurnFilePathListModel();
                                        pathModel.ID = item.ID;
                                        pathModel.Type = item.TYPE.ToString();
                                        pathModel.Apply_ID = applyModel.ID.ToString();
                                        pathModel.FileName = Path.GetFileName(files[i]);
                                        if (item.TYPE == _cloudFile)
                                        {
                                            pathModel.Path = Path.Combine(webpath, pathModel.FileName);
                                        }
                                        else if (item.TYPE == _customerFile)
                                        {
                                            pathModel.Path = files[i]; //fileName[i].ToString();
                                        }
                                        select_Files.Add(pathModel);
                                    }
                                }
                                returnVM.Result = select_Files;
                                returnVM.TotalCount = select_Files.Count;
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
        /// 通过文件地址获取到下载路径
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<TableDataModel>> GetDownAddress([FromBody] BurnFileaddressModel model)
        {
            ApiBaseReturn<TableDataModel> returnVM = new ApiBaseReturn<TableDataModel>();
            returnVM.Result = new TableDataModel();
            if (!ErrorInfo.Status)
            {
                try
                {
                    if (!ErrorInfo.Status && (model.APPLY_ID == null || model.APPLY_ID <= 0))
                    {
                        ErrorInfo.Set(string.Format(_localizer["PARAMETER_CANNOT_EMPTY"], "APPLY_ID"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model != null && model.DownLoad.Count() > 0)
                    {
                        foreach (var downModle in model.DownLoad)
                        {

                            #region 参数验证
                            if (!ErrorInfo.Status && (downModle.Type == null || downModle.Type <= 0))
                            {
                                ErrorInfo.Set(string.Format(_localizer["PARAMETER_CANNOT_EMPTY"], "TYPE"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            #endregion
                        }
                    }

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var result = await _repository.DownAddressByTrans(model);
                        if (result.code != -1)
                        {
                            returnVM.Result = result;
                            returnVM.TotalCount = result.count;
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
        #endregion

        /// <summary>
        /// 替换文件 
        /// BURN_FILE_ID:烧录文件
        /// APPLY_ID:申请编号
        ///USER_NAME:用户名字
        /// </summary>
        /// <param name="BURN_FILE_ID">烧录文件</param>
        /// <param name="APPLY_ID">申请编号</param>
        /// <param name="USER_NAME">用户名字</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> Repalce([FromBody] MesBurnApplyRelationAddOrModifyModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if ((model.BURN_FILE_ID == null || model.BURN_FILE_ID <= 0) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(string.Format(_localizer["PARAMETER_CANNOT_EMPTY"], "BURN_FILE_ID"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if ((model.APPLY_ID == null || model.APPLY_ID <= 0) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(string.Format(_localizer["PARAMETER_CANNOT_EMPTY"], "APPLY_ID"), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var relationModel = (await _repository.GetListByTableEX<MesBurnApplyRelationAddOrModifyModel>("*", "MES_BURN_APPLY_RELATION", " AND APPLY_ID=:APPLY_ID ", new { APPLY_ID = model.APPLY_ID })).FirstOrDefault();
                        if (relationModel != null)
                        {
                            MesBurnApplyRelationModel relationAddOrModifyModel = new MesBurnApplyRelationModel();

                            #region 更新关系表为禁用

                            relationModel.STATUS = _disable;
                            relationAddOrModifyModel.UpdateRecords = new List<MesBurnApplyRelationAddOrModifyModel>();
                            relationAddOrModifyModel.UpdateRecords.Add(relationModel);
                            var result = await _repository.SaveApplyRelationDataByTrans(relationAddOrModifyModel);

                            #endregion

                            #region 插入数据

                            if (result != -1)
                            {
                                model.CREATE_TIME = DateTime.Now;
                                model.STATUS = _notDisabled;
                                relationAddOrModifyModel.InsertRecords = new List<MesBurnApplyRelationAddOrModifyModel>();
                                relationAddOrModifyModel.InsertRecords.Add(model);
                                var data = await _repository.SaveApplyRelationDataByTrans(relationAddOrModifyModel);
                                if (data != -1)
                                {
                                    returnVM.Result = true;
                                }
                            }

                            #endregion
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
        /// 烧录文件编号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<CodeNo>> GetCodeNo()
        {
            ApiBaseReturn<CodeNo> returnVM = new ApiBaseReturn<CodeNo>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetFIleManagerSEQID();
                        string num = resdata > 10000 ? resdata.ToString() : resdata.ToString().PadLeft(6, '0');
                        string code = "BF-" + num; // 一共6位,位数不够时从左边开始用0补;
                        if (resdata > 0 && !code.IsNullOrWhiteSpace())
                        {
                            returnVM.Result = new CodeNo()
                            {
                                ID = resdata,
                                No = code
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
        /// 文件申请编号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<CodeNo>> GetApplyNo()
        {
            ApiBaseReturn<CodeNo> returnVM = new ApiBaseReturn<CodeNo>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回


                    if (!ErrorInfo.Status)
                    {

                        var ID = await _repository.GetFileApplySEQID();
                        string code = ID > 10000 ? ID.ToString() : ID.ToString().PadLeft(6, '0');
                        string apply_NO = "AYN-" + code; // 一共6位,位数不够时从左边开始用0补;
                        if (ID > 0 && !apply_NO.IsNullOrWhiteSpace())
                        {
                            returnVM.Result = returnVM.Result = new CodeNo()
                            {
                                ID = ID,
                                No = apply_NO
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
        /// 获取下载编号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiBaseReturn<CodeNo>> GetDownNo()
        {
            ApiBaseReturn<CodeNo> returnVM = new ApiBaseReturn<CodeNo>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回


                    if (!ErrorInfo.Status)
                    {

                        var ID = await _repository.GetBurnFIleSEQID();
                        string code = ID > 10000 ? ID.ToString() : ID.ToString().PadLeft(6, '0');
                        string down_NO = "DN-" + code; // 一共6位,位数不够时从左边开始用0补;
                        if (ID > 0 && !down_NO.IsNullOrWhiteSpace())
                        {
                            returnVM.Result = new CodeNo()
                            {
                                ID = ID,
                                No = down_NO
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
        /// 保存烧录文件数据
        /// 传TYPE(首页程序类型下拉数据),PATH,FILENAME
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveBurnFileManager([FromBody] MesBurnFileManagerModel model)
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

                        var resdata = await _repository.SaveFileManagerDataByTrans(model);
                        if (resdata.code != -1)
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
        /// 保存数据
        /// 新增时ApplyModel RelationArrary都是使用InsertRecords:ApplyModel对应的idAPPLY_NO  PART_CODE(料号)或者 WO_NO(工单) ,USER_NAME(用户名) BURN_FILE_ID APPLY_ID
        /// 修改时ApplyModel RelationArrary都是使用UpdateRecords:传参数和上面一样
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveBurnFileApplyAndRelation([FromBody] BurnFileApplyAndRelation model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status && model.ApplyModel.InsertRecords != null && model.ApplyModel.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.ApplyModel.InsertRecords)
                        {
                            if (!ErrorInfo.Status && item.PART_CODE.IsNullOrWhiteSpace() && item.WO_NO.IsNullOrWhiteSpace())
                            {
                                //工单号和料号不能同时为空,注意请检查！
                                ErrorInfo.Set(_localizer["WORKNO_CANNOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            if (!ErrorInfo.Status)
                            {
                                var woCount = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = item.WO_NO })).Count;
                                if (woCount <= 0)
                                    //工单不存在,请注意检查! 
                                    ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status)
                            {
                                var pnCount = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And PART_NO=:PART_NO", new { PART_NO = item.PART_CODE })).Count;
                                if (pnCount <= 0)
                                    //成品料号不存在,请注意检查! 
                                    ErrorInfo.Set(_localizer["PN_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                        }
                    }

                    if (!ErrorInfo.Status && model.ApplyModel.UpdateRecords != null && model.ApplyModel.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.ApplyModel.UpdateRecords)
                        {
                            if (!ErrorInfo.Status && item.PART_CODE.IsNullOrWhiteSpace() && item.WO_NO.IsNullOrWhiteSpace())
                            {
                                //工单号和料号不能同时为空,注意请检查！
                                ErrorInfo.Set(_localizer["WORKNO_CANNOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status)
                            {
                                var woCount = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And WO_NO=:WO_NO", new { WO_NO = item.WO_NO })).Count;
                                if (woCount <= 0)
                                    //工单不存在,请注意检查! 
                                    ErrorInfo.Set(_localizer["WO_NO_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            if (!ErrorInfo.Status)
                            {
                                var pnCount = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " And PART_NO=:PART_NO", new { PART_NO = item.PART_CODE })).Count;
                                if (pnCount <= 0)
                                    //成品料号不存在,请注意检查! 
                                    ErrorInfo.Set(_localizer["PN_NOT_EXIST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                        }
                    }

                    if (!ErrorInfo.Status && model.RelationArrary.Count <= 0)
                    {
                        //请选择需要绑定的文件！ 
                        ErrorInfo.Set(_localizer["PLEASE_SELECT_FILE"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveApplyAndRelationDataByTrans(model);
                        if (resdata != -1)
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


        /// <summary>
        /// 保存数据
        /// 只传:PART_CODE(料号)或者 WO_NO(工单) ,USER_NAME(用户名)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiBaseReturn<bool>> SaveBurnFileApply([FromBody] MesBurnFileApplyModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            if (!ErrorInfo.Status && item.PART_CODE.IsNullOrWhiteSpace() && item.WO_NO.IsNullOrWhiteSpace())
                            {
                                //工单号和料号不能同时为空,注意请检查！
                                ErrorInfo.Set(_localizer["WORKNO_CANNOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    if (model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            if (!ErrorInfo.Status && item.PART_CODE.IsNullOrWhiteSpace() && item.WO_NO.IsNullOrWhiteSpace())
                            {
                                //工单号和料号不能同时为空,注意请检查！
                                ErrorInfo.Set(_localizer["WORKNO_CANNOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveFileApplyDataByTrans(model);
                        if (resdata != -1)
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



        /// <summary>
        /// 保存申请编号与烧录文件数据
        /// 传 APPLY_ID(申请文件ID), BURN_FILE_ID(烧录文件ID),USER_NAME(用户)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveApplyRelation([FromBody] MesBurnApplyRelationModel model)
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
                        decimal resdata = await _repository.SaveApplyRelationDataByTrans(model);
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

        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ApiBaseReturn<bool>> SavBurnFileDown([FromBody] MesBurnFileDownModel model)
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
                        var resdata = await _repository.SaveFileDownDataByTrans(model);
                        if (resdata.code != -1)
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
        /// 保存SN数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveBurnSNByTrans([FromBody] MesBurnSnDownModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status)
                    {
                        List<List<MesBurnSnDownAddOrModifyModel>> modellist = new List<List<MesBurnSnDownAddOrModifyModel>>();
                        if (model.InsertRecords != null) modellist.Add(model.InsertRecords);
                        if (model.UpdateRecords != null) modellist.Add(model.UpdateRecords);

                        modellist.ForEach(c =>
                        {
                            c.ForEach(async t =>
                            {
                                if ((await _repository.GetListByTableEX<MesBurnSnDown>("*", "MES_BURN_SN_DOWN", " AND DOWN_NO=:DOWN_NO AND SN=:SN ", new { DOWN_NO = t.DOWN_NO, SN = t.SN })).Any())
                                    //SN、下载编号、申请编号已经同时存在,请注意检查! 
                                    ErrorInfo.Set(string.Format(_localizer["SN_DOWN_APPLY_ALREADY_EXIST"], 50), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);

                            });
                        });
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.SaveBurnSNByTrans(model);
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
    }

}