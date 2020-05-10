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
    public class GetCustomerHandler : IRequestHandler<GetCustomerRequest, List<CreateUserModel>>
    {
        private readonly ContentoDbContext _context;

        public GetCustomerHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<CreateUserModel>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            var list = await _context.Users.AsNoTracking()
                .Include(x => x.Accounts)
                .Where(x => x.Accounts.Any(i => i.IdRole == 5))
                .Where(u => u.IdManager == request.MarketerId)
                .Select(x => new CreateUserModel
                {
                    Id = x.Id,
                    FullName = x.FirstName + " " + x.LastName,
                    Email = x.Accounts.First().Email,
                    CompanyName = x.Company,
                    Phone = string.IsNullOrEmpty(x.Phone) == true ? null : x.Phone.Trim()

                }).ToListAsync();
            return list;
        }
    }
}
