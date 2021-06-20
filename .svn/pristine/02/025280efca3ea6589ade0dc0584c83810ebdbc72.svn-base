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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 呼叫记录 控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CallRecordController : BaseController
	{
		private readonly IAndonCallRecordService _service;
		private readonly IAndonCallRecordHandleService _handleService;
		private readonly ISfcsParametersService _andonCallTypeService;
		private readonly ISfcsOperationLinesService _sfcsOperationLinesService;
		private readonly IAndonCallNoticeService _noticeService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CallRecordController(IAndonCallRecordService service, ISfcsParametersService andonCallTypeService,
			ISfcsOperationLinesService sfcsOperationLinesService, IAndonCallRecordHandleService handleService,
			IAndonCallNoticeService noticeService, IHttpContextAccessor httpContextAccessor)
		{
			_service = service;
			_andonCallTypeService = andonCallTypeService;
			_sfcsOperationLinesService = sfcsOperationLinesService;
			_handleService = handleService;
			_noticeService = noticeService;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// 呼叫记录首页
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
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
							StatusList = GetStatusList(),
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
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]AndonCallRecordRequestModel model)
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
		/// 查询呼叫记录处理
		/// </summary>
		/// <param name="MST_ID">呼叫记录ID</param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadDataHandle(string MST_ID)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _handleService.LoadDataByMstIdAsync(MST_ID);
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
		/// 获取通知列表
		/// </summary>
		/// <param name="MST_ID">呼叫记录ID</param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadDataNotice(string MST_ID)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var resdata = await _noticeService.LoadDataByMstIdAsync(MST_ID);
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
		/// 添加处理记录视图
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public ApiBaseReturn<bool> AddOrModifyHandle()
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
		/// 保存添加处理记录
		/// </summary>
		/// <param name="item">请求体中的数据的映射</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<bool>> AddOrModifyHandleSave([FromBody]AndonCallRecordHandleAddOrModifyModel item)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					//item.HANDLER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
					item.HANDLE_TIME = DateTime.Now;

					#region 保存并返回

					if (!ErrorInfo.Status)
					{
						var resultData = await _handleService.AddOrModifyAsync(item);
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
			/// 呼叫类型
			/// </summary>
			public List<SfcsParameters> CallTypeList { get; set; }

			/// <summary>
			/// 获取激活的线体集合
			/// </summary>
			public List<SfcsOperationLines> LinesList { get; set; }

			/// <summary>
			/// 获取状态列表
			/// </summary>
			public List<SelectListItem> StatusList { get; set; }
		}

		/// <summary>
		/// 获取状态列表
		/// </summary>
		/// <returns></returns>
		private List<SelectListItem> GetStatusList()
		{
			List<SelectListItem> returnValue = new List<SelectListItem>();
			try
			{
				returnValue.Add(new SelectListItem { Value = "", Text = "所有状态" });
				returnValue.Add(new SelectListItem { Value = "0", Text = "待处理" });
				returnValue.Add(new SelectListItem { Value = "1", Text = "处理中" });
				returnValue.Add(new SelectListItem { Value = "2", Text = "处理成功" });
				returnValue.Add(new SelectListItem { Value = "3", Text = "处理失败" });
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return returnValue;
		}

		#endregion
	}
}