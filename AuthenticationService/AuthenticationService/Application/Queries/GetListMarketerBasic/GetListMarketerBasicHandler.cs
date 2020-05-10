using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetListMarketerBasic
{
    public class GetListMarketerBasicHandler : IRequestHandler<GetListMarketerBasicRequest, List<ListUserModel>>
    {
        private readonly ContentoDbContext _context;

        public GetListMarketerBasicHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<List<ListUserModel>> Handle(GetListMarketerBasicRequest request, CancellationToken cancellationToken)
        {
            var list = await _context.Users.AsNoTracking()
               .Include(x => x.Accounts)
               .Where(x => x.Accounts.Any(i => i.IdRole == 1) && x.IsActive == true)
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
