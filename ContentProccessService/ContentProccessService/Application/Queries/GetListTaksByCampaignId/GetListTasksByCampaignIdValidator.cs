using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentProccessService.Application.Queries.GetListTaksByCampaignId
{
    public class GetListTasksByCampaignIdValidator : AbstractValidator<GetListTasksByCampaignIdRequest>
    {
        public GetListTasksByCampaignIdValidator()
        {
            RuleFor(x => x.IdCampaign).NotEmpty().WithMessage("IdCampaign is requied");
        }
    }
}
