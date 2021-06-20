using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Public;
using System.Reflection;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 呼叫通知 控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CallNoticeController : BaseController
	{
		private readonly IAndonCallNoticeService _service;
		private readonly ISfcsParametersService _andonCallTypeService;
		private readonly ISfcsOperationLinesService _sfcsOperationLinesService;
		private readonly IAndonCallNoticeReceiverService _andonCallNoticeReceiverService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CallNoticeController(IAndonCallNoticeService service, ISfcsParametersService andonCallTypeService, 
			ISfcsOperationLinesService sfcsOperationLinesService, IAndonCallNoticeService noticeService, 
			IAndonCallNoticeReceiverService andonCallNoticeReceiverService, IHttpContextAccessor httpContextAccessor)
		{
			_service = service;
			_andonCallTypeService = andonCallTypeService;
			_sfcsOperationLinesService = sfcsOperationLinesService;
			_andonCallNoticeReceiverService = andonCallNoticeReceiverService;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// 呼叫通知首页
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
						List<SfcsParameters> _callTypeList = _andonCallTypeService.GetListByCondition();
						List<SfcsOperationLines> _sfcsOperationLinesList = _sfcsOperationLinesService.GetLinesList();

						returnVM.Result = new IndexResult
						{
							CallTypeList = _callTypeList,
							LinesList = _sfcsOperationLinesList,
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
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]AndonCallNoticeRequestModel model)
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
		/// 查询接收信息集
		/// </summary>
		/// <param name="MST_ID"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadDataReceiver(string MST_ID)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _andonCallNoticeReceiverService.LoadDataAsync(MST_ID);
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

		#region 内部方法

		/// <summary>
		/// 首页返回类
		/// </summary>
		public class IndexResult
		{
			/// <summary>
			/// 呼叫类型
			/// </summary>
			public List<SfcsParameters> CallTypeList { get; set; }

			/// <summary>
			/// 获取激活的线体集合
			/// </summary>
			public List<SfcsOperationLines> LinesList { get; set; }
		}

		#endregion
	}
}