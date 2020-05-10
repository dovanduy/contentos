using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTagsByCampaignId
{
    public class GetTagsByCampaignIdRequest : IRequest<List<TagViewModel>>
    {
        public int CampaignId { get; set; }
    }
}
