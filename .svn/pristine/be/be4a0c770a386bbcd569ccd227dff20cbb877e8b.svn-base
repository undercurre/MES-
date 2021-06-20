using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using JZ.IMS.IRepository;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SQLDataController : BaseController
	{
		private readonly IHostingEnvironment _hostingEnv;
		private readonly ISfcsModelRepository _repository;
		private readonly IStringLocalizer<SQLDataController> _localizer;

		public SQLDataController(IHostingEnvironment hostingEnv, IStringLocalizer<SQLDataController> localizer, ISfcsModelRepository repository)
		{
			_hostingEnv = hostingEnv;
			_localizer = localizer;
			_repository = repository;

		}

		#region SQL

		/// <summary>
		/// SQL
		/// </summary>
		/// <returns></returns>
		//[HttpGet]
		//[Authorize]
		//public ApiBaseReturn<List<decimal>> GetDataBySQL([FromQuery]string sql)
		//{
		//	ApiBaseReturn<List<decimal>> returnVM = new ApiBaseReturn<List<decimal>>();

		//	#region 验证参数
		//	if (!ErrorInfo.Status&&string.IsNullOrWhiteSpace(sql))
		//	{
		//		ErrorInfo.Set(_localizer["No_Null"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
		//	}
		//	#endregion

		//	if (!ErrorInfo.Status)
		//	{
		//		try
		//		{
		//			_repository.GetListByTable();




		//		}
		//		catch (Exception ex)
		//		{
		//			ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
		//		}
		//	}

		//	#region 如果出现错误，则写错误日志并返回错误内容

		//	if (ErrorInfo.Status)
		//	{
		//		returnVM.ErrorInfo.Set(ErrorInfo);
		//		if (ErrorInfo.ErrorType == EnumErrorType.Error)
		//		{
		//			CreateErrorLog(ErrorInfo);
		//		}
		//		ErrorInfo.Clear();
		//	}

		//	#endregion

		//	return returnVM;
		//} 

		#endregion
	}
}