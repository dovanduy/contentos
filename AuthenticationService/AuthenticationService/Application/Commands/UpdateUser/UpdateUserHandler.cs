using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommands, UserAdminModels>
    {
        private readonly ContentoDbContext _context;


        public UpdateUserHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<UserAdminModels> Handle(UpdateUserCommands request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var upUser = await _context.Users.Include(x => x.Accounts).Include(x => x.IdManagerNavigation).FirstOrDefaultAsync(x => x.Id == request.Id);
                upUser.LastName = request.FirstName;
                upUser.FirstName = request.LastName;
                upUser.Gender = request.Gender;
                upUser.Age = request.Age;
                upUser.Phone = request.Phone;
                upUser.Company = request.Company;
                upUser.Accounts.FirstOrDefault().ModifiedDate = DateTime.UtcNow;
                //= request.Role;
                if (upUser.Accounts.FirstOrDefault().IdRole == 1)
                {
                    _context.Users
                   .Include(x => x.Accounts)
                   .Where(x => x.Accounts.Any(i => i.IdRole == 2) && x.IsActive == true && x.IdManager == request.Id)
                   .ToList().ForEach(x=>x.IdManager = null);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else if (upUser.Accounts.FirstOrDefault().IdRole == 2)
                {
                    upUser.IdManager = null;
                    _context.Users
                   .Include(x => x.Accounts)
                   .Where(x => x.Accounts.Any(i => i.IdRole == 3) && x.IsActive == true && x.IdManager == request.Id)
                   .ToList().ForEach(x => x.IdManager = null);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else if (upUser.Accounts.FirstOrDefault().IdRole == 3)
                {
                    upUser.IdManager = null;
                    await _context.SaveChangesAsync(cancellationToken);
                }
                if (request.IdMarketer != null)
                {
                    if (request.IdMarketer.Count == 1 && request.Role == 2)
                    {
                        upUser.IdManager = request.IdMarketer.FirstOrDefault();
                    }
                }
                if (request.IdEditor != null)
                {
                    if (request.IdEditor.Count == 1 && request.Role == 3)
                    {
                        upUser.IdManager = request.IdEditor.FirstOrDefault();
                    }
                }
                upUser.Accounts.FirstOrDefault().IdRole = request.Role;
                _context.Attach(upUser);
                _context.Entry(upUser).State = EntityState.Modified;
                _context.Users.Update(upUser);
                await _context.SaveChangesAsync();
                if (request.IdWriter != null)
                {
                    if (request.IdWriter.Count >= 1 && request.Role == 2)
                    {
                        //foreach (var item in request.IdWriter)
                        //{
                        //    var acc = _context.Users.FirstOrDefault(x => x.Id == item);
                        //    if (acc != null)
                        //    {
                        //        acc.IdManager = upUser.Id;
                        //        _context.Users.Update(acc);
                        //    }
                        //    await _context.SaveChangesAsync(cancellationToken);
                        //}
                        _context.Users.Where(x => request.IdWriter.Contains(x.Id)).ToList().ForEach(x => x.IdManager = upUser.Id);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
                if (request.IdEditor != null)
                {
                    if (request.IdEditor.Count >= 1 && request.Role == 1)
                    {
                        //foreach (var item in request.IdEditor)
                        //{
                        //    var acc = _context.Users.FirstOrDefault(x => x.Id == item);
                        //    if (acc != null)
                        //    {
                        //        acc.IdManager = upUser.Id;
                        //        _context.Users.Update(acc);
                        //    }
                        //    await _context.SaveChangesAsync(cancellationToken);
                        //}
                        _context.Users.Where(x => request.IdEditor.Contains(x.Id)).ToList().ForEach(x => x.IdManager = upUser.Id);
                        await _context.SaveChangesAsync(cancellationToken);

                    }
                }
                transaction.Commit();
                var returnUser = new UserAdminModels()
                {
                    Id = upUser.Id,
                    Role = new RoleModel { Id = upUser.Accounts.FirstOrDefault().IdRole, Name = _context.Roles.Find(upUser.Accounts.FirstOrDefault().IdRole).Role },
                    Age = upUser.Age,
                    Email = upUser.Accounts.FirstOrDefault().Email,
                    CompanyName = upUser.Company,
                    FullName = upUser.FirstName + " " + upUser.LastName,
                    Gender = upUser.Gender,
                    IsActive = upUser.IsActive,
                    Phone = string.IsNullOrEmpty(upUser.Phone) == true ? null : upUser.Phone.Trim(),
                };
                return returnUser;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return null;
            }
        }
    }
}
