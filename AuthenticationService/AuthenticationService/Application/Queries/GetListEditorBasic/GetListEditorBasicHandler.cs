using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetListEditorBasic
{
    public class GetListEditorBasicHandler : IRequestHandler<GetListEditorBasicRequest, List<ListUserModel>>
    {
        private readonly ContentoDbContext _context;

        public GetListEditorBasicHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<List<ListUserModel>> Handle(GetListEditorBasicRequest request, CancellationToken cancellationToken)
        {
            var list = await _context.Users.AsNoTracking()
              .Include(x => x.Accounts)
              .Where(x => x.Accounts.Any(i => i.IdRole == 2) && x.IdManager == null && x.IsActive == true)
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
