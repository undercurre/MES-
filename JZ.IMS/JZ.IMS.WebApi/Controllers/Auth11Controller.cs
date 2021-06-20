using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using Microsoft.Extensions.Localization;

namespace ftw.jwt.webapi.Controllers
{
	/// <summary>
	/// 登陆并获得Token 
	/// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Auth11Controller : BaseController
	{
		private readonly IStringLocalizer<Auth11Controller> _localizer;

        public Auth11Controller(IStringLocalizer<Auth11Controller> localizer)
        {
			_localizer = localizer;
		}

		// GET: api/Test
		[HttpGet]
		public string Get()
		{
			string hello = _localizer["Hello"];
			string goodBye = _localizer["GoodBye"];

			return $"hello: {hello},goodBye: {goodBye}";
		}

		[HttpPost]
        public ApiBaseReturn<IActionResult> GetToken(UserInfo model)
        {
			ApiBaseReturn<IActionResult> returnVM = new ApiBaseReturn<IActionResult>();

			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && string.IsNullOrEmpty(model.userName))
					{
						ErrorInfo.Set(_localizer["User_Name_Required"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					if (!ErrorInfo.Status && string.IsNullOrEmpty(model.passWord))
					{
						ErrorInfo.Set(_localizer["User_Password_Required"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						Dictionary<string, string> clims = new Dictionary<string, string>();
						clims.Add("userName", model.userName);
						//returnVM.Result = new JsonResult(this._jwt.GetToken(clims));
						returnVM.Result = new JsonResult(clims);
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

    public class UserInfo
    {
        public string userName { get; set; }
        public string passWord { get; set; }
    }
}