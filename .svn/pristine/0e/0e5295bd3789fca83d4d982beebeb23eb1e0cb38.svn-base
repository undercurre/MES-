using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using Microsoft.AspNetCore.Http;
using JZ.IMS.Models;
using System.Reflection;
using JZ.IMS.WebApi.Public;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using JZ.IMS.IRepository;
using JZ.IMS.WebApi.Validation;
using FluentValidation.Results;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 设备点检维修控制器  
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsEquipRepairHeadController : BaseController
	{
		private readonly ISfcsEquipRepairHeadService _service;
		private readonly ISfcsEquipmentService _serviceEquipment;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISfcsEquipRepairDetailService _serviceEquipRepairDetail;
		private readonly ISfcsEquipmentLinesService _serviceLines;
		private readonly ISfcsParametersService _serviceParameters;
		private readonly IStringLocalizer<SfcsEquipRepairHeadController> _localizer;
		private readonly IStringLocalizer<SfcsEquipContentConfController> _confLocalizer;

		public SfcsEquipRepairHeadController(ISfcsEquipRepairHeadService service, ISfcsEquipmentService serviceEquipment,
			IHttpContextAccessor httpContextAccessor, ISfcsEquipRepairDetailService serviceEquipRepairDetail, ISfcsEquipmentLinesService serviceLines,
			ISfcsParametersService serviceParameters, IStringLocalizer<SfcsEquipRepairHeadController> localizer,
			IStringLocalizer<SfcsEquipContentConfController> confLocalizer)
		{
			_service = service;
			_serviceEquipment = serviceEquipment;
			_httpContextAccessor = httpContextAccessor;
			_serviceEquipRepairDetail = serviceEquipRepairDetail;
			_serviceLines = serviceLines;
			_serviceParameters = serviceParameters;
			_localizer = localizer;
			_confLocalizer = confLocalizer;
		}

		/// <summary>
		/// 设备点检首页
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
						returnVM.Result = new IndexResult()
						{
							CategoryList = _serviceParameters.GetEquipmentCategoryList(),
							EquipStatusList = GetEquipStatusList(),
							LinesList = _serviceLines.GetLinesList(),
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
		///根据设备ID查询对应的维修记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]SfcsEquipRepairHeadRequestModel model)
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
		/// 添加或修改视图
		/// </summary>
		/// <param name="ID">设备ID</param>
		/// <param name="UserName">用户名称</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public ApiBaseReturn<decimal> AddOrModify(decimal ID, string UserName)
		{
			ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					//判断设备状态是否为待维修
					SfcsEquipmentListModel model = _serviceEquipment.GetModelById(ID);
					decimal headID = 0;
					if (model != null && model.STATUS == 2)
					{
						SfcsEquipRepairHeadAddOrModifyModel item = new SfcsEquipRepairHeadAddOrModifyModel();
						item.EQUIP_ID = ID;
						item.REPAIR_CODE = "ER" + DateTime.Now.ToString("yyyyMMddHHmmss");
						item.REPAIR_USER = UserName;
						item.BEGINTIME = DateTime.Now;
						item.REPAIR_STATUS = 3;//维修中
						headID = int.Parse(_service.AddOrModifyAsync(item).Result.ResultData);
					}

					if (model != null && model.STATUS == 3)
					{
						//获取正在维修中的记录ID
						headID = _service.GetRepairID(ID) ?? 0;
					}
					returnVM.Result = headID;

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
		/// 保存操作
		/// </summary>
		/// <param name="item">请求体中的数据的映射</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody]SfcsEquipRepairHeadAddOrModifyModel item)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 保存并返回

					if (!ErrorInfo.Status)
					{
						item.BEGINTIME = DateTime.Now;
						item.ENDTIME = DateTime.Now;
						//item.REPAIR_USER = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;

						BaseResult result = new BaseResult();
						result = await _service.AddOrModifyAsync(item);
						if (result != null && result.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = true;
						}
						else if (result != null && result.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
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
		/// 删除
		/// </summary>
		/// <param name="id">主表ID</param>
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
						var result = await _service.DeleteAsync(id);
						if (result != null && result.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = true;
						}
						else if (result != null && result.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
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
		/// 根据设备条件获取设备信息
		/// </summary>
		/// <param name="model">设备条件对象</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> GetEquipmentList([FromQuery]SfcsEquipmentRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					var resdata = await _serviceEquipment.GetEquipmentList(model);
					returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
					returnVM.TotalCount = resdata.count;
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
		/// 根据维修记录获取维修配件信息
		/// </summary>
		/// <param name="headId">维修记录ID</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> GetEquipRepairDetail(decimal headId)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					returnVM.Result = JsonHelper.ObjectToJSON(await _serviceEquipRepairDetail.GetEquipRepairDetail(headId));
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
		/// 获取设备状态列表
		/// </summary>
		/// <returns></returns>
		private List<SelectListItem> GetEquipStatusList()
		{
			List<SelectListItem> returnValue = new List<SelectListItem>();
			try
			{
				returnValue.Add(new SelectListItem { Value = "", Text = "所有设备状态" });
				returnValue.Add(new SelectListItem { Value = "0", Text = "正常" });
				returnValue.Add(new SelectListItem { Value = "1", Text = "闲置" });
				returnValue.Add(new SelectListItem { Value = "2", Text = "待维修" });
				returnValue.Add(new SelectListItem { Value = "3", Text = "维修中" });
				returnValue.Add(new SelectListItem { Value = "4", Text = "报废" });
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return returnValue;
		}

		/// <summary>
		/// 首页返回类
		/// </summary>
		public class IndexResult
		{
			/// <summary>
			/// 设备分类
			/// </summary>
			public List<SfcsParameters> CategoryList { get; set; }

			/// <summary>
			/// 设备状态列表
			/// </summary>
			public List<SelectListItem> EquipStatusList { get; set; }

			/// <summary>
			/// 所有线别列表
			/// </summary>
			/// <returns></returns>
			public List<SfcsEquipmentLinesModel> LinesList { get; set; }
		}

		#endregion
	}
}