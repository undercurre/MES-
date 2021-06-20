using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// SOP(作业指导书) 控制器  
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SOPController : BaseController
	{
		private readonly ISOPRoutesService _service;
		private readonly ISOP_ROUTESRepository _repository;
		private readonly ISysEmployeeService _serviceSysEmployee;
		public SOPController(ISOPRoutesService service, ISOP_ROUTESRepository repository, ISysEmployeeService serviceSysEmployee)
		{
			_serviceSysEmployee = serviceSysEmployee;
			_repository = repository;
			_service = service;
		}

		/// <summary>
		/// 作业指导书 首页
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
			return returnVM;
		}

		/// <summary>
		/// 产品图:资源列表
		/// </summary>
		/// <param name="routeId"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadResourceProductData(decimal routeId)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _repository.LoadResourceProductData(routeId);
					returnVM.Result = JsonHelper.ObjectToJSON(resdata);

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
		/// 零件图:资源列表
		/// </summary>
		/// <param name="operationRouteId"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadResourceCmpData(decimal operationRouteId)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _repository.LoadResourceCmpData(operationRouteId);
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
		/// 加载作业图列表
		/// </summary>
		/// <param name="operationRouteId"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadResourceData(decimal operationRouteId)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _repository.LoadResourceData(operationRouteId);
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
		/// 获取员工信息
		/// </summary>
		/// <param name="user_id"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadEmployeeData(string user_id)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _serviceSysEmployee.LoadDataAsync(user_id);
					returnVM.Result = JsonHelper.ObjectToJSON(resdata);

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
		/// 获取工序评判标准
		/// </summary>
		/// <param name="site_id">站点ID</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadSkillStandard(decimal site_id)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _service.LoadSkillStandard(site_id);
					returnVM.Result = JsonHelper.ObjectToJSON(resdata);

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