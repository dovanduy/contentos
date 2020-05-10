using CampaignService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetListCampaignByEditorId
{
    public class GetCampaignByEditorIdRequest : IRequest<List<CampaignData>>
    {
        [Required]
        public int IdEditor { get; set; }
    }
}
