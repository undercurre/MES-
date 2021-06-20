
using JZ.IMS.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Extensions;
using Microsoft.Extensions.Localization;
using JZ.IMS.WebApi.Controllers;

namespace JZ.IMS.WebApi.Validation
{
    public class ChangeStatusModelValidation : AbstractValidator<ChangeStatusModel>
    {
		private readonly IStringLocalizer<SfcsEquipContentConfController> _localizer;

		public ChangeStatusModelValidation(IStringLocalizer<SfcsEquipContentConfController> localizer)
        {
			_localizer = localizer;

			CascadeMode = CascadeMode.StopOnFirstFailure;

			//主键不能为空
			RuleFor(x => x.Id).NotNull().GreaterThan(0).WithMessage(_localizer["id_notnull"]);

			//状态不能为空
			RuleFor(x => x.Status).NotNull().WithMessage(_localizer["status_notnull"]) ;
        }
    }
}
