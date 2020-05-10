using ContentProccessService.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTasksByEditorId
{
    public class GetTasksByEditorIdValidator :AbstractValidator<GetTasksByEditorIdRequest>
    {
          public GetTasksByEditorIdValidator()
        {
            RuleFor(x => x.IdEditor).NotEmpty().WithMessage("IdEditor is Required");
        }
    }
}
