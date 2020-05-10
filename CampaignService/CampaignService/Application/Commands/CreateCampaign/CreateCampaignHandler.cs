using AutoMapper;
using CampaignService.Entities;
using CampaignService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CampaignService.Application.Commands.CreateCampaign
{
    public class CreateCampaignHandler : IRequestHandler<CreateCampaignCommand, CampaignData>
    {
        private readonly ContentoDbContext _context;

        public CreateCampaignHandler(ContentoDbContext campaignDbContext)
        {
            _context = campaignDbContext;
        }

        public async Task<CampaignData> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
        {
            // Start a local transaction.
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var Tags = new List<TagsCampaigns>();

                foreach (var item in request.Tags)
                {
                    var tag = new TagsCampaigns { IdTag = item};
                    Tags.Add(tag);
                }

                var newCampaign = new Campaigns
                {

                    EndDate = request.EndDate,
                    IdCustomer = request.Customer.Id,
                    IdEditor = request.Editor.Id,
                    Description = request.Description,
                    Title = request.Title,
                    CreatedDate = DateTime.UtcNow,
                    Status = 1,
                    IdMarketer = request.IdMarketer,
                    TagsCampaigns = Tags
                };

                _context.Campaigns.Add(newCampaign);

                await _context.SaveChangesAsync(cancellationToken);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<Campaigns, CampaignData>()
                .ForMember(x => x.Status, opt => opt.Ignore()));
                var mapper = config.CreateMapper();

                CampaignData model = mapper.Map<CampaignData>(newCampaign);

                ////Get Editor Name & Id
                //model.Editor = new Models.Editor();
                //model.Editor.Id = newCampaign.IdEditor;
                //model.Editor.Name = _context.Users.Find(newCampaign.IdEditor).Name;

                //Get Customer Name & Id
                model.Customer = new Models.Customer();
                model.Customer.Id = newCampaign.IdCustomer;
                var cus = _context.Users.Find(newCampaign.IdCustomer);
                model.Customer.Name = cus.FirstName + " " + cus.LastName;

                //Get Status Name & Id
                model.Status = new Models.Status();
                model.Status.Id = newCampaign.Status;
                var stat = _context.StatusCampaigns.Find(newCampaign.Status);
                model.Status.Name = stat.Name;
                model.Status.Color = stat.Color;
                model.EmailEditor = _context.Accounts.FirstOrDefault(x => x.IdUserNavigation.Id == request.Editor.Id).Email;

                //Get ListTag
                List<Models.Tag> ls = new List<Models.Tag>();
                foreach (var tag in newCampaign.TagsCampaigns)
                {
                    var cTag = new Models.Tag { Id = tag.IdTag, Name = _context.Tags.Find(tag.IdTag).Name };
                    ls.Add(cTag);
                }

                model.listTag = ls;
                transaction.Commit();
                return model;

            }catch(Exception e)
            {
                transaction.Rollback();
                return null;
            }
        }
    }
}
