using ContentProccessService.Application.Dtos;
using ContentProccessService.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Commands.CreateTag
{
    public class CreateTagHandler : IRequestHandler<CreateTagRequest, Unit>
    {
        private readonly ContentoDbContext contentodbContext;
        public CreateTagHandler(ContentoDbContext contentodbContext)
        {
            this.contentodbContext = contentodbContext;
        }
        public async Task<Unit> Handle(CreateTagRequest request, CancellationToken cancellationToken)
        {
            var tag = new Tags {Name = request.dto.Name, IsActive = request.dto.IsActive};
            contentodbContext.Tags.Add(tag);
            await contentodbContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
