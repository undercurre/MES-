/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备巡检配置 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-23 16:18:50                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IIpqaConfigController                                      
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
using Microsoft.AspNetCore.Http;
using JZ.IMS.IRepository;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 巡检配置控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class IpqaConfigController : BaseController
	{
		private readonly IIpqaConfigService _service;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IIpqaConfigRepository _repository;
		private readonly IStringLocalizer<IpqaConfigController> _localizer;

		public IpqaConfigController(IStringLocalizer<IpqaConfigController> localizer, IIpqaConfigService service, IHttpContextAccessor httpContextAccessor,
			IIpqaConfigRepository repository)
		{
			_localizer = localizer;
			_service = service;
			_httpContextAccessor = httpContextAccessor;
			_repository = repository;
		}

		/// <summary>
		/// 获取巡检配置列表首页
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
					#region 检查参数

					#endregion

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
		/// 查询所有
		/// 搜索按钮对应的处理也是这个方法
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]IpqaConfigRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && model != null && (model.Limit == 0 || model.Page == 0))
					{
						ErrorInfo.Set(_localizer["pageparam_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resultData = await _service.LoadDataAsync(model);
						if (resultData != null)
						{
							returnVM.Result = JsonHelper.ObjectToJSONOfDate(resultData.data);
							returnVM.TotalCount = resultData.count;
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
		/// 导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]IpqaConfigRequestModel model)
		{

			ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();

			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					#endregion

					#region 设置返回值
					var result = await _repository.GetExportData(model);
					returnVM.Result = result.data;
					returnVM.TotalCount = result.count;
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
		public ApiBaseReturn<Ipqa_type> AddOrModify(decimal? ipqa_type)
		{
			ApiBaseReturn<Ipqa_type> returnVM = new ApiBaseReturn<Ipqa_type>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						returnVM.Result = new Ipqa_type
						{
							ipqa_type = ipqa_type ?? 0,
							ipqa_type_name = ipqa_type == 1 ? "巡检分类：产线车间巡检" : "巡检分类：SMT车间巡检",
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
		/// 巡检项目是否已被使用 
		/// </summary>
		/// <param name="id">巡检项目id</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<bool>> IpqaConfigIsByUsed(decimal id)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			bool result = false;

			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						if (id > 0)
						{
							result = await _repository.IpqaConfigIsByUsed(id);
						}
						returnVM.Result = result;
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
		/// 通过ID删除记录
		/// </summary>
		/// <param name="id">要删除的记录的id</param>
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

					#region 设置返回值

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
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> SaveData([FromBody] IpqaConfigModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					//model.UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							item.CATEGORY = item.CATEGORY ?? "-";
							item.REFERENCE_STANDARD = item.REFERENCE_STANDARD ?? "-";
						}
					}
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							item.CATEGORY = item.CATEGORY ?? "-";
							item.REFERENCE_STANDARD = item.REFERENCE_STANDARD ?? "-";
						}
					}

					#endregion

					#region 保存并返回

					if (!ErrorInfo.Status)
					{
						decimal resdata = await _repository.SaveDataByTrans(model);
						if (resdata != -1)
						{
							returnVM.Result = true;
						}
						else
						{
							returnVM.Result = false;
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

	/// <summary>
	///  巡检类型
	/// </summary>
	public class Ipqa_type
	{
		/// <summary>
		/// 
		/// </summary>
		public decimal ipqa_type { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string ipqa_type_name { get; set; }
	}
}