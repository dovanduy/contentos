using ContentProccessService.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateTag
{
    public class CreateTagValidator : AbstractValidator<TagsDto>
    {
        public CreateTagValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();
        }
    }
}
