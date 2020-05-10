using AuthenticationService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.CheckEmail
{
    public class CheckEmailHandler : IRequestHandler<CheckEmailRequest, bool>
    {
        private readonly ContentoDbContext _context;

        public CheckEmailHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async  Task<bool> Handle(CheckEmailRequest request, CancellationToken cancellationToken)
        {
            var check = await _context.Accounts.AnyAsync(x => x.Email == request.Email && x.IsActive == true);
            return !check ;
        }
    }
}
