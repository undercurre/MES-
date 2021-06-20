using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 设备点检看板 控制器  
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsEquipReportController : BaseController
	{
		private readonly ISfcsEquipmentService _serviceEquip;
		private readonly ISfcsEquipmentLinesService _serviceLines;
		private readonly ISfcsParametersService _serviceParameters;
		private readonly ISfcsEquipRepairHeadService _serviceRepairHead;
		private readonly ISfcsEquipRepairDetailService _serviceEquipRepairDetail;
		private readonly ISfcsEquipKeepService _serviceEquipKeep;
		private readonly ISfcsEquipReportService _service;
		private readonly ISfcsEquipKeepRepository _repository;

		public SfcsEquipReportController(ISfcsEquipmentService serviceEquip, ISfcsEquipmentLinesService serviceLines,
			ISfcsParametersService serviceParameters, ISfcsEquipRepairHeadService serviceRepairHead,
			ISfcsEquipRepairDetailService serviceEquipRepairDetail, ISfcsEquipKeepService serviceEquipKeep,
			ISfcsEquipReportService service, ISfcsEquipKeepRepository repository)
		{
			_serviceEquip = serviceEquip;
			_serviceLines = serviceLines;
			_serviceParameters = serviceParameters;
			_serviceRepairHead = serviceRepairHead;
			_serviceEquipRepairDetail = serviceEquipRepairDetail;
			_serviceEquipKeep = serviceEquipKeep;
			_service = service;
			_repository = repository;
		}

		/// <summary>
		/// 设备点检看板首页
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
						var equipmentCategoryList = _serviceParameters.GetEquipmentCategoryList();
						var linesList = _serviceLines.GetLinesList();

						returnVM.Result = new IndexResult
						{
							CategoryList = equipmentCategoryList,
							LinesList = linesList,
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
		/// 查询设备数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]SfcsEquipmentRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resdata = await _serviceEquip.GetEquipmentList(model);

						returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
						returnVM.TotalCount = resdata.count;
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
		/// 查询设备维修记录数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadRepairHeadData([FromQuery]SfcsEquipRepairHeadRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resdata = await _serviceRepairHead.LoadDataAsync(model);

						returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
						returnVM.TotalCount = resdata.count;
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
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resdata = await _serviceEquipRepairDetail.GetEquipRepairDetail(headId);

						returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
						returnVM.TotalCount = resdata.count;
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
		/// 获取年月日报表信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadEquipKeepData([FromQuery]EquipKeepRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var LineList = _serviceLines.GetLinesList();
						var CategoryList = _serviceParameters.GetEquipmentCategoryList();
						var resdata = await _serviceEquipKeep.LoadDataAsync(model);
						foreach (var item in (List<EquipKeepListModel>)resdata.data)
						{
							item.Line_Name = LineList.Where(t => t.ID == item.STATION_ID)
													 .Select(t => t.LINE_NAME)
													 .FirstOrDefault() ?? string.Empty;

							item.CATEGORY_Name = CategoryList.Where(t => t.LOOKUP_CODE == item.CATEGORY)
													 .Select(t => t.CHINESE)
													 .FirstOrDefault() ?? string.Empty;
						}

						returnVM.Result = JsonHelper.ObjectToJSONOfDate(resdata.data);
						returnVM.TotalCount = resdata.count;
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
		/// 获取年月日报表明细数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> GetEquipKeepDetail(decimal id)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resdata = await _service.GetEquipKeepDetail(id);

						returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
						returnVM.TotalCount = resdata.count;
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
		/// 获取年月日报表明细数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> GetEquipKeepDayDetail(decimal id)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resdata = await _service.GetEquipKeepDetail(id);

						returnVM.Result = JsonHelper.ObjectToJSON(resdata.data);
						returnVM.TotalCount = resdata.count;
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
			public List<SfcsParameters> CategoryList { get; set; }

			/// <summary>
			/// 获取所有线别列表
			/// </summary>
			public List<SfcsEquipmentLinesModel> LinesList { get; set; }

			/// <summary>
			/// 获取状态列表
			/// </summary>
			public List<SelectListItem> StatusList { get; set; }
		}

		/// <summary>
		/// 获取设备状态列表
		/// </summary>
		/// <returns></returns>
		private List<SelectListItem> GetStatusList()
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

		#endregion
	}
}