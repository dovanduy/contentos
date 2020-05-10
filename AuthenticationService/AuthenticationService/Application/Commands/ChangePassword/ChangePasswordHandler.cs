using AuthenticationService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommands,bool>
    {
        private readonly ContentoDbContext _context;

        public ChangePasswordHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(ChangePasswordCommands request, CancellationToken cancellationToken)
        {

            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Email == request.Email);
            bool check = BCrypt.Net.BCrypt.Verify(request.OldPassword, account.Password);
            if (!check)
            {
                return check;
            }
            account.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
