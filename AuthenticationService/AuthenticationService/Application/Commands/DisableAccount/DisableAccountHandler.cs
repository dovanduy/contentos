using AuthenticationService.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Commands.DisableAccount
{
    public class DisableAccountHandler : IRequestHandler<DisableAccountCommand, string>
    {
        private readonly ContentoDbContext _context;

        public DisableAccountHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(DisableAccountCommand request, CancellationToken cancellationToken)
        {
            try 
            { 
                var acccount = _context.Users.Include(i => i.Accounts).FirstOrDefault(f => f.Id == request.proAccount).Accounts.FirstOrDefault();
                var role = acccount.IdRole;

                if (role == 1)
                {
                    //campaigns assign
                    var campaigns = _context.Campaigns.Where(w => w.IdMarketer == request.proAccount && w.Status < 3).ToList();

                    foreach (var campaign in campaigns)
                    {
                        campaign.IdMarketer = request.reicAccount;
                    }

                    //account assign
                    var accounts = _context.Users.Where(w => w.IdManager == request.proAccount);

                    foreach (var account in accounts)
                    {
                        account.IdManager = request.reicAccount;
                    }

                    _context.UpdateRange(campaigns);
                    _context.UpdateRange(accounts);
                }

                if (role == 2)
                {
                    //campaigns assign
                    var campaigns = _context.Campaigns.Where(w => w.IdEditor == request.proAccount && w.Status < 3).ToList();

                    foreach (var campaign in campaigns)
                    {
                        campaign.IdEditor = request.reicAccount;
                    }

                    //account assign
                    var accounts = _context.Users.Where(w => w.IdManager == request.proAccount);

                    foreach (var account in accounts)
                    {
                        account.IdManager = request.reicAccount;
                    }

                    _context.UpdateRange(campaigns);
                    _context.UpdateRange(accounts);
                }

                if (role == 3)
                {
                    //campaigns assign
                    var tasks = _context.Tasks.Where(w => w.IdWritter == request.proAccount && w.Status < 4).ToList();

                    foreach (var task in tasks)
                    {
                        task.IdWritter = request.reicAccount;
                    }

                    _context.UpdateRange(tasks);
                }

                var user = _context.Users.Find(request.proAccount);
                user.IsActive = false;
                acccount.IsActive = false;
                user.IdManager = null; 
                
                _context.Update(user);
                _context.Update(acccount);

                await _context.SaveChangesAsync();
                return "Success";
            }
            catch (Exception)
            {
                return "Error";
            }
            
        }
    }
}
