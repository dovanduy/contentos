using AuthenticationService.Entities;
using AuthenticationService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Queries.GetNotify
{
    public class GetNotifysHandler : IRequestHandler<GetNotifysRequest, List<GetNotifyModel>>
    {
        private readonly ContentoDbContext _context;


        public GetNotifysHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetNotifyModel>> Handle(GetNotifysRequest request, CancellationToken cancellationToken)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.IdUser == request.UserId);

            if (token != null)
            {
                var list = await _context.Notifys.AsNoTracking()
                .Where(x => x.IdToken == token.Id)
                .Select(x => new GetNotifyModel
                {
                    Id = x.Id,
                    Messager = x.Messager,
                    Title = x.Title,
                    //TypeSend = x.TypeSender,
                    //IdObject = x.IdObject
                }).ToListAsync();
                return list;
            }
            return null;
        }


    }
}
