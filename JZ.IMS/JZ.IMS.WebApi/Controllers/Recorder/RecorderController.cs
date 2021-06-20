using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Utilities;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels.BurnFile;
using JZ.IMS.ViewModels.Recorder;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace JZ.IMS.WebApi.Controllers.Recorder
{
    /// <summary>
    /// 烧录 控制器  
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecorderController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<RecorderController> _localizer;
        private readonly IMesBurnFileApplyRepository _repository;


        const int _cloudFile = 2;//云服务
        const int _customerFile = 1;//客户文件

        public RecorderController(IMesBurnFileApplyRepository repository,
            IMapper mapper,
            IStringLocalizer<RecorderController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
        }

        /// <summary>
        /// 烧录文件下载接口
        /// </summary>
        /// <param name="WORK_ORDER">工单号</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<RecorderFilesResponse> RecorderFiles(String WORK_ORDER)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            RecorderFilesResponse response = new RecorderFilesResponse();
            try
            {
                //工单查找数据
                List<MesBurnFileManager> listmodel = await _repository.GetMesBurnManagerByNo(WORK_ORDER);
                var applyModel = (await _repository.GetMesFileApplyByWONO(WORK_ORDER)).FirstOrDefault();
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
                                pathModel.Path = files[i];
                                //if (item.TYPE == _cloudFile)
                                //{
                                //    pathModel.Path = files[i];
                                //}
                                //else if (item.TYPE == _customerFile)
                                //{
                                //    pathModel.Path = files[i]; //fileName[i].ToString();
                                //}
                                select_Files.Add(pathModel);
                            }
                        }
                    }
                }

                //下载文件并记录
                BurnFileaddressModel model = new BurnFileaddressModel();
                model.USER_NAME = "BurnFileMachine";
                model.APPLY_ID = applyModel.ID;
                model.DownLoad = new List<BurnFileAddressRequest>();
                foreach (var file in select_Files)
                {
                    BurnFileAddressRequest burnFileAddress = new BurnFileAddressRequest();
                    burnFileAddress.BURN_FILE_ID = file.ID;
                    burnFileAddress.Path = file.Path;
                    burnFileAddress.Type = file.Type.ToDecimal();
                    model.DownLoad.Add(burnFileAddress);
                }
                if (model.DownLoad.Count <= 0)
                {
                    //没有找到下载文件！
                    response.MSG = _localizer["error_no_data_down"];
                    ErrorInfo.Set(response.MSG, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    return response;
                }

                #region 处理下载文件记录到表
                var no = 0;
                var result = await _repository.DownAddressByTrans(model);
                if (result.code != -1)
                {
                    //成功的状态码
                    response.CODE = 1;
                    if (result.data != null)
                    {
                        var pathList = (List<String>)result.data;
                        if (pathList != null && pathList.Count > 0)
                        {
                            foreach (var item in pathList)
                            {
                                FileItemDetail fileItem = new FileItemDetail();
                                fileItem.NO = no++;
                                fileItem.FILENAME = Path.GetFileName(item);
                                fileItem.FILEPATH = item;
                                response.FILE_LIST.Add(fileItem);
                            }
                        }
                    }
                }
                else
                {
                    //下载文件异常:{0}
                    response.MSG = String.Format(_localizer["error_down_exception"], result.msg);
                    ErrorInfo.Set(response.MSG, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                #endregion

            }
            catch (Exception ex)
            {
                response.MSG = ex.Message;
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return response;
        }

        /// <summary>
        /// 烧录完成上传结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<MesBurnFileResponseModel> UpdateRecorderResult([FromBody] MesBurnFileResultRequestModel model)
        {
            MesBurnFileResponseModel result = new MesBurnFileResponseModel();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 验证参数

                    if (!ErrorInfo.Status && !String.IsNullOrEmpty(model.MachineInfo.HandlerType))
                    {
                        //设备型号有错误，请注意检查!
                        ErrorInfo.Set(_localizer["error_device_wrong"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && !String.IsNullOrEmpty(model.MachineInfo.ProgrammerType))
                    {
                        //烧录器型号有错误，请注意检查!
                        ErrorInfo.Set(_localizer["error_burning_type_wrong"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model.Progress.ICTotal <= 0)
                    {
                        //烧录芯片数量 有错误，请注意检查!
                        ErrorInfo.Set(_localizer["error_burning_number_wrong"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && String.IsNullOrEmpty(model.LotInfo.LotNumber))
                    {
                        //工单号 有错误，请注意检查!
                        ErrorInfo.Set(_localizer["error_work_order_wrong"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 验证参数
                    if (!ErrorInfo.Status)
                    {
                        result.CODE = await _repository.InsertBurnResult(model) ? GlobalVariables.FailedCode : GlobalVariables.successCode;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    result.MSG = ex.Message;
                }
            }
            return result;
        }

    }
}
