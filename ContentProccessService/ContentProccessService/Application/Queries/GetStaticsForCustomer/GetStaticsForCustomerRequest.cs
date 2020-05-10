using ContentProccessService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetStaticsForCustomer
{
    public class GetStaticsForCustomerRequest : IRequest<List<ListTaskStaticModel>>
    {
        public int IdCampaign { get; set; }
    }
}
