using BatchjobService.Entities;
using BatchjobService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.UpdateFanpage
{
    public class UpdateFanpageHandler : IRequestHandler<UpdateFanpageCommand, FanpageViewModel>
    {
        private readonly ContentoDbContext _context;

        public UpdateFanpageHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<FanpageViewModel> Handle(UpdateFanpageCommand request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                Fanpages fanpage = _context.Fanpages.Include(x => x.FanpagesTags).FirstOrDefault(x => x.Id == request.FanpageId);

                fanpage.IdChannel = request.ChannelId;
                if (request.CustomerId > 0)
                {
                    fanpage.IdCustomer = request.CustomerId;
                }
                fanpage.IsActive = true;
                fanpage.Token = request.Token;
                fanpage.ModifiedDate = DateTime.UtcNow;
                fanpage.Name = request.Name;
                fanpage.Link = request.Link;
                var lstTag = new List<FanpagesTags>();
                var returnTags = new List<TagModel>();
                foreach (var item in request.Tags)
                {
                    var tag = new FanpagesTags
                    {
                        IdTag = item,
                        IdFanpage = request.FanpageId
                    };
                    var returnTag = new TagModel
                    {

                        Id = item,
                        Name = _context.Tags.Find(item).Name
                    };
                    returnTags.Add(returnTag);
                    lstTag.Add(tag);
                }
                _context.FanpagesTags.RemoveRange(fanpage.FanpagesTags);
                _context.FanpagesTags.AddRange(lstTag);
                _context.Update(fanpage);

                await _context.SaveChangesAsync();
                transaction.Commit();
                _context.Entry(fanpage).Reference(p => p.IdChannelNavigation).Load();

                FanpageViewModel model = new FanpageViewModel();

                model.Id = fanpage.Id;
                model.Name = fanpage.Name;
                model.Channel = new Channel { Id = fanpage.IdChannelNavigation.Id, Name = fanpage.IdChannelNavigation.Name };
                if (fanpage.IdCustomer != null)
                {
                    var customer = _context.Users.Find(fanpage.IdCustomer);
                    model.Customer = new Customer { Id = customer.Id, Name = customer.FirstName + " " + customer.LastName };
                }
                else
                {
                    model.Customer = new Customer { Id = 0, Name = "" };
                }

                model.ModifiedDate = fanpage.ModifiedDate;
                model.Tags = returnTags;
                model.TagId = request.Tags;
                model.Link = request.Link;
                return model;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return null;
            }
          
        }
    }
}
