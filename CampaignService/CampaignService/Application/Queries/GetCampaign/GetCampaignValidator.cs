using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetCampaign
{
    public class GetCampaignValidator : AbstractValidator<GetCampaignRequest>
    {
        public GetCampaignValidator()
        {
            RuleFor(x => x.IdCampaign).NotEmpty().WithMessage("CampaignId is Required");
        }
    }
}
