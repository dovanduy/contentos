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

namespace CampaignService.Application.Queries.GetListCampaignByEditorId
{
    public class GetCampaignByEditorIdHandler : IRequestHandler<GetCampaignByEditorIdRequest, List<CampaignData>>
    {
        private readonly ContentoDbContext _context;
        public GetCampaignByEditorIdHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }

        public async Task<List<CampaignData>> Handle(GetCampaignByEditorIdRequest request, CancellationToken cancellationToken)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Campaigns, CampaignData>().ForMember(x => x.Status, opt => opt.Ignore()));
            var mapper = config.CreateMapper();

            var entity = await _context.Campaigns.AsNoTracking()
                .Include(i=>i.IdCustomerNavigation)
                .Include(i => i.TagsCampaigns).ThenInclude(TagsCampaigns=> TagsCampaigns.IdTagNavigation)
                .Include(i=>i.StatusNavigation)
                .Where(w => w.IdEditor == request.IdEditor)
                .OrderByDescending(i=>i.CreatedDate)
                .ToListAsync();

            //Map from entity to model
            List<CampaignData> models = new List<CampaignData>();

            foreach (var item in entity)
            {
                CampaignData model = mapper.Map<CampaignData>(item);

                ////Get Editor Name & Id
                //model.Editor = new Models.Editor();
                //model.Editor.Id = item.IdEditor;
                //model.Editor.Name = _context.Users.Find(item.IdEditor).Name;

                //Get Customer Name & Id
                model.Customer = new Models.Customer();
                model.Customer.Id = item.IdCustomer;
                model.Customer.Name = item.IdCustomerNavigation.FirstName + " " + item.IdCustomerNavigation.LastName;

                //Get Status Name & Id
                model.Status = new Models.Status();
                model.Status.Id = item.Status;
                model.Status.Name = item.StatusNavigation.Name;
                model.Status.Color = item.StatusNavigation.Color;

                //Get ListTag
                List<Tag> ls = new List<Tag>();
                foreach (var tag in item.TagsCampaigns)
                {
                    var cTag = new Tag { Id = tag.IdTag, Name = tag.IdTagNavigation.Name };
                    ls.Add(cTag);
                }
                model.StartedDate = item.StartDate;
                model.listTag = ls;
                models.Add(model);
            }
            return models;
        }
    }
}
