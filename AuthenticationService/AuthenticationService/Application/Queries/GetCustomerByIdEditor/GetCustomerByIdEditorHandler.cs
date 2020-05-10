using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetCustomerByIdEditor
{
    public class GetCustomerByIdEditorHandler : IRequestHandler<GetCustomerByIdEditorRequest, List<ListUserModel>>
    {
        private readonly ContentoDbContext _context;

        public GetCustomerByIdEditorHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListUserModel>> Handle(GetCustomerByIdEditorRequest request, CancellationToken cancellationToken)
        {
            var idMarketer = _context.Users.Find(request.EditorId).IdManager;
            var list = await _context.Users.AsNoTracking()
                .Include(x => x.Accounts)
                .Where(x => x.Accounts.Any(i => i.IdRole == 5) && x.IsActive == true)
                .Where(u => u.IdManager == idMarketer)
                .Select(x => new ListUserModel
                {
                    Id = x.Id,
                    Name = x.FirstName + " " + x.LastName
                })
                .ToListAsync();
            return list;
        }
    }
}
