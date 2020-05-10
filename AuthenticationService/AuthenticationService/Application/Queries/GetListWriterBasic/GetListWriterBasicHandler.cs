using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetListWriterBasic
{
    public class GetListWriterBasicHandler : IRequestHandler<GetListWriterBasicRequest, List<ListUserModel>>
    {
        private readonly ContentoDbContext _context;

        public GetListWriterBasicHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<List<ListUserModel>> Handle(GetListWriterBasicRequest request, CancellationToken cancellationToken)
        {
            var list = await _context.Users.AsNoTracking()
               .Include(x => x.Accounts)
               .Where(x => x.Accounts.Any(i => i.IdRole == 3) && x.IdManager == null && x.IsActive == true)
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
