using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ValuesController : BaseController
	{
		private readonly IStringLocalizer<ValuesController> _localizer;

		public ValuesController(IStringLocalizer<ValuesController> localizer)
		{
			_localizer = localizer;
		}

		// GET api/values1
		[HttpGet]
		public ActionResult<IEnumerable<string>> value1()
		{
			return new string[] { "value1", "value1" };
		}

		// GET api/values2
		/**
         * 该接口用Authorize特性做了权限校验，如果没有通过权限校验，则http返回状态码为401
         * 调用该接口的正确姿势是：
         * 1.登陆，调用api/Auth接口获取到token
         * 2.调用该接口 api/value2 在请求的Header中添加参数 Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOiIxNTYwMzM1MzM3IiwiZXhwIjoxNTYwMzM3MTM3LCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiemhhbmdzYW4iLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAifQ.1S-40SrA4po2l4lB_QdzON_G5ZNT4P_6U25xhTcl7hI
         * Bearer后面有空格，且后面是第一步中接口返回的token值
         * */
		[HttpGet]
		[Authorize]
		public ActionResult<IEnumerable<string>> value2()
		{
			//这是获取自定义参数的方法
			var auth = HttpContext.AuthenticateAsync().Result.Principal.Claims;
			var userName = auth.FirstOrDefault(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
			return new string[] { "这个接口登陆过的用户都可以访问", $"userName={userName}" };
		}

		/**
         * 这个接口必须进行授权
         **/
		[HttpGet]
		[Authorize("Permission")]
		public ActionResult<IEnumerable<string>> value3()
		{
			//这是获取自定义参数的方法
			var auth = HttpContext.AuthenticateAsync().Result.Principal.Claims;
			var userName = auth.FirstOrDefault(t => t.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
			var role = auth.FirstOrDefault(t => t.Type.Equals("Role"))?.Value;

			return new string[] { "这个接口必须进行授权才可以访问", $"userName={userName}", $"Role={role}" };
		}

		//// GET api/values
		//[HttpGet]
		//public ActionResult<IEnumerable<string>> Get()
		//{

		//	return new string[] { "value1", "value2" };
		//}

		[HttpPost]
		public ApiBaseReturn<bool> TestPost([FromBody]TestClass model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && string.IsNullOrEmpty(model.value))
					{
						ErrorInfo.Set("请提供正确的人员分类。", MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						throw new Exception("出个错测试一下！");

						//returnVM.ReturnValue = true;
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

	public class TestClass
	{
		public string value { get; set; }
	}


}
