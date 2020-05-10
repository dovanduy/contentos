using CampaignService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetCampaignStatics
{
    public class GetCampaignStaticsRequest : IRequest<CampaignStaticsTotalModels>
    {

        public int? IdMarketer { get; set; }
        public int? IdCustomer { get; set; }
    }
}
