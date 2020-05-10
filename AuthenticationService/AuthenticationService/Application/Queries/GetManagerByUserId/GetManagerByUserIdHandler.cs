using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetManagerByUserId
{
    public class GetManagerByUserIdHandler : IRequestHandler<GetManagerByUserIdRequest, ManagerModel>
    {
        private readonly ContentoDbContext _context;

        public GetManagerByUserIdHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<ManagerModel> Handle(GetManagerByUserIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.AsNoTracking().Include(i => i.IdManagerNavigation).FirstOrDefaultAsync(f => f.Id == request.id);
            var result = new ManagerModel();
            result.Id = entity.IdManager != null ? entity.IdManager : 0;
            if (entity.IdManager != null)
            {
                var entityRole = _context.Accounts.FirstOrDefault(f => f.IdUser == entity.IdManager).IdRole;
                var lstRole = _context.Roles.Select(x => new Role{ Id = x.Id, Name = x.Role }).ToList();

                var role = lstRole.Find(x => x.Id == entityRole);
                result.Role = role;
            }
            return result;
        }
    }
}
