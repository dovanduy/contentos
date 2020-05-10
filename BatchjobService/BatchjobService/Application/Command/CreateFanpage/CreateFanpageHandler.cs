using BatchjobService.Entities;
using BatchjobService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BatchjobService.Application.Command.CreateFanpage
{
    public class CreateFanpageHandler : IRequestHandler<CreateFanpageCommand, FanpageViewModel>
    {
        private readonly ContentoDbContext _context;

        public CreateFanpageHandler(ContentoDbContext context)
        {
            _context = context;
        }
        public async Task<FanpageViewModel> Handle(CreateFanpageCommand request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                Fanpages fanpage = new Fanpages();

                fanpage.IdChannel = request.ChannelId;
                if (request.CustomerId > 0)
                {
                    fanpage.IdCustomer = request.CustomerId;
                }
                var lstTag = new List<FanpagesTags>();
                var returnTags = new List<TagModel>();
                foreach (var item in request.Tags)
                {
                    var tag = new FanpagesTags
                    {
                        IdTag = item
                    };
                    var returnTag = new TagModel
                    {

                        Id = item,
                        Name = _context.Tags.Find(item).Name
                    };
                    returnTags.Add(returnTag);
                    lstTag.Add(tag);
                }
                fanpage.IdMarketer = request.MarketerId;
                fanpage.IsActive = true;
                fanpage.Token = request.Token;
                fanpage.ModifiedDate = DateTime.UtcNow;
                fanpage.Name = request.Name;
                fanpage.FanpagesTags = lstTag;
                fanpage.Link = request.Link;

                _context.Add(fanpage);

                await _context.SaveChangesAsync();
                transaction.Commit();

                _context.Entry(fanpage).GetDatabaseValues();
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
                model.Tags = returnTags;
                model.TagId = request.Tags;
                model.ModifiedDate = fanpage.ModifiedDate;
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
