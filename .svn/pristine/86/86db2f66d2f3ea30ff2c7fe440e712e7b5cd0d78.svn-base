using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        private readonly IRolePermissionRepository _rolePermission;
        public PolicyHandler(IRolePermissionRepository rolePermission)
        {
            _rolePermission = rolePermission;
        }

        /// <summary>
        /// 策略授权
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            //从AuthorizationHandlerContext转成HttpContext，以便取出请求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;

            //请求Url
            var questUrl = httpContext.Request.Path.Value.ToUpper().TrimEnd('/');

            //是否经过验证
            var isAuthenticated = httpContext.User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                //检测是否包含'Authorization'请求头
                if (!httpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    //无权限跳转到拒绝页面
                    httpContext.Response.Redirect(requirement.DeniedAction);
                    return Task.CompletedTask;
                }

                var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                try
                {
                    if (tokenHeader.Length >= 128)
                    {
                        var jwtHandler = new JwtSecurityTokenHandler();
                        JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(tokenHeader);
                        object role = new object();
                        object userName = new object();
                        try
                        {
                            jwtToken.Payload.TryGetValue("Role", out role);
                            jwtToken.Payload.TryGetValue(ClaimTypes.NameIdentifier, out userName);

                            if (userName.ToString().ToUpper() == "ADMIN")
                            {
                                context.Succeed(requirement);
                                return Task.CompletedTask;
                            }

                            List<Sys_Menu> menuList = CacheHelper.Get<List<Sys_Menu>>(Convert.ToString(role));
                            if (menuList == null)
                            {
                                CacheHelper.Set(Convert.ToString(role), _rolePermission.GetMenuByRoleId(Convert.ToDecimal(role)));
                                menuList = CacheHelper.Get<List<Sys_Menu>>(Convert.ToString(role));
                                if (menuList == null)
                                {
                                    httpContext.Response.Redirect(requirement.DeniedAction);
                                    return Task.CompletedTask;
                                }
                            }
                            string curUrl = questUrl.Replace("/API", "");
                            var currentModule = menuList.FirstOrDefault(u => !string.IsNullOrWhiteSpace(u.Link_Url) &&
                                u.Link_Url.ToUpper().Equals(curUrl));
                            //当前登录用户没有Action记录
                            if (currentModule != null)
                            {
                                context.Succeed(requirement);
                            }
                            else
                            {
                                //无权限跳转到拒绝页面
                                httpContext.Response.Redirect(requirement.DeniedAction);
                                return Task.CompletedTask;
                            }
                        }
                        catch (Exception ex)
                        {
                            var cd = ex.Message;
                            httpContext.Response.Redirect(requirement.DeniedAction);
                        }
                    }
                    else
                    {
                        httpContext.Response.Redirect(requirement.DeniedAction);
                        return Task.CompletedTask;
                    }
                }
                catch (Exception)
                {
                    httpContext.Response.Redirect(requirement.DeniedAction);
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }
}
