using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using JZ.IMS.Core.Options;
using JZ.IMS.IRepository;
using JZ.IMS.Repository.Oracle;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JZ.IMS.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : ControllerBase
	{
		private static IManagerLogRepository _logRepository = null;

		/// <summary>
		/// 错误信息
		/// </summary>
		protected ErrorInfoClass ErrorInfo { get; set; } = new ErrorInfoClass();

		#region 建立错误日志

		/// <summary>
		/// 写错误日志
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="returnVM"></param>
		internal void WriteLog<T>(ref ApiBaseReturn<T> returnVM)
		{
			if (ErrorInfo.Status)
			{
				returnVM.ErrorInfo.Set(ErrorInfo);
				if (ErrorInfo.ErrorType == EnumErrorType.Error)
				{
					CreateErrorLog(ErrorInfo);
				}
				ErrorInfo.Clear();
			}
		}

		/// <summary>
		/// 创建错误日志
		/// </summary>
		internal void CreateErrorLog()
		{
			if (ErrorInfo.Status) CreateErrorLog(ErrorInfo.Clone());
		}

		/// <summary>
		/// 创建错误日志
		/// </summary>
		/// <param name="errorInfo">错误信息</param>
		internal void CreateErrorLog(ErrorInfoClass errorInfo)
		{
			try
			{
				if (_logRepository == null)
				{
					var builder = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json");

					var config = builder.Build();

					//读取配置
					DbOption _dbOption = new DbOption();
					_dbOption.DbType = config["DbOpion:DbType"];
					_dbOption.ConnectionString = config["DbOpion:ConnectionString"];

					_logRepository = new ManagerLogRepository(_dbOption);
				}
				var result =  _logRepository.SaveErrorLog(errorInfo);
			}
			catch (Exception)
			{
			}
		}

		#endregion

		#region 本地化

		/// <summary>
		/// 
		/// </summary>
		/// <param name="_httpContextAccessor">http上下文访问器</param>
		/// <param name="resultCode">结果编号</param>
		/// <param name="resultMsg">结果信息</param>
		/// <returns></returns>
		internal static string GetLocalMessage(IHttpContextAccessor _httpContextAccessor, int resultCode, string resultMsg)
		{
			string result = resultMsg;
			string acceptLanguage = "zh-CN,zh;";
			if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Accept-Language"))
			{
				acceptLanguage = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString();
				if (!acceptLanguage.Contains("zh-CN"))  //zh-CN,zh;
				{
					result = MessageOfLanguage.GetMessageOfLanguage(acceptLanguage, resultCode);
				}
			}
			return result;
		}

		/// <summary>
		/// 当前请求的语言
		/// </summary>
		/// <param name="_httpContextAccessor"></param>
		/// <returns></returns>
		internal static string GetCurMessage(IHttpContextAccessor _httpContextAccessor)
		{
			string result = "zh-CN,zh;";
			if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Accept-Language"))
			{
				result = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString();
			}
			return result;
		}

		#endregion

		#region JWT
		internal void GetUserName(object userName)
		{
			var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			var jwtHandler = new JwtSecurityTokenHandler();
			JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(tokenHeader);
			jwtToken.Payload.TryGetValue(ClaimTypes.NameIdentifier, out userName);
		}
        #endregion
    }
}