using CampaignService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetListCampaignByWriterId
{
    public class GetListCampaignByWriterIdRequest : IRequest<List<CampaignModels>>
    {
        [Required]
        public int IdWriter { get; set; }
    }
}