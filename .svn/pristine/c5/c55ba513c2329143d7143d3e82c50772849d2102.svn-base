/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-08 15:21:14                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISfcsCustomersComplaintController                                      
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
using JZ.IMS.WebApi.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using JZ.IMS.IRepository;
using JZ.IMS.Core.Extensions;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.Extensions.Localization;
using JZ.IMS.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;

namespace JZ.IMS.Admin.Controllers  
{
    /// <summary>
    /// 客户投诉
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsCustomersComplaintController : BaseController
	{
        private readonly ISfcsCustomersComplaintRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ShareResourceController> _localizer;
        private readonly IHostingEnvironment hostingEnv;
        public SfcsCustomersComplaintController(ISfcsCustomersComplaintRepository repository, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<ShareResourceController> localizer, IHostingEnvironment hostingEnv)
		{
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            this.hostingEnv = hostingEnv;
        }

		
		/// <summary>
		/// 查询所有
		/// 搜索按钮对应的处理也是这个方法
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>	
        [HttpGet]
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]SfcsCustomersComplaintRequestModel model)
		{
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.LoadData(model);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
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
        /// 通过ID查询已经保存的资源图片
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<string>> GetPhotoList(decimal ID)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var resdata = await _repository.GetPhotoList(ID);
                    returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
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
		/// 获取客户信息
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<ApiBaseReturn<List<SfcsCustomers>>> GetCustomerList()
        {
            ApiBaseReturn<List<SfcsCustomers>> returnVM = new ApiBaseReturn<List<SfcsCustomers>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var customerList =await _repository.GetCustomerList();
                        returnVM.Result = customerList;
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
		/// 获取客户信息
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public async Task<ApiBaseReturn<string>> GetPartDesc(string Code)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var customerList = await _repository.GetPartDesc(Code);
                        returnVM.Result = customerList;
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
        /// 添加或修改的相关操作
        /// </summary>
        /// <param name="item">请求体中的数据的映射</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        public async Task<ApiBaseReturn<decimal>> AddOrModifyAsync(SfcsCustomersComplaintAddOrModifyModel item)
		{
            var result = new BaseResult();
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        SfcsCustomersComplaint model;
                        if (item.ID == 0)
                        {
                            //TODO ADD
                            item.CREATE_TIME = DateTime.Now;
                            model = _mapper.Map<SfcsCustomersComplaint>(item);
                            model.ID = await Task.Run(() => { return _repository.GetSEQID(); });
                            //model.Is_Delete = "N";
                            //model.Add_Time = DateTime.Now;
                            if (await _repository.InsertAsync(model) > 0)
                            {
                                result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
                                result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
                            }
                            else
                            {
                                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                                result.ResultMsg = ResultCodeAddMsgKeys.CommonExceptionMsg;
                            }
                        }
                        else
                        {
                            //TODO Modify
                            model = _repository.Get(item.ID);
                            if (model != null)
                            {
                                _mapper.Map(item, model);
                                //model.Modify_Time = DateTime.Now;
                                if (await _repository.UpdateAsync(model) > 0)
                                {
                                    result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
                                    result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
                                }
                                else
                                {
                                    result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                                    result.ResultMsg = ResultCodeAddMsgKeys.CommonExceptionMsg;
                                }
                            }
                            else
                            {
                                result.ResultCode = ResultCodeAddMsgKeys.CommonFailNoDataCode;
                                result.ResultMsg = ResultCodeAddMsgKeys.CommonFailNoDataMsg;
                            }
                        }
                        returnVM.Result = result.ResultCode;
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
		/// 通过ID删除记录
		/// </summary>
		/// <param name="Id">要删除的记录的ID</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		public async Task<ApiBaseReturn<int>> DeleteOneById(decimal Id)
		{
            ApiBaseReturn<int> returnVM = new ApiBaseReturn<int>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var count = await _repository.DeleteAsync(Id);
                        returnVM.Result = count;
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
        public async Task<ApiBaseReturn<string>> UploadImage(decimal ID)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();

            var imgFile = Request.Form.Files;
            var resource_name = string.Empty;
            var filename = string.Empty;
            var extname = string.Empty;
            long size = 0;
            decimal filesize = 0;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && (imgFile == null || imgFile[0].FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(imgFile[0].ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        resource_name = filename;

                        #region 判断后缀

                        //if (!extname.ToLower().Contains("jpg") && !extname.ToLower().Contains("png") && !extname.ToLower().Contains("gif"))
                        //{
                        //    return Json(new { code = 1, msg = "只允许上传jpg,png,gif格式的图片.", });
                        //}

                        #endregion

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(imgFile[0].Length / 1024.00, 3));
                        long mb = imgFile[0].Length / 1024 / 1024; // MB
                        if (mb > 5)
                        {
                            //"只允许上传小于 5MB 的图片."
                            ErrorInfo.Set(_localizer["upload_size_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion
                    }

                    #endregion

                    #region 保存文件并设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var filenameNew = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var path = $"upload/sopfile/" + DateTime.Now.ToString("yyyyMM");
                        var pathWebRoot = AppContext.BaseDirectory + path;
                        if (Directory.Exists(pathWebRoot) == false)
                        {
                            Directory.CreateDirectory(pathWebRoot);
                        }
                        filename = pathWebRoot + $"/{filenameNew}";

                        size += imgFile[0].Length;
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            imgFile[0].CopyTo(fs);
                            fs.Flush();
                        }

                        //保存资源

                        returnVM.Result = JsonHelper.ObjectToJSON(await _repository.SavePhoto(ID, filename, filenameNew));
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
        /// 删除图片
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiBaseReturn<int>> DeleteFile(decimal ID) {
            ApiBaseReturn<int> returnVM = new ApiBaseReturn<int>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        var count = _repository.DeleteFile(ID);
                        returnVM.Result = count;
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
    }
}