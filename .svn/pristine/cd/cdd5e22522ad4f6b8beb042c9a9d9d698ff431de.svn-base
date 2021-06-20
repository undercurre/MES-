using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthController : BaseController
	{
		private readonly IStringLocalizer<AuthController> _localizer;
		private readonly IRolePermissionRepository _rolePermission;
		private readonly IManagerService _service;

		public AuthController(IStringLocalizer<AuthController> localizer, IRolePermissionRepository rolePermission, IManagerService service)
		{
			_localizer = localizer;
			_rolePermission = rolePermission;
			_service = service;
		}

		[AllowAnonymous]
		[HttpGet]
		//[Route("api/nopermission")]
		public IActionResult NoPermission()
		{
			return Forbid(_localizer["NoPermission"]);
		}

		/// <summary>
		/// 注: 先不用
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		private ApiBaseReturn<string> NoLogin()
		{
			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			string noLoginMsg = _localizer["NoLoginMsg"];

			if (!ErrorInfo.Status)
			{
				try
				{
					returnVM.Result = "NoLogin_Err_110";
					ErrorInfo.Set(noLoginMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        /// 获取Token
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<UserVM>> GetToken([FromBody]LoginModel model)
		{
			ApiBaseReturn<UserVM> returnVM = new ApiBaseReturn<UserVM>();
			UserVM userVM = new UserVM();
			string token_json = string.Empty;

			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && model!= null && (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password)))
					{
						ErrorInfo.Set(_localizer["login_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 登陆校验并获取token

					if (!ErrorInfo.Status)
					{
						LoginState login = await CheckAccount(model);
						if (login != null && login.state)
						{
							//这里可以加入自定义的参数
							var claims = new[]
							{
								new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
								new Claim (JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
								new Claim(ClaimTypes.NameIdentifier, model.UserName),
								new Claim("Role", login.role)
							};

							//sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
							var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
							var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
							//.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
							var token = new JwtSecurityToken(
									//颁发者
									issuer: Const.Domain,
									//接收者
									audience: Const.Domain,
									//过期时间
									expires: DateTime.Now.AddMinutes(Const.ExpireTime),
									//签名证书
									signingCredentials: creds,
									//自定义参数
									claims: claims
								);
							token_json = new JwtSecurityTokenHandler().WriteToken(token);

							userVM.Userinfo = login.userinfo;
							userVM.Token = token_json;
						}
						else
						{
							ErrorInfo.Set(_localizer["login_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						returnVM.Result = userVM;
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
        /// Android获取Token
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiBaseReturn<UserVM>> GetTokenToAndroid([FromQuery] LoginModel model)
        {
            ApiBaseReturn<UserVM> returnVM = new ApiBaseReturn<UserVM>();
            UserVM userVM = new UserVM();
            string token_json = string.Empty;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model != null && (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password)))
                    {
                        ErrorInfo.Set(_localizer["login_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 登陆校验并获取token

                    if (!ErrorInfo.Status)
                    {
                        LoginState login = await CheckAccount(model);
                        if (login != null && login.state)
                        {
                            //这里可以加入自定义的参数
                            var claims = new[]
                            {
                                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                                new Claim (JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
                                new Claim(ClaimTypes.NameIdentifier, model.UserName),
                                new Claim("Role", login.role)
                            };

                            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
                            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
                            var token = new JwtSecurityToken(
                                    //颁发者
                                    issuer: Const.Domain,
                                    //接收者
                                    audience: Const.Domain,
                                    //过期时间
                                    expires: DateTime.Now.AddMinutes(Const.ExpireTime),
                                    //签名证书
                                    signingCredentials: creds,
                                    //自定义参数
                                    claims: claims
                                );
                            token_json = new JwtSecurityTokenHandler().WriteToken(token);

                            userVM.Userinfo = login.userinfo;
                            userVM.Token = token_json;
                        }
                        else
                        {
                            ErrorInfo.Set(_localizer["login_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = userVM;
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

        [HttpPost]
        public async Task<ApiBaseReturn<UserVM>> GetUserInfoByToken()
        {
            ApiBaseReturn<UserVM> returnVM = new ApiBaseReturn<UserVM>();
            UserVM userVM = new UserVM();
            if (!ErrorInfo.Status)
            {
                try
                {
                    var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    var jwtHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(tokenHeader);
                    object userName = new object();
                    jwtToken.Payload.TryGetValue(ClaimTypes.NameIdentifier, out userName);
                    var userInfo = await _service.GetUserInfoByName((string)userName);
                    userVM.Userinfo = userInfo;

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = userVM;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }
            return returnVM;
        }


        /// <summary>
        /// 登陆校验
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <returns></returns>
        private async Task<LoginState> CheckAccount(LoginModel model)
        {
            var result = new LoginState();
            model.Ip = HttpContext.GetClientUserIp();
            var manager = await _service.SignInAsync(model);
            if (manager != null && manager.ENABLED == "Y")
            {
                CacheHelper.Set(manager.ROLE_ID.ToString(), _rolePermission.GetMenuByRoleId(manager.ROLE_ID));

                result.state = true;
                result.role = manager.ROLE_ID.ToString();
                result.userinfo = manager;
            }
            return result;
        }

    }

    public class UserVM
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// 用户信息
        /// </summary>
        public Sys_Manager Userinfo { get; set; }
    }

    internal class LoginState
    {
        public bool state { get; set; } = false;

        public string role { get; set; } = string.Empty;

        public Sys_Manager userinfo { get; set; }
    }
}