using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetListTaksByCampaignId
{
    public class GetListTasksByCampaignIdRequest : IRequest<List<TasksViewModel>>
    {
        public int IdCampaign { get; set; }
    }
}
