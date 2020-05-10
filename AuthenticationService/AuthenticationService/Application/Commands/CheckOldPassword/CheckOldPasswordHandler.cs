using AuthenticationService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.CheckOldPassword
{
    public class CheckOldPasswordHandler : IRequestHandler<CheckOldPasswordCommands, bool>
    {
        private readonly ContentoDbContext _context;

        public CheckOldPasswordHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CheckOldPasswordCommands request, CancellationToken cancellationToken)
        {

            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Email == request.Email);
            bool check = BCrypt.Net.BCrypt.Verify(request.OldPassword, account.Password);
            return check == true ? true : false;
        }
    }
}
