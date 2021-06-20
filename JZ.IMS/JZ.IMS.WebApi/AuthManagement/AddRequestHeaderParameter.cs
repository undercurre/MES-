using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 控制swagger中是否需要添加请求头: Authorization验证，多语言验证
	/// </summary>
	public class AddRequestHeaderParameter : IOperationFilter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="context"></param>
		public void Apply(Operation operation, OperationFilterContext context)
		{
			if (operation.Parameters == null) operation.Parameters = new List<IParameter>();
			var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;

			//先判断是否是匿名访问,
			var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
			if (descriptor != null)
			{
				var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
				bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
				//非匿名的方法,链接中添加accesstoken值
				if (!isAnonymous)
				{
					operation.Parameters.Add(new NonBodyParameter()
					{
						Name = "Authorization",
						In = "header",  //query header body path formData
						Type = "string",
						Required = true, //是否必选
						Default = "Bearer ",
					});
				}

				List<object> languageEnum = new List<object> { "zh-CN,zh;", "en-US,en;" };
				//本地化
				operation.Parameters.Add(new NonBodyParameter()
				{
					Name = "Accept-Language",
					In = "header",  //query header body path formData
					Type = "string",
					Required = false, //是否必选
					Default = "zh-CN,zh;",
					Description = "语言",
					Enum = languageEnum, //下拉列表
				}); 
			}
		}
	}
}
