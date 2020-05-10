using CampaignService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetListCampaignByCustomerId
{
    public class GetListCampaignByCustomerIdRequest : IRequest<List<CampaignStaticsModels>>
    {
        public int Id { get; set; }
    }
}
