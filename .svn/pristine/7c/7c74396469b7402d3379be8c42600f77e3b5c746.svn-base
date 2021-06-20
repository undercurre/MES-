using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Validation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Configuration;
using System.IO;
using JZ.IMS.Core.Options;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// PAD升级 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class PADUpgradeController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MenuController> _localizer;

        public PADUpgradeController(IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MenuController> localizer)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 获取PAD升级信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ApiBaseReturn<UpgradeInfo> GetUpgradeInfo()
        {
            ApiBaseReturn<UpgradeInfo> returnVM = new ApiBaseReturn<UpgradeInfo>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                        var config = builder.Build();
                        //读取配置
                        returnVM.Result = new UpgradeInfo
                        {
                            Version = config["PADUpgrade:Version"],
                            Title = config["PADUpgrade:Title"],
                            Note = config["PADUpgrade:Note"],
                            Url = config["PADUpgrade:Url"],
                        };
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取MES PAD升级信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ApiBaseReturn<UpgradeInfo> GetMesPADUpgradeInfo()
        {
            ApiBaseReturn<UpgradeInfo> returnVM = new ApiBaseReturn<UpgradeInfo>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                        var config = builder.Build();
                        //读取配置
                        returnVM.Result = new UpgradeInfo
                        {
                            Version = config["MES_PADUpgrade:Version"],
                            Title = config["MES_PADUpgrade:Title"],
                            Note = config["MES_PADUpgrade:Note"],
                            Url = config["MES_PADUpgrade:Url"],
                        };
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取PAD升级信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ApiBaseReturn<UpgradeInfo> GetAndroidUpgradeInfo()
        {
            ApiBaseReturn<UpgradeInfo> returnVM = new ApiBaseReturn<UpgradeInfo>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                        var config = builder.Build();
                        //读取配置
                        returnVM.Result = new UpgradeInfo
                        {
                            Version = config["AndroidUpgrade:Version"],
                            Title = config["AndroidUpgrade:Title"],
                            Note = config["AndroidUpgrade:Note"]
                        };
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 升级信息类
        /// </summary>
        public class UpgradeInfo
        {
            /// <summary>
            /// 服务器APP版本
            /// </summary>
            public string Version { get; set; }

            /// <summary>
            /// 升级标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 升级内容说明
            /// </summary>
            public string Note { get; set; }

            /// <summary>
            /// 升级APP地址
            /// </summary>
            public string Url { get; set; }
        }

    }
}