
using JZ.IMS.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Controllers;

namespace JZ.IMS.WebApi.Validation
{
    public class MenuValidation : AbstractValidator<MenuAddOrModifyModel>
    {
		private readonly IStringLocalizer<MenuController> _localizer;

		public MenuValidation(IStringLocalizer<MenuController> localizer)
        {
			_localizer = localizer;

			CascadeMode = CascadeMode.StopOnFirstFailure;

			//上级菜单不能为空。
			RuleFor(x => x.Parent_Id).NotNull().WithMessage(_localizer["parent_id_error"]);
			
			//"菜单别名的长度范围为5-200个字符"
			RuleFor(x => x.Menu_Code).NotEmpty().Length(5, 200).WithMessage(_localizer["menu_code_error"]);
			
			//菜单显示名称长度不能超过64个字符。
			RuleFor(x => x.Menu_Name).Length(0, 64).WithMessage(_localizer["menu_name_error"]);

			//图标地址长度不能超过128个字符。
			RuleFor(x => x.Icon_Url).Length(0, 500).WithMessage(_localizer["icon_url_error"]);

			//链接地址长度不能超过128个字符。
			RuleFor(x => x.Link_Url).Length(0, 500).WithMessage(_localizer["link_url_error"]);

			//是否系统默认必须选择。
			RuleFor(x => x.Is_System).NotNull().WithMessage(_localizer["is_system_error"]) ;

			//是否显示必须选择。
			RuleFor(x => x.ENABLED).NotNull().WithMessage(_localizer["enabled_error"]);
        }
    }
}
