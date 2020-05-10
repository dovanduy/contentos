using CampaignService.Entities;
using CampaignService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetListCampaignByCustomerId
{
    public class GetListCampaignByCustomerIdHandler : IRequestHandler<GetListCampaignByCustomerIdRequest, List<CampaignStaticsModels>>
    {
        private readonly ContentoDbContext _context;
        public GetListCampaignByCustomerIdHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }

        public async Task<List<CampaignStaticsModels>> Handle(GetListCampaignByCustomerIdRequest request, CancellationToken cancellationToken)
        {
            var lstCampaign = await _context.Campaigns.Where(x => x.IdCustomer == request.Id)
                .Include(x=>x.StatusNavigation)
                .Select(x => new CampaignStaticsModels { Id = x.Id, Name = x.Title, StartDate = x.StartDate, EndDate = x.EndDate ,Status = new Status { Id = x.StatusNavigation.Id,Name = x.StatusNavigation.Name,Color = x.StatusNavigation.Color } }).ToListAsync();
            return lstCampaign;
        }
    }
}
