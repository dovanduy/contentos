using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetAllWriterByIdMarketer
{
    public class GetAllWriterByIdMarketerHandler : IRequestHandler<GetAllWriterByIdMarketerRequest, List<ListUserModel>>
    {
        private readonly ContentoDbContext _context;

        public GetAllWriterByIdMarketerHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListUserModel>> Handle(GetAllWriterByIdMarketerRequest request, CancellationToken cancellationToken)
        {
            var list = await _context.Users.AsNoTracking()
                .Where(x => x.Accounts.Any(i => i.IdRole == 2) && x.IsActive == true)
                .Where(u => u.IdManager == request.MarketerId).Select(x => x.Id)
                .ToListAsync();
            var lstWriter = new List<ListUserModel>();
            foreach (var item1 in list)
            {
                var listid = await _context.Users.AsNoTracking()
               .Include(x => x.Accounts)
               .Where(x => x.Accounts.Any(i => i.IdRole == 3) && x.IsActive == true)
               .Where(u => u.IdManager == item1).Select(x => new ListUserModel
               {
                   Id = x.Id,
                   Name = x.FirstName + " " + x.LastName
               }).ToListAsync();
                lstWriter.AddRange(listid);
            }
            return lstWriter;
        }
    }
}
