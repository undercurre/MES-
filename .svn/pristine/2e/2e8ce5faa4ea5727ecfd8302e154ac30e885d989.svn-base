using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Reflection;
using JZ.IMS.WebApi.Public;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 巡检管理控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class IpqaMstController : BaseController
	{
		private readonly IIpqaMstService _service;
		private readonly IIpqaMstRepository _repository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISfcsEquipmentLinesRepository _linesRepository;
		private readonly ISfcsParametersRepository _parameterRepository;
		private readonly IStringLocalizer<IpqaMstController> _localizer;

		public IpqaMstController(IStringLocalizer<IpqaMstController> localizer, IIpqaMstService service, IIpqaMstRepository repository,
			IHttpContextAccessor httpContextAccessor, ISfcsEquipmentLinesRepository linesRepository,
			ISfcsParametersRepository parameterRepository)
		{
			_localizer = localizer;
			_service = service;
			_httpContextAccessor = httpContextAccessor;
			_repository = repository;
			_linesRepository = linesRepository;
			_parameterRepository = parameterRepository;
		}

		/// <summary>
		/// 获取巡检列表首页
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
					#region 检查参数

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						returnVM.Result = new IndexResult
						{
							LineList = _linesRepository.GetLinesList(),
							BusinessUnitsList = _parameterRepository.GetBusinessUnitsList(),
							DepartmentList = _parameterRepository.GetDepartmentList(),
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
		/// 查询列表
		/// 搜索按钮对应的处理也是这个方法
		/// </summary>
		/// <remarks>
		/// 说明:
		/// 返回数据: 巡检主表数据集: 
		/// 
		/// </remarks>
		/// <param name="model">巡检主表查询条件实体</param>
		/// <returns>..</returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]IpqaMstRequestModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var LineList = _linesRepository.GetLinesList();
					var businessUnitList = _parameterRepository.GetBusinessUnitsList();
					var departmentList = _parameterRepository.GetDepartmentList();

					var resdata = await _service.LoadDataAsync(model);
					foreach (var item in (List<IpqaMst>)resdata.data)
					{
						item.U_LINE = LineList.Where(t => t.ID == item.U_LINE_ID)
												 .Select(t => t.LINE_NAME)
												 .FirstOrDefault() ?? string.Empty;

						item.BUSINESS_UNIT = businessUnitList.Where(t => t.ID == item.BUSINESS_UNIT_ID)
												 .Select(t => t.CHINESE)
												 .FirstOrDefault() ?? string.Empty;

						item.DEPARTMENT = departmentList.Where(t => t.ID == item.DEPARTMENT_ID)
												 .Select(t => t.CHINESE)
												 .FirstOrDefault() ?? string.Empty;
					}

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
		/// 查询明细数据
		/// </summary>
		/// <param name="mst_id">巡检主表id</param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadDtlData(decimal mst_id)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var tmpdata = await _repository.LoadDetailAsync(mst_id);
					returnVM.Result = JsonHelper.ObjectToJSONOfDate(tmpdata);

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
		/// 获取配置列表
		/// </summary>
		/// <param name="ipqa_type">巡检类型</param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> GetIpqaConfig(decimal ipqa_type)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var tmpdata = await _repository.GetIpqaConfigAsync(ipqa_type);
					returnVM.Result = JsonHelper.ObjectToJSON(tmpdata);

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
		/// 获取工单信息列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> GetWoList([FromQuery] PageModel model)
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var tmpdata = await _repository.GetWoList(model);
					returnVM.Result = JsonHelper.ObjectToJSONOfDate(tmpdata.data);
					returnVM.TotalCount = tmpdata.count;

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
		/// 获取部门信息列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public ApiBaseReturn<string> GetDepartmentList()
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					var tmpdata = _parameterRepository.GetDepartmentList();
					returnVM.Result = JsonHelper.ObjectToJSONOfDate(tmpdata);

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
		public async Task<ApiBaseReturn<AddOrModifyResult>> AddOrModify(decimal ipqa_type, decimal id = 0)
		{
			ApiBaseReturn<AddOrModifyResult> returnVM = new ApiBaseReturn<AddOrModifyResult>();
			AddOrModifyResult resdata = new AddOrModifyResult();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					IpqaMst item = await _repository.LoadMainAsync(id);
					resdata.ID = id;
					resdata.ipqa_type = ipqa_type;
					resdata.ipqa_type_name = ipqa_type == 1 ? "产线车间巡检" : "SMT车间巡检";
					resdata.CHECK_STATUS = 4; //默认新增
					resdata.LineList = _linesRepository.GetLinesList();
					resdata.BusinessUnitsList = _parameterRepository.GetBusinessUnitsList();
					resdata.DepartmentList = _parameterRepository.GetDepartmentList();

					if (item != null)
					{
						resdata.BILL_CODE = item.BILL_CODE;
						resdata.BUSINESS_UNIT_ID = item.BUSINESS_UNIT_ID;
						resdata.DEPARTMENT_ID = item.DEPARTMENT_ID;
						resdata.U_LINE_ID = item.U_LINE_ID;
						resdata.PRODUCT_NAME = item.PRODUCT_NAME;
						resdata.PRODUCT_MODEL = item.PRODUCT_MODEL;
						resdata.PRODUCT_DATE = item.PRODUCT_DATE.ToString("yyyy-MM-dd");
						resdata.PRODUCT_BILLNO = item.PRODUCT_BILLNO;
						resdata.PRODUCT_QTY = item.PRODUCT_QTY;
						resdata.CREATEDATE = item.CREATEDATE.ToString("yyyy-MM-dd");
						resdata.CREATETIME = item.CREATETIME;
						resdata.CHECK_STATUS = item.CHECK_STATUS;
						resdata.CHECK_STATUS_CAPTION = GetCHECK_STATUS_CAPTION(item.CHECK_STATUS);
					}

					returnVM.Result = resdata;
					returnVM.TotalCount = 1;

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
		public async Task<ApiBaseReturn<bool>> SaveData([FromBody] IpqaMstModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					//model.CREATOR = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? string.Empty;
					if (model.ID == 0)
					{
						model.BILL_CODE = string.Format("IA{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
						model.CREATEDATE = DateTime.Now;
						model.CHECK_STATUS = 4; //4: 新增
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

		/// <summary>
		/// 删除巡检记录
		/// </summary>
		/// <param name="id">巡检主表id</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> Delete(decimal id)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					var billStatus = await GetBillStatus(id);
					if (billStatus != 4)
					{
						var result = new BaseResult();
						result.ResultCode = ResultCodeAddMsgKeys.CommonBillNotIsNewCode;
						result.ResultMsg = ResultCodeAddMsgKeys.CommonBillNotIsNewMsg;

						returnVM.Result = false;
						//通用提示类的本地化问题处理
						string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
						ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

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

		#region 更改状态

		/// <summary>
		/// 提交审核
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> PostToCheck([FromBody]BillStatusModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					//if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model?.Operator))
					//{
					//	// "操作员不能为空.";
					//	ErrorInfo.Set(_localizer["Operator_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					//}

					if (!ErrorInfo.Status)
					{
						var billStatus = await GetBillStatus(model.ID);
						if (billStatus != 4)
						{
							// "单据不是新增状态,不允许提交审核.";
							ErrorInfo.Set(_localizer["PostToCheck_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						model.Operator = string.Empty;
						model.OperatorDatetime = null;
						model.NewStatus = 0;
						var resdata = await _repository.UpdateStatusById(model);
						if (resdata > 0)
						{
							returnVM.Result = true;
						}
						else
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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
		/// 审核
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> CheckBill([FromBody]BillStatusModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model?.Operator))
					{
						// "操作员不能为空.";
						ErrorInfo.Set(_localizer["Operator_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					if (!ErrorInfo.Status)
					{
						var result = new BaseResult();
						var billStatus = await GetBillStatus(model.ID);
						if (billStatus != 0)
						{
							result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
							result.ResultMsg = ResultCodeAddMsgKeys.CommonBillisCheckedMsg;
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
							ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						model.OperatorDatetime = DateTime.Now;
						model.NewStatus = 1;
						var resdata = await _repository.UpdateStatusById(model);
						if (resdata > 0)
						{
							returnVM.Result = true;
						}
						else
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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
		/// 拒绝
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> RejectBill([FromBody]BillStatusModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && string.IsNullOrWhiteSpace(model?.Operator))
					{
						// "操作员不能为空.";
						ErrorInfo.Set(_localizer["Operator_Error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					if (!ErrorInfo.Status)
					{
						var result = new BaseResult();
						var billStatus = await GetBillStatus(model.ID);
						if (billStatus != 0)
						{
							result.ResultCode = ResultCodeAddMsgKeys.CommonBillisNotCheckedCode;
							result.ResultMsg = ResultCodeAddMsgKeys.CommonBillIsNotCheckedMsg;
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
							ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						model.OperatorDatetime = DateTime.Now;
						model.NewStatus = 3;
						var resdata = await _repository.UpdateStatusById(model);
						if (resdata > 0)
						{
							returnVM.Result = true;
						}
						else
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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
		/// 取消审核
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> UnCheckBill([FromBody]BillStatusModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status)
					{
						var result = new BaseResult();
						var billStatus = await GetBillStatus(model.ID);
						if (billStatus != 1)
						{
							result.ResultCode = ResultCodeAddMsgKeys.CommonBillisNotCheckedCode;
							result.ResultMsg = ResultCodeAddMsgKeys.CommonBillIsNotCheckedMsg;
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, result.ResultCode, result.ResultMsg);
							ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						model.Operator = string.Empty;
						model.OperatorDatetime = null;
						model.NewStatus = 0;
						var resdata = await _repository.UpdateStatusById(model);
						if (resdata > 0)
						{
							returnVM.Result = true;
						}
						else
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode, ResultCodeAddMsgKeys.CommonExceptionMsg);
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

		#endregion

		#region 内部方法

		/// <summary>
		/// 单据状态: (0：待审核，1：已审核，3：拒绝)
		/// </summary>
		/// <param name="id">单据ID</param>
		/// <returns></returns>
		private async Task<decimal> GetBillStatus(decimal id)
		{
			return await _repository.GetBillStatus(id);
		}

		/// <summary>
		/// 获取设备点检状态说明
		/// </summary>
		/// <param name="check_status"></param>
		/// <returns></returns>
		private string GetCHECK_STATUS_CAPTION(decimal? check_status)
		{
			string returnValue = string.Empty;
			try
			{
				if (check_status == 0)
					returnValue = "待审核";
				else if (check_status == 1)
					returnValue = "已审核";
				else if (check_status == 3)
					returnValue = "拒绝";
				else if (check_status == 4)
					returnValue = "新增";
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return returnValue;
		}

		#endregion

		/// <summary>
		/// 巡检列表首页返回类
		/// </summary>
		public class IndexResult
		{
			/// <summary>
			/// 线别列表
			/// </summary>
			/// <returns></returns>
			public List<SfcsEquipmentLinesModel> LineList { get; set; }

			/// <summary>
			/// 经营单位列表
			/// </summary>
			public List<SfcsParameters> BusinessUnitsList { get; set; }

			/// <summary>
			/// 部门列表
			/// </summary>
			public List<SfcsDepartment> DepartmentList { get; set; }
		}

		/// <summary>
		/// 巡检列表明细返回类
		/// </summary>
		public class AddOrModifyResult
		{
			public decimal ID { get; set; }

			/// <summary>
			/// 巡检分类(0:SMT车间巡检，1:产线车间巡检)
			/// </summary>
			public decimal ipqa_type { get; set; }

			public string ipqa_type_name { get; set; }

			/// <summary>
			/// 
			/// </summary>
			public decimal CHECK_STATUS { get; set; }

			public string BILL_CODE { get; set; }

			public decimal BUSINESS_UNIT_ID { get; set; }

			public decimal DEPARTMENT_ID { get; set; }

			public decimal U_LINE_ID { get; set; }

			public string PRODUCT_NAME { get; set; }

			public string PRODUCT_MODEL { get; set; }

			public string PRODUCT_DATE { get; set; }

			public string PRODUCT_BILLNO { get; set; }

			public decimal PRODUCT_QTY { get; set; }

			public string CREATEDATE { get; set; }

			public string CREATETIME { get; set; }

			public string CHECK_STATUS_CAPTION { get; set; }

			/// <summary>
			/// 线别列表
			/// </summary>
			/// <returns></returns>
			public List<SfcsEquipmentLinesModel> LineList { get; set; }

			/// <summary>
			/// 经营单位列表
			/// </summary>
			public List<SfcsParameters> BusinessUnitsList { get; set; }

			/// <summary>
			/// 部门列表
			/// </summary>
			public List<SfcsDepartment> DepartmentList { get; set; }
		}

	}
}