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

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 通用错误信息控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class GeneralErrorController : BaseController
	{
		private readonly ISmtFeederRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<GeneralErrorController> _localizer;
		
		public GeneralErrorController(ISmtFeederRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<GeneralErrorController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}

		/// <summary>
		/// 首页视图
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> Index()
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status) {
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

					ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
				}
			}

			#region 如果出现错误，则写错误日志并返回错误内容

			WriteLog(ref returnVM);

			#endregion

			return returnVM;
	    }

	}
}