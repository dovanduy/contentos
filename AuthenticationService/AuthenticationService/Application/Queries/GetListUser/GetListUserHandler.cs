using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetListUser
{
    public class GetListUserHandler : IRequestHandler<GetListUserRequest, List<UserAdminModels>>
    {
        private readonly ContentoDbContext _context;

        public GetListUserHandler(ContentoDbContext context)
        {

            _context = context;
        }
        public async Task<List<UserAdminModels>> Handle(GetListUserRequest request, CancellationToken cancellationToken)
        {
            var lstUser = await _context.Accounts.AsNoTracking()
               .Include(x => x.IdUserNavigation)
               .Include(x=>x.IdRoleNavigation)
               .Where(x=>x.IdRole != 4 && x.IdRole != 5 && x.IdRole != 6)
               .Select(x => new UserAdminModels
               {
                   Id = x.IdUserNavigation.Id,
                   FullName = x.IdUserNavigation.FirstName + " " + x.IdUserNavigation.LastName,
                   Age = x.IdUserNavigation.Age,
                   CompanyName  = x.IdUserNavigation.Company,
                   Email = x.Email,
                   Gender = x.IdUserNavigation.Gender,
                   Phone = string.IsNullOrEmpty(x.IdUserNavigation.Phone) == true ? null : x.IdUserNavigation.Phone.Trim(),
                   Role = new RoleModel { Id  = x.IdRoleNavigation.Id,Name = x.IdRoleNavigation.Role},
                   IsActive = x.IsActive ?? false
               })
               .ToListAsync();
            return lstUser;
        }
    }
}
