using AuthenticationService.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using AuthenticationService.Common;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Application.Commands.CreateCustomer
{
    public class CreateCustomerAccountHandler : IRequestHandler<CreateCustomerAccountCommads, CreateUserModel>
    {
        private readonly ContentoDbContext _context;
        private readonly IHelperFunction _helper;


        public CreateCustomerAccountHandler(ContentoDbContext context, IHelperFunction helper)
        {
            _context = context;
            _helper = helper;
        }
        public async Task<CreateUserModel> Handle(CreateCustomerAccountCommads request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var returnResult = new CreateUserModel();
                string newPassword = "";

                if (!IsEmailUnique(request.Email))
                {
                    newPassword = _helper.GenerateRandomPassword();
                    var newAccount = new Accounts
                    {
                        Email = request.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(newPassword),
                        IdRole = 5,
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow

                    };
                    var lstAcc = new List<Accounts>();
                    lstAcc.Add(newAccount);
                    var newUser = new Users
                    {
                        IsActive = true,
                        LastName = request.LastName,
                        FirstName = request.FirstName,
                        IdManager = request.IdMarketer,
                        Company = request.CompanyName,
                        Phone = request.Phone,
                        Accounts = lstAcc
                    };
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new CreateUserModel
                    {
                        Id = _context.Users.AsNoTracking().OrderByDescending(x => x.Id).First().Id,
                        FullName = request.FirstName + " " + request.LastName,
                        Email = request.Email,
                        CompanyName = request.CompanyName,
                        Phone = string.IsNullOrEmpty(request.Phone) ? null : request.Phone.Trim(),
                        Password = newPassword,
                        IdError = 1
                    };
                }
                returnResult.IdError = 0;
                return returnResult;

            }
            catch (Exception e)
            {
                transaction.Rollback();
                return null;
            }

        }
        public bool IsEmailUnique(string Email)
        {
            return _context.Accounts.Where(x => x.Email == Email).Any();

        }
    }
}
