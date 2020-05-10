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

namespace AuthenticationService.Application.Commands.SaveToken
{
    public class SaveTokenHandler : IRequestHandler<SaveTokenCommands, UserToken>
    {
        private readonly ContentoDbContext _context;

        public SaveTokenHandler(ContentoDbContext context)
        {
            _context = context;
        }

        public async Task<UserToken> Handle(SaveTokenCommands request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            Tokens token = await IsExitToken(request.UserId);
            try
            {
                if (token == null)
                {
                    
                    var newToken = new Tokens
                    {
                        IdUser = request.UserId,
                        DeviceType = request.DeviceType,
                        Token = request.Token,
                        ModifiedDate = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow

                    };
                   
                    _context.Tokens.Add(newToken);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new UserToken
                    {
                        Id = _context.Tokens.AsNoTracking().OrderByDescending(x => x.Id).First().Id,
                        UserId = request.UserId,
                        Token = request.Token,
                        DeviceType = request.DeviceType,
                    };
                }
                else
                {
                    token.Token = request.Token;
                    token.ModifiedDate = DateTime.UtcNow;
                    _context.Attach(token);
                    _context.Entry(token).State = EntityState.Modified;
                    _context.Tokens.Update(token);
                    await _context.SaveChangesAsync();
                    var returnResult = new UserToken
                    {
                        Id = token.Id,
                        Token = request.Token,
                        UserId = request.UserId,
                        DeviceType = token.DeviceType,
                    };
                    transaction.Commit();
                    return returnResult;

                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return null;
            }

        }

        public Task<Tokens> IsExitToken(int UserId)
        { 
            var token = _context.Tokens.SingleOrDefaultAsync(t => t.IdUser == UserId);
            return token;

        }
    }
}
