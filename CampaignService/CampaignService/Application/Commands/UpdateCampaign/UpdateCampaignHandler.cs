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

namespace CampaignService.Application.Commands.UpdateCampaign
{
    public class UpdateCampaignHandler : IRequestHandler<UpdateCampaignCommand, CampaignData>
    {

        private readonly ContentoDbContext _context;

        public UpdateCampaignHandler(ContentoDbContext campaignDbContext)
        {
            _context = campaignDbContext;
        }

        public async Task<CampaignData> Handle(UpdateCampaignCommand request, CancellationToken cancellationToken)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var upCampaign = _context.Campaigns.Include(x => x.TagsCampaigns).Single(s => s.Id == request.Id);

                upCampaign.EndDate = request.EndDate;
                if (request.EndDate > DateTime.UtcNow && upCampaign.Status == 4)
                {
                    upCampaign.Status = 2;
                }
                upCampaign.IdCustomer = request.Customer.Id;

                upCampaign.IdEditor = request.Editor.Id;

                upCampaign.Description = request.Description;

                upCampaign.Title = request.Title;
                upCampaign.ModifiedDate = DateTime.UtcNow;

                var upTags = new List<TagsCampaigns>();

                foreach (var item in request.Tags)
                {
                    var tag = new TagsCampaigns { IdTag = item };
                    upTags.Add(tag);
                }

                _context.TagsCampaigns.RemoveRange(upCampaign.TagsCampaigns);

                upCampaign.TagsCampaigns = upTags;

                _context.Entry(upCampaign).State = EntityState.Modified;

                //_context.Campaign.Update(upCampaign);

                await _context.SaveChangesAsync(cancellationToken);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<Campaigns, CampaignData>()
                .ForMember(x => x.Status, opt => opt.Ignore()));
                var mapper = config.CreateMapper();

                CampaignData model = mapper.Map<CampaignData>(upCampaign);

                ////Get Editor Name & Id
                //model.Editor = new Models.Editor();
                //model.Editor.Id = upCampaign.IdEditor;
                //model.Editor.Name = _context.Users.Find(upCampaign.IdEditor).Name;

                //Get Customer Name & Id
                model.Customer = new Models.Customer();
                model.Customer.Id = upCampaign.IdCustomer;
                var cus = _context.Users.Find(upCampaign.IdCustomer);
                model.Customer.Name = cus.FirstName + " " + cus.LastName;

                //Get Status Name & Id
                model.Status = new Models.Status();
                model.Status.Id = upCampaign.Status;
                var stat = _context.StatusCampaigns.Find(upCampaign.Status);
                model.Status.Name = stat.Name;
                model.Status.Color = stat.Color;

                //Get ListTag
                List<Models.Tag> ls = new List<Models.Tag>();
                foreach (var tag in upCampaign.TagsCampaigns)
                {
                    var cTag = new Models.Tag { Id = tag.IdTag, Name = _context.Tags.Find(tag.IdTag).Name };
                    ls.Add(cTag);
                }

                model.listTag = ls;
                transaction.Commit();
                return model;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return null;
            }

        }
    }
}
