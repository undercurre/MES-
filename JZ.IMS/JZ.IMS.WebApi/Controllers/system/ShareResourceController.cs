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
	/// <summary>
	/// 共用资源 
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ShareResourceController : BaseController
	{
		private readonly IStringLocalizer<ShareResourceController> _localizer;

		public ShareResourceController(IStringLocalizer<ShareResourceController> localizer)
		{
			_localizer = localizer;
		}


	}
}