using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 权限承载实体
    /// </summary>
    public class PolicyRequirement : IAuthorizationRequirement
	{
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; }

		/// <summary>
		/// 没有登陆的action
		/// </summary>
		public string NoLoginAction { get; set; }

		/// <summary>
		/// 构造
		/// </summary>
		public PolicyRequirement()
        {
            //没有权限则跳转到这个路由
            DeniedAction = new PathString("/api/Auth/NoPermission");
		}
    }
}
