using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using JZ.IMS.Core.Options;
using JZ.IMS.IRepository;
using JZ.IMS.Repository.Oracle;
using JZ.IMS.Services;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace JZ.IMS.WebApi
{
	public class Startup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configuration"></param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// 
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.Configure<DbOption>("iWMS", Configuration.GetSection("DbOpion"));
			//services.Configure<DbOption>("LoginDbOpion", Configuration.GetSection("LoginDbOpion"));

			if (Configuration.GetSection("UseU9").Value == "true")
			{
				services.Configure<DbOption>("erpU9", Configuration.GetSection("U9DbOpion"));
			}

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.AddDistributedMemoryCache();//启用session之前必须先添加内存
			services.AddSession(options =>
			{
				options.Cookie.Name = ".AdventureWorks.Session";
				options.IdleTimeout = TimeSpan.FromSeconds(120);//设置session的过期时间
				options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该cookie的值
			});

			//添加本地化
			services.AddLocalization(o =>
			{
				o.ResourcesPath = "Resources";
			});

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//添加策略鉴权模式
			services.AddAuthorization(options =>
			{
				options.AddPolicy("Permission", policy => policy.Requirements.Add(new PolicyRequirement()));
			})
			.AddAuthentication(s =>
			{
				//添加JWT Scheme
				s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			//添加jwt验证：
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateLifetime = true,//是否验证失效时间
											//注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
					ClockSkew = TimeSpan.FromMinutes(10),

					ValidateAudience = true,//是否验证Audience
					ValidAudience = Const.Domain,

					ValidateIssuer = true, //是否验证Issuer
					ValidIssuer = Const.Domain, //Issuer，这两项和前面签发jwt的设置一致

					ValidateIssuerSigningKey = true,//是否验证SecurityKey
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey))//拿到SecurityKey
				};
			});

			//注入授权Handler
			services.AddSingleton<IAuthorizationHandler, PolicyHandler>();

			services.AddMvc()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.ContractResolver = new DefaultContractResolver();
					options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
				})
				.AddControllersAsServices()
				.AddFluentValidation(fv => {
					//程序集方式引入
					//fv.RegisterValidatorsFromAssemblyContaining<ManagerRoleValidation>();
					//去掉其他的验证，只使用FluentValidation的验证规则
					fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
				});
			//.AddApplicationPart(typeof(ControllerFromClassLib).Assembly)
			
			//注册Swagger
			if (Configuration.GetSection("UseSwagger").Value == "true")
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "IMS Web Api",
                        Description = "接口文档"
                    });
                    options.CustomSchemaIds(type => type.FullName); // 解决相同类名会报错的问题

                    //swagger中控制请求的时候发是否需要在url中增加RequestHeader
                    options.OperationFilter<AddRequestHeaderParameter>();
                    // 为 Swagger JSON and UI设置xml文档注释路径
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    //... and tell Swagger to use those XML comments.
                    options.IncludeXmlComments(xmlPath, true); //默认的第二个参数是false，这个是controller的注释

                    var xmlPath_Models = Path.Combine(AppContext.BaseDirectory, "JZ.IMS.Models.xml");
                    options.IncludeXmlComments(xmlPath_Models, true);

                    var xmlPath_ViewModels = Path.Combine(AppContext.BaseDirectory, "JZ.IMS.ViewModels.xml");
                    options.IncludeXmlComments(xmlPath_ViewModels, true);

                });
            }
			//DI了AutoMapper中需要用到的服务，其中包括AutoMapper的配置类 Profile
			services.AddAutoMapper();
			var builder = new ContainerBuilder();
			builder.Populate(services);
			builder.RegisterAssemblyTypes(typeof(ManagerRoleRepository).Assembly)
				   .Where(t => t.Name.EndsWith("Repository"))
				   .AsImplementedInterfaces();
			builder.RegisterAssemblyTypes(typeof(ManagerRoleService).Assembly)
				 .Where(t => t.Name.EndsWith("Service"))
				 .AsImplementedInterfaces();

			return new AutofacServiceProvider(builder.Build());
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
            if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//可以访问根目录下面的静态文件
			//var staticfile = new StaticFileOptions
			//{
			//	FileProvider = new PhysicalFileProvider(AppContext.BaseDirectory+ @"\upload\")
			//};
			//app.UseStaticFiles(staticfile);

			string path = AppContext.BaseDirectory;
			if (!Directory.Exists(path + $@"\upload"))
			{
				Directory.CreateDirectory(path + $@"\upload");
			}

			app.UseFileServer(new FileServerOptions()
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "upload")), //实际目录地址
				RequestPath = new Microsoft.AspNetCore.Http.PathString("/upload"),  //用户访问地址
				EnableDirectoryBrowsing = true
			});

			IList<CultureInfo> supportedCultures = new List<CultureInfo>
			{
				new CultureInfo("zh-CN"),
				new CultureInfo("en-US"),
			};

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("zh-CN"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			//允许跨域
			app.UseCors(builder => builder.AllowAnyOrigin()
										  .AllowAnyMethod()
										  .AllowAnyHeader());

			//添加jwt验证
			app.UseAuthentication();

			app.UseHttpsRedirection();
			app.UseCookiePolicy();
			app.UseSession();

            //Swagger
            if (Configuration.GetSection("UseSwagger").Value == "true")
            {
                app.UseSwagger();
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "IMS Web Api V1");
                    s.RoutePrefix = string.Empty;
                    s.DocExpansion(DocExpansion.None);  //DocExpansion.List 
                    s.OAuthClientId("JZ.IMS.WebApi");  //客户端名称
                    s.OAuthAppName("客户端为JZ.IMS.WebApi"); // 描述
                });
            }

			app.UseMvcWithDefaultRoute();

			//app.UseMvc(routes =>
			//{
			//	routes.MapRoute(
			//		name: "default",
			//		template: "api/{controller=Auth}/{action=Get}");
			//});
		}
	}
}
