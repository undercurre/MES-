using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace JZ.IMS.WebApi.Controllers.system
{
    [Route("/role/api/[action]")]
    [ApiController]
    public class SingleAuthController : BaseController
    {
        private readonly IStringLocalizer<AuthController> _localizer;
        private readonly IRolePermissionRepository _rolePermission;
        private readonly IManagerService _service;

        public SingleAuthController(IStringLocalizer<AuthController> localizer, IRolePermissionRepository rolePermission, IManagerService service)
        {
            _localizer = localizer;
            _rolePermission = rolePermission;
            _service = service;
        }

        public class ResultVM
        {
            public dynamic msg { get; set; }
            public bool Result { get; set; }
            public string recoredCount { get; set; }
        }

        /// <summary>
        /// 角色信息查询
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<dynamic> GetRole()
        {
            ResultVM resultVM = new ResultVM();
            resultVM.Result = false;
            List<dynamic> data = new List<dynamic>();
            try
            {
                data = await _rolePermission.GetRole();
                resultVM.msg = data;
                resultVM.Result = true;
                resultVM.recoredCount = data.Count.ToString();
            }
            catch (Exception ex)
            {
                resultVM.msg = ex.Message;
            }
            return resultVM;
        }

        /// <summary>
        /// 角色用户关联信息查询
        /// </summary>
        /// <param name="Account">账号</param>
        /// <param name="Name">用户名</param>
        /// <param name="RoleName">角色名</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<dynamic> GetUserRoleInfo([FromQuery] string Account = "", string Name = "", string RoleName = "", string PageIndex = "1", string PageSize = "30")
        {
            ResultVM resultVM = new ResultVM();
            resultVM.Result = false;
            try
            {
                var dataTable = await _rolePermission.GetUserRoleInfo(Account, Name, RoleName, PageIndex, PageSize);
                resultVM.msg = dataTable == null || dataTable.code == -1 ? "" : dataTable.data;
                resultVM.Result = true;
                resultVM.recoredCount = dataTable == null || dataTable.code == -1 ? "0" : dataTable.count.ToString();
            }
            catch (Exception ex)
            {
                resultVM.msg = ex.Message;
            }
            return resultVM;
        }

        /// <summary>
        /// 用户查询
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<dynamic> GetUser([FromQuery] string Account = "", string Name = "", string OrganizeID = "", string OrganizeName = "", string PageIndex = "1", string PageSize = "30")
        {
            ResultVM resultVM = new ResultVM();
            resultVM.Result = false;
            List<dynamic> data = new List<dynamic>();
            try
            {
                var dataTable = await _rolePermission.GetUser(Account, Name, OrganizeID, OrganizeName, PageIndex, PageSize);
                resultVM.msg = dataTable == null || dataTable.code == -1 ? "" : dataTable.data;
                resultVM.Result = true;
                resultVM.recoredCount = dataTable == null || dataTable.code == -1 ? "0" : dataTable.count.ToString();
            }
            catch (Exception ex)
            {
                resultVM.msg = ex.Message;
            }
            return resultVM;
        }

        /// <summary>
        /// 用户角色配置
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<dynamic> SetUserRole([FromBody] dynamic model)
        {
            ResultVM resultVM = new ResultVM();
            resultVM.Result = false;
            string UserID = model == null ? "" : model.UserID;
            string RoleID = model == null ? "" : model.RoleID;
            try
            {
                #region 检验参数
                if (!UserID.IsNullOrEmpty() && !RoleID.IsNullOrEmpty())
                {
                    if (await _rolePermission.SetUserRole(UserID, RoleID))
                    {
                        resultVM.Result = true;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                resultVM.msg = ex.Message;
            }
            return resultVM;
        }

        /// <summary>
        /// 登录地址请求
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiBaseReturn<UserVM>> Login([FromQuery] string UserID)
        {
            ApiBaseReturn<UserVM> returnVM = new ApiBaseReturn<UserVM>();
            UserVM userVM = new UserVM();
            string token_json = string.Empty;
            try
            {
                string SafetyKey = "ut2020";
                var account = AESEncryptHelper.Decode(UserID, SafetyKey);

                #region 登陆校验并获取token
                if (!ErrorInfo.Status)
                {
                    var managerModle = (await _rolePermission.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " And ID=:UserID", new { UserID = account })).FirstOrDefault();
                    LoginModel model = new LoginModel() { IsCheckPassword = false, UserName = managerModle.USER_NAME, Password = managerModle.PASSWORD };
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
}
