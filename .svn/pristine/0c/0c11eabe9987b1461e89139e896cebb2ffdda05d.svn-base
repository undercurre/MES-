/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-11 10:19:01                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtFeederController                                      
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
using FluentValidation.Results;
using JZ.IMS.WebApi.Validation;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using JZ.IMS.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 替换条码控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SmtFeederReplaceController : BaseController
	{
		private readonly ISmtFeederRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SmtFeederReplaceController> _localizer;
		
		public SmtFeederReplaceController(ISmtFeederRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SmtFeederReplaceController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}

		public class IndexVM
		{
			/// <summary>
			/// 获取状态类型列表
			/// </summary>
			/// <returns></returns>
			public List<IDNAME> StatusList { get; set; }
		}

		/// <summary>
		/// 首页视图
		/// </summary>
		/// <returns>返回一个状态类型列表</returns>
		[HttpGet]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<IndexVM>> Index()
		{
			ApiBaseReturn<IndexVM> returnVM = new ApiBaseReturn<IndexVM>();
			if (!ErrorInfo.Status) {
				try
				{
					#region 设置返回值
					if (!ErrorInfo.Status)
					{
						returnVM.Result = new IndexVM
						{
							StatusList = await _repository.GetStatus(),
						};
					}

					#endregion
				}
				catch (Exception ex)
				{

					ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
				}
			}

			#region 如果出现错误，则写错误日志并返回错误内容

			WriteLog(ref returnVM);

			#endregion

			return returnVM;
		}

		#region 查询Action 
		// <summary>
		// 根据原条码是否存在
		// 如果存在就返回数据
		// </summary>
		// <param name = "model" ></ param >
		// <param name = "FEEDER" >料架编号</ param >
		// < returns ></ returns >
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<List<SmtFeeder>>> LoadData([FromQuery]SmtFeederRequestModel model)
		{

			ApiBaseReturn<List<SmtFeeder>> returnVM = new ApiBaseReturn<List<SmtFeeder>>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数
					if (!ErrorInfo.Status&&!model.FEEDER.IsNullOrWhiteSpace())
					{
					  var tmpdata = await _repository.ItemIsByFeeder(model.FEEDER);
						if (tmpdata==null)
						{
							ErrorInfo.Set(_localizer["Feeder_noeixt_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
					}
					#endregion

					#region 设置返回值
					if (!ErrorInfo.Status)
					{
						int count = 0;
						string conditions = " WHERE ID > 0 ";
						if (!model.FEEDER.IsNullOrWhiteSpace())
						{
						   conditions += $" AND (FEEDER=:FEEDER) ";
						}
						var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
						//var viewList = new List<SmtFeederListModel>();
						//list?.ForEach(x =>
						//{
						//	var item = _mapper.Map<SmtFeederListModel>(x);
						//	viewList.Add(item);
						//});

						count = await _repository.RecordCountAsync(conditions, model);

						returnVM.Result = list;
						returnVM.TotalCount = count;
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

		/// <summary>
		/// 查新条码是否已经存在,存在就为true,不存在为false
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> QueryByNewFeeder(string feeder)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			returnVM.Result = true;
			if (!ErrorInfo.Status)
			{
				try
				{
					if (!ErrorInfo.Status&&feeder.IsNullOrWhiteSpace())
					{
						// throw new Exception("请输入新料架编号！！");
						ErrorInfo.Set(_localizer["Newfeeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}
					if (!ErrorInfo.Status)
					{
						var tmpdata = await _repository.ItemIsByFeeder(feeder);
						if (tmpdata!=null)
						{
							string msg = string.Format(_localizer["NewFeeder_noeixt_error"], feeder);
							ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
						else
						{
							returnVM.Result = false;
						}
					}
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
		/// 保存数据,
		/// 只使用更新的信息(updateRecords)就可以，
		/// 传一个ID和需要更新的新条码(FEEDER)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtFeederModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数
					if (!ErrorInfo.Status && model.updateRecords[0].FEEDER.IsNullOrWhiteSpace())
					{
						// throw new Exception("请输入新料架编号！！");
						ErrorInfo.Set(_localizer["Newfeeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}
					if (!ErrorInfo.Status)
					{
						var tmpdata = await _repository.ItemIsByFeeder(model.updateRecords[0].FEEDER);
						if (tmpdata != null)
						{
							string msg = string.Format(_localizer["NewFeeder_noeixt_error"], model.updateRecords[0].FEEDER);
							ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
					}
						#endregion

						#region 保存并返回

						if (!ErrorInfo.Status)
					{
						decimal resdata = await _repository.SaveUpdateByTrans(model);
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
}