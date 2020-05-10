using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetTaskByCampaignIdInteraction
{
    public class GetTaskByCampaignIdInteractionRequest : IRequest<List<ListTaskStaticModel>>
    {
        public int IdCampaign { get; set; }
    }
}
