using JZ.IMS.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.WebApi.Validation {
	public class CallContentValidation : AbstractValidator<CallContentAddOrModifyModel> {
		public CallContentValidation() {
			CascadeMode = CascadeMode.StopOnFirstFailure;
			// =========================== 待解决 =========================== 
			RuleFor(x => x.Version).NotNull().WithMessage("版本不能为空");
			RuleFor(x => x.Enable_Bill_Id).Length(0, 50).WithMessage("ID最大长度不能超过个字符");
			RuleFor(x => x.Disable_Bill_Id).Length(0, 50).WithMessage("菜单显示名称长度不能超过64个字符");
			RuleFor(x => x.Call_Type_Code).NotEmpty().Length(0, 50).WithMessage("菜单显示名称长度不能超过128个字符");
			RuleFor(x => x.Description).NotEmpty().Length(0, 250).WithMessage("菜单显示名称长度不能超过128个字符");
			RuleFor(x => x.Chinese).NotEmpty().Length(0, 250).WithMessage("是否系统默认必须选择");
			RuleFor(x => x.Enabled).NotEmpty().WithMessage("是否显示必须选择");
		}
	}
}
