using CampaignService.Entities;
using CampaignService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetListCampaignByUserId
{
    public class GetListCampaignByUserIdRequest : IRequest<List<CampaignData>>
    {

        public int IdCustomer { get; set; }

    }
}
