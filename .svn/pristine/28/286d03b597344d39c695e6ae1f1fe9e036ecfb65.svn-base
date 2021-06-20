using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.Models;
using JZ.IMS.IRepository;
using System.Text;
using FluentValidation.Results;
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using AutoMapper;
using JZ.IMS.IServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Validation;
using JZ.IMS.WebApi.Public;
using System.Reflection;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 呼叫内容配置 控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CallContentConfigController : BaseController
	{
		private readonly ICallContentConfigService _service;
		private readonly ISfcsParametersService _andonCallTypeService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<MenuController> _menuLocalizer;
		private readonly ICallContentConfigRepository _repository;

		public CallContentConfigController(ICallContentConfigService service, ISfcsParametersService andonCallTypeService,
			IHttpContextAccessor httpContextAccessor, IStringLocalizer<MenuController> menuLocalizer, ICallContentConfigRepository repository)
		{
			_httpContextAccessor = httpContextAccessor;
			_menuLocalizer = menuLocalizer;
			_service = service;
			_andonCallTypeService = andonCallTypeService;
			_repository = repository;
		}

		/// <summary>
		/// 呼叫内容配置首页
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
						returnVM.Result = new IndexResult
						{
							CallTypeList = _service.GetAllCallType(),
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
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]CallContentConfigRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _service.LoadDataAsync(model);
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

			WriteLog(ref returnVM);

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
		public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]CallContentConfigRequestModel model)
		{
			ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var res = await _repository.GetExportData(model);
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
		public ApiBaseReturn<IndexResult> AddOrModify()
		{
			ApiBaseReturn<IndexResult> returnVM = new ApiBaseReturn<IndexResult>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						returnVM.Result = new IndexResult
						{
							CallTypeList = _andonCallTypeService.GetListByCondition()
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
		/// 添加或修改的相关操作
		/// </summary>
		/// <param name="item">请求体中的数据的映射</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> AddOrModifyServer([FromBody]CallContentAddOrModifyModel item)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					item.Description = "EN";

					#endregion

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
		/// 通过ID删除记录
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
					#region 检查参数

					#endregion

					#region 删除并返回

					if (!ErrorInfo.Status)
					{
						var resultData = await _service.DeleteIdsAsync(id);
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
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> ChangeEnabled([FromBody]ChangeStatusModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status)
					{
						ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_menuLocalizer);
						ValidationResult results = validationRules.Validate(model);
						if (!results.IsValid)
						{
							ErrorInfo.Set(results.Errors[0]?.ErrorMessage, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resultData = await _service.ChangeLockStatusAsync(model);
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

		#region 内部方法

		/// <summary>
		/// 首页返回类
		/// </summary>
		public class IndexResult
		{
			/// <summary>
			/// 获取所有呼叫类型（用于显示到搜索框）
			/// </summary>
			public List<SfcsParameters> CallTypeList { get; set; }
		}

		#endregion
	}
}
