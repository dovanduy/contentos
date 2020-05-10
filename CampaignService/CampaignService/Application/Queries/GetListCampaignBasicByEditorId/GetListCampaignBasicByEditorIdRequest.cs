using CampaignService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetListCampaignBasicByEditorId
{
    public class GetListCampaignBasicByEditorIdRequest : IRequest<List<CampaignModels>>
    {
        [Required]
        public int IdEditor { get; set; }
    }
}