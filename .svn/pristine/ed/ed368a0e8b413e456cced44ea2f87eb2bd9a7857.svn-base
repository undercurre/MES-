
using JZ.IMS.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Validation
{
    public class ManagerRoleValidation : AbstractValidator<ManagerRoleAddOrModifyModel>
    {
		private readonly IStringLocalizer<ManagerRoleController> _localizer;

		public ManagerRoleValidation(IStringLocalizer<ManagerRoleController> localizer)
        {
			_localizer = localizer;

			CascadeMode = CascadeMode.StopOnFirstFailure;

			//角色名称不能为空并且长度不能超过64个字符。
			RuleFor(x => x.Role_Name).NotEmpty().Length(1, 64).WithMessage("角色名称不能为空并且长度不能超过64个字符");

			//角色类型格式不正确。
			RuleFor(x => x.Role_Type).NotNull().InclusiveBetween(1,2).WithMessage("角色类型格式不正确") ;

			//必须选择是否系统默认。
			RuleFor(x => x.Is_System).NotNull().WithMessage("必须选择是否系统默认。") ;

			//备注信息的长度必须符合规则: 小于128个字符。
			RuleFor(x => x.Remark).Length(0, 128).WithMessage("备注信息的长度必须符合规则: 小于128个字符。");
        }
    }
}
