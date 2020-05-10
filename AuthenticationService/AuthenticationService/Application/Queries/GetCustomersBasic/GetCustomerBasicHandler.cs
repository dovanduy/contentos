using AuthenticationService.Entities;
using MediatR;
using System;
using AuthenticationService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthenticationService.Application.Queries.GetWriter;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Application.Queries.GetCustomer
{
    public class GetCustomerBasicHandler : IRequestHandler<GetCustomerBasicRequest, List<ListUserModel>>
    {
        private readonly ContentoDbContext _context;

        public GetCustomerBasicHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListUserModel>> Handle(GetCustomerBasicRequest request, CancellationToken cancellationToken)
        {
            var list = await _context.Users.AsNoTracking()
                .Include(x => x.Accounts)
                .Where(x => x.Accounts.Any(i => i.IdRole == 5) && x.IsActive == true)
                .Where(u => u.IdManager == request.MarketerId)
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
