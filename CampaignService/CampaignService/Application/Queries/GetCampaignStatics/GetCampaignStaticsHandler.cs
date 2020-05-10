using CampaignService.Entities;
using CampaignService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetCampaignStatics
{
    public class GetCampaignStaticsHandler : IRequestHandler<GetCampaignStaticsRequest, CampaignStaticsTotalModels>
    {
        private readonly ContentoDbContext _context;
        public GetCampaignStaticsHandler(ContentoDbContext contentodbContext)
        {
            _context = contentodbContext;
        }
        public async Task<CampaignStaticsTotalModels> Handle(GetCampaignStaticsRequest request, CancellationToken cancellationToken)
        {
            var retCampaign = new CampaignStaticsTotalModels();
            if (request.IdCustomer != null && request.IdCustomer != 0)
            {
                var lstCampaigncustomer = await _context.Campaigns.AsNoTracking().Where(x => x.IdCustomer == request.IdCustomer).ToListAsync();
                retCampaign.TotalCampaign = lstCampaigncustomer.Count;
                retCampaign.CampaignCompleted = lstCampaigncustomer.Count(x => x.Status == 3);
                retCampaign.CampaignInProcess = lstCampaigncustomer.Count(x => x.Status == 2);
            }
            else
            {
                var lstCampaign = await _context.Campaigns.AsNoTracking().Where(x => x.IdMarketer == request.IdMarketer).ToListAsync();
                retCampaign.TotalCampaign = lstCampaign.Count;
                retCampaign.CampaignCompleted = lstCampaign.Count(x => x.Status == 3);
                retCampaign.CampaignInProcess = lstCampaign.Count(x => x.Status == 2);
            }
           
            return retCampaign;
        }
    }
}
