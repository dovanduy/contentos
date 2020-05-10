using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampaignService.Application.Queries.GetListCampaignByUserId
{
    public class GetListCampaignByUserIdValidator : AbstractValidator<GetListCampaignByUserIdRequest>
    {
        public GetListCampaignByUserIdValidator()
        {
            RuleFor(x => x.IdCustomer).NotEmpty().WithMessage("Idcustomer is Required");
        }
    }

}
