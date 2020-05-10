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

namespace CampaignService.Application.Queries.GetCampaign
{
    public class GetCampaignHandler : IRequestHandler<GetCampaignRequest, CampaignTaskDetail>
    {
        private readonly ContentoDbContext _context;
        public GetCampaignHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }

        public async Task<CampaignTaskDetail> Handle(GetCampaignRequest request, CancellationToken cancellationToken)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Campaigns, CampaignTaskDetail>()
            .ForMember(x => x.Status, opt => opt.Ignore()));
            var mapper = config.CreateMapper();

            var entity = await _context.Campaigns.AsNoTracking()
                .Include(i => i.TagsCampaigns).ThenInclude(TagsCampaigns => TagsCampaigns.IdTagNavigation)
                .Include(x => x.IdEditorNavigation)
                .Include(x => x.IdCustomerNavigation)
                .Include(x => x.StatusNavigation)
                .FirstOrDefaultAsync(x => x.Id == request.IdCampaign);

            CampaignTaskDetail model = mapper.Map<CampaignTaskDetail>(entity);

            //Get Editor Name & Id
            model.Editor = new Models.Editor();
            model.Editor.Id = entity.IdEditor;
            model.Editor.Name = entity.IdEditorNavigation.FirstName + " " + entity.IdEditorNavigation.LastName;

            //Get Customer Name & Id
            model.Customer = new Models.Customer();
            model.Customer.Id = entity.IdCustomer;
            model.Customer.Name = entity.IdCustomerNavigation.FirstName + " " + entity.IdCustomerNavigation.LastName;

            //Get Status Name & Id
            model.Status = new Models.Status();
            model.Status.Id = entity.Status;
            model.Status.Name = entity.StatusNavigation.Name;
            model.Status.Color = entity.StatusNavigation.Color;

            //Get ListTag
            List<Tag> lsfull = new List<Tag>();
            List<int> ls = new List<int>();
            foreach (var tag in entity.TagsCampaigns)
            {
                var cTag = new Tag { Id = tag.IdTag, Name = tag.IdTagNavigation.Name };
                ls.Add(tag.IdTag);
                lsfull.Add(cTag);
            }
            model.listTag = ls;
            model.TagFull = lsfull;
            model.Description = entity.Description;
            return model;
        }


    }
}
