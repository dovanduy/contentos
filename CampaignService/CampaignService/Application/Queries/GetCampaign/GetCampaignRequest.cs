using CampaignService.Entities;
using CampaignService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetCampaign
{
    public class GetCampaignRequest : IRequest<CampaignTaskDetail>
    {
        public int IdCampaign { get; set; }
    }
}
