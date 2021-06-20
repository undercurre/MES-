using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.Services;
using FluentValidation.Results;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using JZ.IMS.Models;
using JZ.IMS.WebApi.Validation;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using JZ.IMS.IRepository;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 生产线体配置 控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsOperationLinesController : BaseController
	{
		private readonly ISfcsOperationLinesService _service;
		private readonly IStringLocalizer<MenuController> _menuLocalizer;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISfcsOperationLinesRepository _repository;
		private readonly IStringLocalizer<ShareResourceController> _localizer;

		public SfcsOperationLinesController(ISfcsOperationLinesService service, IStringLocalizer<MenuController> menuLocalizer,
			IHttpContextAccessor httpContextAccessor, ISfcsOperationLinesRepository repository, IStringLocalizer<ShareResourceController> localizer)
		{
			_service = service;
			_httpContextAccessor = httpContextAccessor;
			_menuLocalizer = menuLocalizer;
			_repository = repository;
			_localizer = localizer;
		}

		/// <summary>
		/// 线体配置 首页
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

			WriteLog(ref returnVM);

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
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]SfcsOperationLineRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && (model.USER_ID ?? 0) <= 0)
					{
						ErrorInfo.Set(_localizer["USER_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{

						var resdata = await _repository.LoadData(model);
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
		public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SfcsOperationLineRequestModel model)
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
		[Authorize]
		public ApiBaseReturn<DetailResult> AddOrModify()
		{
			ApiBaseReturn<DetailResult> returnVM = new ApiBaseReturn<DetailResult>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						returnVM.Result = new DetailResult
						{
							PhyLocList = _service.GetPhyLocList(),
							PlantCodeList = _service.GetPlantCodeList(),
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
		public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody]SfcsOperationLinesAddOrModifyModel item)
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

			WriteLog(ref returnVM);

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
						ManagerLockStatusModelValidation validationRules = new ManagerLockStatusModelValidation(_menuLocalizer);
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
						var resultData = await _service.ChangeLockStatusAsync(item);
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

			WriteLog(ref returnVM);

			#endregion

			return returnVM;
		}

		#region 内部方法

		/// <summary>
		/// 明细返回类
		/// </summary>
		public class DetailResult
		{
			/// <summary>
			/// 物理位置列表
			/// </summary>
			public List<SfcsParameters> PhyLocList { get; set; }

			/// <summary>
			/// 厂别列表
			/// </summary>
			public List<SfcsParameters> PlantCodeList { get; set; }
		}

		#endregion
	}
}