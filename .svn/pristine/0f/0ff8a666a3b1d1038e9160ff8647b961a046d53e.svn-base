
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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using JZ.IMS.Models;
using AutoMapper;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Validation;

/// <summary>
/// 设备点检内容配置控制器
/// </summary>
namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 设备点检内容配置控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsEquipContentConfController : BaseController
	{
		private readonly IStringLocalizer<SfcsEquipContentConfController> _localizer;
		private readonly ISfcsEquipContentConfService _service;
		private readonly ISfcsParametersService _serviceParameters;
		private readonly IHostingEnvironment _hostingEnv;
		private readonly ISOPRoutesService _serviceSOPRoutes;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SfcsEquipContentConfController(IStringLocalizer<SfcsEquipContentConfController> localizer, ISfcsEquipContentConfService service,
			ISfcsParametersService serviceParameters, IHostingEnvironment hostingEnv, ISOPRoutesService serviceSOPRoutes, IMapper mapper,
			IHttpContextAccessor httpContextAccessor)
		{
			_localizer = localizer;
			_httpContextAccessor = httpContextAccessor;
			_service = service;
			_serviceParameters = serviceParameters;
			_hostingEnv = hostingEnv;
			_serviceSOPRoutes = serviceSOPRoutes;
			_mapper = mapper;
		}

		/// <summary>
		/// 设备点检内容配置首页
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize("Permission")]
		public ApiBaseReturn<List<SfcsParameters>> Index()
		{
			ApiBaseReturn<List<SfcsParameters>> returnVM = new ApiBaseReturn<List<SfcsParameters>>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resdata = _serviceParameters.GetEquipmentCategoryList();
						returnVM.Result = resdata;
						returnVM.TotalCount = resdata?.Count() ?? 0;
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
		/// 查询列表
		/// 搜索按钮对应的处理也是这个方法
		/// </summary>
		/// <remarks>
		/// 说明:
		/// 返回数据: 设备点检内容配置列表: 
		/// 
		/// </remarks>
		/// <param name="model">查询条件模型</param>
		/// <returns>..</returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]SfcsEquipContentConfRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _service.LoadDataAsync(model);
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
		/// 添加或修改视图
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize("Permission")]
		public ApiBaseReturn<List<SfcsParameters>> AddOrModify()
		{
			ApiBaseReturn<List<SfcsParameters>> returnVM = new ApiBaseReturn<List<SfcsParameters>>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = _serviceParameters.GetEquipmentCategoryList();
					returnVM.Result = resdata;
					returnVM.TotalCount = resdata?.Count() ?? 0;

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
		/// <param name="item">请求体中的数据的映射</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody]SfcsEquipContentConfAddOrModifyModel item)
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
		/// 上传图片
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public ApiBaseReturn<bool> SOPInfo()
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			returnVM.Result = true;
			return returnVM;
		}

		/// <summary>
		/// 图片上传功能
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public ApiBaseReturn<SOP_OPERATIONS_ROUTES_RESOURCE> UploadImage()
		{
			ApiBaseReturn<SOP_OPERATIONS_ROUTES_RESOURCE> returnVM = new ApiBaseReturn<SOP_OPERATIONS_ROUTES_RESOURCE>();
			var imgFile = Request.Form.Files[0];
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
						//    return Json(new { code = 1, msg = "只允许上传jpg,png,gif格式的图片.", });
						//}

						#endregion

						#region 判断大小

						filesize = Convert.ToDecimal(Math.Round(imgFile.Length / 1024.00, 3));
						long mb = imgFile.Length / 1024 / 1024; // MB
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
						var path = $"/upload/sopfile/" + DateTime.Now.ToString("yyyyMM");
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
						var res_entity = new SOP_OPERATIONS_ROUTES_RESOURCE
						{
							ORDER_NO = 99,
							RESOURCE_TYPE = 0,//图片
							RESOURCE_URL = path + $"/{filenameNew}",
							RESOURCE_URL_THUMB = "",
							RESOURCE_NAME = resource_name,
							RESOURCE_SIZE = filesize,
							RESOURCES_CATEGORY = 5
						};

						returnVM.Result = res_entity;
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
		/// 资源列表
		/// </summary>
		/// <param name="mstId">设备点检事项主表id</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadImgList(string mstId)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (string.IsNullOrWhiteSpace(mstId))
					{
						mstId = "0";
					}
					var resdata = await _serviceSOPRoutes.GetEquipContentConfResource(Convert.ToDecimal(mstId));
					returnVM.Result = JsonHelper.ObjectToJSON(resdata);
					returnVM.TotalCount = resdata?.Count() ?? 0;

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
		/// 在首页点击编辑时执行的方法
		/// </summary>
		/// <param name="Id">要编辑的ID</param>
		/// <returns>JSON格式的响应结果</returns>				
		[HttpGet]
		[Authorize]
		public ApiBaseReturn<string> LoadEditData(decimal Id)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = _service.LoadEditDataAsync(Id);
					returnVM.Result = JsonHelper.ObjectToJSON(resdata);
					returnVM.TotalCount = resdata != null ? 1 : 0;

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
		public async Task<ApiBaseReturn<bool>> ChangeEnabled([FromBody]ChangeStatusModel item)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status)
					{
						ChangeStatusModelValidation validationRules = new ChangeStatusModelValidation(_localizer);
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

	}
}