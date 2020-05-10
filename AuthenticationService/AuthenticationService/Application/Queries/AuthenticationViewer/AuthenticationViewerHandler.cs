using AuthenticationService.Common;
using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.AuthenticationViewer
{
    public class AuthenticationViewerHandler : IRequestHandler<AuthenticationViewerRequest, LoginSuccessViewModel>
    {
        private readonly IHelperFunction _helper;
        private readonly ContentoDbContext _context;

        public AuthenticationViewerHandler(IHelperFunction helper, ContentoDbContext context)
        {

            _helper = helper;
            _context = context;
        }

        public async Task<LoginSuccessViewModel> Handle(AuthenticationViewerRequest request, CancellationToken cancellationToken)
        {
            var accounts = await _context.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == request.Email && x.IdRole == 4);

            bool checkPassword = false;

            if (accounts != null)
            {
                LoginSuccessViewModel resultReturn = new LoginSuccessViewModel();
                if (accounts.IsActive == false)
                {
                    resultReturn.IdError = 1;
                    return resultReturn;
                }
                    checkPassword = BCrypt.Net.BCrypt.Verify(request.Password, accounts.Password);
                    //var appUser = _userManager.Users.SingleOrDefault(r => r.Email == request.Email);
                    //var user = _context.Accounts.FirstOrDefault(u => u.Email == request.Email);
                    if (checkPassword)
                    {
                        string role = _context.Roles.AsNoTracking().FirstOrDefault(r => r.Id == accounts.IdRole).Role;
                        string fullname = _context.Users.Find(accounts.IdUser).FirstName + " " + _context.Users.Find(accounts.IdUser).LastName;


                    resultReturn.Id = accounts.IdUser;
                    resultReturn.FullName = fullname;
                    resultReturn.Role = role;
                    resultReturn.Token = _helper.GenerateJwtToken(request.Email, accounts, role);
                        return resultReturn;
                    }

            }
            return null;
        }
    }
}
