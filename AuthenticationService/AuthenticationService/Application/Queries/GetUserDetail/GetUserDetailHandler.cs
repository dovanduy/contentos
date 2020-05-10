using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetUserDetail
{
    public class GetUserDetailHandler : IRequestHandler<GetUserDetailRequest, UserDetailModels>
    {
        private readonly ContentoDbContext _context;

        public GetUserDetailHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<UserDetailModels> Handle(GetUserDetailRequest request, CancellationToken cancellationToken)
        {
            var upUser = _context.Accounts
                .Include(x => x.IdUserNavigation).ThenInclude(IdUserNavigation => IdUserNavigation.IdManagerNavigation)
                .Include(x => x.IdRoleNavigation)
                .FirstOrDefault(x => x.IdUserNavigation.Id == request.Id);

            var returnUser = new UserDetailModels
            {
                Id = upUser.IdUserNavigation.Id,
                FirstName = upUser.IdUserNavigation.FirstName,
                LastName = upUser.IdUserNavigation.LastName,
                Age = upUser.IdUserNavigation.Age,
                CompanyName = upUser.IdUserNavigation.Company,
                Email = upUser.Email,
                Gender = upUser.IdUserNavigation.Gender,
                Phone = string.IsNullOrEmpty(upUser.IdUserNavigation.Phone) == true ? null : upUser.IdUserNavigation.Phone.Trim(),
                Role = new RoleModel { Id = upUser.IdRoleNavigation.Id, Name = upUser.IdRoleNavigation.Role },
                IsActive = upUser.IsActive ?? false
            };
            if (upUser.IdRoleNavigation.Id == 1)
            {
                var list = await _context.Users.AsNoTracking()
               .Include(x => x.Accounts)
               .Where(x => x.Accounts.Any(i => i.IdRole == 2) && x.IdManager == null && x.IsActive == true)
               .Select(x => new ListUserModel
               {
                   Id = x.Id,
                   Name = x.FirstName + " " + x.LastName
               })
               .ToListAsync();
                var choiceEditor = await _context.Users.AsNoTracking()
                .Include(x => x.Accounts)
                .Where(x => x.Accounts.Any(i => i.IdRole == 2) && x.IsActive == true && x.IdManager == request.Id)
                .Select(x => new ListUserModel
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName
                })
                .ToListAsync();
                list.AddRange(choiceEditor);
                returnUser.Editor = list;
                returnUser.IdEditor = list.Count == 0 ? null : list.Select(x=>x.Id).ToList();
                returnUser.ChoiceEditor = choiceEditor.Count == 0 ? null : choiceEditor.Select(x => x.Id).ToList();
            }
            if (upUser.IdRoleNavigation.Id == 2)
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
                var choiceMarketer = await _context.Users.AsNoTracking()
                .Include(x => x.Accounts)
                .Where(x => x.Accounts.Any(i => i.IdRole == 1) && x.IsActive == true && x.Id == upUser.IdUserNavigation.IdManager)
                .Select(x => new ListUserModel
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName
                })
                .ToListAsync();
                returnUser.Marketer = list;
                returnUser.IdMarketer = list.Count == 0 ? null : list.Select(x => x.Id).ToList();
                returnUser.ChoiceMarketer = choiceMarketer.Count == 0 ? null : choiceMarketer.Select(x => x.Id).ToList();
                //Writer
                var list2 = await _context.Users.AsNoTracking()
              .Include(x => x.Accounts)
              .Where(x => x.Accounts.Any(i => i.IdRole == 3) && x.IdManager == null && x.IsActive == true)
              .Select(x => new ListUserModel
              {
                  Id = x.Id,
                  Name = x.FirstName + " " + x.LastName
              })
              .ToListAsync();
                var choiceWriter = await _context.Users.AsNoTracking()
                .Include(x => x.Accounts)
                .Where(x => x.Accounts.Any(i => i.IdRole == 3) && x.IsActive == true && x.IdManager == request.Id)
                .Select(x => new ListUserModel
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName
                })
                .ToListAsync();
                list2.AddRange(choiceWriter);
                returnUser.Writer = list2;
                returnUser.IdWriter = list2.Count == 0 ? null : list2.Select(x => x.Id).ToList();
                returnUser.ChoiceWriter = choiceWriter.Count == 0 ? null : choiceWriter.Select(x => x.Id).ToList();
            }
            if (upUser.IdRoleNavigation.Id == 3)
            {
                var list = await _context.Users.AsNoTracking()
               .Include(x => x.Accounts)
               .Where(x => x.Accounts.Any(i => i.IdRole == 2)  && x.IsActive == true)
               .Select(x => new ListUserModel
               {
                   Id = x.Id,
                   Name = x.FirstName + " " + x.LastName
               })
               .ToListAsync();
                var choiceEditor = await _context.Users.AsNoTracking()
                .Include(x => x.Accounts)
                .Where(x => x.Accounts.Any(i => i.IdRole == 2) && x.IsActive == true && x.Id == upUser.IdUserNavigation.IdManager)
                .Select(x => new ListUserModel
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName
                })
                .ToListAsync();
                list.AddRange(choiceEditor);
                returnUser.Editor = list;
                returnUser.IdEditor = list.Count == 0 ? null : list.Select(x => x.Id).ToList();
                returnUser.ChoiceEditor = choiceEditor.Count == 0 ? null : choiceEditor.Select(x => x.Id).ToList();
            }
            return returnUser;
        }
    }
}
