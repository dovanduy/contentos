using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetcustomerDetail
{
    public class GetCustomerDetailHandler : IRequestHandler<GetCustomerDetailRequest, UserModelDetail>
    {
        private readonly ContentoDbContext _context;

        public GetCustomerDetailHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<UserModelDetail> Handle(GetCustomerDetailRequest request, CancellationToken cancellationToken)
        {
            var acc = await _context.Accounts.AsNoTracking()
                .Include(x => x.IdUserNavigation)
                .FirstOrDefaultAsync(x => x.IdUserNavigation.Id == request.IdCustomer && x.IdRole == 5);
            var reUser = new UserModelDetail
            {
                Id = acc.IdUserNavigation.Id,
                CompanyName = acc.IdUserNavigation.Company,
                FirstName = acc.IdUserNavigation.FirstName,
                LastName = acc.IdUserNavigation.LastName,
                Email = acc.Email,
                Phone = string.IsNullOrEmpty(acc.IdUserNavigation.Phone) == true  ? null : acc.IdUserNavigation.Phone.Trim()
            };
            return reUser;

        }
    }
}
