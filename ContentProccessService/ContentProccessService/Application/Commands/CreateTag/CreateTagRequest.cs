using ContentProccessService.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateTag
{
    public class CreateTagRequest : IRequest<Unit>
    {
        public TagsDto dto { get; set; }
    }
}
