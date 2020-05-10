using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetListEditorForWriter
{
    public class GetListEditorForWriterHandler : IRequestHandler<GetListEditorForWriterRequest, List<ListUserModel>>
    {
        private readonly ContentoDbContext _context;

        public GetListEditorForWriterHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<List<ListUserModel>> Handle(GetListEditorForWriterRequest request, CancellationToken cancellationToken)
        {
            var list = await _context.Users.AsNoTracking()
              .Include(x => x.Accounts)
              .Where(x => x.Accounts.Any(i => i.IdRole == 2) && x.IsActive == true)
              .Select(x => new ListUserModel
              {
                  Id = x.Id,
                  Name = x.FirstName + " " + x.LastName
              })
              .ToListAsync();
            return list;
        }
    }
}
